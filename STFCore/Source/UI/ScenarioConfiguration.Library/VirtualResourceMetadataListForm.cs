using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Form for listing virtual resource metadata and allowing selection from the list.
    /// </summary>
    public partial class VirtualResourceMetadataListForm : Form
    {
        /// <summary>
        /// Gets the selected metadata.
        /// </summary>
        public IEnumerable<VirtualResourceMetadata> SelectedMetadata
        {
            get { return metadata_GridView.SelectedRows.Select(n => n.DataBoundItem).Cast<VirtualResourceMetadata>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceMetadataListForm"/> class.
        /// </summary>
        public VirtualResourceMetadataListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(metadata_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes this instance with the specified list of <see cref="VirtualResourceMetadata"/> items.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void Initialize(IEnumerable<VirtualResourceMetadata> metadata)
        {
            metadata_GridView.DataSource = metadata;
            metadata_GridView.BestFitColumns();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (metadata_GridView.SelectedRows.Any())
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please select a metadata item.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
