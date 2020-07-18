using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Base class for managing all events published to listening subscribers 
    /// listening subscribers.
    /// </summary>
    public class EventPublisherBase
    {
        private Collection<Uri> _subscribers = new Collection<Uri>();
        private Thread _subscriberThread;
        private readonly string _dumpFile;
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisherBase"/> class.
        /// </summary>
        public EventPublisherBase()
        {
            _dumpFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "subscribers.dat");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void StartSubscriberMonitor()
        {
            _subscriberThread = new Thread(RemoveDroppedSubscribers) {IsBackground = true};
            _subscriberThread.Start();
        }

        /// <summary>
        /// Gets the subscribers.
        /// </summary>
        public IEnumerable<Uri> Subscribers
        {
            get
            {
                lock (_lock)
                {
                    foreach (var subscriber in _subscribers)
                    {
                        yield return subscriber;
                    }
                }
            }
        }

        /// <summary>
        /// Dumps the subscribers list to a data file.
        /// </summary>
        public void SaveSubscriberData()
        {
            try
            {
                if (File.Exists(_dumpFile))
                {
                    File.Delete(_dumpFile);
                    TraceFactory.Logger.Debug("Deleted {0}".FormatWith(_dumpFile));
                }

                lock (_lock)
                {
                    if (_subscribers.Count > 0)
                    {
                        File.WriteAllText(_dumpFile, LegacySerializer.SerializeDataContract(_subscribers).ToString());
                        TraceFactory.Logger.Debug("Wrote out data for {0} subscribers".FormatWith(_subscribers.Count));
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Nothing to save...");
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to dump subscribers", ex);
            }
        }

        /// <summary>
        /// Loads the subscribers list from a data file.
        /// </summary>
        public void LoadSubscriberData()
        {
            if (!File.Exists(_dumpFile))
            {
                TraceFactory.Logger.Debug("Nothing to load...");
                return;
            }

            lock (_lock)
            {
                _subscribers = LegacySerializer.DeserializeDataContract<Collection<Uri>>(File.ReadAllText(_dumpFile));
            }

            TraceFactory.Logger.Debug("Loaded {0} subscribers".FormatWith(_subscribers.Count));
            try
            {
                File.Delete(_dumpFile);
                TraceFactory.Logger.Debug("Deleted {0}".FormatWith(_dumpFile));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error deleting saved subscriber file", ex);
            }
        }

        /// <summary>
        /// Releases the session.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public List<Task> ReleaseSession(string sessionId)
        {
            List<Task> taskList = new List<Task>();

            lock (_lock)
            {
                foreach (var subscriber in _subscribers)
                {
                    taskList.Add(Task.Factory.StartNew(() => ClearSessionHandler(sessionId, subscriber)));
                }
            }

            return taskList;
        }

        private void ClearSessionHandler(string sessionId, Uri subscriber)
        {
            try
            {
                using (var callback = GetConnection(subscriber))
                {
                    TraceFactory.Logger.Debug("ClearSession({0}, {1})".FormatWith(sessionId, subscriber.AbsoluteUri));
                    callback.Channel.ClearSession(sessionId);
                    TraceFactory.Logger.Debug("ClearSession({0}, {1})...DONE".FormatWith(sessionId, subscriber.AbsoluteUri));
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error: {0}: {1}".FormatWith(subscriber.AbsoluteUri, ex.Message));
                DropSubscriber(subscriber);
            }
        }

        /// <summary>
        /// Subscribes the specified endpoint to status updates.
        /// </summary>
        /// <param name="subscriber">The subscriber endpoint.</param>
        public void Subscribe(Uri subscriber)
        {
            lock (_lock)
            {
                if (!_subscribers.Contains(subscriber))
                {
                    _subscribers.Add(subscriber);
                    TraceFactory.Logger.Debug("Endpoint registered: {0}".FormatWith(subscriber));
                }
                else
                {
                    TraceFactory.Logger.Debug("Endpoint already registered: {0}".FormatWith(subscriber));
                }
            }
        }

        /// <summary>
        /// Unsubscribes the specified endpoint.
        /// </summary>
        /// <param name="subscriber">The endpoint.</param>
        public void Unsubscribe(Uri subscriber)
        {
            lock (_lock)
            {
                if (_subscribers.Remove(subscriber))
                {
                    TraceFactory.Logger.Info("Successfully unsubscribed endpoint: " + subscriber);
                }
                else
                {
                    TraceFactory.Logger.Info("Failed at unsubscribing the endpoint: " + subscriber);
                }
            }
        }

        /// <summary>
        /// Checks the subscriber's subscription to see if it's still active.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>true if the subscription is still active, otherwise false</returns>
        public bool CheckSubscription(Uri subscriber)
        {
            lock (_lock)
            {
                return _subscribers.Any(x => x.AbsoluteUri.Equals(subscriber.AbsoluteUri));
            }
        }

        /// <summary>
        /// Drops the subscriber.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        protected void DropSubscriber(Uri endpoint)
        {
            lock (_lock)
            {
                if (_subscribers.Contains(endpoint))
                {
                    TraceFactory.Logger.Debug("Dropping {0}".FormatWith(endpoint.AbsoluteUri));
                    _subscribers.Remove(endpoint);
                }
                else
                {
                    TraceFactory.Logger.Debug("Subscriber {0} not listed".FormatWith(endpoint.AbsoluteUri));
                }
            }

        }

        private void RemoveDroppedSubscribers()
        {
            TimeSpan sleepAmount = new TimeSpan(0, 1, 0);
            while (true)
            {
                Thread.Sleep(sleepAmount);

                lock (_lock)
                {
                    var itemsToRemove = new List<Uri>();

                    foreach (var subscriber in _subscribers)
                    {
                        using (var callback = GetConnection(subscriber))
                        {
                            try
                            {
                                TraceFactory.Logger.Debug("Ping({0})".FormatWith(subscriber.AbsoluteUri));
                                callback.Channel.Ping();
                                TraceFactory.Logger.Debug("Ping({0})...DONE".FormatWith(subscriber.AbsoluteUri));
                            }
                            catch (EndpointNotFoundException)
                            {
                                TraceFactory.Logger.Debug("EndpointNotFound {0}".FormatWith(subscriber));
                                itemsToRemove.Add(subscriber);
                            }
                            catch (TimeoutException)
                            {
                                TraceFactory.Logger.Debug("Timeout {0}".FormatWith(subscriber));
                                itemsToRemove.Add(subscriber);
                            }
                            catch (CommunicationException)
                            {
                                TraceFactory.Logger.Debug("CommunicationFailure {0}".FormatWith(subscriber));
                                itemsToRemove.Add(subscriber);
                            }
                            catch (Exception)
                            {
                                TraceFactory.Logger.Debug("GeneralFailure {0}".FormatWith(subscriber));
                                itemsToRemove.Add(subscriber);
                            }
                        }
                    }

                    foreach (var endpoint in itemsToRemove)
                    {
                        TraceFactory.Logger.Info("Removing subscriber: " + endpoint);
                        _subscribers.Remove(endpoint);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        protected SessionClientConnection GetConnection(Uri endpoint)
        {
            return SessionClientConnection.Create(endpoint);
        }
    }
}