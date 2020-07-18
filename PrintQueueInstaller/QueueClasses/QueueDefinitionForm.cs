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
    /// Form used to display all queue definitions
    /// </summary>
    public partial class QueueDefinitionForm : Form
    {
        QueueManager _manager = null;
        QueueDefinitionUserControl _queueControl = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueDefinitionForm"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public QueueDefinitionForm(QueueManager manager)
        {
            InitializeComponent();
            _manager = manager;
        }

        /// <summary>
        /// Gets the installation timeout.
        /// </summary>
        public TimeSpan InstallationTimeout
        {
            get { return _queueControl.InstallationTimeout; }
        }

        private void QueueDefinitionForm_Load(object sender, EventArgs e)
        {
            _queueControl = new QueueDefinitionUserControl(_manager);
            main_Panel.Controls.Add(_queueControl);
            _queueControl.Dock = DockStyle.Fill;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (_queueControl.ValidateChildren())
            {
                try
                {
                    // Tell the dialog to build the correct number of queues
                    // based on the selected information
                    _queueControl.CreateDefinitions();

                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                catch (MissingFieldException ex)
                {
                    MessageBox.Show(ex.Message, "Create Queue Definitions", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
