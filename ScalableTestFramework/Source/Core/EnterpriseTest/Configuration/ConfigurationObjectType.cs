namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// A class of object used in test scenario configuration.
    /// </summary>
    public enum ConfigurationObjectType
    {
        /// <summary>
        /// Configuration object is an unknown type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Configuration object type is <see cref="EnterpriseTest.EnterpriseScenario" />.
        /// </summary>
        EnterpriseScenario,

        /// <summary>
        /// Configuration object type is <see cref="EnterpriseTest.VirtualResource" />
        /// </summary>
        VirtualResource,

        /// <summary>
        /// Configuration object type is <see cref="EnterpriseTest.VirtualResourceMetadata" />
        /// </summary>
        VirtualResourceMetadata,

        /// <summary>
        /// Configuration object type is a <see cref="ConfigurationTreeFolder" /> containing scenarios.
        /// </summary>
        ScenarioFolder,

        /// <summary>
        /// Configuration object type is a <see cref="ConfigurationTreeFolder" /> containing virtual resources.
        /// </summary>
        ResourceFolder,

        /// <summary>
        /// Configuration object type is a <see cref="ConfigurationTreeFolder" /> containing virtual resource metadata.
        /// </summary>
        MetadataFolder
    }
}
