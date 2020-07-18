using System;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Interface for Activity Execution Plan
    /// </summary>
    public interface IActivityExecutionPlan
    {
        /// <summary>
        /// Gets or sets the execution order for the activity.
        /// </summary>
        /// <value>The execution order.</value>
        int Order { get; set; }

        /// <summary>
        /// Gets or sets the number of times the activity will execution.
        /// </summary>
        /// <value>The execution count.</value>
        int Value { get; set; }

        /// <summary>
        /// Gets or sets the execution mode.
        /// </summary>
        /// <value>The execution mode.</value>
        ExecutionMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the phase.
        /// </summary>
        /// <value>The phase.</value>
        ResourceExecutionPhase Phase { get; set; }

        /// <summary>
        /// Gets or sets the pacing.
        /// </summary>
        /// <value>The pacing.</value>
        ActivitySpecificPacing ActivityPacing { get; set; }
    }
}
