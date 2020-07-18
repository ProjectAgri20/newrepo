using System;
using System.Linq;
using System.Threading;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Manages all events happening on the dispatcher and publishes them to all listening subscribers.
    /// </summary>
    public class SessionDispatcherEventPublisher : EventPublisherBase
    {
        public SessionDispatcherEventPublisher()
        {
            //StartSubscriberMonitor();
        }

        /// <summary>
        /// Sends the general dispatcher error to subscribed clients.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void PublishDispatcherException(Exception exception)
        {
            TraceFactory.Logger.Error("Unhandled Exception", exception);

            if (exception == null)
            {
                return;
            }

            // Spin off threads to update all listening clients
            foreach (var subscriber in Subscribers)
            {
                ThreadPool.QueueUserWorkItem(t => PublishDispatcherExceptionHandler(exception, subscriber));
            }
        }

        private void PublishDispatcherExceptionHandler(Exception exception, Uri subscriber)
        {
            try
            {
                using (var callback = GetConnection(subscriber))
                {
                    TraceFactory.Logger.Debug("PublishDispatcherException()");
                    callback.Channel.PublishDispatcherException(exception.JoinAllErrorMessages(), exception.ToString());
                    TraceFactory.Logger.Debug("PublishDispatcherException()...DONE");
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Error: {0}: {1}".FormatWith(subscriber.AbsoluteUri, ex.Message));
                DropSubscriber(subscriber);
            }
        }
    }
}