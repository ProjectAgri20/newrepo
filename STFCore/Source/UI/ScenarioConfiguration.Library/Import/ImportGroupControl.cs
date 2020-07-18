using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportGroupControl : UserControl
    {
        private Collection<string> _groups = null;

        public event EventHandler OnNodeSelected;

        public ImportGroupControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            editorGroups_CheckedListBox.ItemCheck += EditorGroups_CheckedListBox_ItemCheck;
        }

        private void EditorGroups_CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateGroups((UserGroup)editorGroups_CheckedListBox.Items[e.Index], e.NewValue == CheckState.Checked);
        }

        private void UpdateGroups(UserGroup group, bool add)
        {
            if (add && !_groups.Contains(group.GroupName))
            {
                _groups.Add(group.GroupName);
            }
            else if (!add && _groups.Contains(group.GroupName))
            {
                _groups.Remove(group.GroupName);
            }
        }

        public void LoadGroups(EnterpriseScenarioContract contract)
        {
            _groups = contract.UserGroups;

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                editorGroups_CheckedListBox.Items.Clear();
                foreach (UserGroup group in context.UserGroups.OrderBy(x => x.GroupName))
                {
                    editorGroups_CheckedListBox.Items.Add(group);
                }
            }

            var listBoxItems = editorGroups_CheckedListBox.Items.Cast<UserGroup>().Select(x => x.GroupName).ToList();
            var missingGroups = _groups.Where(x => !listBoxItems.Contains(x)).ToList();

            foreach (var group in missingGroups)
            {
                _groups.Remove(group);
            }

            foreach (var groupName in listBoxItems)
            {
                if (_groups.Contains(groupName))
                {
                    int index = listBoxItems.IndexOf(groupName);
                    editorGroups_CheckedListBox.SetItemCheckState(index, CheckState.Checked);
                }
            }
        }

        private void ImportGroupControl_Load(object sender, EventArgs e)
        {
        }
    }
}
