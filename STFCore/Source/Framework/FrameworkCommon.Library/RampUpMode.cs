using System;
using System.Linq;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Defines way that activities will be ramped up.
    /// </summary>
    public enum RampUpMode
    {
        /// <summary>
        /// Indicates there is no ramp up and each thread starts immediately
        /// </summary>
        None,

        /// <summary>
        /// Start items on a fixed or random time frame
        /// </summary>
        TimeBased,

        /// <summary>
        /// Start items based on a ramp up rate
        /// </summary>
        RateBased,
    }
}
