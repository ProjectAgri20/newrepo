using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A framework server tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{HostName,nq}")]
    public class FrameworkServer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the server.
        /// </summary>
        public Guid FrameworkServerId { get; set; }

        /// <summary>
        /// Gets or sets the server host name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the server IP address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the server operating system.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the server processor architecture (i.e. x86, x64, etc.).
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Gets or sets the number of processors on the server.
        /// </summary>
        public int Processors { get; set; }

        /// <summary>
        /// Gets or sets the number of cores per processor on the server.
        /// </summary>
        public int Cores { get; set; }

        /// <summary>
        /// Gets or sets the amount of memory on the server, in megabytes.
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// Gets or sets information about the disk space available on the server.
        /// </summary>
        public string DiskSpace { get; set; }

        /// <summary>
        /// Gets or sets the status of the server.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the server description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the person who is the main point of contact for the server.
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the STF environment that this server is connected to. 
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the version of services running on this server.
        /// </summary>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Gets or sets the version of STF code running on this server.
        /// </summary>
        public string StfServiceVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FrameworkServer" /> is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the collection of server types associated with this server.
        /// </summary>
        public virtual ICollection<FrameworkServerType> ServerTypes { get; set; } = new HashSet<FrameworkServerType>();

        /// <summary>
        /// Gets or sets the collection of settings associated with this server.
        /// </summary>
        public virtual ICollection<ServerSetting> ServerSettings { get; set; } = new HashSet<ServerSetting>();
    }
}
