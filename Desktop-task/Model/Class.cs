using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_task.Model
{
    public class Class : BaseModel
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int FileId { get; set; }
        public File? File { get; set; }
        public ICollection<Finance>? Finances { get; set; }
    }
}
