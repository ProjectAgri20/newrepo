using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Control for viewing and managing badge data.
    /// </summary>
    public partial class BadgeListControl : UserControl
    {
        private AssetInventoryContext _context = null;
        private SortableBindingList<Badge> _badges = new SortableBindingList<Badge>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeListControl"/> class.
        /// </summary>
        public BadgeListControl()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Initializes the <see cref="BadgeListControl"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Initialize(AssetInventoryContext context)
        {
            _context = context;
            Initialize(_context.Badges.AsEnumerable<Badge>());
        }

        /// <summary>
        /// Initializes the <see cref="BadgeListControl"/>.
        /// </summary>
        /// <param name="badges">The badges.</param>
        /// <param name="context">The context.</param>
        public void Initialize(IEnumerable<Badge> badges, AssetInventoryContext context)
        {
            _context = context;
            Initialize(badges);
        }

        /// <summary>
        /// Gets or sets the associated badge box.
        /// </summary>
        /// <value>The badge box.</value>
        public BadgeBox BadgeBox { get; set; }


        /// <summary>
        /// Refreshes the data in the control.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Initialize(_context.Badges);
        }

        private void Initialize(IEnumerable<Badge> badges)
        {
            dataGridView.DataSource = null;
            _badges.Clear();

            foreach (Badge badge in badges)
            {
                _badges.Add(badge);
            }

            dataGridView.DataSource = _badges;
        }

        private Badge SelectedBadge
        {
            get
            {
                if (dataGridView.SelectedRows.Count > 0)
                {
                    var row = dataGridView.SelectedRows[0];
                    return (Badge)row.DataBoundItem;
                }
                return null;
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            Badge newBadge = new Badge();
            if (BadgeBox != null)
            {
                newBadge.BadgeBoxId = BadgeBox.BadgeBoxId;
            }

            using (BadgeEditForm dialog = new BadgeEditForm())
            {
                dialog.Initialize(newBadge, _context);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _badges.Add(newBadge);
                }
            }
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            Badge selected = SelectedBadge;

            if (selected != null)
            {
                using (BadgeEditForm dialog = new BadgeEditForm())
                {
                    dialog.Initialize(selected, _context);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        // Refresh the grid UI to display any updates
                        ((CurrencyManager)dataGridView.BindingContext[_badges]).Refresh();
                    }
                }
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            Badge selected = SelectedBadge;
            if (selected != null && MessageBox.Show($"Delete '{selected.BadgeId}'?", "Delete Badge", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _badges.Remove(selected);
                _context.Badges.Remove(selected);
                _context.SaveChanges();
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button_Edit_Click(sender, EventArgs.Empty);
        }
    }
}
