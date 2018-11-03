using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utilities
{
    public static  class WriteListToTxt
    {

        public static void WriteFile(string resultFilePath, IEnumerable<string> results)
        {
            var path = Directory.GetParent(resultFilePath);
            Directory.CreateDirectory(resultFilePath);

            if (File.Exists(resultFilePath)) File.Delete(resultFilePath);

            File.WriteAllLines(resultFilePath, results);
        }

    }
}
