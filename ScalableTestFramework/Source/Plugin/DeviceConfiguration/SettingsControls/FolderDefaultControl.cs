using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class FolderDefaultControl : UserControl, IGetSetComponentData
    {
        private FolderSettingsData _folderSettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;

        public FolderDefaultControl()
        {
            InitializeComponent();
            _folderSettingsData = new FolderSettingsData();

            scanSettingsUserControl.ControlComponentChanged += OnControlComponentChanged;
            fileSettingsControl1.ControlComponentChanged += OnControlComponentChanged;
            enable_ComboBox.choice_Combo.DataSource = ListValues.EnableScanToEmailValues;
            SetChoiceControlDataSource(folder_choiceComboControl, ListValues.FolderAccessType);
            SetChoiceControlDataSource(cropping_choiceComboControl,ListValues.CroppingOptions);

            AddEventHandlers();
        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary)
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        public IComponentData GetData()
        {
            return _folderSettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _folderSettingsData = list.OfType<FolderSettingsData>().FirstOrDefault();

            if (_folderSettingsData != null)
            {
                enable_ComboBox.onOff_CheckBox.Checked = _folderSettingsData.EnableScanToFolder.Value;
                enable_ComboBox.choice_Combo.SelectedItem = _folderSettingsData.EnableScanToFolder.Key;

                folder_choiceComboControl.onOff_CheckBox.Checked = _folderSettingsData.Folder.Value;
                folder_choiceComboControl.choice_Combo.SelectedItem =ListValues.FolderAccessType.FirstOrDefault(x => x.Value == _folderSettingsData.Folder.Key);
                
                cropping_choiceComboControl.onOff_CheckBox.Checked = _folderSettingsData.CroppingOption.Value;
                cropping_choiceComboControl.choice_Combo.SelectedItem = ListValues.CroppingOptions.FirstOrDefault(x => x.Value == _folderSettingsData.CroppingOption.Key);

                scanSettingsUserControl.SetData(_folderSettingsData.ScanSettingsData);
                fileSettingsControl1.SetData(_folderSettingsData.FileSettingsData);
            }
            AddEventHandlers();
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(enable_ComboBox, _folderSettingsData.EnableScanToFolder);
            GetDataFromChoiceComboBox(folder_choiceComboControl, _folderSettingsData.Folder);
            _folderSettingsData.ScanSettingsData = scanSettingsUserControl.GetData();
            GetDataFromChoiceComboBox(cropping_choiceComboControl, _folderSettingsData.CroppingOption);
            _folderSettingsData.FileSettingsData = fileSettingsControl1.GetData();
        }

        private void GetDataFromChoiceComboBox(ChoiceComboControl choiceComboControl, DataPair<string> dataPair)
        {
            dataPair.Key = choiceComboControl.choice_Combo.SelectedValue.ToString();
            dataPair.Value = choiceComboControl.onOff_CheckBox.Checked;
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        private void AddEventHandlers()
        {
            AddChoiceComboControlEventHandler(enable_ComboBox);
            AddChoiceComboControlEventHandler(folder_choiceComboControl);
            AddChoiceComboControlEventHandler(cropping_choiceComboControl);
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(enable_ComboBox);
            RemoveChoiceComboControlEventHandler(folder_choiceComboControl);
            RemoveChoiceComboControlEventHandler(cropping_choiceComboControl);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}