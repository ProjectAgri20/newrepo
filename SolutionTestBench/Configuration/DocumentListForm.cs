using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Core.UI;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Core.ImportExport;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core;

namespace HP.ScalableTest
{
    /// <summary>
    /// List form showing STF Server entries
    /// </summary>
    public partial class DocumentListForm : Form
    {
        private DocumentLibraryContext _context = null;

        private SortableBindingList<TestDocument> _documents = null;
        private Collection<TestDocument> _deletedItems = new Collection<TestDocument>();
        private Dictionary<Guid, string> _addedDocumentPath = new Dictionary<Guid, string>();
        private Dictionary<Guid, string> _importedDocumentData = new Dictionary<Guid, string>();

        private BindingSource _bindingSource = null;
        bool _unsavedChanges = false;
        private string _directory = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentListForm"/> class.
        /// </summary>
        public DocumentListForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            ShowIcon = true;

            UserInterfaceStyler.Configure(radGridViewDocuments, GridViewStyle.ReadOnly);

            _context = DbConnect.DocumentLibraryContext();
            _documents = new SortableBindingList<TestDocument>();
        }

        private void DocumentListForm_Load(object sender, System.EventArgs e)
        {
            using (new BusyCursor())
            {
                foreach (var item in _context.TestDocuments.Include(n => n.TestDocumentExtension))
                {
                    _documents.Add(item);
                }

                _bindingSource = new BindingSource();
                _bindingSource.DataSource = _documents;
                radGridViewDocuments.DataSource = _bindingSource;

                radGridViewDocuments.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                radGridViewDocuments.BestFitColumns();
            }
        }

        private GridViewRowInfo GetFirstSelectedRow()
        {
            return radGridViewDocuments.SelectedRows.FirstOrDefault();
        }

