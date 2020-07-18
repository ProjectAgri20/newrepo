using System;
using HP.ScalableTest.Framework.Manifest;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Settings;
using System.ServiceModel;
using System.ServiceModel.Channels;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Client connection class for <see cref="ISessionProxyBackend"/>.
    /// </summary>
    public sealed class SessionProxyBackendConnection : WcfClient<ISessionProxyBackend>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SessionProxyBackendConnection"/> class from being created.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private SessionProxyBackendConnection(MessageTransferType transferType, Uri endpoint)
            : base(transferType, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ServiceHost" />.
        /// </summary>
        /// <param name="singleton">The singleton.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>ServiceHost.</returns>
        public static ServiceHost CreateServiceHost(ISessionProxyBackend singleton, string sessionId)
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                Uri uri = WcfService.SessionBackend.GetLocalHttpUri(sessionId);
                return new WcfHost<ISessionProxyBackend>(singleton, MessageTransferType.CompressedHttp, uri);
            }
            else
            {
                Uri uri = WcfService.SessionBackend.GetLocalNamedPipeUri(sessionId);
                return new WcfHost<ISessionProxyBackend>(singleton, MessageTransferType.NamedPipe, uri);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SessionProxyBackendConnection" />.
        /// </summary>
        /// <param name="serviceHost">Hostname to connect to.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns></returns>
        public static SessionProxyBackendConnection Create(string serviceHost, string sessionId)
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                var uri = WcfService.SessionBackend.GetHttpUri(serviceHost, sessionId);
                return new SessionProxyBackendConnection(MessageTransferType.CompressedHttp, uri);
            }
            else
            {
                var uri = WcfService.SessionBackend.GetLocalNamedPipeUri(sessionId);
                return new SessionProxyBackendConnection(MessageTransferType.NamedPipe, uri);
            }
        }

        public static void ChangeResourceState(RuntimeState state)
        {
            Run(c => c.ChangeResourceState(GlobalDataStore.Manifest.HostMachine, GlobalDataStore.ResourceInstanceId, state));
        }
        public static void ChangeResourceState(string name, RuntimeState state)
        {
            Run(c => c.ChangeResourceState(GlobalDataStore.Manifest.HostMachine, name, state));
        }

        public static void RegisterResource(Uri endpoint)
        {
            Run(c => c.RegisterResource(GlobalDataStore.Manifest.HostMachine, GlobalDataStore.ResourceInstanceId, endpoint));
        }

        public static void ChangeResourceStatusMessage(string message)
        {
            Run(c => c.ChangeResourceStatusMessage(GlobalDataStore.Manifest.HostMachine, GlobalDataStore.ResourceInstanceId, message));
        }

        public static void ChangeMachineStatusMessage(string message)
        {
            Run(c => c.ChangeMachineStatusMessage(GlobalDataStore.Manifest.HostMachine, message));
        }

        public static void HandleAssetError(RuntimeError error)
        {
            SessionProxyBackendConnection.Run(c => c.HandleAssetError(error));
        }

        public static void SignalSynchronizationEvent(string eventName)
        {
            Run(n => n.SignalSynchronizationEvent(eventName));
        }

        public static void SaveLogFiles(string location)
        {
            LogFileDataCollection logFiles = LogFileDataCollection.Create(location);
            TraceFactory.Logger.Debug($"Location={location}.  Collection Size={Math.Round(logFiles.SizeInMb, 3)} MB.");

            Run(c => c.SaveLogFiles(logFiles));
        }

        /// <summary>
        /// Collects the device memory profile.
        /// </summary>
        /// <param name="serializedDeviceInfo">The serialized <see cref="IDeviceInfo"/> data.</param>
        /// <param name="snapshotLabel">The label for the memory profile.</param>
        public static void CollectDeviceMemoryProfile(string serializedDeviceInfo, string snapshotLabel)
        {
            var retryAction = new Action(() =>
            {
                using (var proxy = SessionProxyBackendConnection.Create(GlobalDataStore.Manifest.Dispatcher, GlobalDataStore.Manifest.SessionId))
                {
                    proxy.Channel.CollectDeviceMemoryProfile(serializedDeviceInfo, snapshotLabel);
                }
            });

            Retry.WhileThrowing(retryAction, 2, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });
        }

        /// <summary>
        /// Runs the specified <see cref="ISessionProxyBackend"/> action against the backend proxy service.
        /// </summary>
        /// <param name="action">The action.</param>
        private static void Run(Action<ISessionProxyBackend> action)
        {
            var retryAction = new Action(() =>
            {
                using (var proxy = SessionProxyBackendConnection.Create(GlobalDataStore.Manifest.Dispatcher, GlobalDataStore.Manifest.SessionId))
                {
                    action(proxy.Channel);
                }
            });

            try
            {
                Retry.WhileThrowing(retryAction, 2, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to communicate with Session Proxy backend.", ex);
            }
        }
    }
}