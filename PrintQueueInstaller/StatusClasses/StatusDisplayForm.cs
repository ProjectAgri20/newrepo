using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Displays text in a Multiline TextBox with the ability to save the data.
    /// </summary>
    public partial class StatusDisplayForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusDisplayForm"/> class.
        /// </summary>
        public StatusDisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data
        {
            get { return data_TextBox.Text; }
            set 
            {
                data_TextBox.Text = string.Empty;
                data_TextBox.AppendText(value); 
            } 
        }

        private void save_Button_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AutoUpgradeEnabled = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "txt";
                dialog.Filter = "CSV Files(*.csv)|*.csv|Text Files(*.txt)|*.txt";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.OverwritePrompt = true;
                dialog.Title = "Save";
                dialog.ValidateNames = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                string destination = dialog.FileName;
                if (!Directory.Exists(Path.GetDirectoryName(destination)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destination));
                }

                // If the file is a csv, then convert the text to CSV by replacing the 
                // '|' character with ','
                string data = data_TextBox.Text;

                if (destination.Trim().EndsWith(".csv", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data_TextBox.Text.Replace(" | ", ",");
                }
                File.WriteAllText(destination, data);

                Cursor = Cursors.Default;
            }
        }

        private void close_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void StatusDisplayForm_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            data_TextBox.KeyDown += new KeyEventHandler(data_TextBox_KeyDown);
        }

        void data_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                data_TextBox.SelectAll();
            }
        }
    }
}
