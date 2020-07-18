using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Manages all <see cref="SessionMapObject"/> status changes and publishes them to all 
    /// listening subscribers.
    /// </summary>
    public class SessionProxyEventPublisher : EventPublisherBase
    {
        private readonly string _sessionId = string.Empty;
        private readonly Collection<SessionMapElement> _elements = null;
        private SessionState _lastState = SessionState.Unavailable;
        private readonly object _lock = new object();

        /// <summary>
        /// Occurs when a status change is received.
        /// </summary>
        public event EventHandler<SessionMapElementEventArgs> MapElementChangeReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProxyEventPublisher"/> class.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionProxyEventPublisher(string sessionId)
        {
            _elements = new Collection<SessionMapElement>();
            _sessionId = sessionId;
        }

        /// <summary>
        /// Registers the specified element map with this publisher.
        /// </summary>
        /// <param name="root">The root map element.</param>
        public void RegisterMapElements(ISessionMapElement root)
        {
            Register(root);
        }

        private void Register(ISessionMapElement item)
        {
            TraceFactory.Logger.Debug("Registering {0} : {1}".FormatWith(item.MapElement.ElementType, item.MapElement.Name));
            item.MapElement.OnUpdateStatus += new EventHandler(PublishSessionMapElement);

            // Recursively iterate through all properties marked with the
            // [SessionMapElementCollection] attribute and register them with the publisher
            // so that any changes to the status for those items will be sent
            // out to listening clients.  This will only process the first
            // result, but in the future if there is more than one property
            // it can be extended.  Also wire the parent/child relationship.
            var property =
                (
                    from p in item.GetType().GetProperties()
                    where Attribute.IsDefined(p, typeof(SessionMapElementCollectionAttribute), false)
                    select p
                ).FirstOrDefault();

            if (property != null)
            {
                foreach (var entry in (IEnumerable<ISessionMapElement>)property.GetValue(item, null))
                {
                    entry.MapElement.ParentId = item.MapElement.Id;
                    entry.MapElement.SessionId = _sessionId;
                    Register(entry);
                }
            }
        }

        /// <summary>
        /// Publishes the session startup transition.
        /// </summary>
        /// <param name="transition">The transition.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="callbackAddress">The callback address.</param>
        /// <param name="background">Whether to run this process on a background thread.</param>
        public void PublishSessionStartupTransition(SessionStartupTransition transition, string sessionId, Uri callbackAddress, bool background = true)
        {
            if (background)
            {
                ThreadPool.QueueUserWorkItem(t => PublishSessionStartupTransitionHandler(transition, sessionId, callbackAddress));
            }
            else
            {
                PublishSessionStartupTransitionHandler(transition, sessionId, callbackAddress);
            }

            TraceFactory.Logger.Debug("Session Step: {0}".FormatWith(transition));
        }

        /// <summary>
        /// Publishes the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="sessionId">The session id.</param>
        public void PublishSessionState(SessionState state, string sessionId)
        {
            _lastState = state;

            // Spin off threads to update all listening clients
            foreach (var subscriber in Subscribers)
            {
                ThreadPool.QueueUserWorkItem(t => PublishSessionStateHandler(subscriber, string.Empty));
            }

            TraceFactory.Logger.Debug("Session State (1): {0}".FormatWith(state));
        }

        /// <summary>
        /// Publishes the specified state using Tasks.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="sessionId">The session Id.</param>
        /// <returns></returns>
        public List<Task> PublishSessionStateInTask(SessionState state, string sessionId)
        {
            _lastState = state;

            List<Task> taskList = new List<Task>();

            // Spin off threads to update all listening clients
            foreach (var subscriber in Subscribers)
            {
                taskList.Add(Task.Factory.StartNew(() => PublishSessionStateHandler(subscriber, string.Empty)));
            }

            TraceFactory.Logger.Debug("Session State (1): {0}".FormatWith(state));

            return taskList;
        }

        /// <summary>
        /// Publishes the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="message">The message.</param>
        public void PublishSessionState(SessionState state, string sessionId, string message)
        {
            _lastState = state;

            // Spin off threads to update all listening clients
            foreach (var subscriber in Subscribers)
            {
                ThreadPool.QueueUserWorkItem(t => PublishSessionStateHandler(subscriber, message));
            }

            TraceFactory.Logger.Debug("Session State (2): {0}".FormatWith(state));
        }

        /// <summary>
        /// Publishes an update to a SessionMapElement.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The Event Argument.</param>
        public void PublishSessionMapElement(object sender, EventArgs e)
        {
            var element = sender as SessionMapElement;

            lock (_lock)
            {
                if (!_elements.Any(n => n.Id == element.Id))
                {
                    _elements.Add(element);
                }
                else
                {
                    var item = _elements.Where(x => x.Id == element.Id).FirstOrDefault();
                    if (item != null)
                    {
                        item.UpdateFrom(element);
                    }
                }
            }

            MapElementChangeReceived?.Invoke(this, new SessionMapElementEventArgs(element));

            foreach (var subscriber in Subscribers)
            {
                ThreadPool.QueueUserWorkItem(t => PublishSessionMapElementHandler(element, subscriber));
            }
        }

        private void PublishSessionMapElementHandler(SessionMapElement element, Uri subscriber)
        {
            try
            {
                using (SessionClientConnection callback = GetConnection(subscriber))
                {
                    //TraceFactory.Logger.Debug("PublishSessionMapElement({0}, {1})".FormatWith(element.Name, subscriber.AbsoluteUri));
                    callback.Channel.PublishSessionMapElement(element);
                    //TraceFactory.Logger.Debug("PublishSessionMapElement({0}, {1})...DONE".FormatWith(element.Name, subscriber.AbsoluteUri));
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error: {0}: {1}".FormatWith(subscriber.AbsoluteUri, ex.Message));
                DropSubscriber(subscriber);
            }
        }

        private void PublishSessionStateHandler(Uri subscriber, string message)
        {
            try
            {
                using (var callback = GetConnection(subscriber))
                {
                    //TraceFactory.Logger.Debug("PublishSessionState({0}:{1}:{2})".FormatWith(_lastState, _sessionId, message));
                    callback.Channel.PublishSessionState(_lastState, _sessionId, message);
                    //TraceFactory.Logger.Debug("PublishSessionState({0}:{1}:{2})...DONE".FormatWith(_lastState, _sessionId, message));
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error: {0}: {1}".FormatWith(subscriber.AbsoluteUri, ex.Message));
                DropSubscriber(subscriber);
            }
        }

        private void PublishSessionStartupTransitionHandler(SessionStartupTransition transition, string sessionId, Uri subscriber)
        {
            try
            {
                using (var callback = GetConnection(subscriber))
                {
                    //TraceFactory.Logger.Debug("Calling PublishSessionStartupTransition({0}) on client".FormatWith(transition));
                    callback.Channel.PublishSessionStartupTransition(transition, sessionId);
                    //TraceFactory.Logger.Debug("Calling PublishSessionStartupTransition({0}) on client...DONE".FormatWith(transition));
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error: {0}: {1}".FormatWith(subscriber.AbsoluteUri, ex.Message));
                DropSubscriber(subscriber);
            }
        }

        
        /// <summary>
        /// Sends all status nodes to the requesting client.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void RefreshSubscriber(Uri subscriber)
        {
            TraceFactory.Logger.Info("Refreshing {0}".FormatWith(subscriber));

            ThreadPool.QueueUserWorkItem(t =>
            {
                PublishSessionStateHandler(subscriber, string.Empty);
            });

            lock (_lock)
            {
                ThreadPool.QueueUserWorkItem(t =>
                {
                    foreach (var element in _elements)
                    {
                        PublishSessionMapElementHandler(element, subscriber);
                    }
                });
            }
        }

        /// <summary>
        /// Refreshes all subscribers.
        /// </summary>
        public void RefreshAllSubscribers()
        {
            foreach (var subscriber in Subscribers)
            {
                RefreshSubscriber(subscriber);
            }
        }
    }
}