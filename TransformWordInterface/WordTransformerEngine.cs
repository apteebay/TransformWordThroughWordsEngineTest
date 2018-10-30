using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformWordInterface
{
    public delegate void TransformMethod(IWordTransformer transformer);

    public abstract class WordTransformerEngine
    {
        protected abstract TransformMethod Method { get; set; }

        public void Run(IWordTransformer transformer) => Method(transformer);
    }
}
