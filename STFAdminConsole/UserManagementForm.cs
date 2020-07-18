using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    public partial class UserManagementForm : Form
    {
        private User _user = null;
        private EnterpriseTestContext _context = null;

        public UserManagementForm(User user, EnterpriseTestContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _user = user;
            _context = context;
        }

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            if (_context.Users.Any(x => x.UserName.Equals(_user.UserName, StringComparison.OrdinalIgnoreCase)))
            {
                userName_TextBox.ReadOnly = true;
                domain_TextBox.Focus();
            }

            groups_CheckedListBox.DisplayMember = "GroupName";
            
            foreach (var group in _context.UserGroups)
            {
                int index = groups_CheckedListBox.Items.Add(group);

                if (_user.UserGroups.Any(x => x.GroupName.Equals(group.GroupName)))
                {
                    groups_CheckedListBox.SetItemChecked(index, true);
                }
            }            

            foreach (string item in EnumUtil.GetDescriptions<UserRole>().OrderBy(x => x))
            {
                role_ComboBox.Items.Add(item);
            }

            if (!string.IsNullOrEmpty(_user.RoleName))
            {
                role_ComboBox.SelectedItem = _user.RoleName;
            }


            userName_TextBox.DataBindings.Add("Text", _user, "UserName", true, DataSourceUpdateMode.OnPropertyChanged);
            domain_TextBox.DataBindings.Add("Text", _user, "Domain", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (!userName_TextBox.ReadOnly)
            {
                if (_context.Users.Any(x => x.UserName.Equals(userName_TextBox.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Username {0} already exists.".FormatWith(userName_TextBox.Text), "User Name Exists", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    userName_TextBox.Focus();
                    return;
                }
            }

            if (string.IsNullOrEmpty(userName_TextBox.Text))
            {
                MessageBox.Show("A User Name is required", "User Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                userName_TextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(domain_TextBox.Text))
            {
                MessageBox.Show("A domain is required", "Domain Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                domain_TextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(role_ComboBox.Text))
            {
                MessageBox.Show("A Role is required", "Role Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                role_ComboBox.Focus();
                return;
            }

            _user.Role = EnumUtil.GetByDescription<UserRole>(role_ComboBox.Text);
            
            _user.UserGroups.Clear();
            foreach (var item in groups_CheckedListBox.CheckedItems.Cast<UserGroup>())
            {
                _user.UserGroups.Add(item);
            }

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
