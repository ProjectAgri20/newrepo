using System;
using OXPd.Service.Scan;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class PathManager
    {
        public EndpointPath FileSharePath { get; set; }
        public EndpointPath FtpPath { get; set; }
        public EndpointPath HttpPath { get; set; }

        public PathManager(EndpointPath fileSharePath, EndpointPath ftpPath, EndpointPath httpPath)
        {
            FileSharePath = fileSharePath;
            FtpPath = ftpPath;
            HttpPath = httpPath;
        }

        public EndpointPath GetEndpointPath(DestinationType protocol)
        {
            switch (protocol)
            {
                case DestinationType.NetworkFolder:
                case DestinationType.LocalFolder:
                    return FileSharePath;
                case DestinationType.Ftp:
                    return FtpPath;
                case DestinationType.Http:
                case DestinationType.Https:
                    return HttpPath;
                default:
                    throw new NotSupportedException($"Protocol '{protocol}' not supported.");
            }
        }

        public override string ToString()
        {
            return $"File: {FileSharePath}; Ftp: {FtpPath}; Http: {HttpPath}";
        }
    }
}
