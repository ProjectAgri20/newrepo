using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.STF.TriageDataTool.Properties;

namespace HP.STF.TriageDataTool
{
    public partial class TriageDataForm : Form
    {
        private string _activityExecutionId;
        private DataTable _dtPerformanceEvents;
        private DataTable _dtTriageEvents;
        private Dictionary<string, string> _sessionInfo;
        private string _triageDataId;

        /**********************************************************************
         * TriageDataForm event handlers
         *********************************************************************/

        public TriageDataForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the application's main form is loaded,
        ///   * Get and apply the last user's settings
        ///   * Get the list of database environments from the App.config file.
        ///   * Load the list of sessions where the start date was 1 day earlier.
        /// </summary>
        private void TriageDataForm_Load(object sender, EventArgs e)
        {
            // Restore the previously stored user settings.
            Location = Settings.Default.MainFormLocation != null ? Settings.Default.MainFormLocation : new Point(0, 0);
            Size = Settings.Default.MainFormSize != null ? Settings.Default.MainFormSize : new Size(100, 100);

            try
            {
                cbDbEnvironment.SelectedIndex = Settings.Default.SelectedDbEnvironment;
            }
            catch (ArgumentOutOfRangeException)
            {
                cbDbEnvironment.SelectedIndex = -1;
            }

            // Get the list of database environments from App.config.
            LoadDatabaseInformation();

            // Get the sessions ID's that started within the previous day.
            dtpStartDate.Value = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            LoadSessionInfo();
        }

        /// <summary>
        /// When the application is closing, save some user settings.
        /// </summary>
        private void TriageDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.SelectedDbEnvironment = cbDbEnvironment.SelectedIndex;
            Settings.Default.MainFormLocation = Location;
            Settings.Default.MainFormSize = Size;

            Settings.Default.Save();
        }

        /**********************************************************************
         * Controls event handlers
         *********************************************************************/

        private void BtnExit_Click(object sender, EventArgs e) => Close();

        /// <summary>
        /// Switch to the new database environment and load the session ID's.
        /// </summary>
        private void CbDbEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyValuePair<string, string> selectedDbEnvironment = (KeyValuePair<string, string>)cbDbEnvironment.SelectedItem;

            tbDbServer.Text = selectedDbEnvironment.Value;
            tbDbName.Text = selectedDbEnvironment.Key.Equals("Boise Production") ? "EnterpriseTestODS" : "ScalableTestDatalog";

