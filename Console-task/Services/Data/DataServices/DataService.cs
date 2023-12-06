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

namespace Console_Test.Services.Data.DataServices
{
    public class DataService : BaseDataService<DataModel>
    {
        IRepository<DataModel> repository;

        public DataService() : base()
        {
            this.repository = new DataRepo();
        }


        public void setData(List<DataModel> data)
        {
           this.repository.insertData(data);
        }

        public void calculateSumAndMedian(out long sum, out double median)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(SqlQueries.createProcedureScript, connection))
                {
                    command.ExecuteNonQuery();
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
    }
}
