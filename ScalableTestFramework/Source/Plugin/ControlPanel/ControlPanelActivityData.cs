using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    /// <summary>
    /// Contains data needed to execute a Control Panel Configuration activity.
    /// </summary>

    [DataContract]
    public class ControlPanelActivityData
    {
        #region Properties

        /// <summary>
        /// Control Panel Action
        /// </summary>
        [DataMember]
        public string ControlPanelAction { get; set; }

        /// <summary>
        /// Name value pair which are passed to the control panel feature intent module
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ParameterValues { get; set; }

        [DataMember]
        public string ControlPanelType { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPanelActivityData"/> class.
        /// </summary>
        public ControlPanelActivityData()
        {
            ControlPanelAction = string.Empty;

            ParameterValues = new Dictionary<string, string>();
        }

        #endregion Constructor
    }
}