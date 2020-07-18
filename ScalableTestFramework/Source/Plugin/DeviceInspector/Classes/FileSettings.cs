namespace HP.ScalableTest.Plugin.DeviceInspector.Classes
{
    public class FileSettings
    {
        public DataPair<string> FileName { get; set; }
        public DataPair<string> FileNamePrefix { get; set; }

        public DataPair<string> FileNameSuffix { get; set; }
        public DataPair<string> FileType { get; set; }

        public DataPair<string> Resolution { get; set; }

        public DataPair<string> FileSize { get; set; }

        public DataPair<string> FileColor { get; set; }

        public DataPair<string> FileNumbering { get; set; }

        public FileSettings()
        {
            FileName = new DataPair<string> {Key = string.Empty};
            FileNamePrefix = new DataPair<string> { Key = string.Empty };
            FileNameSuffix = new DataPair<string> { Key = string.Empty };
            FileType = new DataPair<string> { Key = string.Empty };
            Resolution = new DataPair<string> { Key = string.Empty };
            FileSize = new DataPair<string> { Key = string.Empty };
            FileColor = new DataPair<string> { Key = string.Empty };
            FileNumbering = new DataPair<string> { Key = string.Empty };
        }
    }
}
