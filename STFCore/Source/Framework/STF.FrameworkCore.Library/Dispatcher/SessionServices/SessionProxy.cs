using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Core framework class that represents the configuration and execution of a scalable test session.  It
    /// includes all methods needed to configure maps of components and automation that are used
    /// to define all parts of the test.  This class runs within a secondary process that is started by
    /// the dispatcher.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SessionProxy : ISessionMapElement, ISessionProxy, ISessionProxyBackend, IDisposable
    {
        private ServiceHost _sessionFrontendService = null;
        private ServiceHost _sessionBackendService = null;
        private bool _firstCallToReserve = true;
        private SessionState _runtimeState = SessionState.Available;
        private SessionStartupTransition _startupStep = SessionStartupTransition.None;
        private Uri _ownerCallback = null;
        private readonly object _lock = new object();
        private Timer _healthCheckTimer = null;
        private SessionStartSchedule _startSchedule = null;
        private System.Timers.Timer _powerUpTimer = null;
        private System.Timers.Timer _runTimer = null;

        private ClientController.EventLogCollector.EventLogCollector _eventLogCollector = null;
        private SystemManifestAgentDictionary _manifestAgents = null;
        private string _sessionId = string.Empty;
        private int _currentScenarioIndex = 0;
        private FailNotification _failNotifier = null;
        private AssetReservationManager _reservationManager;

        private static HashSet<string> _offlineDevices = new HashSet<string>();
        private readonly Dictionary<string, Timer> _offlineDeviceRetryTimers = new Dictionary<string, Timer>();

        /// <summary>
        /// Occurs when the session proxy is closed.
        /// </summary>
        public event EventHandler OnExit;

        /// <summary>
        /// Gets the event.
        /// </summary>
        public SessionProxyEventPublisher EventPublisher { get; private set; }

        /// <summary>
        /// Gets the session ticket.
        /// </summary>
        public SessionTicket Ticket { get; private set; }

        /// <summary>
        /// Gets the assets associated with this session.
        /// </summary>
        public AssetDetailCollection Assets { get; private set; }

        /// <summary>
        /// Gets the <see cref="SessionMapElement"/> for this item.
        /// </summary>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the collection of session maps objects that define all the resources and assets
        /// being used for this test session.
        /// </summary>
        [SessionMapElementCollection]
        public Collection<SessionMapObject> SessionMaps { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProxy"/> class.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionProxy(string sessionId)
        {
            _sessionId = sessionId;

            TraceFactory.SetSessionContext(sessionId);
            EventPublisher = new SessionProxyEventPublisher(sessionId);
            SessionMaps = new Collection<SessionMapObject>();
            Assets = new AssetDetailCollection();
            _healthCheckTimer = new Timer(HealthCheckTimer_Elapsed);
            _reservationManager = new AssetReservationManager(DbConnect.AssetInventoryConnectionString, "STF Console");
        }

        private void PublishSessionState(SessionState state, string message)
        {
            _runtimeState = state;
            EventPublisher.PublishSessionState(state, Ticket.SessionId, message);
        }

        private void PublishSessionState(SessionState state)
        {
            if (state != _runtimeState)
            {
                _runtimeState = state;
                EventPublisher.PublishSessionState(state, Ticket.SessionId);
            }
        }

        /// <summary>
        /// Gets or sets the current <see cref="StartupStep"/>.
        /// </summary>
        public SessionStartupTransition StartupStep
        {
            get { return _startupStep; }
            set
            {
                if (_startupStep != value)
                {
                    _startupStep = value;
                    EventPublisher.PublishSessionStartupTransition(value, Ticket.SessionId, _ownerCallback);
                }
            }
        }

        private void OnResourceMapCompleted(object sender, EventArgs e)
        {
            // Because all the resources are completed, tell the Asset map to also mark
            // all assets and print queues used in the test as complete.
            
            Parallel.ForEach<AssetMap>(SessionMaps.OfType<AssetMap>(), m => m.Finished());
            Parallel.ForEach<RemotePrintQueueMap>(SessionMaps.OfType<RemotePrintQueueMap>(), m => m.Finished());

            // Finds the EventLog and PerfMon Collectors and shuts them down so that the test can be completed, it is threaded so that other activities can finish
            Task.Factory.StartNew(() =>
                {
                    ShutdownCollectors();
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default
            );

            // Stop the health check
            ToggleHealthCheck(false);

            TraceFactory.Logger.Debug($"Completed scenario {Ticket.ScenarioIds.ElementAt(_currentScenarioIndex)}.");

            if (_currentScenarioIndex < Ticket.ScenarioIds.Count() - 1)
            {
                ShutdownOptions options = new ShutdownOptions()
                {
                    AllowWorkerToComplete = false,
                    CopyLogs = false,
                    PowerOff = true,
                    PowerOffOption = VMPowerOffOption.PowerOff,
                    PerformCleanup = false
                };

                // From here on we have to thread everything so this event can finish. 
                // Otherwise the users trying to contact the server will get WCF Fault exceptions.
                Task.Factory.StartNew(() => ShutdownScenario(options));
                Task.Factory.StartNew(() => Reset());
                return;
            }

            // Session is complete
            LogSessionStatus(SessionStatus.Complete);
            PublishSessionState(SessionState.RunComplete);
            MapElement.UpdateStatus("Completed", RuntimeState.Completed);
        }

        private void ShutdownCollectors()
        {
            ShutdownOptions options = new ShutdownOptions();
            options.PowerOff = false;
            options.ReleaseDeviceReservation = false;

            // Find all of the hosts running EventLog and PerfMon collectors and shut them down (without turning off the power) in order to allow the session to complete
            Parallel.ForEach<ResourceHost>(SessionMaps.OfType<ResourceMap>().SelectMany(x => x.Hosts).Where(h => h.Resources.Any(i => i.Detail.ResourceType.RunsIndefinitely())), (h, l) =>
            {
                h.Shutdown(options, l);
            });
        }

        /// <summary>
        /// Waits for all SessionMaps to go a state of Offline which doesn't happen until all the client processes
        /// have "called in" that they have finished shutting down.
        /// This method is used in favor of SpinWait.SpinUntil() because of the length of time this could spend waiting.
        /// SpinWait.SpinUntil is only efficient when waiting less than 20,000 CPU cycles.
        /// </summary>
        private void WaitForOffline(int timeoutInSeconds)
        {
            const int interval = 3;
            IEnumerable<SessionMapObject> waitingFor = null;
            StringBuilder message = new StringBuilder();
            int i = 0;
            do
            {
                message.Clear();
                waitingFor = SessionMapElement.GetElementsNotSetTo(RuntimeState.Offline, SessionMaps);
                foreach (SessionMapObject element in waitingFor)
                {
                    if (message.Length > 0)
                    {
                        message.Append(", ");
                    }
                    message.Append(element.MapElement.Name);
                }

                if (message.Length > 0)
                {
                    TraceFactory.Logger.Debug($"Waiting for: {message}");
                    Thread.Sleep(TimeSpan.FromSeconds(interval));
                    i += interval;
                }
            } 
            while (message.Length > 0 && i <= timeoutInSeconds);

            if (message.Length == 0)
            {
                TraceFactory.Logger.Debug("All SessionMaps are now offline.");
            }

            if (i >= timeoutInSeconds)
            {
                TraceFactory.Logger.Debug("Timeout exceeded waiting for SessionMaps to go offline.");
            }

        }

        /// <summary>
        /// Resets the SessionProxy for execution of a different scenario while maintaining the same SessionId.
        /// </summary>
        private void Reset()
        {
            Thread.CurrentThread.SetName($"Reset-{Thread.CurrentThread.ManagedThreadId}");

            // Wait for the previously running scenario to finish shutting down before continuing
            WaitForOffline(120);

            TraceFactory.Logger.Debug("Resetting the SessionProxy for execution of next scenario.");
            PublishSessionState(SessionState.Resetting);

            //Reset the SessionMaps which are specific to the executing scenario.
            SessionMaps = new Collection<SessionMapObject>();
            //Intentionally not resetting the EventPublisher.  We want to keep all subcription data across all scenario execution.
            //Intentionally not resetting the Assets collection which contains all assets reserved for all scenarios.

            TraceFactory.Logger.Debug($"Transitioning from {StartupStep} to {SessionStartupTransition.ReadyToStage}");
            StartupStep = SessionStartupTransition.ReadyToStage;

            //Set the next current scenario
            _currentScenarioIndex++;

            // This will kick off the execution of the next scenario.
            // This call to Stage() will kick off the process for the next scenario.
            // The other startup states will be handled by the SessionClient.
            StageHandler();
        }

        #region ISessionBackend

        /// <summary>
        /// Registers the machine.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <returns>
        /// A string representation of the DataContract serialized SystemManifest
        /// </returns>
        public string RegisterMachine(string machineName)
        {
            try
            {
                return SelectHost(machineName).Register().Serialize();
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }

            return string.Empty;
        }

        /// <summary>
        /// Indicates that there was a status message change on the specified resource
        /// </summary>
        /// <param name="machineName">Name of the machine that this resource resides on.</param>
        /// <param name="instanceId">The name of the resource</param>
        /// <param name="message">The resource status message</param>
        public void ChangeResourceStatusMessage(string machineName, string instanceId, string message)
        {
            try
            {
                lock (_lock)
                {
                    SelectResource(machineName, instanceId).ChangeStatusMessage(message);
                }
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Indicates that a resource had a status change
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="instanceId"></param>
        /// <param name="state"></param>
        public void ChangeResourceState(string machineName, string instanceId, RuntimeState state)
        {
            try
            {
                lock (_lock)
                {
                    TraceFactory.Logger.Debug("{0} : {1} : {2}".FormatWith(machineName, instanceId, state));
                    SelectResource(machineName, instanceId).ChangeState(state);
                }
            }
            catch (DispatcherOperationException ex)
            {
                // When resetting, we want to ignore cases where we couldn't find the resource because the SessionMap is being rebuilt.
                if (_runtimeState != SessionState.Resetting)
                {
                    TraceFactory.Logger.Error("{0}, {1}, {2}".FormatWith(machineName, instanceId, state));
                    PublishError(ex);
                }
            }
        }

        /// <summary>
        /// Indicates that the status of a machine has changed
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="message">The status message.</param>
        public void ChangeMachineStatusMessage(string machineName, string message)
        {
            try
            {
                SelectHost(machineName).ChangeStatusMessage(message);
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Registers the specified resource.
        /// </summary>
        /// <param name="machineName">The name.</param>
        /// <param name="instanceId">The resource name</param>
        /// <param name="endpoint">The endpoint.</param>
        public void RegisterResource(string machineName, string instanceId, Uri endpoint)
        {
            try
            {
                // Once the client resource process registers with the proxy, then it
                // is automatically in a Ready state.
                SelectResource(machineName, instanceId).Register(endpoint);
                ChangeResourceState(machineName, instanceId, RuntimeState.Ready);
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Signals to the listener that there was a runtime error.
        /// </summary>
        /// <param name="error">The error object.</param>
        public void HandleAssetError(RuntimeError error)
        {
            if (Ticket.RemoveUnresponsiveDevices)
            {
                TraceFactory.Logger.Debug(error.ElementName);
                try
                {
                    Task.Factory.StartNew(() => HandleAssetErrorHandler(error));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Runtime error that happen during the session.
        /// </summary>
        /// <param name="error">The Runtime Error that happen during the session</param>
        public void HandleAssetErrorHandler(RuntimeError error)
        {
            bool errorHandled = SelectAsset(error.ElementName).HandleError(error);

            if (!errorHandled)
            {
                TraceFactory.Logger.Debug($"Asset {error.ElementName} has had too many errors.  Taking offline.");
                TakeOffline(error.ElementName);

                // Set up a timer to check in 15 minutes to see if this device can be brought back online
                if (!_offlineDeviceRetryTimers.TryGetValue(error.ElementName, out Timer timer))
                {
                    timer = new Timer(OfflineDeviceRetry, error.ElementName, -1, -1);
                    _offlineDeviceRetryTimers[error.ElementName] = timer;
                }

                timer.Change((int)TimeSpan.FromMinutes(15).TotalMilliseconds, Timeout.Infinite);
            }
        }

        private void OfflineDeviceRetry(object state)
        {
            string elementName = (string)state;
            if (SelectAsset(elementName).CanResumeActivity())
            {
                TraceFactory.Logger.Debug($"Asset {elementName} has recovered.  Bringing online.");
                BringOnline(elementName);
            }
            else
            {
                TraceFactory.Logger.Debug($"Asset {elementName} has not recovered.  Keeping offline.");
            }
        }

        /// <summary>
        /// Collect memory profile data for a device with the specified IP.
        /// </summary>
        /// <param name="serializedDeviceInfo">The serialized <see cref="IDeviceInfo"/> data.</param>
        /// <param name="snapshotLabel">Descriptor for the memory profile.</param>
        public void CollectDeviceMemoryProfile(string serializedDeviceInfo, string snapshotLabel)
        {
            try
            {
                IDeviceInfo deviceInfo = Serializer.Deserialize<IDeviceInfo>(XElement.Parse(serializedDeviceInfo));
                if (DeviceConstructor.Create(deviceInfo) is JediDevice && MemoryGatheringOkay(deviceInfo.AssetId))
                {
                    string memoryPools = GlobalSettings.Items["MemoryPools"];
                    TraceFactory.Logger.Debug("Retrieving Memory Count: {0} '{1}' '{2}'".FormatWith(deviceInfo.Address, deviceInfo.AdminPassword, snapshotLabel));
                    CollectJediMemoryProfile(deviceInfo, snapshotLabel, memoryPools);
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Checks to ensure we haven't already gathered the memory on the specified device within the last 4 minutes.
        /// </summary>
        /// <param name="assetId">The Asset Id.</param>
        /// <returns>true if it's okay to gather the memory on the device.</returns>
        private bool MemoryGatheringOkay(string assetId)
        {
            bool retrieveMemory = false;

            // Find the affected AssetHost and suspend it.
            AssetHost assetHost = SessionMaps.OfType<AssetMap>().SelectMany(m => m.Hosts).FirstOrDefault(h => h.Asset.AssetId == assetId);
            if (assetHost != null)
            {
                DateTime dtNow = DateTime.Now;
                TimeSpan ts = dtNow.Subtract(assetHost.AssetMemoryRetrieved);
                if (ts.Minutes > 4)
                {
                    retrieveMemory = true;
                    assetHost.AssetMemoryRetrieved = dtNow;
                }
            }

            return retrieveMemory;
        }

        /// <summary>
        /// Collects device memory counters.
        /// If IsDistributedSystem == false, the memory XML blob should NOT be saved.
        /// If we need to save the XML blob for STB, a new SystemSetting should be created to control this.  It should NOT be tied to EnterpriseEnabled.
        /// </summary>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo"/> (address, admin password, etc.)</param>
        /// <param name="snapshotLabel">The descriptor for the memory profile.</param>
        /// <param name="memoryPools">The memory pools to gather.</param>
        private void CollectJediMemoryProfile(IDeviceInfo deviceInfo, string snapshotLabel, string memoryPools)
        {
            JediMemoryRetrievalAgent memoryCollector = new JediMemoryRetrievalAgent(deviceInfo, _sessionId, memoryPools, GlobalSettings.IsDistributedSystem);
            memoryCollector.CollectDeviceMemoryCounters(snapshotLabel);

            // allow the device to reset before any more HTTP calls
            Thread.Sleep(10000);
        }

        /// <summary>
        /// Sends a synchronization signal with the specified event name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            Task.Factory.StartNew(() => SignalSynchronizationEventHandler(eventName));
        }

        private void SignalSynchronizationEventHandler(string eventName)
        {
            try
            {
                Parallel.ForEach(SessionMaps, n => n.SignalSynchronizationEvent(eventName));
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        #endregion ISessionBackend

        #region ISessionProxy

        /// <summary>
        /// Initiates a new session based on the specified session ticket
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ownerCallback">The owner callback.</param>
        public void Initiate(SessionTicket ticket, Uri ownerCallback)
        {
            Thread.CurrentThread.SetName($"Initiate-{Thread.CurrentThread.ManagedThreadId}");

            TraceFactory.Logger.Debug("SESSION TICKET:{0}{1}".FormatWith(Environment.NewLine, LegacySerializer.SerializeDataContract(ticket).ToString()));

            Ticket = ticket;
            _ownerCallback = ownerCallback;
            _firstCallToReserve = true; //This method may be called again after the user returned to the first wizard screen.  Needs to be reset here.
            UserManager.CurrentUser = Ticket.SessionOwner;

            TraceFactory.Logger.Debug("Queueing call to insert Session Log Data");
            Task.Factory.StartNew(LogSessionId);

            MapElement = new SessionMapElement(ticket.SessionId + "-" + ticket.SessionOwner.UserName, ElementType.Session)
            {
                SessionId = ticket.SessionId
            };

            if (!string.IsNullOrEmpty(Ticket.EmailAddresses))
            {
                //Add Collection of strings
                _failNotifier = new FailNotification(Ticket.SessionId, Ticket.SessionName, Ticket.FailureCount, Ticket.EmailAddresses, Ticket.CollectDARTLogs, Ticket.TriggerList);
            }

            if (GlobalSettings.IsDistributedSystem == false)
            {
                StartEventLogCollector();
            }
            TraceFactory.Logger.Debug("Transitioning to ReadyToStart");

            StartupStep = SessionStartupTransition.ReadyToStart;
        }

        /// <summary>
        /// Reserves all assets associated with this session using the specified key.
        /// </summary>
        /// <param name="reservationKey">The reservation key.</param>
        /// <returns></returns>
        public AssetDetailCollection Reserve(string reservationKey)
        {
            Thread.CurrentThread.SetName("Reserve-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            if (Ticket == null)
            {
                TraceFactory.Logger.Debug("Ticket is null");
            }
            TraceFactory.Logger.Debug("ENTERING - SessionId...{0}".FormatWith(Ticket.SessionId));

            AssetDetailCollection assets = new AssetDetailCollection();

            if (StartupStep != SessionStartupTransition.ReadyToStart && StartupStep != SessionStartupTransition.ReadyToStage)
            {
                var ex = new DispatcherOperationException("Not ready to reserve.  Current Ready state: {0}".FormatWith(StartupStep));
                PublishError(ex);
                return assets;
            }
            
            Collection<string> assetIds = new Collection<string>();
            if (_firstCallToReserve)
            {
                TraceFactory.Logger.Debug("First call to reserve");

                // Reverse the setting for the next time.
                _firstCallToReserve = false;

                // Log to the session Id table only on the first call to this method.
                Task.Factory.StartNew(LogSessionId);

                // Build all the SystemManifestAgents needed to run this session
                _manifestAgents = new SystemManifestAgentDictionary();
                foreach (Guid scenarioId in Ticket.ScenarioIds)
                {
                    SystemManifestAgent agent = new SystemManifestAgent(Ticket, scenarioId);
                    if (agent.Resources == null || !agent.Resources.Any())
                    {
                        var ex = new ArgumentException($"No Virtual Resources found for the selected Scenario: {scenarioId}");
                        PublishError(ex);
                        return assets;
                    }

                    foreach (string assetId in agent.RequestedAssets)
                    {
                        assetIds.Add(assetId);
                    }
                    _manifestAgents.Add(scenarioId, agent);
                }
            }
            else
            {
                // This is a subsequent call to reserve, so just add the asset Ids that
                // are listed in the ticket.  In the future this will be enhanced to only
                // include those assets that didn't succeed the first time.
                // At a later point when we want to support the ability to reserve assets
                // using multiple reservation keys, there will be more here to pull that off
                foreach (var asset in Assets)
                {
                    assetIds.Add(asset.AssetId);
                }
            }

            //Reserve the Domain Accounts.
            ReserveDomainAccounts(_manifestAgents.DomainAccountQuantities);

            //Reserve the Assets
            List<AssetReservationResult> assetReservations = new List<AssetReservationResult>();
            try
            {
                // Try to reserve the assets from the inventory service
                TraceFactory.Logger.Debug("Contacting asset inventory");
                if (assetIds.Count > 0)
                {
                    
                    _reservationManager.ReleaseSessionReservations(Ticket.SessionId);
                    

                    DateTime sessionStart = DateTime.Now;
                    DateTime sessionEnd = sessionStart.AddHours(Ticket.DurationHours);

                    assetReservations.AddRange(_reservationManager.ReserveAssets(assetIds, Ticket.SessionId, reservationKey, sessionStart, sessionEnd));
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Reservation failed", ex);

                // If the assets cannot be reserved, build the asset detail manually.
                assetReservations.AddRange(assetIds.Select(n => new AssetReservationResult(n, AssetAvailability.Unknown)));
            }
            finally
            {
                // Finally convert the test asset information into asset detail information
                foreach (AssetDetail assetDetail in AssetDetailCreator.CreateAssetDetails(assetReservations))
                {

                    assets.Add(assetDetail);
                }

                // Update the ticket with the new asset details before returning those details.
                Assets.Replace(assets);
            }

            // Update the state value locally only as it doesn't need to be sent back to the client
            // this intermediate state is only used locally within this session.
            TraceFactory.Logger.Debug("Transitioning to ReadyToStage");
            StartupStep = SessionStartupTransition.ReadyToStage;

            return assets;
        }

        /// <summary>
        /// Reserves the Domain Accounts on a separate thread since DomainAccountService.Reserve makes a WCF call which can deadlock if running on the UI thread.
        /// </summary>
        /// <param name="quantities">User quantities per Account Pool</param>
        private void ReserveDomainAccounts(DomainAccountQuantityDictionary quantities)
        {
            DomainAccountReservationSet accounts = null;
            Task reserveTask = Task.Factory.StartNew(() => accounts = DomainAccountService.Reserve(Ticket.SessionId, quantities));
            reserveTask.Wait();

            foreach (string userPool in accounts.UserPools)
            {
                TraceFactory.Logger.Debug("Reserved {0}:{1}".FormatWith(userPool, accounts.TotalReserved(userPool)));
            }
        }

        /// <summary>
        /// Stages the specified asset details.
        /// </summary>
        /// <param name="assetDetails">The asset details.</param>
        public void Stage(AssetDetailCollection assetDetails)
        {
            // Update the asset information with the values provided by the client.  These new values
            // will be used when creating the session map for asset related items.
            Assets.Replace(assetDetails);

            Task.Factory.StartNew(() => StageHandler());
        }

        private void StageHandler()
        {
            Thread.CurrentThread.SetName("Stage-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            if (StartupStep != SessionStartupTransition.ReadyToStage)
            {
                var ex = new DispatcherOperationException("Invalid state to stage: {0}".FormatWith(StartupStep));
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
                return;
            }

            try
            {
                UpdateSessionShutdownState(MachineShutdownState.NotStarted, Ticket.SessionId, ignoreMissing: true);
                PublishSessionState(SessionState.Staging);

                // Refresh the assets available for this session
                SystemManifestAgent manifestAgent = _manifestAgents[Ticket.ScenarioIds.ElementAt(_currentScenarioIndex)];
                manifestAgent.Assets.Clear();
                manifestAgent.Assets.AddRange(Assets);

                // Build the manifests
                manifestAgent.BuildManifests();
                LogSessionScenario(manifestAgent, _currentScenarioIndex + 1);
                TraceFactory.Logger.Debug("Manifest created and logged");

                // Add the map for each type of component
                var resourceMap = new ResourceMap(manifestAgent);
                resourceMap.SessionMapCompleted += new EventHandler(OnResourceMapCompleted);
                SessionMaps.Add(resourceMap);
                TraceFactory.Logger.Debug("Resource Map added");

                if (manifestAgent.ManifestSet.Select(x => x.Assets).Count() > 0)
                {
                    SessionMaps.Add(new AssetMap(manifestAgent));
                    TraceFactory.Logger.Debug("Asset Map added");
                }

                if (manifestAgent.ManifestSet.SelectMany(n => n.ActivityPrintQueues.Values.SelectMany(m => m.OfType<RemotePrintQueueInfo>())).Any())
                {
                    SessionMaps.Add(new RemotePrintQueueMap(manifestAgent));
                    TraceFactory.Logger.Debug("Remote Print Queue Map Added");
                }

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Stage(l));
                if (!SessionMapElement.AllElementsSetTo<SessionMapObject>(SessionMaps, RuntimeState.Available))
                {
                    MapElement.UpdateStatus("Some elements did not setup correctly", RuntimeState.Error);
                    PublishSessionState(SessionState.Error, "Some required resources did not set up correctly.  The test can't continue");
                    TraceFactory.Logger.Debug("All elements are not set to Available");
                    LogSessionStatus(SessionStatus.Error);
                    return;
                }

                TraceFactory.Logger.Debug("Registering map elements with publisher");
                EventPublisher.RegisterMapElements(this);

                TraceFactory.Logger.Debug("Transitioning to ReadyToValidate");
                StartupStep = SessionStartupTransition.ReadyToValidate;
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
                LogSessionStatus(SessionStatus.Aborted);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
            catch (InsufficientDomainAccountsException ex)
            {
                TraceFactory.Logger.Error("Insufficient accounts: {0}", ex);
                PublishSessionState(SessionState.Error, ex.Message);
                LogSessionStatus(SessionStatus.Aborted);
            }
            catch (Exception ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
        }

        /// <summary>
        /// Initializes all maps within the built hierarchy for this framework.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Current state is invalid:  + StatusPublisher.DispatcherState</exception>
        public void Validate()
        {
            if (StartupStep != SessionStartupTransition.ReadyToValidate)
            {
                var ex = new InvalidOperationException("Current state is invalid: {0}".FormatWith(StartupStep));
                PublishError(ex);
                return;
            }

            if (!ValidateSharedFolder())
            {
                return;
            }

            TraceFactory.Logger.Debug("Starting validate thread");
            Task.Factory.StartNew(ValidateHandler);
        }

        /// <summary>
        /// Signals the dispatcher to validate again.
        /// </summary>
        public void Revalidate()
        {
            if (StartupStep != SessionStartupTransition.ReadyToRevalidate && StartupStep != SessionStartupTransition.ReadyToPowerUp)
            {
                var ex = new InvalidOperationException("Invalid state for revalidation: {0}".FormatWith(StartupStep));
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
                return;
            }

            Task.Factory.StartNew(RevalidateHandler);
        }

        private void RevalidateHandler()
        {
            try
            {
                Thread.CurrentThread.SetName("Revalidate-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

                PublishSessionState(SessionState.Validating);
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Revalidate(l));
                if (SessionMapElement.AnyElementsSetTo(SessionMaps, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    TraceFactory.Logger.Debug("Transitioning to ReadyToRevalidate");
                    StartupStep = SessionStartupTransition.ReadyToRevalidate;
                }
                else
                {
                    MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                    TraceFactory.Logger.Debug("Transitioning to ReadyToPowerUp");
                    StartupStep = SessionStartupTransition.ReadyToPowerUp;
                }

                // Since this could be called multiple times with the same state, force the event tracker to notify listening clients
                EventPublisher.PublishSessionStartupTransition(StartupStep, Ticket.SessionId, _ownerCallback);

            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
                LogSessionStatus(SessionStatus.Aborted);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
        }

        private void ValidateHandler()
        {
            try
            {
                Thread.CurrentThread.SetName("Validate{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

                PublishSessionState(SessionState.Validating);
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Validate(l));

                if (SessionMapElement.AnyElementsSetTo(SessionMaps, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    TraceFactory.Logger.Debug("Transitioning to ReadyToRevalidate");
                    StartupStep = SessionStartupTransition.ReadyToRevalidate;
                }
                else
                {
                    MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                    TraceFactory.Logger.Debug("Transitioning to ReadyToPowerUp");
                    StartupStep = SessionStartupTransition.ReadyToPowerUp;
                }

                RegisterWithSTFServices();
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
                LogSessionStatus(SessionStatus.Aborted);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
            catch (CommunicationException ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
        }

        /// <summary>
        /// Deploys all maps that have been built and intialized for this session.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Current state is invalid:  + StatusPublisher.DispatcherState</exception>
        public void PowerUp()
        {
            Task.Factory.StartNew(PowerUpHandler);
        }

        /// <summary>
        /// Deploys all maps that have been built and intialized for this session
        /// </summary>
        /// <param name="schedule">Allows for a time to be set for the maps to be deployed.</param>
        public void PowerUp(SessionStartSchedule schedule)
        {
            // If a schedule is provided this will set up a timer that waits until the scheduled is
            // reached before it actually calls the powerup method.

            TraceFactory.Logger.Debug("PowerUp scheduled start @ {0}".FormatWith(schedule.SetupDateTime));
            _startSchedule = schedule;

            var now = DateTime.Now;
            var setupDateTime = _startSchedule.SetupDateTime;

            TraceFactory.Logger.Debug("NOW: {0}".FormatWith(now.ToLongTimeString()));
            TraceFactory.Logger.Debug("SCH: {0}".FormatWith(setupDateTime.ToLongTimeString()));

            if (setupDateTime > now)
            {
                var delayTime = setupDateTime.Subtract(now);
                TraceFactory.Logger.Debug("Timer set to expire in {0} minutes".FormatWith(delayTime.TotalMinutes));
                _powerUpTimer = new System.Timers.Timer(delayTime.TotalMilliseconds);
                _powerUpTimer.Elapsed += ScheduledStartTimeElapsed;
                _powerUpTimer.Start();

                Parallel.ForEach<ResourceHost>
                (
                    SessionMaps.OfType<ResourceMap>().SelectMany(x => x.Hosts),
                    (m) => m.MapElement.UpdateStatus("waiting")
                );

                SessionMaps.OfType<ResourceMap>().First().MapElement
                    .UpdateStatus("PowerUp @ {0:ddMMMyyyy HH:mm}".FormatWith(setupDateTime));
            }
            else
            {
                TraceFactory.Logger.Debug("Starting PowerUp Immediately");
                Task.Factory.StartNew(PowerUpHandler);
            }
        }

        private void ScheduledStartTimeElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _powerUpTimer.Stop();
            Task.Factory.StartNew(PowerUpHandler);
        }

        private void PowerUpHandler()
        {
            Thread.CurrentThread.SetName("PowerUp-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            if (StartupStep != SessionStartupTransition.ReadyToPowerUp)
            {
                var ex = new InvalidOperationException("Current state is invalid: {0}".FormatWith(StartupStep));
                PublishError(ex);
                return;
            }

            try
            {
                PublishSessionState(SessionState.PowerUp);
                MapElement.UpdateStatus("Starting", RuntimeState.Starting);

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.PowerUp(l));
                if (SessionMapElement.AllElementsSetTo<SessionMapObject>(SessionMaps, RuntimeState.Ready))
                {
                    MapElement.UpdateStatus("Ready", RuntimeState.Ready);
                    TraceFactory.Logger.Debug("Transitioning to ReadyToRun");
                    StartupStep = SessionStartupTransition.ReadyToRun;
                }
                else
                {
                    MapElement.UpdateStatus(RuntimeState.Error);
                    PublishSessionState(SessionState.Error);
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Runs this test scenario by starting all maps that have been built, initialized and deployed.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Current state is invalid:  + StatusPublisher.DispatcherState</exception>
        public void Run()
        {
            if (StartupStep != SessionStartupTransition.ReadyToRun)
            {
                var ex = new InvalidOperationException("Current state is invalid to run: {0}".FormatWith(StartupStep));
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
                return;
            }

            if (_startSchedule == null)
            {
                RunNow();
            }
            else
            {
                TraceFactory.Logger.Debug("Run scheduled to start @ {0}".FormatWith(_startSchedule.StartDateTime));

                var now = DateTime.Now;
                TraceFactory.Logger.Debug("NOW: {0}".FormatWith(now.ToLongTimeString()));
                TraceFactory.Logger.Debug("SCH: {0}".FormatWith(_startSchedule.StartDateTime.ToLongTimeString()));
                if (_startSchedule.StartDateTime > now)
                {
                    var delayTime = _startSchedule.StartDateTime.Subtract(now);
                    TraceFactory.Logger.Debug("Timer set to expire in {0} minutes".FormatWith(delayTime.TotalMinutes));

                    _runTimer = new System.Timers.Timer(delayTime.TotalMilliseconds);
                    _runTimer.Elapsed += RunTimer_Elapsed;
                    _runTimer.Start();

                    Parallel.ForEach<ResourceHost>
                    (
                        SessionMaps.OfType<ResourceMap>().SelectMany(x => x.Hosts),
                        (m) => m.MapElement.UpdateStatus("waiting")
                    );

                    SessionMaps.OfType<ResourceMap>().First().MapElement
                        .UpdateStatus("Run @ {0:ddMMMyyyy HH:mm}".FormatWith(_startSchedule.StartDateTime));
                }
                else
                {
                    TraceFactory.Logger.Debug("Starting Run immediately");
                    RunNow();
                }
            }
        }

        private void RunTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _runTimer.Stop();
            RunNow();
        }

        private void RunNow()
        {
            // First execute a run on the admin worker which should deliver up only the activities
            // that are listed in the preprocess portion.
            // NOTE: for now this is restricting the selection to AdminWorkers.  This will improve
            // performance as we won't spin up a bunch of threads for resources that aren't doing
            // any setup.  This can be changed very easily in the future.
            var resources = Resources.OfType<AdminWorkerInstance>();
            TraceFactory.Logger.Debug("Performing pre run activities on any resources configured to perform setup");
            Parallel.ForEach<ResourceInstance>(resources, m => m.StartSetup());

            // Log any components used in the scenario
            SystemManifestAgent manifestAgent = _manifestAgents[Ticket.ScenarioIds.ElementAt(_currentScenarioIndex)];
            manifestAgent.LogSessionComponents();
            manifestAgent.LogAssociatedProducts();

            Task.Factory.StartNew(RunHandler);
        }

        private void RunHandler()
        {
            try
            {
                Thread.CurrentThread.SetName("Run-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

                PublishSessionState(SessionState.Running);

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Run(l));

                // If the state is still what is was before the ForEach call then we can move
                // to the next state as there wasn't a change behind the scenes like a shutdown.
                if (_runtimeState == SessionState.Running)
                {
                    MapElement.UpdateStatus("Running", RuntimeState.Running);
                    TraceFactory.Logger.Debug("Trasitioning to StartupComplete");
                    StartupStep = SessionStartupTransition.StartupComplete;
                    LogSessionStatus(SessionStatus.Running);
                }
                //Turn on Health check timer
                ToggleHealthCheck(true);
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
            }
        }

        /// <summary>
        /// Repeats this instance of the test.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Current state is invalid to repeat:  + State</exception>
        public void Repeat()
        {
            
            if (_runtimeState != SessionState.RunComplete)
            {
                var ex = new InvalidOperationException("Current state is invalid to repeat: {0}".FormatWith(_runtimeState));
                PublishError(ex);
                LogSessionStatus(SessionStatus.Error);
                return;
            }

            

            Task.Factory.StartNew(RepeatHandle);
           
        }

        private void RepeatHandle()
        {
            try
            {
                Thread.CurrentThread.SetName("Repeat-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

                PublishSessionState(SessionState.Running);

                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Repeat(l));

                // For loop to ignore all the devices that were put offline.
                foreach (string assetId in _offlineDevices)
                {
                    TakeOffline(assetId);
                }

                // If the state is still what it was before the ForEach call then we can move
                // to the next state as there wasn't a change behind the scenes like a shutdown.
                if (_runtimeState == SessionState.Running)
                {
                    MapElement.UpdateStatus("Running", RuntimeState.Running);
                    StartupStep = SessionStartupTransition.StartupComplete;

                    //Turn on Health check timer
                    ToggleHealthCheck(true);
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }
        /// <summary>
        /// Get the list of offline devices during the session.
        /// </summary>
        /// <returns>offline devices</returns>
        public HashSet<string> GetSessionOfflineDevices()
        {
            //SessionMaps.OfType<AssetMap>().ToList<AssetMap>().ForEach(AddToOfflineDevices);
            foreach (var host in SessionMaps.OfType<AssetMap>().SelectMany(p => p.Hosts))
            {
                if (host.MapElement.State == RuntimeState.Offline)
                {
                    _offlineDevices.Add(host.AssetName);
                }
            }
            return _offlineDevices;
        }

        /// <summary>
        /// Set the list of devices that needs to be brought back online 
        /// for the repeat session or the pause session.
        /// </summary>
        /// <param name="onlineDevices">Offline Devices</param>
        public void SetSessionOfflineDevices(HashSet<string> onlineDevices)
        {

            foreach (string assetId in onlineDevices)
            {
                if (_offlineDevices.Contains(assetId))
                {
                    BringOnline(assetId);
                    _offlineDevices.Remove(assetId);
                }
            }

        }

        /// <summary>
        /// Shuts down this session using the defined <see cref="ShutdownOptions"/>
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        public void Shutdown(ShutdownOptions options)
        {
            TraceFactory.Logger.Debug("Options --{0}{1}".FormatWith(Environment.NewLine, options));
            Task.Factory.StartNew(() => ShutdownHandler(options));
        }

        private void ShutdownHandler(ShutdownOptions options)
        {
            bool errorOccurred = false;

            Thread.CurrentThread.SetName("Shutdown-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (_runtimeState == SessionState.ShutdownComplete)
            {
                string message = "Invalid state for shutdown: {0}".FormatWith(_runtimeState);
                var ex = new DispatcherOperationException(message);
                PublishError(ex);
                TraceFactory.Logger.Error(message);
                return;
            }

            // Keep track of whether or not the session is being aborted before completion.
            bool aborted = (_runtimeState != SessionState.RunComplete);

            PublishSessionState(SessionState.ShuttingDown);
            MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

            try
            {
                UpdateSessionShutdownState(MachineShutdownState.Pending, Ticket.SessionId);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Failed to update session shutdown state", ex);
                errorOccurred = true;
            }

            if (!ShutdownScenario(options))
            {
                errorOccurred = true;
            }

            if (!errorOccurred)
            {
                WaitForOffline(120);
            }

            // Raise the "Going out of business" event and close the front door into this instance
            TraceFactory.Logger.Debug("Closing front channel service endpoint");
            _sessionFrontendService?.Close();

            try
            {
                _reservationManager.ReleaseSessionReservations(Ticket.SessionId);
            }
            catch (Exception ex)
            {
                // Send an exception back to the client, but continue the shutdown process
                TraceFactory.Logger.Error("Error releasing asset reservations", ex);
                MapElement.UpdateStatus(RuntimeState.Warning, "Error releasing asset reservations", ex);
                errorOccurred = true;
            }

            if (aborted)
            {
                try
                {
                    LogSessionStatus(SessionStatus.Aborted);
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error($"Failed to log session status {SessionStatus.Aborted}.", ex);
                    errorOccurred = true;
                }
            }

            var finalState = errorOccurred ? MachineShutdownState.Partial : MachineShutdownState.Complete;
            Task updateTask = Task.Factory.StartNew(() => UpdateSessionShutdownState(finalState, Ticket.SessionId));
            TraceFactory.Logger.Debug($"Session {Ticket.SessionId} final state: {finalState}");
            updateTask.Wait();

            TraceFactory.Logger.Debug("Closing back channel service endpoint");
            _sessionBackendService?.Close();

            if (options.CopyLogs)
            {
                TraceFactory.Logger.Debug("Copying system logs.  This will be the last entry");
                // Perform one last copy of session proxy logs at the end to capture as much log
                // information as possible

                if (GlobalSettings.IsDistributedSystem)
                {
                    // Save log files for SessionProxy for this session.
                    SaveLogFiles(LogFileDataCollection.Create(LogFileReader.DataLogPath()), Ticket.SessionId);
                }

                CopySystemLogs();
            }

            // Publish back to all clients the state change for this session being shut down.
            // Note that it is all started in the background.
            List<Task> finalTasks = new List<Task>();
            finalTasks.AddRange(EventPublisher.PublishSessionStateInTask(SessionState.ShutdownComplete, Ticket.SessionId));
            Action publishStateAction = new Action(() =>
            {
                EventPublisher.PublishSessionStartupTransition
                (
                    SessionStartupTransition.ReadyToStart,
                    Ticket.SessionId,
                    _ownerCallback,
                    background: false
                );

                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            });

            finalTasks.Add(Task.Factory.StartNew(publishStateAction));
            finalTasks.AddRange(EventPublisher.ReleaseSession(Ticket.SessionId)); //This clears the session data from the client

            TraceFactory.Logger.Debug("Starting final background tasks");
            Task.WaitAll(finalTasks.ToArray());
            TraceFactory.Logger.Debug("All background tasks have completed");

            OnExit?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Shuts down the scenario while maintaining the current session state.
        /// </summary>
        /// <returns>true if successful, false if an error occured.</returns>
        private bool ShutdownScenario(ShutdownOptions options)
        {
            bool errorOccurred = false;

            TraceFactory.Logger.Debug($"Shutting down scenario {Ticket.ScenarioIds.ElementAt(_currentScenarioIndex)}.");

            //Turn off Health check timer
            ToggleHealthCheck(false);

            // First execute any teardown activities
            // NOTE: for now this is restricting the selection to AdminWorkers.  This will improve
            // performance as we won't spin up a bunch of threads for resources that aren't doing
            // any setup.  This can be changed very easily in the future.
            try
            {
                var resources = Resources.OfType<AdminWorkerInstance>();
                Parallel.ForEach<AdminWorkerInstance>(resources, m => m.StartTeardown());
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Failed to Start Teardown: {0}".FormatWith(ex.Message));
                errorOccurred = true;
            }

            // If we are allowing the workers to complete, then force the running session to
            // pause first before continuing.  If all resources don't pause within a defined
            // period of time, then just move on and shutdown.  Not being able to fully pause
            // should not stop the overall shutdown process.
            if (options.AllowWorkerToComplete)
            {
                try
                {
                    // Create options for the parallel processing that will contain the ability
                    // to cancel.  Then create a cancellation token that will be used by the timer
                    // thread to abort the pause process if it doesn't complete in time.
                    var parallelOptions = new ParallelOptions();
                    var cancelSource = new CancellationTokenSource();
                    parallelOptions.CancellationToken = cancelSource.Token;

                    // Queue the timer thread that will abort the pause after a period of time
                    // if the pause is complete, the call to Cancel should have no effect.
                    Task.Factory.StartNew(() => PauseWaitOnShutdown(cancelSource));

                    // Start pausing all items in the map, using the parallel options
                    Parallel.ForEach(SessionMaps, parallelOptions, m => m.Pause());
                }
                catch (OperationCanceledException ex)
                {
                    TraceFactory.Logger.Error("The pause was aborted", ex);
                    errorOccurred = true;
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Pause was aborted for a general exception", ex);
                    errorOccurred = true;
                }
            }

            try
            {
                // Spin out each component group and wait for them to finish...
                Parallel.ForEach<SessionMapObject>(SessionMaps, (m, l) => m.Shutdown(options, l));
                TraceFactory.Logger.Debug("All session maps shut down.");
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Shutdown failed across the session maps.", ex);
                errorOccurred = true;
            }            

          
            LogScenarioEnd(_currentScenarioIndex + 1); //Log the Scenario end time (includes shutdown time intentionally)

            return !errorOccurred;

        }

        /// <summary>
        /// Opens the front channel service endpoint.
        /// </summary>
        /// <param name="serviceUri"></param>
        public void StartFrontendService(Uri serviceUri)
        {
            _sessionFrontendService = new WcfHost<ISessionProxy>
                (
                    this,
                    MessageTransferType.Http,
                    serviceUri
                );

            _sessionFrontendService.Open();
            TraceFactory.Logger.Debug("Opening front channel service endpoint");
        }

        /// <summary>
        /// Opens the back channel service endpoint.
        /// </summary>
        public void StartBackendService()
        {
            _sessionBackendService = SessionProxyBackendConnection.CreateServiceHost(this, _sessionId);
            _sessionBackendService.Open();
            TraceFactory.Logger.Debug($"Opening back channel service endpoint: {_sessionBackendService.BaseAddresses[0].AbsoluteUri}");
        }

        /// <summary>
        /// Starts the local eventlog collector for non-distributed systems
        /// </summary>
        public void StartEventLogCollector()
        {
            if (Ticket.CollectEventLogs)
            {
                EventLogCollectorDetail detail = new EventLogCollectorDetail();
                //detail.Components = ?;
                detail.ComponentsData = "<ArrayOfString><string>&lt;Any&gt;</string></ArrayOfString>";
                detail.EntryTypesData = "<ArrayOfString><string>Error</string><string>Warning</string><string>Information</string><string>SuccessAudit</string><string>FailureAudit</string></ArrayOfString>";
                detail.Description = "Client VM Event Collector";
                detail.Enabled = true;   //Don't know what this does
                detail.HostName = Environment.MachineName;
                detail.Name = Environment.MachineName;
                detail.ResourceType = VirtualResourceType.EventLogCollector;
                detail.PollingInterval = 15;

                _eventLogCollector = new ClientController.EventLogCollector.EventLogCollector(Ticket.SessionId, detail);
                _eventLogCollector.Start();
            }
        }

        /// <summary>
        /// Pauses this <see cref="SessionProxy" /> instance, which pauses all maps.
        /// </summary>
        /// <exception cref="DispatcherOperationException">Invalid state to Pause: {0}.FormatWith(StatusPublisher.DispatcherState)
        /// or
        /// Unexpected state: {0}.FormatWith(StatusPublisher.DispatcherState)</exception>
        public void Pause()
        {
            if (_runtimeState != SessionState.Running)
            {
                var ex = new DispatcherOperationException("Invalid state to Pause: {0}".FormatWith(_runtimeState));
                PublishError(ex);
                return;
            }

            Task.Factory.StartNew(PauseHandler);
        }

        private void PauseHandler()
        {
            try
            {
                Thread.CurrentThread.SetName("Pause-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

                PublishSessionState(SessionState.Pausing);
                MapElement.UpdateStatus("Pausing", RuntimeState.Pausing);

                Parallel.ForEach<SessionMapObject>(SessionMaps, m => m.Pause());

                // If the state is still what is was before the ForEach call then we can move
                // to the next state as there wasn't a change behind the scenes like a shutdown.
                if (_runtimeState == SessionState.Pausing)
                {
                    PublishSessionState(SessionState.PauseComplete);
                    MapElement.UpdateStatus("Paused", RuntimeState.Paused);
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Resumes this instance, which resumes all maps.
        /// </summary>
        /// <exception cref="DispatcherOperationException">Invalid state to Resume: {0}.FormatWith(StatusPublisher.DispatcherState)</exception>
        public void Resume()
        {
            Thread.CurrentThread.SetName("Resume-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            if (_runtimeState != SessionState.PauseComplete)
            {
                var ex = new DispatcherOperationException("Invalid state to Resume: {0}".FormatWith(_runtimeState));
                PublishError(ex);
                return;
            }

            try
            {
                Parallel.ForEach<SessionMapObject>(SessionMaps, m => m.Resume());

                // If the state is still what is was before the ForEach call then we can move
                // to the next state as there wasn't a change behind the scenes like a shutdown.
                if (_runtimeState == SessionState.PauseComplete)
                {
                    PublishSessionState(SessionState.Running);
                    MapElement.UpdateStatus("Running", RuntimeState.Running);

                    //Making sure that whatever devices that was taking offline because of ERROR stay offline.
                    foreach (string assetId in _offlineDevices)
                    {
                        TakeOffline(assetId);
                    }
                    
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Takes an Asset Offline while allowing the test to continue running.
        /// </summary>
        /// <param name="assetId"></param>
        public void TakeOffline(string assetId)
        {
            Thread.CurrentThread.SetName("TakeOffline-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            
            try
            {
                // Call Suspend on each ResourceHost
                Parallel.ForEach<ResourceHost>(SessionMaps.OfType<ResourceMap>().SelectMany(m => m.Hosts), h => h.TakeOffline(assetId));

                // Find the affected AssetHost and suspend it.
                AssetHost assetHost = SessionMaps.OfType<AssetMap>().SelectMany(m => m.Hosts).FirstOrDefault(h => h.Asset.AssetId == assetId);
                if (assetHost != null)
                {
                    _offlineDevices.Add(assetId);
                    assetHost.TakeOffline();
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Brings an Asset Online after being offline.
        /// </summary>
        /// <param name="assetId"></param>
        public void BringOnline(string assetId)
        {
            Thread.CurrentThread.SetName("BringOnline-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            try
            {
                // Call Resume on each ResourceHost
                Parallel.ForEach<ResourceHost>(SessionMaps.OfType<ResourceMap>().SelectMany(m => m.Hosts), h => h.BringOnline(assetId));

                // Find the affected AssetHost and suspend it.
                AssetHost assetHost = SessionMaps.OfType<AssetMap>().SelectMany(m => m.Hosts).FirstOrDefault(h => h.Asset.AssetId == assetId);
                if (assetHost != null)
                {
                    assetHost.BringOnline();
                }
            }
            catch (OperationCanceledException ex)
            {
                TraceFactory.Logger.Debug("Operation canceled: {0}".FormatWith(ex.Message));
                PublishSessionState(SessionState.Canceled);
            }
            catch (AggregateException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Sets the CRC.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="crcOn">if set to <c>true</c> [CRC on].</param>
        public void SetCrc(string sessionId, string assetId, bool crcOn)
        {
            // Find the affected AssetHost and pass in the CRC setting.
            PrintDeviceHost deviceHost = SessionMaps.OfType<AssetMap>().SelectMany(m => m.Hosts).Where(h => h.Asset.AssetId == assetId).FirstOrDefault() as PrintDeviceHost;

            if (deviceHost != null)
            {
                deviceHost.SetPaperlessPrintMode(crcOn);
            }
            else
            {
                TraceFactory.Logger.Debug("Failed getting DeviceHost {0} for CRC".FormatWith(assetId));
            }
        }

        /// <summary>
        /// Restarts the specified machine.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="replaceMachine">if set to <c>true</c> use a new machine as a replacement.</param>
        public void RestartMachine(string machineName, bool replaceMachine)
        {
            try
            {
                Thread.CurrentThread.SetName("RestartMachine-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
                Task.Factory.StartNew(() => SelectHost(machineName).Restart(replaceMachine));
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Restarts the specified asset.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        public void RestartAsset(string assetId)
        {
            try
            {
                Thread.CurrentThread.SetName("Restart-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
                ThreadPool.QueueUserWorkItem(t => SelectAsset(assetId).Restart());
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Pauses the worker.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void PauseWorker(string userName)
        {
            try
            {
                Thread.CurrentThread.SetName("Pause-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
                SelectResource(userName).Pause();
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Resumes the worker.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void ResumeWorker(string userName)
        {
            try
            {
                Thread.CurrentThread.SetName("Resume-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
                SelectResource(userName).Resume();
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Halts the worker.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void HaltWorker(string userName)
        {
            try
            {
                Thread.CurrentThread.SetName("Halt-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
                SelectResource(userName).Halt();
            }
            catch (DispatcherOperationException ex)
            {
                PublishError(ex);
            }
        }

        /// <summary>
        /// Subscribes the defined listener to dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        public void Subscribe(Uri subscriber)
        {
            Thread.CurrentThread.SetName("Sub-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            TraceFactory.Logger.Debug("Subscribing {0}".FormatWith(subscriber));
            EventPublisher.Subscribe(subscriber);
            EventPublisher.RefreshSubscriber(subscriber);
        }

        /// <summary>
        /// Unsubscribes the defined listener from dispatcher updates coming from the status publisher
        /// </summary>
        /// <param name="subscriber">The client listener</param>
        public void Unsubscribe(Uri subscriber)
        {
            Thread.CurrentThread.SetName("Unsub-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            EventPublisher.Unsubscribe(subscriber);
        }

        /// <summary>
        /// Refreshes the defined listener with current dispatcher status information.
        /// </summary>
        /// <param name="subscriber">The client listener.</param>
        public void RefreshSubscription(Uri subscriber)
        {
            Thread.CurrentThread.SetName("RefreshSub-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            EventPublisher.RefreshSubscriber(subscriber);
        }

        /// <summary>
        /// Refreshes status for all subscribed listeners
        /// </summary>
        public void RefreshAllSubscriptions()
        {
            Thread.CurrentThread.SetName("RefreshAll-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            EventPublisher.RefreshAllSubscribers();
        }

        /// <summary>
        /// Checks the subscriber's subscription to see if it's still active.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>
        /// true if the subscription is still active, otherwise false
        /// </returns>
        public bool CheckSubscription(Uri subscriber)
        {
            Thread.CurrentThread.SetName("CheckSub-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));
            return EventPublisher.CheckSubscription(subscriber);
        }

        /// <summary>
        /// Returns the current session state
        /// </summary>
        /// <returns></returns>
        public SessionState GetSessionState()
        {
            return _runtimeState;
        }

        /// <summary>
        /// Returns the current Session Startup state
        /// </summary>
        /// <returns></returns>
        public SessionStartupTransition GetSessionStartupState()
        {
            return _startupStep;
        }

        /// <summary>
        /// Pings this instance.
        /// </summary>
        public void Ping()
        {
            TraceFactory.Logger.Debug("Ping request was made");
        }

        #endregion ISessionProxy

        private void RegisterWithSTFServices()
        {
            string domain = GlobalSettings.Items[Setting.DnsDomain];

            TraceFactory.Logger.Debug("Retrieving HostNames from MonitorConfig table.");

            //if (string.IsNullOrEmpty(Ticket.LogLocation))
            //{
            //    throw new OperationCanceledException("Monitor log service location is required. Select the log settings tab and provide the location: i.e.; STFData01-bc.etl.boi.rd.hpicorp.net");
            //}


            List<string> stfMonitorHostNames = null;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                stfMonitorHostNames = context.MonitorConfigs.Select(config => config.ServerHostName).Distinct().ToList();
            }

            //Register will all STF Monitor hosts
            foreach (string serverHostName in stfMonitorHostNames)
            {
                try
                {
                    TraceFactory.Logger.Debug($"Registering {Ticket.SessionId}:{Ticket.LogLocation} with {serverHostName}.");
                    using (STFMonitorServiceConnection stfMonitorSvcCxn = STFMonitorServiceConnection.Create($"{serverHostName}.{domain}"))
                    {
                        stfMonitorSvcCxn.Channel.Register(_sessionId, Ticket.LogLocation);
                    }
                }
                catch (CommunicationException)
                {
                    //Right now we're just going to log these and move on. (See note below)
                    TraceFactory.Logger.Error($"Unable to communicate with '{serverHostName}'.");
                    //TraceFactory.Logger.Error(ex.ToString());
                }
            }

            //kyoungman 11/10/2017 - The intent here is to only contact the servers that are running the STF Monitor service.
            //However, some servers are not present in the VirtualResourceMetadata (FileServer destinations for DSS, for example.)
            //Putting the following code on hold until we can address the issue of determining servers that are connected to a run.
            /*
            TraceFactory.Logger.Debug($"HostNames from ManifestAgent: {_manifestAgent.ServerHostNames.Count()}.");
            foreach (string serverHostName in _manifestAgent.ServerHostNames)
            {
                TraceFactory.Logger.Debug(serverHostName);
            }

            // Only contact the servers that are running STF Monitor service.
            IEnumerable<string> intersectedList = stfMonitorHostNames.Intersect(_manifestAgent.ServerHostNames);
            TraceFactory.Logger.Debug($"Hostnames to register: {intersectedList.Count()}.");

            foreach (string serverHostName in intersectedList)
            {
                TraceFactory.Logger.Debug($"Registering {Ticket.SessionId} with {serverHostName}.");
                using (STFMonitorServiceConnection stfMonitorSvcCxn = STFMonitorServiceConnection.Create($"{serverHostName}.{domain}"))
                {
                    stfMonitorSvcCxn.Channel.Register(_sessionId, Ticket.LogLocation);
                }
            }
            */
        }

        private void LogSessionStatus(SessionStatus status)
        {
            DateTime? endDate = null;
            if (status == SessionStatus.Aborted || status == SessionStatus.Complete)
            {
                endDate = DateTime.UtcNow;
            }

            using (DataLogContext context = DbConnect.DataLogContext())
            {
                SessionSummary summary = context.DbSessions.First(n => n.SessionId == Ticket.SessionId);
                summary.Status = status.ToString();
                summary.EndDateTime = endDate;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Error updating session status.", ex);
                }
            }
        }

        /// <summary>
        /// Updates the ShutdownState for the specified Session.
        /// Note: The updating of the ShutdownUser and the ShutdownDateTime occurs from the UI 
        /// and should already be set by the time this method is called.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="sessionId"></param>
        /// <param name="ignoreMissing"></param>
        internal static void UpdateSessionShutdownState(MachineShutdownState state, string sessionId, bool ignoreMissing = false)
        {
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

        /// <summary>
        /// Persists the session Information.
        /// </summary>
        private void LogSessionId()
        {
            lock (this)
            {
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    // Check to see if there is already a SessionSummary record for this session ID.
                    // This could happen if the user moves backward in the UI wizard.
                    SessionSummary sessionSummary = context.DbSessions.FirstOrDefault(s => s.SessionId == Ticket.SessionId);
                    if (sessionSummary == null)
                    {
                        sessionSummary = context.DbSessions.Add(new SessionSummary());
                    }

                    sessionSummary.SessionId = Ticket.SessionId;
                    sessionSummary.SessionName = Ticket.SessionName;
                    sessionSummary.Owner = Ticket.SessionOwner.UserName;
                    sessionSummary.Dispatcher = Environment.MachineName;
                    sessionSummary.StartDateTime = DateTime.UtcNow;
                    sessionSummary.Status = SessionStatus.Starting.ToString();
                    sessionSummary.StfVersion = AssemblyProperties.Version;
                    sessionSummary.Type = Ticket.SessionType;
                    sessionSummary.Cycle = Ticket.SessionCycle;
                    sessionSummary.Reference = Ticket.Reference;
                    sessionSummary.Notes = Ticket.SessionNotes;
                    sessionSummary.ShutdownState = MachineShutdownState.Unknown.ToString();
                    sessionSummary.ProjectedEndDateTime = DateTime.UtcNow.AddHours(Ticket.DurationHours);
                    sessionSummary.ExpirationDateTime = Ticket.ExpirationDate.ToUniversalTime();

                    using (EnterpriseTestContext enterpriseContext = new EnterpriseTestContext())
                    {
                        EnterpriseScenario scenario = EnterpriseScenario.Select(enterpriseContext, Ticket.ScenarioIds.ElementAt(_currentScenarioIndex));
                        sessionSummary.Tags = scenario.Vertical;
                    }

                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error("Error saving session summary.", ex);
                    }
                }
            }
        }

        private void PauseWaitOnShutdown(CancellationTokenSource cancellation)
        {
            // Wait for a defined period of time, then cancel the task.  If the pause is still
            // running this will abort it and allow the shutdown to continue.
            Thread.Sleep(1500);
            cancellation.Cancel();
        }

        /// <summary>
        /// Saves the log files for a worker running in this session.
        /// This is called by other processes to save the log files that have been created under each respective process.
        /// </summary>
        /// <param name="logFiles">The log files.</param>
        public void SaveLogFiles(LogFileDataCollection logFiles)
        {
            SaveLogFiles(logFiles, string.Empty);
        }

        /// <summary>
        /// Saves the log files for a worker running in this session.
        /// Private implementation allows filtering out all filenames that do not contain the specified text.
        /// </summary>
        /// <param name="logFiles">The log files.</param>
        /// <param name="filter">The filter.  Empty string applies no filter (writes all files).</param>
        private void SaveLogFiles(LogFileDataCollection logFiles, string filter)
        {
            // Write the log files to this sessions log file path in AppData.
            // They will be picked up later from there.
            string path = LogFileReader.DataLogPath(Ticket.SessionId);

            TraceFactory.Logger.Debug("{0} {1} filter: '{2}'".FormatWith(logFiles.MachineName, path, filter));
            logFiles.Write(path, filter);
        }

        /// <summary>
        /// Zips all log files that have been gathered from other STF processes and copies the zip file to
        /// the path specified in SystemSettings.
        /// </summary>
        public void CopySystemLogs()
        {
            try
            {
                // Any logs that have been sent to the dispatcher will be in the SessionProxy\Logs directory
                string sourceDir = LogFileReader.DataLogPath(Ticket.SessionId);

                if (!Directory.Exists(sourceDir))
                {
                    // No folder for log files exists, so no reason to continue.
                    return;
                }

                // Build the zip file path
                string zipFileName = @"{0}.zip".FormatWith(Ticket.SessionId);
                string rootDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
                string destinationDir = GlobalSettings.Items[Setting.DispatcherLogCopyLocation].Replace("${ROOT}", rootDrive);

                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                string destinationZip = Path.Combine(destinationDir, zipFileName);
                TraceFactory.Logger.Debug("Zipping log files. Source:{0}  Destination:{1}".FormatWith(sourceDir, destinationZip));
                //We have to retry because for STB the user processes may not quite be shut down.  Retrying after a second allows enough time.
                Retry.WhileThrowing
                (
                    () => ZipLogFiles(sourceDir, destinationZip), 5, TimeSpan.FromSeconds(1), new List<Type>() { typeof(IOException) }
                );

                //Clean up the temp directory
                Directory.Delete(sourceDir, true);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error creating and copying log files: {0}".FormatWith(ex.ToString()));
            }
        }

        /// <summary>
        /// Zips the log files pertaining to the specified session.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="destinationFilePath">The destination file path.</param>
        private static void ZipLogFiles(string sourceDirectoryPath, string destinationFilePath)
        {
            if (File.Exists(destinationFilePath))
            {
                //Clean up previous attempts
                File.Delete(destinationFilePath);
            }

            ZipFile.CreateFromDirectory(sourceDirectoryPath, destinationFilePath);
        }

        /// <summary>
        /// Logs information about the current scenario being run in this Session.
        /// </summary>
        /// <param name="manifestAgent"></param>
        /// <param name="runOrder"></param>
        private void LogSessionScenario(SystemManifestAgent manifestAgent, int runOrder)
        {
            lock (this)
            {
                using (DataLogContext dataLogContext = DbConnect.DataLogContext())
                {
                    SessionScenario sessionScenario = new SessionScenario()
                    {
                        SessionId = Ticket.SessionId,
                        RunOrder = (byte)runOrder,
                        ScenarioStart = DateTime.UtcNow
                    };

                    using (EnterpriseTestContext etContext = new EnterpriseTestContext())
                    {
                        EnterpriseScenario scenario = EnterpriseScenario.Select(etContext, manifestAgent.ScenarioId);
                        EnterpriseScenarioCompositeContract contract = ContractFactory.Create(scenario, true, false);
                        sessionScenario.ConfigurationData = contract.Save();
                    }

                    try
                    {
                        dataLogContext.DbSessionScenarios.Add(sessionScenario);
                        dataLogContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Error saving session scenario (index:{runOrder}).", ex);
                    }
                }
            }
        }

        private void LogScenarioEnd(int runOrder)
        {
            try
            {
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    SessionScenario sessionScenario = context.DbSessionScenarios.First(n => n.SessionId == Ticket.SessionId && n.RunOrder == runOrder);
                    sessionScenario.ScenarioEnd = DateTime.UtcNow;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error($"Error updating session scenario (index:{runOrder}).", ex);
            }
        }

        private void PublishError(Exception ex)
        {
            PublishSessionState(SessionState.Error, ex.Message);
            MapElement.UpdateStatus(RuntimeState.Error, ex.Message, ex);
            TraceFactory.Logger.Error(ex);
        }

        /// <summary>
        /// Selects the host based on the given name.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <returns>A resource host object for the specified machine name.</returns>
        public ResourceHost SelectHost(string machineName)
        {
            return SessionMaps.OfType<ResourceMap>().SelectMany(x => x.Hosts)
                              .FirstOrDefault(x => x.Machine.Name.EqualsIgnoreCase(machineName));
        }

        /// <summary>
        /// Selects the asset based on the given asset identifier.
        /// </summary>
        /// <param name="assetId">The asset unique identifier.</param>
        /// <returns>The <see cref="AssetHost"/> for the given identifier</returns>
        public AssetHost SelectAsset(string assetId)
        {

            return SessionMaps.OfType<AssetMap>().SelectMany(x => x.Hosts)
                              .FirstOrDefault(x => x.AssetName.EqualsIgnoreCase(assetId)
                                                || x.Asset.AssetId.EqualsIgnoreCase(assetId));
        }

        /// <summary>
        /// Gets the <see cref="ResourceInstance"/>s being used in this scenario.
        /// </summary>
        /// <value>
        /// The resources.
        /// </value>
        public IEnumerable<ResourceInstance> Resources
        {
            get { return SessionMaps.OfType<ResourceMap>().SelectMany(x => x.Hosts).SelectMany(x => x.Resources); }
        }

        /// <summary>
        /// Selects the resource based on the given machine name and user name.
        /// </summary>
        /// <param name="instanceId">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="DispatcherOperationException">Resource for the specified name was not found</exception>
        public ResourceInstance SelectResource(string instanceId)
        {
            var resource = Resources.Where(x => x.Id.Equals(instanceId)).FirstOrDefault();

            if (resource == null)
            {
                throw new DispatcherOperationException("Resource for {0} not found".FormatWith(instanceId));
            }

            return resource;
        }

        /// <summary>
        /// returns the ResourceHost for the machinename
        /// </summary>
        /// <param name="machineName">The machine that is hosting this resource</param>
        /// <param name="resourceInstanceId">The unique Id for the resource instance making this request</param>
        /// <returns></returns>
        public ResourceInstance SelectResource(string machineName, string resourceInstanceId)
        {
            var host = SelectHost(machineName);

            if (host == null)
            {
                throw new DispatcherOperationException("Machine {0} not found".FormatWith(machineName));
            }


            var resource = host.Resources.Where(e => e.Id.Equals(resourceInstanceId)).FirstOrDefault();

            if (resource == null)
            {
                if (!Ticket.CollectEventLogs)
                {
                    throw new DispatcherOperationException("Resource for {0} on host {1} not found".FormatWith(resourceInstanceId, machineName));
                }
                else
                {
                    EventLogCollectorDetail detail = new EventLogCollectorDetail();
                    //detail.Components = ?;
                    detail.ComponentsData = "<ArrayOfString><string>&lt;Any&gt;</string></ArrayOfString>";
                    detail.EntryTypesData = "<ArrayOfString><string>Error</string><string>Warning</string><string>Information</string><string>SuccessAudit</string><string>FailureAudit</string></ArrayOfString>";
                    detail.Description = "Client VM Event Collector";
                    detail.Enabled = true;   //Don't know what this does
                    detail.HostName = Environment.MachineName;
                    detail.Name = Environment.MachineName;
                    detail.ResourceType = VirtualResourceType.EventLogCollector;
                    detail.PollingInterval = 15;

                    resource = new ResourceInstance(machineName, detail);
                }

            }

            return resource;
        }

        private void ToggleHealthCheck(bool on)
        {
            if (!GlobalSettings.IsDistributedSystem)
            {

                TraceFactory.Logger.Debug("Skipping health check service for STB");
                return;
            }

            if (on)
            {
                int settingInterval = 3; //Default value
                try
                {
                    settingInterval = int.Parse(GlobalSettings.Items[Setting.HealthCheckInterval]);
                }
                catch (SettingNotFoundException)
                { }
                TimeSpan interval = TimeSpan.FromMinutes(settingInterval);
                _healthCheckTimer.Change(interval, interval);
            }
            else
            {
                _healthCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void HealthCheckTimer_Elapsed(object notUsed)
        {
            ToggleHealthCheck(false);

            foreach (ResourceInstance resource in Resources)
            {
                resource.CheckHealth();
            }

            ToggleHealthCheck(true);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TraceFactory.Logger.Debug("Releasing reserved user accounts");
                ThreadPool.QueueUserWorkItem(t => DomainAccountService.Release(Ticket.SessionId));

                //Clean up SystemManifestAgents
                foreach (Guid scenarioId in _manifestAgents.Keys)
                {
                    _manifestAgents[scenarioId].Dispose();
                }
                _manifestAgents.Clear();

                //Clean up health check timer
                _healthCheckTimer.Dispose();
                _healthCheckTimer = null;
            }
        }

        #endregion IDisposable Members

        #region SharedFolderCheck

        private bool ValidateSharedFolder()
        {
            if (!GlobalSettings.IsDistributedSystem)
            {
                // If we are in a local system there will be no copying of software from
                // one system to the other so there is not need to validate that the folder is shared.
                return true;
            }

            string sharedFolderPath = string.Empty;
            bool isFolderShared = false;
            Exception ex = new Exception("VirtualResource folder is not shared");

            try
            {
                //get the list of shared folders on the local system
                System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Share");
                var sharedFolderCollection = searcher.Get();

                //if there are no shared folders, we throw the error
                if (sharedFolderCollection.Count == 0)
                {
                    TraceFactory.Logger.Debug("There are no shared folders on this Dispatcher!");
                }

                //get the sharedfolder and check its status
                foreach (System.Management.ManagementObject queryObj in sharedFolderCollection)
                {
                    if (queryObj["Name"].ToString() == "VirtualResource")
                    {
                        if ("OK" == queryObj.GetPropertyValue("Status").ToString())
                        {
                            sharedFolderPath = @"\\" + System.Net.Dns.GetHostName() + "\\VirtualResource";
                            isFolderShared = true;
                        }
                        break;
                    }
                }

                searcher.Dispose();
            }
            catch (System.Management.ManagementException e)
            {
                TraceFactory.Logger.Debug("An error occurred while querying for WMI data: " + e.Message);
                PublishError(ex);
                return false;
            }

            //if the folder shared then check the access for read data
            if (isFolderShared)
            {
                return HasReadPermissionOnDir(sharedFolderPath);
            }
            else
            {
                TraceFactory.Logger.Debug("The VirtualResource Directory is not shared on this dispatcher");
                PublishError(ex);
                return false;
            }
        }

        private bool HasReadPermissionOnDir(string path)
        {
            DirectoryInfo sharedDir = new DirectoryInfo(path);

            //impersonate the user which the VM is going to boot as always
            bool result = false;
            UserImpersonator.Execute(() => result = CheckDirectory(sharedDir), GlobalSettings.Items.DomainAdminCredential);
            return result;
        }

        private bool CheckDirectory(DirectoryInfo sharedDir)
        {
            bool result = false;
            try
            {
                sharedDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
                result = true;
            }
            catch (System.Security.SecurityException secException)
            {
                PublishError(secException);
            }
            catch (UnauthorizedAccessException uaException)
            {
                PublishError(uaException);
            }
            return result;
        }

        

        #endregion SharedFolderCheck
    }
}
