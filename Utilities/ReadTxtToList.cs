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
    public class ReadTxtToList
    {
        public ReadTxtToList(string dictionaryFilePath) => readFile(dictionaryFilePath);

        public IList<string> Lines { get; private set; }

        private void readFile(string dictionaryFilePath)
        {
            Lines = new List<string>();

            if (File.Exists(dictionaryFilePath))
            {
                using (var reader = new StreamReader(dictionaryFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null) Lines.Add(line);
                }

            }

        }
    }
}
