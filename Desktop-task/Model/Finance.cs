using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class Finance : BaseModel
    {
        public int DataId { get; set; }
        public int ClassId { get; set; }

        public Class Class { get; set; }

        public Data Data { get; set; }
    }
}
