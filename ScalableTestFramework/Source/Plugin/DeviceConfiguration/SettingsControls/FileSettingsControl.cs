using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    public partial class FileSettingsControl : UserControl
    {
        private readonly FileSettings _fileSettings;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public FileSettingsControl()
        {
            InitializeComponent();
            _fileSettings = new FileSettings();

            prefix_choiceComboControl.choice_Combo.DataSource = ListValues.FilePrefix;
            suffix_choiceComboControl.choice_Combo.DataSource = ListValues.FileSuffix;

            SetChoiceControlDataSource(fileType_choiceComboControl, ListValues.FileType);
            SetChoiceControlDataSource(fileNumbering_choiceComboControl, ListValues.FileNumbering);
            SetChoiceControlDataSource(resolution_choiceComboControl,ListValues.Resolution);
            SetChoiceControlDataSource(fileSize_choiceComboControl, ListValues.FileSizeList);
            SetChoiceControlDataSource(color_choiceComboControl, ListValues.ChromaticModes);

            AddEventHandlers();
        }

        public void SetData(FileSettings fileSettings)
        {
            RemoveEventHandlers();
            fileName_choiceTextControl.onOff_CheckBox.Checked = fileSettings.FileName.Value;
            fileName_choiceTextControl.text_Box.Text = fileSettings.FileName.Key;

            prefix_choiceComboControl.onOff_CheckBox.Checked = fileSettings.FileNamePrefix.Value;
            prefix_choiceComboControl.choice_Combo.SelectedItem = fileSettings.FileNamePrefix.Key;

            suffix_choiceComboControl.onOff_CheckBox.Checked = fileSettings.FileNameSuffix.Value;
            suffix_choiceComboControl.choice_Combo.SelectedItem = fileSettings.FileNameSuffix.Key;

            SetComboBoxData(fileSettings.FileType, fileType_choiceComboControl, ListValues.FileType);
            SetComboBoxData(fileSettings.FileNumbering, fileNumbering_choiceComboControl, ListValues.FileNumbering);
            SetComboBoxData(fileSettings.Resolution, resolution_choiceComboControl,ListValues.Resolution);
            SetComboBoxData(fileSettings.FileSize, fileSize_choiceComboControl, ListValues.FileSizeList);
            SetComboBoxData(fileSettings.FileColor, color_choiceComboControl, ListValues.ChromaticModes);
            AddEventHandlers();
        }

        public FileSettings GetData()
        {
            _fileSettings.FileName.Value = fileName_choiceTextControl.onOff_CheckBox.Checked;
            _fileSettings.FileName.Key = fileName_choiceTextControl.text_Box.Text;

            _fileSettings.FileNamePrefix = GetComboBoxData(prefix_choiceComboControl);
            _fileSettings.FileNameSuffix = GetComboBoxData(suffix_choiceComboControl);
            _fileSettings.FileType = GetComboBoxData(fileType_choiceComboControl);
            _fileSettings.FileNumbering = GetComboBoxData(fileNumbering_choiceComboControl);
            _fileSettings.Resolution = GetComboBoxData(resolution_choiceComboControl);
            _fileSettings.FileSize = GetComboBoxData(fileSize_choiceComboControl);
            _fileSettings.FileColor = GetComboBoxData(color_choiceComboControl);
            return _fileSettings;   
        }

        private DataPair<string> GetComboBoxData(ChoiceComboControl choiceComboControl)
        {
            return new DataPair<string>
            {
                Key = choiceComboControl.choice_Combo.SelectedValue.ToString(),
                Value = choiceComboControl.onOff_CheckBox.Checked
            };
        }

        private void SetComboBoxData(DataPair<string> dataPair, ChoiceComboControl choiceComboControl,
            Dictionary<string, string> dictionary)
        {
            choiceComboControl.onOff_CheckBox.Checked = dataPair.Value;

            choiceComboControl.choice_Combo.SelectedItem = dictionary != null
                ? (object)dictionary.FirstOrDefault(x => x.Value == dataPair.Key)
                : dataPair.Key;
        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary)
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        private void AddEventHandlers()
        {
            fileName_choiceTextControl.text_Box.TextChanged += OnControlComponentChanged;
            fileName_choiceTextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            AddChoiceComboControlEventHandler(prefix_choiceComboControl);
            AddChoiceComboControlEventHandler(suffix_choiceComboControl);
            AddChoiceComboControlEventHandler(fileType_choiceComboControl);
            AddChoiceComboControlEventHandler(fileNumbering_choiceComboControl);
            AddChoiceComboControlEventHandler(resolution_choiceComboControl);
            AddChoiceComboControlEventHandler(fileSize_choiceComboControl);
            AddChoiceComboControlEventHandler(color_choiceComboControl);
           
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            fileName_choiceTextControl.text_Box.TextChanged -= OnControlComponentChanged;
            fileName_choiceTextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            RemoveChoiceComboControlEventHandler(prefix_choiceComboControl);
            RemoveChoiceComboControlEventHandler(suffix_choiceComboControl);
            RemoveChoiceComboControlEventHandler(fileType_choiceComboControl);
            RemoveChoiceComboControlEventHandler(fileNumbering_choiceComboControl);
            RemoveChoiceComboControlEventHandler(resolution_choiceComboControl);
            RemoveChoiceComboControlEventHandler(fileSize_choiceComboControl);
            RemoveChoiceComboControlEventHandler(color_choiceComboControl);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}
