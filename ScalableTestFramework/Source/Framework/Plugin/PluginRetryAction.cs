namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// The action to be taken when a retry condition is met.
    /// </summary>
    public enum PluginRetryAction
    {
        /// <summary>
        /// Retry plugin execution.
        /// </summary>
        Retry,

        /// <summary>
        /// Continue without retrying.
        /// </summary>
        Continue,

        /// <summary>
        /// Halt further execution.
        /// </summary>
        Halt
    }
}
