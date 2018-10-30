using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransformWordInterface;
using Utilities;

namespace WordTransformFastenshtein
{
    public class WordTransformer : WordTransformTxtDict.WordTransformer
    {
        public WordTransformer(string dictionaryFilePath) : base(dictionaryFilePath) { Engine = new Engine(); }
        public WordTransformer(IEnumerable<string> dictionary) : base(dictionary) { Engine = new Engine(); }
    }

    public class Engine : WordTransformerEngine
    {
        private TransformMethod c_method;

        protected override TransformMethod Method
        {
            get
            {
                if (c_method == null) c_method = method;
                return c_method;
            }

            set => c_method = value;
        }

        private void method(IWordTransformer transformer)
        {
            Dictionary<string, string> parents = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            var dictionary = transformer.Dictionary.ToList();
            // This code is here just to show that the alphabetical order of the Dictionary does not matter
            dictionary.Shuffle();

            List<string> currentFrontier = new List<string>();
            List<string> nextFrontier = new List<string>();

            Func<string, string, IList<string>> extractPath = (start, end) =>
            {
                List<string> path = new List<string>();
                string current = end;
                path.Add(end);

                while (current != start)
                {
                    var parent = parents[current];
                    
                    // Correct Caps
                    if (Fastenshtein.AutoCompleteLevenshtein.Distance(current, parent) > 1)
                    {
                        var parentChars = parent.ToCharArray();
                        for (int i = 0; i < current.Length; i++)
                        {
                            string curLetter = current.Substring(i, 1);
                            string parLetter = parent.Substring(i, 1);
                            bool upperCase = curLetter == curLetter.ToUpper();

                            parLetter = upperCase ? parLetter.ToUpper() : parLetter.ToLower();
                            parentChars[i] = parLetter[0];
                        }
                        parent = new string(parentChars);
                    }

                    current = parent;
                    path.Add(current);
                }
                path.Reverse();
                return path;
            };

            currentFrontier.Add(transformer.StartWord);
            parents.Add(transformer.StartWord, transformer.EndWord);
            // Build up a Parents Dictionary of everything that is within 1 letter change
            // of the Start word and in the Dictionary.
            // Then do the same for each word found and same again for each of those.
            // When the End word is the word we are checking we should be able to
            // walk the Parents Backwards to get our list
            while (currentFrontier.Count > 0)
            {
                foreach (string s in currentFrontier)
                {
                    
                    nextFrontier.AddRange(dictionary.Where(w => w.Length == transformer.StartWord.Length && Fastenshtein.AutoCompleteLevenshtein.Distance(w.ToLower(), s.ToLower()) == 1));

                    foreach (var word in nextFrontier) if (!parents.Keys.Contains(word)) parents.Add(word, s);

                    if (nextFrontier.Any(w => w.ToLower() == transformer.EndWord.ToLower()))
                    {
                        extractPath(transformer.StartWord, transformer.EndWord).ToList().ForEach(w => transformer.Transforms.Add(w));

                        return;
                    }
                }
                currentFrontier = new List<string>(nextFrontier);
                nextFrontier.Clear();
            }
        }

    }
}

