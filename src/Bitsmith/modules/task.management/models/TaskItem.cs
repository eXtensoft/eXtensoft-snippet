using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith.ProjectManagement
{
    public class TaskItem
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("group")]
        public string GroupId { get; set; }
        [XmlAttribute("workflow")]
        public string WorkflowId { get; set; }
        [XmlAttribute("domain")]
        public string DomainId { get; set; }
        [XmlAttribute("dueOn")]
        public DateTime CreatedOn { get; set; }
        public TagIdentifier Identifier { get; set; }
        public DateTime DueOn { get; set; }
        public string Display { get; set; }
        public string Description { get; set; }

        public List<Disposition> Dispositions { get; set; }


    }
}
