namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Interface for the execution portion of a plugin.
    /// </summary>
    public interface IPluginExecutionEngine
    {
        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" /> to use for execution.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        PluginExecutionResult Execute(PluginExecutionData executionData);
    }
}
