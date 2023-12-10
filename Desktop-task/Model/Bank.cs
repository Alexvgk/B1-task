using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class Bank : BaseModel
    {
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
