using System;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Event args containing data about a resource state.
    /// </summary>
    public class ResourceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public RuntimeState State { get; set; }

        /// <summary>
        /// Gets or sets the resource ID.
        /// </summary>
        /// <value>The resource ID.</value>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy logs.
        /// </summary>
        /// <value><c>true</c> if copying logs; otherwise, <c>false</c>.</value>
        public bool CopyLogs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEventArgs"/> class.
        /// </summary>
        /// <param name="resourceId">The name.</param>
        /// <param name="state">The state.</param>
        public ResourceEventArgs(string resourceId, RuntimeState state)
        {
            ResourceId = resourceId;
            State = state;
            CopyLogs = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceEventArgs"/> class.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="state">The state.</param>
        /// <param name="copyLogs">if set to <c>true</c> if copying logs.</param>
        public ResourceEventArgs(string resourceId, RuntimeState state, bool copyLogs)
            : this(resourceId, state)
        {
            CopyLogs = copyLogs;
        }
    }
}
