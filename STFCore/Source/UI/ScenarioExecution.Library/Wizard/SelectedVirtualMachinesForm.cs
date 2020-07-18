using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// Displays a selection of virtual machines.
    /// </summary>
    internal partial class SelectedVirtualMachinesForm : Form
    {
        private IEnumerable<VirtualMachine> _machines = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedVirtualMachinesForm"/> class.
        /// </summary>
        public SelectedVirtualMachinesForm(IEnumerable<VirtualMachine> machines)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(virtualMachine_GridView, GridViewStyle.ReadOnly);

            _machines = machines;
        }

        private void SelectedVirtualMachinesForm_Load(object sender, EventArgs e)
        {
            virtualMachine_GridView.DataSource = _machines;
            virtualMachine_GridView.BestFitColumns();
        }
    }
}
