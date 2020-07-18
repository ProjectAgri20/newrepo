using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.CertificateManagement
{
    [ToolboxItem(false)]
    public partial class CertificateManagementConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Constants

        /// <summary>
        /// Directory name for the documents.
        /// </summary>        
        public const string DIRECTORY_DRIVERS = "Drivers";

        #endregion

        #region Local variables

        private CertificateManagementActivityData _activityData;

        private int RADIUS_SERVER_SUFFIX = 240;
        private int DHCP_SERVER_SUFFIX = 254;
        private int PHYSICAL_MACHINE_SUFFIX = 252;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of CertificateManagementConfigurationControl
        /// </summary>
        public CertificateManagementConfigurationControl()
        {
            InitializeComponent();

            base.OnProductNameChanged += new ProductNameChangedEventHandler(CertificateManagementConfigurationControl_OnProductNameChanged);
            fieldValidator.RequireCustom(primaryWired_IpAddressControl, primaryWired_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(rootSha1_IpAddressControl, rootSha1_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(rootSha2_IpAddressControl, rootSha2_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(switchDetailsControl, switchDetailsControl.ValidateControls);

            if (secondaryWired_IpAddressControl.Enabled)
            {
                fieldValidator.RequireCustom(secondaryWired_IpAddressControl, secondaryWired_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            }

            if (wireless_IpAddressControl.Enabled)
            {
                fieldValidator.RequireCustom(wireless_IpAddressControl, wireless_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            }

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            switchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);

            rootSha1_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            rootSha2_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            secondaryWired_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            wireless_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);

            europaPortNumber_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            serpentPortNumber_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads Certificate Management Configuration Control
        /// </summary>		
        private void LoadUI()
        {
            sitemapVersionSelector.Initialize();

            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName;

            ProductCategory = _activityData.ProductFamily;
            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(CertificateManagementTests), _activityData.SelectedTests, PluginType.Default);
            ProductName = _activityData.ProductName;
            primaryWired_IpAddressControl.Text = _activityData.Ipv4Address;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIp;
            switchDetailsControl.SwitchPortNumber = _activityData.AuthenticatorPort;
            rootSha1_IpAddressControl.Text = _activityData.RootSha1ServerIp;
            rootSha2_IpAddressControl.Text = _activityData.RootSha2ServerIp;
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SiteMapVersion;
            secondaryWired_IpAddressControl.Text = _activityData.EuropaWiredIP;
            wireless_IpAddressControl.Text = _activityData.SerpentWirelessIP;
            europaPortNumber_NumericUpDown.Value = Convert.ToInt32(_activityData.EuropaPort);
            serpentPortNumber_NumericUpDown.Value = Convert.ToInt32(_activityData.SerpentPort);

            // In future we may add few more test cases on different interfaces, sow for now we have disabled this controls
            wireless_IpAddressControl.Enabled = false;
            secondaryWired_IpAddressControl.Enabled = false;
            europaPortNumber_NumericUpDown.Enabled = false;
            serpentPortNumber_NumericUpDown.Enabled = false;
            // Initialize the Printer Driver path 
            printDriverSelector.Initialize();
            if (!string.IsNullOrEmpty(_activityData.DriverPackagePath))
            {
                printDriverSelector.DriverPackagePath = _activityData.DriverPackagePath;
                printDriverSelector.DriverModel = _activityData.DriverModel;
            }
            else
            {
                printDriverSelector.DriverPackagePath = CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + ProductCategory + Path.DirectorySeparatorChar + ProductName + Path.DirectorySeparatorChar + DIRECTORY_DRIVERS;
            }

        }

        #endregion

        #region Implementation of IPluginConfigurationControl

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new CertificateManagementActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<CertificateManagementActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            // Get list of test cases
            _activityData.SelectedTests.Clear();
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.ProductFamily = base.ProductCategory;
            _activityData.ProductName = base.ProductName;
            _activityData.Ipv4Address = primaryWired_IpAddressControl.Text;
            _activityData.EuropaWiredIP = secondaryWired_IpAddressControl.Text;
            _activityData.SerpentWirelessIP = wireless_IpAddressControl.Text;
            _activityData.SwitchIp = switchDetailsControl.SwitchIPAddress;
            _activityData.AuthenticatorPort = switchDetailsControl.SwitchPortNumber;
            _activityData.RootSha1ServerIp = rootSha1_IpAddressControl.Text;
            _activityData.RootSha2ServerIp = rootSha2_IpAddressControl.Text;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SiteMapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.EuropaPort = Convert.ToInt32(europaPortNumber_NumericUpDown.Value);
            _activityData.SerpentPort = Convert.ToInt32(serpentPortNumber_NumericUpDown.Value);
            _activityData.DriverPackagePath = printDriverSelector.DriverPackagePath;
            _activityData.DriverModel = printDriverSelector.DriverModel;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(sitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion

        #region Events

        private void printerIp_IpAddressControl_TextChanged(object sender, EventArgs e)
        {
            IPAddress address = null;

            if (IPAddress.TryParse(primaryWired_IpAddressControl.Text, out address))
            {
                string serverPrefix = address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase));

                _activityData.RootSha1ServerIp = rootSha1_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, RADIUS_SERVER_SUFFIX);
                _activityData.RootSha2ServerIp = rootSha2_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, RADIUS_SERVER_SUFFIX + 1);
                _activityData.DhcpServerIp = "{0}.{1}".FormatWith(serverPrefix, DHCP_SERVER_SUFFIX);
                _activityData.PhysicalMachineIp = "{0}.{1}".FormatWith(serverPrefix, PHYSICAL_MACHINE_SUFFIX);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IP Address"));
        }

        private void validatePrint_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            _activityData.RequirePrintValidation = checkBox.Checked;
            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Print Validation"));
        }

        /// <summary>
        /// Occurs when the product name is changed in the base plugin
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        void CertificateManagementConfigurationControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;

            printDriverSelector.PrinterFamily = productFamily;
            printDriverSelector.PrinterName = productName;

            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;

            // In future we may add few more test cases on different interfaces, sow for now we have disabled this controls
            wireless_IpAddressControl.Enabled = false;
            secondaryWired_IpAddressControl.Enabled = false;
            europaPortNumber_NumericUpDown.Enabled = false;
            serpentPortNumber_NumericUpDown.Enabled = false;
        }


        #endregion
    }
}
