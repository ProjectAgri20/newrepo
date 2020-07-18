using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;
using HP.ScalableTest.LabConsole.Properties;

namespace HP.ScalableTest.LabConsole
{
    public partial class GlobalSettingsEditForm : Form
    {
        private SystemSetting _setting = null;
        private EnterpriseTestContext _context = null;
        private List<string> _subTypes = new List<string>();
        private List<SystemSetting> _linkedSettings = null;
        private List<SystemSetting> _addedItems = new List<SystemSetting>();

        /// <summary>
        /// Edit a SystemSetting.  Also link the current SystemSetting to other Plugin types.
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="context"></param>
        public GlobalSettingsEditForm(SystemSetting setting, EnterpriseTestContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _setting = setting;
            _context = context;

            LoadSubTypes();

            fieldValidator.RequireValue(name_TextBox, name_Label);
            fieldValidator.RequireValue(value_TextBox, value_Label);
            fieldValidator.RequireCustom(subType_ListBox, () => subType_ListBox.CheckedItems.Count > 0, "At least one Type must be selected.");
            fieldValidator.SetIconAlignment(name_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(value_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(subType_ListBox, ErrorIconAlignment.MiddleLeft);

            this.Text = EnumUtil.Parse<SettingType>(_setting.Type).GetDescription();
        }

        /// <summary>
        /// Edit a SystemSetting.  Also link the current SystemSetting to other Plugin types.
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="context"></param>
        /// <param name="linkedSettings"></param>
        public GlobalSettingsEditForm(SystemSetting setting, EnterpriseTestContext context, List<SystemSetting> linkedSettings) :
            this(setting, context)
        {
            _linkedSettings = linkedSettings;
        }

        private void GlobalSettingsEditForm_Load(object sender, EventArgs e)
        {
            subType_ListBox.DataSource = _subTypes;
            RefreshSelectedSubTypes();
            //Wire up the Check event handler new that the data source has been set.
            subType_ListBox.ItemCheck += new ItemCheckEventHandler(this.subType_ListBox_ItemCheck);

            name_TextBox.DataBindings.Add("Text", _setting, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            value_TextBox.DataBindings.Add("Text", _setting, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            description_TextBox.DataBindings.Add("Text", _setting, "Description", true, DataSourceUpdateMode.OnPropertyChanged);

            // If the setting name already exists, lock it down so the value can only be changed.
            if (HasDuplicateName(_setting.Name))
            {
                value_TextBox.Focus();
                name_TextBox.ReadOnly = true;
                subType_ListBox.Enabled = SettingType == SettingType.PluginSetting;
            }
            else
            {
                if (SettingType != SettingType.PluginSetting)
                {
                    _setting.SubType = _subTypes.First();
                }
                name_TextBox.Focus();
            }
        }

        /// <summary>
        /// Any SystemSettings added as linked items to the existing SystemSetting being viewed.
        /// </summary>
        public List<SystemSetting> AddedItems
        {
            get { return _addedItems; }
        }

        private SettingType SettingType
        {
            get { return (SettingType)Enum.Parse(typeof(SettingType), _setting.Type); }
        }

        private void LoadSubTypes()
        {
            switch (SettingType)
            {
                case SettingType.SystemSetting:
                    _subTypes.Add("FrameworkSetting");
                    _subTypes.Add("WcfHostSetting");
                    break;
                case SettingType.PluginSetting:
                    _subTypes = _context.MetadataTypes.OrderBy(x => x.Name).Select(x => x.Name).ToList();
                    break;
                case SettingType.ServerSetting:
                    _subTypes.Add(_setting.SubType);
                    break;
                default:
                    break;
            }
        }

        private void RefreshSelectedSubTypes()
        {
            int idx = -1;
            switch (SettingType)
            {
                case SettingType.PluginSetting:
                    instruction_Label.Text = Resources.PluginSettingsPrompt;
                    instruction_Label.Visible = true;
                    foreach (SystemSetting setting in _linkedSettings)
                    {
                        if (_linkedSettings.Contains(_setting))
                        {
                            idx = subType_ListBox.Items.IndexOf(setting.SubType);
                            if (idx > -1)
                            {
                                subType_ListBox.SetItemChecked(idx, true);
                            }
                        }
                    }
                    break;
                default:
                    idx = subType_ListBox.Items.IndexOf(_setting.SubType);
                    if (idx > -1)
                    {
                        subType_ListBox.SetItemChecked(idx, true);
                    }
                    break;
            }
            ScrollToFirst();
        }

        private void ScrollToFirst()
        {
            int idx = 0;

            if (subType_ListBox.CheckedItems.Count > 0)
            {
                idx = subType_ListBox.CheckedIndices[0];
            }

            subType_ListBox.TopIndex = idx;
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                UpdateLinkedSettings();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool HasDuplicateName(string name)
        {
            return _context.SystemSettings.Any(x => 
                x.Type.Equals(_setting.Type, StringComparison.OrdinalIgnoreCase) &&
                x.SubType.Equals(_setting.SubType, StringComparison.OrdinalIgnoreCase) &&
                x.Name.Equals(name_TextBox.Text, StringComparison.OrdinalIgnoreCase));

        }

        private bool HasDuplicateName()
        {
            if (!name_TextBox.ReadOnly)
            {
                if (HasDuplicateName(name_TextBox.Text))
                {
                    MessageBox.Show("Setting Name '{0}' already exists.".FormatWith(name_TextBox.Text), "Save Setting", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    name_TextBox.Focus();
                    return true;
                }
            }

            return false;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void subType_ListBox_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < subType_ListBox.Items.Count; i++)
            {
                if (subType_ListBox.GetItemRectangle(i).Contains(subType_ListBox.PointToClient(MousePosition)))
                {
                    switch (subType_ListBox.GetItemCheckState(i))
                    {
                        case CheckState.Checked:
                            subType_ListBox.SetItemCheckState(i, CheckState.Unchecked);
                            break;
                        case CheckState.Indeterminate:
                        case CheckState.Unchecked:
                            subType_ListBox.SetItemCheckState(i, CheckState.Checked);
                            break;
                    }
                }
                else
                {
                    if (SettingType != SettingType.PluginSetting)
                    {
                        subType_ListBox.SetItemCheckState(i, CheckState.Unchecked);
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            return (fieldValidator.ValidateAll().All(x => x.Succeeded == true)
                && HasDuplicateName() == false);

        }

        private void UpdateLinkedSettings()
        {
            if (_linkedSettings != null)
            {
                foreach (SystemSetting setting in _linkedSettings)
                {
                    setting.Value = value_TextBox.Text;
                    setting.Description = description_TextBox.Text;
                }
            }
        }

        private void subType_ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string selectedSubType = (string)subType_ListBox.Items[e.Index];

            if (e.NewValue == CheckState.Checked)
            {
                if (string.IsNullOrEmpty(_setting.SubType))
                {
                    _setting.SubType = selectedSubType;
                    return;
                }

                if (SettingType == SettingType.PluginSetting)
                {
                    AddLinkedItem(selectedSubType);
                }
            }
            else if (SettingType == SettingType.PluginSetting)
            {
                RemoveLinkedItem(selectedSubType);
            }
        }

        private void AddLinkedItem(string selectedSubType)
        {
            // Only add if the subType is not already in the list AND is not the curent setting
            if (!_addedItems.Any(i => i.SubType == selectedSubType) && selectedSubType != _setting.SubType)
            {
                SystemSetting setting = new SystemSetting()
                {
                    Type = _setting.Type,
                    SubType = selectedSubType,
                    Name = name_TextBox.Text,
                    Value = value_TextBox.Text,
                    Description = description_TextBox.Text
                };

                _addedItems.Add(setting);
            }
        }

        private void RemoveLinkedItem(string selectedSubType)
        {
            SystemSetting setting = _addedItems.Where(i => i.SubType == selectedSubType).FirstOrDefault();
            if (setting != null)
            {
                _addedItems.Remove(setting);
            }

            //Check linked settings list as well.
            setting = _linkedSettings.Where(i => i.SubType == selectedSubType).FirstOrDefault();
            if (setting != null)
            {
                _linkedSettings.Remove(setting);
            }
        }

    }
}
