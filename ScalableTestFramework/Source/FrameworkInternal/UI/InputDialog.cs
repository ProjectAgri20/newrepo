using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// A simple dialog with either a text box or combo box for user input.
    /// </summary>
    public partial class InputDialog : Form
    {
        /// <summary>
        /// Gets the value specified in the input dialog.
        /// </summary>
        public string Value
        {
            get { return input_TextBox.Enabled ? input_TextBox.Text : input_ComboBox.Text; }
        }

        /// <summary>
        /// Gets or sets the character used to mask characters of a password.
        /// </summary>
        public char PasswordChar
        {
            get => input_TextBox.PasswordChar;
            set => input_TextBox.PasswordChar = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog" /> class.
        /// </summary>
        private InputDialog()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog" /> class.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        public InputDialog(string prompt)
            : this()
        {
            prompt_Label.Text = prompt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog" /> class.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        public InputDialog(string prompt, string title)
            : this(prompt)
        {
            Text = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog" /> class.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        /// <param name="initialValue">The initial value of the text box.</param>
        public InputDialog(string prompt, string title, string initialValue)
            : this(prompt, title)
        {
            input_TextBox.Text = initialValue;
        }

        /// <summary>
        /// Initializes the combo box with the list of items to display.
        /// </summary>
        /// <param name="items">The items.</param>
        public void InitializeComboBox(IEnumerable<string> items)
        {
            InitializeComboBox(items, ComboBoxStyle.DropDownList);
        }

        /// <summary>
        /// Initializes the combo box with the list of items to display and the specified <see cref="ComboBoxStyle" />.
        /// </summary>
        /// <param name="items">The items to display in the combo box.</param>
        /// <param name="style">The <see cref="ComboBoxStyle" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items" /> is null.</exception>
        public void InitializeComboBox(IEnumerable<string> items, ComboBoxStyle style)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            // Switch from text box to combo box
            input_TextBox.Visible = input_TextBox.Enabled = false;
            input_ComboBox.Visible = input_ComboBox.Enabled = true;
            input_ComboBox.DropDownStyle = style;

            // Add all the items to the combo box
            foreach (string item in items)
            {
                input_ComboBox.Items.Add(item);
            }

            // Set the selected item (if one was specified in the text box)
            if (!string.IsNullOrEmpty(input_TextBox.Text))
            {
                input_ComboBox.Text = input_TextBox.Text;
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt. 
        /// Displays a text field for user entry.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <returns>The value entered by the user, or null if the user clicked Cancel.</returns>
        public static string Show(string prompt)
        {
            using (InputDialog dialog = new InputDialog(prompt))
            {
                return Show(dialog);
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt and title. 
        /// Displays a text field for user entry.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        /// <returns>The value entered by the user, or null if the user clicked Cancel.</returns>
        public static string Show(string prompt, string title)
        {
            using (InputDialog dialog = new InputDialog(prompt, title))
            {
                return Show(dialog);
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt, title, and initial value. 
        /// Displays a text field for user entry.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        /// <param name="initialValue">The initial value of the text box.</param>
        /// <returns>The value entered by the user, or null if the user clicked Cancel.</returns>
        public static string Show(string prompt, string title, string initialValue)
        {
            using (InputDialog dialog = new InputDialog(prompt, title, initialValue))
            {
                return Show(dialog);
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt. 
        /// Displays a combo box with the specified items for user selection.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="comboBoxItems">The items to display in the combo box.</param>
        /// <returns>The value selected by the user, or null if the user clicked Cancel.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBoxItems" /> is null.</exception>
        public static string Show(string prompt, IEnumerable<string> comboBoxItems)
        {
            using (InputDialog dialog = new InputDialog(prompt))
            {
                dialog.InitializeComboBox(comboBoxItems);
                return Show(dialog);
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt and title. 
        /// Displays a combo box with the specified items for user selection.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        /// <param name="comboBoxItems">The items to display in the combo box.</param>
        /// <returns>The value selected by the user, or null if the user clicked Cancel.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBoxItems" /> is null.</exception>
        public static string Show(string prompt, string title, IEnumerable<string> comboBoxItems)
        {
            using (InputDialog dialog = new InputDialog(prompt, title))
            {
                dialog.InitializeComboBox(comboBoxItems);
                return Show(dialog);
            }
        }

        /// <summary>
        /// Shows an <see cref="InputDialog" /> with the specified prompt, title, and initial value. 
        /// Displays a combo box with the specified items for user selection.
        /// </summary>
        /// <param name="prompt">The prompt to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        /// <param name="initialValue">The initial value of the combo box.</param>
        /// <param name="comboBoxItems">The items to display in the combo box.</param>
        /// <returns>The value selected by the user, or null if the user clicked Cancel.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="comboBoxItems" /> is null.</exception>
        public static string Show(string prompt, string title, string initialValue, IEnumerable<string> comboBoxItems)
        {
            using (InputDialog dialog = new InputDialog(prompt, title, initialValue))
            {
                dialog.InitializeComboBox(comboBoxItems);
                return Show(dialog);
            }
        }

        private static string Show(InputDialog dialog)
        {
            DialogResult result = dialog.ShowDialog();
            return result == DialogResult.OK ? dialog.Value : null;
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            BringToFront();
        }
    }
}
