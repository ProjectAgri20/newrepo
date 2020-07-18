using System;
using System.ComponentModel;
using System.Net;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.WebProxy
{
    [ToolboxItem(false)]
    public partial class WebProxyConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variable

        /// <summary>
        /// Web Proxy Activity Data
        /// </summary>
        private WebProxyActivityData _activityData;

        private const string SERVER_IP_FORMAT = "192.168.{0}.254";
        private int UNSECURE_PROXY_SERVER_SUFFIX = 244;
        private int SECURE_PROXY_SERVER_SUFFIX = 245;
        private int WPAD_SERVER_SUFFIX = 246;
        private int SWITCH_SUFFIX = 253;
        private int DHCP_SERVER_SUFFIX = 254;
        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public WebProxyConfigurationControl()
        {
            InitializeComponent();

            // Initialize
            _activityData = new WebProxyActivityData();

            // Subscribe for Product change event 
            base.OnProductNameChanged += WebProxyEditControl_OnProductNameChanged;

            // Set current Product Family and Name
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName.ToString();

            fieldValidator.RequireCustom(wiredIPv4_IPAddressControl, wiredIPv4_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(unsecureProxy_IPAddressControl, unsecureProxy_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(secureProxy_IPAddressControl, secureProxy_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(wpadServer_IPAddressControl, wpadServer_IPAddressControl.IsValidIPAddress, "Enter valid IP address.");
            //TODO - Add field validation for username/password/cURL
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
            // load the data from activity data to the UI elements
            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(WebProxyTests), _activityData.SelectedTests);
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SiteMapVersion;
            wiredIPv4_IPAddressControl.Text = _activityData.WiredIPv4Address;
            primaryDHCPServer_IPAddressControl.Text = _activityData.PrimaryDHCPServerIPAddress;
            secondaryDHCPServer_IPAddressControl.Text = _activityData.SecondaryDHCPServerIPAddress;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIPAddress;
            switchDetailsControl.SwitchPortNumber = _activityData.PortNumber;
            unsecureProxy_IPAddressControl.Text = _activityData.UnsecureWebProxyServerIPAddress;
            secureProxy_IPAddressControl.Text = _activityData.SecureWebProxyServerIPAddress;
            wpadServer_IPAddressControl.Text = _activityData.WPADServerIPAddress;
            unsecureProxyPort_TextBox.Text = _activityData.UnsecureWebProxyServerPortNumber.ToString();
            secureProxyPort_TextBox.Text = _activityData.SecureWebProxyServerPortNumber.ToString();
            secureProxyUsername_TextBox.Text = _activityData.SecureWebProxyServerUsername;
            secureProxyPassword_TextBox.Text = _activityData.SecureWebProxyServerPassword;
            cURLPath_TextBox.Text = _activityData.cURLPathIPAddress;
        }

        #endregion

        #region Events		

        /// <summary>
        /// Product Change Event
        /// </summary>
        /// <param name="productFamily">Product Family</param>
        /// <param name="productName">Product Name</param>
        private void WebProxyEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;
        }

        /// <summary>
        /// Occurs when the Wired IPv4 address is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wiredIPv4_IPAddressControl_TextChanged(object sender, EventArgs e)
        {
            IPAddress address = null;

            if (IPAddress.TryParse(wiredIPv4_IPAddressControl.Text, out address))
            {
                string serverPrefix = address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase));
                _activityData.UnsecureWebProxyServerIPAddress = unsecureProxy_IPAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, UNSECURE_PROXY_SERVER_SUFFIX);
                _activityData.SecureWebProxyServerIPAddress = secureProxy_IPAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, SECURE_PROXY_SERVER_SUFFIX);
                _activityData.WPADServerIPAddress = wpadServer_IPAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, WPAD_SERVER_SUFFIX);
                _activityData.PrimaryDHCPServerIPAddress = primaryDHCPServer_IPAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, DHCP_SERVER_SUFFIX);
                _activityData.SwitchIPAddress = switchDetailsControl.SwitchIPAddress = "{0}.{1}".FormatWith(serverPrefix, SWITCH_SUFFIX);

                string value = wiredIPv4_IPAddressControl.Text.Split(new char[] { '.' })[2];
                _activityData.PrimaryDHCPServerIPAddress = primaryDHCPServer_IPAddressControl.Text = SERVER_IP_FORMAT.FormatWith(value);
                if (IPAddress.Parse(primaryDHCPServer_IPAddressControl.Text).IsInSameSubnet(IPAddress.Parse(wiredIPv4_IPAddressControl.Text)))
                {
                    if (value.EndsWith("1", StringComparison.Ordinal))
                    {
                        value = string.Concat(value.Remove(value.Length - 1), "0");
                    }
                    else if (value.EndsWith("0", StringComparison.Ordinal))
                    {
                        value = string.Concat(value.Remove(value.Length - 1), "1");
                    }
                    _activityData.SecondaryDHCPServerIPAddress = secondaryDHCPServer_IPAddressControl.Text = SERVER_IP_FORMAT.FormatWith(value);
                }
                base.OnPropertyChanged(new PropertyChangedEventArgs(sender.ToString()));
            }
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new WebProxyActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<WebProxyActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();


            _activityData.ProductFamily = base.ProductCategory.ToString();
            _activityData.ProductName = base.ProductName;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SiteMapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.WiredIPv4Address = wiredIPv4_IPAddressControl.Text;
            _activityData.UnsecureWebProxyServerIPAddress = unsecureProxy_IPAddressControl.Text;
            _activityData.UnsecureWebProxyServerPortNumber = Convert.ToInt32(unsecureProxyPort_TextBox.Text);
            _activityData.SecureWebProxyServerIPAddress = secureProxy_IPAddressControl.Text;
            _activityData.SecureWebProxyServerPortNumber = Convert.ToInt32(secureProxyPort_TextBox.Text);
            _activityData.SecureWebProxyServerUsername = secureProxyUsername_TextBox.Text;
            _activityData.SecureWebProxyServerPassword = secureProxyPassword_TextBox.Text;
            _activityData.cURLPathIPAddress = cURLPath_TextBox.Text;
            _activityData.WPADServerIPAddress = wpadServer_IPAddressControl.Text;
            _activityData.PrimaryDHCPServerIPAddress = primaryDHCPServer_IPAddressControl.Text;
            _activityData.SecondaryDHCPServerIPAddress = secondaryDHCPServer_IPAddressControl.Text;
            _activityData.SwitchIPAddress = switchDetailsControl.SwitchIPAddress;
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
    }
}
