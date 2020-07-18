using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Hpec
{
    /// <summary>
    /// Control to configure data for an HPEC plugin activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpecConfigControl : UserControl, IPluginConfigurationControl
    {
        private HpecActivityData _data;
        private const Framework.Assets.AssetAttributes _deviceAttributes = Framework.Assets.AssetAttributes.Scanner | Framework.Assets.AssetAttributes.ControlPanel;

        public const string Version = "1.0";

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpecConfigControl"/> class.
        /// </summary>
        public HpecConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(workflowName_comboBox, workflowName_label);
            fieldValidator.RequireAssetSelection(deviceSelectionControl, "scanner");
            deviceSelectionControl.SelectionChanged += OnConfigurationChanged;
            pageCount_NumericUpDown.ValueChanged += OnConfigurationChanged;
            skipTimeout_TimeSpanControl.ValueChanged += OnConfigurationChanged;
            workflowName_comboBox.TextChanged += OnConfigurationChanged;
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, EventArgs.Empty);
            }
        }

        private void ConfigureControls()
        {
            // Set up data bindings
            if (_data != null)
            {
                workflowName_comboBox.Text = _data.WorkflowName;
                pageCount_NumericUpDown.Value = _data.PageCount;
                radioButtonHpcr.Checked = _data.ApplicationAuthentication;
                radioButtonDeviceSignIn.Checked = !_data.ApplicationAuthentication;

                if (string.IsNullOrEmpty(_data.WorkflowName) && workflowName_comboBox.Items.Count > 0)
                {
                    workflowName_comboBox.SelectedIndex = 0;
                }
            }

            skipTimeout_TimeSpanControl.Value = _data.LockTimeouts.AcquireTimeout;
        }

        public void Initialize(PluginEnvironment environment)
        {
            deviceSelectionControl.Initialize(_deviceAttributes);
            _data = new HpecActivityData();
            ConfigureControls();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            deviceSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            _data = configuration.GetMetadata<HpecActivityData>();
            ConfigureControls();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _data.LockTimeouts = new LockTimeoutData(skipTimeout_TimeSpanControl.Value, TimeSpan.FromMinutes(5));
            _data.PageCount = (int)pageCount_NumericUpDown.Value;
            _data.WorkflowName = workflowName_comboBox.Text;
            _data.ApplicationAuthentication = radioButtonHpcr.Checked;

            return new PluginConfigurationData(_data, Version)
            {
                Assets = deviceSelectionControl.AssetSelectionData,
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

    }
}
