﻿using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.BusinessProcess
{
    public static class WorkflowExtensions
    {
        public static Workflow Default(this Workflow model)
        {
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.Name = "workflow";
            model.Display = "Workflow";
            model.Machine = new StateMachine().Default();
            return model;
        }
        public static Transition Default(this Transition model)
        {
            model.Name = "transition";
            return model;
        }

        public static State Default(this State model)
        {
            model.Name = "state";
            return model;
        }

        public static Builder Default(this Builder model)
        {
            model.Transitions = new List<Transition>();
            model.States = new List<State>();
            model.EndStates = new List<string>();
            model.Name = "state-machine";
            return model;
        }

        public static StateMachine Build(this BuilderViewModel builder)
        {
            StateMachine machine = new StateMachine();
            builder.States.ToList().ForEach(s => {
                machine.States.Add(new State() { Display = s.Display, Name = s.Name });
                if(s.IsEnd)
                {
                    machine.EndStates.Add(s.Name);
                }
                else if (s.IsBegin)
                {
                    machine.BeginState = s.Name;
                }
                
            });
            builder.Transitions.ToList().ForEach(t => {
                machine.Transitions.Add(new Transition() 
                { 
                    Display = t.Display, 
                    //Name = t.Name, 
                    OriginState = t.From.Name, 
                    DestinationState = t.To.Name 
                });

            });

            return machine;
        }

        public static bool IsEnd(this StateMachine machine)
        {
            return machine.EndStates.Contains(machine.CurrentState);
        }


        public static void SetBeginState(this WorkflowViewModel workflow, string stateName)
        {
            workflow.Model.Machine.BeginState = stateName;
            foreach (var state in workflow.States)
            {
                state.SetBegin(state.Name.Equals(stateName));
            }
        }

        public static void AddEndState(this WorkflowViewModel workflow, string stateName)
        {
            if (!workflow.Model.Machine.EndStates.Contains(stateName))
            {
                workflow.Model.Machine.EndStates.Add(stateName);
            }
            var found = workflow.States.FirstOrDefault(x => x.Name.Equals(stateName));
            if (found != null)
            {
                found.SetEnd(true);
            }
        }

        public static void RemoveEndState(this WorkflowViewModel workflow, string stateName)
        {
            if (workflow.Model.Machine.EndStates.Contains(stateName))
            {
                workflow.Model.Machine.EndStates.Remove(stateName);
            }
            var found = workflow.States.FirstOrDefault(x => x.Name.Equals(stateName));
            if (found != null)
            {
                found.SetEnd(false);
            }
        }



    }
}
