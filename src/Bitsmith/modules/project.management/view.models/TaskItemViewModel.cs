using Bitsmith.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Bitsmith.ViewModels
{
    public class TaskItemViewModel : ViewModel<TaskItem>
    {

        public List<Disposition> UrgencySelections { get; set; }
        public List<Disposition> ImportanceSelections { get; set; }
        public List<Disposition> StatusSelections { get; set; }

        public string Display
        {
            get
            {
                return Model.Display;
            }
            set
            {
                Model.Display = value;
                OnPropertyChanged("Display");
            }
        }

        public string Identifier
        {
            get
            {
                return Model.Identifier.Display;
            }
            set
            {
                Model.Identifier.Display = value;
                Model.Identifier.Token = value.ToToken();
                OnPropertyChanged("Identifier");
            }
        }

        public DateTime DueOn
        {
            get
            {
                return Model.DueOn;
            }
            set
            {
                Model.DueOn = value;
                OnPropertyChanged("DueOn");
            }
        }



        private Disposition _Status;
        public Disposition Status
        {
            get
            {
                if (_Status == null)
                {
                    _Status = Model.Dispositions.Last(x => x.Key.Equals("status"));
                }
                return _Status;
            }
            set
            {
                value.StartedAt = DateTime.Now;
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        private Disposition _Urgency;
        public Disposition Urgency
        {
            get
            {
                if (_Urgency == null)
                {
                    _Urgency = Model.Dispositions.Last(x => x.Key.Equals("urgency"));
                }
                return _Urgency;
            }
            set
            {
                value.StartedAt = DateTime.Now;
                _Urgency = value;
                OnPropertyChanged("Urgency");
            }
        }

        private Disposition _Importance;
        public Disposition Importance
        {
            get
            {
                if (_Importance == null)
                {
                    _Importance = Model.Dispositions.Last(x => x.Key.Equals("importance"));
                }
                return _Importance;
            }
            set
            {
                value.StartedAt = DateTime.Now;
                _Importance = value;
                OnPropertyChanged("Importance");
            }
        }


        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                OnPropertyChanged("Description");
            }
        }



        public TaskItemViewModel(TaskItem model)
        {
            Model = model;
        }

        //protected override IEnumerable<string> GetProperties()
        //{
        //    return new string[] { "Urgency","Importance","Status" };
        //}
    }
}
