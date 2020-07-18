using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Represents contexts in which certain sets of framework services are enabled or disabled.
    /// </summary>
    public enum FrameworkServicesContext
    {
        /// <summary>
        /// No context specified.  All services are disabled.
        /// </summary>
        None,

        /// <summary>
        /// Only <see cref="ConfigurationServices" /> are available.
        /// Should be used with <see cref="IPluginConfigurationControl" />.
        /// </summary>
        Configuration,

        /// <summary>
        /// Only <see cref="ExecutionServices" /> are available.
        /// Should be used with <see cref="IPluginExecutionEngine" />.
        /// </summary>
        Execution,

        /// <summary>
        /// Only <see cref="ConfigurationServices" /> that should be used in the constructor are available.
        /// Should be used for the constructor of <see cref="IPluginConfigurationControl" />.
        /// </summary>
        ConfigurationConstructor,

        /// <summary>
        /// Only <see cref="ExecutionServices" /> that should be used in the constructor are available.
        /// Should be used for the constructor of <see cref="IPluginExecutionEngine" />.
        /// </summary>
        ExecutionConstructor,

        /// <summary>
        /// All services are available.
        /// </summary>
        All
    }
}
