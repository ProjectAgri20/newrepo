using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    [ObjectFactory(ManagedMachineType.WindowsDesktop)]
    public class WindowsMachineDesktop : HostMachine
    {
        private VirtualClientControllerLite _controller = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsMachineDesktop" /> class.
        /// </summary>
        /// <param name="manifest">The system manifest used for this host.</param>
        public WindowsMachineDesktop(ManagedMachine machine, SystemManifest manifest)
            : base(manifest)
        {
        }

        public override string Name
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        /// Initializes this machine by ensuring it's available and ready to boot
        /// </summary>
        public override void Validate()
        {
            // Since this is running on the local machine, there really isn't anything
            // to do to validate that the machine is ready to run, since this code
            // is already running on the machine.
        }

        /// <summary>
        /// Sets up the machine which may involve booting, configuration, etc.
        /// </summary>
        public override void Setup()
        {
            _controller = new VirtualClientControllerLite();
            Task.Factory.StartNew(() => _controller.Start(Manifest.SessionId));
        }

        public override void Shutdown(ShutdownOptions options)
        {
            _controller.Stop();
        }
    }
}