using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Model
{
    public class DataModel : BaseModel
    {
        public DateTime Date { get; set; }
        public string LatinSymbols { get; set; }
        public string RussianSymbols { get; set; }
        public long IntegerNumber { get; set; }
        public double FloatingPointNumber { get; set; }
    }
}
