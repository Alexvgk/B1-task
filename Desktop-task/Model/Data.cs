using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class Data : BaseModel
    {
        public decimal ActivIncomSaldo { get; set; }
        public decimal PassivIncomSaldo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal ActivOutcomSaldo { get; set; }
        public decimal PassOutcomSaldo { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
