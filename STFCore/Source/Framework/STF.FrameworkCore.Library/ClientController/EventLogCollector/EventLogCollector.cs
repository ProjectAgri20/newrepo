using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.ClientController.EventLogCollector
{
    internal class EventLogCollector
    {
        private Timer _collectionTimer = null;
        private EventLogCollectorDetail _resourceDefinition = null;
        private Collection<string> _components = null;
        int _entryTypes = 0;
        private TimeSpan _interval;
        private Dictionary<string, DateTime> _lastItemLogged;
        private bool _collecting = false;
        private bool _shuttingDown = false;
        private const string Any = "<Any>";
        private string _sessionID = null;
        public EventLogCollectorDetail ResourceDefinition { get { return _resourceDefinition; } }
        private readonly DataLogger _dataLogger;

        private enum EventLogName
        {
            Application,
            System,
            Security
        }


        public EventLogCollector(string sessionId, EventLogCollectorDetail detail)
        {
            _sessionID = sessionId;
            _resourceDefinition = detail;
            _lastItemLogged = new Dictionary<string, DateTime>();
            _components = _resourceDefinition.Components;
            _entryTypes = _resourceDefinition.EntryTypesBitwise;
            _interval = new TimeSpan(0, _resourceDefinition.PollingInterval, 0);
            _dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
        }


        public void Start()
        {
            _collectionTimer = new Timer(new TimerCallback(CollectionTimerCallback));

            DateTime start = DateTime.Now;
            TraceFactory.Logger.Info("Start message received.");

            //Initialize the last item logged tracker
            foreach (EventLogName name in Enum.GetValues(typeof(EventLogName)))
            {
                _lastItemLogged[name.ToString()] = start;
            }

            // Start up the timer for collecting the event logs.
            _collectionTimer.Change(_interval, _interval);
        }


        private void CollectionTimerCallback(object stateInfo)
        {
            if (_collecting == false)
            {
                _collectionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _collecting = true;

                TraceFactory.Logger.Info("Collecting event logs for {0}.".FormatWith(_resourceDefinition.HostName));
                try
                {
                    RetrieveEventLogs();
                }
                catch (NullReferenceException nullEx)
                {
                    TraceFactory.Logger.Error("Unable to collect Event Log data for {0}".FormatWith(_resourceDefinition.HostName), nullEx);
                }
                _collecting = false;

                //If we're not trying to shut down, re-enable the timer.
                if (_shuttingDown == false)
                {
                    _collectionTimer.Change(_interval, _interval);
                }
                else
                {
                    if (_collectionTimer != null)
                    {
                        _collectionTimer.Dispose();
                        _collectionTimer = null;
                    }

                    TraceFactory.Logger.Debug("Collection timer stopped");
                }
            }
        }
        private void RetrieveEventLogs()
        {
            IEnumerable<EventLog> logs = null;

            try
            {
                logs = EventLog.GetEventLogs(_resourceDefinition.HostName).Where(l => l.Log == "Application" || l.Log == "System" || l.Log == "Security");

                foreach (EventLog log in logs)
                {
                    CollectEventLog(log);
                }
            }
            catch (InvalidOperationException opEx)
            {
                TraceFactory.Logger.Error(opEx.Message, opEx);
            }
            catch (ArgumentException argEx)
            {
                TraceFactory.Logger.Error(argEx.Message, argEx);
            }
            catch (System.IO.IOException ioEx) //Network path not found
            {
                TraceFactory.Logger.Error(ioEx.Message, ioEx);
            }
        }



        /// <summary>
        /// EventLogEntryType Enum
        /// 1 - Error
        /// 2 - Warning
        /// 4 - Information
        /// 8 - SuccessAudit
        /// 16 - FailureAudit
        /// </summary>
        private void CollectEventLog(EventLog eventLog)
        {
            DateTime startTime = _lastItemLogged[eventLog.Log];
            DateTime endTime = _lastItemLogged[eventLog.Log];
            bool collectAny = _components[0] == Any;
            int count = 0;

            try
            {
                IOrderedEnumerable<EventLogEntry> query = from EventLogEntry eLog in eventLog.Entries
                                                          where eLog.TimeGenerated > startTime && ((int)eLog.EntryType & _entryTypes) > 0
                                                          orderby eLog.TimeGenerated
                                                          select eLog;

                foreach (EventLogEntry entry in query)
                {
                    if (collectAny || MatchesCriteria(entry))
                    {
                        endTime = entry.TimeGenerated;
                        count++;
                        WindowsEventDataLog log = new WindowsEventDataLog(_sessionID, eventLog, entry);
                        _dataLogger.SubmitAsync(log);
                    }
                }

                TraceFactory.Logger.Info("{0}: Collected {1} log entries".FormatWith(eventLog.Log, count));

                _lastItemLogged[eventLog.Log] = endTime;
            }
            finally
            {
                if (eventLog != null)
                {
                    eventLog.Dispose();
                }
            }
        }


        /// <summary>
        /// Check the event log entry against the list of components we want to monitor
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>true if the event log entry matches one of the components</returns>
        private bool MatchesCriteria(EventLogEntry entry)
        {
            bool isMatch = false;

            foreach (string component in _components)
            {
                isMatch = (entry.Source.Contains(component) || entry.Message.Contains(component));
                if (isMatch)
                {
                    break;
                }
            }

            return isMatch;
        }


        public void Stop()
        {
            if (_collectionTimer != null)
            {
                TraceFactory.Logger.Info("Stop message received.");

                _shuttingDown = true;


                if (_collecting == false)
                {
                    //Collect one last time before shutting down.
                    CollectionTimerCallback(null);
                }
            }
        }







    }
}
