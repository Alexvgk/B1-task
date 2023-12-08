using Console_Test.Model;
using Console_Test.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Console_Test.Services.Data.Procedures;
using Console_Test.Services.Parsing;

namespace Console_Test.Services.Data.DataServices
{
    public class DataService : BaseDataService<DataModel>, IRepository<DataModel>, IProceduresAccessObject<DataModel>
    {

        public DataService() : base()
        {
        }

        // Deletes data based on the specified identifier
        public void deleteData(int id)
        {
            throw new NotImplementedException();
        }

        // Retrieves a list of all data
        public List<DataModel> getData()
        {
            throw new NotImplementedException();
        }

        // Retrieves data based on the specified identifier
        public DataModel getDataById(int id)
        {
            throw new NotImplementedException();
        }

        // Inserts data into the database using SqlBulkCopy
        public void insertData(List<DataModel> data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Set the destination table for the bulk copy operation
                        bulkCopy.DestinationTableName = "DataTable";

                        // Configure bulk copy settings
                        bulkCopy.NotifyAfter = 500;
                        bulkCopy.SqlRowsCopied += (sender, e) => OnSqlRowsCopied(e, data.Count);

                        // Convert the list of data models to a DataTable
                        DataTable dataTable = ParsingService.ParseToDataTable(data);

                        // Execute the bulk copy operation
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }
            catch (SqlException)
            {
                // Handle SQL-specific exceptions
                throw;
            }
            catch (Exception)
            {
                // Handle general exceptions
                throw;
            }
        }

        // Updates data based on the specified identifier
        public void updateData(int id, DataModel model)
        {
            throw new NotImplementedException();
        }

        // Calculates the sum and median of the data
        public void calculateSumAndMedian(out long sum, out double median)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(SqlQueries.createProcedureScript, connection))
                {
                    // Create and execute a stored procedure to calculate sum and median
                    command.ExecuteNonQuery();

                    // Retrieve the results of the stored procedure
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sum = Convert.ToInt64(reader["SumOfIntegers"]);
                            median = Convert.ToDouble(reader["MedianOfFloats"]);
                        }
                        else
                        {
                            sum = 0;
                            median = 0;
                        }
                    }
                }
            }
        }

        // Event handler for the SqlRowsCopied event, displaying information about the copied rows
        private void OnSqlRowsCopied(SqlRowsCopiedEventArgs e, int totalRowCount)
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"Copied {e.RowsCopied} rows. Remaining to copy: {totalRowCount - e.RowsCopied}.");

            Console.CursorVisible = true;
        }
    }
}
