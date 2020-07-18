using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.USBFirmwarePerformance
{
    [ToolboxItem(false)]
    public partial class USBFirmwarePerformanceConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private USBFirmwarePerformanceActivityData _data;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="USBFirmwarePerformanceConfigurationControl" /> class.
        /// </summary>
        public USBFirmwarePerformanceConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            validatetimeSpanControl.DataBindings.Add("Enabled", checkBoxValidate, "Checked");

            fieldValidator.RequireCustom(validatetimeSpanControl, ValidateTimeout, "Please enter sufficient time for validation");
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new USBFirmwarePerformanceActivityData();
            validatetimeSpanControl.Value = TimeSpan.FromMinutes(30.0);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<USBFirmwarePerformanceActivityData>();
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);

            checkBoxValidate.Checked = _data.ValidateFlash;
            validatetimeSpanControl.Value = _data.ValidateTimeOut == TimeSpan.FromMinutes(0) ? TimeSpan.FromMinutes(1) : _data.ValidateTimeOut;
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.ValidateFlash = checkBoxValidate.Checked;
            _data.ValidateTimeOut = validatetimeSpanControl.Value;

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private bool ValidateTimeout()
        {
            if (!checkBoxValidate.Checked)
            {
                return true;
            }
            return validatetimeSpanControl.Value > TimeSpan.FromMinutes(1);
        }
    }
}
