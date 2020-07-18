using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using HP.ScalableTest.Core.AssetInventory;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Proxy class to support UI integration with the <see cref="FrameworkServer"/> class.
    /// </summary>
    public class FrameworkServerProxy : INotifyPropertyChanged
    {
        private Collection<FrameworkServerType> _serverTypes = new Collection<FrameworkServerType>();
        private string _hostName = string.Empty;
        private string _architecture = string.Empty;
        private bool _active = true;
        private int _cores = 1;
        private string _diskSpace = string.Empty;
        private int _processors = 1;
        private int _memory = 2048;
        private string _contact = string.Empty;
        private string _status = "Available";
        private string _ipAddress = string.Empty;
        private string _serviceVersion = string.Empty;

        /// <summary>
        /// Occurs when a property in this class changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerProxy" /> class.
        /// </summary>
        public FrameworkServerProxy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerProxy" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        public FrameworkServerProxy(FrameworkServer server)
            : this()
        {
            CopyFrom(server);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerProxy" /> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public FrameworkServerProxy(string hostName)
            : this()
        {
            HostName = hostName;
        }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName
        {
            get { return _hostName; }
            set
            {
                _hostName = value;
                OnPropertyChanged("HostName");
            }
        }

        /// <summary>
        /// Gets or sets the memory.
        /// </summary>
        /// <value>
        /// The memory.
        /// </value>
        public int Memory
        {
            get { return _memory; }
            set
            {
                _memory = value;
                OnPropertyChanged("Memory");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FrameworkServerProxy" /> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged("Active");
            }
        }

        /// <summary>
        /// Gets or sets the cores.
        /// </summary>
        /// <value>
        /// The cores.
        /// </value>
        public int Cores
        {
            get { return _cores; }
            set
            {
                _cores = value;
                OnPropertyChanged("Cores");
            }
        }

        /// <summary>
        /// Gets or sets the architecture.
        /// </summary>
        /// <value>
        /// The architecture.
        /// </value>
        public string Architecture
        {
            get { return _architecture; }
            set
            {
                _architecture = value;
                OnPropertyChanged("Architecture");
            }
        }

        /// <summary>
        /// Gets or sets the service version.
        /// </summary>
        /// <value>The service version.</value>
        public string ServiceVersion
        {
            get { return _serviceVersion; }
            set
            {
                _serviceVersion = value;
                OnPropertyChanged("ServiceVersion");
            }
        }

        /// <summary>
        /// Gets or sets the disk space.
        /// </summary>
        /// <value>
        /// The disk space.
        /// </value>
        public string DiskSpace
        {
            get { return _diskSpace; }
            set
            {
                _diskSpace = value;
                OnPropertyChanged("DiskSpace");
            }
        }

        /// <summary>
        /// Gets or sets the processors.
        /// </summary>
        /// <value>
        /// The processors.
        /// </value>
        public int Processors
        {
            get { return _processors; }
            set
            {
                _processors = value;
                OnPropertyChanged("Processors");
            }
        }

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }

        /// <summary>
        /// Gets or sets the operating system.
        /// </summary>
        /// <value>
        /// The operating system.
        /// </value>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Called when a property changed.
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Gets the server types.
        /// </summary>
        /// <value>
        /// The server types.
        /// </value>
        public Collection<FrameworkServerType> ServerTypes
        {
            get { return _serverTypes; }
        }

        /// <summary>
        /// Copies from an instance of a <see cref="FrameworkServer"/>.
        /// </summary>
        /// <param name="server">The server.</param>
        public void CopyFrom(FrameworkServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            Active = server.Active;
            Architecture = server.Architecture;
            Cores = server.Cores;
            DiskSpace = server.DiskSpace;
            HostName = server.HostName;
            Memory = server.Memory;
            OperatingSystem = server.OperatingSystem;
            Processors = server.Processors;
            Contact = server.Contact;
            Status = server.Status;
            IpAddress = server.IPAddress;
            ServiceVersion = server.ServiceVersion;

            foreach (var type in server.ServerTypes)
            {
                ServerTypes.Add(type);
            }
        }

        /// <summary>
        /// Copies to an instance of a <see cref="FrameworkServer"/>.
        /// </summary>
        /// <param name="server">The server.</param>
        public void CopyTo(FrameworkServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            server.Active = Active;
            server.Architecture = Architecture;
            server.Cores = Cores;
            server.DiskSpace = DiskSpace;
            server.HostName = HostName;
            server.Memory = Memory;
            server.OperatingSystem = OperatingSystem;
            server.Processors = Processors;
            server.Contact = Contact;
            server.Status = Status;
            server.IPAddress = IpAddress;
            server.ServiceVersion = ServiceVersion;

            server.ServerTypes.Clear();
            foreach (var item in ServerTypes)
            {
                server.ServerTypes.Add(item);
            }
        }

        /// <summary>
        /// Copies from an instance of a <see cref="FrameworkServerProxy"/>.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        public void CopyFrom(FrameworkServerProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            Active = proxy.Active;
            Architecture = proxy.Architecture;
            Cores = proxy.Cores;
            DiskSpace = proxy.DiskSpace;
            HostName = proxy.HostName;
            Memory = proxy.Memory;
            OperatingSystem = proxy.OperatingSystem;
            Processors = proxy.Processors;
            IpAddress = proxy.IpAddress;
            ServiceVersion = proxy.ServiceVersion;

            foreach (var type in proxy.ServerTypes)
            {
                ServerTypes.Add(type);
            }
        }
    }
}
