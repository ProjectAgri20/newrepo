using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Framework.Documents;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class TestDocumentResolverForm : Form
    {
        private TestDocumentContract _contract;

        public TestDocumentResolverForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes this control by loading documents in the specified locations
        /// </summary>
        /// <param name="contract">The test document contract.</param>
        /// <exception cref="System.ArgumentNullException">selectedDocuments</exception>
        public void Initialize(TestDocumentContract contract)
        {
            _contract = contract;
            var selectedDocument = contract.Replacement;

            if (allDocuments_TreeView.Nodes.Count == 0)
            {
                DocumentLibraryController documentLibrary = new DocumentLibraryController(DbConnect.DocumentLibraryConnectionString);
                DocumentCollection documents = documentLibrary.GetDocuments();

                // First, add the top level nodes from the extensions table                
                foreach (string location in documents.Select(n => n.Group).Distinct())
                {
                    RadTreeNode locationNode = allDocuments_TreeView.Nodes.Add(location, "FOLDER");

                    // Now add the document nodes for this location
                    var files = documents.Where(n => n.Group == location).Select(n => n.FileName);
                    foreach (string fileName in files)
                    {
                        locationNode.Nodes.Add(fileName, Path.GetExtension(fileName).ToUpperInvariant());
                    }
                }
            }

            if (!string.IsNullOrEmpty(selectedDocument))
            {
                var node = allDocuments_TreeView.Nodes
                    .SelectMany(x => x.Nodes)
                    .FirstOrDefault(x => x.Tag != null && x.Text.Equals(selectedDocument));

                if (node != null)
                {
                    allDocuments_TreeView.SelectedNode = node;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_contract.Replacement))
            {
                MessageBox.Show("You must select a replacement document", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PrintQueueUsageResolverForm_Load(object sender, EventArgs e)
        {
            oldNameValueLabel.Text = _contract.Original;
        }

        private void allDocuments_TreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            _contract.Replacement = e.Node.FullPath;
        }
    }
}
