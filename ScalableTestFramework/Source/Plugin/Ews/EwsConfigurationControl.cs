using System;
using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Plugin.Ews
{
    [ToolboxItem(false)]
    public partial class EwsConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Member Variables
        private EwsActivityData _activityData;

        public event EventHandler ConfigurationChanged;
        #endregion

        #region Constructor annd Initialization
        public EwsConfigurationControl()
        {
            InitializeComponent();

            _activityData = new EwsActivityData();
            base.OnProductNameChanged += new ProductNameChangedEventHandler(EWSEditControl_OnProductNameChanged);
           // EWS_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
          //  EWS_sitemapVersionSelector.PrinterName = base.ProductName;

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            EWS_sitemapVersionSelector.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxIP.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new EwsActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<EwsActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            // Get list of test cases
            _activityData.TestNumbers.Clear();

            _activityData.TestNumbers = SelectedTests;
            _activityData.ProductName = ProductName;
            _activityData.PrinterIP = textBoxIP.Text;
            _activityData.ProductCategory = ProductCategory.ToString();
            _activityData.SitemapPath = EWS_sitemapVersionSelector.SitemapPath;
            _activityData.SitemapsVersion = EWS_sitemapVersionSelector.SitemapVersion;

            if (radioButtonIE.Checked)
            {
                _activityData.BrowserNumber = 1;
                _activityData.Browser = "Explorer";
            }
            else if (radioButtonFirefox.Checked)
            {
                _activityData.BrowserNumber = 2;
                _activityData.Browser = "Firefox";
            }
            else if (radioButtonChrome.Checked)
            {
                _activityData.BrowserNumber = 3;
                _activityData.Browser = "Chrome";
            }
            else if (radioButtonSafari.Checked)
            {
                _activityData.BrowserNumber = 4;
                _activityData.Browser = "Safari";
            }
            else
            {
                _activityData.BrowserNumber = 5;
                _activityData.Browser = "Opera";
            }

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            IEnumerable<Framework.ValidationResult> result = fieldValidator.ValidateAll();
            result = result.Concat(EWS_sitemapVersionSelector.ValidateControl());
            return new PluginValidationResult(result);
        }

        #endregion        

        #region Methods
        private void LoadUI()
        {

            LoadUIControls();
            textBoxIP.Text = _activityData.PrinterIP;

            EWS_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            EWS_sitemapVersionSelector.PrinterName = base.ProductName;

            if (_activityData.ProductCategory != string.Empty)
            {
                ProductCategory = (ProductFamilies)Enum.Parse(typeof(ProductFamilies), _activityData.ProductCategory);
            }
            else
            {
                ProductCategory = ProductFamilies.None;
            }

            ProductName = _activityData.ProductName;

            switch (_activityData.BrowserNumber)
            {
                case 1:
                    radioButtonIE.Checked = true;
                    break;

                case 2:
                    radioButtonFirefox.Checked = true;
                    break;

                case 3:
                    radioButtonChrome.Checked = true;
                    break;

                case 4:
                    radioButtonSafari.Checked = true;
                    break;

                case 5:
                    radioButtonOpera.Checked = true;
                    break;

                default:
                    radioButtonIE.Checked = true;
                    break;
            }

            EWS_sitemapVersionSelector.SitemapPath = _activityData.SitemapPath;
            EWS_sitemapVersionSelector.SitemapVersion = _activityData.SitemapsVersion;
            LoadTestCaseList();
        }

        private void LoadTestCaseList()
        {
            LoadTestCases(typeof(EwsTests), _activityData.TestNumbers);
        }

        private void Setbrowsertype(int browserNumber)
        {
            switch (browserNumber)
            {
                case 1:
                    _activityData.BrowserNumber = 1;
                    _activityData.Browser = "Explorer";
                    break;

                case 2:
                    _activityData.BrowserNumber = 2;
                    _activityData.Browser = "Firefox";
                    break;

                case 3:
                    _activityData.BrowserNumber = 3;
                    _activityData.Browser = "Chrome";
                    break;

                case 4:
                    _activityData.BrowserNumber = 4;
                    _activityData.Browser = "Safari";
                    break;

                case 5:
                    _activityData.BrowserNumber = 5;
                    _activityData.Browser = "Opera";
                    break;

                default:
                    _activityData.BrowserNumber = 1;
                    _activityData.Browser = "Explorer";
                    break;
            }
        }
        #endregion

        #region Events

        private void radioButtonIE_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonIE.Checked)
            {
                Setbrowsertype(1);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Browser Type"));
        }

        private void radioButtonChrome_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonChrome.Checked)
            {
                Setbrowsertype(3);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Browser Type"));
        }

        private void radioButtonFirefox_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFirefox.Checked)
            {
                Setbrowsertype(2);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Browser Type"));
        }

        private void radioButtonSafari_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSafari.Checked)
            {
                Setbrowsertype(4);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Browser Type"));
        }

        private void radioButtonOpera_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOpera.Checked)
            {
                Setbrowsertype(5);
            }

            base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Browser Type"));
        }

        /// <summary>
        /// Occurs when the product name is changed on the UI
        /// </summary>
        /// <param name="productFamily">Product Family</param>
        /// <param name="productName">Product Name</param>
        void EWSEditControl_OnProductNameChanged(string productFamily, string productName)
        {
            EWS_sitemapVersionSelector.PrinterFamily = base.ProductCategory.ToString();
            EWS_sitemapVersionSelector.PrinterName = base.ProductName;
        }

        #endregion

    }
}
