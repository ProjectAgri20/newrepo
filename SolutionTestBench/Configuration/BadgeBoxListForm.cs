using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Form for managing Badge Box and Badge data.
    /// </summary>
    public partial class BadgeBoxListForm : Form
    {
        private SortableBindingList<BadgeBox> _badgeBoxes = new SortableBindingList<BadgeBox>();
        private AssetInventoryContext _context = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBoxListForm"/> class.
        /// </summary>
        public BadgeBoxListForm()
        {
            InitializeComponent();
            dataGridView_BadgeBox.AutoGenerateColumns = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _context = DbConnect.AssetInventoryContext();

            foreach (BadgeBox badgeBox in _context.BadgeBoxes)
            {
                _badgeBoxes.Add(badgeBox);
            }

            dataGridView_BadgeBox.DataSource = _badgeBoxes;
            badgeListControl.Initialize(_context.Badges, _context);

        }

        private BadgeBox SelectedBadgeBox
        {
            get
            {
                if (dataGridView_BadgeBox.SelectedRows.Count > 0)
                {
                    var row = dataGridView_BadgeBox.SelectedRows[0];
                    return (BadgeBox)row.DataBoundItem;
                }
                return null;
            }
        }

        private void button_AddBadgeBox_Click(object sender, EventArgs e)
        {
            BadgeBox newBadgeBox = new BadgeBox();
            using (BadgeBoxEditForm dialog = new BadgeBoxEditForm())
            {
                dialog.Initialize(newBadgeBox, _context);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _badgeBoxes.Add(newBadgeBox);
                    badgeListControl.Refresh();
                }
            }
        }

        private void button_EditBadgeBox_Click(object sender, EventArgs e)
        {
            BadgeBox selected = SelectedBadgeBox;

            if (selected != null)
            {
                using (BadgeBoxEditForm dialog = new BadgeBoxEditForm())
                {
                    dialog.Initialize(selected, _context);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        badgeListControl.Refresh();
                    }
                }
            }
        }

        private void button_DeleteBadgeBox_Click(object sender, EventArgs e)
        {
            BadgeBox selected = SelectedBadgeBox;
            if (selected != null && MessageBox.Show($"Delete '{selected.BadgeBoxId}'?", "Delete Badge Box", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _badgeBoxes.Remove(selected);
                _context.BadgeBoxes.Remove(selected);
                _context.SaveChanges();
                badgeListControl.Refresh();
            }
        }

        private void dataGridView_BadgeBox_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button_EditBadgeBox_Click(this, EventArgs.Empty);
        }
    }
}
