using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage;

namespace HP.ScalableTest.Plugin.iManage
{
    [ToolboxItem(false)]
    public partial class iManageConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private iManageActivityData _data;
        private LinkScanOptions _soptions = new LinkScanOptions();
        private LinkPrintOptions _poptions = new LinkPrintOptions();
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="iManageConfigurationControl" /> class.
        /// </summary>
        public iManageConfigurationControl()
        {
            InitializeComponent();
            pwd_TextBox.PasswordChar = '●';
            destination_comboBox.DataSource = EnumUtil.GetDescriptions<StorageLocation>().ToList();
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(id_TextBox, id_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(pwd_TextBox, pwd_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(path_textBox, path_label, ValidationCondition.IfEnabled);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);

            comboBox_SIO.DataSource = EnumUtil.GetDescriptions<SIOMethod>().ToList();
            comboBox_SIO.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            comboBox_Logout.DataSource = EnumUtil.GetDescriptions<LogOutMethod>().ToList();
            comboBox_Logout.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            destination_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new iManageActivityData();
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
            _data = configuration.GetMetadata<iManageActivityData>();
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            ConfigureControls(_data);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Configuration control with defined value.(Load)
        /// </summary>      
        /// <param name="data">LinkScanAppsActivityData form.</param>
        private void ConfigureControls(iManageActivityData data)
        {
            comboBox_SIO.Text = data.SIO.GetDescription();
            id_TextBox.Text = data.ID;
            pwd_TextBox.Text = data.Password;
            comboBox_Logout.Text = data.LogOut.GetDescription();
            path_textBox.Text = data.FolderPath;
            jobBuildPageCount_numericUpDown.Value = data.PageCount;
            destination_comboBox.Text = EnumUtil.GetDescription(data.Location);

            if (data.JobType.Equals(iManageJobType.Scan))
            {
                scan_RadioButton.Checked = true;
                _soptions = data.ScanOptions;
            }
            else
            {
                print_RadioButton.Checked = true;
                _poptions = data.PrintOptions;
            }
            lockTimeoutControl.Initialize(data.LockTimeouts);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.(Save)
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.SIO = EnumUtil.GetByDescription<SIOMethod>(comboBox_SIO.SelectedItem.ToString());
            _data.ID = id_TextBox.Text;
            _data.Password = pwd_TextBox.Text;
            _data.LogOut = EnumUtil.GetByDescription<LogOutMethod>(comboBox_Logout.SelectedItem.ToString());
            _data.ScanOptions = _soptions;
            _data.PrintOptions = _poptions;
            _data.FolderPath = path_textBox.Text;
            _data.LockTimeouts = lockTimeoutControl.Value;
            _data.Location = EnumUtil.GetByDescription<StorageLocation>(destination_comboBox.Text);
            _data.LockTimeouts = lockTimeoutControl.Value;

            if (scan_RadioButton.Checked)
            {
                _data.ScanOptions.AppName = $"iManage";
                _data.JobType = iManageJobType.Scan;
                _data.PageCount = (int)jobBuildPageCount_numericUpDown.Value;
            }
            else if (print_RadioButton.Checked)
            {
                _data.PrintOptions.AppName = $"iManage";
                _data.JobType = iManageJobType.Print;
            }

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
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

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }

        private void options_Button_Click(object sender, EventArgs e)
        {
            if (print_RadioButton.Checked)
            {
                using (var preferences = new LinkPrintOptionsForm(_poptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _poptions = preferences.LinkPrintOption;
                    }
                }
            }
            else
            {
                using (var preferences = new LinkScanOptionsForm(_soptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _soptions = preferences.LinkScanOption;
                    }
                }
            }
        }

        private void scan_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (scan_RadioButton.Checked)
            {
                path_label.Enabled = true;
                path_label.Visible = true;
                path_textBox.Enabled = true;
                path_textBox.Visible = true;
                jobBuildPageCount_label.Enabled = true;
                jobBuildPageCount_label.Visible = true;
                jobBuildPageCount_numericUpDown.Enabled = true;
                jobBuildPageCount_numericUpDown.Visible = true;
                job_label.Text = "Scan";
            }
        }

        private void print_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (print_RadioButton.Checked)
            {
                jobBuildPageCount_label.Enabled = false;
                jobBuildPageCount_label.Visible = false;
                jobBuildPageCount_numericUpDown.Enabled = false;
                jobBuildPageCount_numericUpDown.Visible = false;
                path_textBox.Enabled = true;
                path_textBox.Visible = true;
                job_label.Text = "Print";
            }
        }
    }
    
}
