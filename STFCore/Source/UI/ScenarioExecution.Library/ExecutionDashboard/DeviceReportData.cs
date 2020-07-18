using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.UI.SessionExecution
{
    public class DeviceReportData
    {
        public string Asset { get; set; }
        public string ProductName { get; set; }
        public string SessionId { get; set; }
        public string FirmwareRevision { get; set; }
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityType { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }

        public static List<DeviceReportData> GetReportData(string sessionId, string deviceId)
        {
            var result = new List<DeviceReportData>();
            if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(deviceId))
            {
                using (var context = DbConnect.DataLogContext())
                {
                    var device = context.DbSessions.Find(sessionId).Devices.FirstOrDefault(n => n.DeviceId.Equals(deviceId, StringComparison.OrdinalIgnoreCase));
                    string productName = device?.ProductName;
                    string firmwareRevision = device?.FirmwareRevision;

                    var reportData = from activity in context.SessionData(sessionId).Activities
                                     where activity.Assets.Contains(deviceId)
                                     orderby activity.StartDateTime descending
                                     select new DeviceReportData()
                                     {
                                         Asset = deviceId,
                                         ProductName = productName,
                                         SessionId = sessionId,
                                         FirmwareRevision = firmwareRevision,
                                         ActivityId = activity.ActivityExecutionId,
                                         ActivityName = activity.ActivityName,
                                         ActivityType = activity.ActivityType,
                                         StartTime = activity.StartDateTime,
                                         EndTime = activity.EndDateTime,
                                         Result = activity.ResultMessage,
                                         Status = activity.Status,
                                         HostName = activity.HostName,
                                         UserName = activity.UserName
                                     };
                    result = reportData.ToList();

                    // Convert start and end times to local time
                    foreach (DeviceReportData data in result)
                    {
                        data.StartTime = data.StartTime?.ToLocalTime();
                        data.EndTime = data.EndTime?.ToLocalTime();
                    }
                }
            }
            return result;
        }
    }
}
