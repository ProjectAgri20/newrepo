using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// A multi-column dropdown for displaying a list of servers.
    /// </summary>
    public partial class CitrixServerDetailsComboBox : UserControl
    {
        private List<CitrixServerComboBoxItem> _items = new List<CitrixServerComboBoxItem>();

        /// <summary>
        /// Occurs when the selected index has changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged
        {
            add { servers_ComboBox.SelectedIndexChanged += value; }
            remove { servers_ComboBox.SelectedIndexChanged -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerDetailsComboBox"/> class.
        /// </summary>
        public CitrixServerDetailsComboBox()
        {
            InitializeComponent();
            servers_ComboBox.DropDownStyle = RadDropDownStyle.DropDownList;
            servers_ComboBox.DropDownMinSize = new Size(600, 0);
            servers_ComboBox.EditorControl.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        public string SelectedServer
        {
            get
            {
                var info = servers_ComboBox.SelectedItem as GridViewDataRowInfo;
                if (info != null)
                {
                    return ((CitrixServerComboBoxItem)info.DataBoundItem).HostName;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Adds the specified servers to the drop down.
        /// </summary>
        /// <param name="frameworkServers">The framework servers.</param>
        public void AddServers(IQueryable<FrameworkServer> frameworkServers)
        {
            if (frameworkServers == null)
            {
                throw new ArgumentNullException("frameworkServers");
            }

            foreach (FrameworkServer server in frameworkServers)
            {
                _items.Add(new CitrixServerComboBoxItem(server));
            }

            servers_ComboBox.EditorControl.AutoGenerateColumns = true;
            servers_ComboBox.EditorControl.DataSource = _items;
        }

        /// <summary>
        /// Sets the selected server.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        public void SetSelectedServer(string serverName)
        {
            servers_ComboBox.SelectedIndex = _items.FindIndex(n => n.HostName.Equals(serverName, StringComparison.OrdinalIgnoreCase));
        }

        private class CitrixServerComboBoxItem
        {
            public string HostName { get; set; }
            public string Version { get; set; }
            public string Architecture { get; set; }
            public string Stats { get; set; }
            public string OperatingSystem { get; set; }

            public CitrixServerComboBoxItem(FrameworkServer server)
            {
                float memory = (float)server.Memory / 1000.0F;
                HostName = server.HostName;
                Version = server.ServiceVersion;
                Architecture = server.Architecture.ToLowerInvariant();
                Stats = "{0}x{1}, {2:F}GB".FormatWith(server.Processors, server.Cores, memory);
                OperatingSystem = server.OperatingSystem;
            }
        }
    }
}
