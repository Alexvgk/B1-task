using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Conbination
{
    public interface IConbinator
    {
        public int combineSpecificNumberOfFiles(string folderPath, string outputPath, int[] selectedFileNumbers, string patternToRemove = " ");
        public int combineAllFiles(string folderPath, string outputPath, string patternToRemove = " ");
    }
}
