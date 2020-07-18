using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// A simple dialog that displays XML data with options to copy or save.
    /// </summary>
    public partial class XmlDisplayDialog : Form
    {
        private readonly string _xmlData;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDisplayDialog" /> class.
        /// </summary>
        /// <param name="xmlData">The XML data to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlData" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public XmlDisplayDialog(XDocument xmlData)
        {
            if (xmlData == null)
            {
                throw new ArgumentNullException(nameof(xmlData));
            }

            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);

            _xmlData = xmlData.ToString();

            string tempFile = Path.GetTempFileName();
            string tempXmlFile = Path.ChangeExtension(tempFile, "xml");
            File.Move(tempFile, tempXmlFile);
            File.WriteAllText(tempXmlFile, _xmlData);

            xmlWebBrowser.DocumentCompleted += (s, e) =>
            {
                try
                {
                    File.Delete(tempXmlFile);
                }
                catch
                {
                    // Ignore errors.
                }
            };

            xmlWebBrowser.Navigate(tempXmlFile);

        }

        private void save_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, _xmlData);
            }
        }

        private void copy_ToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_xmlData);
        }
    }
}
