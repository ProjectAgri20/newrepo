using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    /// <summary>
    /// Edit control for a Print Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class ConnectivityPrintConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Constants

        /// <summary>
        /// Directory name for the documents.
        /// </summary>
        public const string DIRECTORY_DOCUMENTS = "Documents";
        public const string COMMONPATH = "Common";
        public const string DIRECTORY_DRIVERS = "Drivers";

        #endregion

        #region Local Variables

        private ConnectivityPrintActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectivityPrintConfigurationControl"/> class.
        /// </summary>
        public ConnectivityPrintConfigurationControl()
        {
            InitializeComponent();

            _activityData = new ConnectivityPrintActivityData();

            base.OnProductNameChanged += PrintEditControl_OnProductNameChanged;

            //sitemaps_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
           // sitemaps_SitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(ipv4Address_IpAddressControl, ipv4Address_IpAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(sitemaps_SitemapVersionSelector, sitemaps_SitemapVersionSelector.ValidateControls);
            fieldValidator.RequireCustom(printSwitchDetails_SwtchDetails, printSwitchDetails_SwtchDetails.ValidateControls);
            fieldValidator.RequireCustom(print_PrintDriverSelector, print_PrintDriverSelector.ValidateControls);

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            sitemaps_SitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            print_PrintDriverSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            printSwitchDetails_SwtchDetails.PropertyChanged += (s, e) => ConfigurationChanged(s, e);

            paperlessMode_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            documentsPath_ComboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            ipv4Address_IpAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);

        }

        #endregion

        #region Events

        /// <summary>
        /// Loads the test cases and UI controls on Load event of the ConnectivityPrintEditControl
        /// </summary>
        private void LoadUI()
        {

            LoadUIControls();
            ProductCategory = _activityData.ProductFamily;
            ProductName = _activityData.ProductName;

            sitemaps_SitemapVersionSelector.Initialize();
            sitemaps_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            sitemaps_SitemapVersionSelector.PrinterName = base.ProductName;
            
            LoadTestCases(typeof(ConnectivityPrintTests), _activityData.SelectedTests, PluginType.Print, _activityData.TestDetails);

            ipv4Address_IpAddressControl.Text = _activityData.Ipv4Address;

            if (_activityData.Ipv6AddressTypes.Count == 3)
            {
                if (_activityData.Ipv6AddressTypes[0] != Ipv6AddressTypes.None)
                {
                    linkLocal_CheckBox.Checked = true;
                }

                if (_activityData.Ipv6AddressTypes[1] != Ipv6AddressTypes.None)
                {
                    stateLess_CheckBox.Checked = true;
                }

                if (_activityData.Ipv6AddressTypes[2] != Ipv6AddressTypes.None)
                {
                    stateFull_CheckBox.Checked = true;
                }
            }

            // Initialize the Printer Driver path 
            print_PrintDriverSelector.Initialize();
            if (!string.IsNullOrEmpty(_activityData.DriverPackagePath))
            {
                print_PrintDriverSelector.DriverPackagePath = _activityData.DriverPackagePath;
                print_PrintDriverSelector.DriverModel = _activityData.DriverModel;
            }
            else
            {
                print_PrintDriverSelector.DriverPackagePath = CtcSettings.ConnectivityShare + Path.DirectorySeparatorChar + ProductCategory + Path.DirectorySeparatorChar + ProductName + Path.DirectorySeparatorChar + DIRECTORY_DRIVERS;
            }

            // load documents path
            LoadDocumentsPath();

            if (!string.IsNullOrEmpty(_activityData.DocumentsPath))
            {
                documentsPath_ComboBox.SelectedIndex = documentsPath_ComboBox.FindStringExact(new DirectoryInfo(_activityData.DocumentsPath).Name);
            }

            if (_activityData.PrinterConnectivity == ConnectivityType.Wired)
            {
                wired_RadioButton.Checked = true;
            }
            else
            {
                wireless_RadioButton.Checked = true;
            }

            // Load switch details
            printSwitchDetails_SwtchDetails.SwitchIPAddress = _activityData.SwitchIPAddress;
            printSwitchDetails_SwtchDetails.SwitchPortNumber = _activityData.SwitchPortNumber;
            sitemaps_SitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            sitemaps_SitemapVersionSelector.SitemapVersion = _activityData.SiteMapVersion;
            paperlessMode_CheckBox.Checked = _activityData.PaperlessMode;
            this.printSwitchDetails_SwtchDetails.ValidationRequired = false;
        }

        /// <summary>
        /// Sets the document path for the print documents on product name changed event
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        private void PrintEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            print_PrintDriverSelector.PrinterFamily = productFamily;
            print_PrintDriverSelector.PrinterName = productName;
            LoadDocumentsPath();

            sitemaps_SitemapVersionSelector.PrinterFamily = productFamily;
            sitemaps_SitemapVersionSelector.PrinterName = productName;
        }

        /// <summary>
        /// Occurs when the radio button checked changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs("Connectivity"));
        }

        /// <summary>
        /// Occurs when the checkbox checked changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs("IPv6 Address Type"));
        }

        /// <summary>
        /// Occurs when the WSP tests selected
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void ConnectivityPrintConfigurationControl_Validating(object sender, CancelEventArgs e)
        {
            ValidateSelectedTestInputs();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the documentsPath
        /// </summary>
        private void LoadDocumentsPath()
        {
            documentsPath_ComboBox.Items.Clear();

            string documentLocation = Path.Combine(CtcSettings.ConnectivityShare, ProductCategory.ToString());
            documentsPath_ComboBox.Tag = documentLocation;
            toolTip.SetToolTip(documentsPath_ComboBox, documentsPath_ComboBox.Tag.ToString());

            if (Directory.Exists(documentLocation))
            {
                List<string> documentPath = Directory.EnumerateDirectories(documentLocation).ToList();

                foreach (string directory in documentPath)
                {
                    if (Directory.Exists(directory + Path.DirectorySeparatorChar))
                    {
                        documentsPath_ComboBox.Items.Add(new DirectoryInfo(directory).Name);
                    }
                }

                // select the "common" directory as default
                if (documentsPath_ComboBox.FindStringExact(COMMONPATH) != -1)
                {
                    documentsPath_ComboBox.SelectedIndex = documentsPath_ComboBox.FindStringExact(COMMONPATH);
                }

            }
        }

        /// <summary>
        /// Verifies if all the document paths defined in FolderType enum are available in the selected document path
        /// </summary>
        private bool ValidateDocumentPath()
        {
            List<string> missingDirectories = new List<string>();
            string documentsPath = string.Empty;

            if (documentsPath_ComboBox.Items.Count > 0 && documentsPath_ComboBox.SelectedItem != null)
            {
                documentsPath = Path.Combine(documentsPath_ComboBox.Tag.ToString(), documentsPath_ComboBox.SelectedItem.ToString(), DIRECTORY_DOCUMENTS);

                // Validation for the print documents folder
                foreach (var subFolder in Enum<FolderType>.EnumValues)
                {
                    if (!Directory.Exists(Path.Combine(documentsPath, subFolder.Value)))
                    {
                        missingDirectories.Add(subFolder.Value);
                    }
                }
            }

            if (documentsPath_ComboBox.Items.Count > 0 && null == documentsPath_ComboBox.SelectedItem)
            {
                return false;
            }
            else if (documentsPath_ComboBox.Items.Count == 0 || null == documentsPath_ComboBox.SelectedItem)
            {
                documentsPath = documentsPath_ComboBox.Tag.ToString();
                return false;
            }

            if (missingDirectories.Count == Enum<FolderType>.EnumValues.Count)
            {
                return false;
            }
            else if (missingDirectories.Count > 0)
            {
                return true;
            }

            return true;
        }

        /// <summary>
        /// Validate inputs for the selected tests
        /// </summary>
        private void ValidateSelectedTestInputs()
        {
            bool isIPv6TestSelected = false;
            bool isHoseBreakTestSelected = false;
            _activityData.IsWspTestsSelected = false;

            foreach (int testNumber in base.SelectedTests)
            {
                // Fetch the TestDetailsAttribute of the test method matching the testNumber
                var attributes = typeof(ConnectivityPrintTests).GetMethods().
                    Where(item => item.GetCustomAttributes(new TestDetailsAttribute().GetType(), false).Length > 0
                        && ((TestDetailsAttribute)item.GetCustomAttributes(new TestDetailsAttribute().GetType(), false)[0]).Id.Equals(testNumber))
                        .Select(x => x.GetCustomAttributes(new TestDetailsAttribute().GetType(), false)[0]);

                TestDetailsAttribute testAttributes = (TestDetailsAttribute)attributes.FirstOrDefault();

                if (testAttributes.Category.Contains("WSP", StringComparison.CurrentCultureIgnoreCase))
                {
                    _activityData.IsWspTestsSelected = true;
                }

                if (testAttributes.Description.Contains("hose break", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    isHoseBreakTestSelected = true;
                }

                if (testAttributes.Protocol.Equals(ProtocolType.IPv6))
                {
                    isIPv6TestSelected = true;
                }

                if (isIPv6TestSelected && isHoseBreakTestSelected && _activityData.IsWspTestsSelected)
                {
                    break;
                }
            }

            // Validation for Ipv6 address type selection
            if (isIPv6TestSelected)
            {
                if (!linkLocal_CheckBox.Checked && !stateLess_CheckBox.Checked && !stateFull_CheckBox.Checked)
                {
                    //fieldValidator.SetError(stateFull_CheckBox, "Select atleast one IPv6 Address Type.");
                }
                else
                {
                    //fieldValidator.SetError(stateFull_CheckBox, string.Empty);
                }
            }

            // Validation for Switch details control if hose break tests are selected
            if (isHoseBreakTestSelected)
            {
                printSwitchDetails_SwtchDetails.ValidationRequired = true;
            }
            else
            {
                printSwitchDetails_SwtchDetails.ValidationRequired = false;
            }
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        void IPluginConfigurationControl.Initialize(PluginEnvironment environment)
        {
            _activityData = new ConnectivityPrintActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        void IPluginConfigurationControl.Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<ConnectivityPrintActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        PluginConfigurationData IPluginConfigurationControl.GetConfiguration()
        {
            _activityData.SelectedTests.Clear();

            // set the UI Elements data to the activity data
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.TestDetails = base.TestsWithDuration;
            _activityData.ProductFamily = base.ProductCategory;
            _activityData.ProductName = base.ProductName;
            _activityData.Ipv4Address = ipv4Address_IpAddressControl.Text;
            _activityData.PrinterConnectivity = wired_RadioButton.Checked ? ConnectivityType.Wired : ConnectivityType.Wireless;
            _activityData.SitemapPath = sitemaps_SitemapVersionSelector.SitemapPath;
            _activityData.SiteMapVersion = sitemaps_SitemapVersionSelector.SitemapVersion;
            _activityData.PaperlessMode = paperlessMode_CheckBox.Checked;

            _activityData.Ipv6AddressTypes.Clear();

            if (linkLocal_CheckBox.Checked)
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.LinkLocal);
            }
            else
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.None);
            }

            if (stateLess_CheckBox.Checked)
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.Stateless);
            }
            else
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.None);
            }

            if (stateFull_CheckBox.Checked)
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.Stateful);
            }
            else
            {
                _activityData.Ipv6AddressTypes.Add(Ipv6AddressTypes.None);
            }

            _activityData.DocumentsPath = documentsPath_ComboBox.SelectedItem != null ? documentsPath_ComboBox.SelectedItem.ToString() : string.Empty;
            _activityData.DriverPackagePath = print_PrintDriverSelector.DriverPackagePath;
            _activityData.DriverModel = print_PrintDriverSelector.DriverModel;
            _activityData.SwitchIPAddress = printSwitchDetails_SwtchDetails.SwitchIPAddress;
            _activityData.SwitchPortNumber = printSwitchDetails_SwtchDetails.SwitchPortNumber;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        PluginValidationResult IPluginConfigurationControl.ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(sitemaps_SitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion
    }
}