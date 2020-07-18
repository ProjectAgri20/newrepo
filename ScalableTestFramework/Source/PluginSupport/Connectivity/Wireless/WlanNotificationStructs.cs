
namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{
    internal struct WlanConnectionNotificationEventData
    {
        public WlanNotificationData notifyData;
        public WlanConnectionNotificationData connNotifyData;
    }

    internal struct WlanReasonNotificationData
    {
        public WlanNotificationData notifyData;
        public WlanReasonCode reasonCode;
    }
}
