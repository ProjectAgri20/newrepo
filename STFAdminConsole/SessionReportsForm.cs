using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using HP.ScalableTest.Data;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Reporting;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI;

namespace HP.ScalableTest.LabConsole
{
    public partial class SessionReportsForm : Form
    {
        private bool _generating = false;
        
        public SessionReportsForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(session_GridView, GridViewStyle.ReadOnly);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Load the list of sessions
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            using (StoredProcedure storedProc = new StoredProcedure(EnterpriseTestSqlConnection.ConnectionString, "sel_SessionsWithCounts"))
            {
                table.Load(storedProc.ExecuteReader());
            }

            session_GridView.DataSource = table;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = IsReportGenerating();            
            base.OnFormClosing(e);
        }

        private void browse_Button_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = GlobalSettings.Items[Setting.ReportTemplateRepository];
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                template_TextBox.Text = openFileDialog.FileName;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void generate_Button_Click(object sender, EventArgs e)
        {
            // Get the selected template and be sure it exists
            string template = template_TextBox.Text;
            if (! ValidateInput(template))
            {
                return;
            }

            // Get the list of selected session ids
            var sessionIds = session_GridView.SelectedRows.Select(n => n.Cells["sessionId_GridViewColumn"].Value.ToString());

            // Prompt the user for the report destination and create the report
            string fileSession = (sessionIds.Count() == 1 ? sessionIds.First() : "Multiple Sessions");
            string reportName = Path.GetFileNameWithoutExtension(template);
            saveFileDialog.FileName = "{0} {1} {2}".FormatWith(DateTime.Today.ToString("MMM dd", CultureInfo.CurrentCulture), fileSession, reportName);
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Copy the template to the destination
                    string destination = saveFileDialog.FileName;
                    File.Copy(template, destination, true);

                    // Create the report
                    ExcelReportFile report = new ExcelReportFile(destination);

                    //Thread the report creation
                    BackgroundWorker generateReportWorker = new BackgroundWorker();
                    generateReportWorker.DoWork += new DoWorkEventHandler(GenerateReport_DoWork);
                    generateReportWorker.RunWorkerCompleted += GenerateReport_RunWorkerCompleted;
                    generateReportWorker.RunWorkerAsync(new object[] { report, sessionIds });
                    _generating = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Report Creation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceFactory.Logger.Error(ex.Message, ex);
                }
            }
        }

        private bool ValidateInput(string selectedTemplate)
        {
            if (IsReportGenerating())
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(selectedTemplate))
            {
                MessageBox.Show("A template must be specified.", "Invalid Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!File.Exists(selectedTemplate))
            {
                MessageBox.Show("The specified report template does not exist.", "Invalid Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;

        }

        private bool IsReportGenerating()
        {
            if (_generating)
            {
                MessageBox.Show("A report is currently being generated. \nPlease wait for it to finish before proceeding.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            return false;
        }

        private void GenerateReport_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;

            try
            {
                ReportingEngine.CreateExcelReport((ExcelReportFile)args[0], (IEnumerable<string>)args[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Report Creation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TraceFactory.Logger.Error(ex.Message, ex);
            }
        }

        private void GenerateReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _generating = false;
            MessageBox.Show("Report Generation Complete", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Manually set the DialogResult and call to close to handle the case when this form may be shown non-modally.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

    }
}
