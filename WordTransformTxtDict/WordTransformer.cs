using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransformWordInterface;
using Utilities;

namespace WordTransformTxtDict
{
    public class WordTransformer : IWordTransformer
    {
        private bool? c_valid;
        // Default to non null values to avoid checking for nulls later
        private string c_startWord=string.Empty;
        private string c_endWord=string.Empty;
        private IEnumerable<string> c_dictionary;
        private WordTransformerEngine c_engine;

        public WordTransformer(string dictionaryFilePath) : this(new ReadTxtToList(dictionaryFilePath).Lines) { }
        public WordTransformer(IEnumerable<string> dictionary) => Dictionary = dictionary;

        public string StartWord
        {
            get => c_startWord;
            set
            {
                c_startWord = value;
                validateReset();
            }
        }
        public string EndWord
        {
            get => c_endWord;
            set
            {
                c_endWord = value;
                validateReset();
            }
        }
        public IEnumerable<string> Dictionary
        {
            get => c_dictionary;
            set
            {
                c_dictionary = value;
                validateReset();
            }
        }
        public IList<string> Transforms { get; private set; }

        public WordTransformerEngine Engine
        {
            get
            {
                if (c_engine == null) c_engine = new Engine(method);
                return c_engine;
            }
            set => c_engine = value;
        }

        public bool IsValid
        {
            get
            {
                if (!c_valid.HasValue) validateReset();
                return c_valid.GetValueOrDefault();
            }
        }


        /// <summary>
        /// If anything changes then reset the Transforms and reValidate
        /// </summary>
        /// <remarks>
        /// We could have gone for something more complex and tracking PropertyChanged but for this
        /// senario this is good and quick enouth. A more co,plete solution would report back
        /// which validation failed.
        /// </remarks>
        private void validateReset()
        {
            Transforms = new List<string> { };
            c_valid =
                (StartWord?.Length == EndWord?.Length) &&
                (Dictionary?.Any(w => w.ToLower() == StartWord?.ToLower())).GetValueOrDefault() &&
                (Dictionary?.Any(w => w.ToLower() == EndWord?.ToLower())).GetValueOrDefault();

            if (c_valid.GetValueOrDefault()) Engine.Run(this);
        }

        private void method(IWordTransformer transformer)
        {
            var allcharacters = new Utilities.UniqueChars(transformer.Dictionary.Where(w => w.Length == transformer.StartWord.Length)).Chars.ToList();
            // Adding the Start and End word Chars to the list ensures that if Caps exist in either word but not in Dictionary
            // they will be scanned and matched
            allcharacters.AddRange(transformer.StartWord.Distinct());
            allcharacters.AddRange(transformer.EndWord.Distinct());
            allcharacters = allcharacters.Distinct().OrderBy(c => c).ToList();
            

            Dictionary<string, string> parents = new Dictionary<string, string>();
            // As we're putting the dictionary into a HashSet alphabetical order of the orginal dictionary
            // does not matter
            HashSet<string> searchDictionary = new HashSet<string>(transformer.Dictionary,StringComparer.CurrentCultureIgnoreCase);
            // The test input is 'Spin' to 'Spot' but the dictionary does not contain either. It actually contains 'spin' and 'spot'
            // By making the StringComparer ignore the case we can still find matches

            List<string> currentFrontier = new List<string>();
            List<string> nextFrontier = new List<string>();

            Func<string, string, IList<string>> extractPath = (start, end) =>
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
            };

            currentFrontier.Add(transformer.StartWord);
 
            // Build up a Parents Dictionary of everything that is within 1 letter change
            // of the Start word and in the Dictionary.
            // Then do the same for each word found and same again for each of those.
            // When the End word is the word we are checking we should be able to
            // walk the Parents Backwards to get our list
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
                            if (searchDictionary.Contains(newWord.TrimEnd()))
                            {
                                //avoid traversing a previously traversed node
                                if (!parents.Keys.Contains(newWord))
                                {
                                    parents.Add(newWord.ToString(), s);
                                    nextFrontier.Add(newWord);
                                }

                            }
                            // check for case also here otherwise we would have to clean the Transforms for case after (as we do in the Fastenstien method)
                            if (newWord.ToString() == transformer.EndWord) 
                            {
                                extractPath(transformer.StartWord, transformer.EndWord).ToList().ForEach(w => transformer.Transforms.Add(w));
                                return;
                            }
                        }
                    }
                }
                currentFrontier = new List<string>(nextFrontier);
                nextFrontier.Clear();
            }
        }

    }

    public class Engine: WordTransformerEngine
    {
        public Engine(TransformMethod method) => Method = method;

        protected override TransformMethod Method { get; set; }
    }
}
