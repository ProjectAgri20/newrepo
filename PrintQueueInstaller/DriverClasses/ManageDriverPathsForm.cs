using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Form used to manage the driver paths which are saved each time the program is used
    /// </summary>
    public partial class ManageDriverPathsForm : Form
    {
        private QueueManager _manager = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageDriverPathsForm"/> class.
        /// </summary>
        public ManageDriverPathsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageDriverPathsForm"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public ManageDriverPathsForm(QueueManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        private void ManageDriverPathsForm_Load(object sender, EventArgs e)
        {
            if (_manager != null)
            {
                foreach (string item in _manager.DriverPackagePaths.Items)
                {
                    driverPaths_ListBox.Items.Add(item);
                }
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            _manager.DriverPackagePaths.Items.Clear();
            _manager.RefreshCurrentPackage();

            foreach (string item in driverPaths_ListBox.Items)
            {
                _manager.DriverPackagePaths.Add(item);
            }

            Close();
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void driverPaths_ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            driverPaths_ListBox.Items.Remove(driverPaths_ListBox.SelectedItem);
        }
    }
}
