using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class UserManagementListForm : Form
    {
        private SortableBindingList<User> _users = null;
        private EnterpriseTestContext _context = null;
        private Collection<User> _deletedItems = new Collection<User>();
        bool _unsavedChanges = false;

        public UserManagementListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialogWithHelp);

            UserInterfaceStyler.Configure(user_RadGridView, GridViewStyle.ReadOnly);
            //user_RadGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            ////user_RadGridView.MasterTemplate.BestFitColumns();

            //foreach (var column in user_RadGridView.Columns)
            //{
            //    column.AutoSizeMode = BestFitColumnMode.AllCells;
            //    column.BestFit();
            //}

            _users = new SortableBindingList<User>();
            _context = new EnterpriseTestContext();
        }

        private void UserManagementListForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                foreach (var item in _context.Users)
                {
                    item.JoinGroups();
                    _users.Add(item);
                }

                user_RadGridView.DataSource = _users;

                user_RadGridView.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                user_RadGridView.BestFitColumns();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return user_RadGridView.SelectedRows.FirstOrDefault();
        }

        private void user_RadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var user = row.DataBoundItem as User;

                if (EditEntry(user) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    user_RadGridView.Refresh();
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            Commit();
            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                    return;
                }
            }
            else
            {
                Close();
                return;
            }
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            Cursor = Cursors.WaitCursor;

            foreach (var entity in _deletedItems)
            {
                _context.Users.DeleteObject(entity);
            }

            foreach (var item in _users)
            {
                switch (item.EntityState)
                {
                    case EntityState.Added:
                        {
                            _context.Users.AddObject(item);
                            break;
                        }
                }
            }

            _context.SaveChanges();
            _deletedItems.Clear();

            Cursor = Cursors.Default;
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Commit();
            Cursor = Cursors.Default;

            _unsavedChanges = false;
        }

        private void add_ToolStripButton_Click(object sender, EventArgs e)
        {
            User user = new User();

            if (EditEntry(user) == DialogResult.OK)
            {
                _context.AddToUsers(user);
                _users.Add(user);

                int index = user_RadGridView.Rows.Count - 1;

                user_RadGridView.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                user_RadGridView.TableElement.ScrollToRow(index);
            }
        }

        private void edit_ToolStripButton_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var user = row.DataBoundItem as User;

                if (EditEntry(user) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    user_RadGridView.Refresh();
                }
            }
        }

        private void delete_ToolStripButton_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var user = row.DataBoundItem as User;

                var dialogResult = MessageBox.Show
                    (
                        "Removing user {0}.  Do you want to continue?".FormatWith(user.UserName),
                        "Delete User",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.Yes)
                {
                    _unsavedChanges = true;
                    _deletedItems.Add(user);
                    _users.Remove(user);
                }
            }
        }

        private DialogResult EditEntry(User user)
        {
            DialogResult result = DialogResult.None;

            using (UserManagementForm form = new UserManagementForm(user, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void UserManagementListForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            using (HelpDialog dialog = new HelpDialog(Properties.Resources.PluginMetadataHelpPage))
            {
                dialog.Title = "User Reference Help";
                dialog.ShowDialog();
            }
        }
    }
}
