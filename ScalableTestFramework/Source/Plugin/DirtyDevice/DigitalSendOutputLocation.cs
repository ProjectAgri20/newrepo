using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    [DataContract]
    public class DigitalSendOutputLocation
    {
        [DataMember]
        public string ServerHostName { get; set; }

        [DataMember]
        public string MonitorLocation { get; set; }

        public DigitalSendOutputLocation(string location)
        {
            int hostNameLength = location.Substring(2).IndexOf("\\");
            ServerHostName = location.Substring(2, hostNameLength);
            MonitorLocation = location.Substring(hostNameLength + 3);
        }

        public string ToShortUncPath()
        {
            if (MonitorLocation.Contains(":"))
            {
                throw new ArgumentException($"Cannot convert drive letter path to UNC. ({MonitorLocation})");
            }

            return $@"\\{ServerHostName}\{MonitorLocation}";
        }

        public string ToFQDNUncPath(PluginEnvironment environment)
        {
            if (MonitorLocation.Contains(":"))
            {
                throw new ArgumentException($"Cannot convert drive letter path to UNC. ({MonitorLocation})");
            }

            return $@"\\{ServerHostName}.{environment.UserDnsDomain}\{MonitorLocation}";
        }
    }
}
