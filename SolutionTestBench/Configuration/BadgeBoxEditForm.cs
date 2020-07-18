using System;
using System.Data.Entity;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Form for editing badge box data.
    /// </summary>
    public partial class BadgeBoxEditForm : Form
    {
        private AssetInventoryContext _context = null;
        private BadgeBox _badgeBox = null;
        private ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBoxEditForm"/> class.
        /// </summary>
        public BadgeBoxEditForm()
        {
            InitializeComponent();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        /// <summary>
        /// Initializes the <see cref="BadgeBoxEditForm"/>.
        /// </summary>
        /// <param name="badgeBox">The badge box.</param>
        /// <param name="context">The context.</param>
        public void Initialize(BadgeBox badgeBox, AssetInventoryContext context)
        {
            _context = context;
            _badgeBox = badgeBox;

            textBox_BadgeBoxId.Text = _badgeBox.BadgeBoxId;
            textBox_BadgeBoxId.Enabled = string.IsNullOrEmpty(_badgeBox.BadgeBoxId);
            textBox_Descr.Text = _badgeBox.Description;
            textBox_Address.Text = _badgeBox.IPAddress;
            textBox_PrinterId.Text = _badgeBox.PrinterId;

            badgeListControl.BadgeBox = _badgeBox;
            badgeListControl.Initialize(_badgeBox.Badges, _context);
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void textBox_BadgeBoxId_Validating(object sender, CancelEventArgs e)
        {
            SetError(textBox_BadgeBoxId, label_BadgeBoxId, e);
        }

        private void textBox_Descr_Validating(object sender, CancelEventArgs e)
        {
            SetError(textBox_Descr, label_Descr, e);
        }

        private void textBox_Address_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetError(textBox_Address, label_Address, e);
        }

        private void SetError(TextBox textBox, Label label, CancelEventArgs e)
        {
            string errorMessage = null;
         
            if (FieldValidator.HasValue(textBox, label) != ValidationResult.Success)
            {
                errorMessage = $"{label.Text} must have a value";
            }

            _errorProvider.SetError(label, errorMessage);
            e.Cancel = (errorMessage != null);
        }

        /// <summary>
        /// Updates the object from the UI and saves any changes.
        /// </summary>
        private void SaveChanges()
        {
            // Check for new object
            if (string.IsNullOrEmpty(_badgeBox.BadgeBoxId))
            {
                _badgeBox.BadgeBoxId = textBox_BadgeBoxId.Text;
                _context.BadgeBoxes.Add(_badgeBox);
            }
            _badgeBox.Description = textBox_Descr.Text;
            _badgeBox.IPAddress = textBox_Address.Text;
            _badgeBox.PrinterId = textBox_PrinterId.Text;

            if (_context.Entry(_badgeBox).State == EntityState.Modified || _context.Entry(_badgeBox).State == EntityState.Added)
            {
                _context.SaveChanges();
            }
        }

    }
}
