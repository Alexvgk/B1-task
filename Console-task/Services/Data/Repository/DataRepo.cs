using Console_Test.Model;
using Console_Test.Services.Data.DataServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data.Repository
{
    public class DataRepo : BaseDataService<DataModel>,IRepository<DataModel>
    {


        public DataRepo() : base() 
        {
        }

        public void deleteData(int id)
        {
            throw new NotImplementedException();
        }

        public List<DataModel> getData()
        {
            throw new NotImplementedException();
        }

        public DataModel getDataById(int id)
        {
            throw new NotImplementedException();
        }

        public void insertData(List<DataModel> data)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "DataTable";

                    bulkCopy.NotifyAfter = 500;
                    bulkCopy.SqlRowsCopied += (sender, e) => OnSqlRowsCopied(e, data.Count);
                    DataTable dataTable = ConvertToDataTable(data);
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }


        public void updateData(int id, DataModel model)
        {
            throw new NotImplementedException();
        }


        private DataTable ConvertToDataTable(List<DataModel> dataList)
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
                table.Rows.Add(data.Id,data.Date.Date, data.LatinSymbols, data.RussianSymbols, data.IntegerNumber, data.FloatingPointNumber);
            }

            return table;
        }

        private void OnSqlRowsCopied(SqlRowsCopiedEventArgs e, int totalRowCount)
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, Console.CursorTop); 
            Console.Write($"Copied {e.RowsCopied} rows. Remaining to copy: {totalRowCount - e.RowsCopied}.");

            Console.CursorVisible = true;
        }
    }
}
