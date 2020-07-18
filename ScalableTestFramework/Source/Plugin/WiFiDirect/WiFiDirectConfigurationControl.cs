using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    /// <summary>
    /// Edit control for a IPSecurity Activity Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class WiFiDirectConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private WiFiDirectActivityData _activityData;

        private const string ServerIpFormat = "{0}.254";
        private const int RadiusServerSuffix = 240;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public WiFiDirectConfigurationControl()
        {
            InitializeComponent();
            ap1Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            ap2Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            ap3Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            switchDetailsControl.HidePortNumber = true;
            //ap1Model_ComboBox.DataSource = ap2Model_ComboBox.DataSource = ap3Model_ComboBox.DataSource = 
            //ap2Model_ComboBox.DataSource = Enum<AccessPointManufacturer>.Value()

            ap1Vendor_ComboBox.SelectedItem = AccessPointManufacturer.Cisco;

            // Initialize
            _activityData = new WiFiDirectActivityData();

            OnProductNameChanged += WirelessConfigurationControl_OnProductNameChanged;
           // sitemapVersionSelector.PrinterFamily = ProductCategory.ToString();
           // sitemapVersionSelector.PrinterName = ProductName;

           // print_PrintDriverSelector.PrinterFamily = base.ProductCategory.ToString();
           // print_PrintDriverSelector.PrinterName = base.ProductName;

            printerDetails1.HideWirelessInterfaceAddress = ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet;
            printerDetails1.HideMacAddress = ProductCategory != ProductFamilies.TPS && ProductCategory != ProductFamilies.InkJet;
            printerDetails1.PrinterInterface = ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet ? InterfaceMode.EmbeddedWired : InterfaceMode.Wireless;
            printerDetails1.PrinterFamily = (PrinterFamilies)Enum<PrinterFamilies>.Parse(base.ProductCategory.ToString());

            printerDetails1.HidePrimaryInterfacePortNumber = ProductCategory == ProductFamilies.VEP;

            printerDetails1.PropertyChanged += PrinterDetails1_PropertyChanged;
            switchDetailsControl.Enabled = base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) ? false : true;
            //printerDetails1.PrinterFamily = (PrinterFamilies)Enum<PrinterFamilies>.Parse(base.ProductCategory.ToString());
        }

        private void PrinterDetails1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IP Address")
            {
                IPAddress address = null;

                if (IPAddress.TryParse(printerDetails1.PrimaryInterfaceAddress, out address))
                {
                    string serverPrefix = address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase));

                    rootSha1_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, RadiusServerSuffix);
                    rootSha2_IpAddressControl.Text = "{0}.{1}".FormatWith(serverPrefix, RadiusServerSuffix + 1);
                }
            }
        }

        private void WirelessConfigurationControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = base.ProductName;

            print_PrintDriverSelector.PrinterFamily = base.ProductCategory.ToString();
            print_PrintDriverSelector.PrinterName = base.ProductName;

            printerDetails1.HideWirelessInterfaceAddress = productFamily == PrinterFamilies.TPS.ToString() ? true : false;
            printerDetails1.HideMacAddress = productFamily == PrinterFamilies.TPS.ToString() || productFamily == PrinterFamilies.InkJet.ToString() ? false : true;

            printerDetails1.PrinterInterface = (base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())) ? InterfaceMode.EmbeddedWired : InterfaceMode.Wireless;
            printerDetails1.PrinterFamily = (PrinterFamilies)Enum<PrinterFamilies>.Parse(base.ProductCategory.ToString());
            printerDetails1.HidePrimaryInterfacePortNumber = base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) ? true : false;

            switchDetailsControl.Enabled = base.ProductCategory.ToString().EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) ? false : true;
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new WiFiDirectActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<WiFiDirectActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();
            _activityData.SelectedTests = SelectedTests;
            _activityData.ProductFamily = ProductCategory;
            _activityData.ProductName = ProductName;
            //_activityData.WirelessInterfaceAddress = printerDetails1.WirelessAddress;
            _activityData.PrimaryInterfaceAddress = printerDetails1.PrimaryInterfaceAddress;
            _activityData.PrimaryInterfaceAddressPortNumber = printerDetails1.PrimaryInterfacePortNumber;

            _activityData.SwitchIpAddress = IPAddress.Parse(switchDetailsControl.SwitchIPAddress);
            _activityData.AccessPointVendor = ap1Vendor_ComboBox.SelectedItem.ToString();
            _activityData.DriverPath = print_PrintDriverSelector.DriverPackagePath;
            _activityData.DriverModel = print_PrintDriverSelector.DriverModel;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SitemapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.PrinterInterfaceType = printerDetails1.PrinterInterfaceType;

            _activityData.AccessPointDetails.Clear();

            if (accessPoint1_IpAddressControl.IsValidIPAddress())
            {
                if (!_activityData.AccessPointDetails.Any(x => x.Address == (IPAddress.Parse(accessPoint1_IpAddressControl.Text))))
                {
                    _activityData.AccessPointDetails.Add(new AccessPointInfo()
                    {
                        Address = IPAddress.Parse(accessPoint1_IpAddressControl.Text),
                        Vendor = (AccessPointManufacturer)ap1Vendor_ComboBox.SelectedItem,
                        Model = ap1Model_ComboBox.SelectedItem.ToString()
                    });
                }
            }

            if (accessPoint2_IpAddressControl.IsValidIPAddress())
            {
                if (!_activityData.AccessPointDetails.Any(x => x.Address == (IPAddress.Parse(accessPoint2_IpAddressControl.Text))))
                {
                    _activityData.AccessPointDetails.Add(new AccessPointInfo()
                    {
                        Address = IPAddress.Parse(accessPoint2_IpAddressControl.Text),
                        Vendor = (AccessPointManufacturer)ap2Vendor_ComboBox.SelectedItem,
                        Model = ap2Model_ComboBox.SelectedItem.ToString()
                    });
                }
            }

            if (accessPoint3_IpAddressControl.IsValidIPAddress())
            {
                if (!_activityData.AccessPointDetails.Any(x => x.Address == (IPAddress.Parse(accessPoint3_IpAddressControl.Text))))
                {
                    _activityData.AccessPointDetails.Add(new AccessPointInfo()
                    {
                        Address = IPAddress.Parse(accessPoint3_IpAddressControl.Text),
                        Vendor = (AccessPointManufacturer)ap3Vendor_ComboBox.SelectedItem,
                        Model = ap3Model_ComboBox.SelectedItem.ToString()
                    });
                }
            }

            _activityData.EnableDebug = debug_CheckBox.Checked;

            //_activityData.PrinterConfiurationDetails = printerDetails1;
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
            ProductCategory = _activityData.ProductFamily;

            ProductName = _activityData.ProductName;
            LoadUIControls();
            sitemapVersionSelector.PrinterFamily = ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = ProductName;

            print_PrintDriverSelector.PrinterFamily = base.ProductCategory.ToString();
            print_PrintDriverSelector.PrinterName = base.ProductName;

            printerDetails1.PrimaryInterfaceAddress = _activityData.PrimaryInterfaceAddress;
            //printerDetails1.WirelessAddress = _activityData.WirelessInterfaceAddress;
            printerDetails1.PrinterInterface = InterfaceMode.Wireless;
            printerDetails1.PrimaryInterfacePortNumber = _activityData.PrimaryInterfaceAddressPortNumber;

            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIpAddress.ToString();
            print_PrintDriverSelector.DriverPackagePath = _activityData.DriverPath;
            print_PrintDriverSelector.DriverModel = _activityData.DriverModel;
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SitemapVersion;
            printerDetails1.PrinterInterfaceType = _activityData.PrinterInterfaceType;

            if (_activityData.AccessPointDetails.Count >= 1)
            {
                accessPoint1_IpAddressControl.Text = _activityData.AccessPointDetails[0].Address.ToString();
                ap1Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[0].Vendor;
                ap1Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[0].Model;
            }

            if (_activityData.AccessPointDetails.Count >= 2)
            {
                accessPoint2_IpAddressControl.Text = _activityData.AccessPointDetails[1].Address.ToString();
                ap2Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[1].Vendor;
                ap2Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[1].Model;
            }

            if (_activityData.AccessPointDetails.Count == 3)
            {
                accessPoint3_IpAddressControl.Text = _activityData.AccessPointDetails[2].Address.ToString();
                ap2Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[2].Vendor;
                ap2Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[2].Model;
            }

            debug_CheckBox.Checked = _activityData.EnableDebug;
            LoadTestCases(typeof(WiFiDirectTests), _activityData.SelectedTests, PluginType.Default);
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

        private void accessPointVendor_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox currentControl = (ComboBox)sender;
            ComboBox currentModel = ap1Model_ComboBox;

            if (currentControl.Name == ap1Vendor_ComboBox.Name)
            {
                currentModel = ap1Model_ComboBox;
            }
            else if (currentControl.Name == ap2Vendor_ComboBox.Name)
            {
                currentModel = ap2Model_ComboBox;
            }
            else
            {
                currentModel = ap3Model_ComboBox;
            }

            List<string> apModels = new List<string>();

            DescriptionAttribute attributes = (DescriptionAttribute)typeof(AccessPointManufacturer).GetMember(currentControl.SelectedItem.ToString()).FirstOrDefault().GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            Array.ForEach(attributes.Description.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), x => apModels.Add(x));

            currentModel.DataSource = apModels;
        }

        private void debug_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _activityData.EnableDebug = ((CheckBox)sender).Checked;
        }
    }
}