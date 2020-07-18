using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.SessionExecution.Wizard;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Class SessionExecutionManager manages interactions between SessionClient.Instance, session execution tree, session execution control, and executing session info control
    /// </summary>
    public class SessionExecutionManager : INotifyPropertyChanged
    {
        private int MAX_ACTIVE_SESSIONS = 5;
        private List<ExecutingSessionInfo> _sessions = new List<ExecutingSessionInfo>();
        private SessionExecutionTreeView _treeView;
        private ToolStripButton _startButton;
        private Panel _elementStatusPanel;
        private Control _dockingControl;
        private ControlSessionExecution _sessionControl;
        private System.Timers.Timer _sessionEndTimer;

        private ExecutingSessionInfo _currentDisplayedSession;
        private bool _ableToStartNewSession = true;
        private bool _sessionResetting = false;
        private bool _hasSessionEnded = false;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionExecutionManager"/> class.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        /// <param name="dockingControl">The docking control.</param>
        /// <param name="sessionControl">The session control.</param>
        /// <param name="startButton">The start button.</param>
        public SessionExecutionManager(SessionExecutionTreeView treeView, Control dockingControl, ControlSessionExecution sessionControl, ToolStripButton startButton)
        {
            _treeView = treeView;
            _dockingControl = dockingControl;
            _sessionControl = sessionControl;
            _startButton = startButton;
            _elementStatusPanel = new Panel();

            if (!GlobalSettings.IsDistributedSystem)
            {
                MAX_ACTIVE_SESSIONS = 1;
            }

            Initialize();
        }

        /// <summary>
        /// Gets the dispatcher host name.
        /// </summary>
        /// <value>The dispatcher.</value>
        public string Dispatcher { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _startButton.Enabled = false;
            while (_dockingControl.Controls.Count > 0)
            {
                Control oldControl = _dockingControl.Controls[0];
                _dockingControl.Controls.Remove(oldControl);
                oldControl.Dispose();
            }
            _dockingControl.Controls.Add(_elementStatusPanel);
            _elementStatusPanel.Dock = DockStyle.Fill;
            _elementStatusPanel.Name = "elementStatus_Panel";
            _elementStatusPanel.TabIndex = 1;

            _treeView.Initialize();
            _sessionControl.Initialize(this);
            ClearSessions();

            _treeView.SessionMapElementSelected += treeView_SessionMapElementSelected;
            STFDispatcherManager.DispatcherChanged += HandleDispatcherChanged;

            SessionClient.Instance.SessionStateReceived += SessionStateReceived;
            SessionClient.Instance.SessionMapElementReceived += SessionMapElementReceived;
            SessionClient.Instance.SessionStartupTransitionReceived += SessionStartupTransitionReceived;

            _sessionEndTimer = new System.Timers.Timer(60 * 1000);
            _sessionEndTimer.Elapsed += _sessionEndTimer_Elapsed;
            _sessionEndTimer.Start();
        }

        /// <summary>
        /// Update the current session estimated end date at specific intervals
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private async void _sessionEndTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_currentDisplayedSession != null && !_currentDisplayedSession.ShutDownDate.HasValue)
                {
                    _sessionEndTimer.Enabled = false;
                    await UpdateSessionInfoFromDatabaseAsynch(_currentDisplayedSession, false);
                }
            }
            finally
            {
                _sessionEndTimer.Enabled = true;
            }
        }

        private void SessionStartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            if (e != null && !string.IsNullOrEmpty(e.SessionId))
            {
                TraceFactory.Logger.Debug($"Incoming transition state: {e.Transition}");
                var execInfo = GetExecutingSessionInfo(e.SessionId);
                if (execInfo != null)
                {
                    TraceFactory.Logger.Debug($"Session state: {execInfo.State}");
                    execInfo.StartupTransition = e.Transition;

                    // SessionExecutionManager is subscribed to the same SessionClient events as the startup wizard.
                    // The startup wizard handles all SessionStartupTransitions except for "Run".  When the session finally
                    // transitions to "ReadyToRun", SessionExecutionManager is the only one listening (the wizard is gone).
                    // This is true also when the session is resetting.
                    if (e.Transition == SessionStartupTransition.ReadyToRun)
                    {
                        TraceFactory.Logger.Debug("Sending Run request to dispatcher");
                        SessionClient.Instance.Run(e.SessionId);
                        _sessionResetting = false;
                    }
                    else if (_sessionResetting)
                    {
                        // SessionExecutionManager should only handle the following transition states if the session is resetting.
                        // Otherwise, it is assumed that these are being handled by the startup wizard.
                        switch (e.Transition)
                        {
                            case SessionStartupTransition.ReadyToValidate:
                                TraceFactory.Logger.Debug("Calling Validate");
                                SessionClient.Instance.Validate(e.SessionId);
                                break;
                            case SessionStartupTransition.ReadyToPowerUp:
                                TraceFactory.Logger.Debug("Calling PowerUp");
                                SessionClient.Instance.PowerUp(e.SessionId);
                                break;
                        }
                    }
                }
            }
        }

        private void SessionMapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            if (e != null && e.MapElement != null && !string.IsNullOrEmpty(e.MapElement.SessionId))
            {
                if (e.MapElement.ElementType == ElementType.Session)
                {
                    var execInfo = GetExecutingSessionInfo(e.MapElement.SessionId);
                    if (execInfo != null)
                    {
                        execInfo.MapElement = e.MapElement;
                    }
                }
            }
        }

        private void SessionStateReceived(object sender, SessionStateEventArgs e)
        {
            if (e != null && !string.IsNullOrEmpty(e.SessionId))
            {
                SessionState state = e.State;
                TraceFactory.Logger.Debug("Received dispatcher state: " + state.ToString());
                var execInfo = GetExecutingSessionInfo(e.SessionId);
                if (execInfo != null)
                {
                    execInfo.State = state;
                }
            }

            if (e != null)
            {
                HasSessionEnded = (e.State == SessionState.Error || e.State == SessionState.RunComplete);
                if (!_sessionResetting)
                {
                    _sessionResetting = (e.State == SessionState.Resetting);
                }                
            }
        }

        /// <summary>
        /// Ensures the dispatcher is connected.
        /// If not connected, attempts to connect.
        /// </summary>
        /// <returns><c>true</c> if dispatcher is connected, <c>false</c> otherwise.</returns>
        public bool EnsureDispatcherConnected()
        {
            if (!STFDispatcherManager.Connected)
            {
                STFDispatcherManager.ConnectToDispatcher();
            }
            return STFDispatcherManager.Connected;
        }

        /// <summary>
        /// Gets the collection of managed sessions.
        /// </summary>
        /// <returns>IEnumerable&lt;ExecutingSessionInfo&gt;.</returns>
        public IEnumerable<ExecutingSessionInfo> GetManagedSessions()
        {
            return _sessions;
        }

        /// <summary>
        /// Sets the dispatcher host name.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public void SetDispatcher(string dispatcher)
        {
            if (string.IsNullOrEmpty(dispatcher))
            {
                Dispatcher = "[Not Connected]";
                _startButton.Enabled = false;
            }
            else
            {
                Dispatcher = dispatcher;
                _startButton.Enabled = true;
            }

            ClearSessions();
            _treeView.RootNode.Text = Dispatcher;
            _treeView.SelectedNode = _treeView.RootNode;
        }

        private void HandleDispatcherChanged(object sender, EventArgs e)
        {
            if (STFDispatcherManager.Dispatcher != null)
            {
                this.SetDispatcher(STFDispatcherManager.Dispatcher.HostName);
                RefreshCurrentSession();
            }
            else
            {
                this.SetDispatcher(null);
                RefreshCurrentSession();
            }
        }

        /// <summary>
        /// Clears the currently running session.
        /// </summary>
        public void ClearSessions()
        {
            ClearCurrentSession();
            _treeView.Clear();
        }

        /// <summary>
        /// Raise the PropertyChanged event for the property specified in the expression
        /// Example - below will raise a PropertyChanged event on the Foo property
        ///     OnPropertyChanged(() => Foo);
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expr">The expression to get the property from</param>
        private void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(body.Member.Name));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the dispatcher service is able to start new session.
        /// </summary>
        /// <value><c>true</c> if dispatcher service is able to start new session; otherwise, <c>false</c>.</value>
        public bool AbleToStartNewSession
        {
            get
            {
                return _ableToStartNewSession;
            }
            private set
            {
                if (_ableToStartNewSession != value)
                {
                    _ableToStartNewSession = value;
                    OnPropertyChanged(() => AbleToStartNewSession);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the currently running session has session ended.
        /// </summary>
        /// <value><c>true</c> if the currently running session has session ended; otherwise, <c>false</c>.</value>
        public bool HasSessionEnded
        {
            get { return _hasSessionEnded; }
            private set
            {
                if (_hasSessionEnded != value)
                {
                    _hasSessionEnded = value;
                    OnPropertyChanged(() => HasSessionEnded);
                }
            }
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <value>The current session.</value>
        internal ExecutingSessionInfo CurrentSession
        {
            get { return _currentDisplayedSession; }
        }

        /// <summary>
        /// Gets the current session identifier.
        /// </summary>
        /// <value>The current session identifier.</value>
        public string CurrentSessionId
        {
            get { return (_currentDisplayedSession == null ? string.Empty : _currentDisplayedSession.SessionId); }
        }

        /// <summary>
        /// Clears the current session.
        /// </summary>
        public void ClearCurrentSession()
        {
            if (_currentDisplayedSession != null)
            {
                _currentDisplayedSession = null;
            }
        }

        /// <summary>
        /// Refreshes the current session.
        /// </summary>
        public void RefreshCurrentSession()
        {
            _treeView.Clear();

            SessionClient.Instance.Refresh();
            _sessionControl.Session = _currentDisplayedSession;
            UpdateSessionInfoFromDatabaseAsynch(_currentDisplayedSession);
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <returns><c>true</c> if start operation successful, <c>false</c> otherwise.</returns>
        public bool StartSession()
        {
            return StartSession(new List<Guid>() { Properties.Settings.Default.LastExecutedScenario });
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="scenarioIds">The list of scenario identifiers.</param>
        /// <returns><c>true</c> if start operation successful, <c>false</c> otherwise.</returns>
        public bool StartSession(List<Guid> scenarioIds)
        {
            SessionConfigurationWizard wizard = new SessionConfigurationWizard(scenarioIds);
            return StartSession(wizard, wizard.Ticket);
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="wizard">The wizard form.</param>
        /// <param name="ticket">The ticket.</param>
        /// <returns><c>true</c> if start operation successful, <c>false</c> otherwise.</returns>
        public bool StartSession(Form wizard, SessionTicket ticket)
        {
            bool result = false;

            var activeSessions = GetActiveSessions().Count();
            if (AbleToStartNewSession == false || activeSessions >= MAX_ACTIVE_SESSIONS)
            {
                MessageBox.Show(_treeView, "{0} active sessions out of {1} allowed".FormatWith(activeSessions, MAX_ACTIVE_SESSIONS), "Unable To Start Session");
                return result;
            }

            if (EnsureDispatcherConnected())
            {
                try
                {
                    AbleToStartNewSession = false;
                    using (wizard)
                    {
                        var newSession = GetNewExecutingSessionInfo(ticket);
                        if (wizard.ShowDialog(_sessionControl) == DialogResult.OK)
                        {
                            //SessionClient.Instance.PowerUp(newSession.SessionId);
                            newSession.UpdateFromTicket(ticket);
                            result = true;
                            UpdateSessionInfoFromDatabaseAsynch(newSession, false);
                        }
                        else
                        {
                            var transition = newSession.StartupTransition;
                            if (transition != SessionStartupTransition.ReadyToStart && transition != SessionStartupTransition.None)
                            {
                                ShutdownOptions options = new ShutdownOptions();
                                options.ShutdownDeviceSimulators = false;
                                SessionClient.Instance.Shutdown(newSession.SessionId, options);
                                newSession.State = SessionState.ShutdownComplete;
                            }
                            else
                            {
                                newSession.State = SessionState.Canceled;
                                RemoveManagedSession(newSession);
                            }
                        }
                    }
                }
                finally
                {
                    AbleToStartNewSession = true;
                }
            }
            return result;
        }

        private void AddManagedSession(ExecutingSessionInfo newSession)
        {
            lock (this)
            {
                _sessions.Add(newSession);
            }
            SetCurrentSession(newSession.SessionId);
        }

        private void RemoveManagedSession(ExecutingSessionInfo session)
        {
            lock (this)
            {
                _sessions.RemoveAll(x => x.Equals(session));
            }
        }

        private ExecutingSessionInfo GetNewExecutingSessionInfo(SessionTicket ticket)
        {
            if (IsManagedSession(ticket.SessionId))
            {
                throw new Exception("Session {0} already exists".FormatWith(ticket.SessionId));
            }

            var result = new ExecutingSessionInfo()
            {
                SessionId = ticket.SessionId,
                Name = ticket.SessionName,
                Owner = ticket.SessionOwner.UserName,
                MapElement = null,
                State = SessionState.Available,
                StartupTransition = SessionStartupTransition.None,
                Dispatcher = Dispatcher,
            };

            AddManagedSession(result);
            SetCurrentSession(result.SessionId);
            return result;
        }

        /// <summary>
        /// Gets the active sessions - sessions that have not been terminated (e.g. not shut down or cancelled)
        /// </summary>
        /// <returns>IEnumerable&lt;ExecutingSessionInfo&gt;.</returns>
        public IEnumerable<ExecutingSessionInfo> GetActiveSessions()
        {
            var active = _sessions.Where(x =>
                x.IsInState(
                    SessionState.Available,
                    SessionState.Error,
                    SessionState.Pausing,
                    SessionState.PauseComplete,
                    SessionState.PowerUp,
                    SessionState.Reserving,
                    SessionState.Staging,
                    SessionState.Validating,
                    SessionState.Running,
                    SessionState.ShuttingDown
                    )
                || (x.State == SessionState.RunComplete && !x.ShutDownDate.HasValue)
                );
            return active;
        }

        private IEnumerable<ExecutingSessionInfo> GetInactiveSessions()
        {
            var inActive = _sessions.Where(x =>
                x.IsInState(
                    SessionState.Canceled,
                    SessionState.ShutdownComplete
                    )
                || (x.State == SessionState.RunComplete && x.ShutDownDate.HasValue)
                );
            return inActive;
        }

        private bool IsManagedSession(string sessionId)
        {
            return (GetExecutingSessionInfo(sessionId) != null);
        }

        private ExecutingSessionInfo GetExecutingSessionInfo(string sessionId)
        {
            ExecutingSessionInfo result;
            lock (this)
            {
                result = _sessions.FirstOrDefault(x => x != null && x.SessionId.EqualsIgnoreCase(sessionId));
            }
            return result;
        }

        /// <summary>
        /// Sets the current session to the specified Session Id.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void SetCurrentSession(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                _currentDisplayedSession = GetExecutingSessionInfo(sessionId);
                RefreshCurrentSession();
            }
            else
            {
                MessageBox.Show("No session found for {0}".FormatWith(sessionId)
                    , "Session Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RemoveControlsFromParent(Control parent, bool dispose = false)
        {
            // Remove all controls from the session status panel
            while (parent.Controls.Count > 0)
            {
                Control oldControl = parent.Controls[0];
                parent.Controls.Remove(oldControl);
                if (dispose)
                {
                    oldControl.Dispose();
                }
            }
        }

        private void treeView_SessionMapElementSelected(object sender, SessionMapElementEventArgs e)
        {
            if (e == null || e.MapElement == null || string.IsNullOrEmpty(e.MapElement.SessionId)) return;

            var sessionId = e.MapElement.SessionId;
            Control parentControl = _elementStatusPanel;

            // Change currently monitored session if needed
            if (!CurrentSessionId.EqualsIgnoreCase(sessionId))
            {
                if (IsManagedSession(sessionId))
                {
                    SetCurrentSession(sessionId);
                }
                else
                {
                    GetNewSessionInfoFromDatabase(sessionId);
                }
            }

            // Load the panel for that session
            if (CurrentSessionId.EqualsIgnoreCase(sessionId))
            {
                try
                {
                    parentControl.SuspendLayout();

                    // Remove all controls from the session status panel
                    RemoveControlsFromParent(parentControl, true);

                    // Load the appropriate control for the selected element
                    try
                    {
                        // Create the session status control and add to the panel
                        SessionStatusControlBase control = null;
                        try
                        {
                            control = SessionStatusControlFactory.Create(e.MapElement);
                            if (control != null)
                            {
                                control.Initialize(e.MapElement, _sessionControl);
                                parentControl.Controls.Add(control);
                                control.Dock = DockStyle.Fill;
                                control.InvokeIfRequired(c => control.Refresh());
                            }
                        }
                        catch
                        {
                            if (control != null)
                            {
                                control.Dispose();
                            }
                            throw;
                        }
                    }
                    catch (ControlTypeMismatchException ex)
                    {
                        MessageBox.Show(ex.Message, "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error(ex.ToString(), ex);
                        throw ex;
                    }
                }
                finally
                {
                    parentControl.ResumeLayout();
                }
            }
        }

        /// <summary>
        /// Refreshes the current running session.
        /// </summary>
        public void Refresh()
        {
            RefreshCurrentSession();
        }

        /// <summary>
        /// Gets the dispatcher log.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>System.String.</returns>
        public string GetDispatcherLog(string sessionId)
        {
            return SessionClient.Instance.GetLogData(sessionId);
        }

        /// <summary>
        /// Shuts down the specified session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="options">The options.</param>
        public void Shutdown(string sessionId, ShutdownOptions options)
        {
            SessionClient.Instance.Shutdown(sessionId, options);
        }

        /// <summary>
        /// Starts the automatic shut down.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <exception cref="System.Exception">{0} is not a currently managed session</exception>
        public void StartAutoShutDown(string sessionId)
        {
            if (!IsManagedSession(sessionId))
            {
                throw new Exception("{0} is not a currently managed session");
            }

            try
            {
                // Check for system setting value on how long to wait before shutting down a completed session
                // ignore if no such setting exists
                string autoShutdownKey = "AutoShutdownTime";
                if (GlobalSettings.Items.ContainsKey(autoShutdownKey))
                {
                    if (!string.IsNullOrEmpty(GlobalSettings.Items[autoShutdownKey]))
                    {
                        double autoShutdownTime = Convert.ToDouble(GlobalSettings.Items[autoShutdownKey]);

                        Task.Factory.StartNew(() =>
                           {
                               Thread.Sleep(TimeSpan.FromHours(autoShutdownTime));
                               ShutdownOptions options = new ShutdownOptions()
                               {
                                   AllowWorkerToComplete = false,
                                   CopyLogs = false,
                                   PowerOff = true,
                                   PowerOffOption = VMPowerOffOption.PowerOff
                               };

                               Shutdown(sessionId, options);
                           }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("StartAutoShutDown", ex);
            }
        }


        private ExecutingSessionInfo GetNewSessionInfoFromDatabase(string sessionId)
        {
            if (IsManagedSession(sessionId))
            {
                throw new Exception("Session {0} already exists".FormatWith(sessionId));
            }

            var result = GetSessionInfoFromDatabase(sessionId);

            // Only add if the dispatcher in the database matches our dispatcher
            if (result != null && result.Dispatcher.EqualsIgnoreCase(Dispatcher))
            {
                AddManagedSession(result);
                SetCurrentSession(result.SessionId);
            }
            else
            {
                return null;
            }
            return result;
        }

        private ExecutingSessionInfo GetSessionInfoFromDatabase(string sessionId)
        {
            ExecutingSessionInfo result = new ExecutingSessionInfo() { SessionId = sessionId };
            UpdateSessionInfoFromDatabase(result);
            return result;
        }

        private bool UpdateSessionInfoFromDatabase(ExecutingSessionInfo execSessionInfo, bool includeStateData = true)
        {
            bool result = false;
            DateTime? actualEndTime = null;
            if (execSessionInfo != null && !string.IsNullOrEmpty(execSessionInfo.SessionId))
            {
                var sessionId = execSessionInfo.SessionId;
                using (var context = DbConnect.DataLogContext())
                {
                    var session = context.Sessions.FirstOrDefault(s => s.SessionId == sessionId);
                    if (session != null)
                    {
                        actualEndTime = session.ShutdownDateTime?.LocalDateTime;
                        if (actualEndTime.HasValue)
                        {
                            execSessionInfo.ShutDownDate = actualEndTime;
                        }

                        execSessionInfo.Dispatcher = session.Dispatcher;
                        execSessionInfo.SessionId = session.SessionId;
                        execSessionInfo.Name = session.SessionName;
                        execSessionInfo.Owner = session.Owner;
                        execSessionInfo.StartDate = session.StartDateTime?.LocalDateTime;
                        execSessionInfo.EstimatedEndDate = session.ProjectedEndDateTime?.LocalDateTime;

                        //if (includeStateData)
                        //{
                        //    var state = Enum<SessionState>.Parse(session.Status, true);
                        //    execSessionInfo.MapElement = null;
                        //    execSessionInfo.State = state;
                        //    execSessionInfo.StartupTransition = SessionStartupTransition.None;
                        //}

                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Repeats the session.
        /// </summary>
        internal void Repeat()
        {
            if (_currentDisplayedSession != null)
            {
                SessionClient.Instance.Repeat(_currentDisplayedSession.SessionId);
                UpdateSessionInfoFromDatabaseAsynch(_currentDisplayedSession);
            }
        }

        /// <summary>
        /// Get the list of offline devices (due to communication errors) during the session.
        /// </summary>
        /// <returns>list of offline devices</returns>
        public HashSet<string> GetSessionOfflineDevices()
        {
            HashSet<string> offlineDevices = null;
            if (_currentDisplayedSession != null)
            {
                offlineDevices = SessionClient.Instance.GetSessionOfflineDevices(_currentDisplayedSession.SessionId);
                return offlineDevices;
            }
            else
                return offlineDevices;

        }

        /// <summary>
        /// Set the list of offline devices (due to communication errors) for the repeat session and pause session. 
        /// </summary>
        /// <param name="onlineDevices">Offline Devices</param>
        public void SetSessionOfflineDevices(HashSet<string> onlineDevices)
        {
            if (_currentDisplayedSession != null)
            {
                SessionClient.Instance.SetSessionOfflineDevices(_currentDisplayedSession.SessionId, onlineDevices);
            }
            
        }

        /// <summary>
        /// Updates the session information from database asynchronously.
        /// </summary>
        /// <param name="sessionInfo">The session information.</param>
        /// <param name="includeStateData">if set to <c>true</c> [include state data].</param>
        /// <returns>Task.</returns>
        internal Task UpdateSessionInfoFromDatabaseAsynch(ExecutingSessionInfo sessionInfo, bool includeStateData = true)
        {
            Task returnValue = null;
            if (sessionInfo != null)
            {
                returnValue = Task.Factory.StartNew(() => UpdateSessionInfoFromDatabase(sessionInfo, includeStateData));
            }
            return returnValue;
        }
    }
}
