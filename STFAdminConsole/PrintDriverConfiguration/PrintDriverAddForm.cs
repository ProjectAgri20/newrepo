using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// A form for adding new print drivers.
    /// </summary>
    public partial class PrintDriverAddForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverAddForm"/> class.
        /// </summary>
        public PrintDriverAddForm()
        {
            InitializeComponent();
            downloadControl.StatusChanged += new EventHandler<StatusChangedEventArgs>(downloadControl_OnUpdateStatus);
        }

        public IEnumerable<string> SelectedDestinationPaths
        {
            get { return downloadControl.PackagePaths; }
        }

        /// <summary>
        /// Update the status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        private void downloadControl_OnUpdateStatus(object sender, StatusChangedEventArgs args)
        {
            statusLabel.Text = args.StatusMessage;
        }

        /// <summary>
        /// Handles the Shown event of the PrintDriverConfigForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintDriverConfigForm_Shown(object sender, EventArgs e)
        {
            downloadControl.LoadTreeControl();
        }


        /// <summary>
        /// Handles the Click event of the addDrivers_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void addDrivers_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //downloadControl.Download();
        }
    }
}
