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

namespace HP.ScalableTest.Plugin.IPSecurity
{
    /// <summary>
    /// Edit control for a IPSecurity Activity Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class IPSecurityConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private IPSecurityActivityData _activityData;

        private const string SERVER_IP_FORMAT = "{0}.254";

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public IPSecurityConfigurationControl()
        {
            InitializeComponent();

            // Initialize
            _activityData = new IPSecurityActivityData();

            // bind the product name change event
            base.OnProductNameChanged += new ProductNameChangedEventHandler(IPSecurityEditControl_OnProductNameChanged);

            //sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //sitemapVersionSelector.PrinterName = base.ProductName;
            //print_PrintDriverSelector.PrinterFamily = base.ProductCategory.ToString();
            //print_PrintDriverSelector.PrinterName = base.ProductName;



            // fieldValidator.RequireCustom(wirelessIpv4_IpAddressControl, () => (ProductCategory == ProductFamilies.VEP ? string.IsNullOrEmpty(wirelessIpv4_IpAddressControl.Text) : true), "Enter valid wireless IP");
            // fieldValidator.RequireCustom(europaWired_IpAddressControl, () => (ProductCategory == ProductFamilies.VEP ? string.IsNullOrEmpty(europaWired_IpAddressControl.Text) : true), "Enter valid Europa IP");
            fieldValidator.RequireCustom(wiredIpv4_IpAddressControl, wiredIpv4_IpAddressControl.IsValidIPAddress, "Enter valid wired IP address.");
            fieldValidator.RequireCustom(windowsSecondaryIP_IpAddressControl, windowsSecondaryIP_IpAddressControl.IsValidIPAddress, "Enter valid Secondary IP address.");
            fieldValidator.RequireCustom(linuxFedoraClient_IpAddressControl, linuxFedoraClient_IpAddressControl.IsValidIPAddress, "Enter valid Linux Client IP address.");
            fieldValidator.RequireCustom(primaryDhcpAddress_IpAddressControl, primaryDhcpAddress_IpAddressControl.IsValidIPAddress, "Enter valid Primary DHCP Server IP address.");
            fieldValidator.RequireCustom(secondDHCPServer_IpAddressControl, secondDHCPServer_IpAddressControl.IsValidIPAddress, "Enter valid Secondary DHCP Server IP address.");
            fieldValidator.RequireCustom(kerberosServer_IpAddressControl, kerberosServer_IpAddressControl.IsValidIPAddress, "Enter valid Kerberos Server address.");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(switchDetailsControl, switchDetailsControl.ValidateControls);
            fieldValidator.RequireCustom(print_PrintDriverSelector, print_PrintDriverSelector.ValidateControls);

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            switchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            print_PrintDriverSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new IPSecurityActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<IPSecurityActivityData>(CtcMetadataConverter.Converters);
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

            _activityData.WiredIPv4Address = wiredIpv4_IpAddressControl.Text;
            _activityData.WirelessIPv4Address = wirelessIpv4_IpAddressControl.Text;
            _activityData.EuropaWiredIPv4Address = europaWired_IpAddressControl.Text;
            _activityData.WindowsSecondaryClientIPAddress = windowsSecondaryIP_IpAddressControl.Text;
            _activityData.LinuxFedoraClientIPAddress = linuxFedoraClient_IpAddressControl.Text;
            _activityData.PrimaryDhcpServerIPAddress = primaryDhcpAddress_IpAddressControl.Text;
            _activityData.SecondDhcpServerIPAddress = secondDHCPServer_IpAddressControl.Text;
            _activityData.KerberosServerIPAddress = kerberosServer_IpAddressControl.Text;

            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SitemapsVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.PrintDriverLocation = print_PrintDriverSelector.DriverPackagePath;
            _activityData.PrintDriverModel = print_PrintDriverSelector.DriverModel;

            _activityData.SwitchIPAddress = switchDetailsControl.SwitchIPAddress;
            _activityData.PortNo = switchDetailsControl.SwitchPortNumber;

            _activityData.MessageBoxCheckBox = message_CheckBox.Checked;
            _activityData.WiredInterface = wired_RadioButton.Checked;
            _activityData.WirelessInterface = wireless_RadioButton.Checked;
            _activityData.EuropaInterface = europa_RadioButton.Checked;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(sitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Load UI Elements with Activity Data
		/// </summary>		
		private void LoadUI()
        {
            LoadUIControls();

            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }
            else
            {
                ProductCategory = ProductFamilies.None;
            }

            ProductName = _activityData.ProductName;

            sitemapVersionSelector.Initialize();
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName;

            // Initialize the Printer Driver path 
            print_PrintDriverSelector.Initialize();
            print_PrintDriverSelector.PrinterFamily = base.ProductCategory.ToString();
            print_PrintDriverSelector.PrinterName = base.ProductName;
            // Loads the tests from plug-in
            LoadTestCases(typeof(IPSecurityTests), _activityData.SelectedTests);

            wiredIpv4_IpAddressControl.Text = _activityData.WiredIPv4Address;
            wirelessIpv4_IpAddressControl.Text = _activityData.WirelessIPv4Address;
            europaWired_IpAddressControl.Text = _activityData.EuropaWiredIPv4Address;

            windowsSecondaryIP_IpAddressControl.Text = _activityData.WindowsSecondaryClientIPAddress;
            linuxFedoraClient_IpAddressControl.Text = _activityData.LinuxFedoraClientIPAddress;

            primaryDhcpAddress_IpAddressControl.Text = _activityData.PrimaryDhcpServerIPAddress;
            secondDHCPServer_IpAddressControl.Text = _activityData.SecondDhcpServerIPAddress;
            kerberosServer_IpAddressControl.Text = _activityData.KerberosServerIPAddress;

            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            print_PrintDriverSelector.DriverModel = _activityData.PrintDriverModel;
            print_PrintDriverSelector.DriverPackagePath = _activityData.PrintDriverLocation;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIPAddress;
            switchDetailsControl.SwitchPortNumber = _activityData.PortNo;

            message_CheckBox.Checked = _activityData.MessageBoxCheckBox;
            wired_RadioButton.Checked = _activityData.WiredInterface;
            wireless_RadioButton.Checked = _activityData.WirelessInterface;
            europa_RadioButton.Checked = _activityData.EuropaInterface;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the product name is changed in the base plug in
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        void IPSecurityEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;

            print_PrintDriverSelector.PrinterFamily = productFamily;
            print_PrintDriverSelector.PrinterName = productName;

            wired_RadioButton.Checked = true;
            if (!(ProductCategory.ToString().EqualsIgnoreCase(ProductFamilies.VEP.ToString())))
            {
                europaWired_IpAddressControl.Enabled = false;
                wirelessIpv4_IpAddressControl.Enabled = false;
                europa_RadioButton.Enabled = false;
                wireless_RadioButton.Enabled = false;
            }
            else
            {
                europaWired_IpAddressControl.Enabled = true;
                wirelessIpv4_IpAddressControl.Enabled = true;
                europa_RadioButton.Enabled = true;
                wireless_RadioButton.Enabled = true;
            }
        }

        /// <summary>
        /// Occurs on text change event of IP address control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void ipAddressControl_TextChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs("IP Address"));
        }

        #endregion

        /// <summary>
        /// Occurs when the wired IPv4 address is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IPAddressControl_TextChanged(object sender, EventArgs e)
        {
            IPAddressControl currentControl = (IPAddressControl)sender;

            IPAddress address = null;

            if (IPAddress.TryParse(currentControl.Text, out address))
            {
                string dhcpServerIP = SERVER_IP_FORMAT.FormatWith(address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));

                if (currentControl.Name == wiredIpv4_IpAddressControl.Name)
                {
                    primaryDhcpAddress_IpAddressControl.Text = dhcpServerIP;
                }
                else if (currentControl.Name == europaWired_IpAddressControl.Name)
                {
                    secondDHCPServer_IpAddressControl.Text = dhcpServerIP;
                }
            }

            base.OnPropertyChanged(new PropertyChangedEventArgs("IP Address"));
        }

        private void message_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (message_CheckBox.Checked)
            {
                MessageBox.Show("Are you sure to enable debug message box, which will halt the execution", "Confirmation");
                return;
            }
        }
    }
}