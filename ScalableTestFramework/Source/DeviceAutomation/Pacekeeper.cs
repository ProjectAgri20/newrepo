using System;
using System.Diagnostics;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Provides synchronization capability to control the speed of execution.
    /// </summary>
    /// <remarks>
    /// The goal of this class is to provide a method for slowing down automation that happens quickly
    /// without introducing additional wait time for automation that is already slow enough.
    /// This is accomplished by tracking a "pace" time, which is the smallest amount of time that an operation
    /// should consume.  If the actual operation finishes early, it waits until the pacekeeper "catches up."
    /// If the operation exceeds the pace time, the pacekeeper allows it to continue immediately.
    /// </remarks>
    [DebuggerDisplay("Pace = {Pace}")]
    public sealed class Pacekeeper
    {
        private TimeSpan _pace = TimeSpan.Zero;
        private DateTime _syncPoint = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the pace (the minimum length of time between sync points).
        /// Setting this value will not modify the existing sync point.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value" /> is less than <see cref="TimeSpan.Zero" />.</exception>
        public TimeSpan Pace
        {
            get
            {
                return _pace;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("value", "Pace cannot be less than zero.");
                }
                _pace = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pacekeeper" /> class and sets the first sync point.
        /// </summary>
        /// <param name="pace">The pace, i.e. the minimum length of time between sync points.</param>
        public Pacekeeper(TimeSpan pace)
            : this(pace, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pacekeeper" /> class, optionally setting the first sync point.
        /// </summary>
        /// <param name="pace">The pace, i.e. the minimum length of time between sync points.</param>
        /// <param name="startImmediately">if set to <c>true</c> set the first sync point immediately.</param>
        public Pacekeeper(TimeSpan pace, bool startImmediately)
        {
            Pace = pace;
            if (startImmediately)
            {
                Reset();
            }
        }

        /// <summary>
        /// Immediately resets the sync point to a future time defined by <see cref="Pace" />.
        /// </summary>
        public void Reset()
        {
            if (Pace > TimeSpan.Zero)
            {
                _syncPoint = DateTime.UtcNow + Pace;
            }
            else
            {
                _syncPoint = DateTime.MinValue;
            }
        }

        /// <summary>
        /// Waits for the next sync point to occur (if it has not already passed),
        /// then resets the sync point to a future time defined by <see cref="Pace" />.
        /// </summary>
        public void Sync()
        {
            TimeSpan timeLeftUntilSync = _syncPoint - DateTime.UtcNow;
            if (timeLeftUntilSync > TimeSpan.Zero)
            {
                Thread.Sleep(timeLeftUntilSync);
            }
            Reset();
        }

        /// <summary>
        /// Immediately pauses for one full synchronization cycle (i.e. the value of <see cref="Pace" />),
        /// then resets the sync point to a future time defined by <see cref="Pace" />.
        /// </summary>
        public void Pause()
        {
            if (Pace > TimeSpan.Zero)
            {
                Thread.Sleep(Pace);
            }
            Reset();
        }
    }
}
