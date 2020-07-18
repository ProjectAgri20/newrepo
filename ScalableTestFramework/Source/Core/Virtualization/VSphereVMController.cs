using System;
using System.Collections.ObjectModel;
using System.Net;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Controller for virtual machine operations on a vSphere server.
    /// </summary>
    public sealed class VSphereVMController : IDisposable
    {
        private readonly Lazy<VSphereClient> _vSphereClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereVMController" /> class.
        /// </summary>
        /// <param name="address">The address of the vSphere server.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> to use to connect to vSphere.</param>
        /// <exception cref="ArgumentNullException"><paramref name="credential" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "False positive.")]
        public VSphereVMController(string address, NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            _vSphereClient = new Lazy<VSphereClient>(() =>
            {
                VSphereClient client = new VSphereClient(address);
                client.Connect(credential);
                return client;
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereVMController" /> class.
        /// </summary>
        /// <param name="serverUri">The server URI for the vSphere server.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> to use to connect to vSphere.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serverUri" /> is null.
        /// <para>or</para>
        /// <paramref name="credential" /> is null.
        /// </exception>
        public VSphereVMController(Uri serverUri, NetworkCredential credential)
        {
            if (serverUri == null)
            {
                throw new ArgumentNullException(nameof(serverUri));
            }

            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            _vSphereClient = new Lazy<VSphereClient>(() =>
            {
                VSphereClient client = new VSphereClient(serverUri);
                client.Connect(credential);
                return client;
            });
        }

        /// <summary>
        /// Gets a collection of all virtual machines available on the vSphere server.
        /// </summary>
        /// <returns>A collection of <see cref="VSphereVirtualMachine" /> objects.</returns>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        public Collection<VSphereVirtualMachine> GetVirtualMachines()
        {
            LogDebug("Retrieving list of virtual machines from vSphere.");

            VSphereSelectionSpec selectAllVMs =
                new VSphereSelectionSpec("recurseFolders", VSphereManagedObjectType.Folder, "childEntity",
                    new VSphereSelectionSpec("recurseFolders"),
                    new VSphereSelectionSpec(VSphereManagedObjectType.Datacenter, "vmFolder",
                        new VSphereSelectionSpec("recurseFolders"))
            );

            Collection<VSphereVirtualMachine> vms = new Collection<VSphereVirtualMachine>();
            foreach (VSphereManagedObject vm in CallVSphere(n => n.RetrieveObjects(selectAllVMs, VSphereVirtualMachine.StandardProperties)))
            {
                vms.Add(new VSphereVirtualMachine(vm));
            }
            return vms;
        }

        /// <summary>
        /// Updates the status of the specified <see cref="VSphereVirtualMachine" />.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void UpdateStatus(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            VSphereManagedObject result = CallVSphere(n => n.RetrieveObject(virtualMachine.ManagedObject, VSphereVirtualMachine.StandardProperties));
            virtualMachine.UpdateStatus(result);
        }

        /// <summary>
        /// Powers on the specified virtual machine.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to power on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void PowerOn(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Powering on virtual machine {virtualMachine.HostName}.");

            VSphereManagedObject task = CallVSphere(n => n.PowerOnVirtualMachine(virtualMachine.ManagedObject));
            WaitForTaskSuccess(task);
            UpdateStatus(virtualMachine);
        }

        /// <summary>
        /// Powers off the specified virtual machine.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to power off.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void PowerOff(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Powering off virtual machine {virtualMachine.HostName}.");

            VSphereManagedObject task = CallVSphere(n => n.PowerOffVirtualMachine(virtualMachine.ManagedObject));
            WaitForTaskSuccess(task);
            UpdateStatus(virtualMachine);
        }

        /// <summary>
        /// Shuts down the specified virtual machine.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to shut down.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void Shutdown(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Shutting down virtual machine {virtualMachine.HostName}.");

            CallVSphere(n => n.ShutdownVirtualMachine(virtualMachine.ManagedObject));
            UpdateStatus(virtualMachine);
        }

        /// <summary>
        /// Reboots the specified virtual machine.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to reboot.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void Reboot(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Rebooting virtual machine {virtualMachine.HostName}.");

            CallVSphere(n => n.RebootVirtualMachine(virtualMachine.ManagedObject));
            UpdateStatus(virtualMachine);
        }

        /// <summary>
        /// Reverts the specified virtual machine to the most recent snapshot.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to revert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="virtualMachine" /> is null.</exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public void RevertToSnapshot(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Reverting virtual machine {virtualMachine.HostName} to latest snapshot.");

            VSphereManagedObject task = CallVSphere(n => n.RevertVirtualMachineToSnapshot(virtualMachine.ManagedObject));
            WaitForTaskSuccess(task);
            UpdateStatus(virtualMachine);
        }

        /// <summary>
        /// Runs a guest process in the specified virtual machine.
        /// </summary>
        /// <param name="virtualMachine">The <see cref="VSphereVirtualMachine" /> to run the guest process.</param>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="credential">The <see cref="NetworkCredential" /> to run the process as.</param>
        /// <param name="waitForExit">if set to <c>true</c> wait for the process to exit before returning.</param>
        /// <returns>The ID of the guest process that was started.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="virtualMachine" /> is null.
        /// <para>or</para>
        /// <paramref name="credential" /> is null.
        /// </exception>
        /// <exception cref="VirtualMachineOperationException">The vSphere service threw an error while processing the command.</exception>
        public long RunGuestProcess(VSphereVirtualMachine virtualMachine, string fileName, string arguments, NetworkCredential credential, bool waitForExit)
        {
            if (virtualMachine == null)
            {
                throw new ArgumentNullException(nameof(virtualMachine));
            }

            LogInfo($"Executing guest process {fileName} on virtual machine {virtualMachine.HostName}.");

            return CallVSphere(n => n.RunVirtualMachineGuestProcess(virtualMachine.ManagedObject, fileName, arguments, credential, waitForExit));
        }

        private void WaitForTaskSuccess(VSphereManagedObject taskObject)
        {
            VSphereTaskResult taskResult = CallVSphere(n => n.WaitForTaskCompletion(taskObject, TimeSpan.FromMinutes(1)));
            if (!taskResult.Success)
            {
                throw new VirtualMachineOperationException(taskResult.Error ?? "The VSphere operation did not complete successfully.");
            }
        }

        private void CallVSphere(Action<VSphereClient> action)
        {
            bool wrapper(VSphereClient client)
            {
                action(client);
                return true;
            }
            CallVSphere(wrapper);
        }

        private T CallVSphere<T>(Func<VSphereClient, T> action)
        {
            try
            {
                return action(_vSphereClient.Value);
            }
            catch (Exception ex)
            {
                throw new VirtualMachineOperationException(ex.Message, ex);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_vSphereClient.IsValueCreated)
            {
                _vSphereClient.Value.Dispose();
            }
        }

        #endregion
    }
}
