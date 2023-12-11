using Console_Test.Model;
using Console_Test.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data.DataServices
{
    public abstract class BaseDataService<T> where T : BaseModel
    {
        protected string _connectionString;

        protected BaseDataService()
        {
            // Path to the database folder
            string dbFolderPath = @"DB\";
            string mdfFileName = "Database.mdf";

            // Get the current application directory
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Absolute path to the database folder
            string absoluteDbFolderPath = System.IO.Path.Combine(currentDirectory, dbFolderPath);

            // Absolute path to the mdf file
            string absoluteMdfPath = System.IO.Path.Combine(absoluteDbFolderPath, mdfFileName);

            // Check if the folder exists, if not, create it
            if (!Directory.Exists(absoluteDbFolderPath))
            {
                Directory.CreateDirectory(absoluteDbFolderPath);
            }

            // Check if the mdf file exists, if not, create it
            if (!File.Exists(absoluteMdfPath))
            {
                CreateDatabase(absoluteMdfPath);
                _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={absoluteMdfPath};Integrated Security=True;";
                CreateTable();
                CreateProcedure();
            }
            _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={absoluteMdfPath};Integrated Security=True;";
        }

        private void CreateDatabase(string mdfPath)
        {
            // Connection string for the master database
            string masterConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;";

            // SQL query to create the database
            string createDatabaseQuery = $"CREATE DATABASE [Data] ON (NAME = N'DataDb', FILENAME = '{mdfPath}')";

            using (SqlConnection masterConnection = new SqlConnection(masterConnectionString))
            {
                masterConnection.Open();

                using (SqlCommand createDbCommand = new SqlCommand(createDatabaseQuery, masterConnection))
                {
                    createDbCommand.ExecuteNonQuery();
                }
            }
        }

        private void CreateTable()
        {
            try
            {
                // Creating a connection to the database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string createTableQuery = SqlQueries.CreateTable;
                    using (SqlCommand createTableCmd = new SqlCommand(createTableQuery, connection))
                    {
                        createTableCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database and table: {ex.Message}");
                // Exception handling
            }
        }

        private void CreateProcedure()
        {
            try
            {
                // Creating a connection to the database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Executing the query to create the stored procedure
                    using (SqlCommand createProcedureCommand = new SqlCommand(SqlQueries.Procedure, connection))
                    {
                        createProcedureCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database and table: {ex.Message}");
                // Exception handling
            }
        }
    }
}
