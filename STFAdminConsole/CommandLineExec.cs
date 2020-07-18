using System;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections.Generic;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Utility that allows the unattended execution of a scenario via command-line.
    
    /// </summary>
    public class CommandLineExec : IDisposable
    {
        private NameValueCollection _arguments = null;
        private SessionTicket _ticket = null;
        private List<SessionTicket> _ticketlist = null;
        private SessionState _state = SessionState.None;
        private bool _runCompleted = false;

        /// <summary>
        /// Allocate and attach a new command console window to this instance.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// Frees the attached command console.
        /// </summary>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        /// <summary>
        /// Publish status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineExec"/> class.
        /// Validates the App Config key-value entries.
        /// </summary>
        /// <param name="appConfig">The App Config arguments.</param>
        public CommandLineExec(NameValueCollection appConfig)
        {
            AllocConsole();
            TraceFactory.SetThreadContextProperty("PID", Process.GetCurrentProcess().Id.ToString(), false);
            _arguments = appConfig;

            _state = SessionState.Available;
            string dispatcher = string.Empty;
            string database = string.Empty;
            string sessionName = string.Empty;
            int durationHours = 2;
            IEnumerable<string> scenarios = null;
            
            if (_arguments != null)
            {
                scenarios = _arguments["scenarios"].ToString().Split(';');
                dispatcher = string.IsNullOrEmpty(_arguments["dispatcher"]) ? Environment.MachineName : _arguments["dispatcher"].ToString();
                database = _arguments["database"].ToString();
                sessionName= _arguments["sessionName"].ToString();
                durationHours = string.IsNullOrEmpty(_arguments["durationHours"]) ? 2 : Convert.ToInt32(_arguments["durationHours"]);
            }

            if (string.IsNullOrEmpty(dispatcher))
            {
                _state = SessionState.Error;
                throw new ArgumentException("Dispatcher is required.", "dispatcher");
            }
            TraceFactory.SetThreadContextProperty("Dispatcher", dispatcher, false);

            if (string.IsNullOrEmpty(sessionName))
            {
                _state = SessionState.Error;
                throw new ArgumentException("Session Name is required.", "sessionName");
            }
            if (string.IsNullOrEmpty(database))
            {
                _state = SessionState.Error;
                throw new ArgumentException("Database Host is required.", "database");
            }

            if (string.IsNullOrEmpty(_arguments["owner"].ToString()))
            {
                _state = SessionState.Error;
                throw new ArgumentException("Session Owner User name is required.", "owner");
            }

            if (GlobalSettings.IsDistributedSystem)
            {
                //We only care about password and domain if it's STF
                if (string.IsNullOrEmpty(_arguments["password"]))
                {
                    _state = SessionState.Error;
                    throw new ArgumentException("Session owner Password is required.", "password");
                }

                if (string.IsNullOrEmpty(_arguments["domain"]))
                {
                    _state = SessionState.Error;
                    throw new ArgumentException("User Domain is required.", "domain");
                }
            }

            // No need to check optional args.  They will be handled later.

            //Initialize the environment and create a ticket.
            GlobalSettings.SetDispatcher(dispatcher);
            GlobalSettings.Load(database);

            _ticket = SessionTicket.Create(scenarios, sessionName, durationHours);
            _ticket.SessionOwner = GetSessionOwner(_arguments["owner"], _arguments["password"], _arguments["domain"] ?? Environment.UserDomainName);
            _ticket.ExpirationDate = SessionLogRetention.Month.GetExpirationDate(DateTime.Now);
        }

        /// <summary>
        /// Validates the App Config data to either start as unattended or UI based execution
        /// </summary>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public static int GetAppConfigCount(NameValueCollection appConfig)
        {
            return (appConfig == null) ? 0 : appConfig.AllKeys.Length;
        }

        /// <summary>
        /// Sets whether SessionClient events will be handled by this instance or not.
        /// Defaults to true;
        /// </summary>
        public bool HandleSessionClientEvents { get; set; } = true;

        /// <summary>
        /// Gets the Session Ticket.
        /// </summary>
        public SessionTicket Ticket
        {
            get { return _ticket; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                FreeConsole();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Starts the session.
        /// If the startup args were not successfully validated, an exception is thrown.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Invalid State.  Unable to start session.</exception>
        public void StartSession()
        {
            if (_state != SessionState.Available)
            {
                throw new InvalidOperationException("Unable to start session. State: {0}.".FormatWith(_state));
            }

            if (HandleSessionClientEvents)
            {
                SessionClient.Instance.DispatcherExceptionReceived += SessionClient_DispatcherErrorReceived;
                SessionClient.Instance.SessionStateReceived += SessionClient_StateChanged;
                SessionClient.Instance.SessionStartupTransitionReceived += SessionClient_StartupTransitionReceived;
                SessionClient.Instance.SessionMapElementReceived += SessionClient_MapElementReceived;
            }
            else
            {
                FreeConsole();
            }

            string reservationKey = _arguments["reservation"]?.ToString();

            UpdateStatus(string.Empty);
            UpdateStatus($"Starting session '{_ticket.SessionName}'");
            UpdateStatus(string.Empty);

            UpdateStatus("Created ticket {0}".FormatWith(_ticket.SessionId));

            TraceFactory.Logger.Debug("Initializing");
            SessionClient.Instance.Initialize(_arguments["dispatcher"]);

            SessionClient.Instance.InitiateSession(_ticket);

            UpdateStatus("Reserving Assets...");
            AssetDetailCollection assetDetails = null;
            if (string.IsNullOrEmpty(reservationKey))
            {
                assetDetails = SessionClient.Instance.Reserve(_ticket.SessionId);
            }
            else
            {
                UpdateStatus("Reservation Key: {0}".FormatWith(reservationKey));
                assetDetails = SessionClient.Instance.Reserve(_ticket.SessionId, reservationKey);
            }

            //Check asset availability
            if (! AllAssetsAvailable(assetDetails))
            {
                string message = "Not all assets are available.  Unable to continue.";
                UpdateStatus(message);
                TraceFactory.Logger.Error(message);
                ShutDown();
                return;
            }

            // This call to Stage() will kick off the process and as each event arrives to indicate
            // a step in the process has completed, the next step will automatically continue.
            SessionClient.Instance.Stage(_ticket.SessionId, assetDetails);
            UpdateStatus("Staged...{0}".FormatWith(_ticket.SessionId));

            if (HandleSessionClientEvents)
            {
                //Keep the console open
                Console.ReadLine();
            }
        }

        private bool LogToConsole(string value)
        {
            if (value != null)
            {
                return bool.Parse(value);
            }

            return true;
        }

        private UserCredential GetSessionOwner(string userName, string password, string domain)
        {
            UserCredential result = new UserCredential(userName, password, domain);

            return result;
        }

        private bool AllAssetsAvailable(AssetDetailCollection assetDetails)
        {
            bool result = true;

            string display = string.Empty;
            foreach (AssetDetail asset in assetDetails)
            {
                display = "{0} - {1}".FormatWith(asset.AssetId, asset.Availability);
                UpdateStatus(display);
                TraceFactory.Logger.Debug(display);
                result = result && (asset.Availability == AssetAvailability.Available);
            }

            return result;
        }

        private void SessionClient_MapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            if (e.MapElement.SessionId.Equals(_ticket.SessionId))
            {
                UpdateStatus("  [{0}] {1,40} : {2}/{3}/{4} -> [{5}]".FormatWith(e.MapElement.SessionId, e.MapElement.Name, e.MapElement.ElementType, e.MapElement.ElementSubtype, e.MapElement.State, e.MapElement.Message));
            }
        }

        private void SessionClient_StartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            UpdateStatus("Startup Transition: {0}".FormatWith(e.Transition));

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

        private void SessionClient_StateChanged(object sender, SessionStateEventArgs e)
        {
            if (e.SessionId.Equals(_ticket.SessionId))
            {
                _state = e.State;
                UpdateStatus("Session State Change: {0} {1}".FormatWith(e.State, e.Message));

                switch (e.State)
                {
                    case SessionState.RunComplete:
                    {
                        // This is a place where you could add logic to decide if you want to
                        // Repeat() the run, or continue to shutdown.  For now, we are just 
                        // going to shutdown.
                        ShutDown();
                        _runCompleted = true;
                        break;
                    }
                    case SessionState.ShutdownComplete:
                    {
                        //Close the environment only when the Run completes .Otherwise this will close the main thread and not all Scenarios get executed 
                        if (_runCompleted)
                        {
                            Environment.Exit(0);
                        }
                        break;
                    }
                }
            }
        }

        private void SessionClient_DispatcherErrorReceived(object sender, ExceptionDetailEventArgs e)
        {
            UpdateStatus("ERROR -----{0}{1}{0}{2}".FormatWith(Environment.NewLine, e.Message, e.Detail));
        }

        private void UpdateStatus(string message)
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(message));
        }

        private void ShutDown()
        {
            ShutdownOptions options = new ShutdownOptions()
            {
                AllowWorkerToComplete = false,
                CopyLogs = true,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.RevertToSnapshot
            };

            try
            {
                SessionClient.Instance.Shutdown(_ticket.SessionId, options);
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
                TraceFactory.Logger.Error(ex);
            }
        }

    }
}
