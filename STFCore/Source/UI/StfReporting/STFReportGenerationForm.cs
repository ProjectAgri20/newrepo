using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.Reporting
{
    public partial class STFReportGenerationForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="STFReportGenerationForm"/> class.
        /// </summary>
        public STFReportGenerationForm()
        {
            InitializeComponent();
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.NumericButtonsCount = 10;
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.FirstPageButtonImage = Resources.DataContainer_MoveFirstHS;
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.FirstButton.ToolTipText = "First page";
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.PreviousPageButtonImage = Resources.DataContainer_MovePreviousHS;
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.PreviousButton.ToolTipText = "Previous page";
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.NextPageButtonImage = Resources.DataContainer_MoveNextHS;
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.NextButton.ToolTipText = "Next page";
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.LastPageButtonImage = Resources.DataContainer_MoveLastHS;
            sessionSummary_radGridView.GridViewElement.PagingPanelElement.LastButton.ToolTipText = "Last page";
            PopulateSessionSummaryGrid();
        }

        private void PopulateSessionSummaryGrid()
        {
            string sessionInfoConnectionString;
            DataTable sessionInfoTable = new DataTable();

            StringBuilder sessionInfoQuery = new StringBuilder();
            sessionInfoQuery.Append("SELECT ss.SessionId, ss.SessionName, ss.[Owner], dbo.fn_CalcLocalDateTime(ss.StartDateTime) AS StartDateTime, dbo.fn_CalcLocalDateTime(ss.EndDateTime) AS EndDateTime, ss.[Status], ss.[Type], ss.Cycle, aec.Activities, ss.Tags ");
            sessionInfoQuery.Append("FROM SessionSummary ss ");
            sessionInfoQuery.Append("LEFT JOIN ( SELECT COUNT(*) Activities, ae.SessionId FROM ActivityExecution ae GROUP BY ae.SessionId ) AS aec ON ss.SessionId = aec.SessionId ");
            sessionInfoQuery.Append("ORDER BY ss.StartDateTime DESC");

            sessionInfoConnectionString = ReportingSqlConnection.ConnectionString;

            // Get the session info records.
            using (SqlDataAdapter adapter = new SqlDataAdapter(sessionInfoQuery.ToString(), sessionInfoConnectionString))
            {
                adapter.Fill(sessionInfoTable);
            }

            sessionSummary_radGridView.DataSource = sessionInfoTable;
        }

        private bool ValidateTemplatePath(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                return false;
            }

            if (!File.Exists(template))
            {
                return false;
            }

            return true;
        }

        private void generateReport_button_Click(object sender, EventArgs e)
        {
            string template = template_textBox.Text;

            if (!ValidateTemplatePath(template))
            {
                errorProvider.SetError(template_textBox, "The selected template does not exist");
                return;
            }
            else
            {
                errorProvider.Clear();
            }

            if (sessionSummary_radGridView.SelectedRows.Count == 0)
            {
                errorProvider.SetError(sessionSummary_radGridView, "At least one row must be selected");
                return;
            }
            else
            {
                errorProvider.Clear();
            }

            // Build the list of selected session ID's into a comma delimited string.
            List<string> sessionIds = sessionSummary_radGridView.Rows.Where(r => r.IsSelected == true).Select(r => r.Cells["sessionId_column"].Value.ToString()).ToList();

            // Get the distination file path.
            saveFileDialog.FileName = BuildReportFilename(Path.GetFileName(template));
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ReportingEngine.GenerateReport(template, saveFileDialog.FileName, sessionIds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Report Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.LogDebug(ex.ToString());
                }
            }

            DialogResult = DialogResult.OK;
        }

        private string BuildReportFilename(string templateFileName)
        {
            string cycle = string.Empty;
            string sessionName = string.Empty;
            string startDate = string.Empty;
            List<string> sessionIds = new List<string>();
            StringBuilder fileName = new StringBuilder();

            foreach (GridViewRowInfo item in sessionSummary_radGridView.SelectedRows)
            {
                sessionIds.Add((string)item.Cells["sessionId_column"].Value);

                if (sessionIds.Count == 1)
                {
                    cycle = (string)item.Cells["cycle_column"].Value;
                    sessionName = (string)item.Cells["sessionName_column"].Value;
                    startDate = ((DateTime)item.Cells["startDateTime_column"].Value).ToString("yyyyMMdd");
                }
            }

            if (sessionSummary_radGridView.SelectedRows.Count > 1)
            {
                fileName.Append($"{string.Join(",", sessionIds)}-");
            }
            else
            {
                if (!string.IsNullOrEmpty(cycle))
                {
                    fileName.Append($"{cycle}-");
                }
                if (!string.IsNullOrEmpty(sessionName))
                {
                    fileName.Append($"{sessionName}-");
                }
                fileName.Append($"{sessionIds[0]}-");
                if (!string.IsNullOrEmpty(startDate))
                {
                    fileName.Append($"{startDate}-");
                }
            }

            fileName.Append(templateFileName);

            return fileName.ToString();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void browse_button_Click(object sender, EventArgs e)
        {
            if (GlobalSettings.Items.ContainsKey(Setting.ReportTemplateRepository))
            {
                openFileDialog.InitialDirectory = GlobalSettings.Items[Setting.ReportTemplateRepository];
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                template_textBox.Text = openFileDialog.FileName;
            }
        }
    }
}
