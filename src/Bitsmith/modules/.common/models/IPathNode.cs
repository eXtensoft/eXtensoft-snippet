using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public interface IPathNode
    {
        string Path { get; set; }

        string Slug { get; set; }
        string Display { get; set; }
    }
}
