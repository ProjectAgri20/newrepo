using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    internal class Program
    {
        private static AutoResetEvent _mainThreadBlock = new AutoResetEvent(false);
        private static string _sessionId = string.Empty;

        static void Main(string[] args)
        {
            var database = args[0];
            _sessionId = args[1];
            var proxyServiceUri = new Uri(args[2]);

            TraceFactory.SetThreadContextProperty("PID", Process.GetCurrentProcess().Id.ToString());
            TraceFactory.SetSessionContext(_sessionId);
            TraceFactory.Logger.Debug(string.Join(", ", args));

            UnhandledExceptionHandler.Attach();

            GlobalSettings.Load(database);
            FrameworkServicesInitializer.InitializeExecution();

            try
            {
                using (var sessionProxy = new SessionProxy(_sessionId))
                {
                    sessionProxy.OnExit += _sessionProxy_OnExit;
                    sessionProxy.StartFrontendService(proxyServiceUri);
                    sessionProxy.StartBackendService();

                    TraceFactory.Logger.Debug("Notify Dispatcher process is up...");
                    Retry.WhileThrowing
                        (
                            () =>
                            {
                                using (var connection = SessionDispatcherConnection.Create("localhost"))
                                {
                                    connection.Channel.NotifyProxyStarted(_sessionId);
                                }
                            },
                            10,
                            TimeSpan.FromSeconds(2),
                            new List<Type>() { typeof(EndpointNotFoundException) }
                        );
                    TraceFactory.Logger.Debug("Notify Dispatcher process is up...Done");

                    _mainThreadBlock.WaitOne();
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
            }
        }

        private static void _sessionProxy_OnExit(object sender, EventArgs e)
        {            
            _mainThreadBlock.Set();
        }
    }
}
