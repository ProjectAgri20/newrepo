using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsData;
using HP.ScalableTest.Plugin.DeviceInspector.FieldControls;
using HP.ScalableTest.Plugin.DeviceInspector.Classes;

//using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls;

namespace HP.ScalableTest.Plugin.DeviceInspector.SettingsControls
{
    [ToolboxItem(false)]
    public partial class EmailDefaultControl : UserControl, IGetSetComponentData
    {
        private EmailSettingsData _emailSettingsData;
        //private ListValues values
        public bool Modified = false;

        /// <summary>
        /// Occurs when configuration data in this control has changed
        /// Used to Track whether the control has unsaved changes.
        /// </summary>
        public EventHandler ControlComponentChanged;

        public EmailDefaultControl()
        {
            InitializeComponent();

            this.outgoingSMTPServer_ChoiceBox.SetGroupHeader("Outgoing SMTP Email Server");
            _emailSettingsData = new EmailSettingsData();

            //Initiate Data
            SetChoiceControlDataSource(originalSize_ComboBox, ListValues.OriginalSize);
            SetChoiceControlDataSource(imagePreview_ComboBox, ListValues.ImagePreview);
            SetChoiceControlDataSource(resolution_ComboBox, ListValues.Resolution);
            SetChoiceControlDataSource(originalSides_ComboBox, ListValues.OriginalSides);
            SetChoiceControlDataSource(fileType_ComboBox, ListValues.FileType);
            outgoingSMTPServer_ChoiceBox.field4_TextBox.Text = @"false";
            //originalSize_ComboBox.choice_Combo.DataSource = ListValues.OriginalSize.Keys.ToList();

            //imagePreview_ComboBox.choice_Combo.DataSource = ListValues.ImagePreview.Keys.ToList();// new BindingSource(_imagePreview, null);

           // resolution_ComboBox.choice_Combo.DataSource = ListValues.Resolution;

           // originalSides_ComboBox.choice_Combo.DataSource = ListValues.OriginalSides.Keys.ToList();// new BindingSource(_originalSides, null);
            
            //This one is backwards on purpose
          //  fileType_ComboBox.choice_Combo.DataSource = ListValues.FileType.Keys.ToList(); // new BindingSource(_fileType, null);

            defaultFrom_ComboBox.choice_Combo.DataSource = ListValues.DefaultFromList;
            defaultTo_ComboBox.choice_Combo.DataSource = ListValues.DefaultToList;
            from_TextBox.text_Box.Text = "jawa@etl.boi.rd.hpicorp.net";
            enable_ComboBox.choice_Combo.DataSource = ListValues.EnableScanToEmailValues;


            AddEventHandlers();
        }


        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        public IComponentData GetData()
        {
            return _emailSettingsData;
        }

