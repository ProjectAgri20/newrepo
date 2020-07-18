using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    /// <summary>
    /// Contains data needed to execute a DeviceConfiguration activity.
    /// </summary>
    [DataContract]
    internal class DeviceConfigurationActivityData
    {
        /// <summary>
        /// Bool Value of whether or not to enable the default password on the device from asset inventory
        /// </summary>
        [DataMember]
        public bool EnableDefaultPW { get; set; }
        /// <summary>
        /// Sets the maximum length password for the device via webservice ticket
        /// </summary>
        [DataMember]
        public int MaxPWLength { get; set; }

        /// <summary>
        /// Sets a list of domains to validate credentials against for users
        /// </summary>
        [DataMember]
        public List<string> WindowsDomains { get; set; }

        /// <summary>
        /// Sets the default domain from the list of windows domains
        /// </summary>
        [DataMember]
        public string DefaultDomain { get; set; }

        /// <summary>
        /// Boolean value to determine whether to use advanced password complexity requirements
        /// </summary>
        [DataMember]
        public bool EnablePasswordComplexity { get; set; }

        /// <summary>
        /// Enables windows Authentication the the device
        /// </summary>
        [DataMember]
        public bool EnableWindowsAuthentication { get; set; }

        /// <summary>
        /// Collection of public components that determines which additional settings will be set by the plugin.
        /// </summary>
        [DataMember]
        public Collection<IComponentData> ComponentData { get; set; }

        //[DataMember]
        //public List<Type> ModifiedControls { get; set; }

        //Add an interface for data so we could just have a list of data to go over


        /// <summary>
        /// Initializes a new instance of the DeviceConfigurationActivityData class.
        /// </summary>
        public DeviceConfigurationActivityData()
        {
            WindowsDomains = new List<string>();
            //ModifiedControls = new List<Type>();
            ComponentData = new Collection<IComponentData>();
        }
    }
}
