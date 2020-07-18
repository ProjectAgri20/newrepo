using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.FieldControls;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsData;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    [ToolboxItem(false)]
    public partial class ScanToUsbDefaultControl : UserControl, IGetSetComponentData
    {

        private ScanUsbSettingData _scanUsbSettingData;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public ScanToUsbDefaultControl()
        {
            InitializeComponent();
            _scanUsbSettingData = new ScanUsbSettingData();
            scanSettingsUserControl.ControlComponentChanged += OnControlComponentChanged;
            fileSettingsControl.ControlComponentChanged += OnControlComponentChanged;
            enable_ComboBox.choice_Combo.DataSource = ListValues.EnableScanToEmailValues;

            AddEventHandlers();
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

        public IComponentData GetData()
        {
            return _scanUsbSettingData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
           RemoveEventHandlers();
            _scanUsbSettingData = list.OfType<ScanUsbSettingData>().FirstOrDefault();

            enable_ComboBox.onOff_CheckBox.Checked = _scanUsbSettingData.EnableScanToUsb.Value;
            enable_ComboBox.choice_Combo.SelectedItem = _scanUsbSettingData.EnableScanToUsb.Key;

            scanSettingsUserControl.SetData(_scanUsbSettingData.ScanSettingsData);
            fileSettingsControl.SetData(_scanUsbSettingData.FileSettingsData);
            AddEventHandlers();
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(enable_ComboBox, _scanUsbSettingData.EnableScanToUsb);
            _scanUsbSettingData.ScanSettingsData = scanSettingsUserControl.GetData();
            _scanUsbSettingData.FileSettingsData = fileSettingsControl.GetData();
        }

        private void GetDataFromChoiceComboBox(ChoiceComboControl choiceComboControl, DataPair<string> dataPair)
        {
            dataPair.Key = choiceComboControl.choice_Combo.SelectedValue.ToString();
            dataPair.Value = choiceComboControl.onOff_CheckBox.Checked;
        }

        private void AddEventHandlers()
        {
            AddChoiceComboControlEventHandler(enable_ComboBox);
            
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(enable_ComboBox);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}
