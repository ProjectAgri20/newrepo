using System;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides extension methods and utilities for working with <see cref="TimeSpan" />.
    /// </summary>
    public static class TimeSpanUtil
    {
        private static Random _random = new Random();

        /// <summary>
        /// Returns a random <see cref="TimeSpan" /> that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The upper bound of the random <see cref="TimeSpan" /> to be generated.</param>
        /// <returns>A random <see cref="TimeSpan" /> that is less than the specified maximum.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue" /> is less than <see cref="TimeSpan.Zero" />.</exception>
        public static TimeSpan GetRandom(TimeSpan maxValue)
        {
            if (maxValue < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "Max value cannot be negative.");
            }

            long ticks = (long)(maxValue.Ticks * _random.NextDouble());
            return TimeSpan.FromTicks(ticks);
        }

        /// <summary>
        /// Returns a random <see cref="TimeSpan" /> that is within a specified range.
        /// </summary>
        /// <param name="minValue">The lower bound of the random <see cref="TimeSpan" /> to be generated.</param>
        /// <param name="maxValue">The upper bound of the random <see cref="TimeSpan" /> to be generated.</param>
        /// <returns>A random <see cref="TimeSpan" /> that is between the specified minimum and maximum.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue" /> is greater than <paramref name="maxValue" />.</exception>
        public static TimeSpan GetRandom(TimeSpan minValue, TimeSpan maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue), "Min value cannot be greater than max value.");
            }

            TimeSpan range = maxValue - minValue;
            return minValue + GetRandom(range);
        }

        /// <summary>
        /// Returns the larger of two <see cref="TimeSpan" /> structs.
        /// </summary>
        /// <param name="time1">The first <see cref="TimeSpan" /> to compare.</param>
        /// <param name="time2">The second <see cref="TimeSpan" /> to compare.</param>
        /// <returns>Parameter <paramref name="time1" /> or <paramref name="time2" />, whichever is larger.</returns>
        public static TimeSpan Max(TimeSpan time1, TimeSpan time2)
        {
            return (time1 >= time2) ? time1 : time2;
        }

        /// <summary>
        /// Returns the smaller of two <see cref="TimeSpan" /> structs.
        /// </summary>
        /// <param name="time1">The first <see cref="TimeSpan" /> to compare.</param>
        /// <param name="time2">The second <see cref="TimeSpan" /> to compare.</param>
        /// <returns>Parameter <paramref name="time1" /> or <paramref name="time2" />, whichever is smaller.</returns>
        public static TimeSpan Min(TimeSpan time1, TimeSpan time2)
        {
            return (time1 <= time2) ? time1 : time2;
        }

        /// <summary>
        /// Multiplies a <see cref="TimeSpan" /> by an integer factor.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan" /> multiplicand.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>A <see cref="TimeSpan" /> representing <paramref name="time" /> multiplied by the specified factor.</returns>
        public static TimeSpan Multiply(TimeSpan time, int multiplier)
        {
            return TimeSpan.FromTicks(time.Ticks * multiplier);
        }

        /// <summary>
        /// Divides a <see cref="TimeSpan" /> by an integer factor.
        /// </summary>
        /// <param name="time">The <see cref="TimeSpan" /> dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>A <see cref="TimeSpan" /> representing <paramref name="time" /> divided by the specified factor.</returns>
        public static TimeSpan Divide(TimeSpan time, int divisor)
        {
            return TimeSpan.FromTicks(time.Ticks / divisor);
        }
    }
}
