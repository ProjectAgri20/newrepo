using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.UI;
using System.Text.RegularExpressions;

namespace HP.ScalableTest
{
    /// <summary>
    /// Class VirtualPrinterEditForm.
    /// </summary>
    public partial class DomainAccountEditForm : Form
    {
        private DomainAccountPool _pool = null;
        private AssetInventoryContext _context = null;
        private ErrorProvider _provider = null;

        private string _usernameFormat = "Virtual worker account usernames must be of the form \"<prefix>{0:0+}<suffix>\""
                + Environment.NewLine + "Examples:"
                + Environment.NewLine + "\tu{0:00000} => u00001, u00025, u99999"
                + Environment.NewLine + "\tprefix{0:000}suffix => prefix001suffix, prefix025suffix, prefix999suffix"
                + Environment.NewLine + "\t{0:00}suffix => 01suffix, 25suffix, 99suffix"
                ;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterEditForm"/> class.
        /// </summary>
        /// <param name="pool">The account pool.</param>
        /// <param name="context">The context.</param>
        public DomainAccountEditForm(DomainAccountPool pool, AssetInventoryContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            ShowIcon = true;
            _provider = new ErrorProvider();
            _provider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _provider.SetIconAlignment(poolNameTextBox, ErrorIconAlignment.MiddleLeft);
            _provider.SetIconAlignment(formatTextBox, ErrorIconAlignment.MiddleLeft);
            _provider.SetIconAlignment(startTextBox, ErrorIconAlignment.MiddleLeft);
            _provider.SetIconAlignment(endTextBox, ErrorIconAlignment.MiddleLeft);

            _pool = pool;
            _context = context;
        }

        private void PrinterEditForm_Load(object sender, EventArgs e)
        {
            descriptionTextBox.Text = _pool.Description;
            startTextBox.Text = _pool.MinimumUserNumber.ToString();
            endTextBox.Text = _pool.MaximumUserNumber.ToString();
            formatTextBox.Text = _pool.UserNameFormat;
            poolNameTextBox.Text = _pool.DomainAccountKey;

            if (!string.IsNullOrEmpty(_pool.DomainAccountKey))
            {
                poolNameTextBox.Enabled = false;
            }

            poolNameTextBox.Validating += PoolNameTextBox_Validating;
            startTextBox.Validating += StartTextBox_Validating;
            endTextBox.Validating += EndTextBox_Validating;
            formatTextBox.Validating += FormatTextBox_Validating;

            toolTip1.SetToolTip(formatTextBox, _usernameFormat);
        }

        private void PoolNameTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(poolNameTextBox.Text))
            {
                _provider.SetError(endTextBox, "A name is required");
                e.Cancel = true;
            }
        }

        private void FormatTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Regex.IsMatch(formatTextBox.Text, @"^\S*{0:[0]+}\S*$"))
            {
                _provider.SetError(formatTextBox, "The format is invalid");
                e.Cancel = true;
            }
            else
            {
                _provider.SetError(formatTextBox, string.Empty);
            }
        }

        void EndTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int value = 0;
            if (string.IsNullOrEmpty(endTextBox.Text))
            {
                _provider.SetError(endTextBox, "An integer value is required");
                e.Cancel = true;
            }
            else if (!int.TryParse(endTextBox.Text, out value))
            {
                _provider.SetError(endTextBox, "The End Index must be an integer");
                e.Cancel = true;
            }
            else
            {
                _provider.SetError(endTextBox, string.Empty);
            }
        }

        void StartTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int value = 0;
            if (string.IsNullOrEmpty(startTextBox.Text))
            {
                _provider.SetError(startTextBox, "An integer value is required");
                e.Cancel = true;
            }
            else if (!int.TryParse(startTextBox.Text, out value))
            {
                _provider.SetError(startTextBox, "The Start Index must be an integer");
                e.Cancel = true;
            }
            else
            {
                _provider.SetError(startTextBox, string.Empty);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (base.ValidateChildren())
            {
                DialogResult = DialogResult.OK;

                _pool.Description = descriptionTextBox.Text;
                _pool.MinimumUserNumber = int.Parse(startTextBox.Text);
                _pool.MaximumUserNumber = int.Parse(endTextBox.Text);
                _pool.UserNameFormat = formatTextBox.Text;
                _pool.DomainAccountKey = poolNameTextBox.Text;

                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void virtualWorkerFormat_Click(object sender, EventArgs e)
        {
            string message = "Virtual worker account usernames must be of the form \"<prefix>{0:0+}<suffix>\""
                + Environment.NewLine + "Examples:"
                + Environment.NewLine + "\tu{0:00000} --> u00001, u00025, u99999"
                + Environment.NewLine + "\t{0:00000} --> 00001, 00025, 99999"
                + Environment.NewLine + "\tprefix{0:000}suffix --> prefix001suffix, prefix025suffix, prefix999suffix"
                + Environment.NewLine + "\t{0:00}suffix --> 01suffix, 25suffix, 99suffix"
                ;
            MessageBox.Show(message, "Virtual Worker Account Username Format", MessageBoxButtons.OK);
        }
    }
}
