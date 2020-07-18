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
    /// Form used to create a virtual queue
    /// </summary>
    public partial class VirtualQueueNameForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualQueueNameForm"/> class.
        /// </summary>
        public VirtualQueueNameForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the data control.
        /// </summary>
        public VirtualQueueNameUserControl DataControl
        {
            get { return virtualQueueDataControl; }
        }

        private void VirtualQueueDataForm_Load(object sender, EventArgs e)
        {
            virtualQueueDataControl.SetFocus();
        }

    }
}
