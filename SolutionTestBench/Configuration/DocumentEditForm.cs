using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest
{
    public partial class DocumentEditForm : Form
    {
        private TestDocument _document = null;
        private DocumentLibraryContext _context = null;
        private ErrorProvider _error = new ErrorProvider();

        public DocumentEditForm(TestDocument document, DocumentLibraryContext context)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            ShowIcon = true;

            _document = document;
            _context = context;

            _error.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        private void DocumentEditForm_Load(object sender, EventArgs e)
        {
            fileTypeComboBox.Focus();

            var documents = _context.TestDocuments;

            foreach (var item in EnumUtil.GetDescriptions<FileType>())
            {
                fileTypeComboBox.Items.Add(item);
            }

            var applications = new List<string>();
            var defaultApps = EnumUtil.GetDescriptions<ApplicationConstant>().ToList();
            applications.AddRange(defaultApps);
            applications.AddRange
                (
                    documents.Select(x => x.Application)
                                .Where(x => x != null && x.Length > 0 && !defaultApps.Any(y => y.Equals(x)))
                                .Distinct()
                );
            applications.Sort();

            foreach (var item in applications)
            {
                applicationComboBox.Items.Add(item);
            }

            foreach (var item in documents.Select(x => x.AppVersion).Where(x => x != null).Distinct())
            {
                appVersionComboBox.Items.Add(item);
            }

            foreach (var item in documents.Select(x => x.Tag).Where(x => x != null).Distinct())
            {
                tag_comboBox.Items.Add(item);
            }

            fileNameTextBox.DataBindings.Add("Text", _document, "FileName", true, DataSourceUpdateMode.OnPropertyChanged);
            fileSizeTextBox.DataBindings.Add("Text", _document, "FileSize", true, DataSourceUpdateMode.OnPropertyChanged);
            pageCountTextBox.DataBindings.Add("Text", _document, "Pages", true, DataSourceUpdateMode.OnPropertyChanged);
            notesTextBox.DataBindings.Add("Text", _document, "Notes", true, DataSourceUpdateMode.OnPropertyChanged);

            if (_document.FileType != null)
            {
                fileTypeComboBox.SelectedItem = _document.FileType;
            }
            
            if (_document.Orientation != null)
            {
                portraitRadioButton.Checked = _document.Orientation.Equals("Portrait");
            }

            if (_document.ColorMode != null)
            {
                colorColorModeRadioButton.Checked = _document.ColorMode.Equals("Color");
            }

            if (applicationComboBox.Items.Cast<string>().Contains(_document.Application))
            {
                applicationComboBox.SelectedItem = _document.Application;
            }

            if (appVersionComboBox.Items.Cast<string>().Contains(_document.AppVersion))
            {
                appVersionComboBox.SelectedItem = _document.AppVersion;
            }

            if (tag_comboBox.Items.Cast<string>().Contains(_document.Tag))
            {
                tag_comboBox.SelectedItem = _document.Tag;
            }

            if (_document.FileSize != 0)
            {
                fileSizeTextBox.ReadOnly = true;
            }
        }

        void pageCountTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int pageCount = 0;
            if (int.TryParse(pageCountTextBox.Text, out pageCount))
            {
                _document.Pages = pageCount;
                _error.SetError(pageCountTextBox, "");
            }
            else
            {
                _error.SetError(pageCountTextBox, "Page count is not a number");
            }
        }

        void fileSizeTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            long fileSize = 0;
            if (long.TryParse(fileSizeTextBox.Text, out fileSize))
            {
                _document.FileSize = fileSize;
                _error.SetError(fileSizeTextBox, "");
            }
            else
            {
                _error.SetError(fileSizeTextBox, "File size is not a number");
            }            
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileNameTextBox.Text.Trim()))
            {
                MessageBox.Show("A unique PrinterId is required", "Printer Id Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                fileNameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(fileTypeComboBox.Text))
            {
                MessageBox.Show("A file type for this document is required", "File Type Missing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                fileTypeComboBox.Focus();
                return;
            }

            _document.FileType = (string)fileTypeComboBox.SelectedItem;
            _document.Application = (string)applicationComboBox.SelectedItem;
            _document.AppVersion = (string)appVersionComboBox.SelectedItem;
            _document.Tag = tag_comboBox.SelectedItem?.ToString() ?? tag_comboBox.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void monoColorModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (monoColorModeRadioButton.Checked)
            {
                _document.ColorMode = "Mono";
            }
        }

        private void colorColorModeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (colorColorModeRadioButton.Checked)
            {
                _document.ColorMode = "Color";
            }
        }

        private void portraitRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (portraitRadioButton.Checked)
            {
                _document.Orientation = "Portrait";
            }
        }

        private void landScapeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (landScapeRadioButton.Checked)
            {
                _document.Orientation = "Landscape";
            }
        }

        private enum ApplicationConstant
        {
            [Description("Microsoft Office Word")]
            Word,

            [Description("Microsoft Office PowerPoint")]
            PowerPoint,

            [Description("Microsoft Excel")]
            Excel,

            [Description("Notepad")]
            Notepad,

            [Description("Adobe Acrobat")]
            Acrobat,
        }

        private enum FileType
        {
            [Description("Graphics")]
            Graphics,

            [Description("Graphics and Images")]
            GraphicsAndImages,

            [Description("Images")]
            Images,

            [Description("Text")]
            Text,

            [Description("Text and Graphics")]
            TextAndGraphics,

            [Description("Text and Graphics and Images")]
            TextAndGraphicsAndImages,

            [Description("Text and Images")]
            TextAndImages,
        }
    }
}
