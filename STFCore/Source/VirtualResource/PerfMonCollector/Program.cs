using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.VirtualResource.PerfMonCollector
{
    internal class Program : IDisposable
    {
        private string _sessionId = string.Empty;
        private ServiceHost _commandService = null; //Host for incoming WCF calls
        private Uri _serviceEndpoint = null;

        //collection to which the data from manifest is being stored
        private Collection<PerfMonCounterData> _perfMonCounters;

        //collection, which groups the counters by Interval
        private Collection<PerfMonCounterByInterval> _perfMonCountersInterval;

        /// <summary>
        ///
        /// </summary>
        /// <param name="resourceId"></param>
        public Program(string resourceId)
        {
            //Set up the local service port
            LoadServiceEndpoint();

            // Sign up for the event that is fired when the client tells us to run.
            VirtualResourceEventBus.OnStartMainRun += EventBus_OnStart;
            VirtualResourceEventBus.OnShutdownResource += EventBus_OnStop;
            VirtualResourceEventBus.OnPauseResource += EventBus_OnPause;
            VirtualResourceEventBus.OnResumeResource += EventBus_OnResume;

            LoadManifest(ref resourceId);

            InitializeCounters();

            SessionProxyBackendConnection.RegisterResource(_serviceEndpoint);

            TraceFactory.Logger.Info("Monitoring {0}".FormatWith(GlobalDataStore.ResourceInstanceId));
            //TraceFactory.Logger.Debug("Counters to monitor: " + components); //TODO: Hook this up
        }

        /// <summary>
        /// args[0] = VirtualResourceId
        /// args[1] = Host Name
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Thread.CurrentThread.SetName("PerfMoncollectorMain");
            TraceFactory.SetThreadContextProperty("ResourceName", "PerfMonCounter_" + DateTime.Now.ToString("ddMMyyy", CultureInfo.InvariantCulture));

            UnhandledExceptionHandler.Attach();

            using (Program program = new Program(args[0]))
            {
                Console.ReadLine();
            }
        }

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
                if (_commandService != null)
                {
                    _commandService.Close();
                }

                _perfMonCounters.Clear();
                _perfMonCountersInterval.Clear();
            }
        }

        private void EventBus_OnStart(object sender, EventArgs e)
        {
            TraceFactory.Logger.Info("Start message received.");
            Console.WriteLine("Starting counters");

            //start monitoring based on interval
            foreach (PerfMonCounterByInterval counter in _perfMonCountersInterval)
            {
                counter.Start();
            }

            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Running);
        }

        /// <summary>
        /// Event raised by the client factory when the "Shutdown" call has been made.
        /// </summary>
        private void EventBus_OnStop(object sender, EventArgs e)
        {
            TraceFactory.Logger.Info("Stop message received.");

            foreach (PerfMonCounterByInterval counter in _perfMonCountersInterval)
            {
                counter.Stop();
            }

            TraceFactory.Logger.Debug("Logs flushed, sending offline signal");

            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Offline);
        }

        private void EventBus_OnPause(object sender, EventArgs e)
        {
            TraceFactory.Logger.Info("Pause message received.");
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Paused);
        }

        private void EventBus_OnResume(object sender, EventArgs e)
        {
            TraceFactory.Logger.Info("Resume message received.");
        }

        /// <summary>
        /// initalizing counters function from vedavyas, the counters are loaded and sorted according to intervals and in turn grouped by machines
        /// </summary>
        private void InitializeCounters()
        {
            _perfMonCountersInterval = new Collection<PerfMonCounterByInterval>();

            // find the intervals chosen by the user
            var uniqueIntervals = _perfMonCounters.Select(c => c.Interval).Distinct();

            // group by distinct intervals
            foreach (var interval in uniqueIntervals)
            {
                PerfMonCounterByInterval tempCounterInterval = null;
                try
                {
                    tempCounterInterval = new PerfMonCounterByInterval(interval);

                    // find the counters matching our interval
                    var countersByInterval = from p in _perfMonCounters where p.Interval == interval select p;

                    // find the targethosts for the counters with current chosen interval
                    var distinctHosts = countersByInterval.Select(c => c.TargetHost).Distinct();

                    // group the counters by machine (targethost) within in that interval
                    foreach (var distinctHost in distinctHosts)
                    {
                        PerfMonCounterByMachine tempCounterMachine = new PerfMonCounterByMachine();
                        var countersByMachine = from p in countersByInterval where p.TargetHost == distinctHost select p;

                        tempCounterMachine.TargetHost = distinctHost;
                        // the user credential will be same for the machine, take the first one
                        tempCounterMachine.Credentials = countersByMachine.FirstOrDefault().Credentials;
                        tempCounterMachine.SessionId = _sessionId;

                        // add the counter for the machine
                        foreach (var counter in countersByMachine)
                        {
                            // while adding the counters, the system checks for the validity of the parameters passed, if the machine is unreachable then it throws an exception
                            try
                            {
                                TraceFactory.Logger.Info("HostName:{0}    Category:{1}  Counter:{2}  Instance{3}".FormatWith(distinctHost, counter.Category, counter.Counter, counter.InstanceName));
                                PerformanceCounter tempCounter = new PerformanceCounter(counter.Category, counter.Counter, counter.InstanceName, distinctHost);
                                tempCounterMachine.Counters.Add(tempCounter);
                            }
                            catch (UnauthorizedAccessException unauthException)
                            {
                                TraceFactory.Logger.Error(unauthException);
                            }
                            catch (InvalidOperationException invoperationException)
                            {
                                TraceFactory.Logger.Error(invoperationException);
                            }
                            catch (ArgumentException argException)
                            {
                                TraceFactory.Logger.Error(argException);
                            }
                        }

                        // load this counter of the host to the collection and then proceed to further hosts, which have the same interval
                        tempCounterInterval.CountersByMachine.Add(tempCounterMachine);
                    }
                    _perfMonCountersInterval.Add(tempCounterInterval);
                }
                catch (Exception)
                {
                    if (tempCounterInterval != null)
                    {
                        tempCounterInterval.Dispose();
                        tempCounterInterval = null;
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Opens a service endpoint for the PerfMonCollector that other clients can talk to
        /// </summary>
        private void LoadServiceEndpoint()
        {
            if (_serviceEndpoint == null)
            {
                _commandService = VirtualResourceManagementConnection.CreateServiceHost(Environment.MachineName, (int)WcfService.VirtualResource);
                _serviceEndpoint = _commandService.BaseAddresses[0];
                _commandService.Open();
            }
        }

        private void LoadManifest(ref string instanceId)
        {
            // Get the manifest from the factory
            TraceFactory.Logger.Debug("Attempting to get the manifest from the Factory Service");
            SystemManifest manifest = null;
            using (var clientController = ClientControllerServiceConnection.Create(Environment.MachineName))
            {
                var data = clientController.Channel.GetManifest(instanceId);
                manifest = SystemManifest.Deserialize(data);
            }

            // Cache the definition that corresponds to the resourceId
            var definition = manifest.Resources.GetByType(VirtualResourceType.PerfMonCollector).FirstOrDefault();
            if (definition == null)
            {
                throw new InvalidOperationException("Resource Definition is null.");
            }

            manifest.PushToGlobalSettings();
            manifest.PushToGlobalDataStore(definition.Name);

            _sessionId = manifest.SessionId;

            // get all the resources which are of type perfmoncollector
            var perfMonResources = manifest.Resources.Where(c => c.ResourceType == VirtualResourceType.PerfMonCollector);

            // if we have permoncounter collectors then we read manifest
            if (perfMonResources.Count() > 0)
            {
                ReadManifest(perfMonResources);
            }
            else
            {
                throw new InvalidOperationException("No performance counters to monitor");
            }


            TraceFactory.Logger.Info(Environment.NewLine + manifest.ToString());
        }

        /// <summary>
        /// reads the performance counters from the manifest
        /// </summary>
        /// <param name="perfResources"></param>
        private void ReadManifest(IEnumerable<ResourceDetailBase> perfResources)
        {
            _perfMonCounters = new ObservableCollection<PerfMonCounterData>();

            foreach (var perfResource in perfResources)
            {
                foreach (var data in perfResource.MetadataDetails)
                {
                    var tempCounterData = LegacySerializer.DeserializeXml<PerfMonCounterData>(data.Data);
                    _perfMonCounters.Add(tempCounterData);
                }
            }
        }
    }
}