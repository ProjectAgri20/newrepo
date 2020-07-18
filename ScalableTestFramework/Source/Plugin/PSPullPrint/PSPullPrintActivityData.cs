using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    [DataContract]
    public class PSPullPrintActivityData
    {
        /// <summary>
        /// Gets or sets the pull printing solution.
        /// </summary>
        /// <value>The pull printing solution.</value>
        ///
        [DataMember]
        public PullPrintingSolution PullPrintingSolution { get; set; }

        /// <summary>
        /// The default Safecom generated pin for the Safecom Users
        /// </summary>
        [DataMember]
        public string SafecomPin { get; set; }

        /// <summary>
        /// Tasks to be executed on the Control Panel
        /// </summary>
        [DataMember]
        public Collection<PrintManagementTask> PullPrintTasks { get; set; }

        [DataMember]
        public TimeSpan ActivityPacing { get; set; }

        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        public PSPullPrintActivityData()
        {
            PullPrintingSolution = new PullPrintingSolution();
            PullPrintTasks = new Collection<PrintManagementTask>();
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            SafecomPin = string.Empty;
            ActivityPacing = TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Execution Task class
    /// </summary>
    [DataContract]
    public class PrintManagementTask
    {
        /// <summary>
        /// The type of operation to be done. Eg: Print,Delete
        /// </summary>
        [DataMember]
        public SolutionOperation Operation { get; set; }

        /// <summary>
        /// The description of the task used to display in editor
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    /// The solution supported printing tasks
    /// </summary>
    public enum SolutionOperation
    {
        [Description("Print one job")]
        Print = 0,

        [Description("Print all jobs")]
        PrintAll,

        [Description("Delete one job")]
        Delete,

        [Description("Cancel job")]
        Cancel,

        [Description("Print and Keep job")]
        PrintKeep,

        [Description("Print and Delete job")]
        PrintDelete,

        [Description("UI Validation")]
        UIValidation,

        [Description("Sign Out")]
        SignOut
    }

    public enum PullPrintingSolution
    {
        /// <summary>
        /// Pull print using HP Access Control
        /// </summary>
        ///
        [Description("HP Access Control")]
        HPAC,

        /// <summary>
        /// Pull print using SafeCom
        /// </summary>
        [Description("Safecom")]
        SafeCom
    }
}