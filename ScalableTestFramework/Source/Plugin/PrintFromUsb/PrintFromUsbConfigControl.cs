using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.PrintFromUsb
{
    [ToolboxItem(false)]
    public partial class PrintFromUsbConfigControl : UserControl, IPluginConfigurationControl
    {
        private PrintFromUsbActivityData _data;
        public const string Version = "1.0";
        public PrintFromUsbConfigControl()
        {
            InitializeComponent();
            fieldValidator.RequireAssetSelection(firmware_assetSelectionControl);
            fieldValidator.RequireValue(textBoxUsbName, "USB Name");
        }

        public event EventHandler ConfigurationChanged;

        public PluginConfigurationData GetConfiguration()
        {
            _data.UsbName = textBoxUsbName.Text;
            return new PluginConfigurationData(_data, Version)
            {
                Assets = firmware_assetSelectionControl.AssetSelectionData
            };
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new PrintFromUsbActivityData();
            firmware_assetSelectionControl.Initialize(AssetAttributes.Printer);
        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<PrintFromUsbActivityData>();
            firmware_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.Printer);
            textBoxUsbName.Text = _data.UsbName;
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());
    }
}
