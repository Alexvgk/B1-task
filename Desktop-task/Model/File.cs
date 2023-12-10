using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class File : BaseModel
    {
        public string? FileName { get; set; }
        public DateTime FileDate { get; set; }

        public ICollection<Class>? Classes { get; set; }
    }
}
