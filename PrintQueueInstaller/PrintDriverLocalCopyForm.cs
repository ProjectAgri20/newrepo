using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// UI component to copy print drivers from a central location to the local machine.
    /// </summary>
    public partial class PrintDriverLocalCopyForm : Form
    {
        private Collection<string> _driverPaths = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverLocalCopyForm"/> class.
        /// </summary>
        public PrintDriverLocalCopyForm()
        {
            InitializeComponent();
            printDriverAddControl.StatusChanged += new EventHandler<StatusChangedEventArgs>(printDriverAddControl_OnUpdateStatus);
            copyTo_TextBox.Text = Properties.Resources.CopyLocation;
        }

        /// <summary>
        /// Update the status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        void printDriverAddControl_OnUpdateStatus(object sender, StatusChangedEventArgs args)
        {
            statusLabel.Text = args.StatusMessage;
        }

        /// <summary>
        /// Handles the Shown event of the PrintDriverLocalCopyForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PrintDriverLocalCopyForm_Shown(object sender, EventArgs e)
        {
            Update();
            printDriverAddControl.LoadTreeControl();
        }

        /// <summary>
        /// Gets the packages.
        /// </summary>
        public Collection<string> DriverPaths
        {
            get { return _driverPaths; }
        }

        /// <summary>
        /// Gets the download path.
        /// </summary>
        public string DownloadPath
        {
            get { return copyTo_TextBox.Text; }
        }

        /// <summary>
        /// Handles the Click event of the addDrivers_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void addDrivers_Button_Click(object sender, EventArgs e)
        {
            Collection<string> destinationPaths = null;

            Cursor.Current = Cursors.WaitCursor;
            destinationPaths = DownloadDrivers(copyTo_TextBox.Text);
            if (destinationPaths != null)
            {
                foreach (string path in printDriverAddControl.PackagePaths)
                {
                    _driverPaths.Add(path);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void browse_Button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder where the drivers will be saved.";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.ShowNewFolderButton = true;
                copyTo_TextBox.Text = dialog.SelectedPath;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    copyTo_TextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private Collection<string> DownloadDrivers(string filePath)
        {
            var sourcePaths = printDriverAddControl.PackagePaths;
            string sharePath = PrintDriversManager.UniversalPrintDriverShareLocation;

            Collection<PrintDeviceDriverCollection> drivers = new Collection<PrintDeviceDriverCollection>();
            foreach (string path in sourcePaths)
            {
                string sourcePath = Path.Combine(sharePath, path);
                drivers.Add(PrintDriversManager.LoadDrivers(sourcePath, sharePath));
            }

            return PrintDriversManager.CopyDrivers(filePath, drivers);
        }
    }
}
