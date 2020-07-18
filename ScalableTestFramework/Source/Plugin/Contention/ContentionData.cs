using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Contains data needed to execute the Contention plugin
    /// </summary>
    [DataContract]
    public class ContentionData
    {
        /// <summary>
        /// Gets or sets the list of selected Control Panel activities
        /// </summary>
        [DataMember]
        public List<object> SelectedControlPanelActivities { get; set; }

        /// <summary>
        /// Gets or sets the list of selected Contention activities
        /// </summary>
        [DataMember]
        public List<object> SelectedContentionActivities { get; set; }

        /// <summary>
        /// Gets or sets the lock timeouts.
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentionData"/> class
        /// </summary>
        public ContentionData()
        {
            SelectedControlPanelActivities = new List<object>();
            SelectedContentionActivities = new List<object>();
            LockTimeouts = new LockTimeoutData(TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(5));
        }
    }

    /// <summary>
    /// Enumeration for Types of Scan jobs in Contention plugin
    /// </summary>
    public enum ContentionScanActivityTypes
    {
        /// <summary>
        /// Scan To Email job
        /// </summary>
        [Description("Scan To Email Job")]
        Email,

        /// <summary>
        /// Scan To Folder Job
        /// </summary>
        [Description("Scan To Folder Job")]
        Folder,

        /// <summary>
        /// Scan To USB Job
        /// </summary>
        [Description("Scan To USB Job")]
        USB,

        /// <summary>
        /// Scan To Job Storage Job
        /// </summary>
        [Description("Scan To Job Storage Job")]
        JobStorage
    }
}
