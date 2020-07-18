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
    public partial class ThreadedInstallForm : Form
    {
        private int _threadCount = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadedInstallForm"/> class.
        /// </summary>
        /// <param name="threadCount">The thread count.</param>
        public ThreadedInstallForm(int threadCount)
        {
            _threadCount = threadCount;
            InitializeComponent();
        }

        /// <summary>
        /// Gets the thread count.
        /// </summary>
        public int ThreadCount
        {
            get { return (int)threadCount_NumericUpDown.Value; }
        }

        private void ThreadedInstallForm_Load(object sender, EventArgs e)
        {
            threadCount_NumericUpDown.Value = _threadCount;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
