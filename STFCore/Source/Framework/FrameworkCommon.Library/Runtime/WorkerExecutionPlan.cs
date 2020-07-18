using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class WorkerExecutionPlan : IActivityExecutionPlan
    {
        private Collection<PluginRetrySetting> _exceptionDefinitions = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerExecutionPlan"/> class.
        /// </summary>
        public WorkerExecutionPlan()
        {
            Order = 1;
            Value = 1;
            Mode = ExecutionMode.Iteration;
            Phase = ResourceExecutionPhase.Main;
            ActivityPacing = new ActivitySpecificPacing();
            _exceptionDefinitions = new Collection<PluginRetrySetting>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (ActivityPacing == null)
            {
                ActivityPacing = new ActivitySpecificPacing();
            }
        }

        /// <summary>
        /// Gets or sets the execution order for the activity.
        /// </summary>
        /// <value>The execution order.</value>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the number of times the activity will execution.
        /// </summary>
        /// <value>The execution count.</value>
        [DataMember]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the execution mode.
        /// </summary>
        /// <value>The execution mode.</value>
        [DataMember]
        public ExecutionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the phase.
        /// </summary>
        /// <value>The phase.</value>
        [DataMember]
        public ResourceExecutionPhase Phase { get; set; }
        /// <summary>
        /// Gets or Sets the Activity Pacing
        /// </summary>
        [DataMember]
        public ActivitySpecificPacing ActivityPacing { get; set; }
    }
}
