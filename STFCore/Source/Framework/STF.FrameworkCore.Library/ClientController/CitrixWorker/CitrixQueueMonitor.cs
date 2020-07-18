using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// System that monitors print queues being autocreated on the Citrix server
    /// </summary>
    public class CitrixQueueMonitor : IDisposable
    {
        static readonly CitrixQueueMonitor _instance = new CitrixQueueMonitor();
        private Dictionary<string, List<PrinterRegistryEntry>> _registryEntries = new Dictionary<string, List<PrinterRegistryEntry>>();

        private System.Timers.Timer _registryCheckTimer = new System.Timers.Timer();
        static object _registryEntriesLock = new object();

        private System.Timers.Timer _fileWriteTimer = new System.Timers.Timer();
        private Dictionary<string, StringBuilder> _fileWriteBuffer = new Dictionary<string, StringBuilder>();
        static object _fileWriteLock = new object();

        private System.Timers.Timer _cleanupTimer = new System.Timers.Timer();
        private TimeSpan _cleanupTimeDelta = TimeSpan.FromHours(24);

        private CitrixQueueMonitor()
        {
            _registryCheckTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            _registryCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(_registryCheckTimer_Elapsed);
            TraceFactory.Logger.Debug("Starting Registry Check Timer");
            _registryCheckTimer.Start();

            _fileWriteTimer.Interval = TimeSpan.FromSeconds(15).TotalMilliseconds;
            _fileWriteTimer.Elapsed += new System.Timers.ElapsedEventHandler(_fileWriteTimer_Elapsed);
            TraceFactory.Logger.Debug("Starting File Writer Timer");
            _fileWriteTimer.Start();

            _cleanupTimer.Interval = TimeSpan.FromMinutes(10).TotalMilliseconds;
            _cleanupTimer.Elapsed += new System.Timers.ElapsedEventHandler(_cleanupTimer_Elapsed);
            TraceFactory.Logger.Debug("Starting Cleanup Timer");
            _cleanupTimer.Start();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CitrixQueueMonitor Instance
        {
            get { return _instance; }
        }


        private void AppendData(string sessionId, string data)
        {
            lock (_fileWriteLock)
            {
                StringBuilder builder = null;
                if (!_fileWriteBuffer.TryGetValue(sessionId, out builder))
                {
                    _fileWriteBuffer.Add(sessionId, builder = new StringBuilder());
                }
                builder.Append(data);
                builder.Append(Environment.NewLine);
            }
        }

        private void _fileWriteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _fileWriteTimer.Stop();

            lock (_fileWriteLock)
            {
                if (_fileWriteBuffer.Count > 0)
                {
                    foreach (string key in _fileWriteBuffer.Keys)
                    {
                        string fileName = Path.Combine(LogFileReader.DataLogPath(key), "CitrixQueueCreation-{0}.csv".FormatWith(key));
                        if (!File.Exists(fileName))
                        {
                            string header = "Session Id, HostName, UserName, Type, Start, End, Duration, Print Driver, Client Queue, Citrix Queue"
                                + Environment.NewLine;
                            File.WriteAllText(fileName, header);
                        }
                        TraceFactory.Logger.Debug(fileName);
                        File.AppendAllText(fileName, _fileWriteBuffer[key].ToString());
                    }

                    _fileWriteBuffer.Clear();
                }
            }

            _fileWriteTimer.Start();
        }

        private void _registryCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _registryCheckTimer.Stop();

            lock (_registryEntriesLock)
            {
                ProcessCurrentEntries();
            }

            _registryCheckTimer.Start();
        }

        /// <summary>
        /// Handles the Elapsed event of the _timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// This method performs a cleanup on stale entries. It does it by trying to 
        /// communicate with the endpoint for the defined Citrix user, and if it doesn't
        /// get a response or if the session Id is different, then this item is considered
        /// stale and can be removed.
        /// </remarks>
        private void _cleanupTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _cleanupTimer.Stop();
            Dictionary<string, List<PrinterRegistryEntry>> registryEntriesCopy = null;

            lock (_registryEntriesLock)
            {
                registryEntriesCopy = (from r in _registryEntries select r).ToDictionary(r => r.Key, r => r.Value);
            }

            foreach (string key in registryEntriesCopy.Keys)
            {
                foreach (PrinterRegistryEntry entry in registryEntriesCopy[key])
                {
                    // Create a time value that is the actual start time (when this service received a message)
                    // plus the delay time for the client to actually start, plus a long wait window.  If all
                    // of that time has passed and there is still an entry, the remove it.
                    DateTime actualStartupTime = entry.ClientData.SessionStart.Add(entry.ClientData.StartupDelay);
                    actualStartupTime = actualStartupTime.Add(_cleanupTimeDelta);
                    DateTime now = DateTime.Now;

                    if (DateTime.Compare(actualStartupTime, now) < 0)
                    {
                        using (var service = VirtualResourceManagementConnection.Create(entry.ClientData.EndPoint))
                        {
                            try
                            {
                                service.Channel.Ping();
                            }
                            catch (FaultException)
                            {
                                PurgeEntry(key, entry);
                            }
                            catch (EndpointNotFoundException)
                            {
                                PurgeEntry(key, entry);
                            }
                            catch (SocketException)
                            {
                                PurgeEntry(key, entry);
                            }
                        }
                    }
                }
            }

            _cleanupTimer.Start();
        }

        private void PurgeEntry(string key, PrinterRegistryEntry entry)
        {
            lock (_registryEntriesLock)
            {
                _registryEntries[key].Remove(entry);
                TraceFactory.Logger.Debug("PURGED {0}:{1}".FormatWith(key, entry.ClientData.QueueName));
                if (_registryEntries[key].Count == 0)
                {
                    _registryEntries.Remove(key);
                }
            }
        }

        private void ProcessCurrentEntries()
        {
            Dictionary<string, List<PrinterRegistryEntry>> deleteList = new Dictionary<string, List<PrinterRegistryEntry>>();

            Collection<string> queueEntries = PrinterRegistryEntry.PrinterEntries;

            foreach (string key in _registryEntries.Keys)
            {
                foreach (PrinterRegistryEntry entry in _registryEntries[key])
                {
                    if (entry.CheckQueueCreationComplete(queueEntries))
                    {
                        DateTime now = DateTime.Now;

                        TimeSpan duration = now.Subtract(entry.ClientData.SessionStart);
                        string durationStr = "{0:D2}:{1:D2}:{2:D2}.{3:D3}"
                            .FormatWith(duration.Hours, duration.Minutes, duration.Seconds, duration.Milliseconds);

                        // Write out the log data for this queue
                        string logItem = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}".FormatWith
                            (
                                entry.ClientData.SessionId,
                                entry.ClientData.HostName,
                                entry.ClientData.UserName,
                                entry.DriverType,
                                entry.ClientData.SessionStart.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                now.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                durationStr,
                                entry.ClientData.PrintDriver,
                                entry.ClientData.QueueName,
                                entry.FullQueueName.Replace(",", "/")
                            );

                        AppendData(entry.ClientData.SessionId, logItem);

                        if (!deleteList.ContainsKey(key))
                        {
                            deleteList.Add(key, new List<PrinterRegistryEntry>());
                        }
                        deleteList[key].Add(entry);

                        TraceFactory.Logger.Debug("CREATED {0}:{1}".FormatWith(key, entry.ClientData.QueueName));
                    }
                }
            }

            foreach (string key in deleteList.Keys)
            {
                foreach (PrinterRegistryEntry entry in deleteList[key])
                {
                    _registryEntries[key].Remove(entry);
                    TraceFactory.Logger.Debug("REMOVED {0}:{1}".FormatWith(key, entry.ClientData.QueueName));
                    if (_registryEntries[key].Count == 0)
                    {
                        _registryEntries.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified client data to the list of monitored queues.
        /// </summary>
        /// <param name="clientData">The client data.</param>
        public void Add(CitrixQueueClientData clientData)
        {
            if (clientData == null)
            {
                throw new ArgumentNullException("clientData");
            }

            lock (_registryEntriesLock)
            {
                List<PrinterRegistryEntry> list;
                if (!_registryEntries.TryGetValue(clientData.HostName, out list))
                {
                    _registryEntries.Add(clientData.HostName, list = new List<PrinterRegistryEntry>());
                }

                // Set the session start time locally so the clocks sync better
                clientData.SessionStart = DateTime.Now;
                list.Add(new PrinterRegistryEntry(clientData));
                TraceFactory.Logger.Debug("ADDED {0}:{1}".FormatWith(clientData.HostName, clientData.QueueName));

                //if (!_watcherStarted)
                //{
                //    //_watcher.Start();
                //    //_watcherStarted = true;
                //    _fileWriteTimer.Start();
                //}
            }
        }

        #region IDisposable Members

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
            if (disposing)
            {
                if (_cleanupTimer != null)
                {
                    _cleanupTimer.Dispose();
                }

                if (_fileWriteTimer != null)
                {
                    _fileWriteTimer.Dispose();
                }

                if (_registryCheckTimer != null)
                {
                    _registryCheckTimer.Dispose();
                }
            }
        }

        #endregion
    }
}
