using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Bitsmith
{
    [Serializable]
    public class TypedItem
    {
        #region properties
        [XmlAttribute("group")]
        public string Group {get;set;}

        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlElement]
        public object Value { get; set; }

        [XmlAttribute("effective")]
        public DateTime Effective { get; set; }

        [XmlIgnore]
        public string Datatype
        {
            get { return Value.GetType().FullName; }
        }

        #endregion

        #region constructors

        public TypedItem() { }

        public TypedItem(string key, int value)
        {
            Key = key;
            Value = value;
            Effective = DateTime.Now;
        }

        public TypedItem(string key, string value)
        {
            Key = key;
            Value = value;
            Effective = DateTime.Now;
        }
        public TypedItem(string key, object value)
        {
            Key = key;
            Value = value;
            Effective = DateTime.Now;
        }

        #endregion

    }
}

