
namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Various states for an activity.
    /// </summary>
    public enum ActivityState
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Activity Halted
        /// </summary>
        Halted,

        /// <summary>
        /// Activity Started
        /// </summary>
        Started,

        /// <summary>
        /// Activity Failed
        /// </summary>
        Failed,

        /// <summary>
        /// Activity Skipped
        /// </summary>
        Skipped,

        /// <summary>
        /// Activity Completed
        /// </summary>
        Completed,

        /// <summary>
        /// Activity is being retried
        /// </summary>
        Retrying,

        /// <summary>
        /// Activity resulted in an error
        /// </summary>
        Error
    }
}
