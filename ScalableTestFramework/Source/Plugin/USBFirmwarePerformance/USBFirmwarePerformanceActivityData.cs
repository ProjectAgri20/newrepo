using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.USBFirmwarePerformance
{
    [DataContract]
    public class USBFirmwarePerformanceActivityData
    {

        /// <summary>
        /// Whether or not to validate success of FW update, not valid for downgrades
        /// </summary>
        [DataMember]
        public bool ValidateFlash { get; set; }

        /// <summary>
        /// Timeout under which to validate the update, experimental
        /// </summary>
        [DataMember]
        public TimeSpan ValidateTimeOut { get; set; }


        public USBFirmwarePerformanceActivityData()
        {
            ValidateFlash = true;
            ValidateTimeOut = TimeSpan.FromMinutes(1);
        }

        

    }
}
