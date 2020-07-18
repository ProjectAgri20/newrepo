using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class QuickSetControl : UserControl, IGetSetComponentData
    {
        private QuickSetSettingsData _quickSetList;
        List<QuickSetTableData> _viewList;
        List<QuickSetParameterData> _parameterList;
        public bool Modified;
        private string _currentType;
        private AQuickSet _SelectedQuickSet;
        private QuickSetTableData _selectedRow;

        public EventHandler ControlComponentChanged;
        
        //TODO-DEFAULT VALUES
        public QuickSetControl()
        {
            InitializeComponent();

            _quickSetList = new QuickSetSettingsData();
            _viewList = new List<QuickSetTableData>();
            _parameterList = new List<QuickSetParameterData>();
            _currentType = "Email";
            _parameterList.Add(new QuickSetParameterData("To", "Default From:", "Default From: ; User's Address"));
            _parameterList.Add(new QuickSetParameterData("Default From", "User's Address", "Blank ; User's Address"));
            _parameterList.Add(new QuickSetParameterData("From User", "jawa@etl.boi.rd.hpicorp.net", "String"));

            //Initialize Data
            name_TextBox.Text = string.Empty;
            SetComboBoxDataSource(originalSize_ComboBox, ListValues.OriginalSize);
            SetComboBoxDataSource(imagePreview_ComboBox, ListValues.ImagePreview);
            SetComboBoxDataSource(resolution_ComboBox, ListValues.Resolution);
            SetComboBoxDataSource(originalSides_ComboBox, ListValues.OriginalSides);
            SetComboBoxDataSource(fileType_ComboBox, ListValues.FileType);
            contentOrientation_ComboBox.DataSource = ListValues.ContentOrientation;
            quickSetType_ComboBox.DataSource = ListValues.QuickSetOptions;

            quickSetTableDataBindingSource.ListChanged += BindQuickSetGrid;
            quickSetParameterDataBindingSource.ListChanged += BindParameterGrid;
            quickSet_GridView.CellClick += PopulateQuickSetOptions;
            quickSetType_ComboBox.SelectedValueChanged += ChangeQuickSetType;


            BindParameterGrid(null, null);
            AddEventHandlers();

        }

        //public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void BindQuickSetGrid(object sender, EventArgs e)
        {
            quickSetTableDataBindingSource.DataSource = _viewList;
            quickSet_GridView.Update();
            quickSet_GridView.Refresh();
        }

        private void BindParameterGrid(object sender, EventArgs e)
        {
            quickSetParameterDataBindingSource.DataSource = _parameterList;
            parameter_GridView.Update();
            parameter_GridView.Refresh();
        }

        private void PopulateQuickSetOptions(object sender, EventArgs e)
        {
            var row = quickSet_GridView.CurrentRow;
            if (row == null)
            {
                return;
            }
            var name = row.Cells[0].Value.ToString();
            var type = row.Cells[1].Value.ToString();
            _selectedRow = _viewList.FirstOrDefault(x => x.QuickSetName == name && x.QuickSetType == type);
            _SelectedQuickSet = _quickSetList.QuickSetData.Key.Where(x => x.QName == name && x.QType == type).Select( x => x).FirstOrDefault();
            _parameterList.Clear();
            _currentType = _SelectedQuickSet.QType;
            //populate selected options
            switch (_currentType)
            {
                case "Email":
                    EmailQuickSetData emailSelected = _SelectedQuickSet as EmailQuickSetData;
                    SetFieldInfo();

                    _parameterList.Add(new QuickSetParameterData("To", emailSelected.To, "Default From: ; User's Address"));
                    _parameterList.Add(new QuickSetParameterData("Default From", emailSelected.DefaultFrom, "Blank ; User's Address"));
                    _parameterList.Add(new QuickSetParameterData("From User", emailSelected.FromUser, "String"));

                    break;
                case "Copy":
                    _currentType = "Copy";
                    CopyQuickSetData copySelected = _SelectedQuickSet as CopyQuickSetData;
                    SetFieldInfo();

                    _parameterList.Add(new QuickSetParameterData("Copies", copySelected.Copies, "Number"));

                    break;
                case "Folder":
                    _currentType = "Folder";
                    STNFQuickSetData stnfSelected = _SelectedQuickSet as STNFQuickSetData;
                    SetFieldInfo();
                    foreach (var path in stnfSelected.FolderPaths)
                    {
                        _parameterList.Add(new QuickSetParameterData("Network Path", path, $@"UNC Path-- \\rdlfs.hpicorp\..."));
                    }
                    AddFolder_Button.Visible = true;
                    deleteFolder_Button.Visible = true;

                    break;
                case "SharePoint":
                    _currentType = "SharePoint";
                    STSharePointQuickSetData sharePointSelected = _SelectedQuickSet as STSharePointQuickSetData;
                    SetFieldInfo();
                    _parameterList.Add(new QuickSetParameterData("Folder Path", sharePointSelected.FolderPath, $@"UNC Path-- \\rdlfs.hpicorp\..."));

                    break;
                case "USB":
                    _currentType = "USB";
                    STUSBQuickSetData usbSelected = _SelectedQuickSet as STUSBQuickSetData;
                    SetFieldInfo();
                    break;
                case "Fax":
                    _currentType = "Fax";
                    ScanFaxQuickSetData faxSelected = _SelectedQuickSet as ScanFaxQuickSetData;
                    SetFieldInfo();
                    _parameterList.Add(new QuickSetParameterData("Phone Number", faxSelected.Number, $@"Ex: ###-###-####"));
                    break;
                case "FTP":
                    _currentType = "FTP";
                    FTPQuickSetData ftpSelected = _SelectedQuickSet as FTPQuickSetData;
                    SetFieldInfo();
                    _parameterList.Add(new QuickSetParameterData("FTP Server", ftpSelected.FTPServer, $@"FTP Server Path Ex: ftp.test.com"));
                    _parameterList.Add(new QuickSetParameterData("Port", ftpSelected.PortNumber, $@"Port number"));
                    _parameterList.Add(new QuickSetParameterData("Directory Path", ftpSelected.DirectoryPath, $@"//folder/folder"));
                    _parameterList.Add(new QuickSetParameterData("User name", ftpSelected.UserName, $@"FTP username"));
                    _parameterList.Add(new QuickSetParameterData("FTP Protocol", ftpSelected.FTPProtocol, $@"FTP/Secure FTP"));

                    break;
                default:
                    throw new Exception("Invalid Quickset");
            }
            if (_currentType != "Folder")
            {
                parameter_GridView.AllowUserToAddRows = false;
                AddFolder_Button.Visible = false;
                deleteFolder_Button.Visible = false;
            }
            quickSetParameterDataBindingSource.ResetBindings(false);
        }


        public void SetFieldInfo()
        {
            name_TextBox.Text = _SelectedQuickSet.Name;
            originalSize_ComboBox.SelectedItem = ListValues.OriginalSize.FirstOrDefault(x => x.Value == _SelectedQuickSet.ScanSetData.OriginalSize.Key);
            originalSides_ComboBox.SelectedItem = ListValues.OriginalSides.FirstOrDefault(x => x.Value == _SelectedQuickSet.ScanSetData.OriginalSides.Key);
            contentOrientation_ComboBox.SelectedItem = _SelectedQuickSet.ScanSetData.ContentOrientation.Key;
            imagePreview_ComboBox.SelectedItem = ListValues.ImagePreview.FirstOrDefault(x => x.Value == _SelectedQuickSet.ScanSetData.ImagePreview.Key);
            fileType_ComboBox.SelectedItem = ListValues.FileType.FirstOrDefault(x => x.Value == _SelectedQuickSet.FileSetData.FileType.Key);
            resolution_ComboBox.SelectedItem = ListValues.Resolution.FirstOrDefault(x => x.Value == _SelectedQuickSet.FileSetData.Resolution.Key);

            quickSetType_ComboBox.SelectedValueChanged -= ChangeQuickSetType;
            quickSetType_ComboBox.SelectedIndex = ListValues.QuickSetOptions.FindIndex(x => x == _currentType);
            quickSetType_ComboBox.SelectedValueChanged += ChangeQuickSetType;
        }

        //change the param string based on type
        private void ChangeQuickSetType(object sender, EventArgs e)
        {
            _parameterList.Clear();
            switch (quickSetType_ComboBox.SelectedValue.ToString())
            {
                case "Email":
                    _currentType = "Email";
                    _parameterList.Add(new QuickSetParameterData("To", "Default From:", "Default From: ; User's Address"));
                    _parameterList.Add(new QuickSetParameterData("Default From", "User's Address","Blank ; User's Address"));
                    _parameterList.Add(new QuickSetParameterData("From User", "jawa@etl.boi.rd.hpicorp.net", "String"));

                    break;
                case "Copy":
                    _currentType = "Copy";
                    _parameterList.Add(new QuickSetParameterData("Copies", "1", "Number"));
                    break;
                case "Folder":
                    _currentType = "Folder";
                    _parameterList.Add(new QuickSetParameterData("Network Path", $@"\\rdlfs.rdl.boi.rd.hpicorp.net\Public", $@"UNC Path-- \\rdlfs.hpicorp\..."));
                    AddFolder_Button.Visible = true;
                    deleteFolder_Button.Visible = true;
                    break;
                case "SharePoint":
                    _currentType = "SharePoint";
                    _parameterList.Add(new QuickSetParameterData("Sharepoint Path", $@"http://sharepoint01.etl.boi.rd.hpicorp.net", $@"http://sharepoint01.etl.boi.rd.hpicorp.net"));

                    break;
                case "USB":
                    _currentType = "USB";
                    break;
                case "Fax":
                    _currentType = "Fax";
                    _parameterList.Add(new QuickSetParameterData("Phone Number", "555-555-5555", $@"Ex: ###-###-####"));
                    break;
                case "FTP":
                    _currentType = "FTP";
                    _parameterList.Add(new QuickSetParameterData("FTP Server", "", $@"FTP Server Path -- ftp.test.com"));
                    _parameterList.Add(new QuickSetParameterData("Port", "21", $@"Port number"));
                    _parameterList.Add(new QuickSetParameterData("Directory Path", "", $@"Folder path -- //folder/folder"));
                    _parameterList.Add(new QuickSetParameterData("User name", "", $@"FTP username"));
                    _parameterList.Add(new QuickSetParameterData("FTP Protocol", "FTP", $@"FTP/Secure FTP(SFTP)"));
                    break;
                default:
                    throw new Exception("Invalid Quickset");

            }
            if (_currentType != "Folder")
            {
                parameter_GridView.AllowUserToAddRows = false;
                AddFolder_Button.Visible = false;
                deleteFolder_Button.Visible = false;
            }
            quickSetParameterDataBindingSource.ResetBindings(false);
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }


        public IComponentData GetData()
        {
            return _quickSetList;
        }

        public void SetData()
        {
            //TODO
            //Depending on what type of data we have set the different parameters
            //Unneeded, we have default data and then change on user interaction --Maybe selected the first item if any are available
        }



        public void SetControl(IEnumerable<IComponentData> list)
        {
            //Updates the list, default values will be set from the buttons/selections

            RemoveEventHandlers();
            _quickSetList = list.OfType<QuickSetSettingsData>().FirstOrDefault();


            foreach (var item in _quickSetList.QuickSetData.Key)
            {
                _viewList.Add(new QuickSetTableData(item.QName, item.QType));
            }
            quickSetTableDataBindingSource.DataSource = _viewList;
            if (_viewList.Count > 0)
            {
                save_changes.Enabled = true;
            }

            AddEventHandlers();
        }


        private void add_Button_Click(object sender, EventArgs e)
        {
            bool success = false;
            string errorMessage = "Please verify your parameters match the criteria";
            //string message = "";
            //List<QuickSetParameterData> param;
            //1. Parse out individual Parameters
            //2. Create Scan settings data
            //3. Create individual quickset data
            //4. ???
            //5. Profit

            if (!string.IsNullOrWhiteSpace(name_TextBox.Text))
            {
                
                var scanSettingsData = new ScanSettings(originalSize_ComboBox.SelectedValue.ToString(), originalSides_ComboBox.SelectedValue.ToString(), contentOrientation_ComboBox.SelectedValue.ToString(), imagePreview_ComboBox.SelectedValue.ToString());
                var fileSettingsData = new FileSettings
                {
                    FileType = new DataPair<string>
                    {
                        Key = fileType_ComboBox.SelectedValue.ToString(),
                        Value = true
                    },
                    Resolution = new DataPair<string>()
                    {
                        Key = resolution_ComboBox.SelectedValue.ToString(),
                        Value = true
                    }
                };
                List<QuickSetParameterData> param = (List<QuickSetParameterData>)quickSetParameterDataBindingSource.List;


                //Validate Fields here
                //Add validation for 

                AQuickSet addQuickSet;
                switch (quickSetType_ComboBox.SelectedValue.ToString())
                {
                    case "Email":
                        _currentType = "Email";

                        if (EmailQuickSetData.Validate(param))
                        {

                            string to = param[0].ParameterValue;
                            string defaultFrom = param[1].ParameterValue;
                            string fromUser = param[2].ParameterValue;

                            addQuickSet = new EmailQuickSetData(fromUser, defaultFrom, to, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }

                        break;
                    case "Copy":
                        _currentType = "Copy";

                        if (CopyQuickSetData.Validate(param))
                        {
                            string copies = param[0].ParameterValue;

                            addQuickSet = new CopyQuickSetData(copies, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }
                        break;
                    case "Folder":
                        _currentType = "Folder";
                        if (STNFQuickSetData.Validate(param))
                        {
                            List<string> folderPaths = param.Select(x => x.ParameterValue).ToList(); 

                            //string folderPath = param[0].ParameterValue;

                            addQuickSet = new STNFQuickSetData(folderPaths, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }
                        break;
                    case "SharePoint":
                        _currentType = "SharePoint";
                        if (STSharePointQuickSetData.Validate(param))
                        {

                            string path = param[0].ParameterValue;

                            addQuickSet = new STSharePointQuickSetData(path, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }

                        break;
                    case "USB":
                        _currentType = "USB";
                        if (STUSBQuickSetData.Validate(param))
                        {
                            addQuickSet = new STUSBQuickSetData(name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }
                        break;
                    case "Fax":
                        _currentType = "Fax";
                        if (ScanFaxQuickSetData.Validate(param))
                        {
                            string number = param[0].ParameterValue;

                            addQuickSet = new ScanFaxQuickSetData(number, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }
                        break;
                    case "FTP":
                        _currentType = "FTP";
                        if (FTPQuickSetData.Validate(param))
                        {
                            List<string> ftpPaths = param.Select(x => x.ParameterValue).ToList();

                            addQuickSet = new FTPQuickSetData(ftpPaths, name_TextBox.Text, _currentType, scanSettingsData, fileSettingsData);
                            _quickSetList.QuickSetData.Key.Add(addQuickSet);
                            _viewList.Add(new QuickSetTableData(addQuickSet.QName, addQuickSet.QType));
                            success = true;
                        }
                        break;
                    default:
                        throw new Exception("Quick Set Type is unhandled, failed to create.");
                }
            }
            else
            {
                success = false;
                errorMessage = "A quick set name is required.";
            }

            if (!success)
            {
                MessageBox.Show(errorMessage, "Parameter Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            quickSetTableDataBindingSource.ResetBindings(false);
            if (_viewList.Count > 0)
            {
                save_changes.Enabled = true;
            }
        }

        private void delete_Button_Click(object sender, EventArgs e)
        {
            if (_SelectedQuickSet == null || _selectedRow == null)
            {
                return;
            }

            string name = string.Empty;
            string type = string.Empty;
            var row = quickSet_GridView.CurrentRow;
            if (row == null)
            {
                return;
            }

            name = row.Cells[0].Value.ToString();
            type = row.Cells[1].Value.ToString();
            
            _SelectedQuickSet = _quickSetList.QuickSetData.Key.Where(x => x.QName == name && x.QType == type).Select(x => x).FirstOrDefault();

            _viewList.Remove(_viewList.FirstOrDefault(x => x.QuickSetName == _SelectedQuickSet.QName &&  x.QuickSetType == _SelectedQuickSet.QType));
            _quickSetList.QuickSetData.Key.Remove(_SelectedQuickSet);
            _SelectedQuickSet = null;
            _selectedRow = null;


            quickSetTableDataBindingSource.ResetBindings(false);
            if (_viewList.Count < 1)
            {
                save_changes.Enabled = false;
            }
        }

        private void AddEventHandlers()
        {
            quickSetTableDataBindingSource.CurrentChanged += OnControlComponentChanged;
        }

        private void RemoveEventHandlers()
        {
            quickSetTableDataBindingSource.CurrentChanged -= OnControlComponentChanged;
        }

        private void save_changes_Click(object sender, EventArgs e)
        {
            if (_selectedRow == null)
            {
                return;
            }

            delete_Button_Click(null, null);
            add_Button_Click(null, null);

        }

        private void SetChoiceControlDataSource(ChoiceComboControl choiceComboControl, Dictionary<string, string> dataSourceDictionary)
        {
            choiceComboControl.choice_Combo.DataSource = new BindingSource(dataSourceDictionary, null);
            choiceComboControl.choice_Combo.DisplayMember = "Key";
            choiceComboControl.choice_Combo.ValueMember = "Value";
        }

        private void SetComboBoxDataSource(ComboBox comboBox, Dictionary<string, string> dataSourceDictionary)
        {
            comboBox.DataSource = new BindingSource(dataSourceDictionary, null);
            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";

        }


        private void AddFolder_Button_Click(object sender, EventArgs e)
        {
            if (_currentType == "Folder")
            {
                parameter_GridView.AllowUserToAddRows = true;
                //_currentType = "Folder";
                STNFQuickSetData stnfSelected = _SelectedQuickSet as STNFQuickSetData;
                _parameterList.Add(new QuickSetParameterData("Network Path", $@"\\rdlfs.rdl.boi.rd.hpicorp.net\Public", $@"UNC Path-- \\rdlfs.hpicorp\..."));
                quickSetParameterDataBindingSource.ResetBindings(false);
            }
            else
            {
                parameter_GridView.AllowUserToAddRows = false;
            }
        }

        private void deleteFolder_Button_Click(object sender, EventArgs e)
        {
            if (_currentType == "Folder" && _SelectedQuickSet != null)
            {
                parameter_GridView.AllowUserToDeleteRows = true;

                STNFQuickSetData stnfSelected = _SelectedQuickSet as STNFQuickSetData;

                //var temp = _parameterList.Where(x => stnfSelected.FolderPaths.Contains(x.ParameterValue, Comparer.));
                //foreach()


                //int index = _parameterList.IndexOf(_parameterList.Select(x => x.ParameterValue).Where(x => x == "FolderPath").Intersect(stnfSelected.FolderPaths)).FirstOrDefault());
                int index = -1;
                index = parameter_GridView.CurrentCell.RowIndex;
                if (index > -1 && parameter_GridView.RowCount > 1)
                {
                    _parameterList.RemoveAt(index);
                    quickSetParameterDataBindingSource.ResetBindings(false);
                }

            }
            else
            {
                parameter_GridView.AllowUserToDeleteRows = false;
            }
        }
    }
}
