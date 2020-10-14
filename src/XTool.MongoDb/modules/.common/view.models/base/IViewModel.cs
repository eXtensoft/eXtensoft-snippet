using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTool.ViewModels
{
    public interface IViewModel
    {
        bool IsDirty { get; }
        bool MarkedForRemoval { get; }
    }
}
