using System;
using System.Management;

namespace HP.ScalableTest.WindowsAutomation.Registry
{
    /// <summary>
    /// Event args for system registry change events.
    /// </summary>
    public sealed class RegistryChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the object whose modification triggered the event.
        /// </summary>
        public ManagementBaseObject ChangedObject { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryChangeEventArgs" /> class.
        /// </summary>
        /// <param name="changedObject">The object whose modification triggered the event.</param>
        public RegistryChangeEventArgs(ManagementBaseObject changedObject)
        {
            ChangedObject = changedObject;
        }
    }
}
