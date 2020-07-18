using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    internal class PhysicalDeviceJobLogger : FrameworkDataLog
    {
        public override string TableName => "PhysicalDeviceJob";

        public override string PrimaryKeyColumn => nameof(PhysicalDeviceJobId);

        public PhysicalDeviceJobLogger()
        {
            PhysicalDeviceJobId = SequentialGuid.NewGuid();
            MonitorStartDateTime = DateTime.Now;
        }

        /// <summary>
        /// Unique id for this log entry (internally generated).
        /// </summary>
        /// <value>The physical device job log identifier.</value>
        [DataLogProperty]
        public Guid PhysicalDeviceJobId { get; private set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the activity execution id.
        /// Typically the activity execution id within the session
        /// </summary>
        /// <value>The activity execution identifier.</value>
        [DataLogProperty]
        public Guid ActivityExecutionId { get; set; }

        [DataLogProperty]
        public string DeviceId { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public string JobId { get; set; }

        [DataLogProperty(IncludeInUpdates = true, MaxLength = 255)]
        public string JobName { get; set; }

        [DataLogProperty(IncludeInUpdates = true, MaxLength = 255)]
        public string JobApplicationName { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public string JobCategory { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? JobStartDateTime { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? JobEndDateTime { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public string JobEndStatus { get; set; }

        [DataLogProperty]
        public DateTimeOffset? MonitorStartDateTime { get; private set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? MonitorEndDateTime { get; set; }
    }
}
