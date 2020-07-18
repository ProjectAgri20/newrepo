using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Class that supports system informtion for Windows systems
    /// </summary>
    public class WindowsSystemInfo
    {
        /// <summary>
        /// A string representing an x64 architecture.
        /// </summary>
        public const string ArchitectureX64 = "x64";

        /// <summary>
        /// A string representing an x86 architecture.
        /// </summary>
        public const string ArchitectureX86 = "x86";

        /// <summary>
        /// Gets or sets the architecture.
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Gets or sets the cores.
        /// </summary>
        public int Cores { get; set; }

        /// <summary>
        /// Gets or sets the disk space.
        /// </summary>
        public string DiskSpace { get; set; }

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the memory.
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// Gets or sets the operating system.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the processors.
        /// </summary>
        public int Processors { get; set; }

        /// <summary>
        /// Gets or sets the service pack.
        /// </summary>
        public int ServicePack { get; set; }

        /// <summary>
        /// Gets or sets the revision.
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Gets or sets the ip addresses.
        /// </summary>
        /// <value>The ip addresses.</value>
        public Collection<string> IpAddresses { get; set; }
    }
}
