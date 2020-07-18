using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This form is used to show the progress of scanning the DriverStore for
    /// in-box drivers
    /// </summary>
    public partial class DriverStoreScanningForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverStoreScanningForm"/> class.
        /// </summary>
        public DriverStoreScanningForm()
        {
            InitializeComponent();
        }

        private void Instance_OnAllDriversLoaded(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => Instance_OnAllDriversLoaded(sender, e)));
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Thread.Sleep(500);
                Close();
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DriverStoreScanner.Instance.Stop();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void DriverStoreScanningForm_Load(object sender, EventArgs e)
        {
            DriverStoreScanner.Instance.OnAllDriversLoaded += Instance_OnAllDriversLoaded;
            if (DriverStoreScanner.Instance.ScanningComplete)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }

            DriverStoreScanner.Instance.OnInfFileScanned += Instance_OnInfFileScanned;

            DriverStoreScanner.Instance.Start();
        }

        private void Instance_OnInfFileScanned(object sender, DriverStoreScanningEventArgs e)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(() => Instance_OnInfFileScanned(sender, e)));
                }
                catch (ObjectDisposedException ex)
                {
                    // Log that this error occurred but continue on.
                    TraceFactory.Logger.Error(ex.Message);
                }
            }
            else
            {
                if (e.Total > 0)
                {
                    loading_ProgressBar.Value = (int)Math.Floor(((double)e.Complete / (double)e.Total) * 100.0);
                    loading_ProgressBar.Refresh();
                    driver_Label.Text = e.Driver;
                }
            }
        }
    }
}
