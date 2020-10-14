using Bitsmith.BusinessProcess;
using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith
{
    public static class SettingsExtensions
    {

        public static UserSettings Default(this UserSettings model)
        {
            model.CreatedAt = DateTime.Now;
            model.Machine = Environment.MachineName;
            model.Username = Environment.UserName;

            return model;
        }
        public static Settings Default(this Settings model)
        {
            model.Workflows = new List<Workflow>().Default();
            HashSet<string> hs = new HashSet<string>();
            var list = Resources.indexer_exclusions.Split(new char[] { '\r','\n',',',';',':' },StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in list)
            {
                string s = item.Trim().ToLowerInvariant();
                if (hs.Add(s))
                {
                    model.IndexExclusions.Add(s);
                }
            }
            return model;
        }

        public static LanguageSettings Default(this LanguageSettings settings)
        {
            settings.Language = "en-US";
            var list = Resources.indexer_exclusions.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> hs = new HashSet<string>();
            foreach (var item in list)
            {
                var s = item.Trim().ToLower();
                if (hs.Add(s))
                {
                    settings.Tokens.Add(new Token() { Language = "en-US", Content = s, Type = TokenTypeOption.Stop });
                }
            }
            return settings;
        }

        private static List<Workflow> Default(this List<Workflow> list)
        {
            list.Add( new Workflow() 
            { 
                Id = AppConstants.Defaults.WorkflowId, 
                Name = "Software Task Long", 
                Display = "Task (long)", 
                Machine = new StateMachine().three() 
            });
            list.Add(new Workflow()
            {
                Id = Guid.NewGuid().ToString().ToLower(),
                Name = "Software Task Short",
                Display = "Task (short)",
                Machine = new StateMachine().two()
            });
            list.Add(new Workflow()
            {
                Id = Guid.NewGuid().ToString().ToLower(),
                Name = "Task",
                Display = "Simple Task",
                Machine = new StateMachine().one()
            });

            return list;
        }

        public static StateMachine Default(this StateMachine model)
        {
            model.States = new List<State>();
            model.Transitions = new List<Transition>();
            return model;
        }

        private static StateMachine one(this StateMachine model)
        {
            model.States.Add(new State() { Display = "Ready", Name = "ready" });
            model.States.Add(new State() { Display = "In Progress", Name = "in-progress" });
            model.States.Add(new State() { Display = "Completed", Name = "completed" });
            model.States.Add(new State() { Display = "Abandoned", Name = "abandoned" });

            model.Transitions.Add(new Transition() { OriginState = "ready", DestinationState = "in-progress", Name = "in-progress", Display = "Begin work" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "completed", Name = "Completed", Display = "Work done" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });

            model.EndStates = new List<string>() { "completed", "abandoned" };
            model.BeginState = "ready";
            return model;
        }


        private static StateMachine two(this StateMachine model)
        {

            model.States.Add(new State() { Display = "Ready", Name = "ready" });
            model.States.Add(new State() { Display = "In Progress", Name = "in-progress" });
            model.States.Add(new State() { Display = "On Hold", Name = "on-hold" });
            model.States.Add(new State() { Display = "Blocked", Name = "blocked" });
            model.States.Add(new State() { Display = "Completed", Name = "completed" });
            model.States.Add(new State() { Display = "Abandoned", Name = "abandoned" });

            model.Transitions.Add(new Transition() { OriginState = "ready", DestinationState = "in-progress", Name = "in-progress", Display = "Begin work" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "completed", Name = "completed", Display = "Work done" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "on-hold", Name = "on-hold", Display = "On hold" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });

            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "blocked", Name = "blocked", Display = "Blocked" });
            model.Transitions.Add(new Transition() { OriginState = "on-hold", DestinationState = "in-progress", Name = "in-progress", Display = "Back to work" });
            model.Transitions.Add(new Transition() { OriginState = "on-hold", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });
            model.Transitions.Add(new Transition() { OriginState = "blocked", DestinationState = "in-progress", Name = "in-progress", Display = "Unblock" });

            model.Transitions.Add(new Transition() { OriginState = "blocked", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });

            model.EndStates = new List<string>() { "abandoned", "completed" };
            model.BeginState = "ready";
            return model;
        }

        private static StateMachine three(this StateMachine model)
        {
            model.States.Add(new State() { Display = "Requirements", Name = "requirements" });
            model.States.Add(new State() { Display = "Estimation", Name = "estimation" });
            model.States.Add(new State() { Display = "Ready", Name = "ready" });
            model.States.Add(new State() { Display = "Analysis", Name = "analysis" });
            model.States.Add(new State() { Display = "In Progress", Name = "in-progress" });
            model.States.Add(new State() { Display = "Work Done", Name = "work-done" });
            model.States.Add(new State() { Display = "Ready for Validation", Name = "ready-for-val" });
            model.States.Add(new State() { Display = "Validation", Name = "validation" });
            model.States.Add(new State() { Display = "On Hold", Name = "on-hold" });
            model.States.Add(new State() { Display = "Blocked", Name = "blocked" });
            model.States.Add(new State() { Display = "Completed", Name = "completed" });
            model.States.Add(new State() { Display = "Abandoned", Name = "abandoned" });

            model.Transitions.Add(new Transition() { OriginState = "requirements", DestinationState = "estimation", Name = "estimation", Display = "Estimate" });
            model.Transitions.Add(new Transition() { OriginState = "requirements", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });
            model.Transitions.Add(new Transition() { OriginState = "estimation", DestinationState = "ready", Name = "ready", Display = "Ready" });
            model.Transitions.Add(new Transition() { OriginState = "ready", DestinationState = "analysis", Name = "analysis", Display = "Analyze" });
            model.Transitions.Add(new Transition() { OriginState = "analysis", DestinationState = "in-progress", Name = "in-progress", Display = "Begin work" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "work-done", Name = "work-done", Display = "Work done" });
            model.Transitions.Add(new Transition() { OriginState = "work-done", DestinationState = "ready-for-val", Name = "ready-for-val", Display = "Ready for Validation" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "on-hold", Name = "on-hold", Display = "On hold" });
            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });

            model.Transitions.Add(new Transition() { OriginState = "in-progress", DestinationState = "blocked", Name = "blocked", Display = "Block" });
            model.Transitions.Add(new Transition() { OriginState = "on-hold", DestinationState = "in-progress", Name = "in-progress", Display = "Back to work" });
            model.Transitions.Add(new Transition() { OriginState = "on-hold", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });
            model.Transitions.Add(new Transition() { OriginState = "blocked", DestinationState = "in-progress", Name = "in-progress", Display = "Unblock" });
            model.Transitions.Add(new Transition() { OriginState = "ready-for-val", DestinationState = "validation", Name = "validation", Display = "Move to Validation" });
            model.Transitions.Add(new Transition() { OriginState = "validation", DestinationState = "completed", Name = "completed", Display = "Pass Validation" });
            model.Transitions.Add(new Transition() { OriginState = "validation", DestinationState = "in-progress", Name = "in-progress", Display = "Fail Validation" });
            model.Transitions.Add(new Transition() { OriginState = "validation", DestinationState = "in-progress", Name = "requirements", Display = "Back to Requirements" });
            model.Transitions.Add(new Transition() { OriginState = "blocked", DestinationState = "abandoned", Name = "abandoned", Display = "Abandon" });

            model.EndStates = new List<string>() { "abandoned", "completed" };
            model.BeginState = "requirements";
            return model;
        }

    }
}
