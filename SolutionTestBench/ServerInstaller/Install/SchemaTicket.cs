using System;
using System.Net;

namespace HP.SolutionTest.Install
{
    internal class SchemaTicket
    {
        public string AdminUserName { get; set; }

        public string AdminDomain { get; set; }

        public string AdminEmail { get; set; }

        public string DatabaseFilesPath { get; set; }

        public string FileSharePath { get; set; }

        public string FileShareName { get; set; }

        public string OrganizationName { get; set; }

        public string ServerHostname { get; set; }

        public Version CurrentVersion { get; set; }

        public BuildConfiguration BuildConfig { get; set; }

        public bool IsNewInstall
        {
            get { return CurrentVersion == new Version(0, 0, 0); }
        }

        public SchemaTicket()
        {
            CurrentVersion = new Version(0, 0, 0);
            ServerHostname = Dns.GetHostEntry("").HostName;
            FileShareName = "STBShare";
            BuildConfig = BuildConfiguration.Base;
        }

        public void Log()
        {
            SystemTrace.Instance.Debug($"Admin User: {AdminUserName}");
            SystemTrace.Instance.Debug($"Admin Domain: {AdminDomain}");
            SystemTrace.Instance.Debug($"Admin Email: {AdminEmail}");
            SystemTrace.Instance.Debug($"Organization: {OrganizationName}");
            SystemTrace.Instance.Debug($"Data Files Path: {DatabaseFilesPath}");
            SystemTrace.Instance.Debug($"File Share: {FileSharePath}");
            SystemTrace.Instance.Debug($"Server Hostname: {ServerHostname}");
            SystemTrace.Instance.Debug($"Current Version: {CurrentVersion}");
            SystemTrace.Instance.Debug($"Build Target: {BuildConfig.ToString()}");
        }
    }

    /// <summary>
    /// Build Configuration Enum.
    /// The intention here is to not have to add an enum for EVERY solution that may come along.
    /// If it's not Base or All Plugins, knowing it's a solution is granular enough for now.
    /// </summary>
    public enum BuildConfiguration
    {
        Base,
        AllPlugins,
        Solution
    }
}
