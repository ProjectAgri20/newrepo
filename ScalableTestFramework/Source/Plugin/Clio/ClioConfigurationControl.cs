using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio;
using HP.ScalableTest.DeviceAutomation.Enums;

namespace HP.ScalableTest.Plugin.Clio
{
    [ToolboxItem(false)]
    public partial class ClioConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ClioActivityData _data;
        private LinkScanOptions _soptions = new LinkScanOptions();
        private LinkPrintOptions _poptions = new LinkPrintOptions();
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClioConfigurationControl" /> class.
        /// </summary>
        public ClioConfigurationControl()
        {
            InitializeComponent();
            pwd_TextBox.PasswordChar = '●';
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(id_TextBox, id_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(pwd_TextBox, pwd_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(matter_TextBox, matter_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(path_TextBox, path_Label, print_RadioButton);

            comboBox_SIO.DataSource = EnumUtil.GetDescriptions<SIOMethod>().ToList();
            comboBox_SIO.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            comboBox_Logout.DataSource = EnumUtil.GetDescriptions<LogOutMethod>().ToList();
            comboBox_Logout.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            destination_comboBox.DataSource = EnumUtil.GetDescriptions<StorageLocation>().ToList();
            destination_comboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);

            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _data = new ClioActivityData();
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
            _data = configuration.GetMetadata<ClioActivityData>();
            lockTimeoutControl.Initialize(_data.LockTimeouts);
            ConfigureControls(_data);
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Configuration control with defined value.(Load)
        /// </summary>      
        /// <param name="data">ClioActivityData form.</param>
        private void ConfigureControls(ClioActivityData data)
        {
            comboBox_SIO.Text = data.SIO.GetDescription();
            id_TextBox.Text = data.ID;
            pwd_TextBox.Text = data.PW;
            comboBox_Logout.Text = data.LogOut.GetDescription();
            destination_comboBox.Text = EnumUtil.GetDescription(data.Location);
            matter_TextBox.Text = data.Matter;
            path_TextBox.Text = data.FolderPath;
            jobBuildPageCount_NumericUpDown.Value = data.PageCount;

            if (data.JobType.Equals(ClioJobType.Scan))
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
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data.SIO = EnumUtil.GetByDescription<SIOMethod>(comboBox_SIO.SelectedItem.ToString());
            _data.ID = id_TextBox.Text;
            _data.PW = pwd_TextBox.Text;
            _data.LogOut = EnumUtil.GetByDescription<LogOutMethod>(comboBox_Logout.SelectedItem.ToString());
            _data.Location = EnumUtil.GetByDescription<StorageLocation>(destination_comboBox.Text);
            _data.ScanOptions = _soptions;
            _data.PrintOptions = _poptions;
            _data.Matter = matter_TextBox.Text;
            _data.FolderPath = path_TextBox.Text;
            _data.LockTimeouts = lockTimeoutControl.Value;
            _data.LockTimeouts = lockTimeoutControl.Value;

            if (scan_RadioButton.Checked)
            {
                _data.ScanOptions.AppName = "Clio";
                _data.JobType = ClioJobType.Scan;
                _data.PageCount = (int)jobBuildPageCount_NumericUpDown.Value;
            }
            else if (print_RadioButton.Checked)
            {
                _data.PrintOptions.AppName = "Clio";
                _data.JobType = ClioJobType.Print;
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
                path_Label.Enabled = true;
                path_Label.Visible = true;
                jobBuildPageCount_Label.Enabled = true;
                jobBuildPageCount_NumericUpDown.Enabled = true;
                job_Label.Text = "Scan";
            }
        }
        private void print_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (print_RadioButton.Checked)
            {
                jobBuildPageCount_Label.Enabled = false;
                jobBuildPageCount_NumericUpDown.Enabled = false;
                job_Label.Text = "Print";
            }
        }

        private void comboBox_SIO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_SIO.Text.Equals(SIOMethod.SIOWithoutIDPWD.GetDescription()))
            {
                id_TextBox.Enabled = false;
                pwd_TextBox.Enabled = false;
            }
            else
            {
                id_TextBox.Enabled = true;
                pwd_TextBox.Enabled = true;
            }
        }
    }
}
