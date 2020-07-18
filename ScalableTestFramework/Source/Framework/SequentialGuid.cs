using System;
using System.Linq;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Generates sequential globally unique identifiers (GUIDs).
    /// </summary>
    public static class SequentialGuid
    {
        private const int _numberOfBytes = 6;
        private const int _numberOfBits = _numberOfBytes * 8;
        private static readonly long _maximumPermutations = (long)Math.Pow(2, _numberOfBits) - 1;

        private static DateTime _startDate;
        private static DateTime _endDate;
        private static TimeSpan _period;
        private static long _lastSequence = -1;

        private static readonly object _syncObject = new object();

        /// <summary>
        /// Creates a new sequential <see cref="Guid" />.
        /// </summary>
        /// <returns>A new sequential <see cref="Guid" />.</returns>
        public static Guid NewGuid()
        {
            DateTime now = DateTime.UtcNow;
            if (now > _endDate)
            {
                Initialize(now);
            }

            long sequence = GetSequence(now);
            return BuildGuid(sequence);
        }

        private static void Initialize(DateTime now)
        {
            // Initialize dates to a 100 year period aligned by century
            int century = now.Year / 100;
            _startDate = new DateTime(century * 100, 1, 1);
            _endDate = _startDate.AddYears(100);
            _period = _endDate - _startDate;
            _lastSequence = -1;
        }

        private static long GetSequence(DateTime now)
        {
            long ticksUntilNow = now.Ticks - _startDate.Ticks;
            double percentile = (double)ticksUntilNow / _period.Ticks;
            long sequence = (long)(percentile * _maximumPermutations);

            lock (_syncObject)
            {
                if (sequence <= _lastSequence)
                {
                    // Prevent double sequence on same machine
                    sequence = _lastSequence + 1;
                }
                _lastSequence = sequence;
            }

            return sequence;
        }

        private static Guid BuildGuid(long sequence)
        {
            var sequenceBytes = BitConverter.GetBytes(sequence).Take(_numberOfBytes).Reverse();
            var guidBytes = Guid.NewGuid().ToByteArray().Take(10);
            byte[] totalBytes = guidBytes.Concat(sequenceBytes).ToArray();

            return new Guid(totalBytes);
        }
    }
}
