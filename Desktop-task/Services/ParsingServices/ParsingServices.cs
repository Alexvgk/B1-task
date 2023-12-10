using Desktop_task.Model;
using Desktop_task.Repositories;
using Microsoft.IdentityModel.Tokens;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Services.ParsingServices
{
    public class ParsingServices
    {

        public static async Task TryParse(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    // Handling the case where the file path is not selected
                    throw new ArgumentException("File path is empty or null.");
                }

                // Extracting the file name from the path
                string fileName = Path.GetFileName(path);
                Model.File xlsFile = new Model.File()
                {
                    FileName = fileName,
                };
                FileRepo fileRepo = new FileRepo(new DataDb.FinanceDbContext());
                await fileRepo.AddAsync(xlsFile);

                // Opening the Excel file for reading
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(0); // Assuming you have only one sheet

                    // Getting the bank name from cell A1
                    Bank bank = new Bank
                    {
                        Name = sheet.GetRow(0).GetCell(0).ToString()
                    };
                    BankRepo bankRepo = new BankRepo(new DataDb.FinanceDbContext());
                    await bankRepo.AddAsync(bank);
                    int rowCount = sheet.LastRowNum + 1;

                    // Starting processing from cell A9
                    Class lastClass = new Class();
                    for (int row = 8; row < rowCount; row++)
                    {
                        try
                        {
                            string accountId = sheet.GetRow(row).GetCell(0).ToString();

                            if (int.TryParse(accountId, out int accountIdValue) && accountIdValue >= 1000 && accountIdValue <= 9999)
                            {
                                // Creating an Account object
                                Account account = new Account
                                {
                                    AccountNumber = accountId,
                                    IsValid = true,
                                    BankId = bank.Id,
                                };
                                AccountRepo accountRepo = new AccountRepo(new DataDb.FinanceDbContext());
                                await accountRepo.AddAsync(account);

                                // Creating a Data object
                                Model.Data data = new Model.Data
                                {
                                    AccountId = account.Id,
                                    ActivIncomSaldo = Convert.ToDecimal(sheet.GetRow(row).GetCell(1).NumericCellValue),
                                    PassivIncomSaldo = Convert.ToDecimal(sheet.GetRow(row).GetCell(2).NumericCellValue),
                                    Debit = Convert.ToDecimal(sheet.GetRow(row).GetCell(3).NumericCellValue),
                                    Credit = Convert.ToDecimal(sheet.GetRow(row).GetCell(4).NumericCellValue),
                                    ActivOutcomSaldo = Convert.ToDecimal(sheet.GetRow(row).GetCell(5).NumericCellValue),
                                    PassOutcomSaldo = Convert.ToDecimal(sheet.GetRow(row).GetCell(6).NumericCellValue),
                                };
                                DataRepo dataRepo = new DataRepo(new DataDb.FinanceDbContext());
                                await dataRepo.AddAsync(data);

                                // Creating a Finance object
                                Model.Finance finance = new Model.Finance
                                {
                                    DataId = data.Id,
                                    ClassId = lastClass.Id,
                                };
                                FinanceRepo financeRepo = new FinanceRepo(new DataDb.FinanceDbContext());
                                await financeRepo.AddAsync(finance);
                            }
                            // If accountId is in the range of 10 to 99, skip it
                            else if (int.TryParse(accountId, out int smallAccountId) && smallAccountId >= 10 && smallAccountId <= 99)
                            {
                                // Skip this iteration of the loop
                                continue;
                            }
                            // If the row contains "КЛАСС", create a new class
                            else if (accountId.Contains("КЛАСС"))
                            {
                                // Extracting the number after the word "КЛАСС"
                                string[] words = accountId.Split(' ');
                                if (words.Length > 1 && words[0].ToUpper() == "КЛАСС" && int.TryParse(words[2], out int classId))
                                {
                                    string description = accountId.Substring(accountId.IndexOf(words[2]) + words[2].Length).Trim();
                                    // Creating a Class object
                                    Class classObject = new Class
                                    {
                                        ClassId = classId,
                                        ClassName = description,
                                        FileId = xlsFile.Id
                                    };
                                    ClassRepo classRepo = new ClassRepo(new DataDb.FinanceDbContext());
                                    await classRepo.AddAsync(classObject);
                                    lastClass = classObject;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Handle exceptions for a specific row
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handling exceptions during Excel parsing
                throw; // Rethrow the exception to propagate it further
            }
        }

        // Method for transforming financial data into a DataTable, grouped by class and account
        public static async Task<DataTable> TryParseToDataTable(Model.File file)
        {
            try
            {
                // Retrieve financial data from the database
                FinanceRepo financeRepo = new FinanceRepo(new DataDb.FinanceDbContext());
                List<Model.Finance> finances = await financeRepo.GetAllIncludeAsync();

                // Filter financial data by the specified file
                finances = finances.Where(finance => finance.Class.FileId == file.Id).ToList();
                int classId = 0;

                // Return null if there is no financial data for the specified file
                if (finances.IsNullOrEmpty())
                {
                    return null;
                }

                // Create a DataTable to hold the transformed data
                DataTable dataTable = CreateTable();

                // Initialize variables for tracking class, account, and totals
                int accountId = int.Parse(finances.FirstOrDefault().Data.Account.AccountNumber);
                Data totalClass = new Data();
                Data totalData = new Data();
                Data General = new Data();

                // Loop through each financial record
                foreach (var finance in finances)
                {
                    // Check if a new class is encountered
                    if (classId == 0 || classId != finance.Class.ClassId)
                    {
                        // Add the total class row to the DataTable
                        if (classId != 0)
                        {
                            dataTable.Rows.Add("По классу", classId
                                , totalClass.ActivIncomSaldo
                                , totalClass.PassivIncomSaldo
                                , totalClass.Debit
                                , totalClass.Credit
                                , totalClass.ActivOutcomSaldo
                                , totalClass.PassOutcomSaldo);

                            // Add lines to the General total
                            AddLines(ref General, totalClass);
                            SetNull(ref totalClass);
                        }

                        // Add the class row to the DataTable
                        dataTable.Rows.Add(finance.Class.ClassName.ToString(), finance.Class.ClassId.ToString());
                        classId = finance.Class.ClassId;
                        accountId = int.Parse(finance.Data.Account.AccountNumber);
                    }

                    // Check if a new account is encountered within the same class
                    if ((int.Parse(finance.Data.Account.AccountNumber) / 100) == accountId / 100)
                    {
                        // Add individual data row to the DataTable
                        AddRow(dataTable, finance.Data);
                        AddLines(ref totalData, finance.Data);
                        accountId = int.Parse(finance.Data.Account.AccountNumber);
                    }
                    // Check if a new account is encountered within a different class
                    else if ((int.Parse(finance.Data.Account.AccountNumber) / 1000) == accountId / 1000)
                    {
                        // Add total data row to the DataTable
                        dataTable.Rows.Add("", accountId / 100
                            , totalData.ActivIncomSaldo
                            , totalData.PassivIncomSaldo
                            , totalData.Debit
                            , totalData.Credit
                            , totalData.ActivOutcomSaldo
                            , totalData.PassOutcomSaldo);

                        // Add lines to the total class
                        AddLines(ref totalClass, totalData);
                        SetNull(ref totalData);

                        // Add individual data row to the DataTable
                        AddRow(dataTable, finance.Data);
                        AddLines(ref totalData, finance.Data);
                        accountId = int.Parse(finance.Data.Account.AccountNumber);
                    }
                }

                // Add the overall total row to the DataTable
                dataTable.Rows.Add("Общее", null
                    , General.ActivIncomSaldo
                    , General.PassivIncomSaldo
                    , General.Debit
                    , General.Credit
                    , General.ActivOutcomSaldo
                    , General.PassOutcomSaldo);

                return dataTable; // Return the populated DataTable
            }
            // Handle the case where a NullReferenceException is thrown
            catch (NullReferenceException)
            {
                return null;
            }
            // Handle other exceptions
            catch (Exception ex)
            {
                // Handle exceptions and print an error message to the console
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You may want to return null or another value in case of an error
                return null;
            }
        }


        private static DataTable CreateTable()
        {
            DataTable dataTable = new DataTable("Data");
            dataTable.Columns.Add("Class", typeof(string));
            dataTable.Columns.Add("Account", typeof(int));
            dataTable.Columns.Add("ActivIncomSaldo", typeof(decimal));
            dataTable.Columns.Add("PassivIncomSaldo", typeof(decimal));
            dataTable.Columns.Add("Debit", typeof(decimal));
            dataTable.Columns.Add("Credit", typeof(decimal));
            dataTable.Columns.Add("ActivOutcomSaldo", typeof(decimal));
            dataTable.Columns.Add("PassivOutcomSaldo", typeof(decimal));
            return dataTable;

        }

        // Helper method to set all properties of a Data object to zero
        private static void SetNull(ref Data totalData)
        {
            totalData.Debit = 0;
            totalData.ActivOutcomSaldo = 0;
            totalData.ActivIncomSaldo = 0;
            totalData.Credit = 0;
            totalData.PassivIncomSaldo = 0;
            totalData.PassOutcomSaldo = 0;
        }


        private static void AddRow(DataTable dataTable, Data data)
        {
            dataTable.Rows.Add("", data.Account.AccountNumber
        , data.ActivIncomSaldo
        , data.PassivIncomSaldo
        , data.Debit
        , data.Credit
        , data.ActivOutcomSaldo
        , data.PassOutcomSaldo);
        }

        // Helper method to add the properties of one Data object to another
        private static void AddLines(ref Data totalData, Data finance)
        {
            totalData.Debit += finance.Debit;
            totalData.ActivOutcomSaldo += finance.ActivOutcomSaldo;
            totalData.ActivIncomSaldo += finance.ActivIncomSaldo;
            totalData.Debit += finance.Debit;
            totalData.Credit += finance.Credit;
            totalData.PassivIncomSaldo += finance.PassivIncomSaldo;
            totalData.PassOutcomSaldo += finance.ActivOutcomSaldo;
        }
        
    }
}
