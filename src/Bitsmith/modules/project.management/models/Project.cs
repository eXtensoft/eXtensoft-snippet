using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Bitsmith.BusinessProcess;
using Bitsmith.Models;

namespace Bitsmith.ProjectManagement
{
    public class Project
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("createdAt")]
        public DateTime CreatedAt { get; set; }
        [XmlElement("Domain")]
        public List<Domain> Domains { get; set; }
        [XmlElement("Workflow")]
        public List<Workflow> Workflows { get; set; }
        [XmlElement("Task")]
        public List<TaskItem> Items { get; set; }
    }
}
