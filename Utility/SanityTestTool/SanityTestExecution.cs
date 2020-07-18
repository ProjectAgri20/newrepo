using System;
using System.Linq;
using System.ServiceModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Network.Wcf;

namespace HP.ScalableTest.Tools
{
    static class SanityTestExecution
    {
        static ServiceHost _sessionManagementService = null;
        static SessionTicket _ticket                 = null;
        static bool _isSessionCompleted              = false;
        static bool _sessionError                    = false;
        static string _clientVM                      = string.Empty;
        static string _errorDetails                  = string.Empty;

        public static bool Start(string dispatcher, string database, string scenario, out string clientVM, out string sessionId, out string result)
        {
            _clientVM           = string.Empty;
            _errorDetails       = string.Empty;
            _isSessionCompleted = false;
            _sessionError       = false;

            Console.WriteLine("Starting session...");

            GlobalSettings.SetDispatcher(dispatcher);
            GlobalSettings.Load(database);

            _ticket = SessionTicket.Create(scenario);
            _ticket.SessionOwner = new UserCredential("youngmak", "AMERICAS.CPQCORP.NET");
            Console.WriteLine("Created ticket {0}".FormatWith(_ticket.SessionId));

            SessionClient.Instance.DispatcherExceptionReceived += DispatcherErrorReceived;
            SessionClient.Instance.SessionStateReceived += SessionStateChanged;
            SessionClient.Instance.SessionStartupTransitionReceived += SessionStartupTransitionReceived;
            SessionClient.Instance.SessionMapElementReceived += Instance_SessionMapElementReceived;

            TraceFactory.Logger.Debug("Initializing");
            SessionClient.Instance.Initialize(dispatcher);

            SessionClient.Instance.InitiateSession(_ticket);

            var assetDetails = SessionClient.Instance.Reserve(_ticket.SessionId);

            Console.WriteLine("Reserved...{0}".FormatWith(_ticket.SessionId));

            foreach (var asset in assetDetails.Where(x => x.Availability == AssetAvailability.NotAvailable))
            {
                Console.WriteLine("Unavailable: {0}".FormatWith(asset.AssetId));
            }

            // This call to Stage() will kick off the process and as each event arrives to indicate
            // a step in the process has completed, the next step will automatically continue.
            SessionClient.Instance.Stage(_ticket.SessionId, assetDetails);
            Console.WriteLine("Staged...{0}".FormatWith(_ticket.SessionId));

            _isSessionCompleted = false;

            while (!_isSessionCompleted)
            {
                // block till the session completed or throws error
            }            
            
            sessionId = _ticket.SessionId;
            clientVM = _clientVM;

            if (_sessionError)
            {
                result = _errorDetails;
                return false;
            }
            else
            {
                result = "Executed Successful";
                return true;
            }
        }

        static void Instance_SessionMapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            if (e.MapElement.SessionId.Equals(_ticket.SessionId))
            {
                //if (e.MapElement.ElementType == ElementType.ResourceHost)
                //{
                //    _clientVM = e.MapElement.Name;
                //}                
                Console.WriteLine("  [{0}] {1,40} : {2}/{3}/{4} -> [{5}]".FormatWith(e.MapElement.SessionId, e.MapElement.Name, e.MapElement.ElementType, e.MapElement.ElementSubtype, e.MapElement.State, e.MapElement.Message));
            }
        }

        static void SessionStartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            Console.WriteLine("{0}".FormatWith(e.Transition));

            switch (e.Transition)
            {
                case SessionStartupTransition.ReadyToValidate:
                    SessionClient.Instance.Validate(_ticket.SessionId);
                    break;
                case SessionStartupTransition.ReadyToPowerUp:
                    SessionClient.Instance.PowerUp(_ticket.SessionId);
                    break;
                case SessionStartupTransition.ReadyToRun:
                    SessionClient.Instance.Run(_ticket.SessionId);
                    break;
            }
        }

        static void Server()
        {
            _sessionManagementService = WcfServiceFactory.CreateHost
                (
                    typeof(ISessionDispatcher),
                    typeof(ISessionDispatcher),
                    WcfService.SessionServer.GetLocalHttpUri(),
                    Network.Wcf.MessageTransferType.CompressedHttp
                );
            Console.WriteLine("Starting Session Management Service: {0}".FormatWith(WcfService.SessionServer.GetLocalHttpUri().AbsoluteUri));
            _sessionManagementService.Open();
        }

        static void SessionStateChanged(object sender, SessionStateEventArgs e)
        {
            if (e.SessionId.Equals(_ticket.SessionId))
            {
                Console.WriteLine("{0}".FormatWith(e.State));

                switch (e.State)
                {
                    case SessionState.RunComplete:
                        // This is a place where you could add logic to decide if you want to
                        // Repeat() the run, or continue to shutdown.  For now, we are just 
                        // going to shutdown.
                        ShutdownOptions options = new ShutdownOptions()
                        {
                            AllowWorkerToComplete = false,
                            CopyLogs = false,
                            PowerOff = true,
                            PowerOffOption = VMPowerOffOption.RevertToSnapshot
                        };

                        
                        SessionClient.Instance.Shutdown(_ticket.SessionId, options);
                        break;
                    case SessionState.ShutdownComplete:
                        _isSessionCompleted = true;
                        break;
                }
            }
        }

        static void DispatcherErrorReceived(object sender, ExceptionDetailEventArgs e)
        {
            Console.WriteLine("ERROR -----{0}{1}{0}{2}".FormatWith(Environment.NewLine, e.Message, e.Detail));
            _errorDetails = "ERROR -----{0}{1}{0}{2}".FormatWith(Environment.NewLine, e.Message, e.Detail);
            _isSessionCompleted = true;
            _sessionError = true;
        }
    }
}
