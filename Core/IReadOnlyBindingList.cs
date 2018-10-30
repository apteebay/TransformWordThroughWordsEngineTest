using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
  internal interface IReadOnlyBindingList
  {
    bool IsReadOnly { get; set; }
  }
}
