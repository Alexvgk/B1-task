using Desktop_task.Services.DataDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Repositories
{
    public class FileRepo : BaseRepository<Model.File>
    {
        public FileRepo(FinanceDbContext dbContext) : base(dbContext)
        {
        }
    }
}
