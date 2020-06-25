using Bitsmith.Models;
using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ProjectManagement
{
    public class ProjectTask
    {
        public AggregationOption Aggregation { get; set; }
        public TaskItem Model { get; set; }
        public string Id { get; set; }

        public string Display { get; set; }

        public TaskEstimate Estimate { get; set; }
        public List<TimeEntry> Effort { get; set; }


        public List<ProjectTask> Items { get; set; } = new List<ProjectTask>();

      

        public ProjectTask(TaskItem model,  AggregationOption aggregation)
        {
            Model = model;
            Aggregation = aggregation;
        }
    }
}
