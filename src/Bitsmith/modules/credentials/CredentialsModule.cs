using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Bitsmith.ViewModels
{
    public class CredentialsModule : Module
    {
        public string Display => "Credentials";

        private ICommand _AddCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                {
                    _AddCommand = new RelayCommand(
                    param => Add(),
                    param => CanAdd());
                }
                return _AddCommand;
            }
        }
        private bool CanAdd()
        {
            return true;
        }
        private void Add()
        {
            var item = new ContentItem() { Mime = "text/credential", Scope = ScopeOption.Encrypt, Display = "Credentials for {}", Id = Guid.NewGuid().ToString().ToLower() };
            item.Properties.DefaultTags();
            item.Properties.Add(new Property()
            {
                Name = $"{AppConstants.Tags.Prefix}-{AppConstants.Tags.Credentials}",
                Value = item.Mime
            });
            var vm = new CredentialViewModel(item);
            Items.Add(vm);
        }


        private ICommand _ExportToCommand;
        public ICommand ExportToCommand
        {
            get
            {
                if (_ExportToCommand == null)
                {
                    _ExportToCommand = new RelayCommand(
                    param => ExportTo(),
                    param => CanExportTo());
                }
                return _ExportToCommand;
            }
        }
        private bool CanExportTo()
        {
            return Items != null && Items.Count > 0;
        }
        private void ExportTo()
        {
            var json = (from x in Items select new {
                Display = x.Display,
                Location = x.Location,
                Identifier = x.Identifier,
                Secret = x.Secret,
                Note = x.Note
            }).ToJson();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "credentials.json";
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, json);
            }
        }

        private ObservableCollection<CredentialViewModel> _Items;
        public ObservableCollection<CredentialViewModel> Items
        {
            get
            {
                if (_Items == null)
                {
                    var found = Workspace.Instance.ViewModel.Content.Content.Items.Where(x => x.Mime.Equals("text/credential"));
                    _Items = new ObservableCollection<CredentialViewModel>( from x in found select new CredentialViewModel(x));
                    _Items.CollectionChanged += _Items_CollectionChanged;
                }
                return _Items;
            }
            set { }
        }

        private void _Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as CredentialViewModel;
                    if (vm != null)
                    {
                        Workspace.Instance.ViewModel.Content.Content.Items.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as CredentialViewModel;
                    if (vm != null)
                    {
                        vm.MarkedForRemoval = true;
                    }
                }
            }
        }


        public CredentialsModule(IDataService dataService)
        {
            DataService = dataService;
        }

    }
}
