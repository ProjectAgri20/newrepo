using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A server used by a test session.
    /// </summary>
    [DebuggerDisplay("{HostName,nq}")]
    public sealed class SessionServer
    {
        /// <summary>
        /// Gets or sets the unique identifier for this session server record.
        /// </summary>
        public Guid SessionServerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that used the server.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the server that was used.
        /// </summary>
        public Guid ServerId { get; set; }

        /// <summary>
        /// Gets or sets the server host name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the server IP address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the server operating system.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the server architecture.
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Gets or sets the number of processors on the server.
        /// </summary>
        public short? Processors { get; set; }

        /// <summary>
        /// Gets or sets the number of cores per processor on the server.
        /// </summary>
        public short? Cores { get; set; }

        /// <summary>
        /// Gets or sets the amount of memory on the server, in megabytes.
        /// </summary>
        public int? Memory { get; set; }
    }
}
