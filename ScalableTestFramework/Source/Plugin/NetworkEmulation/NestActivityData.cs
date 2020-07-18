using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    /// <summary>
    /// NEST Activity Data
    /// </summary>
    [DataContract]
    public class NestActivityData
    {
        /// <summary>
        /// The emulation data which will be used by the driver
        /// </summary>
        [DataMember]
        public string EmulationString { get; set; }

        /// <summary>
        /// the emulation profile name
        /// </summary>
        [DataMember]
        public string EmulationProfileName { get; set; }

        public NestActivityData()
        {
            EmulationString = string.Empty;
        }
    }
}