            LoadSessionInfo();
        }

        /// <summary>
        /// Get and display the triage events for the selected device.
        /// </summary>
        private void CbDeviceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbDeviceId.Text))
            {
                using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
                {
                    TriageDeviceInfo devInfo = tdc.GetDeviceInfoBySessionAndDeviceIds(cbSessionId.Text, cbDeviceId.Text);
                    tbDeviceName.Text = !string.IsNullOrEmpty(devInfo.DeviceName) ? devInfo.DeviceName : string.Empty;
                    tbProductName.Text = !string.IsNullOrEmpty(devInfo.ProductName) ? devInfo.ProductName : string.Empty;
                    tbModelNumber.Text = !string.IsNullOrEmpty(devInfo.ModelNumber) ? devInfo.ModelNumber : string.Empty;
                    tbFirmwareRevision.Text = !string.IsNullOrEmpty(devInfo.FirmwareRevision) ? devInfo.FirmwareRevision : string.Empty;
                    tbFirmwareDatecode.Text = !string.IsNullOrEmpty(devInfo.FirmwareDatecode) ? devInfo.FirmwareDatecode : string.Empty;
                    tbIpAddress.Text = !string.IsNullOrEmpty(devInfo.IpAddress) ? devInfo.IpAddress : string.Empty;
                }

                LoadTriageEventData(cbSessionId.Text, cbDeviceId.Text);
            }
        }

        /// <summary>
        /// Clear the information from the previous session and load the list
        /// of devices for the selected session.
        /// </summary>
        private void CbSessionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbStfVersion.Text = _sessionInfo[cbSessionId.Text];

            ClearDeviceInformationPanel();
            ClearTriageEventsPanel();
            ClearPerformanceEventsPanel();
            ClearTabControlPanel();

            LoadSessionDeviceIds(cbSessionId.Text);
        }

        private void CopyAndroidLogText_Click(object sender, EventArgs e) => Clipboard.SetText(rtbAndroidLog.SelectedText);

        private void CopyControlPanelImage_Click(object sender, EventArgs e) => Clipboard.SetImage(pbControlPanelImage.Image);

        private void CopyErrorInformationText_Click(object sender, EventArgs e) => Clipboard.SetText(rtbErrorInformation.SelectedText);

        /// <summary>
        /// Set the format to be used when displaying date/time in the
        /// performance marker table.
        /// </summary>
        private void DgvPerformanceEvents_DataSourceChanged(object sender, EventArgs e)
        {
            if (dgvPerformanceEvents.Columns.Contains("Event Date/Time"))
            {
                dgvPerformanceEvents.Columns["Event Date/Time"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss.fff tt";
            }
        }

        /// <summary>
        /// When the user clicks on a cell in the Select Triage Events table,
        /// update the information for the selected triage event.
        /// </summary>
        private void DgvTriageEvents_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                UpdatePerformanceEvents(e.RowIndex);

                UpdateErrorInformation(e.RowIndex);

                UpdateControlPanelImage(e.RowIndex);

                UpdateAndroidLog(e.RowIndex);
            }
        }

        /// <summary>
        /// Hide some of the columns in the Select Triage Event table and set
        /// the format for displaying date/time values.
        /// </summary>
        private void DgvTriageEvents_DataSourceChanged(object sender, EventArgs e)
        {
            if (dgvTriageEvents.Columns.Contains("TriageDataId"))
            {
                dgvTriageEvents.Columns["TriageDataId"].Visible = false;
            }

            if (dgvTriageEvents.Columns.Contains("ActivityExecutionId"))
            {
                dgvTriageEvents.Columns["ActivityExecutionId"].Visible = false;
            }

            if (dgvTriageEvents.Columns.Contains("ControlIds"))
            {
                dgvTriageEvents.Columns["ControlIds"].Visible = false;
            }

            if (dgvTriageEvents.Columns.Contains("Exception Message"))
            {
                dgvTriageEvents.Columns["Exception Message"].Visible = false;
            }

            if (dgvTriageEvents.Columns.Contains("Device Warnings"))
            {
                dgvTriageEvents.Columns["Device Warnings"].Visible = false;
            }

            if (dgvTriageEvents.Columns.Contains("Triage Date/Time"))
            {
                dgvTriageEvents.Columns["Triage Date/Time"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
            }
        }

        private void DtpEndDate_ValueChanged(object sender, EventArgs e) => LoadSessionInfo();

        private void DtpStartDate_ValueChanged(object sender, EventArgs e) => LoadSessionInfo();

        /**********************************************************************
         * Helper methods
         *********************************************************************/

        /// <summary>
        /// Clear information that is displayed for a device in preparation for
        /// another device.
        /// </summary>
        private void ClearDeviceInformationPanel()
        {
            foreach (Control control in gbDeviceId.Controls)
            {
                if (control is ComboBox)
                {
                    ((ComboBox)control).DataSource = null;
                }
                else if (control is TextBox)
                {
                    ((TextBox)control).ResetText();
                }
            }
        }

        /// <summary>
        /// Setting the DataSource property for this control effectively clears
        /// the control.
        /// </summary>
        private void ClearPerformanceEventsPanel() => dgvPerformanceEvents.DataSource = null;

        /// <summary>
        /// Clear the information displayed on each tab.
        /// </summary>
        private void ClearTabControlPanel()
        {
            rtbErrorInformation.ResetText();
            pbControlPanelImage.Image = null;
            rtbAndroidLog.ResetText();
        }

        /// <summary>
        /// Setting the DataSource property for this control effectively clears
        /// the control.
        /// </summary>
        private void ClearTriageEventsPanel() => dgvTriageEvents.DataSource = null;

        /// <summary>
        /// Load the list of database environments from the App.config file
        /// into the drop-down list control.
        /// </summary>
        private void LoadDatabaseInformation()
        {
            List<KeyValuePair<string, string>> dbEnvironments = new List<KeyValuePair<string, string>>();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string key in appSettings.AllKeys)
            {
                dbEnvironments.Add(new KeyValuePair<string, string>(key, appSettings[key]));
            }

            cbDbEnvironment.DataSource = null;
            cbDbEnvironment.Items.Clear();

            cbDbEnvironment.DataSource = new BindingSource(dbEnvironments, null);
            cbDbEnvironment.DisplayMember = "Key";
            cbDbEnvironment.ValueMember = "Value";
        }

        /// <summary>
        /// Load the list of device id's into the drop-down list control for
        /// all devices that recorded triage data from the provided session id.
        /// </summary>
        /// <param name="sessionId">The SessionId of interest.</param>
        private void LoadSessionDeviceIds(string sessionId)
        {
            using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
            {
                cbDeviceId.DataSource = tdc.GetDeviceIdsBySession(sessionId);
                cbDeviceId.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Get the list of SessionID's that are between the start and end
        /// dates and put them in the drop-down list control.
        /// </summary>
        private void LoadSessionInfo()
        {
            if (_sessionInfo != null)
            {
                _sessionInfo.Clear();
            }

            using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
            {
                _sessionInfo = tdc.GetSessionInfoByDateRange(dtpStartDate.Value.ToUniversalTime(), dtpEndDate.Value.ToUniversalTime());
            }

            cbSessionId.DataSource = _sessionInfo.Keys.ToList();
        }

        /// <summary>
        /// Get the triage event data for the provided session and device id's.
        /// </summary>
        /// <param name="sessionId">The SessionId of interest.</param>
        /// <param name="deviceId">The DeviceId of interest.</param>
        private void LoadTriageEventData(string sessionId, string deviceId)
        {
            using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
            {
                // Read the triage events from the database for the selected device.
                _dtTriageEvents = tdc.GetTriageEventsBySessionAndDeviceIds(sessionId, deviceId);
                DataView triageEventsView = new DataView(_dtTriageEvents);

                // Set the data BindingSource.
                bsTriageEvents.DataSource = triageEventsView;

                // Set the DataSource for the grid view.
                dgvTriageEvents.DataSource = bsTriageEvents;

                dgvTriageEvents.CurrentCell = dgvTriageEvents[1, 0];
            }
        }

        /// <summary>
        /// Gets the Android information for the triage event that is selected
        /// in the Select Triage Event table, formats it, and puts the
        /// information in the appropriate tab.
        /// </summary>
        /// <param name="rowIndex">The selected row number within the Select
        /// Triage Event table that the user has highlighted.</param>
        private void UpdateAndroidLog(int rowIndex)
        {
            StringBuilder androidLog = new StringBuilder();

            string controlIds = _dtTriageEvents.Rows[rowIndex]["ControlIds"].ToString();
            if (!string.IsNullOrEmpty(controlIds))
            {
                string[] temp = Regex.Split(controlIds, @"[\n\r,]+");
                foreach (string item in temp)
                {
                    androidLog.AppendLine(item);
                }
            }
            else
            {
                androidLog.AppendLine("No Android Log information for this activity.");
            }

            rtbAndroidLog.Clear();
            rtbAndroidLog.Text = androidLog.ToString();
        }

        /// <summary>
        /// Gets the device's control panel image for the triage event that is
        /// selected in the Select Triage Event table and puts it in the
        /// appropriate tab.
        /// </summary>
        /// <param name="rowIndex">The selected row number within the Select
        /// Triage Event table that the user has highlighted.</param>
        private void UpdateControlPanelImage(int rowIndex)
        {
            byte[] controlPanelImage;

            string triageDataId = _dtTriageEvents.Rows[rowIndex]["TriageDataId"].ToString();

            if (!triageDataId.Equals(_triageDataId))
            {
                using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
                {
                    controlPanelImage = tdc.GetControlPanelImage(triageDataId);
                }

                // Convert the bytes stored in the Triage table into an image that canbe displayed.
                if (controlPanelImage != null && controlPanelImage.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(controlPanelImage);
                    Image img = Image.FromStream(ms);
                    pbControlPanelImage.Image = img;
                }

                _triageDataId = triageDataId;
            }
        }

        /// <summary>
        /// Gets the error and device warning information for the triage event
        /// that is selected in the Select Triage Event table, formats it, and
        /// puts the information in the appropriate tab.
        /// </summary>
        /// <param name="rowIndex">The selected row number within the Select
        /// Triage Event table that the user has highlighted.</param>
        private void UpdateErrorInformation(int rowIndex)
        {
            StringBuilder errorText = new StringBuilder();

            // Get the reason for the recorded triage event.
            string reason = _dtTriageEvents.Rows[rowIndex]["Exception Message"].ToString();
            if (!string.IsNullOrEmpty(reason))
            {
                errorText.AppendLine("Exception Message:");
                errorText.AppendLine(reason);
                errorText.AppendLine();
            }

            // Get any warning text recorded on the device.
            string deviceWarnings = _dtTriageEvents.Rows[rowIndex]["Device Warnings"].ToString();
            if (!string.IsNullOrEmpty(deviceWarnings))
            {
                errorText.AppendLine("Device Warnings:");
                errorText.AppendLine(deviceWarnings);
            }

            rtbErrorInformation.Clear();
            rtbErrorInformation.Text = errorText.ToString();
        }

        /// <summary>
        /// Gets the performance labels for the triage event that is selected
        /// in the Select Triage Event table and puts the information in the
        /// appropriate table.
        /// </summary>
        /// <param name="rowIndex">The selected row number within the Select
        /// Triage Event table that the user has highlighted.</param>
        private void UpdatePerformanceEvents(int rowIndex)
        {
            if (_dtTriageEvents.Columns.Contains("ActivityExecutionId"))
            {
                string activityExecutionId = _dtTriageEvents.Rows[rowIndex]["ActivityExecutionId"].ToString();

                if (!activityExecutionId.Equals(_activityExecutionId))
                {
                    using (TriageDbContext tdc = new TriageDbContext(tbDbServer.Text, tbDbName.Text))
                    {
                        _dtPerformanceEvents = tdc.GetPerformanceEventsByActivityExecutionId(activityExecutionId);
                        DataView performanceEventsView = new DataView(_dtPerformanceEvents);

                        bsPerformanceEvents.DataSource = performanceEventsView;

                        dgvPerformanceEvents.DataSource = bsPerformanceEvents;
                    }

                    _activityExecutionId = activityExecutionId;
                }
            }
        }
    }
}
