using System;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Core.EnterpriseTest.Configuration;

namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    /// <summary>
    /// <see cref="EventArgs" /> containing a <see cref="ConfigurationObjectTag" /> and an
    /// <see cref="EnterpriseTestContext" /> that can be used to modify it.
    /// </summary>
    public sealed class ConfigurationObjectEditEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="ConfigurationObjectTag" /> for the object being edited.
        /// </summary>
        public ConfigurationObjectTag ConfigurationObject { get; }

        /// <summary>
        /// Gets the <see cref="EnterpriseTestContext" /> to use for loading the object and tracking changes.
        /// </summary>
        public EnterpriseTestContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectEditEventArgs" /> class.
        /// </summary>
        /// <param name="configurationObject">The <see cref="ConfigurationObjectTag" /> for the object being edited.</param>
        /// <param name="context">The <see cref="EnterpriseTestContext" /> to use for loading the object and tracking changes.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configurationObject" /> is null.
        /// <para>or</para>
        /// <paramref name="context" /> is null.
        /// </exception>
        public ConfigurationObjectEditEventArgs(ConfigurationObjectTag configurationObject, EnterpriseTestContext context)
        {
            ConfigurationObject = configurationObject ?? throw new ArgumentNullException(nameof(configurationObject));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
