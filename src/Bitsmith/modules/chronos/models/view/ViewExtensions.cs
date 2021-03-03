using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models.Views
{
    public static class ViewExtensions
    {
        public static List<TaskViewItem> Build(this List<TaskItem> taskItems, List<TimeEntry> timeEntries = null)
        {
            List<TaskViewItem> list = new List<TaskViewItem>();
            var domains = taskItems.ToLookup(x => x.DomainId);
            foreach (var domain in domains)
            {
                var domainTaskView = new TaskViewItem().Default(domain.Key);
                domainTaskView.Build(domain.ToList(),timeEntries);
                list.Add(domainTaskView);
            }
            return list;
        }
        private static void Build(this TaskViewItem viewItem, List<TaskItem> taskItems,  List<TimeEntry> timeEntries = null, int level = 0)
        {
            level++;
            foreach (var item in taskItems.Where(t=>t.Identifier.MasterId.Equals(viewItem.Id, StringComparison.OrdinalIgnoreCase)))
            {
                if (viewItem.Tasks == null)
                {
                    viewItem.Tasks = new List<TaskViewItem>();
                }
                TaskViewItem view = new TaskViewItem(item).Default(item,viewItem);
                if (timeEntries != null)
                {
                    var entries = timeEntries.Where(x => x.Task.Id.Equals(view.Data.Id, StringComparison.OrdinalIgnoreCase));
                    view.TimeEntries.AddRange(entries);
                }
                view.Build(taskItems, timeEntries, level);
                viewItem.Tasks.Add(view);
            }
            level--;
        }

        private static TaskViewItem Default(this TaskViewItem taskViewItem, TaskItem taskItem,TaskViewItem masterTaskView)
        {
            taskViewItem.Id = taskItem.Id;
            taskViewItem.TaskId = taskItem.Identifier.Display;
            taskViewItem.Display = taskItem.Display;
            taskViewItem.Master = masterTaskView;
            return taskViewItem;
        }

        public static TaskViewItem Default(this TaskViewItem taskViewItem, string id)
        {
            taskViewItem.Id = id;
            taskViewItem.Display = $"{id}";
            return taskViewItem;
        }
    }
}
