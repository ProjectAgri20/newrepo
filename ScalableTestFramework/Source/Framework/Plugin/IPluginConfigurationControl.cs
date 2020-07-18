using System;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Interface for the configuration portion of a plugin.
    /// Implementing classes should inherit from System.Windows.Forms.Control.
    /// </summary>
    public interface IPluginConfigurationControl
    {
        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes this configuration control to default values.
        /// </summary>
        /// <param name="environment">Information about the plugin environment.</param>
        void Initialize(PluginEnvironment environment);

        /// <summary>
        /// Initializes this configuration control with the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configuration">The <see cref="PluginConfigurationData" /> to load.</param>
        /// <param name="environment">Information about the plugin environment.</param>
        void Initialize(PluginConfigurationData configuration, PluginEnvironment environment);

        /// <summary>
        /// Creates and returns a <see cref="PluginConfigurationData" /> instance containing the
        /// configuration data from this control.
        /// </summary>
        /// <returns>The <see cref="PluginConfigurationData" /> from the control.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method may be expensive or create a new object each time it is called.")]
        PluginConfigurationData GetConfiguration();

        /// <summary>
        /// Validates the configuration data present in this control.
        /// </summary>
        /// <returns>A <see cref="PluginValidationResult" /> indicating the result of validation.</returns>
        PluginValidationResult ValidateConfiguration();
    }
}
