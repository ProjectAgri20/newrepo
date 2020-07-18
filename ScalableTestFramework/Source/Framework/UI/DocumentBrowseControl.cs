using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Displays a tree view of all available documents and allows the user to select a set of documents.
    /// </summary>
    internal partial class DocumentBrowseControl : UserControl
    {
        #region Document Image Keys

        private static readonly Dictionary<string, string> _documentImageKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"TXT", "Text" },
            {"RTF", "Text" },
            {"PDF", "PDF" },
            {"DOC", "Word" },
            {"DOCX", "Word" },
            {"DOCM", "Word" },
            {"XLS", "Excel" },
            {"XLM", "Excel" },
            {"XLSX", "Excel" },
            {"XLSM", "Excel" },
            {"PPT", "PowerPoint" },
            {"PPTX", "PowerPoint" },
            {"BMP", "Image" },
            {"JPG", "Image" },
            {"JPEG", "Image" },
            {"PNG", "Image" },
        };

        #endregion

        private readonly List<Document> _selectedDocuments = new List<Document>();
        private readonly Dictionary<Guid, RadTreeNode> _docTreeNodes = new Dictionary<Guid, RadTreeNode>();
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBrowseControl" /> class.
        /// </summary>
        public DocumentBrowseControl()
        {
            InitializeComponent();
            documentTreeView.NodeCheckedChanged += documentTreeView_NodeCheckedChanged;
        }

        /// <summary>
        /// Gets the selected documents from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentCollection SelectedDocuments
        {
            get { return new DocumentCollection(_selectedDocuments); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any selected documents.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return _selectedDocuments.Count > 0; }
        }

        /// <summary>
        /// Clears all document selection.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void ClearSelection()
        {
            SetSelectedDocuments(new DocumentIdCollection());
            documentTreeView.CollapseAll();
        }

        /// <summary>
        /// Initializes this control by loading all documents from the document library.
        /// </summary>
        public void Initialize()
        {
            LoadTree(ConfigurationServices.DocumentLibrary.GetDocuments());
        }

        /// <summary>
        /// Initializes this control by loading all documents from the document library
        /// with the specified documents selected.
        /// </summary>
        /// <param name="selectedDocuments">The selected documents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedDocuments" /> is null.</exception>
        public void Initialize(DocumentIdCollection selectedDocuments)
        {
            Initialize();
            SetSelectedDocuments(selectedDocuments);
        }

        /// <summary>
        /// Initializes this control by loading documents with the specified extensions from the document library.
        /// </summary>
        /// <param name="extensions">The extensions of the documents to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public void Initialize(IEnumerable<DocumentExtension> extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            LoadTree(ConfigurationServices.DocumentLibrary.GetDocuments(extensions));
        }

        /// <summary>
        /// Initializes this control by loading documents with the specified extensions from the document library
        /// with the specified documents selected.
        /// </summary>
        /// <param name="selectedDocuments">The selected documents.</param>
        /// <param name="extensions">The extensions of the documents to load.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectedDocuments" /> is null.
        /// <para>or</para>
        /// <paramref name="extensions" /> is null.
        /// </exception>
        public void Initialize(DocumentIdCollection selectedDocuments, IEnumerable<DocumentExtension> extensions)
        {
            Initialize(extensions);
            SetSelectedDocuments(selectedDocuments);
        }

        /// <summary>
        /// Initializes this control by loading documents matching the specified filter from the document library.
        /// </summary>
        /// <param name="filterCriteria">The criteria for the documents to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterCriteria" /> is null.</exception>
        public void Initialize(DocumentQuery filterCriteria)
        {
            if (filterCriteria == null)
            {
                throw new ArgumentNullException(nameof(filterCriteria));
            }

            LoadTree(ConfigurationServices.DocumentLibrary.GetDocuments(filterCriteria));
        }

        /// <summary>
        /// Initializes this control by loading documents matching the specified filter from the document library
        /// with the specified documents selected.
        /// </summary>
        /// <param name="selectedDocuments">The selected documents.</param>
        /// <param name="filterCriteria">The criteria for the documents to load.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectedDocuments" /> is null.
        /// <para>or</para>
        /// <paramref name="filterCriteria" /> is null.
        /// </exception>
        public void Initialize(DocumentIdCollection selectedDocuments, DocumentQuery filterCriteria)
        {
            Initialize(filterCriteria);
            SetSelectedDocuments(selectedDocuments);
        }

        private void LoadTree(DocumentCollection documents)
        {
            documentTreeView.Nodes.Clear();
            _docTreeNodes.Clear();

            var documentGroups = documents.GroupBy(n => n.Group);
            foreach (var documentGroup in documentGroups)
            {
                RadTreeNode folderNode = new RadTreeNode(documentGroup.Key);
                folderNode.ImageKey = "Folder";
                foreach (Document document in documentGroup)
                {
                    RadTreeNode documentNode = new RadTreeNode(document.FileName);
                    documentNode.Tag = document;
                    documentNode.ImageKey = GetDocumentImageKey(document);
                    _docTreeNodes[document.DocumentId] = documentNode;
                    folderNode.Nodes.Add(documentNode);
                }
                documentTreeView.Nodes.Add(folderNode);
            }
        }

        private static string GetDocumentImageKey(Document document)
        {
            string extension = Path.GetExtension(document.FileName).TrimStart('.');
            if (_documentImageKeys.ContainsKey(extension))
            {
                return _documentImageKeys[extension];
            }
            else
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Sets the selected documents.
        /// </summary>
        /// <param name="selectedDocuments">The selected documents.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedDocuments"/> is null.</exception>
        internal void SetSelectedDocuments(DocumentIdCollection selectedDocuments)
        {
            if (selectedDocuments == null)
            {
                throw new ArgumentNullException(nameof(selectedDocuments));
            }

            _suppressSelectionChanged = true;

            // No other way to clear CheckedNodes.
            foreach (RadTreeNode checkedNode in documentTreeView.CheckedNodes.ToList())
            {
                checkedNode.Checked = false;
            }

            foreach (Guid id in selectedDocuments)
            {
                if (_docTreeNodes.TryGetValue(id, out RadTreeNode node))
                {
                    node.Checked = true;
                }
            }

            _suppressSelectionChanged = false;
        }

        private void documentTreeView_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            RadTreeNode node = e.Node;

            if (node.Tag == null)
            {
                return;
            }

            if (node.Checked)
            {
                if (!selectedDocuments_ListControl.Items.Any(x => x.Text.Equals(node.FullPath)))
                {
                    _selectedDocuments.Add((Document)node.Tag);

                    RadListDataItem listItem = null;
                    try
                    {
                        listItem = new RadListDataItem(node.FullPath);
                        listItem.Image = node.Image;
                        selectedDocuments_ListControl.Items.Add(listItem);
                    }
                    catch
                    {
                        listItem?.Dispose();
                        throw;
                    }
                }
            }
            else
            {
                RadListDataItem item = selectedDocuments_ListControl.Items.FirstOrDefault(n => n.Text == node.FullPath);
                if (item != null)
                {
                    _selectedDocuments.Remove((Document)node.Tag);
                    selectedDocuments_ListControl.Items.Remove(item);
                }
            }

            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (!_suppressSelectionChanged)
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
