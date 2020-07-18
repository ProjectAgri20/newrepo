using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework.Properties;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Provides editing of serialized configuration data for <see cref="OutputMonitorConfig"/> type. 
    /// </summary>
    public partial class OutputMonitorConfigControl : UserControl, IMonitorConfigControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private OutputMonitorConfig _outputMonitorConfig = null;
        private string _serverHostName = string.Empty;
        private STFMonitorType? _monitorType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputMonitorConfigControl"/> form.
        /// </summary>
        /// <param name="monitorConfig">The configuration data for an output monitor.</param>
        public OutputMonitorConfigControl(MonitorConfig monitorConfig)
        {
            InitializeComponent();

            // Populate combo boxes
            retention_ComboBox.DataSource = EnumUtil.GetDescriptions<RetentionOption>().ToArray();
            retention_ComboBox.SelectedIndex = -1;

            //groupBox_Validation.Enabled = IsEnterpriseEnabled();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            _outputMonitorConfig = (OutputMonitorConfig)StfMonitorConfigFactory.Create(monitorConfig.MonitorType, monitorConfig.Configuration);

            // Populate the controls
            _serverHostName = monitorConfig.ServerHostName;
            _monitorType = EnumUtil.Parse<STFMonitorType>(monitorConfig.MonitorType);
            InitializeMonitorType();
            destination_TextBox.Text = _outputMonitorConfig.MonitorLocation;
            retention_ComboBox.SelectedItem = EnumUtil.GetDescription(_outputMonitorConfig.Retention);
            retentionLocation_TextBox.Text = _outputMonitorConfig.RetentionLocation;
            lookForMetadata_CheckBox.Checked = _outputMonitorConfig.LookForMetadataFile;
            metadataExtension_TextBox.Text = _outputMonitorConfig.MetadataFileExtension;            
        }

        /// <summary>
        /// Gets the serialized string of the <see cref="StfMonitorConfig" /> object.
        /// </summary>
        public string Configuration
        {
            get
            {
                return LegacySerializer.SerializeXml(_outputMonitorConfig).ToString();
            }
        }

        /// <summary>
        /// Sets the name of the server hosting the monitor service.
        /// </summary>
        /// <param name="serverHostName">The server host name.</param>
        public void SetServerHostName(string serverHostName)
        {
            _serverHostName = serverHostName;
        }

        /// <summary>
        /// Configures the UI based on the monitor type.
        /// </summary>
        private void InitializeMonitorType()
        {
            // Set the destination type label for the selected monitor type
            switch (_monitorType)
            {
                case STFMonitorType.OutputDirectory:
                case STFMonitorType.LANFax:
                    help_PictureBox.Visible = true;
                    toolTip.SetToolTip(help_PictureBox, Resources.MonitorLocationToolTip);
                    break;

                case STFMonitorType.OutputEmail:
                case STFMonitorType.DigitalSendNotification:
                    label_Destination.Text += " (Email Username)";
                    break;

                case STFMonitorType.SharePoint:
                    label_Destination.Text += " (SharePoint Url)";
                    break;
                default:
                    throw new InvalidOperationException($"{_monitorType} does not have a mapped OutputMonitorConfig implementation.");
            }

            // Set availability of validation/retention options
            if (!IsEnterpriseEnabled())
            {
                return;
            }

            switch (_monitorType)
            {
                case STFMonitorType.DigitalSendNotification:
                    groupBox_Validation.Enabled = false;
                    break;

                case STFMonitorType.LANFax:
                    lookForMetadata_CheckBox.Enabled = false;
                    label_MetadataExtension.Enabled = false;
                    metadataExtension_TextBox.Enabled = false;
                    break;

                default:
                    groupBox_Validation.Enabled = true;
                    lookForMetadata_CheckBox.Enabled = true;
                    label_MetadataExtension.Enabled = true;
                    metadataExtension_TextBox.Enabled = lookForMetadata_CheckBox.Checked;
                    break;
            }
        }

        private void retentionLocation_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!IsEnterpriseEnabled())
            {
                return;
            }

            if (HasValue(retentionLocation_TextBox, label_RetentionLocation, e))
            {
                _outputMonitorConfig.RetentionLocation = retentionLocation_TextBox.Text;
            }
        }

        private void retention_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (!IsEnterpriseEnabled())
            {
                return;
            }

            if (retention_ComboBox.SelectedItem == null)
            {
                _errorProvider.SetError(retention_ComboBox, "You must select a retention policy");
            }
            else
            {
                _errorProvider.SetError(retention_ComboBox, string.Empty);
                _outputMonitorConfig.Retention = EnumUtil.GetByDescription<RetentionOption>(retention_ComboBox.Text);
            }
        }

        private void destination_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (HasValue(destination_TextBox, label_Destination, e))
            {
                if (string.IsNullOrEmpty(_serverHostName) == false && _monitorType == STFMonitorType.OutputDirectory)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (IsFolderPath(destination_TextBox.Text))
                    {
                        ValidatePluginOutputPath(ref e);                        
                    }
                    else
                    {
                        ValidateServiceOutputPath(ref e);
                    }
                    Cursor.Current = Cursors.Default;
                }

                if (e.Cancel == false)
                {
                    _outputMonitorConfig.MonitorLocation = destination_TextBox.Text;
                }
            }
        }

        private void ValidateServiceOutputPath(ref CancelEventArgs e)
        {
            string message = string.Empty;
            bool result = false;

            try
            {
                using (var stfMonitorSvcCxn = STFMonitorServiceConnection.Create($"{_serverHostName}.{GlobalSettings.Items[Setting.DnsDomain]}"))
                {
                    result = stfMonitorSvcCxn.Channel.IsValidDirectoryPath(destination_TextBox.Text);
                    if (!result)
                    {
                        message = "Invalid file path";
                    }
                }
            }
            catch (CommunicationException ex)
            {
                message = ex.Message;
            }

            _errorProvider.SetError(label_Destination, message);
            e.Cancel = !result;
        }

        private void ValidatePluginOutputPath(ref CancelEventArgs e)
        {
            string path = $@"\\{_serverHostName}.{GlobalSettings.Items[Setting.DnsDomain]}\{destination_TextBox.Text}";
            bool goodPath = System.IO.Directory.Exists(path);

            _errorProvider.SetError(label_Destination, goodPath ? string.Empty : $"Invalid file path ({path})");
            e.Cancel = !goodPath;
        }

        private bool HasValue(TextBox textBox, Label label, CancelEventArgs e)
        {
            ValidationResult result = FieldValidator.HasValue(textBox, label);

            _errorProvider.SetError(label, result.Message);
            e.Cancel = !result.Succeeded;
            return result.Succeeded;
        }

        private bool IsEnterpriseEnabled()
        {
            bool isEnterpriseEnabled = false;

            try
            {
                isEnterpriseEnabled = GlobalSettings.Items[Setting.EnterpriseEnabled].Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
            catch (SettingNotFoundException)
            {} //No-op here.  If EnterpriseEnabled is not found, isEnterpriseEnabled == false.

            return GlobalSettings.IsDistributedSystem || isEnterpriseEnabled;
        }

        /// <summary>
        /// Determines if the path is a folder path used for DirtyDS plugin.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsFolderPath(string path)
        {
            return !path.Contains(":");
        }

        private void retention_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_monitorType == null)
            {
                return;
            }

            switch (EnumUtil.GetByDescription<RetentionOption>(retention_ComboBox.SelectedItem.ToString()))
            {
                case RetentionOption.DoNothing:
                case RetentionOption.NeverRetain:
                    retentionLocation_TextBox.Enabled = false;
                    retentionLocation_TextBox.CausesValidation = false;
                    _errorProvider.SetError(retentionLocation_TextBox, string.Empty);
                    break;

                default:
                    retentionLocation_TextBox.Enabled = true;
                    retentionLocation_TextBox.CausesValidation = true;
                    break;
            }
        }

        private void lookForMetadata_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            metadataExtension_TextBox.Enabled = lookForMetadata_CheckBox.Checked;
            if (lookForMetadata_CheckBox.Checked)
            {
                metadataExtension_TextBox.CausesValidation = true;
            }
            else
            {
                metadataExtension_TextBox.CausesValidation = false;
                _errorProvider.SetError(metadataExtension_TextBox, string.Empty);
            }
            _outputMonitorConfig.LookForMetadataFile = lookForMetadata_CheckBox.Checked;
        }

        private void metadataExtension_TextBox_Validating(object sender, CancelEventArgs e)
        {
            if (!IsEnterpriseEnabled())
            {
                return;
            }

            if (HasValue(metadataExtension_TextBox, label_MetadataExtension, e))
            {
                _outputMonitorConfig.MetadataFileExtension = metadataExtension_TextBox.Text;
            }
        }

    }
}
