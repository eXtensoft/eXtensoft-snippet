using Bitsmith.NaturalLanguage;

namespace Bitsmith.ViewModels
{
    public class LanguageTokenViewModel : ViewModel<Token>
    {

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

        public string Content
        {
            get
            {
                return Model.Content;
            }
            set
            {
                Model.Content = value;
                OnPropertyChanged("Content");
            }
        }

        public TokenTypeOption TokenType
        {
            get
            {
                return Model.Type;
            }
            set
            {
                Model.Type = value;
                OnPropertyChanged("TokenType");
            }
        }

        public LanguageTokenViewModel(Token model)
        {
            Model = model;
        }

    }
}
