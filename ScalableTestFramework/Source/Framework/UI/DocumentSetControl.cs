using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Displays a list of document sets and allows a user to select one.
    /// </summary>
    internal partial class DocumentSetControl : UserControl
    {
        private readonly List<DocumentSet> _documentSets = new List<DocumentSet>();
        private DocumentSet _selectedSet = null;
        private bool _showCopyLinkLabel = false;
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Gets or sets a value indicating whether to enable the Copy Documents link label.
        /// </summary>
        [Browsable(true), Category("Behavior"), Description("Indicates whether the Copy Documents link label is visible.")]
        public bool ShowCopyLinkLabel
        {
            get
            {
                return _showCopyLinkLabel;
            }
            set
            {
                _showCopyLinkLabel = value;
                if (_showCopyLinkLabel == false)
                {
                    copy_LinkLabel.Visible = false;
                }
            }
        }

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the user clicks the Copy Documents link label.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the Copy Documents link label is clicked.")]
        public event EventHandler CopyDocumentsSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSetControl" /> class.
        /// </summary>
        public DocumentSetControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the <see cref="SelectedDocumentSet" /> selected in this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentSet SelectedDocumentSet
        {
            get { return _selectedSet; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a selected document set.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get { return _selectedSet != null; }
        }

        /// <summary>
        /// Clears all document selection.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void ClearSelection()
        {
            LoadDocumentSets(new List<DocumentSet>());
        }

        /// <summary>
        /// Initializes this control by loading all document sets from the document library.
        /// </summary>
        public void Initialize()
        {
            LoadDocumentSets(ConfigurationServices.DocumentLibrary.GetDocumentSets());
        }

        /// <summary>
        /// Initializes this control by loading all document sets from the document library
        /// and selecting the specified set.
        /// </summary>
        /// <param name="selectedDocumentSet">The name of the selected document set.</param>
        public void Initialize(string selectedDocumentSet)
        {
            Initialize();
            SetSelectedSet(selectedDocumentSet);
        }

        /// <summary>
        /// Initializes the this control by loading document sets with the specified extensions from the document library.
        /// </summary>
        /// <param name="extensions">The extensions of the document sets to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public void Initialize(IEnumerable<DocumentExtension> extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            LoadDocumentSets(ConfigurationServices.DocumentLibrary.GetDocumentSets(extensions));
        }

        /// <summary>
        /// Initializes the this control by loading document sets with the specified extensions from the document library
        /// and selecting the specified set.
        /// </summary>
        /// <param name="selectedDocumentSet">The name of the selected document set.</param>
        /// <param name="extensions">The extensions of the document sets to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public void Initialize(string selectedDocumentSet, IEnumerable<DocumentExtension> extensions)
        {
            Initialize(extensions);
            SetSelectedSet(selectedDocumentSet);
        }

        private void LoadDocumentSets(IEnumerable<DocumentSet> documentSets)
        {
            _suppressSelectionChanged = true;

            _documentSets.Clear();
            _documentSets.AddRange(documentSets);
            documentSets_ListBox.DataSource = _documentSets;

            _suppressSelectionChanged = false;
        }

        private void SetSelectedSet(string documentSetName)
        {
            _suppressSelectionChanged = true;
            documentSets_ListBox.SelectedItem = _documentSets.FirstOrDefault(x => x.Name.Equals(documentSetName));
            _suppressSelectionChanged = false;
        }

        private void documentSets_ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setContents_ListBox.Items.Clear();
            copy_LinkLabel.Visible = false;

            _selectedSet = (DocumentSet)documentSets_ListBox.SelectedItem;
            if (_selectedSet != null)
            {
                foreach (Document document in _selectedSet.Documents)
                {
                    setContents_ListBox.Items.Add(document);
                }
                copy_LinkLabel.Visible = _showCopyLinkLabel;
            }

            OnSelectionChanged();
        }

        private void copy_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CopyDocumentsSelected?.Invoke(this, new EventArgs());
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
