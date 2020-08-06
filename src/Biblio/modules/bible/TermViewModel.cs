using Biblio.Modules.Bible.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.ViewModels
{
    public class TermViewModel: ViewModel<Verse>
    {
        public string Id { get { return Model.Id; } }
        public string Term { get; set; }
        public string Version { get; set; }
        private string _Display;
        public string Display
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Display))
                {
                    _Display = $"{Book} ch{Chapter}v{Verse}";
                }
                return _Display;
            }
            set { _Display = value; }
        }
        public string Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get { return Model.Index; } }
        public string Text { get; set; }
        public TermViewModel(Verse model)
        {
            Model = model;            
        }

        public void Set(List<string> terms, string version)
        {
            Term = string.Join(",",terms);
            Text = Model.Text.FirstOrDefault(x => x.VersionId.Equals(version)).Text;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
