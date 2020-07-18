using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.JetAdvantage
{
    /// <summary>
    /// Edit control for the Titan Atlas plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class JetAdvantageConfigControl : UserControl, IPluginConfigurationControl
    {
        private JetAdvantageActivityData _activityData;
        private DeviceInfo _device;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the new <see cref="JetAdvantageConfigControl"/> class.
        /// </summary>
        public JetAdvantageConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(deviceId_TextBox, deviceId_Label);
            fieldValidator.RequireValue(oxpdPassword_textBox, oxpdPassword_label);
            fieldValidator.RequireValue(titanLoginId_TextBox, loginId_Label);
            fieldValidator.RequireValue(titanPassword_TextBox, password_label);
            fieldValidator.RequireValue(loginPin_TextBox, loginPin_label, ValidationCondition.IfEnabled);

            loginPin_TextBox.DataBindings.Add("Enabled", loginPin_CheckBox, "Checked");

            deviceId_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pullPrint_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            scanSend_Button.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            printAll_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            deleteAfterPrint_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
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
            _activityData = new JetAdvantageActivityData();
            _device = new DeviceInfo(string.Empty, AssetAttributes.None, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            SetControlValues();
            //SetJetAdvantageSolution();
        }

        /// <summary>
        /// Initializes the control based on saved data
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<JetAdvantageActivityData>();
            _device = (DeviceInfo)ConfigurationServices.AssetInventory.GetAsset(configuration.Assets.SelectedAssets.FirstOrDefault());

            SetControlValues();
            //SetJetAdvantageSolution();
        }
        #region Control Events

        private void jetAdvantage_RbtnCheckedChanged(object sender, EventArgs e)
        {
            //Since this Plugin is now  considered only for the Titan Pullprinting Commenting App Selection Code

            //if(pullPrint_RadioButton.Checked)
            //{
            //    _activityData.JetAdvantageSolution = JetAdvantageSolution.AtlasPullPrint;
            //}
            //else if(scanSend_Button.Checked)
            //{
            //    _activityData.JetAdvantageSolution = JetAdvantageSolution.HeliosScanSend;
            //}
        }
        private void deviceSelection_Button_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm printerSelectionForm = new AssetSelectionForm(AssetAttributes.Printer | AssetAttributes.ControlPanel, false))
            {
                printerSelectionForm.ShowDialog(this);
                if (printerSelectionForm.DialogResult == DialogResult.OK)
                {
                    _device = (DeviceInfo)printerSelectionForm.SelectedAssets.FirstOrDefault(); // what happens if not device selected?
                    if (_device != null)
                    {
                        deviceId_TextBox.Text = _device.AssetId;
                    }
                }
            }
        }

        private void printAll_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (printAll_CheckBox.Checked)
            {
                pluginDescription_Label.Text = "This plugin will log into a pull print enabled device and print all documents.";
            }
            else
            {
                pluginDescription_Label.Text = "This plugin will log into a pull print enabled device, select one print job and pull it.";
            }
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
            _activityData.JetAdvantageLoginPin = loginPin_TextBox.Text;

            _activityData.DeleteAfterPrint = deleteAfterPrint_CheckBox.Checked;
            _activityData.PrintAllDocuments = printAll_CheckBox.Checked;
            _activityData.UseLoginPin = loginPin_CheckBox.Checked;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = new AssetSelectionData(_device)
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

        /// <summary>
        /// Sets the control values based on provided information from the initialized member objects.
        /// </summary>
        private void SetControlValues()
        {
            deviceId_TextBox.Text = _device.AssetId;

            printAll_CheckBox.Checked = _activityData.PrintAllDocuments;
            deleteAfterPrint_CheckBox.Checked = _activityData.DeleteAfterPrint;
            oxpdPassword_textBox.Text = _activityData.AdminPassword;
            titanLoginId_TextBox.Text = _activityData.JetAdvantageLoginId;
            titanPassword_TextBox.Text = _activityData.JetAdvantagePassword;
            loginPin_CheckBox.Checked = _activityData.UseLoginPin;
            loginPin_TextBox.Text = _activityData.JetAdvantageLoginPin;
        }

        #endregion



        private void loginPin_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

        }
    }
}
