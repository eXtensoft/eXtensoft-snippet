using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith
{
    [Serializable]
    public class State
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        #region Display (string)
        private string _Display;

        /// <summary>
        /// Gets or sets the string value for Display
        /// </summary>
        /// <value> The string value.</value>
        [XmlAttribute("display")]
        public string Display
        {
            get { return (String.IsNullOrEmpty(_Display)) ? Name : _Display; }
            set
            {
                if (_Display != value)
                {
                    _Display = value;
                }
            }
        }
        #endregion

        [XmlAttribute("isNav")]
        public bool IsNavigate { get; set; } = false;

        [XmlAttribute("order")]
        public int SortOrder { get; set; } = 0;

        [XmlIgnore]
        public List<IEndpointAction> EndpointActions { get; set; }

        //[XmlIgnore]
        //public List<EndpointAction> Items
        //{
        //    get
        //    {
                
        //        if (EndpointActions != null)
        //        {
        //            List<EndpointAction> list = new List<EndpointAction>();
        //            foreach (var item in EndpointActions)
        //            {
        //                if (item is EndpointAction)
        //                {
        //                    list.Add((EndpointAction)item);
        //                }
        //            }
        //            return list;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            EndpointActions = new List<IEndpointAction>();
        //            foreach (var item in value)
        //            {
        //                if (item is EndpointAction)
        //                {
        //                    EndpointActions.Add((IEndpointAction)item);
        //                }
        //            }
        //        }
        //    }
        //}
    
    }
}
