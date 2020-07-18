using System;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides methods for inserting specified or random delays during execution.
    /// </summary>
    public static class Delay
    {
        /// <summary>
        /// Pauses execution for the specified amount of time.
        /// </summary>
        /// <param name="delay">The amount of time to pause.</param>
        public static void Wait(TimeSpan delay)
        {
            if (delay > TimeSpan.Zero)
            {
                //LogTrace($"Delaying for {delay}.");
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Pauses execution for a random amount of time in the given range.
        /// </summary>
        /// <param name="minDelay">The minimum amount of time to pause.</param>
        /// <param name="maxDelay">The maximum amount of time to pause.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minDelay" /> is greater than <paramref name="maxDelay" />.</exception>
        public static void Wait(TimeSpan minDelay, TimeSpan maxDelay)
        {
            if (maxDelay < minDelay)
            {
                throw new ArgumentOutOfRangeException(nameof(minDelay), "Min value cannot be greater than max value.");
            }

            TimeSpan delay = TimeSpanUtil.GetRandom(minDelay, maxDelay);
            Wait(delay);
        }
    }
}
