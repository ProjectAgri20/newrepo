using System;

namespace HP.ScalableTest.PluginSupport.Connectivity.RadiusServer
{
    /// <summary>
    /// The radius server types
    /// </summary>
    [Flags]
    public enum RadiusServerTypes
    {
        /// <summary>
        /// Root SHA1 radius server
        /// </summary>
        RootSha1 = 1,
        /// <summary>
        /// Root SHA2 radius server
        /// </summary>
        RootSha2 = 2,
        /// <summary>
        /// Subordinate SHA2 radius server
        /// </summary>
        SubSha2 = 4
    }
}
