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
    public partial class JobStorageDefaultControl : UserControl, IGetSetComponentData
    {
        private JobSettingsData _storageSettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;
        public JobStorageDefaultControl()
        {
            InitializeComponent();
            _storageSettingsData = new JobSettingsData();

            enable_choiceComboControl.choice_Combo.DataSource = ListValues.TrueFalseList.ToArray();
            SetChoiceControlDataSource(pinLength_choiceComboControl, ListValues.MinLengthPinRequirement);
            pinRequired_choiceComboControl.choice_Combo.DataSource = ListValues.TrueFalseList.ToArray();
            SetChoiceControlDataSource(retainJobs_choiceComboControl, ListValues.JobRetentionDictionary);
            folderName_choiceTextControl.text_Box.Text = "Untitled";

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
            return _storageSettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _storageSettingsData = list.OfType<JobSettingsData>().FirstOrDefault();

            if (_storageSettingsData != null)
            {
                enable_choiceComboControl.onOff_CheckBox.Checked = _storageSettingsData.EnableJobStorage.Value;
                enable_choiceComboControl.choice_Combo.SelectedItem = _storageSettingsData.EnableJobStorage.Key;

                pinLength_choiceComboControl.onOff_CheckBox.Checked = _storageSettingsData.MinLengthPin.Value;
                pinLength_choiceComboControl.choice_Combo.SelectedItem = ListValues.MinLengthPinRequirement.FirstOrDefault(x => x.Value == _storageSettingsData.MinLengthPin.Key);

                pinRequired_choiceComboControl.onOff_CheckBox.Checked = _storageSettingsData.JobsPinRequired.Value;
                pinRequired_choiceComboControl.choice_Combo.SelectedItem = _storageSettingsData.JobsPinRequired.Key;

                retainJobs_choiceComboControl.onOff_CheckBox.Checked = _storageSettingsData.RetainJobs.Value;
                retainJobs_choiceComboControl.choice_Combo.SelectedItem =
                    ListValues.JobRetentionDictionary.FirstOrDefault(x => x.Value == _storageSettingsData.RetainJobs.Key);

                folderName_choiceTextControl.onOff_CheckBox.Checked = _storageSettingsData.DefaultFolderName.Value;
                folderName_choiceTextControl.text_Box.Text = _storageSettingsData.DefaultFolderName.Key;

            }

            AddEventHandlers();
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(enable_choiceComboControl, _storageSettingsData.EnableJobStorage);
            GetDataFromChoiceComboBox(pinLength_choiceComboControl, _storageSettingsData.MinLengthPin);
            GetDataFromChoiceComboBox(pinRequired_choiceComboControl, _storageSettingsData.JobsPinRequired);
            GetDataFromChoiceComboBox(retainJobs_choiceComboControl, _storageSettingsData.RetainJobs);

            _storageSettingsData.DefaultFolderName.Value = folderName_choiceTextControl.onOff_CheckBox.Checked;
            _storageSettingsData.DefaultFolderName.Key = folderName_choiceTextControl.text_Box.Text;
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
            AddChoiceComboControlEventHandler(enable_choiceComboControl);
            AddChoiceComboControlEventHandler(pinLength_choiceComboControl);
            AddChoiceComboControlEventHandler(pinRequired_choiceComboControl);
            AddChoiceComboControlEventHandler(retainJobs_choiceComboControl);
          

            folderName_choiceTextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            folderName_choiceTextControl.text_Box.TextChanged += OnControlComponentChanged;
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(enable_choiceComboControl);
            RemoveChoiceComboControlEventHandler(pinLength_choiceComboControl);
            RemoveChoiceComboControlEventHandler(pinRequired_choiceComboControl);
            RemoveChoiceComboControlEventHandler(retainJobs_choiceComboControl);
           

            folderName_choiceTextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            folderName_choiceTextControl.text_Box.TextChanged -= OnControlComponentChanged;
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }

    }
}
