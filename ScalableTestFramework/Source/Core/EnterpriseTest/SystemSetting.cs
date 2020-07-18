using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A system setting stored in the Enterprise Test database.
    /// </summary>
    [DebuggerDisplay("{Name,nq}={Value}")]
    public sealed class SystemSetting
    {
        /// <summary>
        /// Gets or sets the setting type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the setting subtype.
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// Gets or sets the setting name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the setting value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the setting description.
        /// </summary>
        public string Description { get; set; }
    }
}
