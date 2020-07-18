using System.Collections.Generic;
using System.Runtime.Serialization;
namespace HP.ScalableTest.Plugin.ServiceStartStop
{
    /// <summary>
    /// Contains data needed to execute a ServiceStartStop activity.
    /// </summary>
    [DataContract]
    internal class ServiceStartStopActivityData
    {
        /// <summary>
        /// Initializes a new instance of the ServiceStartStopActivityData class.
        /// </summary>
        public ServiceStartStopActivityData()
        { }

        /// <summary>
        /// Gets or sets the task to execute
        /// </summary>
        [DataMember]
        [System.ComponentModel.DefaultValue(0)]

        public int task { get; set; }

        /// <summary>
        /// List of selected services to execute task on
        /// </summary>
        [DataMember]
        public List<string> services { get; set; }
        [DataMember]
        public Framework.Assets.ServerInfo serv { get; set; }
        [DataMember]
        public List<int> serviceIDs { get; set; }
    }
}
