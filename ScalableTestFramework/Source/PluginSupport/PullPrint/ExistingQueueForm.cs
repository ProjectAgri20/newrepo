using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    internal partial class ExistingQueueForm : Form
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
            localQueue_comboBox.Text = printQueue.QueueName;
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
            if (string.IsNullOrEmpty(localQueue_comboBox.Text))
            {
                MessageBox.Show("A queue name must be specified.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(assetId_TextBox.Text))
                {
                    PrintQueue = new LocalPrintQueueInfo(localQueue_comboBox.Text);
                }
                else
                {
                    PrintQueue = new LocalPrintQueueInfo(localQueue_comboBox.Text, assetId_TextBox.Text);
                }
                DialogResult = DialogResult.OK;
            }
        }
        
        private void localQueue_comboBox_DropDown(object sender, EventArgs e)
        {
            try
            {
                if(PrinterSettings.InstalledPrinters.Count > 1)
                {
                    List<string> pkInstalledPrinters = new List<string>();

                    foreach (string prt in PrinterSettings.InstalledPrinters)
                    {
                        pkInstalledPrinters.Add(prt);
                    }

                    localQueue_comboBox.DataSource = pkInstalledPrinters;
                }            
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot get the local print list. Please type the print queue name manually.");
            }
        }
    }
}
