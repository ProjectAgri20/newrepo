using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class UserGroupEditForm : Form
    {
        private UserGroup _masterGroup = null;
        private UserGroup _localGroup = null;
        private EnterpriseTestContext _context = null;

        public UserGroupEditForm(UserGroup group, EnterpriseTestContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            _masterGroup = group;
            _localGroup = new UserGroup()
            {
                GroupName = group.GroupName,
                Description = group.Description
            };

            _context = context;
        }

        private void UserEditForm_Load(object sender, EventArgs e)
        {
            if (_context.UserGroups.Any(x => x.GroupName.Equals(_localGroup.GroupName, StringComparison.OrdinalIgnoreCase)))
            {
                groupName_TextBox.ReadOnly = true;
                description_TextBox.Focus();
            }

            groupName_TextBox.DataBindings.Add("Text", _localGroup, "GroupName", true, DataSourceUpdateMode.OnPropertyChanged);
            description_TextBox.DataBindings.Add("Text", _localGroup, "Description", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            if (!groupName_TextBox.ReadOnly)
            {
                if (_context.Users.Any(x => x.UserName.Equals(groupName_TextBox.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Group name {0} already exists.".FormatWith(groupName_TextBox.Text), "Group Name Exists", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    groupName_TextBox.Focus();
                    return;
                }
            }

            if (string.IsNullOrEmpty(groupName_TextBox.Text))
            {
                MessageBox.Show("A Group Name is required", "Group Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                groupName_TextBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;

            _masterGroup.GroupName = _localGroup.GroupName;
            _masterGroup.Description = _localGroup.Description;

            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
