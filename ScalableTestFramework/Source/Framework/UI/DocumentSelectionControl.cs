using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting documents.
    /// </summary>
    public partial class DocumentSelectionControl : UserControl
    {
        private DocumentSelectionMode _selectionMode = DocumentSelectionMode.SpecificDocuments;
        private readonly DocumentSelectionModeAssociations _modeAssociations = new DocumentSelectionModeAssociations();
        private bool _suppressSelectionChanged = false;

        /// <summary>
        /// Gets or sets a value indicating whether to show the document browse control to select documents.
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowDocumentBrowseControl
        {
            get => _modeAssociations.GetEnabled(DocumentSelectionMode.SpecificDocuments);
            set => _modeAssociations.SetEnabled(DocumentSelectionMode.SpecificDocuments, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the document set control for select documents.
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowDocumentSetControl
        {
            get => _modeAssociations.GetEnabled(DocumentSelectionMode.DocumentSet);
            set => _modeAssociations.SetEnabled(DocumentSelectionMode.DocumentSet, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the document query control for selecting documents.
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ShowDocumentQueryControl
        {
            get => _modeAssociations.GetEnabled(DocumentSelectionMode.DocumentQuery);
            set => _modeAssociations.SetEnabled(DocumentSelectionMode.DocumentQuery, value);
        }

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionControl"/> class.
        /// </summary>
        public DocumentSelectionControl()
        {
            InitializeComponent();

            // Create associations between selection modes, controls, and radio buttons
            _modeAssociations.Add(DocumentSelectionMode.SpecificDocuments, documentBrowseControl, documentBrowse_RadioButton);
            _modeAssociations.Add(DocumentSelectionMode.DocumentSet, documentSetControl, documentSet_RadioButton);
            _modeAssociations.Add(DocumentSelectionMode.DocumentQuery, documentQueryControl, documentQuery_RadioButton);

            // Dock all selection controls
            _modeAssociations.ForEach(n => n.SelectionControl.Dock = DockStyle.Fill);

            // Subscribe to selection changed
            documentBrowseControl.SelectionChanged += (s, e) => OnSelectionChanged();
            documentSetControl.SelectionChanged += (s, e) => OnSelectionChanged();
            documentQueryControl.SelectionChanged += (s, e) => OnSelectionChanged();
        }

        /// <summary>
        /// Gets the <see cref="DocumentSelectionData" /> from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentSelectionData DocumentSelectionData
        {
            get
            {
                return new DocumentSelectionData
                (
                    _selectionMode,
                    documentBrowseControl.SelectedDocuments,
                    documentSetControl.SelectedDocumentSet,
                    documentQueryControl.DocumentQuery
                );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a document selection defined.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasSelection
        {
            get
            {
                switch (_selectionMode)
                {
                    case DocumentSelectionMode.SpecificDocuments:
                        return documentBrowseControl.HasSelection;

                    case DocumentSelectionMode.DocumentSet:
                        return documentSetControl.HasSelection;

                    case DocumentSelectionMode.DocumentQuery:
                        return documentQueryControl.HasSelection;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Clears all document selection.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public void ClearSelection()
        {
            documentBrowseControl.ClearSelection();
            documentSetControl.ClearSelection();
            documentQueryControl.ClearSelection();
        }

        /// <summary>
        /// Initializes this control by loading all documents and document sets from the document library.
        /// </summary>
        public void Initialize()
        {
            InitializeSelectionModes();

            documentBrowseControl.Initialize();
            documentSetControl.Initialize();
            documentQueryControl.Initialize();
        }

        /// <summary>
        /// Initializes this control by loading all documents and document sets from the document library
        /// and setting the configuration based on the specified <see cref="DocumentSelectionData" />.
        /// </summary>
        /// <param name="selectionData">The document selection data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectionData" /> is null.</exception>
        public void Initialize(DocumentSelectionData selectionData)
        {
            if (selectionData == null)
            {
                throw new ArgumentNullException(nameof(selectionData));
            }

            InitializeSelectionModes(selectionData.SelectionMode);

            documentBrowseControl.Initialize(selectionData.SelectedDocuments);
            documentSetControl.Initialize(selectionData.DocumentSetName);
            documentQueryControl.Initialize(selectionData.DocumentQuery);
        }

        /// <summary>
        /// Initializes this control by loading documents and document sets with the specified extensions from the document library.
        /// </summary>
        /// <param name="extensions">The extensions of the documents to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public void Initialize(IEnumerable<DocumentExtension> extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            InitializeSelectionModes();

            documentBrowseControl.Initialize(extensions);
            documentSetControl.Initialize(extensions);

            // TODO: Update document query control to work with extension filters
            documentQueryControl.Initialize();
        }

        /// <summary>
        /// Initializes this control by loading documents and document sets with the specified extensions from the document library
        /// and setting the configuration based on the specified <see cref="DocumentSelectionData" />.
        /// </summary>
        /// <param name="selectionData">The document selection data.</param>
        /// <param name="extensions">The extensions of the documents to load.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectionData" /> is null.
        /// <para>or</para>
        /// <paramref name="extensions" /> is null.
        /// </exception>
        public void Initialize(DocumentSelectionData selectionData, IEnumerable<DocumentExtension> extensions)
        {
            if (selectionData == null)
            {
                throw new ArgumentNullException(nameof(selectionData));
            }

            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            InitializeSelectionModes(selectionData.SelectionMode);

            documentBrowseControl.Initialize(selectionData.SelectedDocuments, extensions);
            documentSetControl.Initialize(selectionData.DocumentSetName, extensions);

            // TODO: Update document query control to work with extension filters
            documentQueryControl.Initialize(selectionData.DocumentQuery);
        }

        /// <summary>
        /// Initializes this control by loading all documents and document sets matching the specified filter from the document library.
        /// </summary>
        /// <param name="filterCriteria">The criteria for the documents to load.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filterCriteria" /> is null.</exception>
        public void Initialize(DocumentQuery filterCriteria)
        {
            if (filterCriteria == null)
            {
                throw new ArgumentNullException(nameof(filterCriteria));
            }

            InitializeSelectionModes();

            documentBrowseControl.Initialize(filterCriteria);

            // TODO: Update document set and query controls to work with query filters
            documentSetControl.Initialize();
            documentQueryControl.Initialize();
        }

        /// <summary>
        /// Initializes this control by loading all documents and document sets matching the specified filter from the document library
        /// and setting the configuration based on the specified <see cref="DocumentSelectionData" />.
        /// </summary>
        /// <param name="selectionData">The document selection data.</param>
        /// <param name="filterCriteria">The criteria for the documents to load.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectionData" /> is null.
        /// <para>or</para>
        /// <paramref name="filterCriteria" /> is null.
        /// </exception>
        public void Initialize(DocumentSelectionData selectionData, DocumentQuery filterCriteria)
        {
            if (selectionData == null)
            {
                throw new ArgumentNullException(nameof(selectionData));
            }

            if (filterCriteria == null)
            {
                throw new ArgumentNullException(nameof(filterCriteria));
            }

            InitializeSelectionModes(selectionData.SelectionMode);

            documentBrowseControl.Initialize(selectionData.SelectedDocuments, filterCriteria);

            // TODO: Update document set and query controls to work with query filters
            documentSetControl.Initialize(selectionData.DocumentSetName);
            documentQueryControl.Initialize(selectionData.DocumentQuery);
        }

        private void InitializeSelectionModes()
        {
            // Make sure that at least one mode is available
            int enabledModes = _modeAssociations.Count(n => n.Enabled);
            if (enabledModes == 0)
            {
                throw new InvalidOperationException("There are no UI controls enabled to select documents.");
            }

            // Show the selection panel only if more than 1 method is available
            selectionMode_Panel.Visible = (enabledModes > 1);

            // Set the visibility of all radio buttons
            _modeAssociations.ForEach(n => n.RadioButton.Visible = n.Enabled);

            // Set the status of the copy button depending on the visibility of the browse control
            documentSetControl.ShowCopyLinkLabel = ShowDocumentBrowseControl;

            // Check the first available radio button
            _suppressSelectionChanged = true;
            _modeAssociations.First(n => n.Enabled).RadioButton.Checked = true;
            _suppressSelectionChanged = false;
        }

        private void InitializeSelectionModes(DocumentSelectionMode selectionMode)
        {
            InitializeSelectionModes();

            // If the desired selection mode is available, select it
            if (_modeAssociations[selectionMode].Enabled)
            {
                _suppressSelectionChanged = true;
                _modeAssociations[selectionMode].RadioButton.Checked = true;
                _suppressSelectionChanged = false;
            }
        }

        private void selectionRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _modeAssociations.ForEach(n => n.SelectionControl.Visible = n.RadioButton.Checked);
            _selectionMode = _modeAssociations.First(n => n.RadioButton.Checked).DocumentSelectionMode;
            OnSelectionChanged();
        }

        private void documentSetControl_CopyDocumentsSelected(object sender, EventArgs e)
        {
            DocumentSet selectedSet = documentSetControl.SelectedDocumentSet;
            if (selectedSet != null)
            {
                documentBrowseControl.SetSelectedDocuments(new DocumentIdCollection(selectedSet.Documents));
                documentBrowse_RadioButton.Checked = true;
            }
        }

        private void OnSelectionChanged()
        {
            if (!_suppressSelectionChanged)
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private sealed class DocumentSelectionModeAssociations : List<SelectionModeAssociation>
        {
            public SelectionModeAssociation this[DocumentSelectionMode selectionMode]
            {
                get { return this.First(n => n.DocumentSelectionMode == selectionMode); }
            }

            public bool GetEnabled(DocumentSelectionMode selectionMode)
            {
                return this[selectionMode].Enabled;
            }

            public void SetEnabled(DocumentSelectionMode selectionMode, bool enable)
            {
                this[selectionMode].Enabled = enable;
            }

            public void Add(DocumentSelectionMode documentSelectionMode, Control selectionControl, RadioButton radioButton)
            {
                Add(new SelectionModeAssociation()
                {
                    Enabled = true,
                    DocumentSelectionMode = documentSelectionMode,
                    SelectionControl = selectionControl,
                    RadioButton = radioButton
                });
            }
        }

        private sealed class SelectionModeAssociation
        {
            public bool Enabled { get; set; }
            public DocumentSelectionMode DocumentSelectionMode { get; set; }
            public Control SelectionControl { get; set; }
            public RadioButton RadioButton { get; set; }
        }
    }
}
