using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using System.Linq;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DotOneX
{
    [ToolboxItem(false)]
    public partial class DotOneXConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local variables

        private DotOneXActivityData _activityData;

        private int RADIUS_SERVER_SUFFIX = 240;
        private int DHCP_SERVER_SUFFIX = 254;
        private int PHYSICAL_MACHINE_SUFFIX = 252;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of DotOneXEditControl
        /// </summary>
        public DotOneXConfigurationControl()
        {
            InitializeComponent();
            base.OnProductNameChanged += new ProductNameChangedEventHandler(DotOneXEditControl_OnProductNameChanged);
           //Moving to Load UI
            //sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //sitemapVersionSelector.PrinterName = base.ProductName;

            //printDriverSelector1.PrinterFamily = base.ProductCategory.ToString();
            //printDriverSelector1.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(primaryWired_IpAddressControl, primaryWired_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(rootSha1_IpAddressControl, rootSha1_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(rootSha2_IpAddressControl, rootSha2_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(subSha2_IpAddressControl, subSha2_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(switchDetailsControl, switchDetailsControl.ValidateControls);

            if (secondaryWired_IpAddressControl.Enabled)
            {
                fieldValidator.RequireCustom(secondaryWired_IpAddressControl, secondaryWired_IpAddressControl.IsValidIPAddress, "Enter valid IP address.");
            }

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            switchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);

            rootSha1_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            rootSha2_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            subSha2_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            secondaryWired_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);

            europaPortNumber_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Public Methods

        /// <summary>
		/// Loads Dot OneX Configuration Control
		/// </summary>		
		private void LoadUI()
        {
            LoadUIControls();

            sitemapVersionSelector.Initialize();
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName;

            // Initialize the Printer Driver path 
            printDriverSelector1.Initialize();
            printDriverSelector1.PrinterFamily = base.ProductCategory.ToString();
            printDriverSelector1.PrinterName = base.ProductName;

            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }
            else
            {
                ProductCategory = ProductFamilies.None;
            }

            LoadTestCases(typeof(DotOneXTests), _activityData.SelectedTests, PluginType.Default);
            ProductName = _activityData.ProductName;
            primaryWired_IpAddressControl.Text = _activityData.Ipv4Address;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIp;
            switchDetailsControl.SwitchPortNumber = _activityData.AuthenticatorPort;
            rootSha1_IpAddressControl.Text = _activityData.RootSha1ServerIp;
            rootSha2_IpAddressControl.Text = _activityData.RootSha2ServerIp;
            subSha2_IpAddressControl.Text = _activityData.SubSha2ServerIp;
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SiteMapVersion;
            validatePrint_CheckBox.Checked = _activityData.RequirePrintValidation;
            secondaryWired_IpAddressControl.Text = _activityData.EuropaWiredIP;
            europaPortNumber_NumericUpDown.Value = Convert.ToInt32(_activityData.EuropaPort);
            packetCaptureMachine_IpAddressControl.Text = _activityData.PhysicalMachineIp;

            if (_activityData.RadiusServerType == RadiusServerTypes.RootSha1)
            {
                rootSha1_RadioButton.Checked = true;
            }
            else if (_activityData.RadiusServerType == RadiusServerTypes.RootSha2)
            {
                rootSha2_RadioButton.Checked = true;
            }
            else if (!_activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
            {
                subSha2_RadioButton.Checked = true;
            }

            printDriverSelector1.DriverModel = _activityData.PrintDriverModel;
            printDriverSelector1.DriverPackagePath = _activityData.PrintDriver;
        }

        #endregion

        #region Implementation of IPluginConfigurationControl

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new DotOneXActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<DotOneXActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            // Get list of test cases
            _activityData.SelectedTests.Clear();
            _activityData.SelectedTests = SelectedTests;
            _activityData.ProductFamily = ProductCategory.ToString();
            _activityData.ProductName = ProductName;
            _activityData.Ipv4Address = primaryWired_IpAddressControl.Text;
            _activityData.EuropaWiredIP = secondaryWired_IpAddressControl.Text;
            _activityData.SwitchIp = switchDetailsControl.SwitchIPAddress;
            _activityData.AuthenticatorPort = switchDetailsControl.SwitchPortNumber;
            _activityData.RootSha1ServerIp = rootSha1_IpAddressControl.Text;
            _activityData.RootSha2ServerIp = rootSha2_IpAddressControl.Text;
            _activityData.SubSha2ServerIp = subSha2_IpAddressControl.Text;
            _activityData.RadiusServerIp = rootSha1_RadioButton.Checked ? rootSha1_IpAddressControl.Text : (rootSha2_RadioButton.Checked ? rootSha2_IpAddressControl.Text : subSha2_IpAddressControl.Text);
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SiteMapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.EuropaPort = Convert.ToInt32(europaPortNumber_NumericUpDown.Value);
            _activityData.RequirePrintValidation = validatePrint_CheckBox.Checked ? true : false;
            _activityData.PrintDriver = printDriverSelector1.DriverPackagePath;
            _activityData.PrintDriverModel = printDriverSelector1.DriverModel;
            _activityData.PhysicalMachineIp = packetCaptureMachine_IpAddressControl.Text;

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
                _activityData.SubSha2ServerIp = subSha2_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, RADIUS_SERVER_SUFFIX + 2);
                _activityData.DhcpServerIp = "{0}.{1}".FormatWith(serverPrefix, DHCP_SERVER_SUFFIX);
                packetCaptureMachine_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, PHYSICAL_MACHINE_SUFFIX);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("IP Address"));
        }

        private void RadiusServer_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkBox = (RadioButton)sender;

            if (checkBox.Name == rootSha1_RadioButton.Name)
            {
                rootSha1_IpAddressControl.Enabled = checkBox.Checked;
                _activityData.RadiusServerType = RadiusServerTypes.RootSha1;

            }
            else if (checkBox.Name == rootSha2_RadioButton.Name)
            {
                rootSha2_IpAddressControl.Enabled = checkBox.Checked;
                _activityData.RadiusServerType = RadiusServerTypes.RootSha2;
            }
            else
            {
                subSha2_IpAddressControl.Enabled = checkBox.Checked;
                rootSha2_IpAddressControl.Enabled = checkBox.Checked;
                _activityData.RadiusServerType = RadiusServerTypes.SubSha2;
            }

            rootSha2_IpAddressControl.Enabled = rootSha2_RadioButton.Checked | subSha2_RadioButton.Checked;

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Radius Server Type"));
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
		void DotOneXEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = productFamily;
            sitemapVersionSelector.PrinterName = productName;

            printDriverSelector1.PrinterFamily = productFamily;
            printDriverSelector1.PrinterName = productName;

            if (base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()) || base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
            {
                subSha2_RadioButton.Enabled = false;
                subSha2_IpAddressControl.Enabled = false;
            }
            else
            {
                subSha2_RadioButton.Enabled = true;
                subSha2_IpAddressControl.Enabled = true;
            }

            if (base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
            {
                secondaryWired_IpAddressControl.Enabled = true;
                europaPortNumber_NumericUpDown.Enabled = true;
            }
            else
            {
                secondaryWired_IpAddressControl.Enabled = false;
                europaPortNumber_NumericUpDown.Enabled = false;
            }
        }

        #endregion
    }
}
