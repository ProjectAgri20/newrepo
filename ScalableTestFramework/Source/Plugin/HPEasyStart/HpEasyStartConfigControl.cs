using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpEasyStart
{
    /// <summary>
    /// Configuration Class for the HP Easy Start Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpEasyStartConfigControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel | AssetAttributes.Printer;
        /// <summary>
        /// Constructor for the HP Easy Start Configuration Control
        /// </summary>
        public HpEasyStartConfigControl()
        {
            InitializeComponent();
            fieldValidator.RequireValue(textBoxInstallerPath, "HP Easy Start Installer Path");
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxInstallerPath.TextChanged += (s, e) => ConfigurationChanged(s, e);

        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Get Configuration function to pass back the PluginActivityData to Framework 
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            HpEasyStartActivityData data = new HpEasyStartActivityData()
            {
                HpEasyStartInstallerPath = textBoxInstallerPath.Text,
                PrintTestPage = checkBoxTestPage.Checked,
                SetAsDefaultDriver = checkBoxSetDefault.Checked,
                IsWebPackInstallation = webPackInstallation_RadioButton.Checked
            };
            return new PluginConfigurationData(data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        /// <summary>
        /// Intialization call for the HP Easy Start Plugin
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new HpEasyStartActivityData());
            assetSelectionControl.Initialize(DeviceAttributes);
        }

        /// <summary>
        /// Intialization call for the HP Easy Start Plugin
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ConfigureControls(configuration.GetMetadata<HpEasyStartActivityData>());
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Call to Validate the Configuration before Passing back the Dta to Framework
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Configuring the Controls during Initialization Call to Plugin
        /// </summary>
        /// <param name="data"></param>
        private void ConfigureControls(HpEasyStartActivityData data)
        {
            textBoxInstallerPath.Text = data.HpEasyStartInstallerPath;
            checkBoxTestPage.Checked = data.PrintTestPage;
            checkBoxSetDefault.Checked = data.SetAsDefaultDriver;
            webPackInstallation_RadioButton.Checked = data.IsWebPackInstallation;
        }

        /// <summary>
        /// File Browse operation for HP East Start Setup file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPathSelection_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog
            {
                Title = @"Select HP Easy Installer",
                InitialDirectory = @"c:\",
                Filter = @"Setup File (*.exe)|*.exe",
                DefaultExt = "*.exe",
                Multiselect = false,
                RestoreDirectory = true,
                CheckFileExists = true
            };
            //To prevent mistypes in editbox
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                textBoxInstallerPath.Text = fdlg.FileName;
            }
        }
    }
}
