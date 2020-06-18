using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.ViewModels
{
    public class CredentialViewModel : ViewModel<ContentItem>
    {
        public bool IsRemove
        {
            get
            {
                return Model.IsRemove;
            }
            set
            {
                Model.IsRemove = value;
                OnPropertyChanged("IsRemove");
            }
        }

        private string _Location;
        public string Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
                OnPropertyChanged("Location");
                AdaptOut();
            }
        }

        private string _Identifier;
        public string Identifier
        {
            get
            {
                return _Identifier;
            }
            set
            {
                _Identifier = value;
                OnPropertyChanged("Identifier");
                AdaptOut();
            }
        }


        private string _Secret;
        public string Secret
        {
            get
            {
                return _Secret;
            }
            set
            {
                _Secret = value;
                OnPropertyChanged("Secret");
                AdaptOut();
            }
        }


        private string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
                OnPropertyChanged("Note");
                AdaptOut();
            }
        }






        public CredentialViewModel(ContentItem model)
        {
            Model = model;
            AdaptIn(model);
        }

        private void AdaptIn(ContentItem model)
        {
            if (!string.IsNullOrWhiteSpace(model.Body))
            {
                var unencrypted = Encryptor.Decrypt(model.Body);
                foreach (var line in unencrypted.Split(new char[] {'\r','\n','\t' },StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] parts = line.Trim().Split(new char[] { '`','~' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim().ToLower();
                        string val = parts[1].Trim();
                        if (key.Equals("secret"))
                        {
                            _Secret = val;
                        }
                        else if(key.Equals("identifier"))
                        {
                            _Identifier = val;
                        }
                        else if(key.Equals("location"))
                        {
                            _Location = val;
                        }
                        else if(key.Equals("note"))
                        {
                            _Note = val;
                        }
                    }
                }
            }
        }
        private void AdaptOut()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_Location))
            {
                sb.AppendLine($"location~{ _Location}");
            }
            if (!string.IsNullOrWhiteSpace(_Identifier))
            {
                sb.AppendLine($"identifier~{ _Identifier}");
            }
            if (!string.IsNullOrWhiteSpace(_Secret))
            {
                sb.AppendLine($"secret~{ _Secret}");
            }
            if (!string.IsNullOrWhiteSpace(_Note))
            {
                sb.AppendLine($"note~{ _Note}");
            }

            Model.Body = Encryptor.Encrypt(sb.ToString());
        }
    }
}
