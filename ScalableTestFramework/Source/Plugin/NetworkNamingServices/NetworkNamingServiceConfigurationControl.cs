using System;
using System.ComponentModel;
using System.Net;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    [ToolboxItem(false)]
    public partial class NetworkNamingServiceConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variable

        /// <summary>
        /// Network Naming Service Activity Data
        /// </summary>
        private NetworkNamingServiceActivityData _activityData;

        private const string SERVER_IP_FORMAT = "{0}.254";

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public NetworkNamingServiceConfigurationControl()
        {
            InitializeComponent();

            // Initialize
            _activityData = new NetworkNamingServiceActivityData();

            // Subscribe for Product change event 
            base.OnProductNameChanged += NetworkNamingServiceEditControl_OnProductNameChanged;

            // Set current Product Family and Name
            //sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //sitemapVersionSelector.PrinterName = base.ProductName.ToString();

            fieldValidator.RequireCustom(wiredIpv4_IPAddressControl, wiredIpv4_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(wirelessIpv4_IPAddressControl, wirelessIpv4_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(secondPrinter_IPAddressControl, secondPrinter_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(primaryDhcpAddress_IpAddressControl, primaryDhcpAddress_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(secondDHCPServer_IpAddressControl, secondDHCPServer_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(linuxServer_IpAddressControl, linuxServer_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(switchDetailsControl, switchDetailsControl.ValidateControls);

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            switchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load UI Elements with Activity Data
        /// </summary>		
        private void LoadUI()
        {

            LoadUIControls();
            sitemapVersionSelector.Initialize();
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName.ToString();
            // load the data from activity data to the UI elements
            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(NetworkNamingServiceTests), _activityData.SelectedTests);

            wiredIpv4_IPAddressControl.Text = _activityData.WiredIPv4Address;
            secondPrinter_IPAddressControl.Text = _activityData.SecondPrinterIPAddress;
            wirelessIpv4_IPAddressControl.Text = _activityData.WirelessIPv4Address;
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SiteMapVersion;
            primaryDhcpAddress_IpAddressControl.Text = _activityData.PrimaryDhcpServerIPAddress;
            secondDHCPServer_IpAddressControl.Text = _activityData.SecondDhcpServerIPAddress;
            linuxServer_IpAddressControl.Text = _activityData.LinuxServerIPAddress;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIpAddress;
            switchDetailsControl.SwitchPortNumber = _activityData.PortNumber;
        }

        #endregion

        #region Events		

        /// <summary>
        /// Product Change Event
        /// </summary>
        /// <param name="productFamily">Product Family</param>
        /// <param name="productName">Product Name</param>
        private void NetworkNamingServiceEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;
        }

        /// <summary>
        /// Occurs when the wired IPv4 address is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wiredIpv4_IPAddressControl_TextChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(sender.ToString()));
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new NetworkNamingServiceActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<NetworkNamingServiceActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();

            _activityData.SelectedTests = base.SelectedTests;
            _activityData.ProductFamily = base.ProductCategory.ToString();
            _activityData.ProductName = base.ProductName;
            _activityData.WiredIPv4Address = wiredIpv4_IPAddressControl.Text;
            _activityData.WirelessIPv4Address = wirelessIpv4_IPAddressControl.Text;
            _activityData.SecondPrinterIPAddress = secondPrinter_IPAddressControl.Text;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SiteMapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.PrimaryDhcpServerIPAddress = primaryDhcpAddress_IpAddressControl.Text;
            _activityData.SecondDhcpServerIPAddress = secondDHCPServer_IpAddressControl.Text;
            _activityData.LinuxServerIPAddress = linuxServer_IpAddressControl.Text;
            _activityData.SwitchIpAddress = switchDetailsControl.SwitchIPAddress;
            _activityData.PortNumber = switchDetailsControl.SwitchPortNumber;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(sitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion

        private void wiredIpv4_IPAddressControl_Validated(object sender, EventArgs e)
        {
            IPAddress address = null;

            if (IPAddress.TryParse(wiredIpv4_IPAddressControl.Text, out address))
            {
                string primaryDhcpServerIP = SERVER_IP_FORMAT.FormatWith(address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));

                primaryDhcpAddress_IpAddressControl.Text = primaryDhcpServerIP;

                string[] values = wiredIpv4_IPAddressControl.Text.Split(new char[] { '.' });

                if (values[2].EndsWith("1", StringComparison.Ordinal))
                {
                    secondDHCPServer_IpAddressControl.Text = SERVER_IP_FORMAT.FormatWith("{0}.{1}.{2}".FormatWith(values[0], values[1], string.Concat(values[2].Remove(values[2].Length - 1), "0")));
                }
                else if (values[2].EndsWith("0", StringComparison.Ordinal))
                {
                    secondDHCPServer_IpAddressControl.Text = SERVER_IP_FORMAT.FormatWith("{0}.{1}.{2}".FormatWith(values[0], values[1], string.Concat(values[2].Remove(values[2].Length - 1), "1")));
                }

                linuxServer_IpAddressControl.Text = SERVER_IP_FORMAT.FormatWith("{0}.{1}.{2}".FormatWith(values[0], values[1], string.Concat(values[2].Remove(values[2].Length - 1), "2")));
            }
        }
    }
}
