using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Plugin.Kiosk.Options;
using HP.ScalableTest.Utility;
using System.Linq;
using HP.ScalableTest.Plugin.Kiosk.Controls;

namespace HP.ScalableTest.Plugin.Kiosk
{
    [ToolboxItem(false)]
    public partial class KioskConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private KioskCopyOptions _coptions = new KioskCopyOptions();
        private KioskPrintOptions _poptions = new KioskPrintOptions();
        private KioskScanOptions _soptions = new KioskScanOptions();        

        private string _textJobBuildPageCoundCaption = "Select the number of pages for the scanned document.";
        private string _textFolderPathCaption = "Write the path for the printing document.";        
        private string _textJobBuildPageCount = "Job Build Page Count";
        private string _textFolderPath = "Folder Path";
        private string _textSource = "Source:";
        private string _textDestination = "Destination:";

        public object Enumutil { get; }

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="KioskConfigurationControl" /> class.
        /// </summary>
        public KioskConfigurationControl()
        {
            InitializeComponent();

            pwd_TextBox.PasswordChar = '●';

            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireValue(id_TextBox, id_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(pwd_TextBox, pwd_Label, ValidationCondition.IfEnabled);
            fieldValidator.RequireValue(path_textBox, path_label, ValidationCondition.IfEnabled);

            authtype_ComboBox.DataSource = EnumUtil.GetDescriptions<KioskAuthType>().ToList();
            printSource_comboBox.DataSource = EnumUtil.GetDescriptions<KioskPrintSource>().ToList();
            scanDestination_comboBox.DataSource = EnumUtil.GetDescriptions<KioskScanDestination>().ToList();

            authtype_ComboBox.SelectedValueChanged += (s, e) => ConfigurationChanged(s, e);
            id_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pwd_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);

            copy_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            print_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            scan_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);

