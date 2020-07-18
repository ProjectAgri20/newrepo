using System;
using System.IO;
using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// A simple dialog that displays text data with options to copy or save.
    /// </summary>
    public partial class TextDisplayDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextDisplayDialog" /> class.
        /// </summary>
        /// <param name="data">The data to display in the dialog.</param>
        public TextDisplayDialog(string data)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);

            display_TextBox.Text = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDisplayDialog" /> class.
        /// </summary>
        /// <param name="data">The data to display in the dialog.</param>
        /// <param name="title">The text to display in the title bar of the dialog.</param>
        public TextDisplayDialog(string data, string title)
            : this(data)
        {
            Text = title;
        }

        private void save_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, display_TextBox.Text);
            }
        }

        private void copy_ToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(display_TextBox.Text);
        }
    }
}
