using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Contains methods that interface with WinSpool to monitor the print subsystem.
    /// </summary>
    internal sealed class WinSpoolPrintMonitor : IDisposable
    {
        // Win32 error codes
        private const int ERROR_INVALID_HANDLE = 6;
        private const int RPC_S_SERVER_UNAVAILABLE = 1722;
        private const int RPC_S_CALL_FAILED = 1726;
        private const int ERROR_INVALID_PRINTER_NAME = 1801;

        private readonly List<SafePrinterHandle> _hPrinters = new List<SafePrinterHandle>();
        private readonly List<SafePrinterChangeNotificationHandle> _hChanges = new List<SafePrinterChangeNotificationHandle>();
        private PrinterNotifyOptions _options;
        private Task _listenerTask;
        private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Occurs when the monitor receives a printer notification.
        /// </summary>
        public event EventHandler<JobNotificationEventArgs> JobNotificationReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinSpoolPrintMonitor" /> class.
        /// </summary>
        public WinSpoolPrintMonitor()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Starts monitoring for printer notifications.
        /// </summary>
        public void Start()
        {
            LogDebug("Starting print monitor.");
            Initialize();

            // Start the main listener task
            _listenerTask = Task.Factory.StartNew(Dispatch, TaskCreationOptions.LongRunning);
            _listenerTask.ContinueWith(ListenerTaskComplete, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
        }

        /// <summary>
        /// Signals this <see cref="WinSpoolPrintMonitor" /> to stop monitoring printer notifications.
        /// </summary>
        public void Stop()
        {
            LogDebug("Received request to stop print monitor.");
            _cancelSource.Cancel();
        }

        /// <summary>
        /// The main thread listener.  Creates a task to check the printer(s) for notifications.
        /// </summary>
        private void Dispatch()
        {
            LogDebug("Listening for jobs...");
            _cancelSource = new CancellationTokenSource();

            while (true)
            {
                try
                {
                    Task task = Task.Factory.StartNew(ListenForChanges, _cancelSource.Token);
                    task.Wait(_cancelSource.Token);
                    task.Dispose(); // Explicitly dispose (instead of using statement) because of the Wait call.

                    if (task.IsCanceled)
                    {
                        // Monitor was requested to stop.
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    // Monitor was requested to stop.
                    return;
                }
            }
        }

        /// <summary>
        /// Checks printer(s) for notifications.
        /// </summary>
        private void ListenForChanges()
        {
            // Check for cancellation.
            _cancelSource.Token.ThrowIfCancellationRequested();

            SafePrinterChangeNotificationHandle hChange = null;
            try
            {
                hChange = WaitForMultipleObjects(_hChanges);
            }
            catch (TimeoutException)
            {
                // No object changed within the timeout period.
                // This could mean that there was really no change,
                // or because the spooler is no longer sending updates to us.
                // Since we can't tell the difference, clean up existing structures and sign up for events again.

                // If the cancel token has been set, it might be a bit before we get back to checking it again,
                // so check it here before resetting.  If it has been set, bail out so the monitor can stop.
                _cancelSource.Token.ThrowIfCancellationRequested();

                LogDebug("No job changes detected.  Resetting listener.");
                Reset();
                Initialize();
                return;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Unsuccessful in getting the change notification handle.  Bail out.
                LogDebug(ex);
                return;
            }

            // WaitForMultipleObjects could have taken a while, so check for cancellation again.
            _cancelSource.Token.ThrowIfCancellationRequested();

            // Get the notify information that changed.
            PrinterNotifyInfoReader infoReader = FindNextPrinterChangeNotification(hChange);
            if (infoReader == null)
            {
                LogDebug("Lost job data.  Trying a refresh.");
                infoReader = FindNextPrinterChangeNotification(hChange, _options);
            }

            using (infoReader)
            {
                foreach (PrinterNotifyInfoData infoData in infoReader.ReadInfoData())
                {
                    if (infoData.NotifyType == NotifyType.Job)
                    {
                        JobNotificationReceived?.Invoke(this, new JobNotificationEventArgs(infoData));
                    }
                }
            }
        }

        /// <summary>
        /// Called when the listener task completes, either because it has been canceled or an unhandled exception has occurred.
        /// Check the task status and log any errors, then clean everything up by disposing.
        /// </summary>
        private void ListenerTaskComplete(Task task)
        {
            if (_listenerTask.Status == TaskStatus.Faulted)
            {
                LogError(_listenerTask.Exception);
            }
            Dispose();
            LogDebug("Listener task ended; monitor stopped.");
        }

        /// <summary>
        /// Initializes all fields in the print job monitor.
        /// </summary>
        private void Initialize()
        {
            // In the event that the monitor is started while the spooler is offline,
            // keep retrying until the spooler becomes available.
            SafePrinterHandle hPrinter = null;
            while (true)
            {
                try
                {
                    void action() => hPrinter = OpenPrinter(null);
                    Retry.WhileThrowing<Win32Exception>(action, 5, TimeSpan.FromSeconds(30));
                    break;
                }
                catch (Win32Exception ex)
                {
                    switch (ex.NativeErrorCode)
                    {
                        // Ignore these errors.
                        case RPC_S_SERVER_UNAVAILABLE:
                        case RPC_S_CALL_FAILED:
                        case ERROR_INVALID_PRINTER_NAME:
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            break;

                        default:
                            throw;
                    }
                }
            }

            _options = CreatePrinterNotifyOptions();
            SafePrinterChangeNotificationHandle hChange = FindFirstPrinterChangeNotification(hPrinter, PrinterChanges.Job, _options);
            _hPrinters.Add(hPrinter);
            _hChanges.Add(hChange);
        }

        private static PrinterNotifyOptions CreatePrinterNotifyOptions()
        {
            JobNotifyField[] fields =
            {
                JobNotifyField.BytesPrinted,
                JobNotifyField.DataType,
                JobNotifyField.Document,
                JobNotifyField.DriverName,
                JobNotifyField.MachineName,
                JobNotifyField.NotifyName,
                JobNotifyField.PagesPrinted,
                JobNotifyField.Parameters,
                JobNotifyField.Parameters,
                JobNotifyField.PortName,
                JobNotifyField.Position,
                JobNotifyField.PrintProcessor,
                JobNotifyField.PrinterName,
                JobNotifyField.Priority,
                JobNotifyField.StartTime,
                JobNotifyField.Status,
                JobNotifyField.Submitted,
                JobNotifyField.Time,
                JobNotifyField.TotalBytes,
                JobNotifyField.TotalPages,
                JobNotifyField.UntilTime,
                JobNotifyField.UserName
            };

            PrinterNotifyOptionsType type = new PrinterNotifyOptionsType(NotifyType.Job, fields);
            return new PrinterNotifyOptions(type);
        }

        /// <summary>
        /// Resets all fields in the monitor.
        /// </summary>
        private void Reset()
        {
            LogDebug($"Closing {_hChanges.Count} change notifications, {_hPrinters.Count} printers");

            ExecuteWin32Call(() =>
            {
                foreach (SafePrinterChangeNotificationHandle notification in _hChanges)
                {
                    notification.Dispose();
                }
            });
            _hChanges.Clear();

            ExecuteWin32Call(() =>
            {
                foreach (SafePrinterHandle printer in _hPrinters.ToArray())
                {
                    printer.Dispose();
                }
            });
            _hPrinters.Clear();
        }

        private static void ExecuteWin32Call(Action action)
        {
            try
            {
                action();
            }
            catch (Win32Exception ex)
            {
                switch (ex.NativeErrorCode)
                {
                    case ERROR_INVALID_HANDLE:
                    case RPC_S_SERVER_UNAVAILABLE:
                        // Ignore these exceptions
                        break;

                    default:
                        // We are not catching any other exception types.
                        throw;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Reset();

            if (_listenerTask != null && _listenerTask.Status > TaskStatus.WaitingForChildrenToComplete)
            {
                _listenerTask.Dispose();
                _listenerTask = null;
            }

            if (_cancelSource != null)
            {
                _cancelSource.Dispose();
                _cancelSource = null;
            }

            _options.Dispose();
        }

        #region WinSpool external methods

        /// <summary>
        /// Retrieves a handle to the specified printer, print server, or other handle in the print subsystem.
        /// </summary>
        /// <param name="printerName">The printer name.</param>
        /// <returns>A handle to the opened printer object.</returns>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        private static SafePrinterHandle OpenPrinter(string printerName)
        {
            bool success = NativeMethods.OpenPrinter(printerName, out SafePrinterHandle printerPointer, IntPtr.Zero);

            if (Marshal.GetLastWin32Error() > 0)
            {
                throw new Win32Exception();
            }

            if (!success)
            {
                throw new Win32Exception("Call to OpenPrinter failed.");
            }

            return printerPointer;
        }

        /// <summary>
        /// Creates a change notification object and returns a handle to the object.
        /// </summary>
        /// <param name="hPrinter">A handle to the printer or print server to monitor.</param>
        /// <param name="filter">The <see cref="PrinterChanges" /> that should trigger new events.</param>
        /// <param name="options">The <see cref="PrinterNotifyOptions" /> that dictate which notifications should be sent.</param>
        /// <returns>A handle to a change notification object associated with the specified printer or print server.</returns>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        private static SafePrinterChangeNotificationHandle FindFirstPrinterChangeNotification(SafePrinterHandle hPrinter, PrinterChanges filter, PrinterNotifyOptions options)
        {
            uint fdwOptions = 0; // Must always be zero for 2D printers.
            SafePrinterChangeNotificationHandle result = NativeMethods.FindFirstPrinterChangeNotification(hPrinter, (uint)filter, fdwOptions, ref options);

            if (Marshal.GetLastWin32Error() > 0)
            {
                throw new Win32Exception();
            }

            return result;
        }

        /// <summary>
        /// Retrieves information about the most recent notification for a change notification object associated with a printer or print server.
        /// Call this function when a wait operation on the change notification object is satisfed.
        /// </summary>
        /// <param name="hChange">A handle to a change notification object associated with a printer or print server.</param>
        /// <param name="options">The <see cref="PrinterNotifyOptions" />.</param>
        /// <returns>A <see cref="PrinterNotifyInfoReader" /> containing the notification details.</returns>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        /// <exception cref="System.Exception">
        /// No notification available at this time.
        /// or
        /// Info has been discarded.
        /// </exception>
        private static PrinterNotifyInfoReader FindNextPrinterChangeNotification(SafePrinterChangeNotificationHandle hChange, PrinterNotifyOptions options = new PrinterNotifyOptions())
        {
            if (!NativeMethods.FindNextPrinterChangeNotification(hChange, out _, ref options, out SafePrinterNotifyInfoHandle infoHandle))
            {
                throw new Win32Exception();
            }

            if (infoHandle == null || infoHandle.IsInvalid)
            {
                // Apparently there was no notification after all.  Just return an empty reader.
                return new PrinterNotifyInfoReader();
            }

            PrinterNotifyInfoReader result = new PrinterNotifyInfoReader(infoHandle);

            // Check to see if data has been discarded.
            if (result.InfoDiscarded)
            {
                result.Dispose();
                return null;
            }

            return result;
        }

        /// <summary>
        /// Waits until one or all of the specified objects are in the signaled state.
        /// </summary>
        /// <param name="notifications">The object handles to wait on.</param>
        /// <returns>The object that caused the method to return.</returns>
        /// <exception cref="TimeoutException">The wait operation timed out.</exception>
        /// <exception cref="Win32Exception">The underlying operation failed.</exception>
        private static SafePrinterChangeNotificationHandle WaitForMultipleObjects(IList<SafePrinterChangeNotificationHandle> notifications)
        {
            const int WAIT_TIMEOUT = 258;
            const int WAIT_ABANDONED = 0x00000080;
            const int STATUS_WAIT_0 = 0x00000000;
            const int WAIT_FAILED = -1;

            // Move the list of pointers to an unmanaged array.
            int size = Marshal.SizeOf<IntPtr>();
            IntPtr lpHandles = Marshal.AllocCoTaskMem(size * notifications.Count);
            try
            {
                long location = lpHandles.ToInt64();
                foreach (SafePrinterChangeNotificationHandle notification in notifications)
                {
                    Marshal.StructureToPtr(notification.DangerousGetHandle(), new IntPtr(location), false);
                    location += size;
                }

                int result = NativeMethods.WaitForMultipleObjects(notifications.Count, lpHandles, 0, (uint)TimeSpan.FromMinutes(1).TotalMilliseconds);

                switch (result)
                {
                    case WAIT_TIMEOUT:
                    case WAIT_ABANDONED:
                        throw new TimeoutException("Timed out. " + (result == WAIT_TIMEOUT ? "WAIT_TIMEOUT" : "WAIT_ABANDONED"));

                    case WAIT_FAILED:
                        throw new Win32Exception();

                    default:
                        result -= STATUS_WAIT_0;
                        return notifications[result];
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(lpHandles);
            }
        }

        private static class NativeMethods
        {
            [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool OpenPrinter(string pPrinterName, out SafePrinterHandle phPrinter, IntPtr pDefault);

            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern SafePrinterChangeNotificationHandle FindFirstPrinterChangeNotification(SafePrinterHandle hPrinter, uint printerChanges, uint fdwOptions, ref PrinterNotifyOptions pPrinterNotifyOptions);

            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FindNextPrinterChangeNotification(SafePrinterChangeNotificationHandle hChange, [Out] out uint pdwChange, ref PrinterNotifyOptions pPrinterNotifyOptions, [Out] out SafePrinterNotifyInfoHandle ppPrinterNotifyInfo);

            [DllImport("kernel32", SetLastError = true)]
            internal static extern int WaitForMultipleObjects(int nCount, IntPtr lpHandles, uint bWaitAll, uint dwMilliseconds);
        }

        #endregion
    }
}
