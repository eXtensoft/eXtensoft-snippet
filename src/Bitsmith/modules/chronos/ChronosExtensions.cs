using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Models
{
    public static class ChronosExtensions
    {
        public static Chronos Default(this Chronos model)
        {

            return model;
        }

        public static List<TagIdentifier> Activities(this List<TagIdentifier> list)
        {
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Planning", Token = "planning" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Design", Token = "Design" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Code", Token = "code" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Troubleshoot", Token = "troubleshoot" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Refactor", Token = "refactor" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Unit Test", Token = "unit-test" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Testing", Token = "testing" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Collaborate", Token = "collaborate" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Deploy", Token = "deploy" });
            list.Add(new TagIdentifier() { Id = Guid.NewGuid().ToString().ToLower(), Display = "Sleep", Token = "sleep" });
            return list;
        }
    }
}
