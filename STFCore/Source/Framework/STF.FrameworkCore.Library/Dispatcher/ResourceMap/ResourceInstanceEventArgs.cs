using System;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Event args used by the <see cref="ResourceInstance"/> class for state changes
    /// </summary>
    public class ResourceInstanceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public RuntimeState State { get; set; }
    }
}