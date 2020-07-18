using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Network;
using HP.ScalableTest.Plugin.CtcBase;
using HP.ScalableTest.UI;
using HP.ScalableTest.Plugin.CtcBase.Controls;
using HP.ScalableTest.Printer;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// Control used for editing IPConfiguration activities.
    /// </summary>
    public partial class IPConfigurationConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {

		#region Local Variables

		/// <summary>
		/// Activity Data
		/// </summary>
		private IPConfigurationActivityData _activityData;
	    public const string DHCP_SERVER_IP_FORMAT   = "192.168.{0}.254";
		private const string VEP_LINUX_SERVER        = "192.168.182.254";
		private const string TPS_LINUX_SERVER        = "192.168.172.254";
		private const string LFP_LINUX_SERVER        = "192.168.192.254";

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

            ipconfigPlugin_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            ipconfigPlugin_SitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(primaryDhcpAddress_IpAddressControl, primaryDhcpAddress_IpAddressControl.IsValidIP, "Enter valid IP address.");
            fieldValidator.RequireCustom(secondDHCPServer_IpAddressControl, secondDHCPServer_IpAddressControl.IsValidIP, "Enter valid IP address.");
            fieldValidator.RequireCustom(linuxServer_IpAddressControl, linuxServer_IpAddressControl.IsValidIP, "Enter valid IP address.");
            fieldValidator.RequireCustom(ipconfigPlugin_SitemapVersionSelector, ipconfigPlugin_SitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(ipconfig_SwitchDetailsControl, ipconfig_SwitchDetailsControl.ValidateControls);
            fieldValidator.RequireCustom(ssid_TextBox, () => (ProductCategory == CtcBase.ProductFamilies.TPS ? !string.IsNullOrEmpty(ssid_TextBox.Text) : true), "Enter a value for ssid.");
           
            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipconfigPlugin_SitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipconfig_SwitchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
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

            printerDetails1.PrinterFamily = Enum<PrinterFamilies>.Parse(productFamily);
		}

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new IPConfigurationActivityData();

            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<IPConfigurationActivityData>();

            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();

            // set the UI Elements data to the activity data
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.ProductFamily = base.ProductCategory.ToString();
            _activityData.ProductName = base.ProductName;
           
            _activityData.SitemapsVersion = ipconfigPlugin_SitemapVersionSelector.SitemapVersion;
            _activityData.SwitchIP = ipconfig_SwitchDetailsControl.SwitchIPAddress;
            _activityData.PrimaryInterfacePortNo = ipconfig_SwitchDetailsControl.SwitchPortNumber;
            _activityData.SsidName = ssid_TextBox.Text;
            _activityData.PrinterConnectivity = printerDetails1.Connectivity;
            _activityData.PrimaryDhcpServerIPAddress = primaryDhcpAddress_IpAddressControl.Text;
            _activityData.SecondDhcpServerIPAddress = secondDHCPServer_IpAddressControl.Text;
            _activityData.LinuxServerIPAddress = linuxServer_IpAddressControl.Text;

            _activityData.PrinterInterfaceType = printerDetails1.PrinterInterface;
            _activityData.PrinterConnectivity = printerDetails1.Connectivity;
            _activityData.PrimaryWiredIPv4Address = printerDetails1.PrimaryInterfaceAddress.ToString();
            _activityData.SecondaryWiredIPv4Address = printerDetails1.SecondaryInterfaceAddress;
            _activityData.WirelessIPv4Address = printerDetails1.WirelessAddress;
            _activityData.WirelessMacAddress = printerDetails1.WirelessMacAddress;
            _activityData.PrimaryInterfacePortNo = printerDetails1.PrimaryInterfacePortNumber;
            _activityData.SecondaryInterfacePortNo = printerDetails1.SecondaryInterfacePortNumber;
            _activityData.WirelessInterfacePortNo = printerDetails1.WirelessAccessPointPortNumber;

            return new PluginConfigurationData(_activityData, "1.0");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// IPConfigurationEditControl_Load method loads the control
		/// </summary>
		private void LoadUI()
        {
            // load the data from activity data to the UI elements
            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (CtcBase.ProductFamilies)Enum.Parse(typeof(CtcBase.ProductFamilies), _activityData.ProductFamily);
            }

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(IPConfigurationTests), _activityData.SelectedTests, PluginType.IPConfiguration);


            printerDetails1.PrimaryInterfaceAddress = _activityData.PrimaryWiredIPv4Address ;

            printerDetails1.SecondaryInterfaceAddress = _activityData.SecondaryWiredIPv4Address;
            printerDetails1.WirelessAddress = _activityData.WirelessIPv4Address;
            printerDetails1.WirelessMacAddress = _activityData.WirelessMacAddress;
            printerDetails1.PrimaryInterfacePortNumber = _activityData.PrimaryInterfacePortNo;
            printerDetails1.SecondaryInterfacePortNumber = _activityData.SecondaryInterfacePortNo;
            printerDetails1.WirelessAccessPointPortNumber = _activityData.WirelessInterfacePortNo;
            printerDetails1.Connectivity = _activityData.PrinterConnectivity;
            printerDetails1.PrinterInterface = _activityData.PrinterInterfaceType;
            ipconfigPlugin_SitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            ipconfig_SwitchDetailsControl.SwitchIPAddress = _activityData.SwitchIP;
            ipconfig_SwitchDetailsControl.SwitchPortNumber = _activityData.PrimaryInterfacePortNo;
            ssid_TextBox.Text = _activityData.SsidName;
            primaryDhcpAddress_IpAddressControl.Text = _activityData.PrimaryDhcpServerIPAddress;
            secondDHCPServer_IpAddressControl.Text = _activityData.SecondDhcpServerIPAddress;
            linuxServer_IpAddressControl.Text = _activityData.LinuxServerIPAddress;
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
			fieldValidator.Validate(addressControl);
        }

		/// <summary>
		/// Auto populating the server IP when the IPv4 address is entered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void wiredIpv4_IPAddressControl_Validated(object sender, EventArgs e)
		{
			////string value = wiredIpv4_IPAddressControl.Text.Split(new char[] { '.' })[2];

			////primaryDhcpAddress_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(value);

			////if (IPAddress.Parse(primaryDhcpAddress_IpAddressControl.Text).IsInSameSubnet(IPAddress.Parse(wiredIpv4_IPAddressControl.Text)))
			////{
			////	if (value.EndsWith("1", StringComparison.Ordinal))
			////	{
			////		value = string.Concat(value.Remove(value.Length - 1), "0");
			////	}
			////	else if (value.EndsWith("0", StringComparison.Ordinal))
			////	{
			////		value = string.Concat(value.Remove(value.Length - 1), "1");
			////	}

			////	secondDHCPServer_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(value);
			////	linuxServer_IpAddressControl.Text = DHCP_SERVER_IP_FORMAT.FormatWith(string.Concat(value.Remove(value.Length - 1), "2"));
			////}
		}

        /// <summary>
        /// Validation for wireless mac address for TPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wirelessMacAddress_TextBox_Validating(object sender, CancelEventArgs e)
        {
			if (base.ProductCategory == CtcBase.ProductFamilies.TPS)
			{
				////fieldValidator.Validate(wirelessMacAddress_TextBox);
			}
        }

		/// <summary>
		/// Validation for SSID for TPS
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ssid_TextBox_Validating(object sender, CancelEventArgs e)
		{
			if (base.ProductCategory == CtcBase.ProductFamilies.TPS)
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
