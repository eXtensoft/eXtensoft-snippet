using Bitsmith.Models;
using Bitsmith.Parsing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class TabularDataViewModel : ContentItemViewModel
    {

        private IDataParser _DataParser;

        #region properties




        #endregion

        private ObservableCollection<DataFieldViewModel> _Fields;
        public ObservableCollection<DataFieldViewModel> Fields
        {
            get { return _Fields; }
            set
            {
                _Fields = value;
                OnPropertyChanged("Fields");
            }
        }


        private DataTable _Data;
        public DataTable Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
                OnPropertyChanged("Data");
                OnPropertyChanged("HasData");
            }
        }

        private int _SelectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }


        public bool HasData
        {
            get
            {
                return Data != null && Data.Rows.Count > 0;
            }
        }

        private ICommand _CopyToClipboardCommand;
        public ICommand CopyToClipboardCommand
        {
            get
            {
                if (_CopyToClipboardCommand == null)
                {
                    _CopyToClipboardCommand = new RelayCommand<DataGrid>( 
                        new Action<DataGrid>(CopyToClipboard),CanCopyToClipboard);
                }
                return _CopyToClipboardCommand;
            }
        }
        private bool CanCopyToClipboard(DataGrid dataGrid)
        {
            return HasData;
        }
        private void CopyToClipboard(DataGrid dataGrid)
        {
            dataGrid.SelectAll();
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
        }

        private ICommand _RefreshDataCommand;
        public ICommand RefreshDataCommand
        {
            get
            {
                if (_RefreshDataCommand == null)
                {
                    _RefreshDataCommand = new RelayCommand(
                    param => RefreshData(),
                    param => CanRefreshData());
                }
                return _RefreshDataCommand;
            }
        }
        private bool CanRefreshData()
        {
            return true;
        }
        private void RefreshData()
        {
            if (HasData)
            {
                Data = null;
                Model.Body = string.Empty;
            }
            else if(!string.IsNullOrWhiteSpace(Model.Body))
            {
                ParseData(Model.Body);
                SelectedIndex = 1;
            }
            else
            {
                SelectFile();
                SelectedIndex = 1;
            }
        }

        private ICommand _SelectFileCommand;
        public ICommand SelectFileCommand
        {
            get
            {
                if (_SelectFileCommand == null)
                {
                    _SelectFileCommand = new RelayCommand(
                    param => SelectFile(),
                    param => CanSelectFile());
                }
                return _SelectFileCommand;
            }
        }
        private bool CanSelectFile()
        {
            return true;
        }
        private void SelectFile()
        {
            FileInfo info = null;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                info = new FileInfo(dialog.FileName);
            }
            if (!info.Exists)
            {
                MessageBox.Show($"{dialog.FileName} doesn't exist");
            }
            else
            {
                ParseFile(info);
            }
        }

        private void ParseFile(FileInfo info)
        {
            if (_DataParser.TryParse(info, out TabularData data))
            {
                SetData(data);
            }
            else
            {
                MessageBox.Show(data.Message);
            }
        }

        private void ParseData(string body)
        {
            if (_DataParser.TryParse(body, out TabularData data))
            {
                SetData(data);
            }
            else
            {
                MessageBox.Show(body);
            }
        }
        private void SetData(TabularData tabularData)
        {
            Data = tabularData.Data;
            Fields = new ObservableCollection<DataFieldViewModel>(from x in  tabularData.Fields select new DataFieldViewModel(x));
        }

        private ICommand _RefreshProfileCommand;
        public ICommand RefreshProfileCommand
        {
            get
            {
                if (_RefreshProfileCommand == null)
                {
                    _RefreshProfileCommand = new RelayCommand(
                    param => RefreshProfile(),
                    param => CanRefreshProfile());
                }
                return _RefreshProfileCommand;
            }
        }
        private bool CanRefreshProfile()
        {
            return true;
        }
        private void RefreshProfile()
        {

        }





        public TabularDataViewModel(ContentItem model)
        :base(model){
            IDataFormatResolver resolver = new DataFormatResolver();
            List<IParser> parsers = new List<IParser>();
            parsers.Add(new Parsing.MongoDB.MongoDBJsonParser());

            IDataParserFactory factory = new DataParserFactory(resolver, parsers);
            _DataParser = new DataParser(factory);
        }
    }
}
