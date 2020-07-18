using System;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class ActiveDirectoryGroupManagementForm : Form
    {
        private ActiveDirectoryGroup _group = null;
        private Collection<GroupPrincipal> _masterItemsList = null;
        SortableBindingList<ActiveDirectoryGroup> _selectedItemsList = null;

        public ActiveDirectoryGroupManagementForm
            (
                Collection<GroupPrincipal> masterItemsList,
                SortableBindingList<ActiveDirectoryGroup> selectedItemsList,
                ActiveDirectoryGroup group
            )
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _masterItemsList = masterItemsList;
            _selectedItemsList = selectedItemsList;
            _group = group;

            groupName_ComboBox.DisplayMember = "Name";
        }

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            var selectedItem = _selectedItemsList.FirstOrDefault(x => x.Name.Equals(_group.Name));
            if (selectedItem != null)
            {
                var actualGroup = _masterItemsList.FirstOrDefault(x => x.Name.Equals(_group.Name));
                if (actualGroup != null)
                {
                    groupName_ComboBox.Items.Add(actualGroup);
                    groupName_ComboBox.SelectedIndex = 0;
                    groupName_ComboBox.Enabled = false;
                    description_TextBox.Focus();
                }
                else
                {
                    MessageBox.Show("The group '{0}' is not configured on the Active Directory server, so it will be removed");
                    _selectedItemsList.Remove(selectedItem);
                    Close();
                }                
            }
            else
            {
                foreach (var item in _masterItemsList)
                {
                    if (!_selectedItemsList.Any(x => x.Name.Equals(item.Name)))
                    {
                        groupName_ComboBox.Items.Add(item);
                    }
                }
            }
            
            description_TextBox.DataBindings.Add("Text", _group, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(description_TextBox.Text))
            {
                MessageBox.Show("A description is required defining how you will be using this group in tests", "Description Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                description_TextBox.Focus();
                return;
            }

            _group.Name = ((GroupPrincipal)groupName_ComboBox.SelectedItem).Name;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
