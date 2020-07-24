using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Styx
{
    public static class StyxExtensions
    {
        public static GraphDesigner Default(this GraphDesigner model)
        {
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.CreatedAt = DateTime.Now;
            model.Designs.Add(new GraphDesign().Default(model.Id));
            model.Templates.Add(new GraphTemplate().Default());
            return model;
        }

        public static GraphDesign Default(this GraphDesign model, string masterId, string display = "Display")
        {
            model.Id = Guid.NewGuid().ToString().ToLower();            
            model.CreatedAt = DateTime.Now;
            model.Display = display;
            model.Identifier = new TagIdentifier()
            {
                Id = model.Id,
                Display = display,
                Token = display.ToToken(),
                MasterId = masterId,
            };
            return model;
        }

        public static GraphTemplate Default(this GraphTemplate model)
        {
            model.CreatedAt = DateTime.Now;
            model.Id = Guid.NewGuid().ToString().ToLower();
            model.Description = "template desc";
            model.Display = "Default Template";
            return model;
        }


    }
}
