using System;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core
{
    /// <summary>
    /// A <see cref="ServiceBase" /> that can self-install or be run standalone.
    /// </summary>
    public abstract class SelfInstallingServiceBase : ServiceBase
    {
        private const string _argsCacheFileName = "ServiceStartArgs.dat";

        /// <summary>
        /// Gets the display name for the service.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets or sets a description of the service.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfInstallingServiceBase" /> class.
        /// </summary>
        /// <param name="serviceName">The short name used to identify the service.</param>
        /// <param name="displayName">The display name for the service and its Windows event log.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceName" /> is null.
        /// <para>or</para>
        /// <paramref name="displayName" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected SelfInstallingServiceBase(string serviceName, string displayName)
        {
            ServiceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));

            EventLog.Log = displayName;
            EventLog.Source = displayName;

            AutoLog = false;
        }

        /// <summary>
        /// Executes a service operation based on the command provided by the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ArgumentNullException"><paramref name="args" /> is null.</exception>
        public void Run(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.UnhandledException += LogFatalError;

            CommandLineArguments arguments = new CommandLineArguments(args);
            if (arguments.HasParameter("install"))
            {
                Install(arguments);
            }
            else if (arguments.HasParameter("uninstall"))
            {
                Uninstall(arguments);
            }
            else if (arguments.HasParameter("standalone"))
            {
                // Start the service and hold the program open until a key is pressed
                StartService(arguments);
                Console.ReadKey();
                StopService();
            }
            else
            {
                Run(this);
            }
        }

        private void LogFatalError(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            EventLog.WriteEntry(exception.ToString(), EventLogEntryType.Error);

            // Must explicitly exit to avoid a process hang.
            Environment.Exit(1);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the
        /// Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically).
        /// Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected sealed override void OnStart(string[] args)
        {
            base.OnStart(args);

            CommandLineArguments arguments = ReadCommandLineArguments(args);
            StartService(arguments);
            EventLog.WriteEntry($"{DisplayName} started successfully.");
        }

        private static CommandLineArguments ReadCommandLineArguments(string[] args)
        {
            // Check to see if we should read from or write to the cache
            if (args.Any())
            {
                // Arguments were passed in - write to the cache
                File.WriteAllLines(_argsCacheFileName, args);
            }
            else if (File.Exists(_argsCacheFileName))
            {
                // No args passed in - read from the cache
                args = File.ReadAllLines(_argsCacheFileName);
            }

            return new CommandLineArguments(args);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the
        /// Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected sealed override void OnStop()
        {
            base.OnStop();

            StopService();
            EventLog.WriteEntry($"{DisplayName} stopped successfully.");
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected abstract void StartService(CommandLineArguments args);

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected abstract void StopService();

        /// <summary>
        /// Installs this service.
        /// </summary>
        private void Install(CommandLineArguments args)
        {
            RunInstallerTask(n => n.Install(new Hashtable()), args);
            EventLog.WriteEntry($"{DisplayName} installed successfully.");
        }

        /// <summary>
        /// Uninstalls this service.
        /// </summary>
        private void Uninstall(CommandLineArguments args)
        {
            RunInstallerTask(n => n.Uninstall(null), args);
        }

        private void RunInstallerTask(Action<TransactedInstaller> installerAction, CommandLineArguments args)
        {
            string path = "/assemblypath=" + Assembly.GetEntryAssembly().Location;

            using (TransactedInstaller installer = new TransactedInstaller())
            {
                installer.Installers.Add(new SelfInstallingServiceInstaller(this, args));
                installer.Context = new InstallContext(null, new[] { path });
                installerAction(installer);
            }
        }

        /// <summary>
        /// Installer for a <see cref="SelfInstallingServiceBase" />.
        /// </summary>
        private class SelfInstallingServiceInstaller : Installer
        {
            private readonly string _serviceName;

            /// <summary>
            /// Initializes a new instance of the <see cref="SelfInstallingServiceInstaller" /> class.
            /// </summary>
            /// <param name="service">A prototype of the <see cref="SelfInstallingServiceBase" /> to install.</param>
            /// <param name="args">The <see cref="CommandLineArguments" /> provided to the install command.</param>
            public SelfInstallingServiceInstaller(SelfInstallingServiceBase service, CommandLineArguments args)
            {
                if (service == null)
                {
                    throw new ArgumentNullException(nameof(service));
                }

                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                _serviceName = service.ServiceName;

                ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
                if (args.HasParameter("username") && args.HasParameter("password"))
                {
                    serviceProcessInstaller.Account = ServiceAccount.User;
                    serviceProcessInstaller.Username = args["username"];
                    serviceProcessInstaller.Password = args["password"];
                }
                else
                {
                    serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
                }

                ServiceInstaller serviceInstaller = new ServiceInstaller
                {
                    ServiceName = service.ServiceName,
                    DisplayName = service.DisplayName,
                    Description = service.Description,
                    StartType = ServiceStartMode.Automatic,
                    DelayedAutoStart = true
                };

                Installers.Add(serviceProcessInstaller);
                Installers.Add(serviceInstaller);

                EventLogInstaller eventLogInstaller = serviceInstaller.Installers.OfType<EventLogInstaller>().FirstOrDefault();
                eventLogInstaller.Log = service.DisplayName;
                eventLogInstaller.Source = service.DisplayName;
            }

            /// <summary>
            /// Performs configuration after the service installation has been committed.
            /// </summary>
            protected override void OnCommitted(IDictionary savedState)
            {
                base.OnCommitted(savedState);

                // Configure service to restart on failure.
                using (Process sc = Process.Start("sc", string.Format("failure \"{0}\" reset= 0 actions= restart/2000", _serviceName)))
                {
                    sc.WaitForExit(10000);
                }
            }
        }
    }
}
