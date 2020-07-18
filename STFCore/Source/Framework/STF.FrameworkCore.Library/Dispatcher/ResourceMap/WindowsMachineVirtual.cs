using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Services.Protocols;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    [ObjectFactory(ManagedMachineType.Windows)]
    [ObjectFactory(ManagedMachineType.WindowsVirtual)]
    public class WindowsMachineVirtual : HostMachine
    {
        private ManagedMachine _machine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsMachineVirtual" /> class.
        /// </summary>
        public WindowsMachineVirtual(ManagedMachine machine, SystemManifest manifest)
            : base(manifest)
        {
            _machine = machine;
        }

        /// <summary>
        /// Gets the Machine Name.
        /// </summary>
        public override string Name
        {
            get { return _machine.Name; }
        }

        /// <summary>
        /// Initializes this machine by ensuring it's available and ready to boot
        /// </summary>
        public override void Validate()
        {
            // If the machine is already powered on, then find another one
            while (_machine.IsPoweredOn())
            {
                TraceFactory.Logger.Debug("Machine already powered on {0}, it will be replaced".FormatWith(_machine.Name));
                Replace();
            }
        }

        /// <summary>
        /// Sets up the machine which may involve booting, configuration, etc.
        /// </summary>
        public override void Setup()
        {
            var credential = GlobalSettings.Items.DomainAdminCredential;

            TraceFactory.Logger.Debug("Starting VIRTUAL machine" + _machine.Name);
            string dispatcher = Dns.GetHostEntry("").HostName;

            // Split out the commands by line, then remove all the comments (start with "::")
            // and join them together with the && operator to make it a single statement
            List<string> commands = Properties.Resources.WindowsClientSetup
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Remove all the comments and join the commands together with the && operator to make it a single statement
            commands.RemoveAll(n => n.StartsWith("::", StringComparison.Ordinal));
            string commandList = string.Join(" && ", commands);

            // Run the statement as a guest process on the VM
            VMController.RunGuestProcess
                (
                    _machine.Name,
                    Environment.GetEnvironmentVariable("comspec"),
                    "/c " + commandList.FormatWith(dispatcher, Manifest.SessionId),
                    credential
                );
        }

        public override void PowerOn(CancellationTokenSource cancellation)
        {

            MachineStart.Run(_machine.Name, () =>
            {
                if (cancellation.Token.IsCancellationRequested)
                {
                    return;
                }

                TraceFactory.Logger.Debug("{0}: Reverting".FormatWith(_machine.Name));
                UpdateStatus("Revert");
                _machine.Revert();

                if (cancellation.Token.IsCancellationRequested)
                {
                    return;
                }

                TraceFactory.Logger.Debug("{0}: Power On".FormatWith(_machine.Name));
                UpdateStatus("PowerOn");

                _machine.PowerOn();

                if (!VMController.WaitOnMachineAvailable(_machine.Name, cancellation.Token))
                {
                    TraceFactory.Logger.Debug("Cancellation received");
                    throw new OperationCanceledException("Cancellation received");
                }
            });
        }

        public override void Release()
        {
            _machine.ReleaseReservation();
        }

        public override void Replace()
        {
            TraceFactory.Logger.Info("Replacing {0}.".FormatWith(_machine.Name));
            var replacement = VMInventoryManager.GetReplacement(_machine.Name);
            VMInventoryManager.SetInUseClearSession(_machine.Name);
            _machine = new ManagedMachine(replacement.Name, ManagedMachineType.WindowsVirtual);
            TraceFactory.Logger.Info("New replacement machine {0} selected.".FormatWith(_machine.Name));
        }

        public override void Shutdown(ShutdownOptions options)
        {
            if (!options.PowerOff)
            {
                TraceFactory.Logger.Debug("{0} Power Off option not set, returning...".FormatWith(_machine.Name));
                return;
            }

            if (!_machine.IsPoweredOn())
            {
                TraceFactory.Logger.Debug("{0} not powered on, returning...".FormatWith(_machine.Name));
                return;
            }

            Action shutdownAction = new Action(() =>
            {
                switch (options.PowerOffOption)
                {
                    case VMPowerOffOption.PowerOff:
                        try
                        {
                            _machine.Shutdown(wait: true);
                        }
                        catch (SoapException ex)
                        {
                            TraceFactory.Logger.Warn("{0} Failed graceful shutdown.  Attempting power off: {1}".FormatWith(_machine.Name, ex.Message));
                            _machine.PowerOff();
                        }
                        break;

                    case VMPowerOffOption.RevertToSnapshot:
                        _machine.Revert(wait: true);
                        break;

                    default:
                        throw new NotSupportedException("Unsupported shutdown type: " + options.PowerOffOption);
                }
            });

            try
            {
                MachineStop.Run(_machine.Name, shutdownAction);
                TraceFactory.Logger.Debug("Shutdown completed for {0}".FormatWith(_machine.Name));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error shutting down machine, continuing...", ex);
            }
        }
    }
}