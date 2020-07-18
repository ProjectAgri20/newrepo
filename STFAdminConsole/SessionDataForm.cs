using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.Reporting;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Utility;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// UI display for management of Session data.
    /// </summary>
    public partial class SessionDataForm : Form
    {
        private string _sessionResetting = string.Empty;
        private GridViewCellInfo _selectedSessionCell = null;
        private string _directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Initializes a new instance of the<see cref="SessionDataForm"/> class.
        /// </summary>
        public SessionDataForm()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);
            UserInterfaceStyler.Configure(sessionData_GridView, GridViewStyle.ReadOnly);
        }

        private void SessionDataForm_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private GridViewCellInfo SelectedSession
        {
            get
            {
                if (sessionData_GridView.SelectedRows.Count > 0)
                {
                    return sessionData_GridView.SelectedRows[0].Cells["sessionId_GridViewColumn"];
                }

                return null;
            }
        }

        private string SelectedSessionId
        {
            get
            {
                var session = SelectedSession;
                return (session != null) ? session.Value.ToString() : string.Empty;
            }
        }

        private void RefreshGrid()
        {
            if (sessionData_GridView.InvokeRequired) //Make sure we're on the UI thread
            {
                sessionData_GridView.Invoke(new MethodInvoker(this.RefreshGrid));
            }
            else
            {
                sessionData_GridView.DataSource = null;

                // Store the current location so we can restore it
                int currentLocation = sessionData_GridView.SelectedRows.Any() ? sessionData_GridView.SelectedRows[0].Index : 0;

                // Load the list of sessions
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    var sessionActivityCounts = context.SessionActivityCounts();
                    var sessions = context.DbSessions.AsEnumerable().Select(n => new
                    {
                        n.SessionId,
                        n.Cycle,
                        n.SessionName,
                        n.Type,
                        n.Tags,
                        n.Owner,
                        StartDateTime = n.StartDateTime.HasValue ? n.StartDateTime.Value.ToLocalTime() : n.StartDateTime,
                        EndDateTime = n.EndDateTime.HasValue ? n.EndDateTime.Value.ToLocalTime() : n.EndDateTime,
                        ExpirationDateTime = n.ExpirationDateTime.HasValue ? n.ExpirationDateTime.Value.ToLocalTime() : n.ExpirationDateTime,
                        n.Status,
                        n.ShutdownState,
                        n.Dispatcher,
                        ActivityCount = sessionActivityCounts.ContainsKey(n.SessionId) ? sessionActivityCounts[n.SessionId] : 0
                    });

                    sessionData_GridView.DataSource = sessions;
                }

                //Restore the current location
                if (sessionData_GridView.Rows.Any())
                {
                    if (sessionData_GridView.Rows.Count <= currentLocation)
                    {
                        currentLocation = sessionData_GridView.Rows.Count - 1;
                    }

                    sessionData_GridView.TableElement.ScrollToRow(currentLocation);
                    sessionData_GridView.Rows[currentLocation].IsCurrent = true;
                }
            }
        }

        private static SessionLogRetention GetDefaultLogRetention()
        {
            int result = int.Parse(GlobalSettings.Items[Setting.DefaultLogRetention], CultureInfo.InvariantCulture);
            if (result < (int)SessionLogRetention.None || result > (int)SessionLogRetention.OneYear)
            {
                // Out of bounds of SessionLogRetention enum
                result = 0; //Default to first item
            }
            return (SessionLogRetention)result;
        }

        private void notesToolStripButton_Click(object sender, EventArgs e)
        {
            using (SessionNotesForm form = new SessionNotesForm())
            {
                form.SessionId = this.SelectedSessionId;
                form.ShowDialog(this);
            }
        }

        private void extendToolStripButton_Click(object sender, EventArgs e)
        {
            string result = InputDialog.Show("Select expiration extension for this Session.",
                                             "Extend Expiration",
                                             EnumUtil.GetDescription(GetDefaultLogRetention()),
                                             SessionLogRetentionHelper.ExpirationList);

            if (!string.IsNullOrEmpty(result))
            {
                SessionLogRetention extension = EnumUtil.GetByDescription<SessionLogRetention>(result);

                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    SessionSummary session = context.DbSessions.FirstOrDefault(s => s.SessionId == SelectedSessionId);
                    if (session != null)
                    {
                        session.ExpirationDateTime = extension.GetExpirationDate(session.ExpirationDateTime.Value);
                    }
                    context.SaveChanges();
                }

                RefreshGrid();
            }
        }

        private void configurationToolStripButton_Click(object sender, EventArgs e)
        {
            SessionSummary session = null;
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                session = context.DbSessions.Find(SelectedSessionId);
                context.Entry(session).Collection(n => n.Scenarios).Load();
            }

            bool displayedSomething = false;
            foreach (SessionScenario sessionScenario in session.Scenarios)
            {
                if (!string.IsNullOrEmpty(sessionScenario.ConfigurationData))
                {
                    XmlDisplayDialog dialog = new XmlDisplayDialog(XDocument.Parse(sessionScenario.ConfigurationData));
                    dialog.Text = $"Session {sessionScenario.SessionId} Scenario {sessionScenario.RunOrder} Configuration Data".FormatWith(SelectedSessionId);
                    dialog.Show(this);
                    displayedSomething = true;
                }
            }

            if (!displayedSomething)
            {
                MessageBox.Show(this, $"No configuration data available for session {SelectedSessionId}.", "No Data Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void deleteSessionInfoToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete saved information for the selected session?", "Delete Session Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                // Mark the selected session for deletion by the cleanup process.
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    SessionSummary session = context.DbSessions.FirstOrDefault(s => s.SessionId == SelectedSessionId);
                    if (session != null)
                    {
                        session.ExpirationDateTime = DateTime.UtcNow;
                    }
                    context.SaveChanges();
                }

                Cursor.Current = Cursors.Default;
                RefreshGrid();
            }
        }

        private void releaseSession_ToolStripButton_Click(object sender, EventArgs e)
        {
            _selectedSessionCell = SelectedSession;
            var sessionId = _selectedSessionCell.Value.ToString();

            if (!GlobalSettings.IsDistributedSystem)
            {
                if (sessionData_GridView.SelectedRows[0].Cells[12].Value.ToString() != Environment.MachineName.ToUpper())
                {
                    var message = MessageBox.Show($"You are trying to release a session which was run on client machine -' { sessionData_GridView.SelectedRows[0].Cells[12].Value} ' . This will not close the Virtual worker console on that client.  Do you want to continue?", "Release Session", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (message == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }
            }

            string msg = "This will abort the session (if it is currently running) and release all machines and other assets used in this test.  Are you sure you want to release this Session?";
            var result = MessageBox.Show(msg, "Reset Session", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (!string.IsNullOrEmpty(sessionId))
            {
                _sessionResetting = sessionId;

                if (STFDispatcherManager.Dispatcher == null && STFDispatcherManager.ConnectToDispatcher() == false)
                {
                    //The user canceled the connect dialog
                    return;
                }

                // For STE, require that shutdown requester be authorized
                SessionResetManager resetManager = new SessionResetManager();
                if (GlobalSettings.IsDistributedSystem)
                {
                    if (!resetManager.IsAuthorized(sessionId))
                    {
                        return;
                    }
                }

                using (SessionShutdownForm form = new SessionShutdownForm(sessionId))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        resetManager.LogSessionReset(sessionId);
                        SetControls(true);
                        SessionClient.Instance.ClearSessionRequestReceived += controller_SessionResetComplete;
                        SessionClient.Instance.SetUserCredential(UserManager.CurrentUser);
                        SessionClient.Instance.Close(sessionId, form.ShutdownOptions);
                    }
                }

                statusLabel.Text = "Releasing session {0} and cleaning up associated virtual machines and resources...".FormatWith(sessionId);
            }
        }

        private void SetControls(bool lockDown)
        {
            if (sessionData_GridView.InvokeRequired) //Make sure we're on the UI thread
            {
                this.Invoke(new Action<bool>(this.SetControls), lockDown);
            }
            else
            {
                releaseSession_ToolStripButton.Enabled = !lockDown;
                releaseSession_ToolStripMenuItem.Enabled = !lockDown;

                if (_selectedSessionCell != null)
                {
                    foreach (GridViewCellInfo cell in _selectedSessionCell.RowInfo.Cells)
                    {
                        cell.Style.ForeColor = lockDown ? Color.Red : Color.Black;
                    }
                }
                statusLabel.Text = string.Empty;
            }
        }

        private void controller_SessionResetComplete(object sender, Framework.Dispatcher.SessionIdEventArgs e)
        {
            SessionClient.Instance.ClearSessionRequestReceived -= controller_SessionResetComplete;

            //Enable the button if this form made the reset call.
            if (e.SessionId == _sessionResetting)
            {
                SetControls(false);
                _sessionResetting = string.Empty;
            }
            RefreshGrid();

            TraceFactory.Logger.Error("Session {0} was reset.".FormatWith(e.SessionId));
        }

        private void mainContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = EnableDeleteSessionInfo();
            exportToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled;
        }

        private void sessionData_GridView_SelectionChanged(object sender, EventArgs e)
        {
            deleteSessionInfoToolStripButton.Enabled = EnableDeleteSessionInfo();
            exportToolStripButton.Enabled = deleteSessionInfoToolStripButton.Enabled;
        }

        private bool EnableDeleteSessionInfo()
        {
            if (sessionData_GridView.SelectedRows.Count() > 0)
            {
                var state = sessionData_GridView.SelectedRows[0].Cells["runState_GridViewColumn"].Value;

                if (state != null && (state.ToString().Equals("Aborted") || state.ToString().Equals("Complete")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            RefreshGrid();
            Cursor = Cursors.Default;
        }

        private void exportToolStripButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportSaveFileDialog(_directory, "Export Test Scenario Data", ImportExportType.Scenario))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _directory = Path.GetDirectoryName(dialog.Base.FileName);

                    using (SqlConnection reportDb = new SqlConnection(ReportingSqlConnection.ConnectionString))
                    {
                        reportDb.Open();
                        // Get the schema of the 'SessionConfiguration' table in the reporting database server so we know if we are pointing to the long term data server.
                        string[] columnRestrictions = new string[4];
                        columnRestrictions[2] = "SessionConfiguration";
                        columnRestrictions[3] = "STF_SessionId";
                        DataTable columnSchema = reportDb.GetSchema(SqlClientMetaDataCollectionNames.Columns, columnRestrictions);

                        using (SqlCommand command = reportDb.CreateCommand())
                        {
                            string sessionConfigQuery = "SELECT ConfigurationData FROM SessionConfiguration WHERE {0} LIKE ('{1}')";
                            if (columnSchema.Rows.Count > 0)
                            {
                                command.CommandText = sessionConfigQuery.FormatWith("STF_SessionId", SelectedSessionId);
                            }
                            else
                            {
                                command.CommandText = sessionConfigQuery.FormatWith("SessionId", SelectedSessionId);
                            }

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows && reader.Read())
                                {
                                    string sessionConfigData = reader.GetValue(0).ToString();

                                    File.WriteAllText(dialog.Base.FileName, sessionConfigData);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void exportMemoryToolStripButton_Click(object sender, EventArgs e)
        {
            // Get the folder where the user wants to place the compressed memory xml files.
            if (memoryFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                using (DataLogContext dataLogDb = DbConnect.DataLogContext())
                {
                    // Get the list of devices used in the selected session.
                    var sessionDevices = dataLogDb.DbSessions.SelectMany(n => n.Devices).Where(n => n.SessionId == SelectedSessionId).Select(n => n.DeviceId);
                    foreach (string deviceId in sessionDevices)
                    {
                        // Create a Zip file to contain the memory XML strings.
                        string deviceIdArchiveFilename = $"{SelectedSessionId}_{deviceId}.zip";
                        string deviceIdArchineFilePath = Path.Combine(memoryFolderBrowserDialog.SelectedPath, deviceIdArchiveFilename);

                        using (FileStream deviceIdArchiveFile = new FileStream(deviceIdArchineFilePath, FileMode.OpenOrCreate))
                        {
                            using (ZipArchive deviceIdArchive = new ZipArchive(deviceIdArchiveFile, ZipArchiveMode.Update))
                            {
                                string sql = "SELECT dmx.MemoryXml, sd.ProductName, FORMAT(dbo.fn_CalcLocalDateTime(dms.SnapshotDateTime), 'yyyy.MM.dd-HH.mm.ss.fff') AS SnapshotDateTime "
                                           + "FROM DeviceMemoryXml dmx "
                                           + "JOIN DeviceMemorySnapshot dms ON dmx.DeviceMemorySnapshotId = dms.DeviceMemorySnapshotId "
                                           + "JOIN SessionDevice sd ON dms.DeviceId = sd.DeviceId AND dms.SessionId = sd.SessionId "
                                           + "WHERE dms.SessionId = @p0 AND dms.DeviceId = @p1";
                                IEnumerable<MemoryXmlResult> memResults = dataLogDb.Database.SqlQuery<MemoryXmlResult>(sql, SelectedSessionId, deviceId);

                                foreach (var result in memResults)
                                {
                                    // Create an entry in the archive file.
                                    string deviceIdMemEntryName = $"{SelectedSessionId}_{result.ProductName}_{deviceId}_{result.SnapshotDateTime}.xml";
                                    ZipArchiveEntry deviceIdMemEntry = deviceIdArchive.CreateEntry(deviceIdMemEntryName);
                                    using (StreamWriter writer = new StreamWriter(deviceIdMemEntry.Open()))
                                    {
                                        writer.Write(result.MemoryXml);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private sealed class MemoryXmlResult
        {
            public string MemoryXml { get; set; }
            public string ProductName { get; set; }
            public string SnapshotDateTime { get; set; }
        }
    }
}
