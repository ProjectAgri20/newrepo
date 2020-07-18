using System;
using Microsoft.Exchange.WebServices.Data;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// A subscription to an Exchange server which listens for events asynchronously.
    /// </summary>
    public sealed class ExchangeServiceSubscription : IDisposable
    {
        private readonly ExchangeService _service;
        private readonly StreamingSubscriptionConnection _subscriptionConnection;

        /// <summary>
        /// Occurs when the subscription receives a notification.
        /// </summary>
        public event EventHandler SubscriptionNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeServiceSubscription" /> class.
        /// </summary>
        /// <param name="service">The <see cref="ExchangeService" />.</param>
        /// <param name="folder">The folder to monitor.</param>
        internal ExchangeServiceSubscription(ExchangeService service, WellKnownFolderName folder)
        {
            _service = service;
            _subscriptionConnection = new StreamingSubscriptionConnection(_service, 30); // 30 minutes is the max

            StreamingSubscription subscription = _service.SubscribeToStreamingNotifications(new FolderId[] { folder }, EventType.NewMail);
            _subscriptionConnection.AddSubscription(subscription);
            _subscriptionConnection.OnNotificationEvent += (s, e) => SubscriptionNotification?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Starts monitoring the Exchange server for changes.
        /// </summary>
        public void Start()
        {
            if (!_subscriptionConnection.IsOpen)
            {
                _subscriptionConnection.Open();
            }
        }

        /// <summary>
        /// Stops monitoring the Exchange server.
        /// </summary>
        public void Stop()
        {
            if (_subscriptionConnection.IsOpen)
            {
                _subscriptionConnection.Close();
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
            _subscriptionConnection.Dispose();
        }

        #endregion
    }
}
