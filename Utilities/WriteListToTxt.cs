using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utilities
{
    public class WriteListToTxt
    {
        public WriteListToTxt(string resultFilePath, IEnumerable<string> results) => writeFile(resultFilePath, results);

        private void writeFile(string resultFilePath, IEnumerable<string> results)
        {
            var path = Directory.GetParent(resultFilePath);
            checkDirectory(path);
            if (path.Exists)
            {
                if (File.Exists(resultFilePath)) File.Delete(resultFilePath);
                File.WriteAllLines(resultFilePath, results);
            }
        }

        private void checkDirectory(DirectoryInfo resultPath)
        {
            if (resultPath.Exists) return;

            var path = resultPath.Parent;
            checkDirectory(path);
            path.CreateSubdirectory(resultPath.Name);
        }


    }
}
