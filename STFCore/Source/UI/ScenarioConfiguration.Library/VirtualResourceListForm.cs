using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Form for listing virtual resources and allowing selection from the list.
    /// </summary>
    public partial class VirtualResourceListForm : Form
    {
        /// <summary>
        /// Gets the selected resources.
        /// </summary>
        public IEnumerable<VirtualResource> SelectedResources
        {
            get { return resource_GridView.SelectedRows.Select(n => n.DataBoundItem).Cast<VirtualResource>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceListForm"/> class.
        /// </summary>
        public VirtualResourceListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(resource_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes this instance with the specified list of <see cref="VirtualResource"/>s.
        /// </summary>
        /// <param name="resources">The resources.</param>
        public void Initialize(IEnumerable<VirtualResource> resources)
        {
            resource_GridView.DataSource = resources;
            resource_GridView.BestFitColumns();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (resource_GridView.SelectedRows.Any())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please select a virtual resource.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
