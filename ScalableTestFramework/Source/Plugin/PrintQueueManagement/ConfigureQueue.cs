using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    /// <summary>
    /// Configure print queue preferences
    /// </summary>
    public partial class ConfigureQueueForm : Form
    {
        /// <summary>
        /// the print preference holder class
        /// </summary>
        public PrintQueuePreferences PrintPreference { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigureQueueForm()
        {
            InitializeComponent();
            PrintPreference = new PrintQueuePreferences();
        }

        private void ConfigureQueue_Load(object sender, EventArgs e)
        {
            string[] paperSizes = { "Letter", "Legal", "Executive", "A4", "A5", "A6" };
            string[] trays = { "Automatically Select", "Printer auto select", "Tray 1", "Tray 2", "Tray 3" };
            string[] orientation = { "Portrait", "Landscape" };
            string[] paperTypes = { "Plain", "Light", "Bond", "Letterhead", "Envelope", "Cardstock", "Prepunched" };
            string[] duplexValues = { "No", "Yesflipover", "Yesflipup" };
            string[] duplexer = { "Installed", "NotInstalled" };
            paperSize_comboBox.DataSource = paperSizes;
            paperSource_comboBox.DataSource = trays;
            orientation_comboBox.DataSource = orientation;
            paperType_comboBox.DataSource = paperTypes;
            duplex_comboBox.DataSource = duplexValues;
            duplexer_comboBox.DataSource = duplexer;
            UpdatePreferences();

        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            UpdatePreferences();
            DialogResult = DialogResult.OK;
        }

        private void UpdatePreferences()
        {
            PrintPreference.Copies = Convert.ToInt32(copies_numericUpDown.Value);
            PrintPreference.InputTray = paperSource_comboBox.Text;
            PrintPreference.Orientation = orientation_comboBox.Text;
            PrintPreference.PaperSize = paperSize_comboBox.Text;
            PrintPreference.PaperType = paperType_comboBox.Text;
            PrintPreference.DuplexValue = duplex_comboBox.Text;
            PrintPreference.Duplexer = duplexer_comboBox.Text;
        }
    }





}
