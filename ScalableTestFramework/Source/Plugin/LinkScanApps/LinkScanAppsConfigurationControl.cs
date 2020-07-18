using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.LinkScanApps
{
    [ToolboxItem(false)]
    public partial class LinkScanAppsConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private LinkScanOptions _scanOptions = new LinkScanOptions();
        private LinkScanAppsActivityData _data;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkScanAppsConfigurationControl" /> class.
        /// </summary>
        public LinkScanAppsConfigurationControl()
        {
            InitializeComponent();

            destination_comboBox.DataSource = EnumUtil.GetDescriptions<LinkScanDestination>().ToList();

            password_textBox.PasswordChar = '●';

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(userName_textBox, userName_label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(server_textBox, server_label, ValidationCondition.IfEnabled);

            destination_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
            fileName_checkBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            fileName_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pageCount_numericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);

            from_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            to_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            cc_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            bcc_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            subject_checkBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            subject_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            message_checkBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            message_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);

            server_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            userName_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            folderPath_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            password_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            port_textBox.TextChanged += (s, e) => ConfigurationChanged(s, e);

            lockTimeoutControl.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new LinkScanAppsActivityData();
            ConfigureControls(_data);
            assetSelectionControl.Initialize(DeviceAttributes);            
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _data = configuration.GetMetadata<LinkScanAppsActivityData>();
            ConfigureControls(_data);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Configuration control with defined value.
        /// </summary>      
        /// <param name="data">LinkScanAppsActivityData form.</param>
        private void ConfigureControls(LinkScanAppsActivityData data)
        {
            _scanOptions = data.ScanOptions;
            destination_comboBox.Text = EnumUtil.GetDescription(data.ScanDestination);
            fileName_checkBox.Checked = data.FileNameIsChecked;
            fileName_textBox.Text = data.FileName;
            pageCount_numericUpDown.Value = data.PageCount;

            switch (data.ScanDestination)
            {
                case LinkScanDestination.Email:
                    from_textBox.Text = data.From;
                    to_textBox.Text = data.To;
                    cc_textBox.Text = data.Cc;
                    bcc_textBox.Text = data.Bcc;
                    subject_checkBox.Checked = data.SubjectIsChecked;
                    subject_textBox.Text = data.Subject;
                    message_checkBox.Checked = data.MessageIsChecked;
                    message_textBox.Text = data.Message;
                    break;
                case LinkScanDestination.SMB:
                case LinkScanDestination.FTP:
                    server_textBox.Text = data.Server;
                    userName_textBox.Text = data.UserName;
                    password_textBox.Text = data.Password;
                    folderPath_textBox.Text = data.FolderPath;
                    port_textBox.Text = data.DomainPort;
                    break;
            }
            lockTimeoutControl.Initialize(data.LockTimeouts);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data = new LinkScanAppsActivityData()
            {
                ScanDestination = EnumUtil.GetByDescription<LinkScanDestination>(destination_comboBox.Text),
                FileNameIsChecked = fileName_checkBox.Checked,
                FileName = fileName_textBox.Text,
                PageCount = (int)pageCount_numericUpDown.Value,
                LockTimeouts = lockTimeoutControl.Value
            };
            _data.ScanOptions.PageCount = _data.PageCount;

            switch (_data.ScanDestination)
            {
                case LinkScanDestination.Email:
                    _data.From = from_textBox.Text;
                    _data.To = to_textBox.Text;
                    _data.Cc = cc_textBox.Text;
                    _data.Bcc = bcc_textBox.Text;
                    _data.SubjectIsChecked = subject_checkBox.Checked;
                    _data.Subject = subject_textBox.Text;
                    _data.MessageIsChecked = message_checkBox.Checked;
                    _data.Message = message_textBox.Text;                    
                    break;
                case LinkScanDestination.SMB:
                case LinkScanDestination.FTP:
                    _data.Server = server_textBox.Text;
                    _data.UserName = userName_textBox.Text;
                    _data.Password = password_textBox.Text;
                    _data.FolderPath = folderPath_textBox.Text;
                    _data.DomainPort = port_textBox.Text;
                    break;
            }
            _scanOptions.AppName = $"Scan to {destination_comboBox.Text}";
            _data.ScanOptions = _scanOptions;

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns><see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Configuration change registration.
        /// </summary>
        /// <param name="sender">object</param>.
        /// <param name="e">Event Arugument</param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        /// <summary>
        /// combobox selected index change function control.
        /// </summary>
        /// <param name="sender">object</param>.
        /// <param name="e">Event Arugument</param>
        private void destination_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(EnumUtil.GetByDescription<LinkScanDestination>(destination_comboBox.Text))
            {
                case LinkScanDestination.Email:
                    scantoNetwork_groupBox.Enabled = false;
                    scantoNetwork_groupBox.Visible = false;
                    scantoEmail_groupBox.Enabled = true;
                    scantoEmail_groupBox.Visible = true;
                    break;
                case LinkScanDestination.SMB:
                    scantoNetwork_groupBox.Enabled = true;
                    scantoNetwork_groupBox.Visible = true;
                    scantoEmail_groupBox.Enabled = false;
                    scantoEmail_groupBox.Visible = false;
                    folderpath_label.Visible = false;
                    folderPath_textBox.Visible = false;

                    domainPort_label.Text = "Domain:";
                    break;
                case LinkScanDestination.FTP:
                    scantoNetwork_groupBox.Enabled = true;
                    scantoNetwork_groupBox.Visible = true;
                    scantoEmail_groupBox.Enabled = false;
                    scantoEmail_groupBox.Visible = false;
                    folderpath_label.Visible = true;
                    folderPath_textBox.Visible = true;

                    domainPort_label.Text = "Port:";
                    break;
            }
        }

        /// <summary>
        /// OK button control.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">Event Arugument</param>
        private void options_button_Click(object sender, EventArgs e)
        {
            using (var preferences = new LinkScanOptionsForm(_scanOptions))
            {
                if (preferences.ShowDialog() == DialogResult.OK)
                {
                    _scanOptions = preferences.LinkScanOption;
                }
            }
        }

        private void checkBox_FileName_CheckedChanged(object sender, EventArgs e)
        {
            if (fileName_checkBox.Checked)
            {
                _data.FileNameIsChecked = true;
                fileName_textBox.Enabled = true;
            }
            else
            {
                _data.FileNameIsChecked = false;
                fileName_textBox.Enabled = false;
            }
        }

        private void subject_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (subject_checkBox.Checked)
            {
                _data.SubjectIsChecked = true;
                subject_textBox.Enabled = true;
            }
            else
            {
                _data.SubjectIsChecked = false;
                subject_textBox.Enabled = false;
            }
        }

        private void message_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (message_checkBox.Checked)
            {
                _data.MessageIsChecked = true;
                message_textBox.Enabled = true;
            }
            else
            {
                _data.MessageIsChecked = false;
                message_textBox.Enabled = false;
            }
        }
    }
}
