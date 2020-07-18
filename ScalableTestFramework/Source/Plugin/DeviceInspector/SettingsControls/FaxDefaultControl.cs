using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsData;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;
using HP.ScalableTest.Plugin.DeviceInspector.FieldControls;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    public partial class FaxDefaultControl : UserControl, IGetSetComponentData
    {
        private FaxSettingsData _faxSettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;

        public FaxDefaultControl()
        {
            InitializeComponent();
            //lanFax_ChoiceControl.generic_GroupBox.Text = "LanFax Setup";

            _faxSettingsData = new FaxSettingsData();

            
            enable_ComboBox.choice_Combo.DataSource = ListValues.EnableScanToEmailValues;
            SetChoiceControlDataSource(faxMethod_ComboBox, ListValues.FaxMethods);
            SetChoiceControlDataSource(thirdParty_ChoiceControl, ListValues.ThirdPartyLanFax);
            SetChoiceControlDataSource(fileFormat_ChoiceControl, ListValues.FaxFileFormat);
            SetChoiceControlDataSource(resolution_choiceControl, ListValues.FaxResolutions);


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
            return _faxSettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _faxSettingsData = list.OfType<FaxSettingsData>().FirstOrDefault();

            if (_faxSettingsData != null)
            {
                enable_ComboBox.onOff_CheckBox.Checked = _faxSettingsData.EnableFax.Value;
                enable_ComboBox.choice_Combo.SelectedItem = _faxSettingsData.EnableFax.Key;

                faxMethod_ComboBox.onOff_CheckBox.Checked = _faxSettingsData.FaxMethod.Value;
                faxMethod_ComboBox.choice_Combo.SelectedItem = ListValues.FaxMethods.FirstOrDefault(x => x.Value == _faxSettingsData.FaxMethod.Key);

                thirdParty_ChoiceControl.onOff_CheckBox.Checked = _faxSettingsData.ThirdPartyProduct.Value;
                thirdParty_ChoiceControl.choice_Combo.SelectedItem = ListValues.ThirdPartyLanFax.FirstOrDefault(x => x.Value == _faxSettingsData.ThirdPartyProduct.Key);

                fileFormat_ChoiceControl.onOff_CheckBox.Checked = _faxSettingsData.FileFormat.Value;
                fileFormat_ChoiceControl.choice_Combo.SelectedItem = ListValues.ThirdPartyLanFax.FirstOrDefault(x => x.Value == _faxSettingsData.FileFormat.Key);

                if (_faxSettingsData.FaxResolution != null)
                {
                    resolution_choiceControl.onOff_CheckBox.Checked = _faxSettingsData.FaxResolution.Value;
                    resolution_choiceControl.choice_Combo.SelectedItem = ListValues.FaxResolutions.FirstOrDefault(x => x.Value == _faxSettingsData.FaxResolution.Key);
                }
                else
                {
                    _faxSettingsData.FaxResolution = new DataPair<string>() { Key = string.Empty };
                }

                //lanFax_ChoiceControl.onOff_CheckBox.Checked = _faxSettingsData.UNCFolderPath.Value;
                field1_TextBox.Text = _faxSettingsData.UNCFolderPath.Key;
                field2_TextBox.Text = _faxSettingsData.DomainName.Key;
                field3_TextBox.Text = _faxSettingsData.UserName.Key;
                field4_TextBox.Text = _faxSettingsData.Password.Key;

                field1_TextBox.Enabled = _faxSettingsData.FaxMethod.Value;
                field2_TextBox.Enabled = _faxSettingsData.FaxMethod.Value;
                field3_TextBox.Enabled = _faxSettingsData.FaxMethod.Value;
                field4_TextBox.Enabled = _faxSettingsData.FaxMethod.Value;

                location_choiceTextControl.onOff_CheckBox.Checked = _faxSettingsData.Location != null && _faxSettingsData.Location.Value;
                location_choiceTextControl.text_Box.Text = _faxSettingsData.Location == null ? string.Empty: _faxSettingsData.Location.Key;

                companyName_choiceTextControl.onOff_CheckBox.Checked = _faxSettingsData.CompanyName != null && _faxSettingsData.CompanyName.Value;
                companyName_choiceTextControl.text_Box.Text = _faxSettingsData.CompanyName == null ? string.Empty: _faxSettingsData.CompanyName.Key;

                faxNumber_choiceTextControl.onOff_CheckBox.Checked = _faxSettingsData.FaxNumber != null && _faxSettingsData.FaxNumber.Value;
                faxNumber_choiceTextControl.text_Box.Text = _faxSettingsData.FaxNumber == null ? string.Empty: _faxSettingsData.FaxNumber.Key;


                scanSettingsUserControl.SetData(_faxSettingsData.ScanSettingsData);
            }
            AddEventHandlers();
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(enable_ComboBox, _faxSettingsData.EnableFax);
            GetDataFromChoiceComboBox(faxMethod_ComboBox, _faxSettingsData.FaxMethod);
            GetDataFromChoiceComboBox(thirdParty_ChoiceControl, _faxSettingsData.ThirdPartyProduct);
            GetDataFromChoiceComboBox(fileFormat_ChoiceControl, _faxSettingsData.FileFormat);
            GetDataFromChoiceComboBox(resolution_choiceControl, _faxSettingsData.FaxResolution);
            _faxSettingsData.ScanSettingsData = scanSettingsUserControl.GetData();

            ///Still need to do grouping box
            ///
            //_faxSettingsData.UNCFolderPath.Value = lanFax_ChoiceControl.onOff_CheckBox.Checked;
            //_faxSettingsData.DomainName.Value = lanFax_ChoiceControl.onOff_CheckBox.Checked;
            //_faxSettingsData.UserName.Value = lanFax_ChoiceControl.onOff_CheckBox.Checked;
            //_faxSettingsData.Password.Value = lanFax_ChoiceControl.onOff_CheckBox.Checked;

            _faxSettingsData.UNCFolderPath.Key = field1_TextBox.Text;
            _faxSettingsData.DomainName.Key = field2_TextBox.Text;
            _faxSettingsData.UserName.Key = field3_TextBox.Text;
            _faxSettingsData.Password.Key = field4_TextBox.Text;

            _faxSettingsData.UNCFolderPath.Value = faxMethod_ComboBox.onOff_CheckBox.Checked;
            _faxSettingsData.DomainName.Value = faxMethod_ComboBox.onOff_CheckBox.Checked;
            _faxSettingsData.UserName.Value = faxMethod_ComboBox.onOff_CheckBox.Checked;
            _faxSettingsData.Password.Value = faxMethod_ComboBox.onOff_CheckBox.Checked;

            _faxSettingsData.CompanyName.Key = companyName_choiceTextControl.text_Box.Text;
            _faxSettingsData.CompanyName.Value = companyName_choiceTextControl.onOff_CheckBox.Checked;

            _faxSettingsData.FaxNumber.Key = faxNumber_choiceTextControl.text_Box.Text;
            _faxSettingsData.FaxNumber.Value = faxNumber_choiceTextControl.onOff_CheckBox.Checked;

            _faxSettingsData.Location.Key = location_choiceTextControl.text_Box.Text;
            _faxSettingsData.Location.Value = location_choiceTextControl.onOff_CheckBox.Checked;
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


        public void OnFaxMethodChosen(object sender, EventArgs e)
        {
            if (faxMethod_ComboBox.onOff_CheckBox.Checked)
            {
                field1_TextBox.Enabled = true;
                field2_TextBox.Enabled = true;
                field3_TextBox.Enabled = true;
                field4_TextBox.Enabled = true;
            }
            else
            {
                field1_TextBox.Enabled = false;
                field2_TextBox.Enabled = false;
                field3_TextBox.Enabled = false;
                field4_TextBox.Enabled = false;
            }
        }


        private void AddEventHandlers()
        {
            enable_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            enable_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            faxMethod_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            faxMethod_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            faxMethod_ComboBox.onOff_CheckBox.CheckedChanged += OnFaxMethodChosen;
            thirdParty_ChoiceControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            thirdParty_ChoiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            fileFormat_ChoiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            fileFormat_ChoiceControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            resolution_choiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            resolution_choiceControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            //lanFax_ChoiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            field1_TextBox.TextChanged += OnControlComponentChanged;
            field2_TextBox.TextChanged += OnControlComponentChanged;
            field3_TextBox.TextChanged += OnControlComponentChanged;
            field4_TextBox.TextChanged += OnControlComponentChanged;
            scanSettingsUserControl.ControlComponentChanged += OnControlComponentChanged;
            location_choiceTextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            location_choiceTextControl.text_Box.TextChanged += OnControlComponentChanged;
            faxNumber_choiceTextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            faxNumber_choiceTextControl.text_Box.TextChanged += OnControlComponentChanged;
            companyName_choiceTextControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            companyName_choiceTextControl.text_Box.TextChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            enable_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            enable_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            faxMethod_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            faxMethod_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            thirdParty_ChoiceControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            thirdParty_ChoiceControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            fileFormat_ChoiceControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            fileFormat_ChoiceControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            resolution_choiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            resolution_choiceControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            //lanFax_ChoiceControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            field1_TextBox.TextChanged -= OnControlComponentChanged;
            field2_TextBox.TextChanged -= OnControlComponentChanged;
            field3_TextBox.TextChanged -= OnControlComponentChanged;
            field4_TextBox.TextChanged -= OnControlComponentChanged;
            scanSettingsUserControl.ControlComponentChanged -= OnControlComponentChanged;
            location_choiceTextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            location_choiceTextControl.text_Box.TextChanged -= OnControlComponentChanged;
            faxNumber_choiceTextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            faxNumber_choiceTextControl.text_Box.TextChanged -= OnControlComponentChanged;
            companyName_choiceTextControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            companyName_choiceTextControl.text_Box.TextChanged -= OnControlComponentChanged;
        }

    }


}
