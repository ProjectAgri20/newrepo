using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Form for editing a ServerSetting.
    /// </summary>
    public partial class ServerSettingsEditForm : Form
    {
        private ServerSetting _setting = null;
        private bool _addingNew;

        /// <summary>
        /// Creates a new instance of ServerSettingsEditForm.
        /// </summary>
        /// <param name="setting"></param>
        public ServerSettingsEditForm(ServerSetting setting, bool addingNew)
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _setting = setting;
            _addingNew = addingNew;

            fieldValidator.RequireValue(name_TextBox, name_Label);
            fieldValidator.RequireValue(value_TextBox, value_Label);
            fieldValidator.SetIconAlignment(name_TextBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(value_TextBox, ErrorIconAlignment.MiddleLeft);

        }

        private void GlobalSettingsEditForm_Load(object sender, EventArgs e)
        {
            name_TextBox.DataBindings.Add("Text", _setting, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            value_TextBox.DataBindings.Add("Text", _setting, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            description_TextBox.DataBindings.Add("Text", _setting, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ServerSettingsEditForm_Activated(object sender, EventArgs e)
        {
            // If the setting is being edited, lock down the name so only the value can be changed.
            if (_addingNew)
            {
                //Detached == new
                name_TextBox.Focus();
            }
            else
            {
                value_TextBox.Focus();
                name_TextBox.ReadOnly = true;
            }
        }

        private void Ok_Button_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInput()
        {
            return (fieldValidator.ValidateAll().All(x => x.Succeeded == true));
        }


    }
}