        /// <summary>
        /// If the field checkbox is set, write the value. Else we leave it alone.
        /// </summary>
        public void SetData()
        {
            _emailSettingsData.EnableScanToEmail.Key = enable_ComboBox.choice_Combo.SelectedValue.ToString();
            _emailSettingsData.EnableScanToEmail.Value = enable_ComboBox.onOff_CheckBox.Checked;

            _emailSettingsData.SMTPServer.Key = $"{outgoingSMTPServer_ChoiceBox.field1_TextBox.Text} {outgoingSMTPServer_ChoiceBox.field2_TextBox.Text} {outgoingSMTPServer_ChoiceBox.field3_TextBox.Text} {outgoingSMTPServer_ChoiceBox.field4_TextBox.Text}"; 
            _emailSettingsData.SMTPServer.Value = outgoingSMTPServer_ChoiceBox.onOff_CheckBox.Checked;

            _emailSettingsData.FromUser.Key = from_TextBox.text_Box.Text;
            _emailSettingsData.FromUser.Value = from_TextBox.onOff_CheckBox.Checked;

            _emailSettingsData.DefaultFrom = GetComboBoxData(defaultFrom_ComboBox);
            _emailSettingsData.To = GetComboBoxData(defaultTo_ComboBox);
            _emailSettingsData.OriginalSize = GetComboBoxData(originalSize_ComboBox);
            _emailSettingsData.OriginalSides = GetComboBoxData(originalSides_ComboBox);
            _emailSettingsData.ImagePreview = GetComboBoxData(imagePreview_ComboBox);
            _emailSettingsData.FileType = GetComboBoxData(fileType_ComboBox);
            _emailSettingsData.Resolution = GetComboBoxData(resolution_ComboBox);


        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();
            _emailSettingsData = list.OfType<EmailSettingsData>().FirstOrDefault();

            enable_ComboBox.choice_Combo.SelectedIndex = ListValues.EnableScanToEmailValues.FindIndex(x => x == _emailSettingsData.EnableScanToEmail.Key);
            enable_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.EnableScanToEmail.Value;

            var smtp = _emailSettingsData.SMTPServer.Key.Split(' ').ToList();
            if (smtp.Count == 3)
               smtp.Add("false");

            if(!string.IsNullOrWhiteSpace(_emailSettingsData.SMTPServer.Key))
            {
                outgoingSMTPServer_ChoiceBox.field1_TextBox.Text = smtp[0];
                outgoingSMTPServer_ChoiceBox.field2_TextBox.Text = smtp[1];
                outgoingSMTPServer_ChoiceBox.field3_TextBox.Text = smtp[2];
                outgoingSMTPServer_ChoiceBox.field4_TextBox.Text = smtp[3];
            }
            outgoingSMTPServer_ChoiceBox.onOff_CheckBox.Checked = _emailSettingsData.SMTPServer.Value;

            from_TextBox.text_Box.Text = _emailSettingsData.FromUser.Key;
            from_TextBox.onOff_CheckBox.Checked = _emailSettingsData.FromUser.Value;

            defaultFrom_ComboBox.choice_Combo.SelectedIndex = ListValues.DefaultFromList.FindIndex(x => x == _emailSettingsData.DefaultFrom.Key);
            defaultFrom_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.DefaultFrom.Value;

            defaultTo_ComboBox.choice_Combo.SelectedIndex = ListValues.DefaultToList.FindIndex(x => x == _emailSettingsData.To.Key);
            defaultTo_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.To.Value;

            originalSize_ComboBox.choice_Combo.SelectedItem = ListValues.OriginalSize.FirstOrDefault(x => x.Value == _emailSettingsData.OriginalSize.Key);// _originalSize[_emailSettingsData.OriginalSize.Key];
            originalSize_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.OriginalSize.Value;

            originalSides_ComboBox.choice_Combo.SelectedItem = ListValues.OriginalSides.FirstOrDefault(x => x.Value == _emailSettingsData.OriginalSides.Key);//  _originalSides[_emailSettingsData.OriginalSides.Key];
            originalSides_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.OriginalSides.Value;

            imagePreview_ComboBox.choice_Combo.SelectedItem = ListValues.ImagePreview.FirstOrDefault(x => x.Value == _emailSettingsData.ImagePreview.Key);// _imagePreview[_emailSettingsData.ImagePreview.Key];
            imagePreview_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.ImagePreview.Value;

            fileType_ComboBox.choice_Combo.SelectedItem = ListValues.FileType.FirstOrDefault(x => x.Value == _emailSettingsData.FileType.Key); // _fileType[_emailSettingsData.FileType.Key];
            fileType_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.FileType.Value;

            resolution_ComboBox.choice_Combo.SelectedItem = ListValues.Resolution.FirstOrDefault(x => x.Value == _emailSettingsData.Resolution.Key);
            resolution_ComboBox.onOff_CheckBox.Checked = _emailSettingsData.Resolution.Value;

            AddEventHandlers();
        }

        private void RemoveEventHandlers()
        {
            enable_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            enable_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            outgoingSMTPServer_ChoiceBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field1_TextBox.TextChanged -= OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field2_TextBox.TextChanged -= OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field3_TextBox.TextChanged -= OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field4_TextBox.TextChanged -= OnControlComponentChanged;

            from_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            from_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            defaultFrom_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            defaultFrom_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            defaultTo_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            defaultTo_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            originalSize_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            originalSize_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            originalSides_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            originalSides_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            imagePreview_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            imagePreview_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            fileType_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            fileType_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            resolution_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            resolution_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
        }

        private void AddEventHandlers()
        {
            enable_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            enable_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            outgoingSMTPServer_ChoiceBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field1_TextBox.TextChanged += OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field2_TextBox.TextChanged += OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field3_TextBox.TextChanged += OnControlComponentChanged;
            outgoingSMTPServer_ChoiceBox.field4_TextBox.TextChanged += OnControlComponentChanged;

            from_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            from_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            defaultFrom_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            defaultTo_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            defaultTo_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            defaultTo_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            originalSize_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            originalSize_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            originalSides_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            originalSides_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            imagePreview_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            imagePreview_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            fileType_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            fileType_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            resolution_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            resolution_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary)
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        private DataPair<string> GetComboBoxData(ChoiceComboControl choiceComboControl)
        {
            return new DataPair<string>
            {
                Key = choiceComboControl.choice_Combo.SelectedValue.ToString(),
                Value = choiceComboControl.onOff_CheckBox.Checked
            };
        }



    }
}
