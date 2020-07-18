using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    /// <summary>
    /// Contains data needed to execute a LowLevelConfig activity.
    /// </summary>
    [DataContract]
    public class LowLevelConfigActivityData
    {
        //Deprecated FW Info
        //[DataMember]
        //public bool EnableFW { get; set; }
        //[DataMember]
        //public string FimBundle { get; set; }

        /// <summary>
        /// This value control if the system will reset the JDI
        /// </summary>
        [DataMember]
        public string JDIMfgReset { get; set; }
        /// <summary>
        ///  This physical device serial number. Pulled from assetinventory
        /// </summary>
        [DataMember]
        public string SerialNumberBool { get; set; }
        /// <summary>
        ///  This physical device model number. Pulled from AssetInventory
        /// </summary>
        [DataMember]
        public string ModelNumberBool { get; set; }

        /// <summary>
        /// Sets execution mode for the device between Development and production
        /// </summary>
        [DataMember]
        public string ExecutionMode { get; set; }

        /// <summary>
        /// This value controls the Save/Recover behavior of the system.
        /// </summary>
        [DataMember]
        public string SaveRecoverFlags { get; set; }

        /// <summary>
        /// This value controls the behavior of the 49.XX.XX auto reboot feature when the system encounters a critical 49.XX.XX event.
        /// </summary>
        [DataMember]
        public string RebootCrashMode { get; set; }

        /// <summary>
        /// This value establishes the default sleep time of a product. The value is likely to change over time because of Energy Star and Blue Angel
        /// regulatory requirements. The underlying SPI NVRAM value will not change from the factory default if the User/Admin changes the current sleep time of the system.
        /// </summary>
        [DataMember]
        public string CRDefSleep { get; set; }

        /// <summary>
        /// This value is used to designate the default language of the system.
        /// </summary>
        [DataMember]
        public string Language { get; set; }

        /// <summary>
        ///  This value will be used on the production line along with Keno (VeryLowDealy)
        ///  to drive the test supplies to Very Low levels. This value must not be enabled in customer environments.
        /// </summary>
        [DataMember]
        public string SuppressUsed { get; set; }

        /// <summary>
        /// Indicates that we'll be conducting a partial clean on a device. Wiping settings, passwords, and extensibility partions.
        /// </summary>
        [DataMember]
        public string PartialClean { get; set; }


        /// <summary>
        /// Initializes a new instance of the LowLevelConfigActivityData class.
        /// </summary>
        public LowLevelConfigActivityData()
        {
            //EnableFW = false;
            //FimBundle = "";
            JDIMfgReset = "";
            SerialNumberBool = "";
            ModelNumberBool = "";
            ExecutionMode = "";
            SaveRecoverFlags = "";
            RebootCrashMode = "";
            CRDefSleep = "";
            Language = "";
            SuppressUsed = "";
            PartialClean = "";
        }
    }
}
