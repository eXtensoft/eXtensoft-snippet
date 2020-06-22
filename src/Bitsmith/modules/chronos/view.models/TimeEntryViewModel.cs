using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class TimeEntryViewModel : ViewModel<TimeEntry>
    {
        public string Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
                OnPropertyChanged("Id");
            }
        }




        public TimeEntryViewModel(TimeEntry model)
        {
            Model = model;
        }
    }
}
