
using System.ComponentModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// The running mode for this instance.
    /// </summary>
    public enum ExecutionMode
    {
        /// <summary>
        /// Worker executes its activity set a specified number of times.
        /// </summary>
        Iteration,

        /// <summary>
        /// Worker executes its activity set for a specified duration of time.
        /// </summary>
        Duration,

        /// <summary>
        /// Worker executes its activity set through a scheduled sequence of Active and Idle periods
        /// </summary>
        Scheduled,

        /// <summary>
        /// Worker executes a specified number of activities within a specified duration
        /// </summary>
        RateBased,

        /// <summary>
        /// Worker executes a set of activities according to a Poisson distribution
        /// </summary>
        Poisson,

        /// <summary>
        /// Worker executes a set of activities at a given cadence
        /// </summary>
        SetPaced,
    }
}
