using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Data used for selecting documents for use in a test.
    /// </summary>
    [DataContract]
    public sealed class DocumentSelectionData
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DocumentSelectionMode _selectionMode;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DocumentIdCollection _selectedDocuments;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _documentSetName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DocumentQuery _documentQuery;

        /// <summary>
        /// Gets the selected <see cref="DocumentSelectionMode" />.
        /// </summary>
        public DocumentSelectionMode SelectionMode => _selectionMode;

        /// <summary>
        /// Gets the selected documents.
        /// </summary>
        public DocumentIdCollection SelectedDocuments => _selectedDocuments;

        /// <summary>
        /// Gets the name of the document set.
        /// </summary>
        public string DocumentSetName => _documentSetName;

        /// <summary>
        /// Gets the query criteria to use to select documents.
        /// </summary>
        public DocumentQuery DocumentQuery => _documentQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionData" /> class.
        /// </summary>
        public DocumentSelectionData()
            : this(DocumentSelectionMode.SpecificDocuments, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionData" /> class
        /// with <see cref="SelectionMode" /> set to <see cref="DocumentSelectionMode.SpecificDocuments" />.
        /// </summary>
        /// <param name="selectedDocuments">The collection of <see cref="Document" /> selections.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedDocuments" /> is null.</exception>
        public DocumentSelectionData(IEnumerable<Document> selectedDocuments)
            : this(DocumentSelectionMode.SpecificDocuments, selectedDocuments, null, null)
        {
            if (selectedDocuments == null)
            {
                throw new ArgumentNullException(nameof(selectedDocuments));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionData" /> class
        /// with <see cref="SelectionMode" /> set to <see cref="DocumentSelectionMode.DocumentSet" />.
        /// </summary>
        /// <param name="documentSet">The selected <see cref="DocumentSet" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentSet" /> is null.</exception>
        public DocumentSelectionData(DocumentSet documentSet)
            : this(DocumentSelectionMode.DocumentSet, null, documentSet, null)
        {
            if (documentSet == null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionData" /> class
        /// with <see cref="SelectionMode" /> set to <see cref="DocumentSelectionMode.DocumentQuery" />.
        /// </summary>
        /// <param name="documentQuery">The specified <see cref="DocumentQuery" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentQuery" /> is null.</exception>
        public DocumentSelectionData(DocumentQuery documentQuery)
            : this(DocumentSelectionMode.DocumentQuery, null, null, documentQuery)
        {
            if (documentQuery == null)
            {
                throw new ArgumentNullException(nameof(documentQuery));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSelectionData"/> class
        /// with the specified <see cref="DocumentSelectionMode" /> and components.
        /// </summary>
        /// <param name="mode">The <see cref="DocumentSelectionMode" /> to use.</param>
        /// <param name="selectedDocuments">The collection of <see cref="Document" /> selections, or null.</param>
        /// <param name="documentSet">The selected <see cref="DocumentSet" />, or null.</param>
        /// <param name="documentQuery">The specified <see cref="DocumentQuery" />.</param>
        public DocumentSelectionData(DocumentSelectionMode mode, IEnumerable<Document> selectedDocuments, DocumentSet documentSet, DocumentQuery documentQuery)
        {
            _selectionMode = mode;

            if (selectedDocuments != null)
            {
                _selectedDocuments = new DocumentIdCollection(selectedDocuments);
            }
            else
            {
                _selectedDocuments = new DocumentIdCollection();
            }

            if (documentSet != null)
            {
                _documentSetName = documentSet.Name;
            }

            if (documentQuery != null)
            {
                _documentQuery = documentQuery;
            }
            else
            {
                _documentQuery = new DocumentQuery();
            }
        }
    }
}
