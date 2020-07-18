using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Admin Worker Engine Controller
    /// </summary>
    [ObjectFactory(VirtualResourceType.AdminWorker)]
    public class AdminWorkerEngineController : OfficeWorkerActivityController
    {
        private string _userName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientControllerHostName"></param>
        public AdminWorkerEngineController(string clientControllerHostName)
            : base(clientControllerHostName)
        {
            _userName = GlobalSettings.Items[Setting.DomainAdminUserName];
        }

        /// <summary>
        /// Returns Applicable Execution Phases
        /// </summary>
        protected override IEnumerable<ResourceExecutionPhase> ApplicablePhases
        {
            get
            {
                return Enum.GetValues(typeof(ResourceExecutionPhase)).Cast<ResourceExecutionPhase>();
            }
        }

        /// <summary>
        /// Subscribe to the Event
        /// </summary>
        protected override void SubscribeToEventBus()
        {
            base.SubscribeToEventBus();

            // If the resource type an Admin worker, then add the addition events to watch for
            // Admin Worker level Startup and Teardown events.  Otherwise ignore these events
            // as they don't apply to any other resources.
            TraceFactory.Logger.Debug("Subscribing to Admin Worker Setup/Teardown events");
            VirtualResourceEventBus.OnStartSetup += VirtualResourceEventBus_OnStartRun;
            VirtualResourceEventBus.OnStartTeardown += VirtualResourceEventBus_OnStartRun;
        }

        /// <summary>
        /// Get the Engine
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        protected override EngineBase GetEngine(ResourceExecutionPhase phase)
        {
            EngineBase engine = null;

            if (!Engines.TryGetValue(phase, out engine))
            {
                var detail = GetFilteredWorkerDetail(phase);
                engine = ObjectFactory.Create<EngineBase>(detail.ExecutionMode, detail);
                engine.ActivityStateChanged += OnActivityStateChanged;
                Engines.Add(phase, engine);
            }

            return engine;
        }


        /// <summary>
        /// Sets the State and starts the activities
        /// </summary>
        protected override void StartActivities()
        {
            RuntimeState state = RuntimeState.None;
            switch (CurrentPhase)
            {
                case ResourceExecutionPhase.Main:
                    state = RuntimeState.Completed;
                    break;
                case ResourceExecutionPhase.Setup:
                    state = RuntimeState.SetupCompleted;
                    break;
                case ResourceExecutionPhase.Teardown:
                    state = RuntimeState.TeardownCompleted;
                    break;
            }

            if (state != RuntimeState.None)
            {
                RunEngine(CurrentPhase);
                ChangeState(state);
            }
        }
    }
}
