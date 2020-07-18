using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceInspector
{
    /// <summary>
    /// Contains data needed to execute a DeviceConfiguration activity.
    /// </summary>
    [DataContract]
    internal class DeviceInspectorActivityData
    {
        [DataMember]
        public bool EnableDefaultPW { get; set; }
        [DataMember]
        public int MaxPWLength { get; set; }
        [DataMember]
        public List<string> WindowsDomains { get; set; }
        [DataMember]
        public string DefaultDomain { get; set; }
        [DataMember]
        public bool EnablePasswordComplexity { get; set; }
        [DataMember]
        public bool EnableWindowsAuthentication { get; set; }
        [DataMember]
        public Collection<IComponentData> ComponentData { get; set; }

        //[DataMember]
        //public List<Type> ModifiedControls { get; set; }

        //Add an interface for data so we could just have a list of data to go over


        /// <summary>
        /// Initializes a new instance of the DeviceConfigurationActivityData class.
        /// </summary>
        public DeviceInspectorActivityData()
        {
            WindowsDomains = new List<string>();
            //ModifiedControls = new List<Type>();
            ComponentData = new Collection<IComponentData>();
        }
    }
}
