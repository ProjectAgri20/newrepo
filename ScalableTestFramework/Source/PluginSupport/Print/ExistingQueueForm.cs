using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.Print
{
    public partial class ExistingQueueForm : Form
    {
        public LocalPrintQueueInfo PrintQueue { get; private set; }

        public ExistingQueueForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
        }

        public ExistingQueueForm(LocalPrintQueueInfo printQueue)
            : this()
        {
            queueName_TextBox.Text = printQueue.QueueName;
            assetId_TextBox.Text = printQueue.AssociatedAssetId;
        }

        private void selectPrinters_Button_Click(object sender, EventArgs e)
        {
            using (AssetSelectionForm form = new AssetSelectionForm(AssetAttributes.Printer, false))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    assetId_TextBox.Text = form.SelectedAssets.First().AssetId;
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(queueName_TextBox.Text))
            {
                MessageBox.Show("A queue name must be specified.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(assetId_TextBox.Text))
                {
                    PrintQueue = new LocalPrintQueueInfo(queueName_TextBox.Text);
                }
                else
                {
                    PrintQueue = new LocalPrintQueueInfo(queueName_TextBox.Text, assetId_TextBox.Text);
                }
                DialogResult = DialogResult.OK;
            }
        }
    }
}
