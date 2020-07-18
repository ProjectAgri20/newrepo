using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Definitions of specific error levels apply to session map elements
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// No error level defined
        /// </summary>
        None,

        /// <summary>
        /// The warning error level
        /// </summary>
        Warning,

        /// <summary>
        /// The fatal error level
        /// </summary>
        Fatal,
    }
}
