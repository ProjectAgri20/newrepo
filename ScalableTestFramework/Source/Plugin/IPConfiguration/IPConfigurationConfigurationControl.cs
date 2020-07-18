using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.Utility;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// Control used for editing IPConfiguration activities.
    /// </summary>
    [ToolboxItem(false)]
    public partial class IPConfigurationConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {

        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private IPConfigurationActivityData _activityData;
        private const string DHCP_SERVER_IP_FORMAT = "192.168.{0}.254";
        private const string VEP_LINUX_SERVER = "192.168.182.254";
        private const string TPS_LINUX_SERVER = "192.168.172.254";
        private const string LFP_LINUX_SERVER = "192.168.192.254";

        private bool _requireWirelessInterfaceAddress = false;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IPConfigurationConfigurationControl"/> class.
        /// </summary>
        public IPConfigurationConfigurationControl()
        {
            InitializeComponent();

            _activityData = new IPConfigurationActivityData();

            // bind the product name change event
            base.OnProductNameChanged += new ProductNameChangedEventHandler(IPConfigurationEditControl_OnProductNameChanged);

            //ipconfigPlugin_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //ipconfigPlugin_SitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(wiredIpv4_IPAddressControl, wiredIpv4_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(primaryDhcpAddress_IpAddressControl, primaryDhcpAddress_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(secondDHCPServer_IpAddressControl, secondDHCPServer_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(linuxServer_IpAddressControl, linuxServer_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(ipconfigPlugin_SitemapVersionSelector, ipconfigPlugin_SitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(ipconfig_SwitchDetailsControl, ipconfig_SwitchDetailsControl.ValidateControls);
            fieldValidator.RequireCustom(ssid_TextBox, () => (ProductCategory == ProductFamilies.TPS ? !string.IsNullOrEmpty(ssid_TextBox.Text) : true), "Enter a value for ssid.");
            fieldValidator.RequireCustom(wirelessMacAddress_TextBox, () => (ProductCategory == ProductFamilies.TPS ? wirelessMacAddress_TextBox.Text.Length == 12 : true), "Enter a valid mac address having 12 characters.");

            fieldValidator.RequireCustom(wirelessIpv4_IpAddressControl, () => (_requireWirelessInterfaceAddress ? wirelessIpv4_IpAddressControl.IsValidIPAddress() : true), "Enter a valid IP Address.");

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipconfigPlugin_SitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipconfig_SwitchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            wirelessMacAddress_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            ssid_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Occurs when the product name is changed in the base plug in
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        void IPConfigurationEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            ipconfigPlugin_SitemapVersionSelector.PrinterFamily = productFamily;
            ipconfigPlugin_SitemapVersionSelector.PrinterName = productName;

            if ((ProductCategory == ProductFamilies.TPS) || (ProductCategory == ProductFamilies.InkJet))
            {
                wirelessDetails_GroupBox.Enabled = true;
                wirelessIpv4_IpAddressControl.Enabled = false;
                wirelessMacAddress_TextBox.Enabled = true;
                wirelessMac_Label.Visible = true;
                wirelessMacAddress_TextBox.Visible = true;
            }
            else
            {
                if (ProductCategory == ProductFamilies.VEP)
                {
                    wirelessIpv4_IpAddressControl.Enabled = true;
                }
                else
                {
                    wirelessIpv4_IpAddressControl.Enabled = false;
                }

                wirelessDetails_GroupBox.Enabled = false;
                wirelessMacAddress_TextBox.Enabled = false;
                wirelessMac_Label.Visible = false;
                wirelessMacAddress_TextBox.Visible = false;
            }
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new IPConfigurationActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<IPConfigurationActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();

            // set the UI Elements data to the activity data
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.ProductFamily = base.ProductCategory.ToString();
            _activityData.ProductName = base.ProductName;
            _activityData.WiredIPv4Address = wiredIpv4_IPAddressControl.Text;
            _activityData.WirelessIPv4Address = wirelessIpv4_IpAddressControl.Text;
            _activityData.WirelessMacAddress = wirelessMacAddress_TextBox.Text;
            _activityData.SitemapPath = ipconfigPlugin_SitemapVersionSelector.SitemapPath;
            _activityData.SitemapsVersion = ipconfigPlugin_SitemapVersionSelector.SitemapVersion;
            _activityData.SwitchIP = ipconfig_SwitchDetailsControl.SwitchIPAddress;
            _activityData.PortNo = ipconfig_SwitchDetailsControl.SwitchPortNumber;
            _activityData.SsidName = ssid_TextBox.Text;
            _activityData.PrinterConnectivity = radioButton_Wired.Checked ? ConnectivityType.Wired : ConnectivityType.Wireless;
            _activityData.PrimaryDhcpServerIPAddress = primaryDhcpAddress_IpAddressControl.Text;
            _activityData.SecondDhcpServerIPAddress = secondDHCPServer_IpAddressControl.Text;
            _activityData.LinuxServerIPAddress = linuxServer_IpAddressControl.Text;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            _requireWirelessInterfaceAddress = false;

            // Validate wireless IP only for VEP
            if (base.ProductCategory == ProductFamilies.VEP)
            {
                DialogResult dialogresult = MessageBox.Show("The {0} is not provided. Click OK to continue or Cancel to ignore.".FormatWith(wirelessIpv4_IpAddressControl.Tag), "Missing Input Or Invalid Input", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dialogresult == DialogResult.Cancel)
                {
                    _requireWirelessInterfaceAddress = true;
                    fieldValidator.Validate(wirelessIpv4_IpAddressControl);
                }
                else
                {
                    _requireWirelessInterfaceAddress = false;
                }
            }
          //  MessageBox.Show("Test");
            // IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            // result = result.Concat(ipconfigPlugin_SitemapVersionSelector.ValidateControl());
            // return new PluginValidationResult(result);
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// IPConfigurationEditControl_Load method loads the control
		/// </summary>
		private void LoadUI()
        {
            LoadUIControls();
            ipconfigPlugin_SitemapVersionSelector.Initialize();
            ipconfigPlugin_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            ipconfigPlugin_SitemapVersionSelector.PrinterName = base.ProductName;

            // load the data from activity data to the UI elements
            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(IPConfigurationTests), _activityData.SelectedTests, PluginType.IPConfiguration);

            wiredIpv4_IPAddressControl.Text = _activityData.WiredIPv4Address;
            wirelessIpv4_IpAddressControl.Text = _activityData.WirelessIPv4Address;
            wirelessMacAddress_TextBox.Text = _activityData.WirelessMacAddress;
            ipconfigPlugin_SitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            ipconfigPlugin_SitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            ipconfig_SwitchDetailsControl.SwitchIPAddress = _activityData.SwitchIP;
            ipconfig_SwitchDetailsControl.SwitchPortNumber = _activityData.PortNo;
            ssid_TextBox.Text = _activityData.SsidName;
            primaryDhcpAddress_IpAddressControl.Text = _activityData.PrimaryDhcpServerIPAddress;
            secondDHCPServer_IpAddressControl.Text = _activityData.SecondDhcpServerIPAddress;
            linuxServer_IpAddressControl.Text = _activityData.LinuxServerIPAddress;

            if (_activityData.PrinterConnectivity == ConnectivityType.Wired || _activityData.PrinterConnectivity == ConnectivityType.None)
            {
                radioButton_Wired.Checked = true;
            }
            else if (_activityData.PrinterConnectivity == ConnectivityType.Wireless)
            {
                radioButton_Wireless.Checked = true;
            }
        }

        #endregion

        #region Events		

        /// <summary>
        /// validating IP address control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPAddressControl_Validating(object sender, CancelEventArgs e)
        {
            IPAddressControl addressControl = (IPAddressControl)sender;

            // Validate wireless IP only for VEP
            if (addressControl.Name.EqualsIgnoreCase(wirelessIpv4_IpAddressControl.Name) && base.ProductCategory == ProductFamilies.TPS)
            {
                fieldValidator.Validate(addressControl);
                return;
            }

            fieldValidator.Validate(addressControl);
        }

        /// <summary>
        /// Auto populating the server IP when the IPv4 address is entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wiredIpv4_IPAddressControl_Validated(object sender, EventArgs e)
        {
            string value = wiredIpv4_IPAddressControl.Text.Split(new char[] { '.' })[2];

            primaryDhcpAddress_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(value);

            if (IPAddress.Parse(primaryDhcpAddress_IpAddressControl.Text).IsInSameSubnet(IPAddress.Parse(wiredIpv4_IPAddressControl.Text)))
            {
                if (value.EndsWith("1", StringComparison.Ordinal))
                {
                    value = string.Concat(value.Remove(value.Length - 1), "0");
                }
                else if (value.EndsWith("0", StringComparison.Ordinal))
                {
                    value = string.Concat(value.Remove(value.Length - 1), "1");
                }

                secondDHCPServer_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(value);
                linuxServer_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(string.Concat(value.Remove(value.Length - 1), "2"));
            }
        }

        /// <summary>
        /// Validation for wireless mac address for TPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wirelessMacAddress_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if ((base.ProductCategory == ProductFamilies.TPS) || (base.ProductCategory == ProductFamilies.InkJet))
            {
                fieldValidator.Validate(wirelessMacAddress_TextBox);
            }
        }

        /// <summary>
        /// Validation for SSID for TPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssid_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if ((base.ProductCategory == ProductFamilies.TPS) || (base.ProductCategory == ProductFamilies.InkJet))
            {
                fieldValidator.Validate(ssid_TextBox);
            }
        }

        /// <summary>
        /// Occurs on checked change event of radio button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void radioButton_Wired_CheckedChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs("Connectivity"));
        }

        #endregion

        /// <summary>
        /// Occurs on text change event of IP address control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void iPAddressControl_TextChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs("IP Address"));
        }
    }
}
