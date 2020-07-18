using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    /// <summary>
    /// Edit control for the Titan Atlas / Helios plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class JetAdvantageScanConfigControl : UserControl, IPluginConfigurationControl
    {
        private JetAdvantageScanActivityData _activityData;

        /// <summary>
        /// Initializes a new instance of the new <see cref="JetAdvantageScanConfigControl"/> class.
        /// </summary>
        public JetAdvantageScanConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(oxpdPassword_textBox, oxpdPassword_label);
            fieldValidator.RequireValue(titanLoginId_TextBox, loginId_Label);
            fieldValidator.RequireValue(titanPassword_TextBox, password_label);
            fieldValidator.RequireValue(loginPin_TextBox, loginPin_label, ValidationCondition.IfEnabled);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
            loginPin_TextBox.DataBindings.Add("Enabled", loginPin_CheckBox, "Checked");

            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            oxpdPassword_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            titanLoginId_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            titanPassword_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            loginPin_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            loginPin_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }
        /// <summary>
        /// Initializes the specified environment with empty data.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new JetAdvantageScanActivityData();
            LoadSettings();
            SetControlValues();
        }

        /// <summary>
        /// Initializes the control based on saved data
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<JetAdvantageScanActivityData>();
            assetSelectionControl.AdfDocuments = configuration.Documents;
            LoadSettings();
            SetControlValues();
        }
        #region Control Events

        /// <summary>
        /// Loads the settings groupbox with values.
        /// </summary>
        private void LoadSettings()
        {
            string[] paperSizes = { "Letter", "Legal", "Executive", "A4", "A5" };
            string[] orientation = { "Portrait", "Landscape" };
            string[] sides = { "Simplex", "Duplex" };
            string[] fileType = { "Pdf", "Tiff", "Mtiff", "Jpeg", "Xps" };
            paperSize_comboBox.DataSource = paperSizes;
            orientation_comboBox.DataSource = orientation;
            duplexmode_comboBox.DataSource = sides;
            filetype_comboBox.DataSource = fileType;
        }

        #endregion

        #region Control Methods

        /// <summary>
        /// Gets the configuration data based on user selections or stored meta data.
        /// </summary>
        /// <returns>PluginConfigurationData</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.AdminPassword = oxpdPassword_textBox.Text;
            _activityData.JetAdvantageLoginId = titanLoginId_TextBox.Text;
            _activityData.JetAdvantagePassword = titanPassword_TextBox.Text;
            _activityData.Settings.PaperSize = paperSize_comboBox.SelectedItem.ToString();
            _activityData.Settings.Orientation = orientation_comboBox.SelectedItem.ToString();
            _activityData.Settings.DuplexMode = duplexmode_comboBox.SelectedItem.ToString();
            _activityData.UseLoginPin = loginPin_CheckBox.Checked;
            _activityData.JetAdvantageLoginPin = loginPin_TextBox.Text;
            _activityData.UseAdf = assetSelectionControl.UseAdf;
            _activityData.AutomationPause = assetSelectionControl.AutomationPause;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = assetSelectionControl.AdfDocuments
            };

        }

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        /// <returns>bool</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Sets the control values based on provided information from the initialized member objects.
        /// </summary>
        private void SetControlValues()
        {
            oxpdPassword_textBox.Text = _activityData.AdminPassword;
            titanLoginId_TextBox.Text = _activityData.JetAdvantageLoginId;
            titanPassword_TextBox.Text = _activityData.JetAdvantagePassword;
            loginPin_CheckBox.Checked = _activityData.UseLoginPin;
            loginPin_TextBox.Text = _activityData.JetAdvantageLoginPin;
            paperSize_comboBox.SelectedItem = _activityData.Settings.PaperSize;
            orientation_comboBox.SelectedItem = _activityData.Settings.Orientation;
            duplexmode_comboBox.SelectedItem = _activityData.Settings.DuplexMode;
            filetype_comboBox.SelectedItem = _activityData.Settings.FileType;
        }

        #endregion

        /// <summary>
        /// Restricting the UI input to Numeric value.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="e">EventArgs.</param>
        private void loginPin_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