            lockTimeoutControl.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            ConfigureControls(new KioskActivityData());
            assetSelectionControl.Initialize(DeviceAttributes);
        }

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            var data = configuration.GetMetadata<KioskActivityData>();
            ConfigureControls(data);
            _coptions = data.CopyOptions;
            _poptions = data.PrintOptions;
            _soptions = data.ScanOptions;
            lockTimeoutControl.Initialize(data.LockTimeouts);

            //JobType Enum
            if (data.JobType.Equals(KioskJobType.Copy))
            {
                copy_RadioButton.Checked = true;
                print_RadioButton.Checked = false;
                scan_RadioButton.Checked = false;
            }
            else if (data.JobType.Equals(KioskJobType.Print))
            {
                copy_RadioButton.Checked = false;
                print_RadioButton.Checked = true;
                scan_RadioButton.Checked = false;
            }
            else if (data.JobType.Equals(KioskJobType.Scan))
            {
                copy_RadioButton.Checked = false;
                print_RadioButton.Checked = false;
                scan_RadioButton.Checked = true;
            }

            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            KioskActivityData _activityData = new KioskActivityData()
            {
                AuthType = EnumUtil.GetByDescription<KioskAuthType>(authtype_ComboBox.SelectedItem.ToString()),
                ID = id_TextBox.Text,
                Password = pwd_TextBox.Text,

                CopyOptions = _coptions,
                PrintOptions = _poptions,
                ScanOptions = _soptions,
                LockTimeouts = lockTimeoutControl.Value
            };

            if (copy_RadioButton.Checked)
            {
                _activityData.JobType = KioskJobType.Copy;
                _activityData.CopyOptions.JobBuildPageCount = (int) jobBuildPageCount_numericUpDown.Value;
            }
            else if (print_RadioButton.Checked)
            {
                _activityData.JobType = KioskJobType.Print;
                _activityData.PrintOptions.PrintSource = EnumUtil.GetByDescription<KioskPrintSource>(printSource_comboBox.Text);
                _activityData.PrintOptions.Path = path_textBox.Text;
            }
            else if (scan_RadioButton.Checked)
            {
                _activityData.JobType = KioskJobType.Scan;
                _activityData.ScanOptions.ScanDestination = EnumUtil.GetByDescription<KioskScanDestination>(scanDestination_comboBox.Text);
                _activityData.ScanOptions.JobBuildPageCount = (int)jobBuildPageCount_numericUpDown.Value;
            }
            
            return new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }


        private void ConfigureControls(KioskActivityData data)
        {
            authtype_ComboBox.Text = data.AuthType.GetDescription();

            if (data.AuthType.Equals(KioskAuthType.Login))
            {
                id_TextBox.Text = data.ID;
                pwd_TextBox.Text = data.Password;
            }            

            switch (data.JobType)
            {
                case KioskJobType.Copy:
                    jobBuildPageCount_numericUpDown.Value = data.CopyOptions.JobBuildPageCount;
                    break;
                case KioskJobType.Print:
                    path_textBox.Text = data.PrintOptions.Path;
                    printSource_comboBox.Text = data.PrintOptions.PrintSource.GetDescription();
                    break;
                case KioskJobType.Scan:
                    scanDestination_comboBox.Text = data.ScanOptions.ScanDestination.GetDescription();
                    jobBuildPageCount_numericUpDown.Value = data.ScanOptions.JobBuildPageCount;
                    break;
            }

            lockTimeoutControl.Initialize(data.LockTimeouts);
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
            if (copy_RadioButton.Checked)
            {
                using (var preferences = new KioskCopyOptionsForm(_coptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _coptions = preferences.KioskCopyOptions;
                    }
                }                
            }
            else if (print_RadioButton.Checked)
            {
                _poptions.PrintSource = EnumUtil.GetByDescription<KioskPrintSource>(printSource_comboBox.Text);
                
                using (var preferences = new KioskPrintOptionsForm(_poptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _poptions = preferences.KioskPrintOptions;
                    }
                }                
            }
            else if (scan_RadioButton.Checked)
            {
                _soptions.ScanDestination = EnumUtil.GetByDescription<KioskScanDestination>(scanDestination_comboBox.Text);

                using (var preferences = new KioskScanOptionsForm(_soptions))
                {
                    if (preferences.ShowDialog() == DialogResult.OK)
                    {
                        _soptions = preferences.KioskScanOptions;
                    }
                }                
            }
            else
            {
                return;
            }
        }

        private void authtype_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            KioskAuthType authtype = EnumUtil.GetByDescription<KioskAuthType>(authtype_ComboBox.Text);

            switch (authtype)
            {
                case KioskAuthType.Card :
                    id_TextBox.Enabled = false;
                    pwd_TextBox.Enabled = false;
                    id_Label.Enabled = false;
                    pwd_Label.Enabled = false;
                    break;
                case KioskAuthType.Login :
                    id_TextBox.Enabled = true;
                    pwd_TextBox.Enabled = true;
                    id_Label.Enabled = true;
                    pwd_Label.Enabled = true;
                    break;
            }            
        }

        private void copy_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pageCount_groupBox.Text = _textJobBuildPageCount;
            path_label.Visible = false;
            path_label.Enabled = false;
            path_textBox.Visible = false;
            path_textBox.Enabled = false;
            jobBuildPageCount_label.Visible = true;
            jobBuildPageCount_numericUpDown.Visible = true;
            jobBuildPageCountCaption_label.Text = _textJobBuildPageCoundCaption;

            scanDestination_comboBox.Enabled = false;
            printSource_comboBox.Enabled = false;

            job_label.Text = KioskJobType.Copy.GetDescription();
            sourceDestination_label.Visible = false;
            sourceDestinationCaption_label.Visible = false;

        }

        private void print_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            pageCount_groupBox.Text = _textFolderPath;
            path_label.Visible = true;
            path_label.Enabled = true;
            path_textBox.Visible = true;
            path_textBox.Enabled = true;
            jobBuildPageCount_label.Visible = false;
            jobBuildPageCount_numericUpDown.Visible = false;
            jobBuildPageCountCaption_label.Text = _textFolderPathCaption; 

            scanDestination_comboBox.Enabled = false;
            printSource_comboBox.Enabled = true;

            job_label.Text = KioskJobType.Print.GetDescription();
            sourceDestination_label.Visible = true;
            sourceDestinationCaption_label.Visible = true;
            sourceDestination_label.Text = printSource_comboBox.Text;
            sourceDestinationCaption_label.Text = _textSource;

            options_Button.Enabled = true;

            KioskPrintSource printsource = EnumUtil.GetByDescription<KioskPrintSource>(printSource_comboBox.Text);

            switch (printsource)
            {
                case KioskPrintSource.PrinterOn:
                    options_Button.Enabled = false;
                    break;
                case KioskPrintSource.USB:
                    options_Button.Enabled = true;
                    break;
            }
        }

        private void scan_RadioButton_CheckedChanged(object sender, EventArgs e)
        {            
            pageCount_groupBox.Text = _textJobBuildPageCount;
            path_label.Visible = false;
            path_label.Enabled = false;
            path_textBox.Visible = false;
            path_textBox.Enabled = false;
            jobBuildPageCount_label.Visible = true;
            jobBuildPageCount_numericUpDown.Visible = true;
            jobBuildPageCountCaption_label.Text = _textJobBuildPageCoundCaption;

            scanDestination_comboBox.Enabled = true;
            printSource_comboBox.Enabled = false;

            job_label.Text = KioskJobType.Scan.GetDescription();
            sourceDestination_label.Visible = true;
            sourceDestinationCaption_label.Visible = true;
            sourceDestination_label.Text = scanDestination_comboBox.Text;
            sourceDestinationCaption_label.Text = _textDestination;

            options_Button.Enabled = true;
        }

        private void PrintSource_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceDestination_label.Text = printSource_comboBox.Text;

            KioskPrintSource printsource = EnumUtil.GetByDescription<KioskPrintSource>(printSource_comboBox.Text);

            switch (printsource)
            {
                case KioskPrintSource.PrinterOn:
                    options_Button.Enabled = false;
                    break;
                case KioskPrintSource.USB:
                    options_Button.Enabled = true;
                    break;
            }
        }

        private void ScanDestination_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceDestination_label.Text = scanDestination_comboBox.Text;
        }
    }
} 