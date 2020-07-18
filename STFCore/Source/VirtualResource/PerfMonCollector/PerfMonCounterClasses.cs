using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.VirtualResource.PerfMonCollector
{

    /// <summary>
    /// this class is used group the counters according to the interval at which the counters have to be measured
    /// </summary>
    internal class PerfMonCounterByInterval: IDisposable
    {
        private double _interval;
        private Collection<PerfMonCounterByMachine> _countersByMachine;
        private System.Timers.Timer _intervalTimer;

        /// <summary>
        /// The interval at which the counter is collected
        /// </summary>
        public double Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// collection containing the machines(targethosts)
        /// </summary>
        public Collection<PerfMonCounterByMachine> CountersByMachine
        {
            get { return _countersByMachine; }
           // set { _countersByMachines = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="interval"></param>
        public PerfMonCounterByInterval(double interval)
        {
            if (interval == 0.0)
            {
                interval = 5.0;
            }

            _interval = interval;

            _countersByMachine = new Collection<PerfMonCounterByMachine>();

            //create the timer according to the interval
            _intervalTimer = new System.Timers.Timer(_interval); 
            _intervalTimer.Elapsed += _intervalTimer_Elapsed;
            _intervalTimer.Enabled = false;
        }

        /// <summary>
        /// collects the data at periodic intervals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _intervalTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           
            // browse through each machine and then trigger the collection, using threadpool to multitask the collection by machine else we would have wait for each machine to finish and can slow down the collection
            foreach (var counterByMachine in _countersByMachine)
            {
                if (!counterByMachine.IsError)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(CollectCounterForMachine), counterByMachine);
                }
            }
        }

        private void CollectCounterForMachine(Object machine)
        {
            var counterByMachine = machine as PerfMonCounterByMachine;

            if (counterByMachine != null)
            {
                // trigger the collection for the host
                counterByMachine.Collect();
            }
        }

        /// <summary>
        /// starts the timer for collecting the performance counters
        /// </summary>
        public void Start()
        {
            //start the timer
            _intervalTimer.Enabled = true;
            _intervalTimer.Start();
            
            //executed to collect the counter at start
            _intervalTimer_Elapsed(null, null);
        }

        /// <summary>
        /// stops the timer for collecting the performance counters
        /// </summary>
        public void Stop()
        {
            //stop the timer
            _intervalTimer.Stop();
            _intervalTimer.Enabled = false;
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
                _intervalTimer.Dispose();
            }
        }
    }

    /// <summary>
    /// this classes is used to group the counters by machine
    /// </summary>
    internal class PerfMonCounterByMachine
    {
        private string _targetHost;
        private string _sessionId;
        private PerfMonCounterCredential _credentials;
        private Collection<System.Diagnostics.PerformanceCounter> _counters;
        private bool _isError;

        /// <summary>
        /// the machine for which the counters are to be monitored
        /// </summary>
        public string TargetHost 
        { 
            get { return _targetHost; } 
            set { _targetHost = value; } 
        }

        /// <summary>
        /// the collection of performance counters to be monitored for this machine
        /// </summary>
        public Collection<System.Diagnostics.PerformanceCounter> Counters 
        { 
            get { return _counters; } 
           // set { _counters = value; } 
        }

        /// <summary>
        /// the user credentials for access the machine's performance counters
        /// </summary>
        public PerfMonCounterCredential Credentials
        { 
            get { return _credentials; }
            set { _credentials = value; } 
        }

        /// <summary>
        /// the session ID
        /// </summary>
        public string SessionId 
        { 
            get { return _sessionId; } 
            set { _sessionId = value; } 
        }

        /// <summary>
        /// Value indicating that the current counter encountered an error before and is to be disabled
        /// </summary>
        public bool IsError
        {
            get { return _isError; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public PerfMonCounterByMachine()
        {
            _targetHost = string.Empty;
            _counters = new ObservableCollection<System.Diagnostics.PerformanceCounter>();
            _credentials = new PerfMonCounterCredential();
        }

        /// <summary>
        /// get the counter sample for the machine
        /// </summary>
        public void Collect()
        {
            // if no user name then it means no need to impersonate
            if (!string.IsNullOrEmpty(_credentials.UserName))
            {
                try
                {
                    NetworkCredential networkCredential = new NetworkCredential(_credentials.UserName, _credentials.Password, _credentials.Domain);
                    UserImpersonator.Execute(CollectCounters, networkCredential);
                }
                catch (Win32Exception w32Exception)
                {
                    // we have an error with authorization, we will stop this counter from being collected again to prevent system overheads
                    TraceFactory.Logger.Error("Halting collection on Host: {0}".FormatWith(this.TargetHost), w32Exception);
                    _isError = true;
                }
            }
            else
            {
                CollectCounters();
            }
        }

        private void CollectCounters()
        {
            // collect the performance counter values for each counter specified for the host
            // we want all the counters to have the same datetime values for easy grouping for the charts, otherwise the millisecond differences will play havoc
            DateTime logTime = DateTime.Now;

            foreach (var counter in _counters)
            {
                try
                {
                    TraceFactory.Logger.Info("HostName:{0}    Category:{1}  Counter:{2}  Instance:{3}".FormatWith(counter.MachineName, counter.CategoryName, counter.CounterName, counter.InstanceName));
                    float counterValue = counter.NextValue();
                    OnCollected(counterValue, counter, logTime);
                }
                catch (InvalidOperationException invOperationException)
                {
                    // we have an error while collecting counter, we will stop this counter from being collected again to prevent system overheads
                    TraceFactory.Logger.Error("Halting Counter '{0}' on Host: {1}".FormatWith(counter.CounterName, this.TargetHost), invOperationException);
                    _isError = true;
                }
            }
        }

        private void OnCollected(float counterValue, System.Diagnostics.PerformanceCounter counter, DateTime collectionDateTime)
        {
            DataLogger logger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
            WindowsPerfMonLog log = new WindowsPerfMonLog(GlobalDataStore.Manifest.SessionId, collectionDateTime, counter, counterValue);
            logger.SubmitAsync(log);
        }
    }
  
}