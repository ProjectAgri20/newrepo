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
    public partial class PrintDefaultControl : UserControl, IGetSetComponentData
    {
        private PrintSettingsData _printSettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public PrintDefaultControl()
        {
            InitializeComponent();
            _printSettingsData = new PrintSettingsData();

            enable_choiceComboControl.choice_Combo.DataSource = ListValues.EnableScanToEmailValues;
            SetChoiceControlDataSource(paperSize_choiceComboControl, ListValues.OriginalSize);
            SetChoiceControlDataSource(paperType_choiceComboControl, ListValues.PaperTypes);
            SetChoiceControlDataSource(outputBin_choiceComboControl, ListValues.OutputBin);
            SetChoiceControlDataSource(outputSides_choiceComboControl,ListValues.OutputSides);
            SetChoiceControlDataSource(paperTray_choiceComboControl, ListValues.PaperTrayModes);
            SetChoiceControlDataSource(resolution_ComboBox, ListValues.PrintResolution);

            copies_choiceNumericControl.choice_numericUpDown.Minimum = 1;
            copies_choiceNumericControl.choice_numericUpDown.Maximum = 32000;
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
            return _printSettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _printSettingsData = list.OfType<PrintSettingsData>().FirstOrDefault();

            copies_choiceNumericControl.onOff_CheckBox.Checked = _printSettingsData.Copies.Value;
            copies_choiceNumericControl.choice_numericUpDown.Value = Convert.ToDecimal(_printSettingsData.Copies.Key);
            

            paperSize_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.OriginalSize.Value;
            paperSize_choiceComboControl.choice_Combo.SelectedItem = ListValues.OriginalSize.FirstOrDefault(x => x.Value == _printSettingsData.OriginalSize.Key);

            paperType_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.PaperType.Value;
            paperType_choiceComboControl.choice_Combo.SelectedItem = ListValues.PaperTypes.FirstOrDefault(x => x.Value == _printSettingsData.PaperType.Key);

            paperTray_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.PaperTray.Value;
            paperTray_choiceComboControl.choice_Combo.SelectedItem = ListValues.PaperTrayModes.FirstOrDefault(x => x.Value == _printSettingsData.PaperTray.Key);

            outputBin_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.OutputBin.Value;
            outputBin_choiceComboControl.choice_Combo.SelectedItem = ListValues.OutputBin.FirstOrDefault(x => x.Value == _printSettingsData.OutputBin.Key);

            outputSides_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.OutputSides.Value;
            outputSides_choiceComboControl.choice_Combo.SelectedItem =ListValues.OutputSides.FirstOrDefault(x => x.Value == _printSettingsData.OutputSides.Key);

            resolution_ComboBox.onOff_CheckBox.Checked = _printSettingsData.Resolution.Value;
            resolution_ComboBox.choice_Combo.SelectedItem = ListValues.PrintResolution.FirstOrDefault(x => x.Value == _printSettingsData.Resolution.Key);

            enable_choiceComboControl.onOff_CheckBox.Checked = _printSettingsData.PrintFromUsb.Value;
            enable_choiceComboControl.choice_Combo.SelectedItem = _printSettingsData.PrintFromUsb.Key;

            AddEventHandlers();

        }

        public void SetData()
        {
            _printSettingsData.Copies.Key = copies_choiceNumericControl.choice_numericUpDown.Value.ToString(CultureInfo.CurrentCulture);
            _printSettingsData.Copies.Value = copies_choiceNumericControl.onOff_CheckBox.Checked;

            
            GetDataFromChoiceComboBox(enable_choiceComboControl, _printSettingsData.PrintFromUsb);
            GetDataFromChoiceComboBox(paperSize_choiceComboControl,_printSettingsData.OriginalSize);
            GetDataFromChoiceComboBox(paperType_choiceComboControl, _printSettingsData.PaperType);
            GetDataFromChoiceComboBox(paperTray_choiceComboControl, _printSettingsData.PaperTray);
            GetDataFromChoiceComboBox(outputBin_choiceComboControl, _printSettingsData.OutputBin);
            GetDataFromChoiceComboBox(outputSides_choiceComboControl, _printSettingsData.OutputSides);
            GetDataFromChoiceComboBox(resolution_ComboBox, _printSettingsData.Resolution);
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
           
            AddChoiceComboControlEventHandler(enable_choiceComboControl);
            AddChoiceComboControlEventHandler(paperSize_choiceComboControl);
            AddChoiceComboControlEventHandler(paperType_choiceComboControl);
            AddChoiceComboControlEventHandler(paperTray_choiceComboControl);
            AddChoiceComboControlEventHandler(outputBin_choiceComboControl);
            AddChoiceComboControlEventHandler(outputSides_choiceComboControl);
            AddChoiceComboControlEventHandler(resolution_ComboBox);
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
           
            RemoveChoiceComboControlEventHandler(enable_choiceComboControl);
            RemoveChoiceComboControlEventHandler(paperSize_choiceComboControl);
            RemoveChoiceComboControlEventHandler(paperType_choiceComboControl);
            RemoveChoiceComboControlEventHandler(paperTray_choiceComboControl);
            RemoveChoiceComboControlEventHandler(outputBin_choiceComboControl);
            RemoveChoiceComboControlEventHandler(outputSides_choiceComboControl);
            RemoveChoiceComboControlEventHandler(resolution_ComboBox);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}
