using System;
using System.Windows.Forms;

namespace SdkTemplateWizard
{
    public partial class TemplateParmsDialog : Form
    {
        public TemplateParmsDialog()
        {
            InitializeComponent();
        }

        private void librarySearchButton_Click( object sender, EventArgs e )
        {
            if (findFileDialog.ShowDialog() == DialogResult.OK)
            {
                frameworkLibraryLocationTextBox.Text = findFileDialog.FileName;
            }
        }
    }
}
