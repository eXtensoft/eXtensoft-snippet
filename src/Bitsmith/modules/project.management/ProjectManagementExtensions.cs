using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ProjectManagement
{
    public static class ProjectManagementExtensions
    {
        public static List<Disposition> ToDispositions(this IEnumerable<string> values,string key)
        {
            List<Disposition> list = new List<Disposition>();
            foreach (var value in values)
            {
                list.Add(new Disposition() { Display = value, Token = value.ToToken(),Key = key });
            }
            return list;
        }
        public static TaskItem Default(this TaskItem model, Domain domain)
        {
            DateTime now = DateTime.Now;
            var id = Guid.NewGuid().ToString();
            model.Id = id;
            model.Display = "new task";
            model.Identifier = new TagIdentifier() { Id = id, MasterId = domain.Id };
            model.CreatedOn = now;
            model.DueOn = DateTime.Now.AddDays(3);
            model.Dispositions = new List<Disposition>().Default(now,ScaleOption.None);
            model.Dispositions.Add(new Disposition() { StartedAt = now, Key = "domain", Id = domain.Id });
            return model;
        }

        public static List<Disposition> Default(this List<Disposition> list, DateTime target, ScaleOption option)
        {
            list.Add(new Disposition() 
            { 
                Key = "importance",
                Token = option.ToString().ToLower(), 
                Display = option.ToString(), 
                StartedAt = target            
            });
            list.Add(new Disposition()
            {
                Key = "urgency",
                Token = option.ToString().ToLower(),
                Display = option.ToString(),
                StartedAt = target
            });
            list.Add(new Disposition()
            {
                Key = "status",
                Token = StatusOption.None.ToString().ToLower(),
                Display = StatusOption.None.ToString(),
                StartedAt = target
            });
            return list;
        }
    }
}
