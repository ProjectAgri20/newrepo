using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Plugin.PaperCutInstaller
{
    /// <summary>
    /// Configuration Control Module for the Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class PaperCutInstallerConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private PaperCutInstallerActivityData _pluginData;
        public PaperCutInstallerConfigurationControl()
        {
            InitializeComponent();
            tasks_comboBox.DataSource = Enum.GetNames(typeof(PaperCutInstallerAction));
            fieldValidator.RequireAssetSelection(paperCut_assetSelectionControl);
            fieldValidator.RequireSelection(paperCut_serverComboBox, label_PaperCutServer);
            fieldValidator.RequireSelection(tasks_comboBox, tasks_label);
            fieldValidator.RequireCustom(logon_textBox,()=> !string.IsNullOrEmpty(logon_textBox.Text) || !register_groupBox.Enabled , "Please enter administration credentials of PaperCut");
            fieldValidator.RequireCustom(password_textBox, () => !string.IsNullOrEmpty(password_textBox.Text) || !register_groupBox.Enabled, "Please enter administration credentials of PaperCut");
            fieldValidator.RequireCustom(textBox_QueueName, ()=> !string.IsNullOrEmpty(textBox_QueueName.Text) || !queue_groupBox.Enabled, "Please enter the Shared QueueName for the device");
            fieldValidator.RequireCustom(login_groupBox, ValidateAuthentication,"Please select an authentication method.");
            fieldValidator.RequireCustom(tracking_groupBox, ValidateTracking, "Please select a function to Track.");
            fieldValidator.RequireCustom(bundleFile_textBox, ()=> !string.IsNullOrEmpty(bundleFile_textBox.Text) || !browse_button.Enabled,"Please enter the path of Bundle File.");
            
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Gets the Configuration Settings from the UI to the Activity Data
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _pluginData.Action = (PaperCutInstallerAction) tasks_comboBox.SelectedIndex;
            _pluginData.BundleFile = bundleFile_textBox.Text;
            _pluginData.AdminUserName = logon_textBox.Text;
            _pluginData.AdminPassword = password_textBox.Text;
            _pluginData.SourcePrintQueue = textBox_QueueName.Text;
            _pluginData.AutoRelease = checkBox_AutoRelease.Checked;

            int capability = 0;
            if (radioButton_password.Checked && login_groupBox.Enabled)
                capability += 1;
            if (radioButton_identity.Checked && login_groupBox.Enabled)
                capability += 2;
            if (checkbox_Card.Checked && login_groupBox.Enabled)
                capability += 4;
            if (checkBox_guest.Checked)
                capability = 8;

            _pluginData.AuthenticationMethod = (PaperCutAuthentication)capability;

            capability = 0;
            foreach (CheckBox checkBox in tracking_groupBox.Controls)
            {
                if (checkBox.Checked)
                    capability += int.Parse(checkBox.Tag.ToString());
            }

            _pluginData.Tracking = (PaperCutTracking) capability;

            return new PluginConfigurationData(_pluginData, "1.0")
            {
                Assets = paperCut_assetSelectionControl.AssetSelectionData,
                Servers = new ServerSelectionData(paperCut_serverComboBox.SelectedServer)
            };
        }

        /// <summary>
        /// Initialize the Control for new MetaData
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _pluginData = new PaperCutInstallerActivityData();
            paperCut_assetSelectionControl.Initialize(AssetAttributes.ControlPanel);
            paperCut_serverComboBox.Initialize("PaperCut");
            
        }

        /// <summary>
        /// Initializes the Control for the Existing MetaData
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _pluginData = configuration.GetMetadata<PaperCutInstallerActivityData>();
          
            paperCut_assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.ControlPanel);
            paperCut_serverComboBox.Initialize(configuration.Servers.SelectedServers.FirstOrDefault());
            tasks_comboBox.SelectedItem = _pluginData.Action.ToString();
            bundleFile_textBox.Text = _pluginData.BundleFile;
            logon_textBox.Text = _pluginData.AdminUserName;
            password_textBox.Text = _pluginData.AdminPassword;
            textBox_QueueName.Text = _pluginData.SourcePrintQueue;
            checkBox_AutoRelease.Checked = _pluginData.AutoRelease;

            if (_pluginData.AuthenticationMethod.HasFlag(PaperCutAuthentication.Password))
            {
                radioButton_password.Checked = true;
            }
            if (_pluginData.AuthenticationMethod.HasFlag(PaperCutAuthentication.Identity))
            {
                radioButton_identity.Checked = true;
            }
            if (_pluginData.AuthenticationMethod.HasFlag(PaperCutAuthentication.SwipeCard))
            {
                checkbox_Card.Checked = true;
            }
            if (_pluginData.AuthenticationMethod.HasFlag(PaperCutAuthentication.Guest))
            {
                checkBox_guest.Checked = true;
            }


            foreach (CheckBox checkBox in tracking_groupBox.Controls)
            {
                int checkBoxAttribute = int.Parse(checkBox.Tag.ToString());
                checkBox.Checked = _pluginData.Tracking.HasFlag((PaperCutTracking)checkBoxAttribute);
            }
        }

        /// <summary>
        /// Validates the values Input by user in Configuration Control
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration()
        {
           return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private bool ValidateAuthentication()
        {
            if (login_groupBox.Enabled)
            {
                return (radioButton_identity.Checked || radioButton_password.Checked || checkBox_guest.Checked ||
                        checkbox_Card.Checked);
            }

            return true;
        }

        private bool ValidateTracking()
        {
            if (tracking_groupBox.Enabled)
                return checkbox_TrackFax.Checked || checkbox_TrackPrint.Checked || checkbox_TrackFax.Checked;

            return true;
        }
        private void browse_button_Click(object sender, EventArgs e)
        {
            var bundleOpenFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = @"PaperCut software (*.bdl) | *.bdl"
            };

            if (bundleOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                bundleFile_textBox.Text = bundleOpenFileDialog.FileName;
            }
        }

        private void tasks_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            browse_button.Enabled = false;
            bundleFile_textBox.Enabled = false;
            register_groupBox.Enabled = false;
            queue_groupBox.Enabled = false;
            login_groupBox.Enabled = false;
            tracking_groupBox.Enabled = false;

            if (tasks_comboBox.SelectedIndex == 0)
            {
                browse_button.Enabled = true;
                bundleFile_textBox.Enabled = true;
            }
            if (tasks_comboBox.SelectedIndex == 2)
            {
                register_groupBox.Enabled = true;
            }
            else if (tasks_comboBox.SelectedIndex == 3)
            {
                register_groupBox.Enabled = true;
                queue_groupBox.Enabled = true;
                login_groupBox.Enabled = true;
                tracking_groupBox.Enabled = true;
            }
        }
    }
}
