using Console_Test.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Parsing
{
    public class ParsingService
    {

        public static bool tryParseFileNumbers(string input, int maxNumber, out int[] numbers)
        {
            // Split the input string into individual parts using commas as separators
            string[] parts = input.Split(',');

            // Initialize the output array to store parsed numbers
            numbers = new int[parts.Length];

            // Iterate through each part of the input
            for (int i = 0; i < parts.Length; i++)
            {
                // Try to parse each part into an integer
                if (int.TryParse(parts[i], out int number) && number >= 1 && number <= maxNumber)
                {
                    numbers[i] = number;
                }
                else
                {
                    return false;
                }
            }

            // Return true if all parts are successfully parsed
            return true;
        }


        public static List<DataModel> ParseFile(string filePath)
        {
            List<DataModel> dataList = new List<DataModel>();

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Iterate through each line and parse it into a DataModel object
                foreach (string line in lines)
                {
                    DataModel data = ParseLine(line);
                    if (data != null)
                    {
                        dataList.Add(data);
                    }
                }

            }
            catch (Exception)
            {
                // Any exceptions that may occur during file parsing
                throw;
            }

            return dataList;
        }


        public static DataModel ParseLine(string line)
        {
            try
            {
                // Split the input line into parts using "||" as the delimiter
                string[] parts = line.Split(new[] { "||" }, StringSplitOptions.None);

                // Check if the line contains the expected number of parts
                if (parts.Length == 6)
                {
                    // Create a new DataModel object and populate its properties
                    return new DataModel
                    {
                        Date = DateTime.ParseExact(parts[0].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture).Date,
                        LatinSymbols = parts[1].Trim(),
                        RussianSymbols = parts[2].Trim(),
                        IntegerNumber = int.Parse(parts[3].Trim()),
                        FloatingPointNumber = double.Parse(parts[4].Trim(), CultureInfo.GetCultureInfo("ru-RU"))
                    };
                }
                else
                    // Throw an exception if the input line has an invalid format
                    throw new Exception("Invalid Format");
            }
            catch(Exception) 
            {
                throw;
            }
        }

        public static DataTable ParseToDataTable(List<DataModel> dataList)
        {
            DataTable table = new DataTable();

            // Add columns to the DataTable
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("LatinSymbols", typeof(string));
            table.Columns.Add("RussianSymbols", typeof(string));
            table.Columns.Add("IntegerNumber", typeof(int));
            table.Columns.Add("FloatingPointNumber", typeof(double));

            foreach (DataModel data in dataList)
            {
                // Add a row for each data item
                table.Rows.Add(data.Id, data.Date.Date, data.LatinSymbols, data.RussianSymbols, data.IntegerNumber, data.FloatingPointNumber);
            }

            return table;
        }
    }
}
