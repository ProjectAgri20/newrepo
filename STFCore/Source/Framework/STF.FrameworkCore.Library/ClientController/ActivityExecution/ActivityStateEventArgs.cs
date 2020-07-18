using System;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Contains data for a change in activity state.
    /// </summary>
    public class ActivityStateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the activity id.
        /// </summary>
        public Guid ActivityId { get; private set; }

        /// <summary>
        /// Gets the activity name.
        /// </summary>
        public string ActivityName { get; private set; }

        /// <summary>
        /// Gets the activity state.
        /// </summary>
        public ActivityState State { get; private set; }

        /// <summary>
        /// Gets a message about the activity state.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityStateEventArgs"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="state">The state.</param>
        public ActivityStateEventArgs(Guid id, string name, ActivityState state)
        {
            ActivityId = id;
            ActivityName = name;
            State = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityStateEventArgs"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="state">The state.</param>
        /// <param name="message">The message.</param>
        public ActivityStateEventArgs(Guid id, string name, ActivityState state, string message)
            : this(id, name, state)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Message = message;
            }
        }
    }
}
