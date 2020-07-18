using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Print.Drivers;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// This class supports the scanning of INF files in the DriverStore of the current system to find
    /// all available in-box drivers that come pre-installed on the system.
    /// </summary>
    /// <example>
    /// The example below uses the driver store scanner to find all available drivers natively installed on the
    /// operating system.  It uses two events to track the progress of the scanning.  These events are used
    /// in the STF to bind with a progress control to give feedback to the user.  The start and stop methods
    /// provide a mechanism to begin the background scanning and to abort it if it hasn't finished.
    /// 
    /// In the second example a direct call is made to the scanner which will block until it completes.
    /// <code>
    /// DriverStoreScanner.Instance.OnAllDriversLoaded += Instance_OnAllDriversLoaded;
    /// DriverStoreScanner.Instance.OnInfFileScanned += Instance_OnInfFileScanned;
    /// DriverStoreScanner.Instance.Start();
    /// 
    /// void Instance_OnInfFileScanned(object sender, DriverStoreScanningEventArgs e)
    /// {
    ///     Console.WriteLine("{0}/{1} : {2}".FormatWith(e.Complete, e.Total, e.Driver));
    /// }
    /// 
    /// void Instance_OnAllDriversLoaded(object sender, EventArgs e)
    /// {
    ///     Console.WriteLine("All drivers scanned");
    ///     DriverStoreScanner.Instance.Stop();
    /// }
    /// 
    /// OUTPUT:
    /// 
    /// 1/804 : C:\Windows\system32\DriverStore\FileRepository\1394.inf_amd64_neutral_0b11366838152a76
    /// 2/804 : C:\Windows\system32\DriverStore\FileRepository\2008s4el.inf_amd64_neutral_743fc96fa8857a1a
    /// 3/804 : C:\Windows\system32\DriverStore\FileRepository\5000xzvp.inf_amd64_neutral_eda1913b787038c2
    /// ...
    /// 802/804 : C:\Windows\system32\DriverStore\FileRepository\xcbdav.inf_amd64_neutral_cf80e4da1c95e6e2
    /// 803/804 : C:\Windows\system32\DriverStore\FileRepository\xnacc.inf_amd64_neutral_13c4e272a96185a1
    /// 804/804 : C:\Windows\system32\DriverStore\FileRepository
    /// All drivers scanned
    /// Found 1929 drivers
    /// </code>
    /// <code>
    /// Console.WriteLine("Starting scan...");
    /// DriverStoreScanner.Instance.ScanForInboxDrivers();
    /// Console.WriteLine("Found {0} drivers", DriverStoreScanner.Instance.Drivers.Count());
    /// 
    /// OUTPUT:
    /// Starting scan...
    /// Found 1929 drivers
    /// </code>
    /// </example>
    public class DriverStoreScanner
    {
        static readonly DriverStoreScanner _instance = new DriverStoreScanner();
        private bool _scanningComplete = false;
        private int _totalDirectories = 0;
        private int _totalDirectoriesScanned = 0;
        private readonly PrintDeviceDriverCollection _drivers = new PrintDeviceDriverCollection();
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        /// <summary>
        /// Occurs when all in-box drivers have been found.
        /// </summary>
        public event EventHandler<EventArgs> OnAllDriversLoaded;

        /// <summary>
        /// Occurs when an in-box driver INF file has been scanned.
        /// </summary>
        public event EventHandler<DriverStoreScanningEventArgs> OnInfFileScanned;

        /// <summary>
        /// Prevents a default instance of the <see cref="DriverStoreScanner"/> class from being created.
        /// </summary>
        DriverStoreScanner()
        {
            _scanningComplete = false;
        }

        /// <summary>
        /// Starts a background scan of the DriverStore to load all compatible in-box drivers
        /// </summary>
        /// <remarks>
        /// This method will run the driver store scan in the background.  If you use this method
        /// you should also register for the <see cref="OnAllDriversLoaded"/> event so you can
        /// receive notification when the scan job is complete.
        /// </remarks>
        public void Start()
        {
            lock (this)
            {
                if (!_scanningComplete)
                {
                    _tokenSource = new CancellationTokenSource();
                    Task.Factory.StartNew(() => ScanForInboxDrivers(), _tokenSource.Token);
                }
            }
        }

        /// <summary>
        /// Stops the background scanning process if it is currently running.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                try
                {
                    _tokenSource.Cancel();
                }
                catch (SecurityException ex)
                {
                    TraceFactory.Logger.Error("Failed to abort scanning thread", ex);
                }
                catch (ThreadAbortException ex)
                {
                    TraceFactory.Logger.Error("Failed to abort scanning thread", ex);
                }
                catch (ThreadStateException ex)
                {
                    TraceFactory.Logger.Error("Failed to abort scanning thread", ex);
                }
                finally
                {
                    Reset();
                }
            }
        }

        /// <summary>
        /// Resets the scanner by clearing information on all scanned in-box drivers
        /// </summary>
        public void Reset()
        {
            _scanningComplete = false;
            _totalDirectories = 0;
            _totalDirectoriesScanned = 0;
        }

        /// <summary>
        /// Gets the singleton instance of the driver store scanner.
        /// </summary>
        public static DriverStoreScanner Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets driver information for all drivers found during a driver store scan.
        /// </summary>
        public PrintDeviceDriverCollection Drivers
        {
            get { return _drivers; }
        }

        /// <summary>
        /// Gets a value indicating whether the driver store file scanning is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if scanning complete; otherwise, <c>false</c>.
        /// </value>
        public bool ScanningComplete
        {
            get { return _scanningComplete; }
        }

        /// <summary>
        /// Blocking method that scans for inbox drivers within the driver store repository.
        /// </summary>
        private void ScanForInboxDrivers()
        {
            // This has to scan according to the proper location for the INF files.
            _drivers.Clear();

            string inboxPath = string.Empty;

            // Assume that for Vista or greater the DriverStore is used...
            if (Environment.OSVersion.Version.Major >= 6)
            {
                inboxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"DriverStore\FileRepository");
            }
            else
            {
                inboxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "inf");
            }

            TraceFactory.Logger.Debug("Crawling {0}".FormatWith(inboxPath));
            
            if (Directory.Exists(inboxPath))
            {
                var directories = Directory.GetDirectories(inboxPath);

                // Include one extra directory to account for the top level directory which
                // is searched as a separate call below.
                _totalDirectories = directories.Count() + 1;

                TraceFactory.Logger.Debug("Directory Count: {0}".FormatWith(_totalDirectories));

                // Search all the children of the root.
                foreach (string directory in directories)
                {
                    _drivers.Add(LoadDrivers(directory, false, SearchOption.AllDirectories));

                    if (_tokenSource.IsCancellationRequested)
                    {
                        TraceFactory.Logger.Debug("Cancellation received, returning...");
                        return;
                    }
                }

                // Search the root directory, but don't drill down into the children, they've
                // already been done.
                _drivers.Add(LoadDrivers(inboxPath, false, SearchOption.TopDirectoryOnly));

                Thread.Sleep(TimeSpan.FromSeconds(1));

                if (_tokenSource.IsCancellationRequested)
                {
                    TraceFactory.Logger.Debug("Cancellation received, returning...");
                    return;
                }
            }

            if (OnAllDriversLoaded != null)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    TraceFactory.Logger.Debug("Cancellation received, returning...");
                    return;
                }

                OnAllDriversLoaded(this, new EventArgs());
            }

            _scanningComplete = true;
        }

        private PrintDeviceDriverCollection LoadDrivers
            (
                string currentDirectory,
                bool includeAllArchitectures,
                SearchOption searchOption
            )
        {
            PrintDeviceDriverCollection drivers = new PrintDeviceDriverCollection();
            drivers.AddRange(DriverController.LoadFromDirectory(currentDirectory, includeAllArchitectures, searchOption).Select(n => new PrintDeviceDriver(n)));

            if (_tokenSource.IsCancellationRequested)
            {
                TraceFactory.Logger.Debug("Cancellation received, returning...");
                return drivers;
            }

            _totalDirectoriesScanned++;
            FireScannedEvent(currentDirectory);

            return drivers;
        }

        private void FireScannedEvent(string driverName)
        {
            // Use this to post events to the calling client on the progress
            // of the scanning effort.
            if (OnInfFileScanned != null && !_tokenSource.IsCancellationRequested)
            {
                DriverStoreScanningEventArgs args = new DriverStoreScanningEventArgs();
                args.Driver = driverName;
                args.Total = _totalDirectories;
                args.Complete = _totalDirectoriesScanned;
                OnInfFileScanned(this, args);
            }
        }
    }
}
