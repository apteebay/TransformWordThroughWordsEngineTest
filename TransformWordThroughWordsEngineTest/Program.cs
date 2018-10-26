using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TransformWordThroughWordsEngineTest
{
    class Program
    {
        private static IEnumerable<string> getWords()
        {
            var words = new List<string>();
            var filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\",""),"Data");
            filepath = Path.Combine(filepath, "words-english1.txt");

            using (var reader = new StreamReader(filepath))
            {
                string word;
                while ((word = reader.ReadLine()) != null) words.Add(word);
            }


            return words;
        }

        static void Main(string[] args)
        {
            var transformations = FindPath(getWords(), "AAAS", "ZOOM");
        }

        static HashSet<string> Dictionary = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
        //dictionary used to find the parent in every node in the graph and to avoid traversing an already traversed node
        static Dictionary<string, string> parents = new Dictionary<string, string>();

        public static List<string> FindPath(IEnumerable<string> input, string start, string end)
        {

            // This Engine seems to work
            // We may be able tp improve performance by using LINQ and a Levenshtein distance on the Dicionary to get the Parents
            // but we can look at that later

            char[] allcharacters =
            {
                '\\','&','0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };

            // A P Teebay in final use all Printable Char (ASCII 32 - 126)
            // A P Teebay Use Dictonary.UnionWith(input)
            Dictionary.UnionWith(input);
            List<string> currentFrontier = new List<string>();
            List<string> nextFrontier = new List<string>();
            currentFrontier.Add(start);
            while (currentFrontier.Count > 0)
            {
                foreach (string s in currentFrontier)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        foreach (char c in allcharacters)
                        {
                            StringBuilder newWordBuilder = new StringBuilder(s);
                            newWordBuilder[i] = c;
                            string newWord = newWordBuilder.ToString();
                            if (Dictionary.Contains(newWord.TrimEnd()))
                            {
                                //avoid traversing a previously traversed node
                                if (!parents.Keys.Contains(newWord))
                                {
                                    parents.Add(newWord.ToString(), s);
                                    nextFrontier.Add(newWord);
                                }

                            }
                            if (newWord.ToString() == end)
                            {
                                return ExtractPath(start, end);

                            }
                        }
                    }
                }
                currentFrontier = new List<string>(nextFrontier);
                //currentFrontier.Clear();
                //currentFrontier.Concat(nextFrontier);
                nextFrontier.Clear();
            }
            throw new ArgumentException("The given dictionary cannot be used to get a path from start to end");
        }

        private static List<string> ExtractPath(string start, string end)
        {
            List<string> path = new List<string>();
            string current = end;
            path.Add(end);
            while (current != start)
            {
                current = parents[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

    }
}
