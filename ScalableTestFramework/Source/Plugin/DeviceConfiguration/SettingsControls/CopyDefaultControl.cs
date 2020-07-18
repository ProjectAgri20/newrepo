using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System.ComponentModel;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class CopyDefaultControl : UserControl, IGetSetComponentData
    {
        private CopySettingsData _copySettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public CopyDefaultControl()
        {
            InitializeComponent();
            _copySettingsData = new CopySettingsData();

            SetChoiceControlDataSource(color_choiceComboControl, ListValues.ChromaticModes);
            SetChoiceControlDataSource(scanMode_choiceComboControl, ListValues.CaptureModes);
            SetChoiceControlDataSource(collate_choiceComboControl,ListValues.CollateModes);
            SetChoiceControlDataSource(copySides_choiceComboControl, ListValues.CopySidesModes);
            SetChoiceControlDataSource(pps_choiceComboControl, ListValues.PagesPerSheetModes);
            
            scale_choiceNumericControl.choice_numericUpDown.Value = 100;
            scale_choiceNumericControl.choice_numericUpDown.Maximum = 400;
            scale_choiceNumericControl.choice_numericUpDown.Minimum = 25;

            scanSettingsUserControl.ControlComponentChanged += OnControlComponentChanged;
            AddEventHandlers();
        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary )
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        public IComponentData GetData()
        {
            return _copySettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _copySettingsData = list.OfType<CopySettingsData>().FirstOrDefault();

            copies_choiceNumericControl.onOff_CheckBox.Checked = _copySettingsData.Copies.Value;
            copies_choiceNumericControl.choice_numericUpDown.Value = Convert.ToDecimal(_copySettingsData.Copies.Key);

            scale_choiceNumericControl.onOff_CheckBox.Checked = _copySettingsData.ReduceEnlarge.Value;
            scale_choiceNumericControl.choice_numericUpDown.Value = Convert.ToDecimal(_copySettingsData.ReduceEnlarge.Key);

            color_choiceComboControl.onOff_CheckBox.Checked = _copySettingsData.Color.Value;
            color_choiceComboControl.choice_Combo.SelectedItem = _copySettingsData.Color.Key;

            scanMode_choiceComboControl.onOff_CheckBox.Checked = _copySettingsData.ScanMode.Value;
            scanMode_choiceComboControl.choice_Combo.SelectedItem = ListValues.CaptureModes.FirstOrDefault(x => x.Value == _copySettingsData.ScanMode.Key);
            
            collate_choiceComboControl.onOff_CheckBox.Checked = _copySettingsData.Collate.Value;
            collate_choiceComboControl.choice_Combo.SelectedItem =ListValues.CollateModes.FirstOrDefault(x => x.Value == _copySettingsData.Collate.Key);

            copySides_choiceComboControl.onOff_CheckBox.Checked = _copySettingsData.CopySides.Value;
            copySides_choiceComboControl.choice_Combo.SelectedItem = ListValues.CopySidesModes.FirstOrDefault(x => x.Value == _copySettingsData.CopySides.Key);

            pps_choiceComboControl.onOff_CheckBox.Checked = _copySettingsData.PagesPerSheet.Value;
            pps_choiceComboControl.choice_Combo.SelectedItem = ListValues.PagesPerSheetModes.FirstOrDefault(x => x.Value == _copySettingsData.PagesPerSheet.Key);

            scanSettingsUserControl.SetData(_copySettingsData.ScanSettingsData);

            AddEventHandlers();

        }

        public void SetData()
        {
            _copySettingsData.Copies.Key = copies_choiceNumericControl.choice_numericUpDown.Value.ToString(CultureInfo.CurrentCulture);
            _copySettingsData.Copies.Value = copies_choiceNumericControl.onOff_CheckBox.Checked;

            _copySettingsData.ReduceEnlarge.Key = scale_choiceNumericControl.choice_numericUpDown.Value.ToString(CultureInfo.CurrentCulture);
            _copySettingsData.ReduceEnlarge.Value = scale_choiceNumericControl.onOff_CheckBox.Checked;

            GetDataFromChoiceComboBox(color_choiceComboControl,_copySettingsData.Color);
            GetDataFromChoiceComboBox(scanMode_choiceComboControl, _copySettingsData.ScanMode);
            GetDataFromChoiceComboBox(collate_choiceComboControl, _copySettingsData.Collate);
            GetDataFromChoiceComboBox(copySides_choiceComboControl, _copySettingsData.CopySides);
            GetDataFromChoiceComboBox(pps_choiceComboControl, _copySettingsData.PagesPerSheet);

            _copySettingsData.ScanSettingsData = scanSettingsUserControl.GetData();

        }

        private void GetDataFromChoiceComboBox(ChoiceComboControl choiceComboControl, DataPair<string> dataPair )
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
            copies_choiceNumericControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            copies_choiceNumericControl.choice_numericUpDown.ValueChanged += OnControlComponentChanged;
            scale_choiceNumericControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            scale_choiceNumericControl.choice_numericUpDown.ValueChanged += OnControlComponentChanged;

            AddChoiceComboControlEventHandler(color_choiceComboControl);
            AddChoiceComboControlEventHandler(scanMode_choiceComboControl);
            AddChoiceComboControlEventHandler(collate_choiceComboControl);
            AddChoiceComboControlEventHandler(copySides_choiceComboControl);
            AddChoiceComboControlEventHandler(pps_choiceComboControl);
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            copies_choiceNumericControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            copies_choiceNumericControl.choice_numericUpDown.ValueChanged -= OnControlComponentChanged;
            scale_choiceNumericControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            scale_choiceNumericControl.choice_numericUpDown.ValueChanged -= OnControlComponentChanged;

            RemoveChoiceComboControlEventHandler(color_choiceComboControl);
            RemoveChoiceComboControlEventHandler(scanMode_choiceComboControl);
            RemoveChoiceComboControlEventHandler(collate_choiceComboControl);
            RemoveChoiceComboControlEventHandler(copySides_choiceComboControl);
            RemoveChoiceComboControlEventHandler(pps_choiceComboControl);
            
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}
