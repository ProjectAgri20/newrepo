using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using System.Net;
using System.Net.Sockets;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used by any dispatcher client to communicate with the dispatcher to manage
    /// the execution of a test scenario.  Also contains events that can be used by
    /// the client to receive notifications returned from the dispatcher and session proxy.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SessionClient : ISessionClient
    {
        private static readonly SessionClient _instance = new SessionClient();

        private event EventHandler<SessionStartupTransitionEventArgs> _startupTransitionReceived;
        private event EventHandler<SessionStateEventArgs> _sessionStateReceived;
        private event EventHandler<SessionMapElementEventArgs> _mapElementReceived;
        private event EventHandler<ExceptionDetailEventArgs> _dispatcherExceptionReceived;
        private event EventHandler<SessionIdEventArgs> _clearSessionRequestReceived;

        private Uri _myCallbackEndpoint = null;
        private DateTime _lastUpdate = DateTime.MinValue;
        private Thread _keepSubscriptionAliveThread = null;
        private ServiceHost _myCallbackService = null;
        private string _dispatcherHostName = string.Empty;
        private readonly object _lock = new object();
        private SessionState _lastState = SessionState.Available;

        private SessionMapElement _lastSessionElement = null;
        private SessionMapElement LastSessionMapElement
        {
            get { return _lastSessionElement; }
            set
            {
                if (_lastSessionElement != value)
                {
                    _lastSessionElement = value;
                    SetSessionContext(_lastSessionElement);
                }
            }
        }

        private string _lastSessionId = string.Empty;
        private string LastSessionId
        {
            get { return _lastSessionId; }
            set
            {
                if (_lastSessionId != value)
                {
                    _lastSessionId = value;
                    TraceFactory.SetSessionContext(_lastSessionId);
                }
            }
        }

        private void SetSessionContext(SessionMapElement element)
        {
            string sessionId = null;
            string elementId = null;

            if (element != null)
            {
                sessionId = element.SessionId;
                elementId = element.Id.ToString();
            }
            TraceFactory.SetSessionContext(sessionId);
            TraceFactory.SetThreadContextProperty("SessionMapElementId", elementId, false);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SessionClient"/> class from being created.
        /// </summary>
        private SessionClient()
        {
        }

        /// <summary>
        /// Singleton.
        /// </summary>
        public static SessionClient Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes this management client using the specified hostname.
        /// </summary>
        /// <param name="dispatcherHostName">New name of the dispatcher host.</param>
        /// <exception cref="System.ArgumentNullException">dispatcherHostName</exception>
        public void Initialize(string dispatcherHostName)
        {
            if (string.IsNullOrEmpty(dispatcherHostName))
            {
                throw new ArgumentNullException("dispatcherHostName");
            }
            
            lock (_lock)
            {
                if (!MyCallbackServiceIsOpen)
                {
                    StartCallbackService();
                }

                if (GlobalSettings.IsDistributedSystem)
                {
                    // If the dispatcher name being passed in is different then a few things need to be 
                    // done to update where this client subscribes for events.
                    if (dispatcherHostName != _dispatcherHostName)
                    {
                        if (_keepSubscriptionAliveThread != null)
                        {
                            // This client is already subscribed to some dispatcher, so cancel that subscription
                            // using the current hostname.  This has to be done before the new hostname is set.
                            using (var connection = GetConnection())
                            {
                                connection.Channel.Unsubscribe(_myCallbackEndpoint);
                                TraceFactory.Logger.Debug("Unsubscribed from {0}".FormatWith(_dispatcherHostName));
                            }

                            _dispatcherHostName = dispatcherHostName;
                        }
                        else
                        {
                            _dispatcherHostName = dispatcherHostName;

                            //_keepSubscriptionAliveThread = new Thread(new ThreadStart(KeepDispatcherSubscriptionAlive));
                            //_keepSubscriptionAliveThread.IsBackground = true;
                            //_keepSubscriptionAliveThread.Start();
                            TraceFactory.Logger.Debug("Started new thread to keep server subscription current");
                        }

                        // Subscribe to new dispatcher
                        using (var connection = GetConnection())
                        {
                            connection.Channel.Subscribe(_myCallbackEndpoint);
                            //connection.Channel.RefreshSubscriber(_myCallbackEndpoint);
                            TraceFactory.Logger.Debug("Subscribed to {0}".FormatWith(_dispatcherHostName));
                        }
                    }
                }
                else
                {
                    _dispatcherHostName = dispatcherHostName;
                    _myCallbackEndpoint = CreateCallbackEndpoint(0);
                    // Subscribe to new dispatcher
                    using (var connection = GetConnection())
                    {
                        connection.Channel.Subscribe(_myCallbackEndpoint);
                        TraceFactory.Logger.Debug("Subscribed to {0}".FormatWith(_dispatcherHostName));
                    }
                }
            }
        }

        #region Events

        /// <summary>
        /// Occurs when a runtime status changes
        /// </summary>
        public event EventHandler<SessionStartupTransitionEventArgs> SessionStartupTransitionReceived
        {
            add { _startupTransitionReceived += value; }
            remove { _startupTransitionReceived -= value; }
        }

        /// <summary>
        /// Occurs when a runtime status changes
        /// </summary>
        public event EventHandler<SessionMapElementEventArgs> SessionMapElementReceived
        {
            add { _mapElementReceived += value; }
            remove { _mapElementReceived -= value; }
        }

        /// <summary>
        /// Occurs when there is a general error with the dispatcher
        /// </summary>
        public event EventHandler<ExceptionDetailEventArgs> DispatcherExceptionReceived
        {
            add { _dispatcherExceptionReceived += value; }
            remove { _dispatcherExceptionReceived -= value; }
        }

        /// <summary>
        /// Occurs when the dispatcher state changes.
        /// </summary>
        public event EventHandler<SessionStateEventArgs> SessionStateReceived
        {
            add { _sessionStateReceived += value; }
            remove { _sessionStateReceived -= value; }
        }

        /// <summary>
        /// Occurs when the dispatcher requests that a session be cleared from the client
        /// </summary>
        public event EventHandler<SessionIdEventArgs> ClearSessionRequestReceived
        {
            add { _clearSessionRequestReceived += value; }
            remove { _clearSessionRequestReceived -= value; }
        }

        #endregion

        #region WCF Service Management

        /// <summary>
        /// States if this service is open or not
        /// </summary>
        public bool MyCallbackServiceIsOpen
        {
            get { return _myCallbackService != null && _myCallbackService.State == CommunicationState.Opened; }
        }

        private void StartCallbackService()
        {
            // Try to create a service endpoint and start it.  Because multiple users can run on the same
            // host, there is a need to retry this process until an available endpoint is discovered
            int attempts = 0;
            Action action = () =>
            {
                _myCallbackEndpoint = CreateCallbackEndpoint(attempts++);

                _myCallbackService = new WcfHost<ISessionClient>
                    (
                        this,
                        MessageTransferType.CompressedHttp,
                        _myCallbackEndpoint
                    );

                _myCallbackService.Open();
            };

            Retry.WhileThrowing(action, 100, TimeSpan.Zero, new List<Type>() { typeof(AddressAlreadyInUseException), typeof(InvalidOperationException) });

            TraceFactory.Logger.Debug("Started: {0}".FormatWith(_myCallbackEndpoint.AbsoluteUri));
        }

        private Uri CreateCallbackEndpoint(int index)
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName()).Where(n => n.AddressFamily == AddressFamily.InterNetwork);
            IPAddress local = addresses.FirstOrDefault(n => n.IsRoutable());
            if (local == null)
            {
                // No routable addresses available - pick one on the private network as long as it is not a loopback.
                local = addresses.FirstOrDefault(n => !IPAddress.IsLoopback(n));
            }

            var path = "http://{0}:{1}/SessionClient".FormatWith
                (
                    local.ToString(),
                    (int)WcfService.SessionClient + (index)
                );
            return new Uri(path);
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void Stop()
        {
            if (!MyCallbackServiceIsOpen)
            {
                return;
            }

            if (_keepSubscriptionAliveThread != null && _keepSubscriptionAliveThread.IsAlive)
            {
                _keepSubscriptionAliveThread.Abort();
                _keepSubscriptionAliveThread = null;
            }

            using (var client = GetConnection())
            {
                try
                {
                    TraceFactory.Logger.Debug("Unsubscribing from dispatcher: {0}".FormatWith(_myCallbackEndpoint.AbsoluteUri));
                    client.Channel.Unsubscribe(_myCallbackEndpoint);
                }
                catch (EndpointNotFoundException)
                {
                    // The dispatcher may not be running.  Don't do anything.
                }
            }

            _myCallbackEndpoint = null;
            _lastUpdate = DateTime.MinValue;
            _myCallbackService = null;
            _dispatcherHostName = string.Empty;
            _lastState = SessionState.Available;
            LastSessionMapElement = null;

            //PublishSessionMapElement(LastSessionMapElement);
        }

        /// <summary>
        /// Refresh the listener and send updated information to the listener
        /// </summary>
        public void Refresh()
        {
            CallDispatcher((c) => c.RefreshSubscriber(_myCallbackEndpoint));
        }

        /// <summary>
        /// Keeps the dispatch registration alive by re-registering with the dispatcher at set intervals.
        /// </summary>
        private void KeepDispatcherSubscriptionAlive()
        {
            TimeSpan delay = TimeSpan.FromMinutes(1);
            bool inFailedState = false;

            while (true)
            {
                Thread.Sleep(delay);

                // Check to see if we've heard from the dispatcher recently.
                if ((DateTime.Now - _lastUpdate) > delay)
                {
                    using (var connection = GetConnection())
                    {
                        try
                        {                            
                            // Check to verify that the subscription is still active.  If not
                            // try to resubscribe and update the status.  If the connection fails
                            // then the dispatcher state will be put in unavailable.
                            bool active = connection.Channel.SubscriptionActive(_myCallbackEndpoint);

                            if (!active)
                            {
                                connection.Channel.Subscribe(_myCallbackEndpoint);
                                connection.Channel.RefreshSubscriber(_myCallbackEndpoint);
                                TraceFactory.Logger.Debug("Subscription was not active, re-subscribed and refreshed");
                            }

                            if (inFailedState)
                            {
                                // Even if the subscription is not active, this indicates that it is in 
                                // a failed state, but now connection to the dispatcher is back, so reset
                                // things appropriately.
                                TraceFactory.Logger.Debug("Dispatcher is available again.");
                                inFailedState = false;
                                delay = TimeSpan.FromMinutes(1);

                                if (active)
                                {
                                    // Publish this available state in case there are no sessions currently running.
                                    PublishSessionState(SessionState.Available, LastSessionId, string.Empty);
                                    connection.Channel.RefreshSubscriber(_myCallbackEndpoint);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceFactory.Logger.Error(ex.Message);

                            if (!inFailedState)
                            {
                                string message = "The Dispatcher is not responding: {0}: {1}".FormatWith(ex.GetType(), ex.Message);
                                TraceFactory.Logger.Error(message);
                                inFailedState = true;
                                delay = TimeSpan.FromSeconds(5);

                                PublishSessionState(SessionState.Unavailable, LastSessionId, message);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region ISessionClient handlers

        /// <summary>
        /// Updates the state of the component.
        /// </summary>
        /// <param name="element">The state.</param>
        public void PublishSessionMapElement(SessionMapElement element)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(t => PublishSessionMapElementHandler(element));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Exception", ex);
                throw;
            }
        }

        private void PublishSessionMapElementHandler(SessionMapElement element)
        {
            _lastUpdate = DateTime.Now;
            LastSessionMapElement = element;
            if (_mapElementReceived != null)
            {
                _mapElementReceived(this, new SessionMapElementEventArgs(element));
            }
        }

        /// <summary>
        /// Publishes an error recieved from the dispatcher
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="detail">The error details.</param>
        public void PublishDispatcherException(string message, string detail)
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                // Only fire if there is a message to fire
                if (message != null && _dispatcherExceptionReceived != null)
                {
                    _dispatcherExceptionReceived(this, new ExceptionDetailEventArgs(message, detail));
                }
            });
        }

        /// <summary>
        /// Requests the listening client to clear information on the specified session
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void ClearSession(string sessionId)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(t =>
                {
                    if (_clearSessionRequestReceived != null)
                    {
                        _clearSessionRequestReceived(this, new SessionIdEventArgs(sessionId));
                    }
                });
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Exception", ex);
                throw;
            }
        }

        /// <summary>
        /// Fires an event that says the dispatcher is ready to move to the next state
        /// during session startup.  This is used to queue the listener to call the
        /// next transition transition.
        /// </summary>
        /// <param name="transition">The startup transition.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        public void PublishSessionStartupTransition(SessionStartupTransition transition, string sessionId)
        {
            try
            {
                LastSessionId = sessionId;

                ThreadPool.QueueUserWorkItem(t =>
                {
                    _lastUpdate = DateTime.Now;
                    if (_startupTransitionReceived != null)
                    {
                        _startupTransitionReceived(this, new SessionStartupTransitionEventArgs(transition, sessionId));
                    }
                });
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Exception", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the state of the dispatcher.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="message">Additional text about the session state.</param>
        public void PublishSessionState(SessionState state, string sessionId, string message)
        {
            try
            {
                LastSessionId = sessionId;
                _lastState = state;

                ThreadPool.QueueUserWorkItem(t =>
                {
                    _lastUpdate = DateTime.Now;
                    if (_sessionStateReceived != null)
                    {
                        TraceFactory.Logger.Debug("State: {0}".FormatWith(state));
                        _sessionStateReceived(this, new SessionStateEventArgs(state, sessionId, message));
                    }
                });
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Exception", ex);
                throw;
            }
        }

        /// <summary>
        /// Test to see if the listener service is still alive.
        /// </summary>
        public void Ping()
        {
            TraceFactory.Logger.Debug($"Callback service '{_myCallbackEndpoint}' is open: {MyCallbackServiceIsOpen}");
        }

        #endregion

        #region Calls to SessionManagementService

        /// <summary>
        /// Initiates a new session using the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        public void InitiateSession(SessionTicket ticket)
        {
            if (_myCallbackService.State != CommunicationState.Opened)
            {
                StartCallbackService();
            }

            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(ticket.SessionId));
            CallDispatcher((c) => c.Initiate(ticket, _myCallbackEndpoint));
        }

        /// <summary>
        /// Reserves all assets associated with this session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns>
        /// An updated ticket that contains information on asset reservations
        /// </returns>
        public AssetDetailCollection Reserve(string sessionId)
        {
            return Reserve(sessionId, string.Empty);
        }

        /// <summary>
        /// Reserves all assets associated with this session using the specified key.
        /// If the reservationKey is a comma separated list, will process multiple values but only 
        /// returns one Asset ID in the collection with priority being Available.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="reservationKey">The reservation key.</param>
        /// <returns>AssetDetailCollection</returns>
        public AssetDetailCollection Reserve(string sessionId, string reservationKey)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, Key: {1}".FormatWith(sessionId, reservationKey));

            AssetDetailCollection scenarioAssets = new AssetDetailCollection();
            List<string> rkList = new List<string> { reservationKey };
                
            if(reservationKey.IndexOf(',') > 0)
            {
                rkList = reservationKey.Split(',').Select(r => r.Trim()).ToList();
            }

            foreach(string rk in rkList)
            {
                AssetDetailCollection adcTemp = GetConnection().Channel.Reserve(sessionId, rk);

                if(scenarioAssets.Count.Equals(0))
                {
                    scenarioAssets = adcTemp;
                }
                else
                {
                    foreach(AssetDetail ad in adcTemp)
                    {
                        if (!FoundAssetDetail(scenarioAssets, ad))
                        {
                            scenarioAssets.Add(ad);
                        }
                    }
                }
            }

            TraceFactory.Logger.Debug("Returning...");
            return scenarioAssets;
        }

        /// <summary>
        /// adc is the active list. Returns true if the AssetDetail is already in the list. Will
        /// check to see if the asset detail is active or not. If not active in list and the given is active,
        /// will reset the list.
        /// </summary>
        /// <param name="adc">AssetDetailCollection</param>
        /// <param name="ad">AssetDetail</param>
        /// <returns>bool</returns>
        private bool FoundAssetDetail(AssetDetailCollection adc, AssetDetail ad)
        {
            bool bFound = false;

            foreach (AssetDetail item in adc)
            {
                if(item.AssetId.Equals( ad.AssetId))
                {
                    bFound = true;
                    if (!AssetAvailable(item) && AssetAvailable(ad))
                    {
                        // ID's and descriptions are the same so just reset the availability information for the device in the primary list.
                        item.Availability = ad.Availability;
                        item.AvailabilityStartTime = ad.AvailabilityStartTime;
                        item.AvailabilityEndTime = ad.AvailabilityEndTime;

                        break;
                    }
                }
            }

            return bFound;
        }

        private bool AssetAvailable(AssetDetail assetDetail)
        {
            return assetDetail.Availability.Equals(AssetAvailability.Available) || assetDetail.Availability.Equals(AssetAvailability.PartiallyAvailable);
        }

        private void CallDispatcher(Action<ISessionDispatcher> action)
        {
            Action retryAction = () => action(GetConnection().Channel);

            Retry.WhileThrowing
                (
                    retryAction,
                    5,
                    TimeSpan.FromSeconds(30),
                    new List<Type>() { typeof(EndpointNotFoundException), typeof(TimeoutException) }
                );
        }

        /// <summary>
        /// Starts the enterprise scenario based on the provided configuration.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="assets">The assets.</param>
        public void Stage(string sessionId, AssetDetailCollection assets)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Stage(sessionId, assets));
        }

        /// <summary>
        /// Signals the dispatcher to initialize again.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Validate(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Validate(sessionId));
        }

        /// <summary>
        /// Signals the dispatcher to initialize again.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Revalidate(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Revalidate(sessionId));
        }

        /// <summary>
        /// Signals the dispatcher to power up the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void PowerUp(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.PowerUp(sessionId));
        }

        /// <summary>
        /// Signals the dispatcher to power up the session.
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="schedule">The Startup schedule which defines a delay in starup.</param>
        public void PowerUp(string sessionId, SessionStartSchedule schedule)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.PowerUp(sessionId, schedule));
        }

        /// <summary>
        /// Runs the specified session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Run(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Run(sessionId));
        }

        /// <summary>
        /// Signals the dispatcher to repeat execution of the loaded session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Repeat(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Repeat(sessionId));
        }

        /// <summary>
        /// Signals the dispatcher to get offline devices of the loaded session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public HashSet<string> GetSessionOfflineDevices(string sessionId)
        {
            HashSet<string> offlineDevices = null;
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            TraceFactory.Logger.Debug("Offline Devices: ");
            CallDispatcher((c) => offlineDevices = c.GetSessionOfflineDevices(sessionId));
            foreach (string id in offlineDevices)
            {
                TraceFactory.Logger.Debug(id);
            }
            return offlineDevices;
        }

        /// <summary>
        /// Signals the dispatcher to set the offline devices of the loaded session. 
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="onlineDevices">Offline Devices</param>
        public void SetSessionOfflineDevices(string sessionId, HashSet<string> onlineDevices)
        {
            CallDispatcher((c) => c.SetSessionOfflineDevices(sessionId, onlineDevices));
        }

        /// <summary>
        /// Shuts down the executing session, including resources, etc.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="options">The options.</param>
        public void Shutdown(string sessionId, ShutdownOptions options)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Shutdown(sessionId, options));
        }

        /// <summary>
        /// Pauses the session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Pause(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Pause(sessionId));
        }

        /// <summary>
        /// Resumes execution of a paused session.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Resume(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Resume(sessionId));
        }

        /// <summary>
        /// Gets the current state of the session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public SessionState GetSessionState(string sessionId)
        {
            SessionState state = SessionState.None;
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher(c=>state= c.GetSessionState(sessionId));
            return state;
        }

        /// <summary>
        /// Gets the current Session Startup state
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public SessionStartupTransition GetSessionStartupState(string sessionId)
        {
            SessionStartupTransition state = SessionStartupTransition.None;
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher(c => state = c.GetSessionStartupState(sessionId));
            return state;
        }

        /// <summary>
        /// Gets the dispatcher log data.
        /// </summary>
        /// <returns>
        /// A string buffer containing the current log data
        /// </returns>
        public string GetLogData(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            return GetConnection().Channel.GetSessionLogFiles(sessionId);
        }

        /// <summary>
        /// Closes the specified session and releases all associated resources
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public void Close(string sessionId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Close(sessionId));
        }

        /// <summary>
        /// Closes the specified session with given Shutdown Options
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="options"></param>
        public void Close(string sessionId, ShutdownOptions options)
        {
            TraceFactory.Logger.Debug("SessionId: {0}".FormatWith(sessionId));
            CallDispatcher((c) => c.Close(sessionId, options));
        }

        /// <summary>
        /// Restarts the designated machine which can help when it gets stuck during startup
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="replaceMachine">if set to <c>true</c> then replace this machine with a new instance.</param>
        public void RestartMachine(string sessionId, string machineName, bool replaceMachine)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, Host: {1}".FormatWith(sessionId, machineName));
            CallDispatcher((c) => c.RestartMachine(sessionId, machineName, replaceMachine));
        }

        /// <summary>
        /// Restarts the specified asset.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="assetId">The asset id.</param>
        public void RestartAsset(string sessionId, string assetId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, AssetId: {1}".FormatWith(sessionId, assetId));
            CallDispatcher((c) => c.RestartAsset(sessionId, assetId));
        }

        /// <summary>
        /// Pauses execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="userName">The username of the worker to be paused</param>
        public void PauseWorker(string sessionId, string userName)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, User: {1}".FormatWith(sessionId, userName));
            CallDispatcher((c) => c.PauseWorker(sessionId, userName));
        }

        /// <summary>
        /// Resumes execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="userName">The username of the worker to be resumed</param>
        public void ResumeWorker(string sessionId, string userName)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, User: {1}".FormatWith(sessionId, userName));
            CallDispatcher((c) => c.ResumeWorker(sessionId, userName));
        }

        /// <summary>
        /// Halts execution for a single virtual resource.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="userName">The username of the worker to be halted</param>
        public void HaltWorker(string sessionId, string userName)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, User: {1}".FormatWith(sessionId, userName));
            CallDispatcher((c) => c.HaltWorker(sessionId, userName));
        }

        /// <summary>
        /// Suspends activity execution to the specified asset.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="assetId"></param>
        public void TakeAssetOffline(string sessionId, string assetId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, AssetId: {1}".FormatWith(sessionId, assetId));
            CallDispatcher((c) => c.TakeAssetOffline(sessionId, assetId));
        }

        /// <summary>
        /// Resumes activity execution to the specified asset.
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="assetId">The asset Id.</param>
        public void BringAssetOnline(string sessionId, string assetId)
        {
            TraceFactory.Logger.Debug("SessionId: {0}, AssetId: {1}".FormatWith(sessionId, assetId));
            CallDispatcher((c) => c.BringAssetOnline(sessionId, assetId));
        }

        /// <summary>
        /// Sets device CRC mode (paperless) on or off.
        /// </summary>
        /// <param name="sessionId">The Session Id.</param>
        /// <param name="assetId">The Id of the device.</param>
        /// <param name="crcOn">true to turn CRC on, false to turn it off.</param>
        public void SetCrc(string sessionId, string assetId, bool crcOn)
        {
            TraceFactory.Logger.Debug("AssetId: {0}  CRC On: {1}".FormatWith(assetId, crcOn));
            CallDispatcher((c) => c.SetCrc(sessionId, assetId, crcOn));
        }

        /// <summary>
        /// Loads the specified user credential.
        /// </summary>
        /// <param name="credential"></param>
        public void SetUserCredential(UserCredential credential)
        {
            CallDispatcher(c => c.SetUserCredential(credential));
        }

        #endregion

        private SessionDispatcherConnection GetConnection()
        {
            return SessionDispatcherConnection.Create(_dispatcherHostName);
        }
    }
}

