using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{
    /// <summary>
    /// Scan Settings for job/quickset configuration.
    /// </summary>
    [DataContract]
    public class ScanSettings
    {
        /// <summary>
        /// File Original Size Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> OriginalSize { get; set; }
        /// <summary>
        /// File Original Side Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> OriginalSides { get; set; }
        /// <summary>
        /// File Orientation Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> ContentOrientation { get; set; }
        /// <summary>
        /// Image Preview Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> ImagePreview { get; set; }
        /// <summary>
        /// Optimization Choice Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> Optimize { get; set; }
        /// <summary>
        /// Scan Sharpness Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> Sharpness { get; set; }
        /// <summary>
        /// Scan Cleanup Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> Cleanup { get; set; }
        /// <summary>
        /// Scan Darkness Setting
        /// </summary>
        [DataMember]
        public DataPair<string> Darkness { get; set; }
        /// <summary>
        /// Scan Contrast Setting.
        /// </summary>
        [DataMember]
        public DataPair<string> Contrast { get; set; }
        /// <summary>
        /// Creates instance of ScanSettings Class.
        /// </summary>
        public ScanSettings()
        {
            OriginalSize = new DataPair<string> { Key = string.Empty };
            OriginalSides = new DataPair<string> { Key = string.Empty };
            ContentOrientation = new DataPair<string> { Key = string.Empty };
            ImagePreview = new DataPair<string> { Key = string.Empty };

            Optimize = new DataPair<string> { Key = string.Empty };
            Sharpness = new DataPair<string> { Key = string.Empty };
            Cleanup = new DataPair<string> { Key = string.Empty };
            Darkness = new DataPair<string> { Key = string.Empty };
            Contrast = new DataPair<string> { Key = string.Empty };
        }

        public ScanSettings(string size, string sides, string orientation, string preview)
        {
            OriginalSize = new DataPair<string> { Key = size, Value = true };
            OriginalSides = new DataPair<string> { Key = sides, Value = true };
            ContentOrientation = new DataPair<string> { Key = orientation, Value = true };
            ImagePreview = new DataPair<string> { Key = preview, Value = true };
        }
    }
}