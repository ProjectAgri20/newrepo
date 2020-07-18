using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    public partial class ScanSettingsUserControl : UserControl
    {
        private readonly ScanSettings _scanSettings;
        public EventHandler ControlComponentChanged;
        public bool Modified;

        public ScanSettingsUserControl()
        {
            InitializeComponent();

            
            _scanSettings = new ScanSettings();

            SetChoiceControlDataSource(originalSize_ComboBox, ListValues.OriginalSize);
            SetChoiceControlDataSource(originalSides_choiceComboControl, ListValues.OriginalSides);
            SetChoiceControlDataSource(optimize_choiceComboControl, ListValues.OptimizeModes);
            orientation_choiceComboControl.choice_Combo.DataSource = ListValues.ContentOrientation;
            SetChoiceControlDataSource(sharpness_choiceComboControl, ListValues.SharpnessModes);
            SetChoiceControlDataSource(cleanup_choiceComboControl, ListValues.CleanupModes);
            SetChoiceControlDataSource(darkness_choiceComboControl, ListValues.DarknessModes);
            SetChoiceControlDataSource(contrast_choiceComboControl, ListValues.ContrastModes);
            SetChoiceControlDataSource(preview_choiceComboControl, ListValues.ImagePreview);

            AddEventHandlers();
        }

        public void SetData(ScanSettings scanSettingsData)
        {
            RemoveEventHandlers();

            SetComboBoxData(scanSettingsData.OriginalSize, originalSize_ComboBox, ListValues.OriginalSize);
            SetComboBoxData(scanSettingsData.OriginalSides, originalSides_choiceComboControl, ListValues.OriginalSides);
            SetComboBoxData(scanSettingsData.Optimize, optimize_choiceComboControl, ListValues.OptimizeModes);
            SetComboBoxData(scanSettingsData.ContentOrientation, orientation_choiceComboControl, null);
            SetComboBoxData(scanSettingsData.Sharpness, sharpness_choiceComboControl, ListValues.SharpnessModes);
            SetComboBoxData(scanSettingsData.Cleanup, cleanup_choiceComboControl, ListValues.CleanupModes);
            SetComboBoxData(scanSettingsData.Darkness, darkness_choiceComboControl, ListValues.DarknessModes);
            SetComboBoxData(scanSettingsData.Contrast, contrast_choiceComboControl, ListValues.ContrastModes);
            SetComboBoxData(scanSettingsData.ImagePreview, preview_choiceComboControl, ListValues.ImagePreview);

            AddEventHandlers();
        }

        public ScanSettings GetData()
        {
            _scanSettings.OriginalSize = GetComboBoxData(originalSize_ComboBox);
            _scanSettings.OriginalSides = GetComboBoxData(originalSides_choiceComboControl);
            _scanSettings.Optimize = GetComboBoxData(optimize_choiceComboControl);
            _scanSettings.ContentOrientation = GetComboBoxData(orientation_choiceComboControl);
            _scanSettings.Sharpness = GetComboBoxData(sharpness_choiceComboControl);
            _scanSettings.Cleanup = GetComboBoxData(cleanup_choiceComboControl);
            _scanSettings.Darkness = GetComboBoxData(darkness_choiceComboControl);
            _scanSettings.Contrast = GetComboBoxData(contrast_choiceComboControl);
            _scanSettings.ImagePreview = GetComboBoxData(preview_choiceComboControl);

            return _scanSettings;
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

        private void ScanSettingsUserControl_Load(object sender, System.EventArgs e)
        {
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
            AddChoiceComboControlEventHandler(originalSize_ComboBox);
            AddChoiceComboControlEventHandler(originalSides_choiceComboControl);
            AddChoiceComboControlEventHandler(optimize_choiceComboControl);
            AddChoiceComboControlEventHandler(orientation_choiceComboControl);
            AddChoiceComboControlEventHandler(sharpness_choiceComboControl);
            AddChoiceComboControlEventHandler(cleanup_choiceComboControl);
            AddChoiceComboControlEventHandler(darkness_choiceComboControl);
            AddChoiceComboControlEventHandler(contrast_choiceComboControl);
            AddChoiceComboControlEventHandler(preview_choiceComboControl);
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(originalSize_ComboBox);
            RemoveChoiceComboControlEventHandler(originalSides_choiceComboControl);
            RemoveChoiceComboControlEventHandler(optimize_choiceComboControl);
            RemoveChoiceComboControlEventHandler(orientation_choiceComboControl);
            RemoveChoiceComboControlEventHandler(sharpness_choiceComboControl);
            RemoveChoiceComboControlEventHandler(cleanup_choiceComboControl);
            RemoveChoiceComboControlEventHandler(darkness_choiceComboControl);
            RemoveChoiceComboControlEventHandler(contrast_choiceComboControl);
            RemoveChoiceComboControlEventHandler(preview_choiceComboControl);
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }
    }
}