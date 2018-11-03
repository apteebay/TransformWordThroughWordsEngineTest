using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Utilities
{
    /// <summary>
    /// Reads the lines of a Text file into an IList of string></c>
    /// </summary>
    public static class ReadTxtToList
    {
        public static List<string> readFile(string dictionaryFilePath) => File.Exists(dictionaryFilePath) ? File.ReadAllLines(dictionaryFilePath).ToList() : new List<string>;
    }
}
