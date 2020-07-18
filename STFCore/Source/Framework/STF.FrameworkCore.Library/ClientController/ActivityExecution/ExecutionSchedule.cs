using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Class that defines a schedule item for executing activities in a scheduled
    /// <see cref="ExecutionMode"/> setting.
    /// </summary>
    [Serializable]
    public class ExecutionSchedule
    {
        private readonly Collection<ExecutionScheduleSegment> _items = new Collection<ExecutionScheduleSegment>();
        private readonly Random _randomVariance = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionSchedule"/> class.
        /// </summary>
        public ExecutionSchedule()
        {
            RepeatCount = 1;
		}

        /// <summary>
        /// Gets or sets a value indicating whether to run the schedule for duration.
        /// </summary>
        /// <value>
        ///   <c>true</c> if run for duration; otherwise, <c>false</c>, which will run for a repeat count.
        /// </value>
        public bool UseDuration { get; set; }

        /// <summary>
        /// Gets or sets the repeat count.
        /// Note: This property should more correctly be named "IterationCount".  That is how it is being treated elsewhere in the code.
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Gets or sets the overall schedule duration in minutes.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets the schedule duration in minutes based on the cumulative time and repeat iterations of the segments.
        /// </summary>
        public int CumulativeDuration
        {
            get
            {
                TimeSpan total = new TimeSpan();
                foreach (ExecutionScheduleSegment segment in _items)
                {
                    total += segment.Duration;
                }
                return Convert.ToInt32(total.TotalMinutes) * RepeatCount;
            }
        }

        /// <summary>
        /// Gets or sets the base time used for DateTime calculation.
        /// </summary>
        [XmlIgnore]
        public DateTime BaseTime { get; set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public Collection<ExecutionScheduleSegment> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Returns the cumulative time based on the given base time and duration iteration
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public DateTime CumulativeTime(int iteration)
        {
            var timeSpan = TimeSpan.Zero;

            if (iteration >= 0)
            {
                for (int i = 0; i <= iteration; i++)
                {
                    timeSpan = timeSpan.Add(_items[i % _items.Count].Duration);
                }
            }

            return BaseTime.Add(timeSpan);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(ExecutionScheduleSegment item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Gets the schedule section defined by the iteration.
        /// </summary>
        /// <param name="location">The iteration.</param>
        /// <returns></returns>
        public ExecutionScheduleSegment GetSegment(int location)
        {
            return _items[location % _items.Count];
        }

        /// <summary>
        /// Gets the count of schedule sections.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int SegmentCount
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets the execution state for the given iteration.
        /// </summary>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public WorkerExecutionState GetState(int iteration)
        {
            return EnumUtil.Parse<WorkerExecutionState>(GetSegment(iteration).Mode);
        }

        /// <summary>
        /// Gets the total iterations based on the <see cref="RepeatCount"/> and total schedule sections.
        /// </summary>
        /// <value>
        /// The total iterations.
        /// </value>
        public int TotalIterations
        {
            get { return RepeatCount * _items.Count; }
        }

        /// <summary>
        /// Gets the binding list.
        /// </summary>
        [XmlIgnore]
        public SortableBindingList<ExecutionScheduleSegment> BindingList
        {
            get
            {
                SortableBindingList<ExecutionScheduleSegment> items = new SortableBindingList<ExecutionScheduleSegment>();
                foreach (var item in _items)
                {
                    items.Add(item);
                }

                return items;
            }
        }

        /// <summary>
        /// Calculates the first end time when the activity set is first starting.
        /// </summary>
        /// <returns></returns>
        public DateTime CalculateInitialEndTime()
        {
            var endTime = BaseTime;

            // Determine if there should be an initial stagger by looking at the stagger setting for the
            // very last entry.  Since execution of the schedule can be repeated, using the duration of the
            // last segment provides a way to calculate what the initial variance should be.
            var lastSegment = GetSegment(SegmentCount - 1);

            // If there is a stagger for the last segment, then calculate a variance based on the duration of
            // the last segment and add it to the base time.  This will provide an offset to be used for
            // the initial startup of the schedule.  If no stagger is required, then all clients will start
            // at the BaseTime value, which will create a immediate increase in load.
            if (lastSegment.Stagger)
            {
                endTime = BaseTime.Add(Variance(lastSegment.Duration));
            }

            TraceFactory.Logger.Debug("First End Time: {0}".FormatWith(endTime));
            return endTime;
        }

        /// <summary>
        /// Calculates the segment end time for the segment defined by the index value.
        /// </summary>
        /// <param name="endTime">The end time.</param>
        /// <param name="index">The iteration.</param>
        /// <returns></returns>
        public Tuple<DateTime, WorkerExecutionState> CalculateNextEndTime(DateTime endTime, int index)
        {
            if (index == int.MinValue)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            // Get the segment for this iteration
            var segment = GetSegment(index);
            var state = GetState(index);

            // Find the boundary between sections for the current section and the prior section.  Then determine what the offset
            // is in terms of where the endTime is located with respect to this boundary.  It could be before (-) or after it (+)
            // Then subtract this offset from the current sections duration.  If the endTime was before the boundary (-), then
            // the subtraction will actually increase the duration length for this segment.  If it is after (+), then this
            // segment's duration will actually be a bit shorter.
            var segmentBoundary = CumulativeTime(--index);
            var endTimeOffset = endTime.Subtract(segmentBoundary);
            var adjustedDuration = segment.Duration.Subtract(endTimeOffset);

            if (segment.Stagger)
            {
                var nextDuration = GetSegment(++index).Duration;
                endTime = CalculateEndTime(endTime, adjustedDuration, nextDuration);
                TraceFactory.Logger.Debug("Stagger, Next End Time: {0}".FormatWith(endTime));
            }
            else
            {
                endTime = endTime + adjustedDuration;
                TraceFactory.Logger.Debug("No Stagger, Next End Time: {0}".FormatWith(endTime));
            }

            return new Tuple<DateTime, WorkerExecutionState>(endTime, state);
        }

        /// <summary>
        /// Calculates subsequent end times based on the surrounding duration values
        /// </summary>
        /// <param name="currentEndTime">The current end time.</param>
        /// <param name="adjustedDuration">Duration of the adjusted.</param>
        /// <param name="nextDuration">Duration of the next.</param>
        /// <returns></returns>
        private DateTime CalculateEndTime(DateTime currentEndTime, TimeSpan adjustedDuration, TimeSpan nextDuration)
        {
            TraceFactory.Logger.Debug("ET: {0}, AD: {1}, ND: {2}"
                .FormatWith(currentEndTime, adjustedDuration, nextDuration));

            // Pick a simple way to "randomly" decide if the variance will be positive or negative.
            bool isPositive = currentEndTime.Ticks % 2 == 0;

            // Calculate the variance for the current duration, which has been adjusted to account for the end time
            // of the last segment. This asjusted duration may be larger or smaller than its default value.
            var variance = Variance(adjustedDuration);
            TraceFactory.Logger.Debug("Variance {0}".FormatWith(variance));

            // This variance can't extend beyond 1/2 of the next duration, if it is positive.
            if (isPositive)
            {
                TimeSpan halfNextDuration = TimeSpanUtil.Divide(nextDuration, 2);
                if (variance > halfNextDuration)
                {
                    // Keep the variance to some value less than 1/2 of this shorter duration
                    variance = TimeSpanUtil.GetRandom(halfNextDuration);
                    TraceFactory.Logger.Debug("Variance reduced {0}".FormatWith(variance));
                }
            }

            // Return the new end time based on the current end time, plus the current duration,
            // then +/- the variance.
            var newDuration = currentEndTime.Add(adjustedDuration);
            return isPositive ? newDuration.Add(variance) : newDuration.Subtract(variance);
        }

        private TimeSpan Variance(TimeSpan duration)
        {
            // Determine a variance maximum value by taking 12.5% of the duration's current size. The 12.5% value is
            // one half of an overall 25% value (12.5% on each side of the transition boundary).  Then use that
            // maximum value to calculate a new random value between zero and the maximum value.
            TimeSpan varianceMax = TimeSpanUtil.Divide(duration, 8);
            return TimeSpanUtil.GetRandom(varianceMax);
        }
    }

    /// <summary>
    /// Item containing information on a scheduled execution for a worker
    /// </summary>
    public class ExecutionScheduleSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionScheduleSegment"/> class.
        /// </summary>
        public ExecutionScheduleSegment()
        {
            Mode = string.Empty;
            Days = 0;
            Hours = 0;
            Minutes = 0;
        }

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> duration for the defined values.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration
        {
            get { return new TimeSpan(Days, Hours, Minutes, 0); }
        }

        /// <summary>
        /// Gets or sets the state of the current worker.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the days to be in this state.
        /// </summary>
        /// <value>
        /// The days.
        /// </value>
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the hours to be in this state.
        /// </summary>
        /// <value>
        /// The hours.
        /// </value>
        public int Hours { get; set; }

        /// <summary>
        /// Gets or sets the minutes to be in this state.
        /// </summary>
        /// <value>
        /// The minutes.
        /// </value>
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stagger the transition.
        /// </summary>
        /// <value>
        ///   <c>true</c> if staggering the transition; otherwise, <c>false</c>.
        /// </value>
        public bool Stagger { get; set; }
    }

    /// <summary>
    /// Defines what state the worker is currently in when running under
    /// a scheduled <see cref="ExecutionMode"/>
    /// </summary>
    public enum WorkerExecutionState
    {
        /// <summary>
        /// The worker is currently active and executing activities
        /// </summary>
        Active,

        /// <summary>
        /// The worker is idle and not executing any activities
        /// </summary>
        Idle
    }
}