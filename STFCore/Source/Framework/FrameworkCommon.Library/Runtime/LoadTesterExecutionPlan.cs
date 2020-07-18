using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Activity Execution plan for Load Tester
    /// </summary>
    [DataContract]
    public class LoadTesterExecutionPlan : IActivityExecutionPlan
    {
        /// <summary>
        /// Gets or sets the mode for ramp up, time based or rate based
        /// </summary>
        /// <remarks>
        /// Time based will randomly start each thread within a defined time range.
        /// Rate based will start a designated number of threads at each interval
        /// defined by the pacing delay.
        /// </remarks>
        [DataMember]
        public RampUpMode RampUpMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Collection<RampUpSetting> RampUpSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the time based ramp up
        /// </summary>
        /// <value>
        /// <c>true</c> if each thread will start at a randomly selected time with a time range; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomizeRampUpDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum ramp up delay if using a time based <see cref="RampUpMode"/>.
        /// </summary>
        [DataMember]
        public int MinRampUpDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum ramp up delay if using a time based <see cref="RampUpMode"/>.
        /// </summary>
        [DataMember]
        public int MaxRampUpDelay { get; set; }

        /// <summary>
        /// Gets or sets the number of threads per ramp up segment to start.
        /// </summary>
        /// <remarks>
        /// This is based on the ramp up pace (threads/time).
        /// </remarks>
        [DataMember]
        public int RampUpPacingThreads { get; set; }

        /// <summary>
        /// Gets or sets the delay between ramp up pacing for the threads.
        /// </summary>
        /// <remarks>
        /// This is based on the ramp up pace (threads/time).
        /// </remarks>
        [DataMember]
        public TimeSpan RampUpPacingDelay { get; set; }


        /// <summary>
        /// Gets or sets the total thread count for this execution plan.
        /// </summary>
        /// <value>
        /// The total thread count.
        /// </value>
        [DataMember]
        public int ThreadCount { get; set; }

        /// <summary>
        /// Gets or sets the total amount of time (in minutes) the threads in this plan will execute.
        /// </summary>
        /// <value>
        /// The duration time.
        /// </value>
        [DataMember]
        public int DurationTime { get; set; }

        /// <summary>
        /// Gets or sets the number of times the activity will repeat for each thread in this plan.
        /// </summary>
        /// <value>
        /// The activity repeat count.
        /// </value>
        [DataMember]
        public int RepeatCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the delay after each activity execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if delay between activity execution should be randomized; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RandomizeActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum activity delay if randomized delay is enabled.
        /// </summary>
        /// <value>
        /// The maximum activity delay.
        /// </value>
        [DataMember]
        public int MaxActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum activity delay if randomized, or is the default value if not.
        /// </summary>
        /// <value>
        /// The minimum activity delay.
        /// </value>
        [DataMember]
        public int MinActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to delay one interval before starting threads.
        /// </summary>
        /// <value>
        ///   <c>true</c> if one interval should pass before starting threads; otherwise, <c>false</c> which will start
        ///   the first set of threads immediately.
        /// </value>
        [DataMember]
        public bool DelayOneInterval { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadTesterExecutionPlan"/> class.
        /// </summary>
        public LoadTesterExecutionPlan()
        {
            RampUpMode = RampUpMode.RateBased;
            RandomizeRampUpDelay = false;
            RampUpPacingThreads = 1;
            RampUpPacingDelay = TimeSpan.Zero;
            Mode = ExecutionMode.RateBased;
            ThreadCount = 1;
            DurationTime = 0;
            RepeatCount = 1;
            RandomizeActivityDelay = false;
            DelayOneInterval = false;
            RampUpSettings = new Collection<RampUpSetting>();
        }

        #region IExecutionPlan

        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        /// <value>The execution order.</value>
        public int Order
        {
            // Note that for load testing there is no order as all plugins
            // are running at the same time in their own thread space.
            // So there is not need to set the value either.
            get { return 1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        /// <value>The execution count.</value>
        public int Value
        {
            // Note that for the load tester there is already a thread
            // count that defines how many instances will be running,
            // so there is no need ever for a value greater than 1.
            get { return 1; }
            set { }
        }

        /// <summary>
        /// Gets or sets the execution mode which defines how the thread will execute activities.
        /// </summary>
        [DataMember]
        public ExecutionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the phase.
        /// </summary>
        /// <value>The phase.</value>
        public ResourceExecutionPhase Phase
        {
            get { return ResourceExecutionPhase.Main; }
            set { }
        }

        /// <summary>
        /// Gets the Activity Pacing
        /// </summary>
        public ActivitySpecificPacing ActivityPacing
        {
            get { return new ActivitySpecificPacing(); }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<PluginRetrySetting> ExceptionDefinitions
        { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class RampUpSetting
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ThreadCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TimeSpan Delay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        public RampUpSetting(TimeSpan delay)
        {
            ThreadCount = 0;
            Delay = delay;
        }
    }
}