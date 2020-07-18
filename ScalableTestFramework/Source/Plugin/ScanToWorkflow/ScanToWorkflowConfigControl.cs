using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    /// <summary>
    /// Control to configure data for a Scan to Workflow activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToWorkflowConfigControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes _deviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        public const string Version = "1.1";
        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToWorkflowConfigControl"/> class.
        /// </summary>
        public ScanToWorkflowConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(workflowName_TextBox, workflowName_Label);
            fieldValidator.RequireAssetSelection(assetSelectionControl, "device");

            pageCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            workflowName_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            workflowPrompts_DataGridView.CellValueChanged += (s, e) => ConfigurationChanged(s, e);
            folderDestination_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            logOcr_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            digitalSendServer_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            excludeFileNamePrompt_checkBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            List<KeyValuePair<AuthenticationProvider, string>> authProviders = new List<KeyValuePair<AuthenticationProvider, string>>();
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacDra, AuthenticationProvider.HpacDra.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.HpacIrm, AuthenticationProvider.HpacIrm.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Windows, AuthenticationProvider.Windows.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.DSS, AuthenticationProvider.DSS.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Auto, AuthenticationProvider.Auto.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Card, AuthenticationProvider.Card.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.SafeCom, AuthenticationProvider.SafeCom.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Equitrac, AuthenticationProvider.Equitrac.GetDescription()));
            authProviders.Add(new KeyValuePair<AuthenticationProvider, string>(AuthenticationProvider.Skip, AuthenticationProvider.Skip.GetDescription()));
            comboBox_AuthProvider.DataSource = authProviders;
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new ScanToWorkflowData());

            assetSelectionControl.Initialize(_deviceAttributes);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ConfigureControls(configuration.GetMetadata<ScanToWorkflowData>(ConverterProvider.GetMetadataConverters()));

            assetSelectionControl.Initialize(configuration.Assets, _deviceAttributes);
            assetSelectionControl.AdfDocuments = configuration.Documents;
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            ScanToWorkflowData data = new ScanToWorkflowData()
            {
                WorkflowName = workflowName_TextBox.Text,
                UseOcr = logOcr_CheckBox.Checked,
                PageCount = (int)pageCount_NumericUpDown.Value,
                LockTimeouts = lockTimeoutControl.Value,
                AutomationPause = assetSelectionControl.AutomationPause,
                UseAdf = assetSelectionControl.UseAdf,
                ApplicationAuthentication = radioButton_ScanToWorkflow.Checked,
                ExcludeFileNamePrompt = excludeFileNamePrompt_checkBox.Checked,
                AuthProvider = (AuthenticationProvider)comboBox_AuthProvider.SelectedValue
            };
            data.DestinationType = (folderDestination_RadioButton.Checked ? "Workflow" : "SharePoint");
            data.DigitalSendServer = digitalSendServer_TextBox.Text;

            data.PromptValues.Clear();
            foreach (DataGridViewRow row in workflowPrompts_DataGridView.Rows)
            {
                if (!row.ReadOnly)
                {
                    object promptText = row.Cells[0].Value;
                    object promptValue = row.Cells[1].Value;
                    if (promptText != null && promptValue != null)
                    {
                        data.PromptValues.Add(new WorkflowPromptValue(promptText.ToString(), promptValue.ToString()));
                    }
                }
            }

            return new PluginConfigurationData(data, Version)
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = assetSelectionControl.AdfDocuments
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

        private void ConfigureControls(ScanToWorkflowData data)
        {
            workflowName_TextBox.Text = data.WorkflowName;
            logOcr_CheckBox.Checked = data.UseOcr;
            pageCount_NumericUpDown.Value = data.PageCount;
           
            lockTimeoutControl.Initialize(data.LockTimeouts);

            assetSelectionControl.AutomationPause = data.AutomationPause;
            assetSelectionControl.UseAdf = data.UseAdf;
            excludeFileNamePrompt_checkBox.Checked = data.ExcludeFileNamePrompt;

            // Determine whether the DSS server should be filled in
            if (!string.IsNullOrEmpty(data.DigitalSendServer))
            {
                digitalSendServer_TextBox.Text = data.DigitalSendServer;
            }

            // Initialize other UI values
            sharepointDestination_RadioButton.Checked = (data.DestinationType == "SharePoint");
            if(data.ApplicationAuthentication)
            {
                radioButton_ScanToWorkflow.Checked = true;
            }
            else
            {
                radioButton_SignInButton.Checked = true;
            }

            comboBox_AuthProvider.SelectedValue = data.AuthProvider;
            SetPromptValues(data);
        }

        private void SetPromptValues(ScanToWorkflowData data)
        {
            workflowPrompts_DataGridView.Rows.Clear();

            // Add read-only row for the OCR file name prompt
            workflowPrompts_DataGridView.Rows.Add(new object[] { WorkflowScanManager.FileNamePrompt, "[Automatic]" });
            DataGridViewRow row = workflowPrompts_DataGridView.Rows[0];
            row.DefaultCellStyle = new DataGridViewCellStyle(workflowPrompts_DataGridView.DefaultCellStyle);
            row.DefaultCellStyle.BackColor = SystemColors.Control;
            row.ReadOnly = true;

            // Add a row for each of the user-defined prompt values
            foreach (WorkflowPromptValue prompt in data.PromptValues)
            {
                workflowPrompts_DataGridView.Rows.Add(new object[] { prompt.PromptText, prompt.PromptValue });
            }
        }
    }
}
