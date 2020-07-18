using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Session execution control used to start/pause/end session execution
    /// </summary>
    public partial class ControlSessionExecution : UserControl
    {
        private SessionExecutionManager _manager;
        private ExecutingSessionInfo _session;
        private BindingList<SessionActivityData> _activityList;
        private ActivityDetailsGrid _activityDetailsGrid;
        private DateTime _lastUpdated = DateTime.MinValue;

        /// <summary>
        /// triggers when refresh status button is pressed
        /// </summary>
        public event EventHandler<SessionControlRefreshEventArgs> RefreshRequested;

        private SessionExecutionManager Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                _manager = value;
            }
        }

        public ExecutingSessionInfo Session
        {
            get { return _session; }
            set
            {
                _session = value;
                if (value == null)
                {
                    _session = new ExecutingSessionInfo();
                }
                else
                {
                    _session = value;
                }
                TraceFactory.SetSessionContext(_session.SessionId);
                SetDataBindings();
            }
        }

        public BindingList<SessionActivityData> ActivityList => _activityList;

        private void SetDataBindings()
        {
            SetControlButtonStatus();

            // refresh subscription to property changed events
            try
            {
                Session.PropertyChanged -= Session_PropertyChanged;
            }
            catch { }
            Session.PropertyChanged += Session_PropertyChanged;

        }

        private void Session_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.InvokeIfRequired(n =>
            {
                // Only update if the session state has changed OR we have not updated in the last 10 seconds
                if (e.PropertyName == "State" || _lastUpdated < DateTime.Now.AddSeconds(-10))
                {
                    _lastUpdated = DateTime.Now;
                    UpdateControls();
                }

                SetControlButtonStatus();

                if (e.PropertyName == "LastState")
                {
                    if (Session.State == SessionState.RunComplete)
                    {
                        Manager.StartAutoShutDown(Session.SessionId);
                    }
                }
            });
        }

        private void UpdateControls()
        {
            TraceFactory.Logger.Debug("Updating session execution controls.");
            sessionIdLabel.Text = GetDisplayText(Session.SessionId);
            sessionNameLabel.Text = GetDisplayText(Session.Name);

            if (Session.State == SessionState.RunComplete && Session.ShutDownDate.HasValue)
            {
                sessionStateLabel.Text = "Shut Down";
            }
            else
            {
                sessionStateLabel.Text = GetDisplayText(Session.State.GetDescription());
            }
            sessionOwnerLabel.Text = GetDisplayText(Session.Owner);
            sessionStartLabel.Text = GetDisplayText(Session.StartDate);
            SetEndDateDisplay();
            SetActivityCounts();
            TraceFactory.Logger.Debug("Updating session execution controls complete.");
        }

        private void SetActivityCounts()
        {
            // Get the list of activities by session id.
            using (var context = DbConnect.DataLogContext())
            {
                TraceFactory.Logger.Debug("Getting activity counts.");
                var activityCounts = context.SessionData(Session.SessionId).Activities
                    .GroupBy(n => n.Status)
                    .Select(n => new { n.Key, Count = n.Count() })
                    .ToDictionary(n => n.Key, n => n.Count);
                TraceFactory.Logger.Debug("Activity counts retrieved.");

                int sessionCount = activityCounts.Where(n => n.Key != "Started").Sum(n => n.Value);
                sessionTotalActivitiesLabel.Text = GetDisplayText(sessionCount.ToString());

                sessionPassLabel.Text = GetActivityCount(activityCounts, "Passed");
                sessionFailLabel.Text = GetActivityCount(activityCounts, "Failed");
                sessionErrorLabel.Text = GetActivityCount(activityCounts, "Error");
                sessionSkipLabel.Text = GetActivityCount(activityCounts, "Skipped");
                sessionRetryLabel.Text = GetActivityCount(activityCounts, "Retrying");

                // Refresh the activities grid view if it is displayed.
                if (_activityDetailsGrid?.Visible == true)
                {
                    TraceFactory.Logger.Debug("Refreshing activity details grid.");
                    ActivityList.Clear();
                    _activityList = new BindingList<SessionActivityData>(context.SessionData(Session.SessionId).Activities.ToList());

                    _activityDetailsGrid.activityExecution_BindingSource.DataSource = null;
                    _activityDetailsGrid.activityExecution_BindingSource.DataSource = ActivityList;
                    TraceFactory.Logger.Debug("Done refreshing activity details grid.");
                }
            }
        }

        private static string GetActivityCount(Dictionary<string, int> counts, string status)
        {
            int result = 0;
            counts.TryGetValue(status, out result);
            return result.ToString();
        }

        private void SetEndDateDisplay()
        {
            var endDate = new DateTime?();
            var labelText = "End:";
            if (!Session.IsInState(SessionState.RunComplete, SessionState.ShuttingDown, SessionState.ShutdownComplete))
            {
                labelText = "End (Est.):";
            }
            sessionEndPrompt.Text = labelText;

            if (Session.IsInState(SessionState.PauseComplete, SessionState.Pausing))
            {
                sessionEndLabel.Text = "Paused";
            }
            else
            {
                if (Session.ShutDownDate.HasValue)
                {
                    endDate = Session.ShutDownDate;
                }
                else if (Session.EstimatedEndDate.HasValue)
                {
                    endDate = Session.EstimatedEndDate;
                }
                sessionEndLabel.Text = GetDisplayText(endDate);
            }
        }

        private string GetDisplayText(string origValue)
        {
            return (string.IsNullOrEmpty(origValue) ? "---" : origValue);
        }

        private string GetDisplayText(DateTime? origValue)
        {
            var value = (!origValue.HasValue ? string.Empty : origValue.Value.ToString("g"));
            return GetDisplayText(value);
        }

        private string SessionId
        {
            get { return Session.SessionId; }
        }

        private SessionStartupTransition Transition
        {
            get
            {
                return Session.StartupTransition;
            }
        }

        private SessionState State
        {
            get
            {
                return Session.State;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSessionExecution"/> class.
        /// </summary>
        public ControlSessionExecution()
        {
            InitializeComponent();

            _activityList = new BindingList<SessionActivityData>();
        }

        public void Initialize(SessionExecutionManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            Manager = manager;
        }

        /// <summary>
        /// On load of the control
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// when refresh is called
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            Manager.Refresh();
            SetControlButtonStatus();
        }

        private void SetControlButtonStatus()
        {
            this.InvokeIfRequired(c =>
            {
                //start, pause, resume, repeat, end, notes, refresh
                if (_session != null)
                {
                    end_ToolStripButton.Enabled = Session.CanEnd();
                    dispatcherLog_ToolStripButton.Enabled = (!string.IsNullOrEmpty(SessionId));
                    notes_ToolStripButton.Enabled = (!string.IsNullOrEmpty(SessionId));

                    pause_ToolStripButton.Enabled = (State == SessionState.Running);
                    pause_ToolStripButton.Visible = (State != SessionState.PauseComplete);
                    resume_ToolStripButton.Visible = (State == SessionState.PauseComplete);
                    repeat_ToolStripButton.Visible = (State == SessionState.RunComplete && Session.CanEnd());
                }
                refresh_ToolStripButton.Enabled = SessionClient.Instance.MyCallbackServiceIsOpen;
            });
        }


        /// <summary>
        /// triggers when refresh is called for a session
        /// </summary>
        /// <param name="e">event arguments</param>
        protected virtual void OnRefreshRequested(SessionControlRefreshEventArgs e)
        {
            if (RefreshRequested != null)
            {
                RefreshRequested(this, e);
            }
        }

        private void start_ToolStripButton_Click(object sender, EventArgs e)
        {
            Manager.StartSession();
        }

        private void pause_ToolStripButton_Click(object sender, EventArgs e)
        {
            pause_ToolStripButton.Enabled = false;
            SessionClient.Instance.Pause(SessionId);
        }

        private void resume_ToolStripButton_Click(object sender, EventArgs e)
        {
            resume_ToolStripButton.Visible = false;
            SessionClient.Instance.Resume(SessionId);
        }

        
        private void repeat_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will repeat your whole session again.  Do you want to continue?",
                "Repeat Session", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                HashSet<string> offlineDevices = null;
                offlineDevices = Manager.GetSessionOfflineDevices();

                //Only show the form to the user if there are Offline Devices for the session.
                if (offlineDevices.Count != 0)
                {
                    //Creating the form
                    OfflineDevicesMessageBox message = new OfflineDevicesMessageBox();
                    //Showing the form
                    DialogResult userChoice = message.ShowDialog(offlineDevices);

                    if (userChoice == DialogResult.OK)
                    {
                        offlineDevices = message.GetSelectedOnlineDevicesChecklistBoxes();
                        Manager.SetSessionOfflineDevices(offlineDevices);
                    }
                    //If the user choose to cancel operation, do nothing.
                    else
                    {
                        return;
                    }
                }
                //Repeating the session.
                repeat_ToolStripButton.Visible = false;
                Manager.Repeat();
            }
        }

        private void end_ToolStripButton_Click(object sender, EventArgs e)
        {
            SessionResetManager resetManager = new SessionResetManager();

            // For STE, require that shutdown requester be authorized
            if (GlobalSettings.IsDistributedSystem)
            {
                if (!resetManager.IsAuthorized(SessionId))
                {
                    return;
                }
            }

            using (SessionShutdownForm form = new SessionShutdownForm(SessionId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    resetManager.LogSessionReset(SessionId);
                    end_ToolStripButton.Enabled = false;
                    dispatcherLog_ToolStripButton.Enabled = false;
                    SessionClient.Instance.Shutdown(SessionId, form.ShutdownOptions);
                }
            }
        }

        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            Manager.Refresh();
            OnRefreshRequested(new SessionControlRefreshEventArgs(Session.SessionId));
        }

        private void dispatcherLog_ToolStripButton_Click(object sender, EventArgs e)
        {
            string data = SessionClient.Instance.GetLogData(SessionId);
            using (TextDisplayDialog dialog = new TextDisplayDialog(data))
            {
                dialog.ShowDialog(this);
            }
        }

        private void notes_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (SessionNotesForm form = new SessionNotesForm())
            {
                form.SessionId = SessionId;
                form.ShowDialog(this);
            }
        }

        private void sessionStateLabel_TextChanged(object sender, EventArgs e)
        {
            if (_session.IsInState(SessionState.RunComplete))
            {
                sessionStateLabel.BackColor = System.Drawing.Color.PaleGreen;
            }
            else if (_session.IsInState(SessionState.Error))
            {
                sessionStateLabel.BackColor = System.Drawing.Color.Pink;
            }
            else
            {
                sessionStateLabel.BackColor = DefaultBackColor;
            }
        }

        private void activityCount_Click(object sender, EventArgs e)
        {
            _activityDetailsGrid = ActivityDetailsGrid.Instance;

            _activityDetailsGrid.Icon = ParentForm.Icon;
            _activityDetailsGrid.Text = "Completed Activity Details for Session ID {0}".FormatWith(Session.SessionId);
            _activityDetailsGrid.activityExecution_BindingSource.DataSource = ActivityList;

            _activityDetailsGrid.Show();
        }
    }
}