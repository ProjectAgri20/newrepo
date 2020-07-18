using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.PerfMonCollector;

namespace HP.ScalableTest.VirtualResource.PerfMonCollector
{
    internal class PerfMonCounterCollector : IDisposable
    {
        private PerformanceCounter[] _counters = null;
        private PerfMonCounterData _counterConfig = null;
        string _machineName = string.Empty;
        private Timer _collectionTimer = null;
        private TimeSpan _interval;
        private bool _collecting = false;
        private bool _shuttingDown = false;

        public event EventHandler<PerfMonCollectorEventArgs> Collected;

        public PerfMonCounterCollector(PerfMonCounterData counterConfig, string machineName)
        {
            _interval = new TimeSpan(0, 0, counterConfig.CollectionInterval);
            _counterConfig = counterConfig;
            _machineName = machineName;
        }

        public void Start()
        {
            string errorMsg = _counterConfig.Name + " failed to start.";

            try
            {
                LoadCounters();
                TraceFactory.Logger.Info("{0} {1} monitoring {2} instance(s)".FormatWith(_machineName, _counterConfig.Name, _counters.Length));

                if (_counterConfig.CollectAtStart)
                {
                    try
                    {
                        CollectCounterData();
                    }
                    catch (NullReferenceException nullEx)
                    {
                        TraceFactory.Logger.Error("Unable to collect PerfMon data for {0}".FormatWith(_counterConfig.Name), nullEx);
                    }
                }

                // Start up the timer for collecting the counter data.
                _collectionTimer = new Timer(new TimerCallback(CollectionTimerCallback), null, _interval, _interval);
            }
            catch (InvalidOperationException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (ArgumentNullException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (PlatformNotSupportedException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (Win32Exception ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
        }

        public void Stop()
        {
            _shuttingDown = true;

            TraceFactory.Logger.Info(_counterConfig.Name);
            if (_collecting == false)
            {
                //Collect one last time before shutting down.
                CollectionTimerCallback(null);
                ThreadPool.QueueUserWorkItem(CollectionTimerCallback, null);
            }
        }

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
                if (_collectionTimer != null)
                {
                    _collectionTimer.Dispose();
                }
                if (_counters != null && _counters.Length > 0)
                {
                    DisposeCounters();
                }
            }
        }

        private void CollectionTimerCallback(object stateInfo)
        {
            if (_collecting == false)
            {
                _collectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _collecting = true;

                try
                {
                    CollectCounterData();
                }
                catch (NullReferenceException nullEx)
                {
                    TraceFactory.Logger.Error("Unable to collect PerfMon data for {0}".FormatWith(_counterConfig.Name), nullEx);
                }
                _collecting = false;

                //If we're not trying to shut down, re-enable the timer.
                if (_shuttingDown == false)
                {
                    _collectionTimer.Change(_interval, _interval);
                }
            }
        }

        private void NotifyCollected(float counterValue, DateTime collectionDateTime)
        {
            if (Collected != null)
            {
                ResourceWindowsPerfMonLog logData = new ResourceWindowsPerfMonLog()
                {
                    LogDateTime = collectionDateTime,
                    Category = _counterConfig.Category,
                    InstanceName = _counterConfig.Instance,
                    Counter = _counterConfig.Counter,
                    CounterValue = counterValue
                };

                Collected(this, new PerfMonCollectorEventArgs(logData));
            }
        }

        private void CollectCounterData()
        {
            string errorMsg = _counterConfig.Name + " failed to collect.";

            Console.WriteLine("Collecting PerfMon data for {0}.".FormatWith(_counterConfig.Name));
            try
            {
                for (int i = 0; i < _counters.Length; i++)
                {
                    NotifyCollected(_counters[i].NextValue(), DateTime.Now);
                }
            }
            catch (InvalidOperationException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (PlatformNotSupportedException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
            catch (Win32Exception ex)
            {
                TraceFactory.Logger.Error(errorMsg, ex);
                throw;
            }
        }

        private void LoadCounters()
        {
            string[] instances = GetInstanceNames();

            _counters = new PerformanceCounter[instances.Length];

            for (int i = 0; i < instances.Length; i++)
            {
                _counters[i] = new PerformanceCounter(_counterConfig.Category, _counterConfig.Counter, instances[i], _machineName);
            }

            if (_counters.Length == 0)
            {
                throw new InvalidOperationException("No process instances were found to monitor.  Instance Name: {0}".FormatWith(_counterConfig.Instance));
            }
        }

        private string[] GetInstanceNames()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory(_counterConfig.Category, _machineName);
            string[] instanceNames = category.GetInstanceNames();

            return instanceNames.Where(n => n.StartsWith(_counterConfig.Instance, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        private void DisposeCounters()
        {
            for (int i = 0; i < _counters.Length; i++)
            {
                _counters[i].Dispose();
            }
        }

    }
}
