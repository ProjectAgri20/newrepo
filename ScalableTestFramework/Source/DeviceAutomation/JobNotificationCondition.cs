using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Notification conditions for a job.
    /// </summary>
    public enum NotifyCondition
    {
        /// <summary>
        /// Notification will not notify
        /// </summary>
        [Description("NeverNotify")]
        NeverNotify,

        /// <summary>
        /// Notification will always be sent.
        /// </summary>
        [Description("Always")]
        Always,

        /// <summary>
        /// Notification will be sent only if the job fails.
        /// </summary>
        [Description("OnlyIfJobFails")]
        OnlyIfJobFails
    }
}
