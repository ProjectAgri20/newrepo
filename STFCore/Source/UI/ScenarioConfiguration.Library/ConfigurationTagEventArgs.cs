using System;
using HP.ScalableTest.Data.EnterpriseTest;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Event args that include a <see cref="ConfigurationObjectTag"/> instance.
    /// </summary>
    public class ConfigurationTagEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the tag.
        /// </summary>
        public ConfigurationObjectTag Tag { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationTagEventArgs"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public ConfigurationTagEventArgs(ConfigurationObjectTag tag)
        {
            Tag = tag;
        }
    }
}
