using Bitsmith.Models;
using Bitsmith.Schemas;

namespace Bitsmith.ViewModels
{
    public class SchemaBuilderViewModel : ViewModel<Schema>
    {

        private bool _IsSelectedContentSchema;
        public bool IsSelectedContentSchema
        {
            get
            {
                return _IsSelectedContentSchema;
            }
            set
            {
                _IsSelectedContentSchema = value;
                OnPropertyChanged("IsSelectedContentSchema");
            }
        }


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

        public SchemaBuilderViewModel(Schema model = null)
        {
            if (model == null)
            {
                model = new Schema().Default();
            }
            Model = model;
        }

    }
}
