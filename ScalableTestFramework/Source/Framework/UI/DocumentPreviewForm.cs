using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Shows a preview of selected documents.
    /// </summary>
    internal partial class DocumentPreviewForm : Form
    {
        private readonly BindingList<Document> _documentList = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPreviewForm" /> class.
        /// </summary>
        private DocumentPreviewForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(documents_GridView, GridViewStyle.ReadOnly);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPreviewForm" /> class.
        /// </summary>
        /// <param name="query">The document query to execute.</param>
        public DocumentPreviewForm(DocumentQuery query)
            : this()
        {
            DocumentCollection documentList = ConfigurationServices.DocumentLibrary.GetDocuments(query);
            _documentList = new BindingList<Document>(documentList.ToList());
            documents_GridView.DataSource = _documentList;
        }
    }
}
