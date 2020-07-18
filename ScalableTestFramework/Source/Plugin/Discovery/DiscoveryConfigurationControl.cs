using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Discovery
{
    /// <summary>
    /// Control used for editing NetworkDiscovery activities.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DiscoveryConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
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

        /// <summary>
        /// Activity Data
        /// </summary>
        private DiscoveryActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryConfigurationControl"/> class.
        /// </summary>
        public DiscoveryConfigurationControl()
        {
            InitializeComponent();

            _activityData = new DiscoveryActivityData();

            // bind the product name change event
            base.OnProductNameChanged += new ProductNameChangedEventHandler(NetworkDiscoveryEditControl_OnProductNameChanged);

            //discoveryPlugin_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            //discoveryPlugin_SitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(ipv4Address_IPAddressControl, ipv4Address_IPAddressControl.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(discoveryPlugin_SitemapVersionSelector, discoveryPlugin_SitemapVersionSelector.ValidateControls);

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            discoveryPlugin_SitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            ipv4Address_IPAddressControl.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the product name is changed in the base plugin
        /// </summary>
        /// <param name="productFamily"></param>
        /// <param name="productName"></param>
        void NetworkDiscoveryEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            discoveryPlugin_SitemapVersionSelector.PrinterFamily = productFamily;
            discoveryPlugin_SitemapVersionSelector.PrinterName = productName;
            printDriverSelector.PrinterFamily = productFamily;
            printDriverSelector.PrinterName = productName;
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        /// <summary>
        /// Loads the control
        /// </summary>
        private void LoadUI()
        {

            LoadUIControls();
            // load the data from activity data to the UI elements
            if (!string.IsNullOrEmpty(_activityData.ProductFamily))
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductFamily);
            }

            discoveryPlugin_SitemapVersionSelector.Initialize();
            discoveryPlugin_SitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            discoveryPlugin_SitemapVersionSelector.PrinterName = base.ProductName;

            ProductName = _activityData.ProductName;

            LoadTestCases(typeof(DiscoveryTests), _activityData.SelectedTests);

            ipv4Address_IPAddressControl.Text    = _activityData.IPv4Address;
            discoveryPlugin_SitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            discoveryPlugin_SitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
			// Initialize Print driver package Path
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

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new DiscoveryActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<DiscoveryActivityData>(CtcMetadataConverter.Converters);
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

            _activityData.SitemapsVersion = discoveryPlugin_SitemapVersionSelector.SitemapVersion;
            _activityData.SitemapPath = discoveryPlugin_SitemapVersionSelector.SitemapPath;
            _activityData.DriverPackagePath = printDriverSelector.DriverPackagePath;
            _activityData.DriverModel = printDriverSelector.DriverModel;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(discoveryPlugin_SitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion

        private void printDriverSelector_Load(object sender, EventArgs e)
        {

        }
    }
}
