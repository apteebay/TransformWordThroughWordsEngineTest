using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TransformWordInterface;

namespace WordTransformMock
{
    public class WordTransformer : IWordTransformer
    {
        public WordTransformer() => Engine.Run(this);

        public string StartWord { get; set; } = "Spin";
        public string EndWord { get; set; } = "Spot";
        public IEnumerable<string> Dictionary { get; set; } = new string[] { "Spin", "Spit", "Spat", "Spot", "Span" };
        public IList<string> Transforms { get; private set; }
        public WordTransformerEngine Engine { get => new Engine(new TransformMethod(Method)); set { } }

        public bool IsValid => true;

        private static void Method(IWordTransformer transformer) =>
            // We know in this case that IWordTransformer is an istance of WordTransformer
            (transformer as WordTransformer).Transforms = new string[] { "Spin", "Spit", "Spot" };

        
    }
    public class Engine : WordTransformerEngine
    {
        public Engine(TransformMethod method) => Method = method;

        protected override TransformMethod Method { get; set; }
    }
}
