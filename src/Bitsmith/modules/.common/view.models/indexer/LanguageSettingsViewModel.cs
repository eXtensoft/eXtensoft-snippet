using Bitsmith.NaturalLanguage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Bitsmith.ViewModels
{
    public class LanguageSettingsViewModel : ViewModel<LanguageSettings>
    {
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
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


        private ICommand _AddTokenCommand;
        public ICommand AddTokenCommand
        {
            get
            {
                if (_AddTokenCommand == null)
                {
                    _AddTokenCommand = new RelayCommand(
                    param => AddToken(),
                    param => CanAddToken());
                }
                return _AddTokenCommand;
            }
        }
        private bool CanAddToken()
        {
            return !string.IsNullOrWhiteSpace(_Input);
        }
        private void AddToken()
        {            
            if(!Model.Tokens.Any(x => x.Content == Input && x.Type == SelectedType))
            {
                Tokens.Add(new LanguageTokenViewModel(new Token() 
                { 
                    Content = Input, 
                    Language = Language, 
                    Type = SelectedType 
                }));
            }
            else
            {

            }
        }


        private string _Input;
        public string Input
        {
            get { return _Input; }
            set
            {
                _Input = value;
                OnPropertyChanged("Input");
            }
        }

        private TokenTypeOption _SelectedType = TokenTypeOption.Stop;
        public TokenTypeOption SelectedType
        {
            get { return _SelectedType; }
            set
            {
                _SelectedType = value;
                OnPropertyChanged("SelectedType");
                Items.Refresh();
            }
        }

        private ICommand _RemoveTokenCommand;
        public ICommand RemoveTokenCommand
        {
            get
            {
                if (_RemoveTokenCommand == null)
                {
                    _RemoveTokenCommand = new RelayCommand<LanguageTokenViewModel>(
                    new Action<LanguageTokenViewModel>(RemoveToken));
                }
                return _RemoveTokenCommand;
            }
        }
        private void RemoveToken(LanguageTokenViewModel viewModel)
        {
            Tokens.Remove(viewModel);
        }

        public string Language
        {
            get
            {
                return Model.Language;
            }
            set
            {
                Model.Language = value;
                OnPropertyChanged("Language");
            }
        }

        private ICollectionView _Items;
        public ICollectionView Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = CollectionViewSource.GetDefaultView(Tokens);
                    _Items.Filter = FilterTokens;
                }
                return _Items;
            }
        }

        private bool FilterTokens(object obj)
        {
            bool b = false;
            var vm = obj as LanguageTokenViewModel;
            if (vm != null)
            {
                b = vm.TokenType == _SelectedType;
            }
            return b;
        }

        public ObservableCollection<LanguageTokenViewModel> Tokens { get; set; }


        public LanguageSettingsViewModel(LanguageSettings model)
        {
            Model = model;
            Tokens = new ObservableCollection<LanguageTokenViewModel>(from x in model.Tokens select new LanguageTokenViewModel(x));
            Tokens.CollectionChanged += Tokens_CollectionChanged;

        }

        private void Tokens_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var vm = item as LanguageTokenViewModel;
                    if (vm != null)
                    {
                        Model.Tokens.Add(vm.Model);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var vm = item as LanguageTokenViewModel;
                    if (vm != null)
                    {
                        Model.Tokens.Remove(vm.Model);
                    }
                }
            }



        }
    
    
    }
}
