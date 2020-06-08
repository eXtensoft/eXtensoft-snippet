using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ProjectManagement
{
    public class TaskItem
    {
        public string Id { get; set; }
        public string GroupId { get; set; }

        public TagIdentifier Identifier { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DueOn { get; set; }

        public string Display { get; set; }
        public string Description { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();

        public List<Link> Links { get; set; } = new List<Link>();
        //public ScaleOption Urgency { get; set; }
        //public ScaleOption Importance { get; set; }
        //public List<TaskStatus> Status { get; set; }

        public List<Disposition> Dispositions { get; set; }

    }
}
