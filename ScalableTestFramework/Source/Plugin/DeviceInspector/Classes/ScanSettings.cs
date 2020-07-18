using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceInspector.Classes
{
    [DataContract]
    public class ScanSettings
    {
        [DataMember]
        public DataPair<string> OriginalSize { get; set; }

        [DataMember]
        public DataPair<string> OriginalSides { get; set; }

        [DataMember]
        public DataPair<string> ContentOrientation { get; set; }

        [DataMember]
        public DataPair<string> ImagePreview { get; set; }

        [DataMember]
        public DataPair<string> Optimize { get; set; }

        [DataMember]
        public DataPair<string> Sharpness { get; set; }

        [DataMember]
        public DataPair<string> Cleanup { get; set; }

        [DataMember]
        public DataPair<string> Darkness { get; set; }

        [DataMember]
        public DataPair<string> Contrast { get; set; }

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