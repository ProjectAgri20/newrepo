using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{

    /// <summary>
    /// Class that defines the scenario setting defaults to be run during a session
    /// </summary>
    [DataContract]
    public class ScenarioSettings
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ScenarioSettings"/>
        /// </summary>
        public ScenarioSettings()
        {
            NotificationSettings = new FailNotificationSettings();
            TargetCycle = string.Empty;
            EstimatedRunTime = -1;
            LogLocation = string.Empty;
            CollectEventLogs = false;
            
        }


        //Cycle
        /// <summary>
        /// The target cycle
        /// </summary>
        [DataMember]
        public string TargetCycle { get; set; }


        /// <summary>
        /// Estimated Runtime ???
        /// </summary>
        [DataMember]
        public int EstimatedRunTime { get; set; }

        /// <summary>
        /// Notification settings upon a failure
        /// </summary>
        [DataMember]
        public FailNotificationSettings NotificationSettings { get; set; }
        /// <summary>
        /// Location of log files
        /// </summary>
        [DataMember]
        public string LogLocation { get; set; }
        /// <summary>
        /// Flag to collect event logs
        /// </summary>
        [DataMember]
        public bool CollectEventLogs { get; set; }

        /// <summary>
        /// Used for storing custom name value pairs to be used by utilities 
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ScenarioCustomDictionary { get; set; }

    }
    /// <summary>
    /// Data for Fail Notification Settings
    /// </summary>
    [DataContract]
    public class FailNotificationSettings
    {
        /// <summary>
        /// Flag to collect DART logs
        /// </summary>
        [DataMember]
        public bool CollectDartLogs { get; set; }
        /// <summary>
        /// List of email addresses to fire notification to
        /// </summary>
        [DataMember]
        public string Emails { get; set; }
        /// <summary>
        /// Number of failures in the session
        /// </summary>
        [DataMember]
        public int FailureCount { get; set; }
        /// <summary>
        /// List of triggers to fire notifications
        /// </summary>
        [DataMember]
        public string[] TriggerList { get; set; }
        /// <summary>
        /// Timespan denoting the down time
        /// </summary>
        [DataMember]
        public TimeSpan FailureTime { get; set; }



        /// <summary>
        /// Fail Notification Settings
        /// </summary>
        public FailNotificationSettings()
        {
            CollectDartLogs = false;
            Emails = string.Empty;
            FailureCount = 0;
            FailureTime = TimeSpan.FromMinutes(10);

        }

    }
}
