using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitsmith.ViewModels
{
    public static class WorkflowExtensions
    {

        public static Disposition Archive(this Disposition model)
        {
            model.StartedAt = DateTime.Now;
            model.Id = "archived";
            model.Key = "status";
            model.Token = "archived";
            model.Display = "Archived";
            return model;
        }

        public static Disposition ToDisposition(this State model)
        {
            Disposition disposition = new Disposition() 
            {  
                Display = model.Display, 
                Id = model.Name, 
                Key = "status", 
                StartedAt = DateTime.Now, 
                Token = model.Name           
            };
            return disposition;
        }
        public static WorkflowStep ToStep(this State model)
        {
            WorkflowStep step = new WorkflowStep()
            {
                Display = model.Display,
                Name = model.Name,
                IsTransition = false
            };
            return step;
        }

        public static WorkflowStep ToStep(this Transition model)
        {
            WorkflowStep step = new WorkflowStep()
            {
                Display = model.Display,
                Name = model.Name,
                IsTransition = true
            };
            return step;
        }

        public static IEnumerable<WorkflowStep> ToSteps(this IEnumerable<Transition> list)
        {
            return (from x in list select x.ToStep());
        }

        public static void SetState(this StateMachine machine, string state = "")
        {
            machine.CurrentState = (!string.IsNullOrWhiteSpace(state) &&
                machine.States.Any(x => x.Name.Equals(state, StringComparison.OrdinalIgnoreCase))) ? 
                state : machine.BeginState;
        }

        public static StateMachine Clone(this StateMachine model)
        {
            StateMachine clone = GenericObjectManager.Clone<StateMachine>(model);
            return clone;
        }

    }
}
