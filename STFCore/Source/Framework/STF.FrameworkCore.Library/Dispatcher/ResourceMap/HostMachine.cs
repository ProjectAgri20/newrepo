using System;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Base class that represents a machine used as a host for virtual resources
    /// in a test scenario.  The machine may be virtual, physical or possibly other
    /// in the future.
    /// </summary>
    public class HostMachine
    {
        /// <summary>
        /// Gets the manifest associated with this host.
        /// </summary>
        /// <value>The system manifest.</value>
        public SystemManifest Manifest { get; private set; }

        /// <summary>
        /// Occurs when the status changes for this host.
        /// </summary>
        public event EventHandler<HostMachineEventArgs> OnStatusChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostMachine"/> class.
        /// </summary>
        public HostMachine(SystemManifest manifest)
        {
            Manifest = manifest;
            Configured = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        protected void UpdateStatus(RuntimeState state, string message)
        {
            OnStatusChanged?.Invoke(this, new HostMachineEventArgs(state, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected void UpdateStatus(string message)
        {
            UpdateStatus(RuntimeState.None, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        protected void UpdateState(RuntimeState state)
        {
            UpdateStatus(state, string.Empty);
        }

        /// <summary>
        /// Gets the name of this host.
        /// </summary>
        public virtual string Name
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this machine is configured.
        /// </summary>
        /// <value>
        ///   <c>true</c> if configured; otherwise, <c>false</c>.
        /// </value>
        public bool Configured { get; set; }

        /// <summary>
        /// Powers on this host instance.
        /// </summary>
        /// <param name="cancellation">The cancellation.</param>
        public virtual void PowerOn(CancellationTokenSource cancellation)
        {
        }

        /// <summary>
        /// Sets up the machine which may involve booting, configuration, etc.
        /// </summary>
        public virtual void Setup()
        {
        }

        /// <summary>
        /// Replaces this host instance with another instance.
        /// </summary>
        public virtual void Replace()
        {
        }

        /// <summary>
        /// Validates this host instance.
        /// </summary>
        public virtual void Validate()
        {
        }

        /// <summary>
        /// Shuts down this host instance.
        /// </summary>
        /// <param name="options">The options.</param>
        public virtual void Shutdown(ShutdownOptions options)
        {
        }

        /// <summary>
        /// Releases this host instance.
        /// </summary>
        public virtual void Release()
        {
        }
    }
}
