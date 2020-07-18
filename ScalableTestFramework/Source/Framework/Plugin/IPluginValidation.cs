namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Interface for validation of plugin data.
    /// </summary>
    public interface IPluginValidation
    {
        /// <summary>
        /// Validates the specified <see cref="PluginConfigurationData" /> to ensure it
        /// is valid and compatible with this plugin.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" />.</param>
        /// <returns><c>true</c> if the plugin data is valid for this plugin; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
        bool ValidateMetadata(ref PluginConfigurationData configurationData);
    }
}
