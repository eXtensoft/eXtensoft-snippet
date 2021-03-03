using Bitsmith.Models;

namespace Bitsmith.Parsing
{
    public abstract class Parser : IParser
    {
        public abstract string Extension { get; }
        string IParser.Extensions => Extension;

        void IParser.Parse(TabularData tabularData)
        {
           Parse(tabularData);
        }

        public virtual void Parse(TabularData tabularData)
        {
            tabularData.IsOkay = false;
            tabularData.Message = "Parsing not implemented.";
        }
    }
}
