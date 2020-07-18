using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.EWSFirmwarePerformance
{
    [DataContract]
    public class EWSFirmwarePerformanceActivityData
    {
        /// <summary>
        /// Location of the folder containing firmware bundles
        /// </summary>
        [DataMember]
        public string FimBundlesLocation { get; set; }

        /// <summary>
        /// List of FirmwareData information
        /// </summary>
        [DataMember]
        public List<FirmwareData> FWBundleInfo { get; set; }
        /// <summary>
        /// Whether or not we validate FW again at execution
        /// </summary>
        [DataMember]
        public bool ValidateFWBundles { get; set; }
        /// <summary>
        /// Map of devices to their firmware bundle
        /// </summary>
        [DataMember]
        public Dictionary<string, ModelFileMap> AssetMapping { get; set; }

        /// <summary>
        /// Option to check whether to do an automatic backup/restore operation
        /// </summary>
        [DataMember]
        public bool AutoBackup { get; set; }
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


        public EWSFirmwarePerformanceActivityData()
        {
            FimBundlesLocation = string.Empty;
            FWBundleInfo = new List<FirmwareData>();
            AutoBackup = false;
            ValidateFlash = true;
            ValidateFWBundles = false;
            ValidateTimeOut = TimeSpan.FromMinutes(1);
            AssetMapping = new Dictionary<string, ModelFileMap>();
        }
    }

    [DataContract]
    public class ModelFileMap
    {
        /// <summary>
        /// Type of product an asset is
        /// </summary>
        [DataMember]
        public string ProductFamily { get; set; }
        /// <summary>
        /// Individual Firmware file for a product.
        /// </summary>
        [DataMember]
        public string FirmwareFile { get; set; }

        public ModelFileMap()
        {
            ProductFamily = string.Empty;
            FirmwareFile = string.Empty;
        }
    }



}
