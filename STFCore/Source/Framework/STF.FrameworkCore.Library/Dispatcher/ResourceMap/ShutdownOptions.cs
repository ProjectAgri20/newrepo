using System.Runtime.Serialization;
using System.Text;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Describes how to shutdown a running scenario.
    /// </summary>
    [DataContract]
    public class ShutdownOptions
    {
        /// <summary>
        /// Power off the VMs
        /// </summary>
        [DataMember]
        public bool PowerOff { get; set; }

        /// <summary>
        /// Options when Powering off the VMs
        /// </summary>
        [DataMember]
        public VMPowerOffOption PowerOffOption { get; set; }

        /// <summary>
        /// Copy all log files to a central location.
        /// </summary>
        [DataMember]
        public bool CopyLogs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow all workers to complete before shutting down.
        /// </summary>
        /// <value>
        /// <c>true</c> if all workers will complete before shutting down; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool AllowWorkerToComplete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to shutdown device simulators used in the session.
        /// </summary>
        /// <value>
        /// <c>true</c> if shutting down device simulators; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ShutdownDeviceSimulators { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to turn off paperless mode on the device.
        /// </summary>
        [DataMember]
        public bool DisableDeviceCrc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to release the device reservation on shutdown.
        /// </summary>
        [DataMember]
        public bool ReleaseDeviceReservation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to perform cleanup actions on shutdown.
        /// </summary>
        [DataMember]
        public bool PerformCleanup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to gather the event logs after a device testing is over
        /// <value>
        /// <c>false</c> unless gathering the device event logs after testing; otherwise, <c>true</c>.
        /// </value>
        /// </summary>
        [DataMember]
        public bool CollectDeviceEventLogs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to purge print queues.
        /// </summary>
        /// <value><c>true</c> if [purge print queues]; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool PurgeRemotePrintQueues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownOptions"/> class.
        /// </summary>
        public ShutdownOptions()
        {
            PowerOff = true;
            PowerOffOption = VMPowerOffOption.RevertToSnapshot;
            CopyLogs = false;
            AllowWorkerToComplete = false;
            ShutdownDeviceSimulators = true;
            ReleaseDeviceReservation = true;
            CollectDeviceEventLogs = false;
            PurgeRemotePrintQueues = true;
            PerformCleanup = true;
        }

        public override string ToString()
        {            
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"PowerOff: {PowerOff}");
            builder.AppendLine($"PowerOffOption: {PowerOffOption}");
            builder.AppendLine($"CopyLogs: {CopyLogs}");
            builder.AppendLine($"AllowWorkerToComplete: {AllowWorkerToComplete}");
            builder.AppendLine($"ShutdownDeviceSimulators: {ShutdownDeviceSimulators}");
            builder.AppendLine($"ReleaseDeviceReservation: {ReleaseDeviceReservation}");
            builder.AppendLine($"GatherDeviceEventLogs: {CollectDeviceEventLogs}");
            builder.AppendLine($"PurgePrintQueues: {PurgeRemotePrintQueues}");
            builder.AppendLine($"PerformCleanup: {PerformCleanup}");

            return builder.ToString();
        }
    }

    /// <summary>
    /// Virtual Machine power off options.
    /// </summary>
    public enum VMPowerOffOption
    {
        /// <summary>
        /// Power off.
        /// </summary>
        PowerOff,

        /// <summary>
        /// Revert to snapshot.
        /// </summary>
        RevertToSnapshot
    }
}