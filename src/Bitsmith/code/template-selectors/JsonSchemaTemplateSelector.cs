using Bitsmith.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bitsmith
{
    public class JsonSchemaTemplateSelector : DataTemplateSelector
    {
        public string SchemaTemplateName { get; set; } = "dynamic";
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
           string templatename = $"{SchemaTemplateName}SchemaDataTemplate";
            DataTemplate template = null;
            var vm = item as SchemaBuilderViewModel;
            if (vm != null)
            {                
                string name = $"{vm.Token}SchemaDataTemplate";
                if (Application.Current.Resources.Contains(name))
                {
                    template = Application.Current.Resources[name] as DataTemplate;
                }
                else if(Application.Current.Resources.Contains(templatename))
                {
                    template = Application.Current.Resources[templatename] as DataTemplate;
                }
            }

            return template;
        }
    }
}
