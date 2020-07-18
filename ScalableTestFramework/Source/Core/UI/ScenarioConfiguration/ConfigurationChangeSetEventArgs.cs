using System;
using HP.ScalableTest.Core.EnterpriseTest.Configuration;

namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    /// <summary>
    /// <see cref="EventArgs" /> containing a <see cref="ConfigurationObjectChangeSet" />.
    /// </summary>
    public sealed class ConfigurationChangeSetEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="ConfigurationObjectChangeSet" />.
        /// </summary>
        public ConfigurationObjectChangeSet ChangeSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationChangeSetEventArgs" /> class.
        /// </summary>
        /// <param name="changeSet">The <see cref="ConfigurationObjectChangeSet" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="changeSet" /> is null.</exception>
        public ConfigurationChangeSetEventArgs(ConfigurationObjectChangeSet changeSet)
        {
            ChangeSet = changeSet ?? throw new ArgumentNullException(nameof(changeSet));
        }
    }
}
