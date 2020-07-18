using System;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Linq;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation.LoadTester
{
    [VirtualResourceHandler(VirtualResourceType.LoadTester)]
    internal class LoadTesterHandler : VirtualResourceHandler
    {
        LoadTesterActivityController _controller;

        public LoadTesterHandler(SystemManifest manifest)
            : base(manifest)
        {
            AttachUnhandledExceptionHandlers();
        }

        private void AttachUnhandledExceptionHandlers()
        {
            // Make sure the default unhandled exception handler has been removed.
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler.UnhandledExceptionMethod;
            AppDomain.CurrentDomain.UnhandledException += AppDomain_ThreadException;
        }

        private void AppDomain_ThreadException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = (Exception)e.ExceptionObject;
            TraceFactory.Logger.Fatal(error);
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Error);
            Environment.Exit(1);
        }

        public override void Start()
        {
            // Open the service endpoint so this resource can receive commands from the dispatcher,
            // then start the engine controller and then let the dispatcher know that this
            // resource is ready to run.

            var name = SystemManifest.Resources.OfType<LoadTesterDetail>().First().Name;

            ChangeResourceState(RuntimeState.Starting);

            OpenManagementServiceEndpoint(GlobalDataStore.ResourceInstanceId);

            TraceFactory.Logger.Debug("Service endpoint opened");

            _controller = new LoadTesterActivityController();
            _controller.Start();

            TraceFactory.Logger.Debug("Load tester activity controller started");

            // Register and let the dispatcher know it's available to run.
            SessionProxyBackendConnection.RegisterResource(ServiceEndpoint);

            TraceFactory.Logger.Debug("Dispatcher notified that system is ready to start");
        }

        public override void Cleanup()
        {
            // Dispose of the controller which will cascade down to dispose all plugins
            _controller.Dispose();
        }
    }
}
