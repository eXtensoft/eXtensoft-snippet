using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.Schemas
{
    public static class SchemaExtensions
    {
        public static Schema Default(this Schema model, string masterId = "")
        {
            model.Id = Guid.NewGuid().ToString().ToLower();
            var masterid = !string.IsNullOrWhiteSpace(masterId) ? masterId : model.Id;
            model.Identifier = new TagIdentifier() { Id = model.Id, MasterId = masterid };
            return model;
        }

        public static List<Schema> Default(this List<Schema> list )
        {
            Schema person = new Schema().Default();
            person.SchemaText = JsonSchemas.schemas_person_json;
            person.Identifier.Display = "Person";
            person.Identifier.Token = "person";
            list.Add(person);
            Schema citation = new Schema().Default();
            citation.SchemaText = JsonSchemas.schemas_citation_json;
            citation.Identifier.Display = "Citation";
            citation.Identifier.Token = "citation";
            list.Add(citation);
            Schema contact = new Schema().Default();
            contact.SchemaText = JsonSchemas.schemas_contact_json;
            contact.Identifier.Display = "Contact";
            contact.Identifier.Token = "contact";
            list.Add(contact);
            return list;
        }

        public static TabularData Load(this TabularData model, string content)
        {

            return model;
        }
    }
}
