using System.Collections.ObjectModel;
using System.Text;

namespace Bitsmith.FullText
{
    public class TokenCollection : KeyedCollection<string, Token>
    {
        protected override string GetKeyForItem(Token item)
        {
            return item.Value;
        }

        public override string ToString()
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            foreach (Token token in this)
            {
                if (i++ > 0)
                {
                    sb.Append(",");
                }
                sb.Append(token);
            }
            return sb.ToString();
        }


    }
}
