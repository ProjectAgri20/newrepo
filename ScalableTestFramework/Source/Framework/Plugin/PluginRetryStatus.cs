namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// The result of a plugin's <see cref="PluginRetryAction" /> handling.
    /// </summary>
    public enum PluginRetryStatus
    {
        /// <summary>
        /// The plugin did not handle retries.
        /// </summary>
        DidNotRetry,

        /// <summary>
        /// Execution should continue without further retries.
        /// </summary>
        Continue,

        /// <summary>
        /// Execution should be halted.
        /// </summary>
        Halt
    }
}
