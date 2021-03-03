using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class WorkEffortViewModel : GroupingViewModel<TimeEntry>
    {

        private ChronosViewTypeOptions _SelectedView = ChronosViewTypeOptions.None;
        public ChronosViewTypeOptions SelectedView
        {
            get { return _SelectedView; }
            set
            {
                _SelectedView = value;
                AggregateView(_SelectedView);
            }
        }



        private int _MaxColumns = 7;
        public int MaxColumns
        {
            get { return _MaxColumns; }
            set
            {
                _MaxColumns = value;
                OnPropertyChanged("MaxColumns");
            }
        }
        public WorkEffortViewModel(IEnumerable<TimeEntry> items, ChronosViewTypeOptions options = ChronosViewTypeOptions.Week)
        :base(items){
            Display = items.FirstOrDefault().Task.Display;
            if (_SelectedView == ChronosViewTypeOptions.None)
            {
                SelectedView = options;
            }
        }

        private void AggregateView(ChronosViewTypeOptions selectedView)
        {
            //throw new NotImplementedException();
        }

    }
}
