using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models.Views
{
    public class TaskViewItem : ViewItem
    {
        
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
        public TaskItem Data { get; set; }
        public string TaskId { get; set; }
        public List<ActivityViewItem> Activities { get; set; }
        public List<TaskViewItem> Tasks { get; set; }

        public TaskViewItem()
        {

        }
        public TaskViewItem(TaskItem data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"{Display} {TimeEntries.Count}";
        }
    }
}
