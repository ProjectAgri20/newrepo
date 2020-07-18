using System;
using System.Data;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.DartLog;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Provides Notification emails to users based on failure threshold and time segment.
    /// </summary>
    public class FailNotification
    {
        private Timer _failureTimer;
        private FailNotificationInfo _failInfo;
        private static string[] _triggerList;

        /// <summary>
        /// Instantiates a new FailNotificatiion object.
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="sessionName">The session name</param>
        /// <param name="failureCount">The Failure Threshold</param>
        /// <param name="emailAddresses">The email addresses to send to.</param>
        /// <param name="collectDart">Whether or not to collect DART logs.</param>
        public FailNotification(string sessionId, string sessionName, int failureCount, string emailAddresses, bool collectDart, string[] autoTriggerList)
        {
            _triggerList = autoTriggerList;

            _failInfo = new FailNotificationInfo(sessionId, sessionName, failureCount, emailAddresses, collectDart);

            // Set the timer to fire every minute.  It needs to fire this rapidly to ensure that if Dart Logs are to be collected,
            // they are collected before the buffer on the Dart card is overrun.
            _failureTimer = new Timer(FailureNotify, _failInfo, 30000, 60000);

        }

        /// <summary>
        /// Checks the current time interval for any failures.
        /// </summary>
        /// <param name="state"></param>
        public static void FailureNotify(object state)
        {
            FailNotificationInfo notificationInfo = (FailNotificationInfo)state;
            DartLogCollectorClient dartClient = null;

            TraceFactory.Logger.Debug("Executing Failure Check");
            try
            {
                using (DataLogContext context = DbConnect.DataLogContext())
                {
                    // Only grab activities for the last minute
                    DateTime now = DateTime.Now;
                    DateTime adjustedTimeSpan = now - TimeSpan.FromMinutes(1);

                    // Get activities for this session, grouped by device
                    var assetActivities = from activity in context.SessionData(notificationInfo.SessionId).Activities
                                          from asset in activity.Assets
                                          where activity.EndDateTime < now && activity.EndDateTime > adjustedTimeSpan
                                          orderby activity.EndDateTime
                                          group new { activity.ActivityName, activity.Status, activity.EndDateTime, activity.ResultMessage } by asset;

                    if (notificationInfo.CollectDartLog)
                    {
                        dartClient = new DartLogCollectorClient();
                    }

                    foreach (var deviceData in assetActivities) //deviceData.Key = AssetId
                    {
                        if (!notificationInfo.FailureInfo.ContainsKey(deviceData.Key))
                        {
                            notificationInfo.FailureInfo.Add(deviceData.Key, new DeviceFailureInfo());
                        }

                        DeviceFailureInfo deviceFailureInfo = notificationInfo.FailureInfo[deviceData.Key];

                        foreach (var activity in deviceData)
                        {
                            if (activity.Status == "Failed" || activity.Status == "Error")
                            {
                                deviceFailureInfo.FailureCount++;
                            }
                            else if (activity.Status == "Passed")  //reset if you get a pass.
                            {
                                deviceFailureInfo.FailureCount = 0;
                                deviceFailureInfo.EmailSent = false;
                            }

                            //Check against pull triggers to determine whether or not we have to automatically pull dart logs.
                            if (_triggerList.Count() != 0 && deviceFailureInfo.EmailSent == false && !string.IsNullOrEmpty(activity.ResultMessage))
                            {
                                char[] separators = new char[] { '?' };
                                foreach (var item in _triggerList)
                                {
                                    if (deviceFailureInfo.EmailSent == false)
                                    {
                                        int index = 0;
                                        var stringComponents = item.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var stringPiece in stringComponents)
                                        {
                                            //Look for the first piece
                                            index = activity.ResultMessage.IndexOf(stringPiece, index);

                                            if (index == -1)
                                            {
                                                break;
                                            }
                                            index++;

                                        }
                                        if (index != -1)
                                        {
                                            TraceFactory.Logger.Debug("Sending Notification Email");
                                            dartClient.CollectLog(deviceData.Key, notificationInfo.SessionId, notificationInfo.ToAddresses);

                                            notificationInfo.Message.Body = notificationInfo.Message.Body + string.Format(": Device- {0}, Activity: {1}, Time: {2}, Error string match found", deviceData.Key, activity.ActivityName, activity.EndDateTime);
                                            notificationInfo.SmtpMail.Send(notificationInfo.Message);
                                            notificationInfo.ResetMessageBody();
                                            deviceFailureInfo.EmailSent = true;
                                        }
                                    }
                                }
                            }

                            if (deviceFailureInfo.FailureCount >= notificationInfo.RuleValue && deviceFailureInfo.EmailSent == false)
                            {
                                if (notificationInfo.CollectDartLog)
                                {
                                    dartClient.CollectLog(deviceData.Key, notificationInfo.SessionId, notificationInfo.ToAddresses);
                                }
                                TraceFactory.Logger.Debug("Sending Notification Email");
                                notificationInfo.Message.Body = notificationInfo.Message.Body + string.Format(": Device- {0}, Activity: {1}, Time: {2}", deviceData.Key, activity.ActivityName, activity.EndDateTime);
                                notificationInfo.SmtpMail.Send(notificationInfo.Message);
                                notificationInfo.ResetMessageBody();
                                TraceFactory.Logger.Debug($"Device {deviceData.Key} breached the failure threshold of {notificationInfo.RuleValue}.  Email Sent.  CollectDartLog={notificationInfo.CollectDartLog}");
                                deviceFailureInfo.EmailSent = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TraceFactory.Logger.Debug(e.ToString());
            }
        }
    }
}