using Desktop_task.Model;
using Desktop_task.Services.DataDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Repositories
{
    public class AccountRepo : BaseRepository<Account>
    {
        public AccountRepo(FinanceDbContext dbContext) : base(dbContext)
        {
        }
    }
}
