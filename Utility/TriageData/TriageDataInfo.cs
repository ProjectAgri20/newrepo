using HP.ScalableTestTriageData.Data.DataLog;
using System;
using System.Collections.Generic;

namespace HP.ScalableTestTriageData.Data
{
    public class TriageDataInfo
    {
        public PerformanceMarkerList PerformanceMarkers = new PerformanceMarkerList();
        public string DeviceId { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string Firmware { get; set; } = string.Empty;
        public string FirmwareDatecode { get; set; } = string.Empty;

        public string ActivityType { get; set; } = string.Empty;
        public string ActivityName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public Guid ActivityExecutionId { get; set; }
        public TriageDataInfo()
        {
            
        }
        public void AddPerformanceMarkers(IEnumerable<ActivityExecutionPerformance> performanceMarkers)
        {
            foreach (ActivityExecutionPerformance aep in performanceMarkers)
            {
                PerformanceMarkers.Add(aep);
            }
        }
    }
}
