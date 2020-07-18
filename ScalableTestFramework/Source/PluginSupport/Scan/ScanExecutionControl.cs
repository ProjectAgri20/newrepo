using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Base execution control class for scan plugins.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanExecutionControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanExecutionControl" /> class.
        /// </summary>
        public ScanExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            statusRichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="device">The device.</param>
        protected void UpdateDevice(string device)
        {
            activeDeviceLabel.InvokeIfRequired(n => n.Text = device);
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateDevice(object sender, StatusChangedEventArgs e)
        {
            UpdateDevice(e.StatusMessage);
        }
    }
}
