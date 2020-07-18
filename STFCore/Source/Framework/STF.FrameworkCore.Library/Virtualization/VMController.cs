using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Virtualization;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Service that manages VM usage for multiple dispatchers.
    /// To add a VM host environment to the system, add the VMHostLogin information to the App.config.
    /// </summary>
    public static class VMController
    {
        private static readonly object ResourceLock = new object();

        private static VSphereVMController GetVSphereController()
        {
            string serverUri = GlobalSettings.Items[Setting.VMWareServerUri];
            return new VSphereVMController
            (
                new Uri(serverUri), UserManager.CurrentUser.ToNetworkCredential()
            );
        }

        /// <summary>
        /// Waits the on machine to be available.
        /// </summary>
        /// <param name="hostName">The hostname.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static bool WaitOnMachineAvailable(string hostName, CancellationToken token)
        {
            return WaitOnMachineState(hostName, token, new Collection<VirtualComponentStatus>() { VirtualComponentStatus.Green, VirtualComponentStatus.Yellow });
        }

        /// <summary>
        /// Waits until the VM is in a defined state.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="token">The token.</param>
        /// <param name="statusValues">The status values.</param>
        /// <param name="pollingFrequencySeconds">The default pause between checks in seconds.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">statusValues</exception>
        private static bool WaitOnMachineState(string hostName, CancellationToken token, Collection<VirtualComponentStatus> statusValues, int pollingFrequencySeconds = 5)
        {
            int retries = 0;
            int maxRetries = 20;
            if (statusValues == null)
            {
                throw new ArgumentNullException("statusValues");
            }

            TraceFactory.Logger.Debug("Waiting for {0} status to be {1}...".FormatWith(hostName, string.Join(", ", statusValues)));
            VirtualComponentStatus status;

            do
            {
                status = GetStatus(hostName);
                if (statusValues.Contains(status))
                {
                    break;
                }
                else
                {
                    if (retries > maxRetries)
                    {
                        TraceFactory.Logger.Debug("trying to execute command prompt to check status, retry attempt: {0}".FormatWith(retries));
                        if (RunGuestProcess(hostName, Environment.GetEnvironmentVariable("comspec"), "/c Timeout 3", GlobalSettings.Items.DomainAdminCredential))
                        {
                            break;
                        }

                    }
                    TraceFactory.Logger.Debug("{0} not ready, sleeping {1} before retry".FormatWith(hostName, pollingFrequencySeconds));
                    retries++;
                    Thread.Sleep(TimeSpan.FromSeconds(pollingFrequencySeconds));
                }
            } while (!token.IsCancellationRequested);

            TraceFactory.Logger.Debug("{0} status now set to {1}".FormatWith(hostName, status));

            return !token.IsCancellationRequested;
        }

        /// <summary>
        /// Powers on the specified VM.
        /// </summary>
        /// <param name="machineName">The VM Name to power on.</param>
        /// <exception cref="VMPowerOnException">Unable to power on  + hostname</exception>
        public static void PowerOnMachine(string machineName)
        {
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, machineName);
                if (machine.PowerState != VirtualMachinePowerState.PoweredOn)
                {
                    // The VM is not powered on, so go ahead and power it on.
                    PowerOn(vSphereController, machine);
                }
                else
                {
                    throw new VirtualMachineOperationException(machineName + " was already powered on.");
                }
            }
        }

        /// <summary>
        /// Checks to see if a VM is powered on.
        /// </summary>
        /// <param name="machineName">Name of the VM.</param>
        public static bool IsPoweredOn(string machineName)
        {
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, machineName);
                return machine.PowerState == VirtualMachinePowerState.PoweredOn;
            }
        }

        private static void WaitUntilPoweredOff(VSphereVMController vSphereController, VSphereVirtualMachine machine)
        {
            while (machine.PowerState != VirtualMachinePowerState.PoweredOff)
            {
                TraceFactory.Logger.Debug("{0} still powered on...".FormatWith(machine.HostName));
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Need to update the status, otherwise the property will never update.
                vSphereController.UpdateStatus(machine);
            }
        }

        /// <summary>
        /// Powers on the VM
        /// </summary>
        /// <param name="machine">The vm.</param>
        /// <param name="setInUse">Sets the status to "In Use"</param>
        /// <returns></returns>
        private static void PowerOn(VSphereVMController vSphereController, VSphereVirtualMachine machine)
        {
            TraceFactory.Logger.Debug("Attempting power on: {0}".FormatWith(machine.HostName));
            Retry.WhileThrowing<VirtualMachineOperationException>
            (
                () => vSphereController.PowerOn(machine),
                10,
                TimeSpan.FromSeconds(1)
            );
            TraceFactory.Logger.Debug("Power on succeeded: {0}".FormatWith(machine.HostName));
        }

        /// <summary>
        /// Performs a PowerOff on the specified VM.
        /// </summary>
        /// <param name="hostName">The VM host name.</param>
        public static void PowerOff(string hostName)
        {
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, hostName);

                Retry.WhileThrowing<VirtualMachineOperationException>
                (
                    () => vSphereController.PowerOff(machine),
                    10,
                    TimeSpan.FromSeconds(2)
                );
            }
        }

        /// <summary>
        /// Performs a guest shutdown on the specified VM.
        /// </summary>
        /// <param name="hostName">The VM host name.</param>
        /// <param name="wait">if set to <c>true</c> do not return until powered off.</param>
        public static void Shutdown(string hostName, bool wait = false)
        {
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, hostName);
                Shutdown(vSphereController, machine);

                if (wait)
                {
                    WaitUntilPoweredOff(vSphereController, machine);
                }
            }
        }

        private static void Shutdown(VSphereVMController vSphereController, VSphereVirtualMachine machine)
        {
            TraceFactory.Logger.Debug("Attempting to shutdown: " + machine.HostName);
            Retry.WhileThrowing<VirtualMachineOperationException>
            (
                () => vSphereController.Shutdown(machine),
                10,
                TimeSpan.FromSeconds(2)
            );
        }

        /// <summary>
        /// Reverts the specified VM back to it's most recent snapshot state.
        /// </summary>
        /// <param name="hostName">The VM host name.</param>
        /// <param name="wait">Wait for the operation to complete</param>
        public static void Revert(string hostName, bool wait = false)
        {
            TraceFactory.Logger.Debug("Attempting to revert to snapshot: " + hostName);
            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, hostName);
                Retry.WhileThrowing<VirtualMachineOperationException>
                (
                    () => vSphereController.RevertToSnapshot(machine),
                    10,
                    TimeSpan.FromSeconds(1)
                );

                if (wait)
                {
                    WaitUntilPoweredOff(vSphereController, machine);
                }
            }
        }

        /// <summary>
        /// Gets the current status of the specified host.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        private static VirtualComponentStatus GetStatus(string hostName)
        {
            using (var vSphereController = GetVSphereController())
            {
                return GetVirtualMachine(vSphereController, hostName).Status;
            }
        }

        /// <summary>
        /// Runs the guest process.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="command">The command.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="waitForExit">if set to <c>true</c> [wait for exit].</param>
        public static bool RunGuestProcess(string hostName, string command, string arguments, NetworkCredential credential, bool waitForExit = false)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            using (var vSphereController = GetVSphereController())
            {
                var machine = GetVirtualMachine(vSphereController, hostName);
                var pid = vSphereController.RunGuestProcess(machine, command, arguments, credential, waitForExit);
                return pid > 0;
            }
        }

        private static VSphereVirtualMachine GetVirtualMachine(VSphereVMController vSphereController, string hostName)
        {
            return vSphereController.GetVirtualMachines().First(n => n.HostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
        }
    }
}