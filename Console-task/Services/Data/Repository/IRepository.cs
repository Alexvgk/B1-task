using Console_Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data.Repository
{
    public interface IRepository <T> where T : BaseModel
    {
        public void insertData(List<T> data);
        public T getDataById(int id);
        public List<T> getData();
        public void deleteData(int id);
        public void updateData(int id, T model);

    }
}
