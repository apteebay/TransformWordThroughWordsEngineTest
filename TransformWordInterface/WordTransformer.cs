using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransformWordInterface
{
    public interface IWordTransformer
    {
        string StartWord { get; set; }
        string EndWord { get; set; }
        IEnumerable<string> Dictionary { get; set; }
        IList<string> Transforms { get; }
        bool IsValid { get; }
        WordTransformerEngine Engine { get; set; }
    }
}
