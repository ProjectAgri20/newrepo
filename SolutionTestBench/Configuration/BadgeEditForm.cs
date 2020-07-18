using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Form for editing badge data.
    /// </summary>
    public partial class BadgeEditForm : Form
    {
        private AssetInventoryContext _context = null;
        private Badge _badge = null;
        private ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeEditForm"/>.
        /// </summary>
        public BadgeEditForm()
        {
            InitializeComponent();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        /// <summary>
        /// Initializes the <see cref="BadgeEditForm"/>.
        /// </summary>
        /// <param name="badge">The badge.</param>
        /// <param name="context">The context.</param>
        public void Initialize(Badge badge, AssetInventoryContext context)
        {
            _context = context;
            _badge = badge;

            LoadBadgeBoxItems();
            comboBox_BadgeBox.SelectedIndex = 0; //Default to the "None" item

            // Display badge data
            textBox_BadgeId.Text = _badge.BadgeId;
            textBox_BadgeId.Enabled = string.IsNullOrEmpty(_badge.BadgeBoxId);
            textBox_Descr.Text = _badge.Description;
            textBox_Username.Text = _badge.UserName;
            textBox_Index.Text = _badge.Index.ToString();
            if (_badge.BadgeBoxId != null)
            {
                comboBox_BadgeBox.SelectedValue = _badge.BadgeBoxId;
            }

        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                SaveChanges();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void textBox_Descr_Validating(object sender, CancelEventArgs e)
        {
            SetError(FieldValidator.HasValue(textBox_Descr, label_Descr) == ValidationResult.Success, label_Descr, e);
        }

        private void textBox_Username_Validating(object sender, CancelEventArgs e)
        {
            SetError(FieldValidator.HasValue(textBox_Username, label_Username) == ValidationResult.Success, label_Username , e);
        }

        private void textBox_Index_Validating(object sender, CancelEventArgs e)
        {
            SetError(ValidateIndex(), label_Index, e, $"{label_Index.Text} must be 0-3");
        }

        private void LoadBadgeBoxItems()
        {
            List<KeyValuePair<string,string>> items = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> emptyItem = new KeyValuePair<string, string>("<None>", string.Empty);
            items.Add(emptyItem);

            foreach (BadgeBox badgeBox in _context.BadgeBoxes)
            {
                items.Add(new KeyValuePair<string, string>(badgeBox.BadgeBoxId, badgeBox.BadgeBoxId));
            }

            comboBox_BadgeBox.DataSource = items;
        }

        private void SetError(bool isValid, Label label, CancelEventArgs e, string message = null)
        {
            if (isValid)
            {
                message = null;
            }
            else
            {
                if (message == null)
                {
                    message = $"{label.Text} must have a valid value";
                }
            }

            _errorProvider.SetError(label, message);
            e.Cancel = (message != null);
        }

        private bool ValidateIndex()
        {
            byte byteVal;
            bool result = byte.TryParse(textBox_Index.Text, out byteVal);

            if (result)
            {
                result = (byteVal < 4);
            }

            return result;
        }

        /// <summary>
        /// Updates the object from the UI and saves any changes.
        /// </summary>
        private void SaveChanges()
        {
            // Check for new object
            if (string.IsNullOrEmpty(_badge.BadgeId))
            {
                _badge.BadgeId = textBox_BadgeId.Text;
                _context.Badges.Add(_badge);
            }
            _badge.Description = textBox_Descr.Text;
            _badge.UserName = textBox_Username.Text;
            _badge.Index = byte.Parse(textBox_Index.Text);
            if (comboBox_BadgeBox.SelectedIndex > 0)
            {
                _badge.BadgeBoxId = (string)comboBox_BadgeBox.SelectedValue;
            }
            else // No Badge Box selected
            {
                _badge.BadgeBoxId = null;
            }

            if (_context.Entry(_badge).State == EntityState.Modified || _context.Entry(_badge).State == EntityState.Added)
            {
                _context.SaveChanges();
            }
        }


    }
}
