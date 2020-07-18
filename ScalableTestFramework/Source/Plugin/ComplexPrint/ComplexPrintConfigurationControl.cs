using System;
using System.ComponentModel;
using System.Windows.Forms;

using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    /// <summary>
    /// Edit control for a Print Plug-in
    /// </summary>
    [ToolboxItem(false)]
    public partial class ComplexPrintConfigurationControl : CtcBaseConfigurationControl, IPluginConfigurationControl
    {
        #region Local Variables

        private ComplexPrintActivityData _activityData;

        public event EventHandler ConfigurationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexPrintConfigurationControl"/> class.
        /// </summary>
        public ComplexPrintConfigurationControl()
        {
            InitializeComponent();

            _activityData = new ComplexPrintActivityData();

            base.PropertyChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the test cases on form load event.
        /// </summary>
        private void LoadUI()
        {
            LoadUIControls();
            base.ProductCategory = _activityData.ProductFamily;
            LoadTestCases(typeof(ComplexPrintTests), _activityData.SelectedTests);

            // Since product name is taken from the CNP.xml confirmation file, hide the product name on the base form.
            HideProductName = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Opens the configuration parameters screen for viewing/editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configParams_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You are trying to edit the configuration parameters for '" + ProductCategory + "' product family. Do you want to continue?", "Complex Network Print", MessageBoxButtons.YesNo);

            // If the user want to continue open the configuration dialog else do nothing
            if (DialogResult.Yes == result)
            {
                ConfigurationParametersForm configForm = new ConfigurationParametersForm(ProductCategory);
                configForm.ShowDialog();
            }
        }

        #endregion

        #region IPluginConfigurationControl Implementation

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new ComplexPrintActivityData();
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<ComplexPrintActivityData>(CtcMetadataConverter.Converters);
            CtcSettings.Initialize(environment);
            LoadUI();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SelectedTests.Clear();

            // set the UI Elements data to the activity data
            _activityData.SelectedTests = base.SelectedTests;
            _activityData.ProductFamily = base.ProductCategory;

            return new PluginConfigurationData(_activityData, "1.1");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion
    }
}