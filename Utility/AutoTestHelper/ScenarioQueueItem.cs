using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Xml.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.RDL.EDT.AutoTestHelper
{
    /// <summary>
    /// Class to hold Scenario Queue Item
    /// </summary>
    [Serializable]
    public class ScenarioQueueItem
    {
        public Guid ScenarioId { get; set; }

        public string ScenarioName { get; set; }

        public int Distribution { get; set; }

        [XmlIgnore]
        public bool Active { get; set; }

        [XmlIgnore]
        public string Description { get; set; }

        //public string Retention { get; set; }

        [XmlIgnore]
        public string HoldId { get; set; }

        [XmlIgnore]
        public string Status { get; set; }

        [XmlIgnore]
        public string SessionId { get; set; }

        [XmlIgnore]
        public TimeSpan ExecutionTime { get; set; }

        [XmlIgnore]
        public int TotalActivities => TestResult.Count;

        [XmlIgnore]
        public int PassedActivities { get { return TestResult.Count(x => x.Value); } }

        [XmlIgnore]
        public string Result => $"{PassedActivities}/{TotalActivities} Passed";

        [XmlIgnore]
        public int StartPrintCount { get; set; }

        [XmlIgnore]
        public int StopPrintCount { get; set; }

        [XmlIgnore]
        public string PrintCount => $"S: {StartPrintCount} E: {StopPrintCount}";

        [XmlIgnore]
        public string ScanCount => $"S: {StartScanCount} E: {StopScanCount}";

        [XmlIgnore]
        public int StartScanCount { get; set; }

        [XmlIgnore]
        public int StopScanCount { get; set; }

        [XmlIgnore]
        public int ExecutedCount { get; set; }

        [XmlIgnore]
        public Dictionary<int, bool> TestResult { get; set; }

        [XmlIgnore]
        public string ToolTip
        {
            get
            {
                if (string.IsNullOrEmpty(Status))
                {
                    return string.Empty;
                }
                if (Status.Equals("Completed"))
                {
                    return TestSummary();
                }
                return string.Empty;
            }
        }

        [XmlIgnore]
        public string GroupName { get; set; }

        public ScenarioQueueItem()
        {
            TestResult = new Dictionary<int, bool>();
            Active = true;
            StartPrintCount = StartScanCount = ExecutedCount = StopScanCount = StopPrintCount = 0;
        }

        private string TestSummary()
        {
            StringBuilder summaryText = new StringBuilder();
            foreach (var test in TestResult)
            {
                summaryText.AppendLine(test.Value
                    ? $"TestCase {test.Key}: Passed"
                    : $"TestCase {test.Key}: Failed");
            }

            return summaryText.ToString();
        }
    }

    /// <summary>
    /// Class to hold test instance data
    /// </summary>
    public class TestInstanceData
    {
        /// <summary>
        /// The Test Instance Id, this is mapped to DDS
        /// </summary>
        public Guid TestInstanceId { get; set; }

        /// <summary>
        /// list of sessions executed under this test instance
        /// </summary>
        public List<string> SessionIds { get; set; }

        /// <summary>
        /// The selected scenarios for this test instance
        /// </summary>
        public List<ScenarioQueueItem> ActiveScenarios { get; set; }

        /// <summary>
        /// The directory which has the base firmware files
        /// </summary>
        public string BaseFirmwarePath { get; set; }

        /// <summary>
        /// The directory which has the target firmware files
        /// </summary>
        public string TargetFirmwarePath { get; set; }

        /// <summary>
        /// The selected device for this test instance
        /// </summary>
        public AssetInfoCollection DeviceAssetInfoCollection { get; set; }

        /// <summary>
        /// (Optional) BashCollector host to collect bashlogs
        /// </summary>
        public string BashCollectorHost { get; set; }

        /// <summary>
        /// DDS: RunId for this TestInstance
        /// </summary>
        public Guid RunId { get; set; }
        /// <summary>
        /// DDS: Name of the Run
        /// </summary>
        public string RunName { get; set; }

        /// <summary>
        /// Print Count at the start of test instance
        /// </summary>
        public int StartPrintCount { get; set; }

        /// <summary>
        /// Print Count at the end of test instance
        /// </summary>
        public int StopPrintCount { get; set; }

        /// <summary>
        /// Print Count at the start of test instance
        /// </summary>
        public int StartScanCount { get; set; }

        /// <summary>
        /// Print Count at the end of test instance
        /// </summary>
        public int StopScanCount { get; set; }

        /// <summary>
        /// Total images printed for this test instance
        /// </summary>
        public int TotalImages { get; set; }

        /// <summary>
        /// Total power cycles performed for this test instance
        /// </summary>
        public int TotalPowerCycles { get; set; }

        /// <summary>
        /// Total firmware upgrades performed for this test instance
        /// </summary>
        public int TotalFimCycles { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public TestInstanceData()
        {
            ActiveScenarios = new List<ScenarioQueueItem>();
            SessionIds = new List<string>();
            TotalFimCycles = 0;
        }
    }

    /// <summary>
    /// Converts visibility to negate bool
    /// </summary>
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion IValueConverter Members
    }
}
