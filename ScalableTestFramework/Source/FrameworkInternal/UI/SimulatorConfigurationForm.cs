using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Displays device simulator scan configuration options.
    /// </summary>
    public partial class SimulatorConfigurationForm : Form
    {
        /// <summary>
        /// Gets or sets the automation pause to use for simulators.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AutomationPause
        {
            get { return TimeSpan.FromMilliseconds((int)pause_NumericUpDown.Value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the simulated ADF should be used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseAdf
        {
            get { return scanAdf_RadioButton.Checked; }
        }

        /// <summary>
        /// Gets or sets the set of documents to load into the ADF.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentSelectionData AdfDocuments
        {
            get { return documentSelectionControl.DocumentSelectionData; }
        }

        private SimulatorConfigurationForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            documentSelectionControl.ShowDocumentQueryControl = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorConfigurationForm" /> class.
        /// </summary>
        /// <param name="automationPause">The automation pause to display.</param>
        /// <param name="useAdf">if set to <c>true</c> select the ADF document option.</param>
        /// <param name="adfDocuments">The ADF documents to select.</param>
        public SimulatorConfigurationForm(TimeSpan automationPause, bool useAdf, DocumentSelectionData adfDocuments)
            : this()
        {
            pause_NumericUpDown.Value = (int)automationPause.TotalMilliseconds;
            scanAdf_RadioButton.Checked = useAdf;

            var allExtensions = ConfigurationServices.DocumentLibrary.GetExtensions();
            var jpegs = allExtensions.Where(n => n.FileType.Equals("jpeg", StringComparison.OrdinalIgnoreCase));
            if (adfDocuments != null)
            {
                documentSelectionControl.Initialize(adfDocuments, jpegs);
            }
            else
            {
                documentSelectionControl.Initialize(jpegs);
            }
        }

        private void scanAdf_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            documentSelection_GroupBox.Enabled = scanAdf_RadioButton.Checked;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            ValidationResult result = FieldValidator.HasDocumentSelection(documentSelectionControl, scanAdf_RadioButton);
            if (result.Succeeded)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(result.Message, "Document Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
