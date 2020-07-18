using System;
using System.Windows.Forms;
using HP.ScalableTest.UI;
using System.IO;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Windows form used to display all available inbox drivers
    /// </summary>
    public partial class InboxDriverSelectionForm : Form
    {
        private PrintDeviceDriver _driver = null;
        private const string _driverCountFormat = "Available In-box Drivers ({0} found)";

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxDriverSelectionForm"/> class.
        /// </summary>
        public InboxDriverSelectionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the print driver.
        /// </summary>
        public PrintDeviceDriver PrintDriver
        {
            get { return _driver; }
        }

        private void InboxDriverSelectionForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            WaitOnLoad();
        }

        private void Ok_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void reload_Button_Click(object sender, EventArgs e)
        {
            DriverStoreScanner.Instance.Reset();
            WaitOnLoad();
        }

        private void WaitOnLoad()
        {
            inboxDriver_DataGridView.DataSource = null;

            if (DriverStoreScanner.Instance.ScanningComplete)
            {
                inboxDriver_DataGridView.DataSource = new SortableBindingList<PrintDeviceDriver>(DriverStoreScanner.Instance.Drivers);
                driversFound_Label.Text =
                    _driverCountFormat.FormatWith(DriverStoreScanner.Instance.Drivers.Count);
            }
            else
            {
                using (DriverStoreScanningForm form = new DriverStoreScanningForm())
                {
                    DialogResult result = form.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        inboxDriver_DataGridView.DataSource = new SortableBindingList<PrintDeviceDriver>(DriverStoreScanner.Instance.Drivers);
                        driversFound_Label.Text =
                            _driverCountFormat.FormatWith(DriverStoreScanner.Instance.Drivers.Count);
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void inboxDriver_DataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (inboxDriver_DataGridView.SelectedRows.Count > 0 && e.RowIndex >= 0)
            {
                _driver = inboxDriver_DataGridView.SelectedRows[0].DataBoundItem as PrintDeviceDriver;
            }
        }

        private void inboxDriver_DataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (inboxDriver_DataGridView.SelectedRows.Count > 0 && inboxDriver_DataGridView.SelectedRows[0].Index > 0)
            {
                _driver = inboxDriver_DataGridView.SelectedRows[0].DataBoundItem as PrintDeviceDriver;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void viewInf_Button_Click(object sender, EventArgs e)
        {
            if (inboxDriver_DataGridView.SelectedRows.Count > 0 && inboxDriver_DataGridView.SelectedRows[0].Index > 0)
            {
                _driver = inboxDriver_DataGridView.SelectedRows[0].DataBoundItem as PrintDeviceDriver;
                using (TextDisplayDialog textBox = new TextDisplayDialog(File.ReadAllText(_driver.InfPath), _driver.InfPath))
                {
                    textBox.ShowDialog();
                }
            }
        }
    }
}
