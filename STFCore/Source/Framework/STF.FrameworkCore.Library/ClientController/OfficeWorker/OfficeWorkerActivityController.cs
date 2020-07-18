using HP.ScalableTest.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation.OfficeWorker
{
    /// <summary>
    /// Provides the engine controller implementation used by the Office Worker console.
    /// </summary>
    [ObjectFactory(VirtualResourceType.OfficeWorker)]
    [ObjectFactory(VirtualResourceType.CitrixWorker)]
    public class OfficeWorkerActivityController : IDisposable
    {
        private ServiceHost _commandService = null;
        private readonly string _clientControllerHostName = string.Empty;
        private static string _sessionId = string.Empty;        
        static VirtualResourceInstanceStatusLogger _statusLogger = null;
        protected Dictionary<ResourceExecutionPhase, EngineBase> Engines { get; private set; }        
        protected bool IsHalted { get; set; }
        protected ResourceExecutionPhase CurrentPhase { get; set; }

        /// <summary>
        /// Occurs when an activity state has changed.
        /// </summary>
        public event EventHandler<ActivityStateEventArgs> ActivityStateChanged;

        /// <summary>
        /// Occurs when the worker state has changed.
        /// </summary>
        public event EventHandler<ResourceEventArgs> WorkerStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerActivityController"/> class.
        /// </summary>
        public OfficeWorkerActivityController(string clientControllerHostName)
        {
            _clientControllerHostName = clientControllerHostName;
            IsHalted = false;
            CurrentPhase = ResourceExecutionPhase.Main;
            Engines = new Dictionary<ResourceExecutionPhase, EngineBase>();
            
        }

        protected EngineBase CurrentEngine
        {
            get { return GetEngine(CurrentPhase); }
        }

        /// <summary>
        /// Handles the <see cref="E:ActivityStateChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ActivityStateEventArgs"/> instance containing the event data.</param>
        protected void OnActivityStateChanged(object sender, ActivityStateEventArgs e)
        {
            if (ActivityStateChanged != null)
            {
                ActivityStateChanged(sender, e);
            }
        }

        /// <summary>
        /// Sets up this instance using the specified client controller host name.
        /// </summary>
        /// <param name="clientControllerHostName">Name of the client controller host.</param>
        public static OfficeWorkerActivityController Create(string clientControllerHostName, string instanceId)
        {            
            // The office worker is a separate process, so it must make a call up to the client controller to obtain
            // the manifest that will be used for the test.
            SystemManifest manifest = null;
            using (var serviceConnection = ClientControllerServiceConnection.Create(clientControllerHostName))
            {
                var data = serviceConnection.Channel.GetManifest(instanceId);
                manifest = SystemManifest.Deserialize(data);
                manifest.PushToGlobalDataStore(instanceId);
                manifest.PushToGlobalSettings();
                _sessionId = manifest.SessionId;
                _statusLogger = new VirtualResourceInstanceStatusLogger(_sessionId, Environment.UserName, 0, Enum.GetName(typeof(RuntimeState), 6), false, GlobalDataStore.ResourceInstanceId);
            }
           

            TraceFactory.Logger.Debug("Resource type: {0}".FormatWith(manifest.ResourceType));
            TraceFactory.Logger.Debug("InstanceId: {0}".FormatWith(GlobalDataStore.ResourceInstanceId));
            TraceFactory.Logger.Debug("UserName: {0}".FormatWith(GlobalDataStore.Credential.UserName));

            FrameworkServicesInitializer.InitializeExecution();

            return ObjectFactory.Create<OfficeWorkerActivityController>(manifest.ResourceType, clientControllerHostName);
        }

        /// <summary>
        /// Loads all plugins for this instance's activities.
        /// </summary>
        public virtual Dictionary<ActivityInfo, IPluginExecutionEngine> LoadPlugins()
        {
            var plugins = new Dictionary<ActivityInfo, IPluginExecutionEngine>();

            // Eagerly instantiate engines for each phase
            var phases = Enum.GetValues(typeof(ResourceExecutionPhase)).Cast<ResourceExecutionPhase>();
            foreach (var phase in phases)
            {
                TraceFactory.Logger.Debug("{0} phase".FormatWith(phase));
                foreach (var phasePlugin in GetEngine(phase).GetPlugins())
                {
                    plugins.Add(phasePlugin.Key, phasePlugin.Value);
                }
            }

            return plugins;
        }

        protected virtual IEnumerable<ResourceExecutionPhase> ApplicablePhases
        {
            get
            {
                return Enum.GetValues(typeof(ResourceExecutionPhase)).Cast<ResourceExecutionPhase>()
                    .Where(x => x == ResourceExecutionPhase.Main);
            }
        }

        /// <summary>
        /// Override so that child controllers can establish their own service host
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        protected virtual ServiceHost CreateServiceHost(int port)
        {
            TraceFactory.Logger.Debug("Creating endpoint");
            return VirtualResourceManagementConnection.CreateServiceHost(Environment.MachineName, port);            
        }

        /// <summary>
        /// Starts the OfficeWorker handler to run the controller as an Office Worker
        /// </summary>
        public void Start()
        {
            // Don't try to catch an error here.  There must be a credential entry in the manifest
            // that aligns with the current user running this process.
            var credential = GlobalDataStore.Credential;

            TraceFactory.Logger.Debug("Opening command service endpoint: {0}".FormatWith(credential.UserName));

            _commandService = CreateServiceHost(credential.Port);
            _commandService.Open();

            TraceFactory.Logger.Debug(_commandService.BaseAddresses[0].AbsoluteUri);

            SubscribeToEventBus();

            Action action = () =>
            {
                // Notify the client controller that the work is setup and ready to go.  This will give
                // the controller an opportunity to perform any system wide setup activities before
                // the worker notifies the dispatcher that it is ready to go.
                using (ClientControllerServiceConnection serviceConnection = ClientControllerServiceConnection.Create(_clientControllerHostName))
                {
                    
                    TraceFactory.Logger.Debug(
                        $"Notifying controller to perform any system wide tasks, waiting to be notified to register. {_commandService.BaseAddresses[0]}");
                    serviceConnection.Channel.NotifyResourceState(_commandService.BaseAddresses[0], RuntimeState.Ready);
                }
            };

            Retry.WhileThrowing(action, 5, TimeSpan.Zero, new List<Type>() { typeof(TimeoutException) });

            TraceFactory.Logger.Debug("Controller started.");
        }

        protected virtual void SubscribeToEventBus()
        {
            TraceFactory.Logger.Debug("Subscribing to Office Worker events");
           
                // Subscribe to all incoming messages.  These messages will originate from the Session Proxy
                // on the dispatcher server.  They include all the activities this controller will be asked to do.
                VirtualResourceEventBus.OnStartMainRun += VirtualResourceEventBus_OnStartRun;
                VirtualResourceEventBus.OnPauseResource += VirtualResourceEventBus_PauseResource;
                VirtualResourceEventBus.OnResumeResource += VirtualResourceEventBus_ResumeResource;
                VirtualResourceEventBus.OnShutdownResource += VirtualResourceEventBus_OnShutdownResource;
                VirtualResourceEventBus.OnHaltResource += VirtualResourceEventBus_HaltResource;
                VirtualResourceEventBus.OnReadyToRegister += VirtualResourceEventBus_OnReadyToRegister;
                VirtualResourceEventBus.OnTakeOffline += VirtualResourceEventBus_OnTakeOffline;
                VirtualResourceEventBus.OnBringOnline += VirtualResourceEventBus_OnBringOnline;
                VirtualResourceEventBus.OnSignalSynchronizationEvent += VirtualResourceEventBus_OnSignalSynchronizationEvent;

                // Subscribe to any events created within the Activity Execution environment
                EngineEventBus.ActivityStatusMessageChanged += EngineEventBus_ActivityStatusMessageChanged;

           
            TraceFactory.Logger.Debug("Subscribed to Office Worker events");
        }

        private void VirtualResourceEventBus_OnShutdownResource(object sender, VirtualResourceEventBusShutdownArgs e)
        {
            ChangeState(RuntimeState.ShuttingDown, e.CopyLogs);

            //VirtualResourceInstanceStatusLogger statusChange = new VirtualResourceInstanceStatusLogger(_sessionId, Environment.UserName, _workerStatusIndex++, Enum.GetName(typeof(RuntimeState), 12), false, GlobalDataStore.ResourceInstanceId);
            _statusLogger.Update(_statusLogger.Index + 1, Enum.GetName(typeof(RuntimeState), 12), false, "Shutdown");

            ExecutionServices.DataLogger.AsInternal().Submit(_statusLogger);
        }

        private void VirtualResourceEventBus_OnReadyToRegister(object sender, EventArgs e)
        {
            // Register and let the dispatcher know it's available to run.
            TraceFactory.Logger.Debug("Registering with proxy: {0}:{1} -> {2}"
                .FormatWith(GlobalDataStore.Manifest.HostMachine, GlobalDataStore.ResourceInstanceId, _commandService.BaseAddresses[0].AbsoluteUri));
            SessionProxyBackendConnection.RegisterResource(_commandService.BaseAddresses[0]);
            TraceFactory.Logger.Debug("Registering with proxy complete");
            ChangeState(RuntimeState.Registered);
        }

        protected virtual void VirtualResourceEventBus_OnStartRun(object sender, VirtualResourceEventBusRunArgs e)
        {
            CurrentPhase = e.Phase;

            TraceFactory.Logger.Debug("UserName: {0}, Phase: {1}".FormatWith(GlobalDataStore.ResourceInstanceId, CurrentPhase));

            if (IsHalted)
            {
                TraceFactory.Logger.Debug("Sending Halted state message");
                ChangeState(RuntimeState.Halted);
            }
            else
            {
                // Notify clients that this resource is now starting
                ChangeState(RuntimeState.Running);
                StartActivities();
            }
        }

        protected virtual void StartActivities()
        {
            // Run across all three phases back to back for the Office Worker
            // There may not be anything to run, which is fine.
            RunEngine(ResourceExecutionPhase.Setup);
            WaitOnStartupDelay();
            RunEngine(ResourceExecutionPhase.Main);
            RunEngine(ResourceExecutionPhase.Teardown);
            ChangeState(RuntimeState.Completed);
        }

        protected void RunEngine(ResourceExecutionPhase phase)
        {
            var engine = GetEngine(phase);

            if (!IsHalted)
            {
                if (!engine.Activities.Any())
                {
                    TraceFactory.Logger.Debug("Activity queue empty. This phase will not execute.");
                }
                else
                {
                    try
                    {
                        if (engine.GetType() == typeof(ScheduleBasedEngine))
                        {
                            ((ScheduleBasedEngine)engine).Run(_statusLogger);
                        }
                        else
                        {
                            engine.Run();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Debug(ex.Message);

                        if (ex is WorkerHaltedException)
                        {
                            IsHalted = true;
                        }
                    }
                }
            }
        }

        protected virtual EngineBase GetEngine(ResourceExecutionPhase phase)
        {
            EngineBase engine = null;

            if (!Engines.TryGetValue(phase, out engine))
            {
                var details = GetFilteredWorkerDetail(phase);
                if (phase != ResourceExecutionPhase.Main)
                {
                    details.ExecutionMode = ExecutionMode.Iteration;
                    details.RepeatCount = 1;
                }
                TraceFactory.Logger.Debug("Creating engine for {0} Phase".FormatWith(phase));
                engine = ObjectFactory.Create<EngineBase>(details.ExecutionMode, details);
                engine.ActivityStateChanged += OnActivityStateChanged;
                Engines.Add(phase, engine);
            }

            return engine;
        }

        protected OfficeWorkerDetail GetFilteredWorkerDetail(ResourceExecutionPhase phase)
        {
            TraceFactory.Logger.Debug("Phase: {0}".FormatWith(phase));

            // Create a copy of the manifest and trim out activities that are not part of this phase
            var manifest = SystemManifest.Deserialize(GlobalDataStore.Manifest.Serialize());
            var workerDetail = manifest.Resources.GetWorker<OfficeWorkerDetail>(GlobalDataStore.ResourceInstanceId);

            int count = workerDetail.MetadataDetails.Count();
            TraceFactory.Logger.Debug("Initial activity count {0}".FormatWith(count));

            var toRemove = workerDetail.MetadataDetails.Where(n => n.Plan.Phase != phase).ToList();
            foreach (var detail in toRemove)
            {
                workerDetail.MetadataDetails.Remove(detail);
            }

            count = workerDetail.MetadataDetails.Count();
            TraceFactory.Logger.Debug("Final activity count {0}".FormatWith(count));

            return workerDetail;
        }

        protected virtual void VirtualResourceEventBus_PauseResource(object sender, EventArgs e)
        {
            ApplicationFlowControl.Instance.OnExecutionPaused += Instance_OnExecutionPaused;
            ApplicationFlowControl.Instance.Pause();
        }

        protected virtual void Instance_OnExecutionPaused(object sender, EventArgs e)
        {
            // Once the execution is paused, after a client responds through the
            // ApplicationFlowControl, then disable the event and notify that it is complete.
            ApplicationFlowControl.Instance.OnExecutionPaused -= Instance_OnExecutionPaused;
            ChangeState(RuntimeState.Paused);
            //VirtualResourceInstanceStatusLogger statusChange = new VirtualResourceInstanceStatusLogger(_sessionId, Environment.UserName, _workerStatusIndex++, Enum.GetName(typeof(RuntimeState), 8), false, GlobalDataStore.ResourceInstanceId);
            _statusLogger.Update( _statusLogger.Index + 1, Enum.GetName(typeof(RuntimeState), 6), false, "User");
            
            ExecutionServices.DataLogger.AsInternal().Submit(_statusLogger);
        }

        protected virtual void VirtualResourceEventBus_ResumeResource(object sender, EventArgs e)
        {
            ApplicationFlowControl.Instance.Resume();
            ChangeState(RuntimeState.Running);
            //VirtualResourceInstanceStatusLogger statusChange = new VirtualResourceInstanceStatusLogger(_sessionId, Environment.UserName, _workerStatusIndex++, Enum.GetName(typeof(RuntimeState), 6), true, GlobalDataStore.ResourceInstanceId);
            _statusLogger.Update(_statusLogger.Index + 1, Enum.GetName(typeof(RuntimeState), 6), true, "User");

            ExecutionServices.DataLogger.AsInternal().Submit(_statusLogger);
        }

        private void VirtualResourceEventBus_HaltResource(object sender, EventArgs e)
        {
            CurrentEngine.Halt();
            IsHalted = true;
        }

        private void VirtualResourceEventBus_OnTakeOffline(object sender, VirtualResourceEventBusAssetArgs e)
        {
            TraceFactory.Logger.Debug("Received request to take {0} offline.".FormatWith(e.AssetId));
            GlobalDataStore.Manifest.Assets.SetAvailable(e.AssetId, false);
        }       
        
        private void VirtualResourceEventBus_OnBringOnline(object sender, VirtualResourceEventBusAssetArgs e)
        {
            TraceFactory.Logger.Debug("Received request to bring {0} online".FormatWith(e.AssetId));
            GlobalDataStore.Manifest.Assets.SetAvailable(e.AssetId, true);
        }

        private void VirtualResourceEventBus_OnSignalSynchronizationEvent(object sender, VirtualResourceEventBusSignalArgs e)
        {
            TraceFactory.Logger.Debug("Received request to broadcast".FormatWith(e.EventName));
            ApplicationFlowControl.Instance.SignalSync(e.EventName);
        }

        protected virtual void ChangeState(RuntimeState state, bool copyLogs = false)
        {
            var instanceId = GlobalDataStore.ResourceInstanceId;

            TraceFactory.Logger.Debug("{0} - change state to: {1}".FormatWith(instanceId, state));
            TraceFactory.Logger.Debug("Machine: {0}".FormatWith(GlobalDataStore.Manifest.HostMachine));

            SessionProxyBackendConnection.ChangeResourceState(state);

            if (WorkerStateChanged != null)
            {
                WorkerStateChanged(this, new ResourceEventArgs(instanceId, state, copyLogs));
            }
        }

        private void EngineEventBus_ActivityStatusMessageChanged(object sender, StatusChangedEventArgs e)
        {
            SessionProxyBackendConnection.ChangeResourceStatusMessage(e.StatusMessage);
        }

        protected void WaitOnStartupDelay()
        {
            OfficeWorkerDetail workerDetail = GlobalDataStore.Manifest.Resources.GetWorker<OfficeWorkerDetail>(GlobalDataStore.ResourceInstanceId);

            //if (workerDetail.ResourceType.IsCitrixWorker())
            //{
            //    return;
            //}

            TimeSpan minDelay = TimeSpan.FromSeconds(workerDetail.MinStartupDelay);
            TimeSpan maxDelay = TimeSpan.FromSeconds(workerDetail.MaxStartupDelay);
            var startupDelay = workerDetail.RandomizeStartupDelay ? TimeSpanUtil.GetRandom(minDelay, maxDelay) : minDelay;

            TraceFactory.Logger.Debug("Delay for {0} secs".FormatWith(startupDelay.TotalSeconds));


             
            _statusLogger.Caller = "StartUp";
            _statusLogger.Update(_statusLogger.Index + 1, Enum.GetName(typeof(RuntimeState), 8), true, "User");
            ExecutionServices.DataLogger.AsInternal().Submit(_statusLogger);
            ApplicationFlowControl.Instance.Wait(startupDelay);

            //CR 3192
            //We're seeing an issue with duplicate entry attempts on the primary key even though we generate a new key each time. To circumvent this issue we're creating a brand new instance, which in turn generates a new id and should prevent key issues.

            //VirtualResourceInstanceStatusLogger postwait = new VirtualResourceInstanceStatusLogger(_sessionId, Environment.UserName, ++_statusLogger.Index, Enum.GetName(typeof(RuntimeState), 6), true, GlobalDataStore.ResourceInstanceId);
            _statusLogger.Caller = "StartUp";

            _statusLogger.Update(_statusLogger.Index + 1, Enum.GetName(typeof(RuntimeState), 6), true, "StartUp");

            ExecutionServices.DataLogger.AsInternal().Submit(_statusLogger);

            
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
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
                if (_commandService != null)
                {
                    _commandService.Close();
                    _commandService = null;
                }

                foreach (var engine in Engines.Values)
                {
                    engine.Dispose();
                }
            }
        }

        #endregion IDisposable Members
    }
}
