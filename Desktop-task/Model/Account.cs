using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class Account : BaseModel
    {
        public string? AccountNumber { get; set; }
        public bool IsValid { get; set; }
        public Bank? Bank { get; set; }
        public int BankId { get; set; }
    }
}
