using Console_Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data.Procedures
{
    public interface IProceduresAccessObject<T> where T : BaseModel
    {
        public void calculateSumAndMedian(out long sum, out double median);
    }
}
