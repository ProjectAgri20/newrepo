using System;
using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    /// <summary>
    /// Edit control for a Telnet SNMP Activity Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class TelnetSnmpConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        private TelnetSnmpActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        #region Constructor and Initialize
        public TelnetSnmpConfigurationControl()
        {
            InitializeComponent();

            _activityData = new TelnetSnmpActivityData();
            base.OnProductNameChanged += new ProductNameChangedEventHandler(TelnetConfigurationControl_OnProductNameChanged);
          //  Telnet_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
          //  Telnet_sitemapVersionSelector.PrinterName = base.ProductName;

            fieldValidator.RequireCustom(textBoxIP, textBoxIP.IsValidIPAddress, "Enter valid IP address");
            fieldValidator.RequireCustom(Telnet_sitemapVersionSelector, Telnet_sitemapVersionSelector.ValidateControls);

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            Telnet_sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Private Methods
        private void LoadUI()
        {

            LoadUIControls();
            Telnet_sitemapVersionSelector.Initialize();
            Telnet_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            Telnet_sitemapVersionSelector.PrinterName = base.ProductName;

            if (_activityData.ProductCategory != string.Empty)
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductCategory);
            }

            else
            {
                ProductCategory = ProductFamilies.None;
            }

            textBoxIP.Text = _activityData.PrinterIP;
            radioButtonTelnet.Checked = _activityData.TestType;
            radioButtonSNMP.Checked = !_activityData.TestType;
            ProductName = _activityData.ProductName;
            Telnet_sitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;

            LoadTestCaseList();
        }

        private void LoadTestCaseList()
        {
            LoadTestCases(typeof(TelnetTests), _activityData.TestNumbers);
        }

        #endregion

        #region Events

        /// <summary>
        /// Validating the plug-in edit control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TelnetSnmpConfiguration_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (base.SelectedTests.Count <= 0)
            {
                fieldValidator.Validate(testCaseDetails_GroupBox);
                e.Cancel = true;
            }
            else
            {
                fieldValidator.Validate(testCaseDetails_GroupBox);
            }
        }

        private void radioButtonTelnet_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTelnet.Checked)
            {
                _activityData.TestType = true;
            }
            else
            {
                _activityData.TestType = false;
            }
            LoadTestCaseList();

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Telnet Snmp"));
        }

        private void radioButtonSNMP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSNMP.Checked)
            {
                _activityData.TestType = false;
            }
            else
            {
                _activityData.TestType = true;
            }

            LoadTestCaseList();
        }
        /// <summary>
        /// Occurs when the product name is changed on the UI
        /// </summary>
        /// <param name="productFamily">Product Family</param>
        /// <param name="productName">Product Name</param>
        void TelnetConfigurationControl_OnProductNameChanged(string productFamily, string productName)
        {
            Telnet_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            Telnet_sitemapVersionSelector.PrinterName = base.ProductName;
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new TelnetSnmpActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<TelnetSnmpActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.TestNumbers.Clear();
            _activityData.TestNumbers = SelectedTests;
            _activityData.ProductName = ProductName;
            _activityData.PrinterIP = textBoxIP.Text;
            _activityData.ProductCategory = ProductCategory.ToString();
            _activityData.TestType = radioButtonTelnet.Checked;
            _activityData.SitemapsVersion = Telnet_sitemapVersionSelector.SitemapVersion;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion
    }
}
