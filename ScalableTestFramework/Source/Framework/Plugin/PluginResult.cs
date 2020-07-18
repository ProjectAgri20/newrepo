namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Represents possible outcomes of an <see cref="IPluginExecutionEngine" /> activity.
    /// </summary>
    public enum PluginResult
    {
        /// <summary>
        /// The plugin completed execution with a successful outcome.
        /// </summary>
        /// <remarks>
        /// This result does not necessarily indicate a "passing" test case. It is only
        /// an indication that the plugin was able to complete the scope of its intent.
        /// </remarks>
        Passed,

        /// <summary>
        /// The plugin did not complete succesfully due to a condition within
        /// the scope of intent of the plugin.
        /// </summary>
        /// <remarks>
        /// This result does not necessarily indicate a "failing" test case. It is only
        /// an indication that the plugin encountered an issue within the scope of its
        /// intent that prevented the desired result of execution.
        /// </remarks>
        Failed,

        /// <summary>
        /// The plugin encountered a condition that caused this activity to be skipped.
        /// </summary>
        /// <remarks>
        /// This result should be used to indicate that a plugin stopped execution due to
        /// an issue that prevents meaningful results. For instance, a plugin that
        /// requires access to an external resource but cannot obtain it may wish to
        /// return this result to indicate that no work could be performed.
        /// </remarks>
        Skipped,

        /// <summary>
        /// The plugin encountered an internal error or threw an unhandled exception.
        /// </summary>
        /// <remarks>
        /// This result should be used to indicate that plugin execution was halted
        /// due to an error condition outside the plugin's scope of intent. This generally
        /// indicates an issue with the plugin that should be addressed by the developer.
        /// </remarks>
        Error
    }
}
