using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.VirtualDevice
{
    /// <summary>
    /// Host for a virtual device that listens for and processes PJL print jobs.
    /// </summary>
    public sealed class VirtualDeviceHost : IDisposable
    {
        private readonly TcpListener _tcpListener;
        private Task _listenerTask;
        private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Occurs when a PJL job has been received.
        /// </summary>
        public event EventHandler<VirtualPrinterJobInfoEventArgs> JobReceived;

        /// <summary>
        /// Gets or sets the number of bytes to receive in each packet.
        /// </summary>
        public int PacketSize { get; set; } = 10000;

        /// <summary>
        /// Gets or sets the amount of time to delay between packets.
        /// </summary>
        /// <remarks>
        /// This models a slight delay between blocks of data to slow down the overall
        /// transfer of raw data.  By changing this value up or down, this component
        /// can loosely model a slower or faster device.
        /// </remarks>
        public TimeSpan PacketDelay { get; set; } = TimeSpan.FromMilliseconds(2);

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDeviceHost" /> class.
        /// </summary>
        /// <param name="address">The IP address to bind to.</param>
        /// <param name="port">The port to listen on.</param>
        public VirtualDeviceHost(IPAddress address, int port)
        {
            _tcpListener = new TcpListener(address, port);
        }

        /// <summary>
        /// Starts monitoring for print jobs.
        /// </summary>
        public void Start()
        {
            LogDebug("Starting virtual print device.");
            _listenerTask = Task.Factory.StartNew(Dispatch, TaskCreationOptions.LongRunning);
            _listenerTask.ContinueWith(ListenerTaskComplete, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
        }

        /// <summary>
        /// Signals this <see cref="VirtualDeviceHost" /> to stop monitoring for jobs.
        /// </summary>
        public void Stop()
        {
            LogDebug("Received request to stop virtual print device.");
            _cancelSource.Cancel();
        }

        /// <summary>
        /// The main thread listener.  Creates a task to monitor TCP for incoming jobs.
        /// </summary>
        private void Dispatch()
        {
            LogInfo("Listening for jobs...");
            _tcpListener.Start();
            _cancelSource = new CancellationTokenSource();

            while (true)
            {
                try
                {
                    Task task = Task.Factory.StartNew(ListenForJobs, _cancelSource.Token);
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
        /// Listens for a print job.
        /// </summary>
        private void ListenForJobs()
        {
            Socket socket = _tcpListener.AcceptSocket();
            Console.WriteLine(new string('-', 70));
            LogInfo($"Received connection: {socket.RemoteEndPoint}");
            socket.ReceiveTimeout = (int)TimeSpan.FromMinutes(20).TotalMilliseconds;

            try
            {
                ReceiveJob(socket);
            }
            catch (SocketException ex)
            {
                // This could mean that there was a timeout.  In theory, whoever is consuming this socket will handle
                // this exception.  In our case, we simply do not want the exception to continue. Do nothing.
                LogWarn($"Ignoring exception: {ex.Message}");
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        private void ReceiveJob(Socket socket)
        {
            PjlJobReader pjlProcessor = new PjlJobReader();
            DateTime lastConsoleUpdate = DateTime.Now;

            LogInfo("Processing job...");
            int byteCount = 0;
            DateTime firstByteReceived = DateTime.Now;
            DateTime lastByteReceived = DateTime.MinValue;

            int size = 0;
            byte[] receiveBuffer = new byte[PacketSize];
            while ((size = socket.Receive(receiveBuffer)) > 0)
            {
                byteCount += size;

                pjlProcessor.ReadPrintJobBytes(receiveBuffer, size);
                lastByteReceived = DateTime.Now;

                // In an effort to not log something to the screen every 10,000 bytes 
                // (which can be quite a bit on a fast machine), let's log screen updates 10 times a second.
                if (DateTime.Now > lastConsoleUpdate.AddMilliseconds(100))
                {
                    Console.Write($"BYTES READ: {byteCount}\r");
                    lastConsoleUpdate = DateTime.Now;
                }

                // This models a slight delay between blocks of data to slow down the overall
                // transfer of raw data.  By changing this value up or down, this component
                // can loosely model a slower or faster device.
                Thread.Sleep(PacketDelay);
            }

            LogInfo($"Job Received: {byteCount} bytes");

            PjlHeader header = pjlProcessor.Header;
            VirtualPrinterJobInfo jobInfo = new VirtualPrinterJobInfo(header)
            {
                FirstByteReceived = firstByteReceived,
                LastByteReceived = lastByteReceived,
                BytesReceived = byteCount
            };
            JobReceived?.Invoke(this, new VirtualPrinterJobInfoEventArgs(jobInfo));
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
            LogDebug("Listener task ended; virtual print device stopped.");
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
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
        }

        #endregion
    }
}
