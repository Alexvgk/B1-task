using Desktop_task.Model;
using Desktop_task.Services.DataDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Repositories
{
    public class FinanceRepo : BaseRepository<Finance>
    {
        public FinanceRepo(FinanceDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<List<Model.Finance>> GetAllIncludeAsync()
        {
            return await _dbContext.Set<Model.Finance>()
                .Include(finance => finance.Class)  // Include the associated Class table
                .Include(finance => finance.Data)   // Include the associated Data table
                    .ThenInclude(data => data.Account)  // Then include the associated Account table for Data
                .ToListAsync();
        }
    }
}
