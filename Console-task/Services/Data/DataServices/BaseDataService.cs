using Console_Test.Model;
using Console_Test.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data.DataServices
{
    public abstract class BaseDataService<T> where T : BaseModel
    {


        protected string _connectionString;

        protected BaseDataService(){
            string mdfFilePath = @"DB\Database.mdf";

            // get root directory
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // absolute DbPath
            string absoluteMdfPath = System.IO.Path.Combine(currentDirectory, mdfFilePath);
            _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={absoluteMdfPath};Integrated Security=True;";
        }
    }
}
