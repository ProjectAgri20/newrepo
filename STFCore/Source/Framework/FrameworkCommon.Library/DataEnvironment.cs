using System;
using System.Linq;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Defines the environment of STF
    /// </summary>
    public enum DataEnvironment
    {
        /// <summary>
        /// Represents the Beta environment
        /// </summary>
        Beta,

        /// <summary>
        /// Represents the Development environment
        /// </summary>
        Development,

        /// <summary>
        /// Represents the production environment
        /// </summary>
        Production,

        /// <summary>
        /// Indicates an unassigned state for the environment
        /// </summary>
        Unassigned,
    }
}
