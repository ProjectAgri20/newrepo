using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.DSSInstall
{
    /// <summary>
    /// Provides the control to configure the DSSInstall activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DSSInstallConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private DSSInstallActivityData _data;

        /// <summary>
        /// Initializes a new instance of the DSSInstallConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public DSSInstallConfigurationControl()
        {
            InitializeComponent();
            textBoxCustomLocation.DataBindings.Add("Enabled", CustomLocation_checkBox, "Checked");
            browselocation_button.DataBindings.Add("Enabled", CustomLocation_checkBox, "Checked");
            readme_checkBox.DataBindings.Add(new InvertedBinding("Enabled", uninstall_radioButton, "Checked"));
            launch_checkBox.DataBindings.Add(new InvertedBinding("Enabled", uninstall_radioButton, "Checked"));
            savesettings_checkBox.DataBindings.Add(new InvertedBinding("Enabled", uninstall_radioButton, "Checked"));

        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.InstallOption = fullinstall_radioButton.Checked
                ? InstallOptions.FullInstall
                : configutil_radioButton.Checked ? InstallOptions.Configuration : InstallOptions.Uninstall;
            _data.SetupFilePath = textBoxSetupFile.Text;
            _data.InstallPath = CustomLocation_checkBox.Checked ? textBoxCustomLocation.Text : string.Empty;
            _data.CancelInstall = cancel_checkBox.Checked;
            _data.ValidateInstall = validate_checkBox.Checked;
            _data.TransitionDelay = TimeSpan.FromSeconds(Convert.ToDouble(delay_numericUpDown.Value));
            _data.ViewReadme = !uninstall_radioButton.Checked && readme_checkBox.Checked;
            _data.LaunchApplication = !uninstall_radioButton.Checked && launch_checkBox.Checked;
            _data.SaveSettings = !uninstall_radioButton.Checked && savesettings_checkBox.Checked;
            return new PluginConfigurationData(_data, "1.0");
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new DSSInstallActivityData();
        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<DSSInstallActivityData>();
            textBoxSetupFile.Text = _data.SetupFilePath;
            textBoxCustomLocation.Text = _data.InstallPath;
            CustomLocation_checkBox.Checked = !string.IsNullOrEmpty(_data.InstallPath);

            fullinstall_radioButton.Checked = _data.InstallOption == InstallOptions.FullInstall;
            configutil_radioButton.Checked = _data.InstallOption == InstallOptions.Configuration;
            uninstall_radioButton.Checked = _data.InstallOption == InstallOptions.Uninstall;

            cancel_checkBox.Checked = _data.CancelInstall;
            validate_checkBox.Checked = _data.ValidateInstall;
            delay_numericUpDown.Value = Convert.ToDecimal(_data.TransitionDelay.TotalSeconds);

            launch_checkBox.Checked = _data.LaunchApplication;
            readme_checkBox.Checked = _data.ViewReadme;
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        /// <summary>
        /// Event handler to be called whenever the activity's configuration data changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.exe";
                dialog.Filter = @"Setup File (*.exe)|*.exe";
                dialog.Multiselect = false;
                dialog.Title = @"Select the Setup File";

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    textBoxSetupFile.Text = dialog.FileName;
                }
            }
        }

        private void buttonBrowseLocation_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    textBoxCustomLocation.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
