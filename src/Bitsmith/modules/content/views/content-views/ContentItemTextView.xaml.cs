using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bitsmith
{
    /// <summary>
    /// Interaction logic for ContentItemTextView.xaml
    /// </summary>
    public partial class ContentItemTextView : UserControl, INotifyPropertyChanged
    {

        private ICommand _UpdateBodyCommand;
        public ICommand UpdateBodyCommand
        {
            get
            {
                if (_UpdateBodyCommand == null)
                {
                    _UpdateBodyCommand = new RelayCommand(param => UpdateBody(), param => CanUpdateBody());
                }
                return _UpdateBodyCommand;
            }
        }

        private bool CanUpdateBody()
        {
            return _IsDirty;
        }
        private void UpdateBody()
        {
            TextRange textRange = new TextRange(rtbBody.Document.ContentStart, rtbBody.Document.ContentEnd);
            viewmodel.Body = textRange.Text;
            rtbBody.Document = viewmodel.Body.ToFlowDocument(viewmodel.SearchTerm, Brushes.Yellow);
        }

        private bool _IsDirty = false;

        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                _IsDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        private ContentItemViewModel viewmodel;
        private bool _HasLoaded = false;
        public ContentItemTextView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            viewmodel = DataContext as ContentItemViewModel;
            if (viewmodel != null && !String.IsNullOrWhiteSpace(viewmodel.SearchTerm))
            {
                rtbBody.Document = viewmodel.Body.ToFlowDocument(viewmodel.SearchTerm, Brushes.Yellow);
                _HasLoaded = true;
            }
        }

        private void rtbBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_HasLoaded && !_IsDirty)
            {
                IsDirty = true;
                MessageBox.Show("rtb text changed");
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
