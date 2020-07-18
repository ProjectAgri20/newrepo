using System;
using System.Linq;
using System.ServiceModel;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Abstract base class for all handlers that manage the creation and execution of virtual resources
    /// on the client VMs.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = true)]
    public abstract class VirtualResourceHandler : IVirtualResourceHandler, IClientControllerService, IDisposable
    {
        private ServiceHost _managementService = null;
        private ServiceHost _clientService = null;

        protected Uri ServiceEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the manifest.
        /// </summary>
        /// <value>The manifest.</value>
        protected SystemManifest SystemManifest { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceHandler" /> class.
        /// </summary>
        protected VirtualResourceHandler()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceHandler" /> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <exception cref="System.ArgumentNullException">manifest</exception>
        protected VirtualResourceHandler(SystemManifest manifest)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            SystemManifest = manifest;

            if (_clientService == null)
            {
                try
                {
                    _clientService = ClientControllerServiceConnection.CreateServiceHost(this);
                    _clientService.Open();
                    TraceFactory.Logger.Debug("Started Service Host: {0}".FormatWith(_clientService.BaseAddresses[0].AbsoluteUri));
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Creates the resources.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Changes the machine status.
        /// </summary>
        /// <param name="message">The status.</param>
        public static void ChangeMachineStatusMessage(string message)
        {
            SessionProxyBackendConnection.ChangeMachineStatusMessage(message);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="state">The state.</param>
        public static void ChangeResourceState(RuntimeState state)
        {
            SessionProxyBackendConnection.ChangeResourceState(state);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="resourceInstanceId">Unique id for this resource.</param>
        /// <param name="state">The state.</param>
        public static void ChangeResourceState(string resourceInstanceId, RuntimeState state)
        {
            SessionProxyBackendConnection.ChangeResourceState(resourceInstanceId, state);
        }

        public static void RegisterResource(string resourceInstanceId, Uri endpoint)
        {
            SessionProxyBackendConnection.RegisterResource(endpoint);
        }

        #region IClientControllerService

        /// <summary>
        /// Gets the manifest.
        /// </summary>
        /// <param name="instanceId">Unique identifier for this resource instance.</param>
        /// <returns></returns>
        public virtual string GetManifest(string instanceId)
        {
            TraceFactory.Logger.Debug("Manifest requested by {0}".FormatWith(instanceId));
            return SystemManifest.Serialize();
        }

        /// <summary>
        /// Tells the service to perform any cleanup activities.
        /// </summary>
        public virtual void Cleanup()
        {
            // Do nothing here, child classes will implement as needed.
        }

        /// <summary>
        /// Tells the service to copy any logs that are unique to this resource.
        /// </summary>
        public virtual LogFileDataCollection GetLogFiles(string sessionId)
        {
            return LogFileDataCollection.Create(LogFileReader.DataLogPath());
        }

        /// <summary>
        /// Notifies the state of the worker to the main controller.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="endpoint">The endpoint used to communicate back to the worker.</param>
        /// <param name="state">The state of the worker being sent to the controller.</param>
        public virtual void NotifyResourceState(Uri endpoint, RuntimeState state)
        {
            // Do nothing here, child classes will implement as needed.
        }

        #endregion

        /// <summary>
        /// Registers the resource using the currently defined management endpoint.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <exception cref="System.InvalidOperationException">Unable to start service.</exception> 
        protected void OpenManagementServiceEndpoint(string resourceName)
        {
            _managementService = VirtualResourceManagementConnection.CreateServiceHost(Environment.MachineName, (int)WcfService.VirtualResource);
            _managementService.Open();
            ServiceEndpoint = _managementService.BaseAddresses[0];
        }

        #region IDisposable
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
            if (!disposing)
            {
                return;
            }

            if (_clientService != null)
            {
                ((IDisposable)_clientService).Dispose();
                _clientService = null;
            }

            if (_managementService != null)
            {
                ((IDisposable)_managementService).Dispose();
                _managementService = null;
            }
        }
        #endregion
    }
}
