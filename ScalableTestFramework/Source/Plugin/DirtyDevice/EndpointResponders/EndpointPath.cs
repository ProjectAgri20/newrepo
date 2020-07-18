using System.Collections.Generic;
using System.Linq;
using OXPd.Service.Scan;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class EndpointPath
    {
        public EndpointPath(DestinationType protocol, string endpointServerName, int? port, string externalFacingProtocolSpecificPath, string correspondingFileSystemPath)
        {
            Protocol = protocol;
            EndpointServerName = endpointServerName;
            Port = port;
            ExternalFacingProtocolSpecificPath = externalFacingProtocolSpecificPath;
            CorrespondingFileSystemPath = correspondingFileSystemPath;
        }

        public string EndpointServerName { get; set; }
        public int? Port { get; set; }
        public string EndpointServerIP => NetUtil.GetAddressIpv4(EndpointServerName).ToString();
        public DestinationType Protocol { get; set; }
        public string ExternalFacingProtocolSpecificPath { get; set; }
        public string CorrespondingFileSystemPath { get; set; }

        public string GetPathAsUrl()
        {
            var protocol = GetProtocolAsString(Protocol);

            string protocolDelimiterSlashes;
            switch (Protocol)
            {
                case DestinationType.NetworkFolder:
                    protocolDelimiterSlashes = "////"; // Don't know why, but HP devices require this
                    break;
                default:
                    protocolDelimiterSlashes = "//";
                    break;
            }

            string machineAddress = EndpointServerName;

            var port = (Port == null) ? string.Empty : $":{Port}";
            var suffix = string.Empty;
            switch (Protocol)
            {
                case DestinationType.Http:
                case DestinationType.Https:
                case DestinationType.NetworkFolder:
                    suffix = "/";
                    break;
            }

            string externalFacingProtocolSpecificPath = ExternalFacingProtocolSpecificPath.Replace(@"\", "/");

            return $"{protocol}:{protocolDelimiterSlashes}{machineAddress}{port}/{externalFacingProtocolSpecificPath}{suffix}";
        }

        public static string GetProtocolAsString(DestinationType protocol)
        {
            switch (protocol)
            {
                case DestinationType.LocalFolder:
                case DestinationType.NetworkFolder:
                    return "file";
                default:
                    return protocol.ToString().ToLower();
            }
        }

        public override string ToString()
        {
            var kv = new Dictionary<string, string>()
                {
                    { "GetPathAsUrl(FullyQualifiedDomainName)", GetPathAsUrl() },
                    { nameof(CorrespondingFileSystemPath), CorrespondingFileSystemPath },
                    { nameof(EndpointServerName), EndpointServerName },
                    { nameof(EndpointServerIP), EndpointServerIP },
                    { nameof(ExternalFacingProtocolSpecificPath), ExternalFacingProtocolSpecificPath },
                    { nameof(Protocol), Protocol.ToString() },
                };
            var parameters = string.Join("; ", kv.Select(kvp => $"{kvp.Key}: '{kvp.Value}'"));

            return parameters;
        }
    }
}
