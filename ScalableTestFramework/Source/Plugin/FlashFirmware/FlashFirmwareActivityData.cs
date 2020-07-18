using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.FlashFirmware
{
    /// <summary>
    /// Contains data needed to execute a FlashFirmware activity.
    /// </summary>
    [DataContract]
    public class FlashFirmwareActivityData
    {
        /// <summary>
        /// The firmware file to flash
        /// </summary>

        [DataMember]
        public string FirmwareFileName { get; set; }

        /// <summary>
        /// option to check whether to do an automatic backup/restore operation
        /// </summary>
        [DataMember]
        public bool AutoBackup { get; set; }

        [DataMember]
        public bool ValidateFlash { get; set; }

        [IgnoreDataMember]
        public string FirmwareVersion { get; set; }

        [IgnoreDataMember]
        public string FirmwareRevision { get; set; }

        [IgnoreDataMember]
        public string FirmwareDateCode { get; set; }

        [DataMember]
        public TimeSpan ValidateTimeOut { get; set; }

        [DataMember]
        public FlashMethod FlashMethod { get; set; }

        [DataMember]
        public string ComPort { get; set; }

        [DataMember]
        public bool IsDowngrade { get; set; }

        /// <summary>
        /// Initializes a new instance of the FlashFirmwareActivityData class.
        /// </summary>
        public FlashFirmwareActivityData()
        {
            ValidateTimeOut = TimeSpan.FromMinutes(1);
            FlashMethod = FlashMethod.Ews;
        }
    }

    public enum FlashMethod
    {
        Ews,
        ControlPanel,
        Bios
    }
}