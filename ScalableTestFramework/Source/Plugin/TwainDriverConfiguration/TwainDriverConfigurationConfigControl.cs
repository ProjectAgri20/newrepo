using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Plugin.TwainDriverConfiguration.Properties;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    [ToolboxItem(false)]
    public partial class TwainDriverConfigurationConfigControl : UserControl, IPluginConfigurationControl
    {
        private TwainDriverActivityData _data;
        private const AssetAttributes DeviceAttributes = AssetAttributes.Scanner | AssetAttributes.ControlPanel;
        private TwainConfiguration _selectedconfiguration;
        private TwainOperation _selectedOperation;

        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// constructor
        /// </summary>      
        public TwainDriverConfigurationConfigControl()
        {
            InitializeComponent();
            itemType_ComboBox.DataSource = EnumUtil.GetDescriptions<ItemType>().ToList();
            pageSides_ComboBox.DataSource = EnumUtil.GetDescriptions<PageSides>().ToList();
            pageSize_ComboBox.DataSource = EnumUtil.GetDescriptions<PageSize>().ToList();
            source_ComboBox.DataSource = EnumUtil.GetDescriptions<Source>().ToList();
            colorMode_ComboBox.DataSource = EnumUtil.GetDescriptions<ColorMode>().ToList();
            fileType_ComboBox.DataSource = EnumUtil.GetDescriptions<FileType>().ToList();
            sendTo_ComboBox.DataSource = EnumUtil.GetDescriptions<SendTo>().ToList();
            shortcutSettings_ComboBox.DataSource = EnumUtil.GetDescriptions<ShortcutSettings>().ToList();
            install_RadioButton.Checked = true;
            install_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            deviceAddition_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            scanConfiguration_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            setupPath_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            saveAsPDF_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            saveAsJPEG_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            emailAsPDF_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            emailAsJPEG_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            everyDayScan_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            shortcut_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            itemType_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pageSides_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            pageSize_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            source_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            colorMode_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            fileType_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            sendTo_ComboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            shortcutSettings_ComboBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            saveToFolderPath_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            enableReservation_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pinRequired_CheckBox.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            pin_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            emailAddress_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            fieldValidator.RequireValue(pin_TextBox, pin_Label, ValidationCondition.IfEnabled);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainDriverConfigurationConfigControl"/> class.
        /// </summary> 
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value
            ConfigureControls(new TwainDriverActivityData());
            assetSelectionControl.Initialize(DeviceAttributes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainDriverConfigurationConfigControl"/> class.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            ConfigureControls(configuration.GetMetadata<TwainDriverActivityData>());
            assetSelectionControl.Initialize(configuration.Assets, DeviceAttributes);
        }

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The configuration data.</returns>
        public PluginConfigurationData GetConfiguration()
        {
            _data = new TwainDriverActivityData { TwainOperation = _selectedOperation };
            if (_selectedOperation == TwainOperation.Install)
            {
                _data.SetupFileName = setupPath_TextBox.Text;
            }
            else if (_selectedOperation == TwainOperation.ScanConfiguration)
            {
                _data.TwainConfigurations = _selectedconfiguration;
                _data.ItemType = EnumUtil.GetByDescription<ItemType>(itemType_ComboBox.SelectedItem.ToString());
                _data.PageSides = EnumUtil.GetByDescription<PageSides>(pageSides_ComboBox.SelectedItem.ToString());
                _data.PageSize = EnumUtil.GetByDescription<PageSize>(pageSize_ComboBox.SelectedItem.ToString());
                _data.Source = EnumUtil.GetByDescription<Source>(source_ComboBox.SelectedItem.ToString());
                _data.ColorMode = EnumUtil.GetByDescription<ColorMode>(colorMode_ComboBox.SelectedItem.ToString());
                _data.FileType = EnumUtil.GetByDescription<FileType>(fileType_ComboBox.SelectedItem.ToString());
                _data.SendTo = EnumUtil.GetByDescription<SendTo>(sendTo_ComboBox.SelectedItem.ToString());
                _data.ShortcutSettings = EnumUtil.GetByDescription<ShortcutSettings>(shortcutSettings_ComboBox.SelectedItem.ToString());
                _data.SharedFolderPath = saveToFolderPath_TextBox.Text;
                _data.EmailAddress = emailAddress_TextBox.Text;
                _data.IsPinRequired = pinRequired_CheckBox.Checked;
                _data.IsReservation = enableReservation_CheckBox.Checked;
                _data.Pin = pin_TextBox.Text;
            }

            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
            };
        }

        private void ConfigureControls(TwainDriverActivityData data)
        {
            switch (data.TwainOperation)
            {
                case TwainOperation.Install:
                    install_RadioButton.Checked = true;
                    break;
                case TwainOperation.DeviceAddition:
                    deviceAddition_RadioButton.Checked = true;
                    break;
                case TwainOperation.ScanConfiguration:
                    scanConfiguration_RadioButton.Checked = true;
                    break;
            }

            if (data.TwainOperation == TwainOperation.Install)
            {
                setupPath_TextBox.Text = data.SetupFileName;
            }
            else if (data.TwainOperation == TwainOperation.ScanConfiguration)
            {
                switch (data.TwainConfigurations)
                {
                    case TwainConfiguration.SavesAsPdf:
                        saveAsPDF_RadioButton.Checked = true;
                        break;
                    case TwainConfiguration.SaveAsJpeg:
                        saveAsJPEG_RadioButton.Checked = true;
                        break;
                    case TwainConfiguration.EmailAsPdf:
                        emailAsPDF_RadioButton.Checked = true;
                        break;
                    case TwainConfiguration.EmailAsJpeg:
                        emailAsJPEG_RadioButton.Checked = true;
                        break;
                    case TwainConfiguration.EveryDayScan:
                        everyDayScan_RadioButton.Checked = true;
                        break;
                    case TwainConfiguration.NewScanShortCut:
                        shortcut_RadioButton.Checked = true;
                        break;
                }
                itemType_ComboBox.Text = data.ItemType.GetDescription();
                pageSides_ComboBox.Text = data.PageSides.GetDescription();
                pageSize_ComboBox.Text = data.PageSize.GetDescription();
                source_ComboBox.Text = data.Source.GetDescription();
                colorMode_ComboBox.Text = data.ColorMode.GetDescription();
                fileType_ComboBox.Text = data.FileType.GetDescription();
                sendTo_ComboBox.Text = data.SendTo.GetDescription();
                shortcutSettings_ComboBox.Text = data.ShortcutSettings.GetDescription();
                saveToFolderPath_TextBox.Text = data.SharedFolderPath;
                emailAddress_TextBox.Text = data.EmailAddress;
                enableReservation_CheckBox.Checked = data.IsReservation;
                pinRequired_CheckBox.Checked = data.IsPinRequired;
                pin_TextBox.Text = data.Pin;
                scanConfiguration_RadioButton_CheckedChanged(null, null);
            }
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void browseInstaller_Button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DefaultExt = "*.bat";
                dialog.Filter = Resources.TwainDriverConfigurationConfigControl_filter;
                dialog.Multiselect = false;
                dialog.Title = Resources.TwainDriverConfigurationConfigControl_title;

                if (DialogResult.OK == dialog.ShowDialog())
                {
                    setupPath_TextBox.Text = dialog.FileName;
                }
            }
        }

        private void scanConfiguration_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (saveAsPDF_RadioButton.Checked)
            {
                _selectedconfiguration = TwainConfiguration.SavesAsPdf;
                scanSettings_GroupBox.Visible = true;
                createNewScanShortcut_GroupBox.Visible = false;
                destination_GroupBox.Visible = false;
                networkFolder_GroupBox.Visible = true;
                email_GroupBox.Visible = false;
                reservation_groupBox.Visible = true;
                fieldValidator.RequireValue(saveToFolderPath_TextBox, saveToFolder_Label);
                fieldValidator.Remove(emailAddress_TextBox);
            }
            else if (saveAsJPEG_RadioButton.Checked)
            {
                _selectedconfiguration = TwainConfiguration.SaveAsJpeg;
                scanSettings_GroupBox.Visible = true;
                createNewScanShortcut_GroupBox.Visible = false;
                destination_GroupBox.Visible = false;
                networkFolder_GroupBox.Visible = true;
                email_GroupBox.Visible = false;
                reservation_groupBox.Visible = true;
                fieldValidator.RequireValue(saveToFolderPath_TextBox, saveToFolder_Label);
                fieldValidator.Remove(emailAddress_TextBox);
            }
            else if (emailAsPDF_RadioButton.Checked)
            {
                _selectedconfiguration = TwainConfiguration.EmailAsPdf;
                scanSettings_GroupBox.Visible = true;
                createNewScanShortcut_GroupBox.Visible = false;
                destination_GroupBox.Visible = false;
                networkFolder_GroupBox.Visible = false;
                email_GroupBox.Visible = true;
                reservation_groupBox.Visible = true;
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.RequireValue(emailAddress_TextBox, emailAddress_Label);
            }
            else if (emailAsJPEG_RadioButton.Checked)
            {
                _selectedconfiguration = TwainConfiguration.EmailAsJpeg;
                scanSettings_GroupBox.Visible = true;
                createNewScanShortcut_GroupBox.Visible = false;
                destination_GroupBox.Visible = false;
                networkFolder_GroupBox.Visible = false;
                email_GroupBox.Visible = true;
                reservation_groupBox.Visible = true;
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.RequireValue(emailAddress_TextBox, emailAddress_Label);
            }
            else if (everyDayScan_RadioButton.Checked)
            {
                _selectedconfiguration = TwainConfiguration.EveryDayScan;
                scanSettings_GroupBox.Visible = true;
                createNewScanShortcut_GroupBox.Visible = false;
                destination_GroupBox.Visible = true;
                reservation_groupBox.Visible = true;
                sendTo_ComboBox.Text = SendTo.LocalorNetworkfolder.ToString();
                sendTo_ComboBox_SelectedIndexChanged(null, null);
            }
            else
            {
                _selectedconfiguration = TwainConfiguration.NewScanShortCut;
                createNewScanShortcut_GroupBox.Visible = true;
                destination_GroupBox.Visible = false;
                scanSettings_GroupBox.Visible = false;
                networkFolder_GroupBox.Visible = false;
                email_GroupBox.Visible = false;
                reservation_groupBox.Visible = false;
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.Remove(emailAddress_TextBox);
            }
        }

        private void sendTo_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EnumUtil.GetByDescription<SendTo>(sendTo_ComboBox.SelectedItem.ToString()) == SendTo.LocalorNetworkfolder)
            {
                networkFolder_GroupBox.Visible = true;
                email_GroupBox.Visible = false;
                fieldValidator.RequireValue(saveToFolderPath_TextBox, saveToFolder_Label);
                fieldValidator.Remove(emailAddress_TextBox);
            }
            else
            {
                networkFolder_GroupBox.Visible = false;
                email_GroupBox.Visible = true;
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.RequireValue(emailAddress_TextBox, emailAddress_Label);
            }
        }

        private void itemType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemType_ComboBox.SelectedItem.ToString() == ItemType.Photo.ToString())
            {
                List<string> nList = new List<string> { PageSides.OneSided.GetDescription() };
                pageSides_ComboBox.DataSource = nList;
            }
            else
            {
                pageSides_ComboBox.DataSource = EnumUtil.GetDescriptions<PageSides>().ToList();
            }
            if (source_ComboBox.SelectedItem != null )
            {
                if(source_ComboBox.SelectedItem.ToString() == Source.Flatbed.ToString())
                {
                    List<string> nList = new List<string> { PageSides.OneSided.GetDescription() };
                    pageSides_ComboBox.DataSource = nList;
                }                
            }                    
        }

        private void operation_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (install_RadioButton.Checked)
            {
                _selectedOperation = TwainOperation.Install;
                assetSelectionControl.Location = new System.Drawing.Point(3, 132);
                path_groupBox.Visible = true;
                configuration_GroupBox.Visible = false;
                fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
                fieldValidator.RequireValue(setupPath_TextBox, installerFile_Label);
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.Remove(emailAddress_TextBox);
            }
            else if (deviceAddition_RadioButton.Checked)
            {
                _selectedOperation = TwainOperation.DeviceAddition;
                path_groupBox.Visible = false;
                configuration_GroupBox.Visible = false;
                assetSelectionControl.Location = new System.Drawing.Point(3, 70);
                fieldValidator.RequireAssetSelection(assetSelectionControl, "device");
                fieldValidator.Remove(setupPath_TextBox);
                fieldValidator.Remove(saveToFolderPath_TextBox);
                fieldValidator.Remove(emailAddress_TextBox);
            }
            else
            {
                _selectedOperation = TwainOperation.ScanConfiguration;
                path_groupBox.Visible = false;
                assetSelectionControl.Location = new System.Drawing.Point(3, 70);
                configuration_GroupBox.Visible = true;
                configuration_GroupBox.Location = new System.Drawing.Point(3, 185);
                fieldValidator.Remove(setupPath_TextBox);
                fieldValidator.RequireValue(saveToFolderPath_TextBox, saveToFolder_Label);
            }
        }

        private void enableReservation_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pinRequired_CheckBox.Enabled = enableReservation_CheckBox.Checked;
            pinRequired_CheckBox_CheckedChanged(sender, e);
        }

        private void pinRequired_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pin_TextBox.Enabled = pinRequired_CheckBox.Checked;
        }

        private void pin_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void source_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (source_ComboBox.SelectedItem.ToString() == Source.Flatbed.ToString())
            {
                List<string> nList = new List<string> { PageSides.OneSided.GetDescription() };
                pageSides_ComboBox.DataSource = nList;
            }
            else
            {
                pageSides_ComboBox.DataSource = EnumUtil.GetDescriptions<PageSides>().ToList();
            }
        }
    }
}