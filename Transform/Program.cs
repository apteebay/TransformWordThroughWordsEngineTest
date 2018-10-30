using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transform
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length==4)
            {
                // args[0] -- DictionaryFile 
                // args[1] -- StartWord 
                // args[2] -- EndWord 
                // args[3] -- ResultFile 
                var transfomer = new WordTransformTxtDict.WordTransformer(args[0])
                {
                    StartWord = args[1],
                    EndWord = args[2]
                };

                if (transfomer.IsValid && transfomer.Transforms.Any()) new Utilities.WriteListToTxt(args[3], transfomer.Transforms);
                else new Utilities.WriteListToTxt(args[3], new string[] { string.Format("No path from {0} to {1} found", args[1], args[2]) });
            }
        }
    }
}
