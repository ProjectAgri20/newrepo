namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// A set of <see cref="ConfigurationObjectTag" /> objects that have been changed as the result of an operation.
    /// </summary>
    public sealed class ConfigurationObjectChangeSet
    {
        /// <summary>
        /// Gets a <see cref="ConfigurationObjectTagCollection" /> representing objects that have been added.
        /// </summary>
        public ConfigurationObjectTagCollection AddedObjects { get; } = new ConfigurationObjectTagCollection();

        /// <summary>
        /// Gets a <see cref="ConfigurationObjectTagCollection" /> representing objects that have been modified.
        /// </summary>
        public ConfigurationObjectTagCollection ModifiedObjects { get; } = new ConfigurationObjectTagCollection();

        /// <summary>
        /// Gets a <see cref="ConfigurationObjectTagCollection" /> representing objects that have been removed.
        /// </summary>
        public ConfigurationObjectTagCollection RemovedObjects { get; } = new ConfigurationObjectTagCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectChangeSet" /> class.
        /// </summary>
        public ConfigurationObjectChangeSet()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
