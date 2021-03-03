using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class SchemaViewModel : ViewModel<Schema>
    {
        public string Id
        {
            get
            {
                return Model.Identifier.Id;
            }
            set
            {
                Model.Id = value;
                Model.Identifier.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Display
        {
            get
            {
                return Model.Identifier.Display;
            }
            set
            {
                Model.Identifier.Display = value;
                OnPropertyChanged("Display");
            }
        }

        public string Token
        {
            get
            {
                return Model.Identifier.Token;
            }
            set
            {
                Model.Identifier.Token = value;
                OnPropertyChanged("Token");
            }
        }

        public string MasterId
        {
            get
            {
                return Model.Identifier.MasterId;
            }
            set
            {
                Model.Identifier.MasterId = value;
                OnPropertyChanged("MasterId");
            }
        }

        public string SchemaText
        {
            get
            {
                return Model.SchemaText;
            }
            set
            {
                Model.SchemaText = value;
                OnPropertyChanged("SchemaText");
            }
        }


        public SchemaViewModel(Schema model)
        {
            Model = model;
        }


    }
}
