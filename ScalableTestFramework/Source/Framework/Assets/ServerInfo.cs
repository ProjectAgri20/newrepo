using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a server used in a test.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{HostName,nq}")]
    public class ServerInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _serverId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _hostName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _operatingSystem;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _architecture;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _processors;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _cores;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _memory;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SettingsDictionary _settings;

        /// <summary>
        /// Gets the unique identifier for the server.
        /// </summary>
        public Guid ServerId => _serverId;

        /// <summary>
        /// Gets the server host name.
        /// </summary>
        public string HostName => _hostName;

        /// <summary>
        /// Gets the server IP address.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets the server operating system.
        /// </summary>
        public string OperatingSystem => _operatingSystem;

        /// <summary>
        /// Gets the server processor architecture (i.e. x86, x64, etc.).
        /// </summary>
        public string Architecture => _architecture;

        /// <summary>
        /// Gets the number of processors on the server.
        /// </summary>
        public int Processors => _processors;

        /// <summary>
        /// Gets the number of cores per processor on the server.
        /// </summary>
        public int Cores => _cores;

        /// <summary>
        /// Gets the amount of memory on the server, in megabytes.
        /// </summary>
        public int Memory => _memory;

        /// <summary>
        /// Gets the collection of settings for the server.
        /// </summary>
        public SettingsDictionary Settings => _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerInfo"/> class.
        /// </summary>
        /// <param name="serverId">The server identifier.</param>
        /// <param name="hostName">The server host name.</param>
        /// <param name="address">The server IP address.</param>
        /// <param name="operatingSystem">The server operating system.</param>
        /// <param name="architecture">The processor architecture.</param>
        /// <param name="processors">The number of processors.</param>
        /// <param name="cores">The number of cores.</param>
        /// <param name="memory">The amount of memory, in megabytes.</param>
        /// <param name="settings">The ServerSettings</param>
        /// <exception cref="ArgumentNullException"><paramref name="hostName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="hostName" /> is an empty string.</exception>
        public ServerInfo(Guid serverId, string hostName, string address, string operatingSystem, string architecture, int processors, int cores, int memory, IDictionary<string, string> settings)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException("Host name cannot be an empty string.", nameof(hostName));
            }

            _serverId = serverId;
            _hostName = hostName;
            _address = address;
            _operatingSystem = operatingSystem;
            _architecture = architecture;
            _processors = processors;
            _cores = cores;
            _memory = memory;
            _settings = new SettingsDictionary(settings);
        }
    }
}
