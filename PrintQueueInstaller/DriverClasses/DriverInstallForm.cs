using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DriverInstallForm : Form
    {
        private DriverInstallUserControl _control = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverInstallForm"/> class.
        /// </summary>
        public DriverInstallForm(QueueManager manager)
        {
            _control = new DriverInstallUserControl(manager);

            InitializeComponent();

            _control.Dock = DockStyle.Fill;
            editControl_Panel.Controls.Add(_control);
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;

            if (_control.ValidatePackageLoaded())
            {
                Close();
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
