using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Security
{
    /// <summary>
    /// Edit control for a Security Activity Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class SecurityConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {

        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private SecurityActivityData _activityData;

        private const string SERVER_IP_FORMAT = "{0}.254";
        private const string KERBEROS_SERVER_IP_FORMAT = "{0}.243";

        private bool _requireMultipleInterfaceInputs = false;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public SecurityConfigurationControl()
        {
            InitializeComponent();

            // Initialize
            _activityData = new SecurityActivityData();

            // bind the product name change event
            base.OnProductNameChanged += new ProductNameChangedEventHandler(SecurityEditControl_OnProductNameChanged);

            //sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
           // sitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(wired_IpAddressControl, wired_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(primaryDhcp_IpAddressControl, primaryDhcp_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(security_SwitchDetailsControl, security_SwitchDetailsControl.ValidateControls);
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(SecondaryDhcp_IpAddressControl, SecondaryDhcp_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(europaWired_IpAddressControl, () => (_requireMultipleInterfaceInputs ? europaWired_IpAddressControl.IsValidIPAddress() : true), "Enter a valid IP Address.");
            fieldValidator.RequireCustom(europaWireless_IpAddressControl, () => (_requireMultipleInterfaceInputs ? europaWireless_IpAddressControl.IsValidIPAddress() : true), "Enter a valid IP Address.");

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            security_SwitchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the product name is changed in the base plug in
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        void SecurityEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName.ToString();

            if (!productFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
            {
                europaWiredIP_Label.Visible = false;
                europaWired_IpAddressControl.Enabled = false;
                europaWired_IpAddressControl.Visible = false;
                europaWireless_Label.Visible = false;
                europaWireless_IpAddressControl.Enabled = false;
                europaWireless_IpAddressControl.Visible = false;
                europaWiredPortNo_NumericUpDown.Enabled = false;
                europaWiredPortNo_NumericUpDown.Visible = false;
                europaWirelessPortNo_NumericUpDown.Enabled = false;
                europaWirelessPortNo_NumericUpDown.Visible = false;
                thirdDhcpServer_IpAddressControl.Enabled = false;
            }
            else
            {
                europaWiredIP_Label.Visible = true;
                europaWired_IpAddressControl.Enabled = true;
                europaWired_IpAddressControl.Visible = true;
                europaWireless_Label.Visible = true;
                europaWireless_IpAddressControl.Enabled = true;
                europaWireless_IpAddressControl.Visible = true;
                europaWiredPortNo_NumericUpDown.Enabled = true;
                europaWiredPortNo_NumericUpDown.Visible = true;
                europaWirelessPortNo_NumericUpDown.Enabled = true;
                europaWirelessPortNo_NumericUpDown.Visible = true;
                thirdDhcpServer_IpAddressControl.Enabled = true;
            }
        }

        #endregion

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
            //loads sitemap
            sitemapVersionSelector.Initialize();
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName;

            // Loads the tests from plug-in
            LoadTestCases(typeof(SecurityTests), _activityData.SelectedTests);

            wired_IpAddressControl.Text = _activityData.WiredIPv4Address;
            security_SwitchDetailsControl.SwitchPortNumber = _activityData.WiredPortNo;
            europaWireless_IpAddressControl.Text = _activityData.WirelessIPv4Address;
            europaWirelessPortNo_NumericUpDown.Value = _activityData.WirelessPortNo;
            europaWired_IpAddressControl.Text = _activityData.SecondaryWiredIPv4Address;
            europaWiredPortNo_NumericUpDown.Value = _activityData.SecondaryWiredPortNo;
            primaryDhcp_IpAddressControl.Text = _activityData.PrimaryDhcpServerIPAddress;
            SecondaryDhcp_IpAddressControl.Text = _activityData.SecondaryDhcpServerIPAddress;
            thirdDhcpServer_IpAddressControl.Text = _activityData.ThirdDhcpServerIPAddress;
            sitemapVersionSelector.SitemapPath          = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            security_SwitchDetailsControl.SwitchIPAddress = _activityData.SwitchIPAddress;
            security_SwitchDetailsControl.SwitchPortNumber = _activityData.WiredPortNo;
        }

        private void wiredPortNo_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            security_SwitchDetailsControl.SwitchPortNumber = (int)((NumericUpDown)sender).Value;
        }


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

                if (currentControl.Name == wired_IpAddressControl.Name)
                {
                    primaryDhcp_IpAddressControl.Text = dhcpServerIP;
                }
                else if (currentControl.Name == europaWired_IpAddressControl.Name)
                {
                    SecondaryDhcp_IpAddressControl.Text = dhcpServerIP;
                }
                else
                {
                    thirdDhcpServer_IpAddressControl.Text = dhcpServerIP;
                }
            }

            base.OnPropertyChanged(new PropertyChangedEventArgs("IP Address"));
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new SecurityActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<SecurityActivityData>(CtcMetadataConverter.Converters);
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
            _activityData.WiredIPv4Address = wired_IpAddressControl.Text;
            _activityData.WirelessIPv4Address = europaWireless_IpAddressControl.Text;
            _activityData.WirelessPortNo = (int)europaWirelessPortNo_NumericUpDown.Value;
            _activityData.SecondaryWiredIPv4Address = europaWired_IpAddressControl.Text;
            _activityData.SecondaryWiredPortNo = (int)europaWiredPortNo_NumericUpDown.Value;
            _activityData.SwitchIPAddress = security_SwitchDetailsControl.SwitchIPAddress;
            _activityData.WiredPortNo = security_SwitchDetailsControl.SwitchPortNumber;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SitemapsVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.SwitchIPAddress = security_SwitchDetailsControl.SwitchIPAddress;
            _activityData.PrimaryDhcpServerIPAddress = primaryDhcp_IpAddressControl.Text;
            _activityData.SecondaryDhcpServerIPAddress = SecondaryDhcp_IpAddressControl.Text;
            _activityData.ThirdDhcpServerIPAddress = thirdDhcpServer_IpAddressControl.Text;
            _activityData.KerberosServerIPAddress = KERBEROS_SERVER_IP_FORMAT.FormatWith(wired_IpAddressControl.Text.Substring(0, wired_IpAddressControl.Text.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            _requireMultipleInterfaceInputs = false;

            if (base.ProductCategory == ProductFamilies.VEP)
            {
                if (!europaWired_IpAddressControl.IsValidIPAddress() || !europaWireless_IpAddressControl.IsValidIPAddress())
                {
                    DialogResult result = MessageBox.Show("Either {0} or {1} is not provided or invalid. Click OK to continue or Cancel to ignore.".FormatWith(europaWired_IpAddressControl.Tag, europaWireless_IpAddressControl.Tag), "Missing Input Or Invalid Input", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Cancel)
                    {
                        _requireMultipleInterfaceInputs = true;
                        fieldValidator.Validate(europaWired_IpAddressControl);
                        fieldValidator.Validate(europaWireless_IpAddressControl); ;
                    }
                    else
                    {
                        _requireMultipleInterfaceInputs = false;
                    }
                }
            }

            return new PluginValidationResult(fieldValidator.ValidateAll());
        }
    }
}