using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bitsmith
{
    [Serializable]
    public class EndpointAction : IEndpointAction
    {
        #region Actions (List<Action>)

        private List<Action> _Actions;

        /// <summary>
        /// Gets or sets the List<Action> value for Actions
        /// </summary>
        /// <value> The List<Action> value.</value>
        [XmlIgnore]
        public List<Action> Actions
        {
            get { return _Actions; }
            set
            {
                if (_Actions != value)
                {
                    _Actions = value;
                }
            }
        }

        #endregion

        public EndpointAction(EndpointOption endpointOption, params Action[] actions)
        {
            _EndpointOption = endpointOption;
            _Actions = new List<Action>(actions);
        }
        public EndpointAction() { }

        #region IEndpointAction Members
        private EndpointOption _EndpointOption;
        EndpointOption IEndpointAction.Endpoint
        {
            get
            {
                return _EndpointOption; ;
            }
            set
            {
                _EndpointOption = value;
            }
        }

        void IEndpointAction.Execute()
        {
            if (Actions != null)
            {
                foreach (var item in Actions)
                {
                    item.Invoke();
                }
            }
        }
        #endregion
    }
}
