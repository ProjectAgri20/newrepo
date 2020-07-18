using System;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// User control that allows users to perform Job Accounting and Quota Operations.
    /// </summary>
    internal partial class JobAccountingUserControl : UserControl
    {
        public JobAccountingOperation operation;
        JobAccountingTabData jobAccountingdata = new JobAccountingTabData();

        /// <summary>
        /// Initializes a new instance of the <see cref="JobAccountingUserControl"/> class.
        /// </summary>
        public JobAccountingUserControl()
        {
            InitializeComponent();
            outputFormat_ComboBox.DataSource = EnumUtil.GetValues<OutputFormat>().ToList();
        }

        /// <summary>
        /// Creates and returns a <see cref="JobAccountingTabData" /> instance containing the
        /// JobAccounting tab data from this control.
        /// </summary>
        /// <returns>The Job Accounting data.</returns>
        public JobAccountingTabData GetConfigurationData()
        {
            if (quota_radioButton.Checked)
            {
                operation = JobAccountingOperation.Quota;
            }
            else
            {
                operation = JobAccountingOperation.Report;
                jobAccountingdata.ReportName = reportName_TextBox.Text;
                jobAccountingdata.ReportEmailTo = sendReportEmail_TextBox.Text;
                jobAccountingdata.OutputFormat = EnumUtil.Parse<OutputFormat>(outputFormat_ComboBox.Text, true);
            }

            jobAccountingdata.JobAccountingOperation = operation;

            return jobAccountingdata;
        }


        /// <summary>
        /// Configures the controls per the jobAccounting data either derived from initialization or the saved meta data.
        /// </summary>
        public void LoadConfiguration(JobAccountingTabData jobAccountingdata)
        {
            reportName_TextBox.Text = jobAccountingdata.ReportName;
            sendReportEmail_TextBox.Text = jobAccountingdata.ReportEmailTo;
            outputFormat_ComboBox.Text = jobAccountingdata.OutputFormat.ToString();
            if (jobAccountingdata.JobAccountingOperation == JobAccountingOperation.Quota)
            {
                quota_radioButton.Checked = true;
            }
            else
            {
                report_radioButton.Checked = true;
            }
        }

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        public PluginValidationResult AddValidaton()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        public void RemoveValidation()
        {
            fieldValidator.Remove(report_groupBox);
        }

        private void quota_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (quota_radioButton.Checked)
            {
                report_groupBox.Enabled = false;
                fieldValidator.Remove(reportName_TextBox);
                fieldValidator.Remove(sendReportEmail_TextBox);
                fieldValidator.Remove(outputFormat_ComboBox);
            }
            else
            {
                report_groupBox.Enabled = true;
                fieldValidator.RequireValue(reportName_TextBox, name_Label);
                fieldValidator.RequireValue(sendReportEmail_TextBox, sendReportEmail_Label);
                fieldValidator.RequireValue(outputFormat_ComboBox, outputFormat_Label);
            }

        }
    }
}