        private void ok_Button_Click(object sender, System.EventArgs e)
        {
            Commit();
            Close();
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        private void Commit()
        {
            using (new BusyCursor())
            {
                try
                {
                    if (_deletedItems.Count > 0)
                    {
                        foreach (var item in _deletedItems)
                        {
                            _context.TestDocuments.Remove(item);
                        }
                        _context.SaveChanges();
                        _deletedItems.Clear();
                    }

                    foreach (var document in _documents)
                    {
                        if (_context.Entry(document).State == EntityState.Added)
                        {
                            document.SubmitDate = DateTime.Now;
                            _context.TestDocuments.Add(document);

                            if (_addedDocumentPath.ContainsKey(document.TestDocumentId))
                            {
                                var source = _addedDocumentPath[document.TestDocumentId];

                                try
                                {
                                    ImportExportUtil.CopyDocumentToServer(document, GlobalSettings.Items[Setting.DocumentLibraryServer], source);
                                    TraceFactory.Logger.Debug("File copied");
                                    _addedDocumentPath.Remove(document.TestDocumentId);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    MessageBox.Show
                                    (
                                        "You do not have authorization to save {0} to the STB Document Server. This document will not be added.".FormatWith(source),
                                        "Unauthorized Access",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show
                                    (
                                        "A error occurred when trying to save {0} to the STB Document Server. This document will not be added. Error: {1}".FormatWith(source, ex.Message),
                                        "Unauthorized Access",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                }
                            }
                            else if (_importedDocumentData.ContainsKey(document.TestDocumentId))
                            {
                                var data = _importedDocumentData[document.TestDocumentId];
                                ImportExportUtil.WriteDocumentToServer(document, GlobalSettings.Items[Setting.DocumentLibraryServer], data);
                                TraceFactory.Logger.Debug("File imported");
                                _importedDocumentData.Remove(document.TestDocumentId);
                            }
                        }
                    }
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                    MessageBox.Show
                    (
                        "A error occurred when trying to save to the STB Document Server. This document will not be added. Error: {0}".FormatWith(ex.Message),
                        "Unauthorized Access",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private DialogResult EditEntry(TestDocument document)
        {
            DialogResult result = DialogResult.None;
            using (DocumentEditForm form = new DocumentEditForm(document, _context))
            {
                result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _unsavedChanges = true;
                }
            }

            return result;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges)
            {
                var result = MessageBox.Show("You have unsaved changes that will be lost.  Do you want to continue?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                    return;
                }
            }
            else
            {
                Close();
                return;
            }
        }

        private void edit_Button_Click(object sender, EventArgs e)
        {
            var row = GetFirstSelectedRow();
            if (row != null)
            {
                var document = row.DataBoundItem as TestDocument;

                if (EditEntry(document) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    radGridViewDocuments.Refresh();
                }
            }
        }

        private void apply_Button_Click(object sender, EventArgs e)
        {
            Commit();
            _unsavedChanges = false;
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            var permission = new FileIOPermission(FileIOPermissionAccess.AllAccess, GlobalSettings.Items[Setting.DocumentLibraryServer]);
            try
            {
                permission.Demand();
            }
            catch (SecurityException)
            {
                MessageBox.Show("You do not have authorization to save documents to the STB Document Server.  Please contact your administrator.", "Unauthorized Access", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string fileName = string.Empty;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DereferenceLinks = false;
                dialog.AutoUpgradeEnabled = false;
                dialog.Multiselect = false;
                dialog.AddExtension = true;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = false;
                dialog.DefaultExt = "stb";
                dialog.Filter = "Word Documents|*.doc;*.docx|Excel Worksheets|*.xls;*.xlsx;*.xslm|PowerPoint Presentations|*.ppt;*.pptx|Office Files|*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx|Adobe PDF Files|*.pdf|Text Documents|*.txt|Images|*.jpg;*.jpeg;*.png;*.bmp;*.tif|All Files|*.*";
                dialog.Title = "Add Test Document to Repository";
                dialog.ShowHelp = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = dialog.FileName;

                    TestDocumentExtension extension = null;
                    string extValue = Path.GetExtension(fileName).ToUpper().Replace(".", "");
                    extension = _context.TestDocumentExtensions.FirstOrDefault(x => x.Extension.Equals(extValue));
                    if (extension == null)
                    {
                        MessageBox.Show("The selected file is not a supported format", "Unsupported File Format", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    TestDocument document = new TestDocument()
                    {
                        TestDocumentExtension = extension,
                        Submitter = Environment.UserName,
                        Vertical = null,
                        FileName = Path.GetFileName(fileName),
                        TestDocumentId = SequentialGuid.NewGuid(),
                        Orientation = "Portrait",
                        ColorMode = "Mono"
                    };

                    GetFileStatistics(document, fileName);

                    if (EditEntry(document) == DialogResult.OK)
                    {
                        _addedDocumentPath.Add(document.TestDocumentId, fileName);
                        AddDocument(document);
                    }
                }
            }
        }

        private void GetFileStatistics(TestDocument doc, string filePath)
        {
            FileAnalyzer analyzer = FileAnalyzerFactory.Create(filePath);
            DocumentProperties stats = analyzer.GetProperties();

            doc.Pages = stats.Pages;
            doc.Author = stats.Author;
            doc.Application = stats.Application;
            doc.FileSize = stats.FileSize;
            doc.Orientation = (stats.Orientation ?? Framework.Documents.Orientation.Portrait).ToString();
        }

        private void AddDocument(TestDocument document)
        {
            if (!_context.TestDocuments.Any(x => x.FileName.Equals(document.FileName, StringComparison.OrdinalIgnoreCase)))
            {
                _context.TestDocuments.Add(document);
                _documents.Add(document);

                int index = radGridViewDocuments.Rows.Count - 1;

                radGridViewDocuments.Rows[index].IsSelected = true;

                //In case if you want to scroll down as well.
                radGridViewDocuments.TableElement.ScrollToRow(index);
                _unsavedChanges = true;
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            var firstSelectedRow = GetFirstSelectedRow();
            if (firstSelectedRow != null)
            {
                string msg = "Because the selected document may already be used in test scenarios, it";
                if (radGridViewDocuments.SelectedRows.Count > 1)
                {
                    msg = "Because the selected documents may already be used in test scenarios, they";
                }

                var dialogResult = MessageBox.Show
                    (
                        "{0} will not be removed from the STB Document Server. Only the data entry will be removed. Do you want to continue?".FormatWith(msg),
                        "Remove Documents",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question

                    );

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

                string fileName = string.Empty;
                try
                {
                    var documents = radGridViewDocuments
                                    .SelectedRows.Cast<GridViewRowInfo>()
                                    .Select(x => x.DataBoundItem).Cast<TestDocument>()
                                    .ToList();

                    foreach (var document in documents)
                    {
                        fileName = document.FileName;

                        _unsavedChanges = true;
                        _deletedItems.Add(document);
                        _documents.Remove(document);
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                    MessageBox.Show("Error removing entry for {0}".FormatWith(fileName), "Entry Removal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportSaveFileDialog(_directory, "Export Test Document Data", ImportExportType.Document))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (new BusyCursor())
                    {
                        try
                        {
                            string exportFile = dialog.Base.FileName;
                            _directory = Path.GetDirectoryName(exportFile);

                            DocumentContractCollection documentContracts = new DocumentContractCollection();

                            var documents = radGridViewDocuments
                                                .SelectedRows.Cast<DataGridViewRow>()
                                                .Select(x => x.DataBoundItem).Cast<TestDocument>();

                            documentContracts.Load(documents, true);
                            documentContracts.Export(exportFile);
                            MessageBox.Show("Data successfully exported", "STB Data Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void importToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new ExportOpenFileDialog(_directory, "Open Test Document Export File", ImportExportType.Document))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var file = dialog.Base.FileName;
                        _directory = Path.GetDirectoryName(file);

                        var contracts = LegacySerializer.DeserializeDataContract<DocumentContractCollection>(File.ReadAllText(file));

                        foreach (var contract in contracts)
                        {
                            if (!_context.TestDocuments.Any(x => x.FileName.Equals(contract.FileName, StringComparison.OrdinalIgnoreCase)))
                            {
                                var document = ContractFactory.Create(_context, contract);
                                _importedDocumentData.Add(document.TestDocumentId, contract.Data);
                                AddDocument(document);
                            }
                            else
                            {
                                // Log an error for the current file, but keep going
                                TraceFactory.Logger.Debug("Document already exists: {0}".FormatWith(contract.FileName));
                            }
                        }

                        MessageBox.Show("Documents have been imported", "Import Documents", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                MessageBox.Show("Error importing document: {0}. Check log file for more details.".FormatWith(ex.Message));
            }
        }

        private void printer_ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var enable = radGridViewDocuments.SelectedRows.Count <= 1;
            editToolStripMenuItem.Enabled = enable;
        }

        private void radGridViewDocuments_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var row = e.Row;
            if (row.IsCurrent && row.IsSelected)
            {
                var item = row.DataBoundItem as TestDocument;
                if (EditEntry(item) == DialogResult.OK)
                {
                    _unsavedChanges = true;
                    radGridViewDocuments.Refresh();
                }
            }
        }
    }
}
