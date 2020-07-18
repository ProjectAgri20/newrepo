using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings;
using HP.ScalableTest.Plugin.DeviceConfiguration;
using HP.ScalableTest.Utility;
using System.Reflection;


namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class ManageTraysControl : UserControl, IGetSetComponentData
    {
        public ManageTraysSettingData _manageTraysSettingData;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public TraySettings traySettings;
        public bool imageRotation;
        public ManageTraysControl()
        {
            InitializeComponent();
            _manageTraysSettingData = new ManageTraysSettingData();

            SetChoiceControlDataSource(useRequestedTray_choiceComboControl, ListValues.UseRequesetedTray);
            SetChoiceControlDataSource(manualFeedPrompt_choiceComboControl, ListValues.ManualFeedPrompt);
            SetChoiceControlDataSource(sizeTypePrompt_choiceComboControl, ListValues.SizeTypePrompt);
            SetChoiceControlDataSource(useAnotherTray_choiceComboControl, ListValues.UseAnotherTray);
            SetChoiceControlDataSource(alternativeLetterheadMode_choiceComboControl, ListValues.AlternativeLetterheadMode);
            SetChoiceControlDataSource(duplexBlankPages_choiceComboControl, ListValues.DuplexBlankPages);
            SetChoiceControlDataSource(imageRotation_choiceComboControl, ListValues.ImageRotation);
            SetChoiceControlDataSource(overrideA4Letter_choiceComboControl, ListValues.OverrideA4Letter);
            useRequestedTray_choiceComboControl.Enabled = true;
            AddEventHandlers();

        }

        public IComponentData GetData()
        {
            return _manageTraysSettingData;
        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary)
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(useRequestedTray_choiceComboControl, _manageTraysSettingData.UseRequestedTray);
            GetDataFromChoiceComboBox(manualFeedPrompt_choiceComboControl, _manageTraysSettingData.ManualFeedPrompt);
            GetDataFromChoiceComboBox(sizeTypePrompt_choiceComboControl, _manageTraysSettingData.SizeTypePrompt);
            GetDataFromChoiceComboBox(useAnotherTray_choiceComboControl, _manageTraysSettingData.UseAnotherTray);
            GetDataFromChoiceComboBox(alternativeLetterheadMode_choiceComboControl, _manageTraysSettingData.AlternativeLetterHeadMode);
            GetDataFromChoiceComboBox(duplexBlankPages_choiceComboControl, _manageTraysSettingData.DuplexBlankPages);
            GetDataFromChoiceComboBox(imageRotation_choiceComboControl, _manageTraysSettingData.ImageRotation);
            GetDataFromChoiceComboBox(overrideA4Letter_choiceComboControl, _manageTraysSettingData.OverrideA4Letter);
            _manageTraysSettingData._traySettings = SetTraySettings();

        }

        private void GetDataFromChoiceComboBox(ChoiceComboControl choiceComboControl, DataPair<string> dataPair)
        {
            dataPair.Key = choiceComboControl.choice_Combo.SelectedValue.ToString();
            dataPair.Value = choiceComboControl.onOff_CheckBox.Checked;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();
            _manageTraysSettingData = list.OfType<ManageTraysSettingData>().FirstOrDefault();

            useRequestedTray_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.UseRequestedTray.Value;
            useRequestedTray_choiceComboControl.choice_Combo.SelectedItem = ListValues.UseRequesetedTray.FirstOrDefault(x => x.Value == _manageTraysSettingData.UseRequestedTray.Key);

            manualFeedPrompt_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.ManualFeedPrompt.Value;
            manualFeedPrompt_choiceComboControl.choice_Combo.SelectedItem = ListValues.ManualFeedPrompt.FirstOrDefault(x => x.Value == _manageTraysSettingData.ManualFeedPrompt.Key);

            sizeTypePrompt_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.SizeTypePrompt.Value;
            sizeTypePrompt_choiceComboControl.choice_Combo.SelectedItem = ListValues.SizeTypePrompt.FirstOrDefault(x => x.Value == _manageTraysSettingData.SizeTypePrompt.Key);

            useAnotherTray_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.UseAnotherTray.Value;
            useAnotherTray_choiceComboControl.choice_Combo.SelectedItem = ListValues.UseAnotherTray.FirstOrDefault(x => x.Value == _manageTraysSettingData.UseAnotherTray.Key);

            alternativeLetterheadMode_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.AlternativeLetterHeadMode.Value;
            alternativeLetterheadMode_choiceComboControl.choice_Combo.SelectedItem = ListValues.AlternativeLetterheadMode.FirstOrDefault(x => x.Value == _manageTraysSettingData.AlternativeLetterHeadMode.Key);

            duplexBlankPages_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.DuplexBlankPages.Value;
            duplexBlankPages_choiceComboControl.choice_Combo.SelectedItem = ListValues.DuplexBlankPages.FirstOrDefault(x => x.Value == _manageTraysSettingData.DuplexBlankPages.Key);

            imageRotation = _manageTraysSettingData.ImageRotation.Value? imageRotation_choiceComboControl.onOff_CheckBox.Checked = true : imageRotation_choiceComboControl.onOff_CheckBox.Checked = false;
            if(imageRotation == true)
                imageRotation_choiceComboControl.choice_Combo.SelectedItem = ListValues.ImageRotation.FirstOrDefault(x => x.Value == _manageTraysSettingData.ImageRotation.Key);

            overrideA4Letter_choiceComboControl.onOff_CheckBox.Checked = _manageTraysSettingData.OverrideA4Letter.Value;
            overrideA4Letter_choiceComboControl.choice_Combo.SelectedItem = _manageTraysSettingData.OverrideA4Letter.Key;

            AddEventHandlers();
        }
        public TraySettings SetTraySettings()
        {
            traySettings = new TraySettings();

            
            traySettings.IsUseRequesetedTraySet = useRequestedTray_choiceComboControl.onOff_CheckBox.Checked;
            if(traySettings.IsUseRequesetedTraySet)
            {
                traySettings.UseRequesetedTray = useRequestedTray_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("WhenAvailable") ? false : true;
            }

            traySettings.IsManualFeedPromptSet = manualFeedPrompt_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsManualFeedPromptSet)
            {
                traySettings.ManualFeedPrompt = manualFeedPrompt_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("PromptOnMismatch") ? false : true;
            }

            traySettings.IsSizeTypePromptSet = sizeTypePrompt_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsSizeTypePromptSet)
            {
                traySettings.SizeTypePrompt = sizeTypePrompt_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("DoNotDisplay") ? false : true;
            }

            traySettings.IsUseAnotherTraySet = useAnotherTray_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsUseAnotherTraySet)
            {
                traySettings.UseAnotherTray = useAnotherTray_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("DoNotAllow") ? false : true;
            }

            traySettings.IsAlternativeLetterheadModeSet = alternativeLetterheadMode_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsAlternativeLetterheadModeSet)
            {
                traySettings.AlternativeLetterheadMode = alternativeLetterheadMode_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("On") ? false : true;
            }

            traySettings.IsDuplexBlankPagesSet = duplexBlankPages_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsDuplexBlankPagesSet)
            {
                traySettings.DuplexBlankPages = duplexBlankPages_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("Always") ? false : true;
            }

            if (imageRotation_choiceComboControl.onOff_CheckBox.Checked)
            {
                traySettings.IsImageRotationSet = true;
                if (imageRotation_choiceComboControl.choice_Combo.SelectedValue.ToString() == "LeftToRight")
                {
                    traySettings.ImageRotation = ImageRoationType.LeftToRight;
                }
                else if (imageRotation_choiceComboControl.choice_Combo.SelectedValue.ToString() == "RightToLeft")
                {
                    traySettings.ImageRotation = ImageRoationType.RightToLeft;
                }
                else
                {
                    traySettings.ImageRotation = ImageRoationType.Alternate;
                }
            }
            else
            {
                traySettings.IsImageRotationSet = false;
            }

            traySettings.IsOverrideA4LetterSet = overrideA4Letter_choiceComboControl.onOff_CheckBox.Checked;
            if (traySettings.IsOverrideA4LetterSet)
            {
                traySettings.OverrideA4Letter = overrideA4Letter_choiceComboControl.choice_Combo.SelectedValue.ToString().Equals("Yes") ? false : true;
            }
                                                        
            return traySettings;
        }
        private void AddEventHandlers()
        {
            AddChoiceComboControlEventHandler(useRequestedTray_choiceComboControl);
            AddChoiceComboControlEventHandler(manualFeedPrompt_choiceComboControl);
            AddChoiceComboControlEventHandler(sizeTypePrompt_choiceComboControl);
            AddChoiceComboControlEventHandler(useAnotherTray_choiceComboControl);
            AddChoiceComboControlEventHandler(alternativeLetterheadMode_choiceComboControl);
            AddChoiceComboControlEventHandler(duplexBlankPages_choiceComboControl);
            AddChoiceComboControlEventHandler(imageRotation_choiceComboControl);
            AddChoiceComboControlEventHandler(overrideA4Letter_choiceComboControl);
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(useRequestedTray_choiceComboControl);
            RemoveChoiceComboControlEventHandler(manualFeedPrompt_choiceComboControl);
            RemoveChoiceComboControlEventHandler(sizeTypePrompt_choiceComboControl);
            RemoveChoiceComboControlEventHandler(useAnotherTray_choiceComboControl);
            RemoveChoiceComboControlEventHandler(alternativeLetterheadMode_choiceComboControl);
            RemoveChoiceComboControlEventHandler(duplexBlankPages_choiceComboControl);
            RemoveChoiceComboControlEventHandler(imageRotation_choiceComboControl);
            RemoveChoiceComboControlEventHandler(overrideA4Letter_choiceComboControl);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}
