using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting a framework server.
    /// </summary>
    public partial class ServerComboBox : UserControl
    {
        private ServerInfo _selectedServer;
        private readonly List<ServerComboBoxItem> _items = new List<ServerComboBoxItem>();
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerComboBox" /> class.
        /// </summary>
        public ServerComboBox()
        {
            InitializeComponent();
            servers_MultiColumnComboBox.EditorControl.AutoGenerateColumns = false;
            servers_MultiColumnComboBox.EditorControl.CurrentRowChanged += servers_MultiColumnComboBox_CurrentRowChanged;
        }

        /// <summary>
        /// Gets the <see cref="ServerInfo" /> selected in this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ServerInfo SelectedServer
        {
            get
            {
                return _selectedServer;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                SetSelectedServer(value.ServerId);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a selected server.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return _selectedServer != null; }
        }

        /// <summary>
        /// Initializes this control by loading all servers from the asset inventory.
        /// </summary>
        public void Initialize()
        {
            LoadComboBox(ConfigurationServices.AssetInventory.GetServers());
        }

        /// <summary>
        /// Initializes this control by loading all servers from the asset inventory
        /// with the specified server selected.
        /// </summary>
        /// <param name="serverId">The ID of the server to select.</param>
        public void Initialize(Guid serverId)
        {
            Initialize();
            SetSelectedServer(serverId);
        }

        /// <summary>
        /// Initializes this control by loading all servers from the asset inventory
        /// with the specified server selected.
        /// </summary>
        /// <param name="server">The server to select.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is null.</exception>
        public void Initialize(ServerInfo server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            Initialize();
            SetSelectedServer(server.ServerId);
        }

        /// <summary>
        /// Initializes this control by loading all servers with the specified attribute from the asset inventory.
        /// </summary>
        /// <param name="attribute">The required attribute for servers to load.</param>
        public void Initialize(string attribute)
        {
            LoadComboBox(ConfigurationServices.AssetInventory.GetServers(attribute));
        }

        /// <summary>
        /// Initializes this control by loading all servers with any of the specified attributes from the asset inventory.
        /// </summary>
        /// <param name="attributes">The required attributes for servers to load.</param>
        public void Initialize(IEnumerable<string> attributes)
        {
            LoadComboBox(ConfigurationServices.AssetInventory.GetServers(attributes));
        }

        /// <summary>
        /// Initializes this control by loading all servers with the specified attribute from the asset inventory
        /// with the specified server selected.
        /// </summary>
        /// <param name="serverId">The ID of the server to select.</param>
        /// <param name="attribute">The required attribute for servers to load.</param>
        public void Initialize(Guid serverId, string attribute)
        {
            Initialize(attribute);
            SetSelectedServer(serverId);
        }

        /// <summary>
        /// Initializes this control by loading all servers with the specified attribute from the asset inventory
        /// with the specified server selected.
        /// </summary>
        /// <param name="server">The server to select.</param>
        /// <param name="attribute">The required attribute for servers to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is null.</exception>
        public void Initialize(ServerInfo server, string attribute)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            Initialize(attribute);
            SetSelectedServer(server.ServerId);
        }

        private void LoadComboBox(ServerInfoCollection servers)
        {
            _suppressSelectionChanged = true;

            _items.Clear();
            _items.AddRange(servers.Select(n => new ServerComboBoxItem(n)).OrderBy(n => n.HostName));

            servers_MultiColumnComboBox.EditorControl.DataSource = null;
            servers_MultiColumnComboBox.EditorControl.DataSource = _items;

            _suppressSelectionChanged = false;
        }

        private void SetSelectedServer(Guid serverId)
        {
            _suppressSelectionChanged = true;
            servers_MultiColumnComboBox.SelectedIndex = _items.FindIndex(n => n.Server.ServerId == serverId);
            _suppressSelectionChanged = false;
        }

        private void servers_MultiColumnComboBox_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (servers_MultiColumnComboBox.SelectedItem is GridViewDataRowInfo info)
            {
                _selectedServer = ((ServerComboBoxItem)info.DataBoundItem).Server;
            }
            else
            {
                _selectedServer = null;
            }
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (!_suppressSelectionChanged)
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private sealed class ServerComboBoxItem
        {
            public ServerInfo Server { get; }

            public string HostName { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string Architecture { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string Stats { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used in data grid binding.")]
            public string OperatingSystem { get; set; }

            public ServerComboBoxItem(ServerInfo server)
            {
                Server = server;

                HostName = Server.HostName;
                Architecture = Server.Architecture;
                Stats = $"{Server.Processors}x{Server.Cores}, {Server.Memory / 1024.0F:F}GB";
                OperatingSystem = Server.OperatingSystem;
            }
        }
    }
}
