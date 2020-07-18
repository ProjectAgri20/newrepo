namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{
    public class DevicePackageInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Uuid { get; set; }
        public string installedFileName { get; set; }
        public DevicePackageInfo(string name, string version, string uuid, string fileName)
        {
            Name = name;
            Version = version;
            Uuid = uuid;
            installedFileName = fileName;
        }
    }
}
