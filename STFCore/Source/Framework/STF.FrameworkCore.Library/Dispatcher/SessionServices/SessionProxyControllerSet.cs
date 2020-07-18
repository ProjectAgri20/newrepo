using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    [DataContract]
    public class SessionProxyControllerSet
    {
        [DataMember]
        private readonly Dictionary<string, SessionProxyController> _controllers = new Dictionary<string, SessionProxyController>();

        /// <summary>
        /// Gets the maximum number of entries.
        /// </summary>
        public int Maximum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProxyControllerSet"/> class.
        /// </summary>
        /// <param name="maxEntries">The maximum entries.</param>
        public SessionProxyControllerSet(int maxEntries)
        {
            Maximum = maxEntries;
        }

        /// <summary>
        /// Determines if the specified session unique identifier exists.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns>True if the session exists, otherwise false</returns>
        public bool Contains(string sessionId)
        {
            return _controllers.ContainsKey(sessionId);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _controllers.Clear();
        }

        /// <summary>
        /// Starts the Session Proxy process if not already running.
        /// </summary>
        public void StartProxyProcess(string sessionId)
        {
            try
            {
                SessionProxyController controller = null;
                if (!_controllers.TryGetValue(sessionId, out controller))
                {
                    controller = new SessionProxyController(sessionId);
                    _controllers.Add(sessionId, controller);
                }

                TraceFactory.Logger.Debug(">>>> {0} - Starting Proxy Process...".FormatWith(sessionId));
                controller.StartProxyProcess();
                TraceFactory.Logger.Debug(">>>> {0} - Starting Proxy Process...Done".FormatWith(sessionId));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the <see cref="SessionProxyController"/> with the specified session unique identifier.
        /// </summary>
        /// <value>
        /// The <see cref="SessionProxyController"/>.
        /// </value>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns></returns>
        public SessionProxyController this[string sessionId]
        {
            get { return _controllers[sessionId]; }
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Adding this entry will exceed the maximum allowable of {0}.FormatWith(Maximum)</exception>
        public void Add(SessionProxyController entry)
        {
            if (_controllers.Count() + 1 > Maximum)
            {
                throw new ArgumentOutOfRangeException("Adding this entry will exceed the maximum allowable of {0}".FormatWith(Maximum));
            }

            _controllers.Add(entry.SessionId, entry);
        }

        public bool TryGetValue(string sessionId, out SessionProxyController controller)
        {
            return _controllers.TryGetValue(sessionId, out controller);
        }

        /// <summary>
        /// Gets the count of sessions.
        /// </summary>
        public int Count
        {
            get { return _controllers.Count; }
        }

        /// <summary>
        /// Removes an entry based on the specified session unique identifier.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public void Remove(SessionProxyController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            if (_controllers.ContainsKey(controller.SessionId))
            {
                _controllers.Remove(controller.SessionId);
            }
        }

        /// <summary>
        /// Gets the <see cref="SessionProxyController"/> entries.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IEnumerable<SessionProxyController> Values
        {
            get { return _controllers.Values; }
        }

        /// <summary>
        /// Gets the session Ids currently being tracked.
        /// </summary>
        public IEnumerable<string> SessionIds
        {
            get { return _controllers.Keys; }
        }
    }
}
