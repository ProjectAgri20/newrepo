namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// Represents the enabled/disabled state of a configuration object.
    /// </summary>
    public enum EnabledState
    {
        /// <summary>
        /// Configuration object is a type that cannot be enabled or disabled.
        /// </summary>
        NotApplicable,

        /// <summary>
        /// Configuration object is enabled.
        /// </summary>
        Enabled,

        /// <summary>
        /// Configuration object is disabled.
        /// </summary>
        Disabled
    }
}
