using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.UI;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.UI.SessionExecution.Wizard;
using HP.ScalableTest.Framework.Settings;
using System;
using System.Windows.Forms;
using HP.ScalableTest;
using HP.ScalableTest.Utility;

namespace HP.SolutionTest.WorkBench
{
    /// <summary>
    /// Session execution control used to start/pause/end session execution
    /// </summary>
    public partial class SessionExecutionControlLite : UserControl
    {
        private string _sessionId;
        private SessionState _state = SessionState.Available;
        private SessionStartupTransition _transition = SessionStartupTransition.None;
        private SessionExecutionTreeView _treeView;

        /// <summary>
        /// triggers when refresh status button is pressed
        /// </summary>
        public event EventHandler<SessionControlRefreshEventArgs> RefreshRequested;

        private System.Timers.Timer _shutdownTimer = null;

        private string SessionId
        {
            get { return _sessionId; }
            set
            {
                _sessionId = value;
                TraceFactory.SetSessionContext(_sessionId);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this control is ready to start a new session.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready to start; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToStart
        {
            get
            {
                if (_transition == SessionStartupTransition.None)
                {
                    return _state != SessionState.PowerUp
                        && _state != SessionState.Running
                        && _state != SessionState.ShuttingDown
                        && _state != SessionState.Unavailable;
                }
                else
                {
                    return _transition == SessionStartupTransition.ReadyToStart
                        && _state != SessionState.Unavailable;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSessionExecution"/> class.
        /// </summary>
        public SessionExecutionControlLite()
        {
            InitializeComponent();

            _shutdownTimer = new System.Timers.Timer();
            _shutdownTimer.Elapsed += _shutdownTimer_Elapsed;
        }

        /// <summary>
        /// On load of the control
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //SetControlButtonStatus();
        }

        /// <summary>
        /// when refresh is called
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            SetControlButtonStatus();
        }

        /// <summary>
        /// Initializes this instance by subscribing to <see cref="SessionClient"/> events.
        /// </summary>
        public void Initialize(SessionExecutionTreeView treeView)
        {
            _treeView = treeView;

            // Need the invoke calls to prevent cross-thread exceptions
            SessionClient.Instance.SessionStateReceived += (s, e) => Invoke(new Action(() => SessionStateReceived(e.State)));
            SessionClient.Instance.SessionStartupTransitionReceived += (s, e) => Invoke(new Action(() => SessionStartupTransitionReceived(e.Transition)));
            SessionClient.Instance.SessionMapElementReceived += (s, e) => Invoke(new Action(() => SessionMapElementReceived(e.MapElement)));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            // Clear status
            SessionId = null;
            _state = SessionState.Available;
            _transition = SessionStartupTransition.None;

            // Clear UI labels and reset buttons
            sessionId_Label.Text = "---";
            sessionState_Label.Text = "---";
            sessionName_Label.Text = "---";
            sessionOwner_Label.Text = "---";
            SetControlButtonStatus();
        }

        private void SessionStateReceived(SessionState state)
        {
            TraceFactory.Logger.Debug("Received dispatcher state: " + state.ToString());
            _state = state;
            sessionState_Label.Text = EnumUtil.GetDescription(state);
            SetControlButtonStatus();
            if (state == SessionState.RunComplete)
            {
                SetAutoShutDown();
            }
        }

        /// <summary>
        /// Sets the automatic shut down timer for completed sessions
        /// </summary>
        private void SetAutoShutDown()
        {
            try
            {
                // Check for system setting value on how long to wait before shutting down a completed session
                // ignore if no such setting exists
                string autoShutdownKey = "AutoShutdownTime";
                if (GlobalSettings.Items.ContainsKey(autoShutdownKey))
                {
                    if (!string.IsNullOrEmpty(GlobalSettings.Items[autoShutdownKey]))
                    {
                        double autoShutdownTime = Convert.ToDouble(GlobalSettings.Items[autoShutdownKey]); ;

                        _shutdownTimer.Interval = TimeSpan.FromHours(autoShutdownTime).TotalMilliseconds;
                        _shutdownTimer.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                TraceFactory.Logger.Error("SetAutoShutDown", ex);
                if (_shutdownTimer != null)
                {
                    _shutdownTimer.Enabled = false;
                }
            }
        }

        private void SessionStartupTransitionReceived(SessionStartupTransition transition)
        {
            TraceFactory.Logger.Debug("Received dispatcher transition state: " + transition.ToString());
            _transition = transition;
            SetControlButtonStatus();

            // Special case: if the dispatcher state is "Ready To Run", start the scenario
            if (transition == SessionStartupTransition.ReadyToRun)
            {
                TraceFactory.Logger.Debug("Sending Run request to dispatcher");
                SessionClient.Instance.Run(SessionId);
            }
        }

        private void SetControlButtonStatus()
        {
            //start, pause, resume, repeat, end, notes, refresh
            start_ToolStripButton.Enabled = (IsReadyToStart);
            end_ToolStripButton.Enabled = (_transition != SessionStartupTransition.ReadyToStart &&
                                                _state != SessionState.Unavailable &&
                                                _state != SessionState.Canceled &&
                                                _state != SessionState.ShuttingDown &&
                                                _state != SessionState.ShutdownComplete &&
                                                _state != SessionState.Available);

            dispatcherLog_ToolStripButton.Enabled = (!IsReadyToStart);
            notes_ToolStripButton.Enabled = (!IsReadyToStart);

            pause_ToolStripButton.Enabled = (_state == SessionState.Running);
            pause_ToolStripButton.Visible = (_state != SessionState.PauseComplete);
            resume_ToolStripButton.Visible = (_state == SessionState.PauseComplete);
            repeat_ToolStripButton.Visible = (_state == SessionState.RunComplete);
            refresh_ToolStripButton.Enabled = SessionClient.Instance.MyCallbackServiceIsOpen;
        }

        private void SessionMapElementReceived(SessionMapElement element)
        {
            if (element.ElementType == ElementType.Session)
            {
                SessionId = element.SessionId;
                sessionId_Label.Text = element.SessionId;
            }
        }

        /// <summary>
        /// start the execution of the scenario
        /// </summary>
        /// <param name="scenarioId">the scenario ID</param>
        public void StartSession(Guid scenarioId)
        {
            using (SessionConfigurationWizard wizard = new SessionConfigurationWizard(scenarioId))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    SessionTicket ticket = wizard.Ticket;
                    SessionId = ticket.SessionId;

                    // Set the display details from the ticket
                    sessionId_Label.Text = ticket.SessionId;
                    sessionName_Label.Text = ticket.SessionName;
                    sessionOwner_Label.Text = ticket.SessionOwner.UserName;

                    // Start the session
                    start_ToolStripButton.Enabled = false;
                    SessionClient.Instance.PowerUp(SessionId);
                }
                else
                {
                    if (_transition != SessionStartupTransition.ReadyToStart && _transition != SessionStartupTransition.None)
                    {
                        ShutdownOptions options = new ShutdownOptions();
                        options.ShutdownDeviceSimulators = false;
                        SessionClient.Instance.Shutdown(wizard.Ticket.SessionId, options);
                    }
                }
            }
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
            StartSession(Properties.Settings.Default.LastExecutedScenario);
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
                repeat_ToolStripButton.Visible = false;
                SessionClient.Instance.Repeat(SessionId);
            }
        }

        private void end_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (SessionShutdownForm form = new SessionShutdownForm(SessionId)) 
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    end_ToolStripButton.Enabled = false;
                    dispatcherLog_ToolStripButton.Enabled = false;
                    SessionClient.Instance.Shutdown(SessionId, form.ShutdownOptions);
                }
            }
        }

        private void refresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            _treeView.Clear();
            SessionClient.Instance.Refresh();
            OnRefreshRequested(new SessionControlRefreshEventArgs(_sessionId));
        }

        private void dispatcherLog_ToolStripButton_Click(object sender, EventArgs e)
        {
            using (TextDisplayDialog dialog = new TextDisplayDialog())
            {
                dialog.Data = SessionClient.Instance.GetLogData(SessionId);
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

        private void _shutdownTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(
                () =>
                {
                    end_ToolStripButton.Enabled = false;
                    dispatcherLog_ToolStripButton.Enabled = false;
                }));
            ShutdownOptions options = new ShutdownOptions()
            {
                AllowWorkerToComplete = false,
                CopyLogs = false,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.PowerOff
            };

            SessionClient.Instance.Shutdown(SessionId, options);
            _shutdownTimer.Enabled = false;
        }
    }
}