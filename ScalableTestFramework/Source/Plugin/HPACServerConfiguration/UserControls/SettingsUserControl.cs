using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// User control that allows users to view and manage application settings.
    /// </summary>
    internal partial class SettingsUserControl : UserControl
    {
        private SettingsOperation selectedOperation;

        SettingsTabData settingsdata = new SettingsTabData();
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsUserControl"/> class.
        /// </summary>
        public SettingsUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates and returns a <see cref="SettingsTabData" /> instance containing the
        /// Settings data from this control.
        /// </summary>
        /// <returns>The Settings data.</returns>
        public SettingsTabData GetConfigurationData()
        {
            settingsdata.Tracking = new List<SNMPTracking>();
            settingsdata.QueueName = queueName_TextBox.Text;
            settingsdata.SettingsOperation = selectedOperation;
            settingsdata.ServerURI = ServerName_TextBox.Text;
            settingsdata.ServerIPAddress = IPAddress_textBox.Text;

            if (purgedJobs_CheckBox.Checked)
            {
                settingsdata.PurgedJobs = true;
            }
            else
            {
                settingsdata.PurgedJobs = false;
            }
            if (copies_CheckBox.Checked)
            {
                settingsdata.Tracking.Add(SNMPTracking.Copies);
            }
            if (digitalSending_CheckBox.Checked)
            {
                settingsdata.Tracking.Add(SNMPTracking.DigitalSending);
            }

            if (agent_CheckBox.Checked)
            {
                settingsdata.EnableQuota = true;
            }
            else
            {
                settingsdata.EnableQuota = false;
            }

            if (Encryption_CheckBox.Checked)
            {
                settingsdata.Encryption = true;

            }
            else
            {
                settingsdata.Encryption = false;
            }

            settingsdata.ProtocolOptions = EnumUtil.Parse<ProtocolOptions>(ProtocolOptions_comboBox.SelectedItem.ToString(), true);
            return settingsdata;
        }

        private void queueTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (queueTabControl.SelectedTab == queueTabControl.TabPages["queueNameTabPage"])
            {
                if (addRadioButton.Checked)
                {
                    selectedOperation = SettingsOperation.AddPrintQueue;
                }
                else
                {
                    selectedOperation = SettingsOperation.DeletePrintQueue;
                }
            }
            else if (queueTabControl.SelectedTab == queueTabControl.TabPages["Option_TabPage"])
            {
                selectedOperation = SettingsOperation.Protocol;
            }
            else
            {
                selectedOperation = SettingsOperation.QuotaSettings;
            }
        }

        private void addRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (addRadioButton.Checked)
            {
                selectedOperation = SettingsOperation.AddPrintQueue;
            }
            else
            {
                selectedOperation = SettingsOperation.DeletePrintQueue;
            }
        }

        /// <summary>
        /// Configures the controls per the settings data either derived from initialization or the saved meta data.
        /// </summary>
        public void LoadConfiguration(SettingsTabData settingsdata)
        {
            ClearCheckBoxes(this);
            if (settingsdata.SettingsOperation == SettingsOperation.QuotaSettings)
            {
                queueTabControl.SelectedIndex = 1;
            }
            else
            {
                queueTabControl.SelectedIndex = 0;
            }
            switch (settingsdata.SettingsOperation)
            {
                case SettingsOperation.AddPrintQueue:
                    addRadioButton.Checked = true;
                    break;
                case SettingsOperation.DeletePrintQueue:
                    deleteRadioButton.Checked = true;
                    break;
                default:
                    break;
            }
            queueName_TextBox.Text = settingsdata.QueueName;
            settingsdata.EnableQuota = agent_CheckBox.Checked;
            settingsdata.PurgedJobs = purgedJobs_CheckBox.Checked;
            queueName_TextBox.Text = settingsdata.QueueName;
            foreach (var checkedItemText in settingsdata.Tracking)
            {
                switch (checkedItemText)
                {
                    case SNMPTracking.Copies:
                        copies_CheckBox.Checked = true;
                        break;
                    case SNMPTracking.DigitalSending:
                        digitalSending_CheckBox.Checked = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult AddValidaton()
        {
            if (queueTabControl.SelectedTab == queueTabControl.TabPages["queueNameTabPage"])
            {
                fieldValidator.RequireValue(queueName_TextBox, queueName_Label);
            }
            else
            {
                fieldValidator.Remove(queueName_TextBox);
            }
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Removes the Validation for the controls.
        /// </summary>
        public void RemoveValidation()
        {
            fieldValidator.Remove(queueName_TextBox);
        }

        private void ClearCheckBoxes(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                ClearCheckBoxes(ctrl);
            }
        }

        private void SettingsUserControl_Load(object sender, EventArgs e)
        {
            ProtocolOptions_comboBox.SelectedIndex = 0;
        }

        private void ProtocolOptions_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
