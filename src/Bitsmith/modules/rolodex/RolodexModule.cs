using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bitsmith.DataServices.Abstractions;
using Bitsmith.Models;

namespace Bitsmith.ViewModels
{
    public class RolodexModule : Module<Card>
    {

        
        public ObservableCollection<CardViewModel> Items { get; set; }

        public RolodexModule(IDataService dataService)
        {
            DataService = dataService;
        }

        public override void Initialize()
        {
            Items = new ObservableCollection<CardViewModel>(from m in Models select new CardViewModel(m));
        }

        public override string Filepath()
        {
            return Path.Combine(AppConstants.RolodexDirectory, DataService.Filepath<Card>());
        }

        protected override bool LoadData()
        {
            string filepath = Filepath();
            if (!File.Exists(filepath))
            {
                Models = new List<Card>().Default();
                if (!DataService.TryWrite<Card>(Models, out string error, filepath))
                {
                    OnFailure(error);
                }
            }
            bool b = DataService.TryRead<Card>(filepath, out List<Card> list, out string message);
            if (b)
            {
                Models = list;
            }
            else
            {
                OnFailure(message);
            }


            return b;
        }

        protected virtual void OnFailure(string message)
        {
            MessageBox.Show(message);
        }
    }
}
