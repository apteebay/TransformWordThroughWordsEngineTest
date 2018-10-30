using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class UniqueChars
    {
        public UniqueChars(IEnumerable<string> lines)
        {
            var chars = new List<char>();
            for (int i=0; i < lines.Count(); i++)
            {
                var charsToAdd = lines.ElementAt(i).Distinct();
                chars.AddRange(charsToAdd);

                // To avoid creating a large list of repeating Chars run distinct every time the i Mod 100 is 0
                if ((i % 100) == 0 && charsToAdd.Any()) chars = chars.Distinct().ToList();
            }

            Chars = chars.Distinct();
        }

        public IEnumerable<char> Chars { get; private set; }
    }
}
