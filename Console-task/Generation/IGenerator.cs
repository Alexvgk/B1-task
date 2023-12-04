using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Generation
{
    public interface IGenerator
    {
        public bool GenerateFile();
        public string GenerateFileContent();
    }
}
