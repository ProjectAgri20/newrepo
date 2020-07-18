using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Implements the <see cref="ISessionDispatcher"/> contract
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SessionDispatcher : ISessionDispatcher
    {
        private SessionProxyControllerSet _proxyControllers = null;
        private readonly string _backupFile = string.Empty;

        /// <summary>
        /// Gets the event tracker used to manage subscribers and events occurring in the dispatcher
        /// </summary>
        public SessionDispatcherEventPublisher EventPublisher { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionDispatcher"/> class.
        /// </summary>
        public SessionDispatcher()
        {
            TraceFactory.Logger.Debug("Initializing dispatcher...");

            EventPublisher = new SessionDispatcherEventPublisher();

            _backupFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "proxies.dat");

            AppDomain.CurrentDomain.ProcessExit += OnDispatcherProcessExit;

            int maxEntries = 1;
            if (GlobalSettings.IsDistributedSystem)
            {
                try
                {
                    string count = GlobalSettings.Items["MaxSessionsPerDispatcher"];
                    if (int.TryParse(count, out maxEntries))
                    {
                        TraceFactory.Logger.Debug("Max proxy instances set to {0}".FormatWith(maxEntries));
                    }
                }
                catch (SettingNotFoundException ex)
                {
                    TraceFactory.Logger.Error(ex.Message);
                }
            }

            _proxyControllers = new SessionProxyControllerSet(maxEntries);

            // Load any existing data in case the dispatcher service died.
            //SessionDataLoad();
        }

        /// <summary>
        /// Gets the proxies.
        /// </summary>
        /// <value>The proxies.</value>
        public SessionProxyControllerSet ProxyControllers
        {
            get { return _proxyControllers; }
        }

        /// <summary>
        /// Let's the dispatcher know that the proxy is started
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void NotifyProxyStarted(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            // This is called when the new proxy process is up and ready to go.  The proxy will
            // call the ISessionProxyCallback interface to inform the dispatcher that it is running.
            if (_proxyControllers.Contains(sessionId))
            {
                TraceFactory.Logger.Debug("Received signal that proxy process {0} is running".FormatWith(sessionId));
                _proxyControllers[sessionId].ProxyProcessStarted = true;
            }
            else
            {
                EventPublisher.PublishDispatcherException(new OperationCanceledException("Proxy process started for {0} with no visible entry in the dispatcher...".FormatWith(sessionId)));
            }
        }

        /// <summary>
        /// Initiates a new session based on the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ownerCallback">The owner callback.</param>
        public void Initiate(SessionTicket ticket, Uri ownerCallback)
        {
            string sessionId = ticket.SessionId;
            SetTraceSessionContext(sessionId);

            TraceFactory.Logger.Debug("SessionId {0}".FormatWith(sessionId));

            if (_proxyControllers.Contains(sessionId))
            {
                StackTrace trace = new StackTrace(true);

                StringBuilder builder = new StringBuilder();
                foreach (StackFrame item in trace.GetFrames())
                {
                    builder.AppendLine("{0}::[{1}, {2}]::{3}"
                        .FormatWith(item.GetFileName(), item.GetFileLineNumber(), item.GetFileColumnNumber(), item.GetMethod()));
                }
                TraceFactory.Logger.Error("A session proxy {0} already exists{1}{2}".FormatWith(sessionId, Environment.NewLine, builder));

                //string msg = "A session proxy {0} already exists".FormatWith(sessionId);
                //var exception = new ApplicationException(msg);
                //EventPublisher.PublishDispatcherException(exception);
                return;
            }

            TraceFactory.Logger.Debug("Starting proxy process");

            // Create a new entry for this session Id and start the associated Proxy Process
            _proxyControllers.StartProxyProcess(sessionId);

            if (GlobalSettings.IsDistributedSystem)
            {
                // Once the proxy service is started, give it a few more seconds to ensure it's fully initialized
                Thread.Sleep(3000);
            }

            // Make the initiate call but do it in the foreground to ensure it's completed before returning
            TraceFactory.Logger.Debug("CallProxy -> Initiate({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Initiate(ticket, ownerCallback), useThread: false);

            // If we now have a new session proxy running, then make sure all subscribers are added to
            // this session proxy's subscriber list.
            foreach (var subscriber in EventPublisher.Subscribers)
            {
                TraceFactory.Logger.Debug("CallProxy -> Subscribe({0})".FormatWith(sessionId));
                CallSessionProxy(sessionId, (c) => c.Subscribe(subscriber), useThread: false);
                TraceFactory.Logger.Debug("Subscriber {0} added to session proxy {1}".FormatWith(subscriber.AbsoluteUri, sessionId));
            }
        }

        /// <summary>
        /// Reserves all assets associated with this session using the specified key.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="reservationKey">The reservation key.</param>
        /// <returns></returns>
        /// <exception cref="System.ServiceModel.EndpointNotFoundException"></exception>
        public AssetDetailCollection Reserve(string sessionId, string reservationKey)
        {
            SetTraceSessionContext(sessionId);
            AssetDetailCollection assetDetails = new AssetDetailCollection();

            foreach (var item in assetDetails)
            {
                TraceFactory.Logger.Debug("Item availability:" + item.Availability);
            }

            if (!_proxyControllers.Contains(sessionId))
            {
                var msg = "Proxy not found for Session Id {0}".FormatWith(sessionId);
                TraceFactory.Logger.Debug(msg);
                throw new EndpointNotFoundException(msg);
            }

            var controller = _proxyControllers[sessionId];
            if (controller.ProxyProcessStarted)
            {
                TraceFactory.Logger.Debug("CallProxy -> Reserve({0})".FormatWith(sessionId));

                Action retryAction = () => assetDetails = controller.Channel.Reserve(reservationKey);
                TraceFactory.Logger.Debug("Returned Reservation");
                try
                {
                    Retry.WhileThrowing
                        (
                            retryAction,
                            10,
                            TimeSpan.FromSeconds(1),
                            new List<Type>() { typeof(EndpointNotFoundException) }
                        );
                }
                catch (Exception ex)
                {
                    var msg = "Error communicating with session proxy for {0} - 1".FormatWith(sessionId);
                    TraceFactory.Logger.Error(msg, ex);
                    var exception = new ApplicationException(msg, ex);
                    EventPublisher.PublishDispatcherException(exception);
                }
            }
            else
            {
                var msg = "Proxy service not started for session {0} (1)".FormatWith(sessionId);
                var exception = new ApplicationException(msg);
                EventPublisher.PublishDispatcherException(exception);
            }

            TraceFactory.Logger.Debug("Returning asset details");

            return assetDetails;
        }

        /// <summary>
        /// Starts the enterprise scenario based on the provided configuration.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="assets">The assets.</param>
        public void Stage(string sessionId, AssetDetailCollection assets)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Stage({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Stage(assets));
        }

        /// <summary>
        /// Validates all assets and components used in this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public void Validate(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Validate({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Validate());
        }

        /// <summary>
        /// Signals the dispatcher to validate again.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        public void Revalidate(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Revalidate({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Revalidate());
        }

        /// <summary>
        /// Powers on all assets and components used in this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public void PowerUp(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> PowerUp({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.PowerUp());
        }

        /// <summary>
        /// Powers on all assets and components used in this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public void PowerUp(string sessionId, SessionStartSchedule schedule)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> PowerUp({0}) - Scheduled".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.PowerUp(schedule));
        }

        /// <summary>
        /// Runs the specified session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public void Run(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Run({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Run());
        }

        /// <summary>
        /// Signals the dispatcher to repeat execution of the loaded session.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        public void Repeat(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Repeat({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Repeat());
        }

        /// <summary>
        /// Signals the dispatcher to get the Offline Devices for the loaded session.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        public HashSet<string> GetSessionOfflineDevices(string sessionId)
        {
            HashSet<string> offlineDevices = null;
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> getSessionOfflineDevices({0})".FormatWith(sessionId));
            TraceFactory.Logger.Debug("Offline Devices: ");
            CallSessionProxy(sessionId, (c) => offlineDevices = c.GetSessionOfflineDevices(), false);
            foreach (string id in offlineDevices)
            {
                TraceFactory.Logger.Debug(id);
            }
            return offlineDevices;
        }

        /// <summary>
        /// Signals the dispatcher to set Offline Devices for the loaded session. 
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="onlineDevices">Offline Devices</param>
        public void SetSessionOfflineDevices(string sessionId, HashSet<string> onlineDevices)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> setSessionOfflineDevices({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.SetSessionOfflineDevices(onlineDevices), false);
        }


        /// <summary>
        /// Pauses the session.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        public void Pause(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Pause({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Pause());
        }

        /// <summary>
        /// Resumes execution of a paused session.
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        public void Resume(string sessionId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> Resume({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.Resume());
        }

        /// <summary>
        /// Gets the current state of the session
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <returns>Session State</returns>
        public SessionState GetSessionState(string sessionId)
        {
            SessionState state = SessionState.None;
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug($"CallProxy -> GetSessionState({sessionId})");
            CallSessionProxy(sessionId, c=> state = c.GetSessionState(), false);

            return state;
        }

        /// <summary>
        /// Gets the current state of Session Startup
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Session Startup Transition</returns>
        public SessionStartupTransition GetSessionStartupState(string sessionId)
        {
            SessionStartupTransition startupState = SessionStartupTransition.None;
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug($"CallProxy -> GetSessionState({sessionId})");
            CallSessionProxy(sessionId, c => startupState = c.GetSessionStartupState(), false);

            return startupState;
        }

        /// <summary>
        /// Restarts the designated machine which can help when it gets stuck during startup
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="replaceMachine">if set to <c>true</c> then replace this machine with a new instance.</param>
        public void RestartMachine(string sessionId, string machineName, bool replaceMachine)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> RestartMachine({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.RestartMachine(machineName, replaceMachine));
        }

        /// <summary>
        /// Restarts the specified asset.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="assetId">The asset id.</param>
        public void RestartAsset(string sessionId, string assetId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> RestartAsset({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.RestartAsset(assetId));
        }

        /// <summary>
        /// Pauses execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public void PauseWorker(string sessionId, string userName)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> PauseWorker({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.PauseWorker(userName));
        }

        /// <summary>
        /// Resumes execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public void ResumeWorker(string sessionId, string userName)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> ResumeWorker({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.ResumeWorker(userName));
        }

        /// <summary>
        /// Suspends activity execution to the specified asset.
        /// </summary>
        /// <param name="sessionId">The session unique identifier</param>
        /// <param name="assetId">The asset Id.</param>
        public void TakeAssetOffline(string sessionId, string assetId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> TakeAssetOffline({0}, {1})".FormatWith(sessionId, assetId));
            CallSessionProxy(sessionId, (c) => c.TakeOffline(assetId));
        }

        /// <summary>
        /// Resumes activity execution to the specified asset.
        /// </summary>
        /// <param name="sessionId">The session unique identifier</param>
        /// <param name="assetId">The asset Id.</param>
        public void BringAssetOnline(string sessionId, string assetId)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> BringAssetOnline({0}, {1})".FormatWith(sessionId, assetId));
            CallSessionProxy(sessionId, (c) => c.BringOnline(assetId));
        }

        /// <summary>
        /// Sets the CRC.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="crcOn">if set to <c>true</c> [CRC on].</param>
        public void SetCrc(string sessionId, string assetId, bool crcOn)
        {
            CallSessionProxy(sessionId, (c) => c.SetCrc(sessionId, assetId, crcOn));
        }

        /// <summary>
        /// Halts execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public void HaltWorker(string sessionId, string userName)
        {
            SetTraceSessionContext(sessionId);
            TraceFactory.Logger.Debug("CallProxy -> HaltWorker({0})".FormatWith(sessionId));
            CallSessionProxy(sessionId, (c) => c.HaltWorker(userName));
        }

        /// <summary>
        /// Subscribes the defined listener to dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        public void Subscribe(Uri subscriber)
        {
            TraceFactory.Logger.Debug("{0}".FormatWith(subscriber.AbsoluteUri));

            EventPublisher.Subscribe(subscriber);

            // Tell all active sessions to add this subscriber to their list
            foreach (var controller in _proxyControllers.Values)
            {
                SetTraceSessionContext(controller.SessionId);
                TraceFactory.Logger.Debug("CallProxy -> Subscribe({0})".FormatWith(controller.SessionId));
                controller.Channel.Subscribe(subscriber);
            }
        }

        /// <summary>
        /// Unsubscribes the defined listener from dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        public void Unsubscribe(Uri subscriber)
        {
            TraceFactory.Logger.Debug("Subscriber: {0}".FormatWith(subscriber.AbsoluteUri));

            EventPublisher.Unsubscribe(subscriber);

            foreach (var controller in _proxyControllers.Values)
            {
                SetTraceSessionContext(controller.SessionId);
                TraceFactory.Logger.Debug("CallProxy -> Unsubscribe({0})".FormatWith(controller.SessionId));
                controller.Channel.Unsubscribe(subscriber);
            }
        }

        /// <summary>
        /// Refreshes the subscriber.
        /// </summary>
        /// <param name="subscriber">The requestor.</param>
        public void RefreshSubscriber(Uri subscriber)
        {
            TraceFactory.Logger.Debug("Subscriber: {0}".FormatWith(subscriber.AbsoluteUri));

            foreach (var controller in _proxyControllers.Values)
            {
                SetTraceSessionContext(controller.SessionId);
                TraceFactory.Logger.Debug("CallProxy -> RefreshSubscription({0})".FormatWith(controller.SessionId));
                controller.Channel.RefreshSubscription(subscriber);
            }
        }

        /// <summary>
        /// Checks the subscriber's subscription to see if it's still active.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>
        /// true if the subscription is still active, otherwise false
        /// </returns>
        public bool SubscriptionActive(Uri subscriber)
        {
            return EventPublisher.CheckSubscription(subscriber);
        }

        /// <summary>
        /// Gets the dispatcher log data.
        /// </summary>
        /// <returns>
        /// A string buffer containing the current log data
        /// </returns>
        public string GetSessionLogFiles(string sessionId)
        {
            SetTraceSessionContext(sessionId);

            if (string.IsNullOrEmpty(sessionId))
            {
                return "Session does not exist";
            }

            string logFilePath = string.Empty;
            string pattern = string.Empty;
            if (GlobalSettings.IsDistributedSystem)
            {
                string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                logFilePath = Path.Combine(Path.GetDirectoryName(location), "SessionProxy", "Logs");
                //the log extension was missing previously
                pattern = "SessionProxy-{0}.log".FormatWith(sessionId);
            }
            else
            {
                logFilePath = LogFileReader.DataLogPath();
                pattern = "STBConsole.log";
            }
            StringBuilder builder = new StringBuilder();
            var logFiles = LogFileDataCollection.Create(logFilePath);
            
            foreach (var file in logFiles.Items.Where(x => x.FileName.Equals(pattern)))
            {
               builder.AppendLine(file.FileData);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Shuts down the executing session, including resources, etc.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="options">The shutdown options.</param>
        public void Shutdown(string sessionId, ShutdownOptions options)
        {
            TraceFactory.Logger.Debug("{0}{1}{2}".FormatWith(sessionId, Environment.NewLine, options));
            ShutdownScenarioHandler(sessionId, options);
        }

        /// <summary>
        /// Closes the specified session and releases all associated resources.
        /// </summary>
        /// <param name="sessionId"></param>
        public void Close(string sessionId)
        {
            TraceFactory.Logger.Debug("{0}".FormatWith(sessionId));
            CloseSessionHandler(sessionId);
        }
        /// <summary>
        /// Closes the specified session with specified shutdown options
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="options"></param>
        public void Close(string sessionId, ShutdownOptions options)
        {
            TraceFactory.Logger.Debug("{0}{1}{2}".FormatWith(sessionId, Environment.NewLine, options));
            CloseSessionHandler(sessionId, options);
        }

        /// <summary>
        /// Returns the system database that this service instance is pointing to.
        /// </summary>
        /// <returns></returns>
        public string GetSystemDatabase()
        {
            return GlobalSettings.Items[Setting.EnterpriseTestDatabase];
        }

        /// <summary>
        /// Checks if there any active sessions
        /// </summary>
        /// <returns></returns>
        public bool AreThereActiveSessions()
        {
            if (_proxyControllers == null)
            {
                return false;
            }

            return _proxyControllers.Count > 0;
        }

        /// <summary>
        /// Loads the specified user credential.
        /// </summary>
        /// <param name="credential"></param>
        public void SetUserCredential(UserCredential credential)
        {
            UserManager.CurrentUser = credential;
        }

        #region Session Shutdown Methods

        private void ShutdownScenarioHandler(string sessionId, ShutdownOptions options)
        {
            try
            {
                // If a session exists, proceed with the shutdown, otherwise, just return.
                SessionProxyController controller = null;
                if (_proxyControllers.TryGetValue(sessionId, out controller))
                {
                    _proxyControllers.Remove(controller);
                    TraceFactory.Logger.Debug("Found & removed controller: Cnt: {0} : {1}".FormatWith(_proxyControllers.Count, controller.Endpoint.AbsoluteUri));

                    if (!controller.SessionClosing) //Make sure the session isn't already closing
                    {
                        lock (controller)
                        {
                            controller.SessionClosing = true;
                            ThreadPool.QueueUserWorkItem(t => ShutdownScenario(controller, sessionId, options));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
            }
        }

        private void ShutdownScenario(SessionProxyController controller, string sessionId, ShutdownOptions options)
        {
            try
            {
                SetTraceSessionContext(sessionId);
                TraceFactory.Logger.Debug("{0}: {1}".FormatWith(sessionId, controller.Endpoint.AbsoluteUri));
                if (controller.ProxyProcessStarted)
                {
                    controller.Channel.Shutdown(options);
                }
                else
                {
                    TraceFactory.Logger.Debug("Proxy process not started");
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Warn("Error sending shutdown call, ignoring", ex);
            }
            finally
            {
                controller.Dispose();
            }
        }

        /// <summary>
        /// Sets the session context for tracing
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        private void SetTraceSessionContext(string sessionId)
        {
            TraceFactory.SetSessionContext(sessionId);
        }

        private void CloseSessionHandler(string sessionId, ShutdownOptions options)
        {
            SetTraceSessionContext(sessionId);
            SessionProxyController controller = null;
            if (_proxyControllers.TryGetValue(sessionId, out controller))  //If a session exists, proceed with normal shutdown.  Otherwise reset manually.
            {
                _proxyControllers.Remove(controller);
                TraceFactory.Logger.Debug("Found & removed controller: Count: {0} : {1}".FormatWith(_proxyControllers.Count, controller.Endpoint.AbsoluteUri));

                if (!controller.SessionClosing) //Make sure the session isn't already closing
                {
                    lock (controller)
                    {
                        controller.SessionClosing = true;
                        try
                        {
                            ThreadPool.QueueUserWorkItem(t => ShutdownScenario(controller, sessionId, options));
                        }
                        catch (Exception ex)
                        {
                            TraceFactory.Logger.Error(ex);
                        }
                    }
                }
            }
            else
            {
                TraceFactory.Logger.Debug(string.Format("Closing session {0}", sessionId));

                // Manually reset DomainAccountReservation for all inactive SessionIds
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    List<string> activeSessionIds = context.FrameworkClients.Select(n => n.SessionId).Distinct().Where(n => n != null).ToList();

                    foreach (var reservation in context.DomainAccountReservations)
                    {
                        if (!activeSessionIds.Contains(reservation.SessionId))
                        {
                            context.DomainAccountReservations.Remove(reservation);
                        }
                    }
                    context.SaveChanges();
                }

                try
                {
                    // Parallel process the two actions required to clean up this session, first define
                    // the collection of actions, then spawn them each off in the background and then
                    // wait for them to return.
                    var actions = new Collection<Action>()
                    {
                        () => CleanupAssetHosts(sessionId),
                        () => CleanupResourceHosts(sessionId)
                    };
                    Parallel.ForEach<Action>(actions, a => a());

                    using (DataLogContext context = DbConnect.DataLogContext())
                    {
                        SessionSummary summary = context.DbSessions.First(n => n.SessionId == sessionId);
                        summary.Status = SessionStatus.Aborted.ToString();
                        summary.EndDateTime = DateTime.UtcNow;
                        context.SaveChanges();
                    }

                    UpdateSessionShutdownState(MachineShutdownState.ManualReset, sessionId, ignoreMissing: true);
                    TraceFactory.Logger.Info("Done closing {0}".FormatWith(sessionId));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                }

                // Tell all clients to clean up any session info
                EventPublisher.ReleaseSession(sessionId);
                //Checks for any Virtual worker process which may have not been terminated correctly from the previous runs
                if (GlobalSettings.IsDistributedSystem == false)
                {
                    using (DataLogContext context = DbConnect.DataLogContext())
                    {
                        SessionSummary summary = context.DbSessions.First(n => n.SessionId == sessionId);
                        if(summary.Dispatcher == Environment.MachineName)
                        {
                            KillOrphanedWorkerProcesses(sessionId);
                        }
                    }
                }
            }
        }

        private static void KillOrphanedWorkerProcesses(string sessionId)
        {
            foreach (Process process in Process.GetProcessesByName("SolutionTesterConsole"))
            {
                // The Virtual worker in STB is started via command line by passing instance id and session ID.
                string orphanSessionId = ProcessUtil.GetCommandLine(process)?.Split(' ')[2];
                if (sessionId.Equals(orphanSessionId))
                {
                    TraceFactory.Logger.Debug($"Found orphaned virtual worker with Session ID {sessionId} and Process Id {process.Id}");
                    try
                    {
                        process.Kill();
                        TraceFactory.Logger.Debug($"Succefully Killed process with Session ID {sessionId} and Process Id {process.Id}");

                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Exception occured trying to kill Virtual worker process with session ID {sessionId}", ex);
                    }
                }
            }
        }
	
        private void CloseSessionHandler(string sessionId)
        {
            ShutdownOptions options = new ShutdownOptions()
            {
                AllowWorkerToComplete = false,
                CopyLogs = false,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.RevertToSnapshot
            };
            CloseSessionHandler(sessionId, options);


            //SetTraceSessionContext(sessionId);
            //SessionProxyController controller = null;
            //if (_proxyControllers.TryGetValue(sessionId, out controller))  //If a session exists, proceed with normal shutdown.  Otherwise reset manually.
            //{
            //    _proxyControllers.Remove(controller);
            //    TraceFactory.Logger.Debug("Found & removed controller: Cnt: {0} : {1}".FormatWith(_proxyControllers.Count, controller.Endpoint.AbsoluteUri));

            //    if (!controller.SessionClosing) //Make sure the session isn't already closing
            //    {
            //        lock (controller)
            //        {
            //            controller.SessionClosing = true;
            //            ShutdownOptions options = new ShutdownOptions()
            //            {
            //                AllowWorkerToComplete = false,
            //                CopyLogs = false,
            //                PowerOff = true,
            //                PowerOffOption = VMPowerOffOption.RevertToSnapshot
            //            };

            //            try
            //            {
            //                ThreadPool.QueueUserWorkItem(t => ShutdownScenario(controller, sessionId, options));
            //            }
            //            catch (Exception ex)
            //            {
            //                TraceFactory.Logger.Error(ex);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    TraceFactory.Logger.Debug(string.Format("Closing session {0}", sessionId));

            //    // Manually reset DomainAccountReservation for all inactive SessionIds
            //    using (AssetInventoryContext context = new AssetInventoryContext())
            //    {
            //        List<string> activeSessionIds = VirtualMachineReservation.SelectSessionIds(context).ToList();

            //        foreach (var reservation in context.DomainAccountReservations)
            //        {
            //            if (!activeSessionIds.Contains(reservation.SessionId))
            //            {
            //                context.DeleteObject(reservation);
            //            }
            //        }
            //        context.SaveChanges();
            //    }

            //    try
            //    {
            //        // Parallel process the two actions required to clean up this session, first define
            //        // the collection of actions, then spawn them each off in the background and then
            //        // wait for them to return.
            //        var actions = new Collection<Action>()
            //        {
            //            () => CleanupAssetHosts(sessionId),
            //            () => CleanupResourceHosts(sessionId)
            //        };
            //        Parallel.ForEach<Action>(actions, a => a());

            //        UpdateSessionShutdownState(MachineShutdownState.ManualReset, sessionId, ignoreMissing: true);
            //        TraceFactory.Logger.Info("Done closing {0}".FormatWith(sessionId));
            //    }
            //    catch (Exception ex)
            //    {
            //        TraceFactory.Logger.Error(ex);
            //    }

            //    // Tell all clients to clean up any session info
            //    EventPublisher.ReleaseSession(sessionId);
            //}
        }

        private void UpdateSessionShutdownState(MachineShutdownState state, string sessionId, bool ignoreMissing = false)
        {
            SetTraceSessionContext(sessionId);
            using (var context = DbConnect.DataLogContext())
            {
                SessionSummary session = context.DbSessions.FirstOrDefault(s => s.SessionId == sessionId);
                if (session != null)
                {
                    session.ShutdownState = state.ToString();
                    context.SaveChanges();
                }
                else
                {
                    // If we could not find the requested session, check to see if this method has been called with
                    // the parameters to ignore missing sessions.  If not, throw an exception.
                    if (!ignoreMissing)
                    {
                        throw new ArgumentException($"Session ID {sessionId} not found.");
                    }
                }
            }
        }

        private void CleanupAssetHosts(string sessionId)
        {
            try
            {
                SetTraceSessionContext(sessionId);
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var reservedAssets = context.AssetReservations.Where(n => n.SessionId == sessionId).Select(n => n.AssetId).ToList();

                    // Parallel process each Jedi simulator reserved for this session - shut down the host machine.
                    List<DeviceSimulator> jediSimulators = context.Assets.OfType<DeviceSimulator>().Where(n => reservedAssets.Contains(n.AssetId) && n.SimulatorType == "Jedi").ToList();
                    Parallel.ForEach(jediSimulators, a => CleanupAssetHostsHandler(a));
                }

                AssetReservationManager reservationManager = new AssetReservationManager(DbConnect.AssetInventoryConnectionString, "STF Console");
                reservationManager.ReleaseSessionReservations(sessionId);
            }
            catch (Exception ex)
            {
                EventPublisher.PublishDispatcherException(ex);
            }
        }

        private void CleanupAssetHostsHandler(DeviceSimulator simulatorMachine)
        {
            using (var machine = new ManagedMachine(simulatorMachine.VirtualMachine, ManagedMachineType.WindowsVirtual))
            {
                try
                {
                    MachineStop.Run(machine.Name, () =>
                        {
                            if (machine.IsPoweredOn())
                            {
                                machine.Shutdown(wait: true);
                            }
                        });
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex.Message);
                }
            }
        }

        private void CleanupResourceHosts(string sessionId)
        {
            SetTraceSessionContext(sessionId);

            // Parallel process each machine used by resources in this session.  Revert the machine
            // and release the reservation held for it.
            var machines = VirtualMachine.Select(sessionId: sessionId);

            Parallel.ForEach<VirtualMachine>(machines, m => CleanupResourceHostsHandler(m));

        }

        private void CleanupResourceHostsHandler(VirtualMachine machine)
        {
            using (var managedMachine = new ManagedMachine(machine.Name, EnumUtil.GetByDescription<ManagedMachineType>(machine.MachineType)))
            {
                TraceFactory.Logger.Debug("Resetting resource host {0}".FormatWith(managedMachine));

                try
                {
                    MachineStop.Run(managedMachine.Name, () =>
                        {
                            managedMachine.Revert(wait: true);
                            managedMachine.ReleaseReservation();
                        });
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                }
            }
        }

        #endregion

        #region Calling Session Proxy Process

        private void CallSessionProxy(string sessionId, Action<ISessionProxy> action, bool useThread = true)
        {
            try
            {
                if (useThread)
                {
                    ThreadPool.QueueUserWorkItem(t => DoActionHandler(sessionId, action));
                }
                else
                {
                    DoActionHandler(sessionId, action);
                }
            }
            catch (Exception ex)
            {
                var msg = "Error for session {0} : {1}".FormatWith(sessionId, ex.Message);
                var exception = new ApplicationException(msg, ex);
                EventPublisher.PublishDispatcherException(exception);
            }
        }

        private void DoActionHandler(string sessionId, Action<ISessionProxy> action)
        {
            if (!_proxyControllers.Contains(sessionId))
            {
                var msg = "Proxy not found for Session Id {0}".FormatWith(sessionId);
                TraceFactory.Logger.Debug(msg);
                throw new EndpointNotFoundException(msg);
            }

            var controller = _proxyControllers[sessionId];

            TraceFactory.Logger.Debug("{0}: {1}".FormatWith(action.Method.Name, controller.Endpoint.AbsoluteUri));

            if (controller.ProxyProcessStarted)
            {
                Action retryAction = () => action(controller.Channel);

                try
                {
                    Retry.WhileThrowing
                        (
                            retryAction,
                            10,
                            TimeSpan.FromSeconds(5),
                            new List<Type>() { typeof(EndpointNotFoundException) }
                        );
                }
                catch (Exception ex)
                {
                    var msg = "Error communicating with session proxy for {0} - 2".FormatWith(sessionId);
                    var exception = new ApplicationException(msg);
                    TraceFactory.Logger.Error(msg, ex);
                    EventPublisher.PublishDispatcherException(exception);
                }
            }
            else
            {
                var msg = "Proxy service not started for session {0} (2)".FormatWith(sessionId);
                var exception = new ApplicationException(msg);
                TraceFactory.Logger.Error(msg);
                EventPublisher.PublishDispatcherException(exception);
            }
        }

        #endregion

        #region Saving and Loading Session Data

        void OnDispatcherProcessExit(object sender, EventArgs e)
        {
            // Save any currently active session data.  This event should
            // occur even during an unhandled exception as the default handler
            // for unhandled exceptions will exit gracefully.
            // SessionDataSave();
        }

        /// <summary>
        /// Saves the session data to a local cache.
        /// </summary>
        public void SessionDataSave()
        {
            TraceFactory.Logger.Debug("Saving proxy references and subscribers");
            EventPublisher.SaveSubscriberData();

            // Attempt to dump a cache file of the currently active sessions
            try
            {
                if (File.Exists(_backupFile))
                {
                    File.Delete(_backupFile);
                    TraceFactory.Logger.Debug("deleted {0}".FormatWith(_backupFile));
                }

                if (_proxyControllers.SessionIds.Count() > 0)
                {
                    File.WriteAllText(_backupFile, LegacySerializer.SerializeDataContract(_proxyControllers).ToString());
                    TraceFactory.Logger.Debug("Wrote out {0} proxy entries".FormatWith(_proxyControllers.SessionIds.Count()));
                }
                else
                {
                    TraceFactory.Logger.Debug("Nothing to save...");
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Failed to write cache file", ex);
            }
        }

        /// <summary>
        /// Loads the session data from a local cache
        /// </summary>
        public void SessionDataLoad()
        {
            TraceFactory.Logger.Debug("Loading proxy references and subscribers");
            EventPublisher.LoadSubscriberData();

            // If there is a proxy entries cache file, load it and process it.
            if (File.Exists(_backupFile))
            {
                TraceFactory.Logger.Debug("Cache file exists, loading");
                var proxies = LegacySerializer.DeserializeDataContract<SessionProxyControllerSet>(File.ReadAllText(_backupFile));

                // If the number of entries is greater than the max, then we need to adjust
                // the max to account for all entries.
                if (proxies.Count > _proxyControllers.Maximum)
                {
                    _proxyControllers = new SessionProxyControllerSet(proxies.Count);
                }

                // Create a new entry in the proxie entries and then tell the proxy
                // entry to refresh all subscribers.
                foreach (var proxy in proxies.Values)
                {
                    try
                    {
                        proxy.Channel.Ping();
                        _proxyControllers.Add(proxy);
                        TraceFactory.Logger.Debug("Endpoint reloaded for {0}".FormatWith(proxy.SessionId));
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        TraceFactory.Logger.Debug("Endpoint skipped for {0}:{1}".FormatWith(proxy.SessionId, ex.Message));
                    }
                }

                try
                {
                    File.Delete(_backupFile);
                    TraceFactory.Logger.Debug("deleted {0}".FormatWith(_backupFile));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Error deleting saved proxies file", ex);
                }
            }
            else
            {
                TraceFactory.Logger.Debug("Nothing to load...");
            }
        }

        #endregion
    }
}
