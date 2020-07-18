using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Vpn
{
    [ToolboxItem(false)]
    public partial class VpnConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private Collection<string> _vpnServers;

        public VpnConfigurationControl()
        {
            InitializeComponent();

            vpn_fieldValidator.RequireSelection(vpn_comboBox, vpn_label);
        }

        public void Initialize(PluginEnvironment environment)
        {
            var data = new VpnActivityData();
            _vpnServers = new Collection<string>(environment.PluginSettings.Where(x => x.Key.StartsWith("Server", StringComparison.OrdinalIgnoreCase)).Select(x => x.Key).ToList());
            vpn_comboBox.DataSource = _vpnServers;
            InitializeVpn(data);

            vpn_comboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            connect_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            disconnect_radioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            var data = configuration.GetMetadata<VpnActivityData>();
            _vpnServers = new Collection<string>(environment.PluginSettings.Where(x => x.Key.StartsWith("Server", StringComparison.OrdinalIgnoreCase)).Select(x => x.Key).ToList());
            vpn_comboBox.DataSource = _vpnServers;
            InitializeVpn(data);
        }

        public PluginConfigurationData GetConfiguration()
        {
            VpnActivityData data = new VpnActivityData
            {
                Name = vpn_comboBox.SelectedItem.ToString(),
                Connect = connect_radioButton.Checked
            };

            return new PluginConfigurationData(data, "1.0");
        }

        private void InitializeVpn(VpnActivityData data)
        {
            if (_vpnServers.Count == 0)
            {
                connect_radioButton.Enabled = false;
                disconnect_radioButton.Enabled = false;
                MessageBox.Show(@"No VPN configurations found, Please check with your system administrator.");
                ConfigurationServices.SystemTrace.LogDebug("No VPN connections found, disabling Edit UI elements");
                return;
            }
            vpn_comboBox.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(data.Name))
            {
                var selectedItem = _vpnServers.First(x => x.Equals(data.Name, StringComparison.OrdinalIgnoreCase));
                if (selectedItem != null)
                {
                    vpn_comboBox.SelectedIndex = _vpnServers.IndexOf(selectedItem);
                }
                else
                {
                    MessageBox.Show(
                        @"The selected VPN configuration is now missing from the database.  Defaulting to first available entry.",
                        @"Configuration Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            if (data.Connect)
            {
                connect_radioButton.Checked = true;
            }
            else
            {
                disconnect_radioButton.Checked = true;
            }
        }

        public event EventHandler ConfigurationChanged;

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(vpn_fieldValidator.ValidateAll());
        }
    }
}