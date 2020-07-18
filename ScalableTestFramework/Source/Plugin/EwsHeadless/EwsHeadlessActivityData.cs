using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Contains data needed to execute the activity.
    /// </summary>
    [DataContract]
    public class EwsHeadlessActivityData
    {
        /// <summary>
        /// Main Operation that can be performed on all devices
        /// </summary>
        [DataMember]
        public string Operation { get; set; }

        /// <summary>
        /// Operation performed specific to the particular device
        /// </summary>
        [DataMember]
        public string DeviceSpecificOperation { get; set; }

        /// <summary>
        /// Values specific to the device operation stored dynamically
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ConfigurationValues { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EwsHeadlessActivityData()
        {
            Operation = string.Empty;
            ConfigurationValues = new Dictionary<string, string>();
            //DeviceType = DeviceType.NONE;
        }
    }

    /// <summary>
    /// Supported Device types
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Sirius Device
        /// </summary>
        Sirius,
        /// <summary>
        /// Phoenix device (Low end Laser)
        /// </summary>
        Phoenix,
        /// <summary>
        /// OZ (Legacy VEP)
        /// </summary>
        Oz,
        /// <summary>
        /// Jedi firmware (VEP)
        /// </summary>
        Jedi,
        /// <summary>
        /// Omni UI (VEP)
        /// </summary>
        Omni
    }
}