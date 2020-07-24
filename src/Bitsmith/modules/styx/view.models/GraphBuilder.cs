using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace Bitsmith.Styx
{
    public class GraphBuilder : INotifyPropertyChanged
    {
        public Graph Graph { get; set; }
        public NavGraph<Node, Link> NavGraph { get; set; }

        private string _Input;
        public string Input
        {
            get
            {
                return _Input;
            }
            set
            {
                _Input = value;
                OnPropertyChanged("Input");
            }
        }
        private string _Output;
        public string Output
        {
            get { return _Output; }
            set
            {
                _Output = value;
                OnPropertyChanged("Output");
            }
        }


        private ICommand _ParseGraphCommand;
        public ICommand ParseGraphCommand
        {
            get
            {
                if (_ParseGraphCommand == null)
                {
                    _ParseGraphCommand = new RelayCommand(
                    param => ParseGraph(),
                    param => CanParseGraph());
                }
                return _ParseGraphCommand;
            }
        }
        private bool CanParseGraph()
        {
            return true;
        }
        private void ParseGraph()
        {
            if (!string.IsNullOrWhiteSpace(Input))
            {
                if (GraphBuilder<Node, Link>.TryParse(Input,
                    out NavGraph<Node, Link> navgraph,
                    out string message, (s) => { return new Node(s); }))
                {
                    NavGraph = navgraph;
                    Graph = NavGraph.ToGraph<Node, Link>();
                    Output = JsonConvert.SerializeObject(Graph, Newtonsoft.Json.Formatting.Indented);

                    //var options = new JsonSerializerOptions() { };
                    //options.WriteIndented = true;
                    //Output = JsonSerializer.Serialize<Graph>(Site, options);
                    
                    //GraphViewModel gvm = new GraphViewModel(Site);
                    //Items.Add(gvm.Root);
                }
            }
        }












        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
