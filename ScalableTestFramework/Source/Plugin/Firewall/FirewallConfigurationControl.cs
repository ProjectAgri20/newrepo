using System;
using System.ComponentModel;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.Firewall
{
    /// <summary>
    /// Edit control for Firewall plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class FirewallConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private FirewallActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        #endregion Local Variables

        #region Constructor

        /// <summary>
        /// Edit Control Constructor
        /// </summary>
        public FirewallConfigurationControl()
        {
            InitializeComponent();

            // bind the product name change event
            base.OnProductNameChanged += new ProductNameChangedEventHandler(FirewallEditControl_OnProductNameChanged);
            //Moving to Load ui
            //firewall_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //firewall_SitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(ipv4Address_IPAddressControl, ipv4Address_IPAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(firewall_SitemapVersionSelector, firewall_SitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(firewall_PrintDriverDetails, firewall_PrintDriverDetails.ValidateControls);
            fieldValidator.RequireCustom(ssid_TextBox, () => (ProductCategory == ProductFamilies.TPS ? !string.IsNullOrEmpty(ssid_TextBox.Text) : true), "Enter a value for ssid.");
            fieldValidator.RequireCustom(firewall_SwitchDetailsControl, () => (ProductCategory == ProductFamilies.TPS ? !string.IsNullOrEmpty(firewall_SwitchDetailsControl.SwitchIPAddress) : true), "Enter a value for Switch Ip Address.");
            fieldValidator.RequireCustom(wirelessMacAddress_TextBox, () => (ProductCategory == ProductFamilies.TPS ? wirelessMacAddress_TextBox.Text.Length == 12 : true), "Enter a valid mac address having 12 characters.");
            fieldValidator.RequireCustom(v4_CheckBox, () => (v4_CheckBox.Checked || linkLocal_CheckBox.Checked || stateFull_CheckBox.Checked || stateLess_CheckBox.Checked), "At least one IP address must be selected.");

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            firewall_SitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            firewall_PrintDriverDetails.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipv4Address_IPAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
            firewall_SwitchDetailsControl.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            wirelessMacAddress_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            ssid_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion Constructor

        #region Events

        void FirewallEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            firewall_SitemapVersionSelector.PrinterFamily = productFamily;
            firewall_SitemapVersionSelector.PrinterName = productName;

            firewall_PrintDriverDetails.PrinterFamily = productFamily;
            firewall_PrintDriverDetails.PrinterName = productName;

            if (ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet)
            {
                wirelessDetails_GroupBox.Enabled = true;
                wirelessMacAddress_TextBox.Enabled = true;
                wirelessMac_Label.Visible = true;
                wirelessMacAddress_TextBox.Visible = true;
                firewall_SwitchDetailsControl.Enabled = true;
            }
            else
            {
                wirelessDetails_GroupBox.Enabled = false;
                wirelessMacAddress_TextBox.Enabled = false;
                wirelessMac_Label.Visible = false;
                wirelessMacAddress_TextBox.Visible = false;
                firewall_SwitchDetailsControl.Enabled = false;
            }
        }

        /// <summary>
        /// Validation for wireless mac address for TPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wirelessMacAddress_TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (base.ProductCategory == ProductFamilies.TPS)
            {
                fieldValidator.Validate(wirelessMacAddress_TextBox);
            }
        }

        /// <summary>
        /// Validation for SSID for TPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssid_TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (base.ProductCategory == ProductFamilies.TPS)
            {
                fieldValidator.Validate(ssid_TextBox);
            }
        }

        /// <summary>
        /// Occurs on checked change event of radio button.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void radioButton_Wired_CheckedChanged_1(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Connectivity"));
        }

        private void LoadUI()
        {
            LoadUIControls();

            firewall_SitemapVersionSelector.Initialize();
            firewall_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            firewall_SitemapVersionSelector.PrinterName = base.ProductName;

            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }
            else
            {
                ProductCategory = ProductFamilies.None;
            }

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(FirewallTests), _activityData.SelectedTests);

            // Initialize the Printer Driver path 
            firewall_PrintDriverDetails.Initialize();

            ipv4Address_IPAddressControl.Text = _activityData.IPv4Address.ToString();
            firewall_SitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            firewall_SitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            firewall_PrintDriverDetails.DriverPackagePath = _activityData.PrintDriver;
            firewall_PrintDriverDetails.DriverModel = _activityData.PrintDriverModel;

            wirelessMacAddress_TextBox.Text = _activityData.WiredMacAddress;
            firewall_SwitchDetailsControl.SwitchIPAddress = _activityData.SwitchIP;
            firewall_SwitchDetailsControl.SwitchPortNumber = _activityData.PortNo;
            ssid_TextBox.Text = _activityData.SsidName;

            if (_activityData.PrinterConnectivity == ConnectivityType.Wired || _activityData.PrinterConnectivity == ConnectivityType.None)
            {
                radioButton_Wired.Checked = true;
            }
            else if (_activityData.PrinterConnectivity == ConnectivityType.Wireless)
            {
                radioButton_Wireless.Checked = true;
            }

            v4_CheckBox.Checked = _activityData.IPv4Enable;
            linkLocal_CheckBox.Checked = _activityData.LinkLocal;
            stateFull_CheckBox.Checked = _activityData.Stateful;
            stateLess_CheckBox.Checked = _activityData.Stateless;
            debug_CheckBox.Checked = _activityData.Debug;
        }

        #endregion Events

        #region IPluginConfigurationControl Implementation
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new FirewallActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<FirewallActivityData>(CtcMetadataConverter.Converters);
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
            _activityData.IPv4Address = ipv4Address_IPAddressControl.Text;
            _activityData.SitemapPath = firewall_SitemapVersionSelector.SitemapPath;
            _activityData.SitemapsVersion = firewall_SitemapVersionSelector.SitemapVersion;
            _activityData.PrintDriver = firewall_PrintDriverDetails.DriverPackagePath;
            _activityData.PrintDriverModel = firewall_PrintDriverDetails.DriverModel;
            _activityData.IPv6LinkLocalAddress = string.Empty;
            _activityData.IPv6StatefulAddress = string.Empty;
            _activityData.IPv6StatelessAddress = string.Empty;
            _activityData.WiredMacAddress = wirelessMacAddress_TextBox.Text;
            _activityData.SwitchIP = firewall_SwitchDetailsControl.SwitchIPAddress;
            _activityData.PortNo = firewall_SwitchDetailsControl.SwitchPortNumber;
            _activityData.SsidName = ssid_TextBox.Text;
            _activityData.PrinterConnectivity = radioButton_Wired.Checked ? ConnectivityType.Wired : ConnectivityType.Wireless;
            _activityData.LinkLocal = linkLocal_CheckBox.Checked;
            _activityData.Stateful = stateFull_CheckBox.Checked;
            _activityData.Stateless = stateLess_CheckBox.Checked;
            _activityData.IPv4Enable = v4_CheckBox.Checked;
            _activityData.Debug = debug_CheckBox.Checked;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(firewall_SitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion IPluginEditControl Implemenation

        private void debug_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("You have selected Debug Option which will halt the Test excution to verify the results manually.\nUser need to be available to continue excution", "NOTE");
            return;
        }
    }
}
