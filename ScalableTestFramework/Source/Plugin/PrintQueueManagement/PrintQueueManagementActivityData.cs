using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    /// <summary>
    /// Print Queue Management Activity Data
    /// </summary>

    [DataContract]
    public class PrintQueueManagementActivityData
    {
        /// <summary>
        /// The print queue management tasks, such as install, upgrade, uninstall
        /// </summary>
        [DataMember]
        public Collection<PrintQueueManagementTask> PrintQueueTasks { get; set; }

        /// <summary>
        /// Indicates whether the driver files should be copied locally and then installed, else it will be installed from the specified location
        /// </summary>
        [DataMember]
        public bool LocalCacheInstall { get; set; }

        /// <summary>
        /// The time delay for performing each activity
        /// </summary>
        [DataMember]
        public TimeSpan ActivityPacing { get; set; }

        /// <summary>
        /// Set the print queue as default
        /// </summary>
        [DataMember]
        public bool IsDefaultPrinter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintQueueManagementActivityData()
        {
            PrintQueueTasks = new Collection<PrintQueueManagementTask>();
            LocalCacheInstall = true;
        }
    }

    /// <summary>
    /// The Print Queue Management task class
    /// </summary>
    [DataContract]
    public class PrintQueueManagementTask
    {
        /// <summary>
        /// The type of operation to be done. Eg: Install, Uninstall
        /// </summary>
        [DataMember]
        public PrintQueueOperation Operation { get; set; }

        /// <summary>
        /// Optional argument to the operation, eg: filename, drivername
        /// </summary>
        [DataMember]
        public object TargetObject { get; set; }

        /// <summary>
        /// Printqueue preference for configuration task
        /// </summary>
        [DataMember]
        public PrintQueuePreferences Preference { get; set; }

        /// <summary>
        /// The description of the task used to display in editor
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The status of the task upon execution, used in execution controller only
        /// </summary>
        [IgnoreDataMember]
        public Status Status { get; set; }

        /// <summary>
        /// The delay before the job gets cancelled
        /// </summary>
        [DataMember]
        public int Delay { get; set; }

        /// <summary>
        /// dummy constructor
        /// </summary>
        public PrintQueueManagementTask()
        {
            Delay = 0;

        }
    }

    /// <summary>
    /// Enum object for Print Queue Management Task operation
    /// </summary>
#pragma warning disable 1591

    public enum PrintQueueOperation
    {
        Install = 0,
        Configure,
        Upgrade,
        Uninstall,
        Print,
        Cancel
    }

    [DataContract]
    public class PrintQueuePreferences
    {
        [DataMember]
        public string PaperSize { get; set; }

        [DataMember]
        public string InputTray { get; set; }

        [DataMember]
        public string Orientation { get; set; }

        [DataMember]
        public string PaperType { get; set; }

        [DataMember]
        public string DuplexValue { get; set; }

        [DataMember]
        public int Copies { get; set; }

        [DataMember]
        public string Duplexer { get; set; }

        public override string ToString()
        {
            return
                $"PaperSize: {PaperSize}, PaperType: {PaperType}, InputTray: {InputTray}, Orientation: {Orientation}, Copies: {Copies}, Duplex: {DuplexValue}, Duplexer: {Duplexer}";
            //,PaperSize, PaperType, InputTray, Orientation, Copies, DuplexValue, Duplexer);
        }
    }

    /// <summary>
    /// Enum used for displaying print task execution status
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// yet to be executed
        /// </summary>
        Queued,

        /// <summary>
        /// activity passed
        /// </summary>
        Passed,

        /// <summary>
        /// activity failed
        /// </summary>
        Failed,

        /// <summary>
        /// activity skipped
        /// </summary>
        Skipped,
    }

#pragma warning restore 1591
}