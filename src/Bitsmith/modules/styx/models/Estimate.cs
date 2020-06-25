using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ProjectManagement
{
    public class Estimate
    {
        public string TaskId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Context { get; set; } // sizing, detail, technical
        public string Scale { get; set; } // hours, high-low, fibonacci, none
        public int Min { get; set; }
        public int Max { get; set; }
        public int Expected { get; set; }
        public int Confidence { get; set; }
        public TagIdentifier CreatedBy { get; set; }
    }
}
