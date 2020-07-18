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
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.Plugin.Wireless
{
    /// <summary>
    /// Edit control for a Wireless Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class WirelessConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variables

        /// <summary>
        /// Activity Data
        /// </summary>
        private WirelessActivityData _activityData;
        private const int RadiusServerSuffix = 240;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize Plug-in Pre-requisites
        /// </summary>
        public WirelessConfigurationControl()
        {
            InitializeComponent();
            ap1Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            ap2Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            ap3Vendor_ComboBox.DataSource = Enum.GetValues(typeof(AccessPointManufacturer));
            switchDetailsControl.HidePortNumber = false;

			ap1Vendor_ComboBox.SelectedItem = AccessPointManufacturer.Cisco;
            ap2Vendor_ComboBox.SelectedItem = AccessPointManufacturer.Cisco;
            ap3Vendor_ComboBox.SelectedItem = AccessPointManufacturer.Cisco;


          OnProductNameChanged += WirelessConfigurationControl_OnProductNameChanged;
            ////sitemapVersionSelector.PrinterFamily = ProductCategory.ToString();
            ////sitemapVersionSelector.PrinterName = ProductName;

            ////print_PrintDriverSelector.PrinterFamily = ProductCategory.ToString();
            ////print_PrintDriverSelector.PrinterName = ProductName;

		    printerDetails1.HideWirelessInterfaceAddress = ProductCategory == ProductFamilies.TPS ||
		                                                   ProductCategory == ProductFamilies.InkJet;
            printerDetails1.HideMacAddress = ProductCategory == ProductFamilies.VEP;
			printerDetails1.PrinterInterface = ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet ? InterfaceMode.EmbeddedWired : InterfaceMode.Wireless;
			printerDetails1.PrinterFamily = Enum<PrinterFamilies>.Parse(ProductCategory.ToString());

            printerDetails1.HidePrimaryInterfacePortNumber = ProductCategory == ProductFamilies.VEP;

            printerDetails1.PropertyChanged += PrinterDetails1_PropertyChanged;
            switchDetailsControl.Enabled = ProductCategory != ProductFamilies.VEP;

            fieldValidator.RequireCustom(sitemapVersionSelector, sitemapVersionSelector.ValidateControls);
			
		}

        private void PrinterDetails1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IP Address")
            {
                IPAddress address;

                if (IPAddress.TryParse(printerDetails1.PrimaryInterfaceAddress, out address))
                {
                    string serverPrefix = address.ToString().Substring(0, address.ToString().LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase));
                    rootSha1_IpAddressControl.Text = $"{serverPrefix}.{RadiusServerSuffix}";
                    rootSha2_IpAddressControl.Text = $"{serverPrefix}.{RadiusServerSuffix + 1}";
                }
            }
        }

        private void WirelessConfigurationControl_OnProductNameChanged(string productFamily, string productName)
        {
            sitemapVersionSelector.PrinterFamily = ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = ProductName;

            print_PrintDriverSelector.PrinterFamily = ProductCategory.ToString();
            print_PrintDriverSelector.PrinterName = ProductName;

            printerDetails1.HideWirelessInterfaceAddress = productFamily == PrinterFamilies.TPS.ToString();
            printerDetails1.HideMacAddress = productFamily != PrinterFamilies.TPS.ToString() && productFamily != PrinterFamilies.InkJet.ToString();

            printerDetails1.PrinterInterface = ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet ? InterfaceMode.EmbeddedWired : InterfaceMode.Wireless;
            printerDetails1.PrinterFamily = Enum<PrinterFamilies>.Parse(ProductCategory.ToString());
            printerDetails1.HidePrimaryInterfacePortNumber = ProductCategory == ProductFamilies.VEP;
                     
            if (ProductCategory != ProductFamilies.VEP)
            {
                switchDetailsControl.Enabled = true;
                portNo_Label.Enabled = true;
            }
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new WirelessActivityData();
            CtcSettings.Initialize(environment);
            LoadUi();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<WirelessActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests = SelectedTests;
            _activityData.ProductFamily = ProductCategory;
            _activityData.ProductName = ProductName;
            _activityData.WirelessInterfaceAddress = printerDetails1.WirelessAddress;
            _activityData.PrimaryInterfaceAddress = printerDetails1.PrimaryInterfaceAddress;
            _activityData.PrimaryInterfaceAddressPortNumber = printerDetails1.PrimaryInterfacePortNumber;

            if (ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet)
            {
                _activityData.WirelessMacAddress = printerDetails1.MacAddress;
            }

            _activityData.SwitchIpAddress = IPAddress.Parse(switchDetailsControl.SwitchIPAddress);
            _activityData.AccessPointVendor = ap1Vendor_ComboBox.SelectedItem.ToString();
            _activityData.DriverPath = print_PrintDriverSelector.DriverPackagePath;
			_activityData.DriverModel = print_PrintDriverSelector.DriverModel;
            _activityData.SitemapPath = sitemapVersionSelector.SitemapPath;
            _activityData.SitemapVersion = sitemapVersionSelector.SitemapVersion;
            _activityData.PrinterInterfaceType = printerDetails1.PrinterInterfaceType;
            _activityData.RadiusServerType = rootSha1_RadioButton.Checked ? RadiusServerTypes.RootSha1 : RadiusServerTypes.RootSha2;
            _activityData.RootSha1ServerIp = rootSha1_IpAddressControl.Text;
            _activityData.RootSha2ServerIp = rootSha2_IpAddressControl.Text;
            _activityData.RadiusServerIp = _activityData.RadiusServerType == RadiusServerTypes.RootSha1 ? rootSha1_IpAddressControl.Text : rootSha2_IpAddressControl.Text;

            _activityData.PowerSwitchIpAddress = powerSwitch_IpAddressControl.Text;

			_activityData.AccessPointDetails.Clear();
			
			if (ap1_IpAddressControl.IsValidIPAddress())
			{
				if (_activityData.AccessPointDetails.All(x => !Equals(x.Address, IPAddress.Parse(ap1_IpAddressControl.Text))))
				{
                    _activityData.AccessPointDetails.Add(new AccessPointInfo
                    {
                        Address = IPAddress.Parse(ap1_IpAddressControl.Text),
                        Vendor = (AccessPointManufacturer)ap1Vendor_ComboBox.SelectedItem,
                        Model = ap1Model_ComboBox.SelectedItem.ToString(),
                        UserName = ap1Username_TextBox.Text,
                        password = ap1Password_TextBox.Text,
                        PortNumber = (int)ap1PortNo_NumericUpDown.Value
					});
				}
			}

			if (ap2_IpAddressControl.IsValidIPAddress())
			{
				if (_activityData.AccessPointDetails.All(x => !Equals(x.Address, IPAddress.Parse(ap2_IpAddressControl.Text))))
				{
					_activityData.AccessPointDetails.Add(new AccessPointInfo
					{
						Address = IPAddress.Parse(ap2_IpAddressControl.Text),
						Vendor = (AccessPointManufacturer)ap2Vendor_ComboBox.SelectedItem,
						Model = ap2Model_ComboBox.SelectedItem.ToString(),
                        UserName = ap2Username_TextBox.Text,
                        password = ap2Password_TextBox.Text,
                        PortNumber = (int)ap2PortNo_NumericUpDown.Value
                    });
				}
			}

            if (ap3_IpAddressControl.IsValidIPAddress())
            {
                if (_activityData.AccessPointDetails.All(x => !Equals(x.Address, IPAddress.Parse(ap3_IpAddressControl.Text))))
                {
                    _activityData.AccessPointDetails.Add(new AccessPointInfo
                    {
                        Address = IPAddress.Parse(ap3_IpAddressControl.Text),
                        Vendor = (AccessPointManufacturer)ap3Vendor_ComboBox.SelectedItem,
                        Model = ap3Model_ComboBox.SelectedItem.ToString(),
                        UserName = ap3Username_TextBox.Text,
                        password = ap3Password_TextBox.Text,
                        PortNumber = (int)ap3PortNo_NumericUpDown.Value
                    });
                }
            }

            _activityData.EnableDebug = debug_CheckBox.Checked;

            if (frequency_ComboBox.SelectedIndex == 0)
            {
                _activityData.WirelessRadio = Radio._2dot4Ghz;
            }
            else
            {
                _activityData.WirelessRadio = Radio._5Ghz;
            }

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
        private void LoadUi()
        {
            LoadUIControls();

            ProductCategory = _activityData.ProductFamily;
            sitemapVersionSelector.Initialize();
            sitemapVersionSelector.PrinterFamily = ProductCategory.ToString();
            sitemapVersionSelector.PrinterName = ProductName;
            
            print_PrintDriverSelector.Initialize();
            print_PrintDriverSelector.PrinterFamily = ProductCategory.ToString();
            print_PrintDriverSelector.PrinterName = ProductName;

            ProductName = _activityData.ProductName;
            printerDetails1.PrimaryInterfaceAddress = _activityData.PrimaryInterfaceAddress;
            printerDetails1.WirelessAddress = _activityData.WirelessInterfaceAddress;
            printerDetails1.PrinterInterface = InterfaceMode.Wireless;
            printerDetails1.PrimaryInterfacePortNumber = _activityData.PrimaryInterfaceAddressPortNumber;

            if (ProductCategory == ProductFamilies.TPS || ProductCategory == ProductFamilies.InkJet)
            {
                printerDetails1.MacAddress = _activityData.WirelessMacAddress;
            }

            LoadTestCases(typeof(WirelessTests), _activityData.SelectedTests);
            switchDetailsControl.SwitchPortNumber = _activityData.PrimaryInterfaceAddressPortNumber;
            switchDetailsControl.SwitchIPAddress = _activityData.SwitchIpAddress.ToString();
            print_PrintDriverSelector.DriverPackagePath = _activityData.DriverPath;
			print_PrintDriverSelector.DriverModel = _activityData.DriverModel;
            sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemapVersionSelector.SitemapVersion = _activityData.SitemapVersion;
            printerDetails1.PrinterInterfaceType = _activityData.PrinterInterfaceType;

            if (_activityData.RadiusServerType == RadiusServerTypes.RootSha1)
            {
                rootSha1_RadioButton.Checked = true;
            }
            else if (_activityData.RadiusServerType == RadiusServerTypes.RootSha2)
            {
                rootSha2_RadioButton.Checked = true;
            }

            rootSha1_IpAddressControl.Text = _activityData.RootSha1ServerIp;
            rootSha2_IpAddressControl.Text = _activityData.RootSha2ServerIp;

			if (_activityData.AccessPointDetails.Count >= 1)
			{
				ap1_IpAddressControl.Text = _activityData.AccessPointDetails[0].Address.ToString();
				ap1Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[0].Vendor;
				ap1Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[0].Model;
                ap1Username_TextBox.Text = _activityData.AccessPointDetails[0].UserName;
                ap1Password_TextBox.Text = _activityData.AccessPointDetails[0].UserName;
                ap1PortNo_NumericUpDown.Value = _activityData.AccessPointDetails[0].PortNumber;
            }

			if (_activityData.AccessPointDetails.Count >= 2)
			{
				ap2_IpAddressControl.Text = _activityData.AccessPointDetails[1].Address.ToString();
				ap2Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[1].Vendor;
				ap2Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[1].Model;
                ap2Username_TextBox.Text = _activityData.AccessPointDetails[1].UserName;
                ap2Password_TextBox.Text = _activityData.AccessPointDetails[1].UserName;
                ap2PortNo_NumericUpDown.Value = _activityData.AccessPointDetails[1].PortNumber;
            }

            if (_activityData.AccessPointDetails.Count >= 3)
            {
                ap3_IpAddressControl.Text = _activityData.AccessPointDetails[2].Address.ToString();
                ap3Vendor_ComboBox.SelectedItem = _activityData.AccessPointDetails[2].Vendor;
                ap3Model_ComboBox.SelectedItem = _activityData.AccessPointDetails[2].Model;
                ap3Username_TextBox.Text = _activityData.AccessPointDetails[2].UserName;
                ap3Password_TextBox.Text = _activityData.AccessPointDetails[2].UserName;
                ap3PortNo_NumericUpDown.Value = _activityData.AccessPointDetails[2].PortNumber;
            }

            powerSwitch_IpAddressControl.Text = _activityData.PowerSwitchIpAddress;
            debug_CheckBox.Checked = _activityData.EnableDebug;

            if (_activityData.WirelessRadio == Radio._2dot4Ghz)
            {
                frequency_ComboBox.SelectedIndex = 0;
            }
            else if (_activityData.WirelessRadio == Radio._2dot4Ghz)
            {
                frequency_ComboBox.SelectedIndex = 1;
            }
            else
            {
                frequency_ComboBox.SelectedIndex = 0;
            }

           // LoadTestCases(typeof(WirelessTests), _activityData.SelectedTests);
        }

        #endregion

        #region Events

        #endregion

        private void rootSha1_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            {
                RadioButton checkBox = (RadioButton)sender;

                if (checkBox.Name == rootSha1_RadioButton.Name)
                {
                    rootSha1_IpAddressControl.Enabled = checkBox.Checked;
                    _activityData.RadiusServerType = RadiusServerTypes.RootSha1;

                }
                else
                {
                    rootSha2_IpAddressControl.Enabled = checkBox.Checked;
                    _activityData.RadiusServerType = RadiusServerTypes.RootSha2;
                }

                rootSha2_IpAddressControl.Enabled = rootSha2_RadioButton.Checked;

                OnPropertyChanged(new PropertyChangedEventArgs("Radius Server Type"));
            }
        }

        private void accessPointVendor_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox currentControl = (ComboBox)sender;
            ComboBox currentModel;

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

            DescriptionAttribute attributes = (DescriptionAttribute)typeof(AccessPointManufacturer).GetMember(currentControl.SelectedItem.ToString()).FirstOrDefault()?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            if (attributes != null)
                Array.ForEach(attributes?.Description.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    x => apModels.Add(x));

            currentModel.DataSource = apModels;
        }

        private void debug_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _activityData.EnableDebug = ((CheckBox)sender).Checked;
        }
    }
}