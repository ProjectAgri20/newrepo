using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Linq;
using System.IO;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class that represents an instance of a virtual resource used in a test scenario.
    /// This is used as part of the overall <see cref="SessionMapObject"/> hierarchy that maps
    /// all resources and assets used in a test session to their virtual machine and asset
    /// counterparts.
    /// </summary>
    [ObjectFactory(VirtualResourceType.EventLogCollector)]
    [ObjectFactory(VirtualResourceType.LoadTester)]
    [ObjectFactory(VirtualResourceType.MachineReservation)]
    [ObjectFactory(VirtualResourceType.PerfMonCollector)]
    [ObjectFactory(VirtualResourceType.None)]
    public class ResourceInstance : ISessionMapElement
    {
        private AutoResetEvent _waitEvent = null;

        /// <summary>
        /// Gets the detail information contained in the <see cref="SystemManifest"/>.
        /// </summary>
        public ResourceDetailBase Detail { get; private set; }

        /// <summary>
        /// Gets or sets the name of this resource.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the session map element defined in the class inheriting this interface.
        /// </summary>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the endpoint associated with this resource.  This is a WCF endpoint that
        /// defines the network location for the real resource.
        /// </summary>
        public Uri Endpoint { get; private set; }

        /// <summary>
        /// Gets the collection of metadata (if any) for this resource.
        /// </summary>
        [SessionMapElementCollection]
        public Collection<ResourceMetadata> Metadata { get; private set; }

        /// <summary>
        /// Occurs when the state changes for this element.
        /// </summary>
        public event EventHandler<ResourceInstanceEventArgs> OnStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceInstance"/> class.
        /// </summary>
        /// <param name="instanceId">The unique key for this instance.</param>
        /// <param name="detail">The detail information from the <see cref="SystemManifest"/>.</param>
        public ResourceInstance(string instanceId, ResourceDetailBase detail)
        {
            Metadata = new Collection<ResourceMetadata>();
            Detail = detail;
            Id = instanceId;
            
            MapElement = new SessionMapElement(instanceId, ElementType.Worker)
            {
                ElementSubtype = detail.ResourceType.ToString(),
                ResourceId = Detail.ResourceId
            };

            Endpoint = null;

            // Add the metadata detail to the collection
            foreach (var data in Detail.MetadataDetails)
            {
                var metadata = new ResourceMetadata(Id, data);

                // Clear the message, we will not worry about showing anything at this level.
                metadata.MapElement.Message = string.Empty;

                Metadata.Add(metadata);
            }

            TraceFactory.Logger.Debug("{0} created".FormatWith(instanceId));
        }

        /// <summary>
        /// Starts the setup.
        /// </summary>
        public virtual void StartSetup()
        {
            TraceFactory.Logger.Debug("No Setup to perform for resource type {0} : {1}".FormatWith(Id, GetType()));
            // Leave this blank, if there is a resource specific child class it 
            // can implement this if needed.  This is run just before the 
            // overall Run() command.
        }

        /// <summary>
        /// Starts the teardown.
        /// </summary>
        public virtual void StartTeardown()
        {
            TraceFactory.Logger.Debug("No Teardown to perform for resource type {0} : {1}".FormatWith(Id, GetType()));
            // Leave this blank, if there is a resource specific child class it 
            // can implement this if needed.  This is run just before the 
            // overall Shutdown() command.
        }

        /// <summary>
        /// Builds all data structures for this asset.
        /// </summary>
        public virtual void Stage(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Available", RuntimeState.Available);
        }

        /// <summary>
        /// Initializes this asset host
        /// </summary>
        public virtual void Validate(ParallelLoopState loopState, SystemManifest manifest, string machineName)
        {
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }

        /// <summary>
        /// Turns on this asset host (bootup, power on, etc.).
        /// </summary>
        public virtual void PowerUp(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Ready", RuntimeState.Ready);
        }

        /// <summary>
        /// Executes this asset host, which may mean different things.
        /// </summary>
        public virtual void Run(ParallelLoopState loopState)
        {
            if (Endpoint == null)
            {
                MapElement.UpdateStatus("The endpoint for '{0}' is null".FormatWith(Id), RuntimeState.Error);
                return;
            }

            Action action = () =>
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("{0}: starting activities".FormatWith(Id));
                    client.Channel.StartMainRun();
                }
            };

            try
            {
                Retry.WhileThrowing(action, 2, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (EndpointNotFoundException ex)
            {
                MapElement.UpdateStatus("ComError", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Shuts down using the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loopState">State of the loop.</param>
        public virtual void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        /// <summary>
        /// Registers the specified endpoint with this resource instance.
        /// </summary>
        /// <param name="endpoint">The endpoint URI.</param>
        public void Register(Uri endpoint)
        {
            TraceFactory.Logger.Debug("Endpoint: {0}".FormatWith(endpoint));
            Endpoint = endpoint;
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            // Earlier it was determined that this instance was running, so it could be paused
            // if that has changed, the ResourceHost will still be waiting for this instance
            // to flag that it is paused.  So go ahead and send a state changed notice
            // that will take care of it.  But don't actually change the state of this instance.
            if (MapElement.State != RuntimeState.Running)
            {
               SendStateChangeEvent(RuntimeState.Paused);
               return;
            }

            MapElement.UpdateStatus("Pausing", RuntimeState.Pausing);
            Action action = () =>
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("{0}: pausing activities".FormatWith(Id));
                    client.Channel.PauseResource();
                }
            };

            try
            {
                int retries = Retry.WhileThrowing(action, 2, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });
                TraceFactory.Logger.Debug("RETRIES: {0}".FormatWith(retries));
            }
            catch (EndpointNotFoundException ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Resumes this instance.
        /// </summary>
        public void Resume()
        {
            MapElement.UpdateStatus("Resuming");

            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("{0}: resuming activities".FormatWith(Id));
                    client.Channel.ResumeResource();
                }
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Halts this instance.
        /// </summary>
        public void Halt()
        {
            MapElement.UpdateStatus("Halting", RuntimeState.Halting);

            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("{0}: halting activities".FormatWith(Id));
                    client.Channel.HaltResource();
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("ComError", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Sends a synchronization signal with the specified event name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug($"{Id}: sending sync event '{eventName}");
                    client.Channel.SignalSynchronizationEvent(eventName);
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Suspends operations to an asset which may or may not be used by this instance.
        /// </summary>
        /// <param name="assetId"></param>
        public void TakeOffline(string assetId)
        {
            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("Taking Asset {0} Offline".FormatWith(assetId));
                    client.Channel.TakeOffline(assetId);
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Resumes operations to an asset which may or may not be used by this instance.
        /// </summary>
        /// <param name="assetId"></param>
        public void BringOnline(string assetId)
        {
            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("Bringing Asset {0} Online".FormatWith(assetId));
                    client.Channel.BringOnline(assetId);
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Pings this instance for a responce.
        /// </summary>
        public void CheckHealth()
        {
            try
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    client.Channel.Ping();
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Not Responding", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Shuts down this instance.
        /// </summary>
        public void Shutdown(ShutdownOptions options)
        {
            if (Endpoint == null)
            {
                TraceFactory.Logger.Info("{0} never registered, so marking as already shut down.".FormatWith(Id));
                ChangeState(RuntimeState.Offline);
                return;
            }

            MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

            try
            {
                TraceFactory.Logger.Debug("{0} - Shutdown Signaled".FormatWith(Endpoint));
                ChangeState(RuntimeState.ShuttingDown);

                var action = new Action(() =>
                {
                    using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                    {
                        client.Channel.Shutdown(options.CopyLogs);
                    }
                });

                var listofExceptions = new List<Type>() { typeof(EndpointNotFoundException), typeof(TimeoutException) };
                int retries = Retry.WhileThrowing(action, 2, TimeSpan.FromSeconds(20), listofExceptions);

                TraceFactory.Logger.Debug("{0} -> Shutdown Complete: RETRIES: {1}".FormatWith(Endpoint, retries));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("{0} -> Shutdown Failed, continuing: {1}".FormatWith(Endpoint,ex.ToString()));
                //ChangeState(RuntimeState.Offline);
                //MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            }
            finally
            {
                //Only used to speed up shutdown in ResourceHost
                ChangeState(RuntimeState.Offline);
                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            }
        }

        /// <summary>
        /// Updates this instance with the specified message.
        /// </summary>
        /// <param name="message"></param>
        public void ChangeStatusMessage(string message)
        {
            MapElement.UpdateStatus(message);
        }

        /// <summary>
        /// Forwards any resource state changes onto listening clients
        /// </summary>
        /// <param name="state">The runtime state.</param>
        public void ChangeState(RuntimeState state)
        {
            //TraceFactory.Logger.Debug("{0}:{1}".FormatWith(ResourceName, state));
            MapElement.UpdateStatus(state.ToString(), state);
            SendStateChangeEvent(state);
        }

        private void SendStateChangeEvent(RuntimeState state)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(this, new ResourceInstanceEventArgs()
                {
                    InstanceId = Id,
                    State = state
                });
            }
        }

        /// <summary>
        /// Performs the defined action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void PerformAction(Action<IVirtualResourceManagementService> action)
        {
            if (Endpoint == null)
            {
                MapElement.UpdateStatus("The endpoint for '{0}' is null".FormatWith(Id), RuntimeState.Error);
                return;
            }

            Action retryAction = () =>
            {
                using (var client = VirtualResourceManagementConnection.Create(Endpoint))
                {
                    TraceFactory.Logger.Debug("{0}: starting activities".FormatWith(Id));
                    action(client.Channel);
                }
            };

            try
            {
                _waitEvent = new AutoResetEvent(false);
                var listofExceptions = new List<Type>() { typeof(EndpointNotFoundException), typeof(TimeoutException) };
                int retries = Retry.WhileThrowing(retryAction, 2, TimeSpan.FromSeconds(1), listofExceptions);
                TraceFactory.Logger.Debug("Message sent, waiting for completion: RETRIES: {0}".FormatWith(retries));
                _waitEvent.WaitOne();
            }
            catch (EndpointNotFoundException ex)
            {
                MapElement.UpdateStatus("Error", RuntimeState.Error);
                TraceFactory.Logger.Error(ex);
            }
            finally
            {
                if (_waitEvent != null)
                {
                    _waitEvent.Dispose();
                    _waitEvent = null;
                }
            }
        }

        /// <summary>
        /// Releases this resource instance.
        /// </summary>
        public void Release()
        {
            if (_waitEvent != null)
            {
                TraceFactory.Logger.Debug("{0} is completed and being released".FormatWith(Id));
                _waitEvent.Set();
            }
        }
    }
}
