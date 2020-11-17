using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bitsmith.BusinessProcess;
using Bitsmith.Models;

namespace Bitsmith.ProjectManagement
{
    public class TaskManager
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        //[XmlElement("Domain")]
        //public List<Domain> Domains { get; set; } = new List<Domain>();
        [XmlElement("Workflow")]
        public List<Workflow> Workflows { get; set; } = new List<Workflow>();
        [XmlElement("Task")]
        public List<TaskItem> Items { get; set; } = new List<TaskItem>();
    }
}
