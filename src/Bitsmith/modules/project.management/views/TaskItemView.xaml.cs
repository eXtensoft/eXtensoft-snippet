using Bitsmith.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bitsmith
{
    /// <summary>
    /// Interaction logic for TaskItemView.xaml
    /// </summary>
    public partial class TaskItemView : UserControl, INotifyPropertyChanged
    {

        public TaskItemView()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as TaskItemViewModel;
            if (vm != null)
            {
                if (String.IsNullOrWhiteSpace(vm.WorkflowId))
                {
                    vm.WorkflowId = AppConstants.Defaults.WorkflowId;
                }
                if (vm.Machine == null && Workspace.Instance.ViewModel.Settings.Workflows.Any(x=>x.Id.Equals(vm.WorkflowId,StringComparison.OrdinalIgnoreCase)))
                {
                    vm.Machine = Workspace.Instance.ViewModel.Settings.Workflows.FirstOrDefault(x => x.Id.Equals(vm.WorkflowId, StringComparison.OrdinalIgnoreCase)).Model.Machine.Clone();
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
