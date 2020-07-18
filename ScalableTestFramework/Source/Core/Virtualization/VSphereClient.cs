using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Services.Protocols;
using HP.ScalableTest.WindowsAutomation.Sspi;
using Vim25Api;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Client class that connects to vSphere web services.
    /// </summary>
    public sealed class VSphereClient : IDisposable
    {
        private readonly VimService _vimService;
        private readonly ManagedObjectReference _serviceReference;
        private ServiceContent _serviceContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereClient" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <exception cref="ArgumentNullException"><paramref name="address" /> is null.</exception>
        /// <exception cref="FormatException"><paramref name="address" /> is not in a proper format.</exception>
        public VSphereClient(string address)
            : this(new Uri($"https://{address}/sdk"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereClient" /> class.
        /// </summary>
        /// <param name="serverUri">The server URI.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverUri" /> is null.</exception>
        public VSphereClient(Uri serverUri)
        {
            if (serverUri == null)
            {
                throw new ArgumentNullException(nameof(serverUri));
            }

            _serviceReference = new ManagedObjectReference
            {
                type = "ServiceInstance",
                Value = "ServiceInstance"
            };

            _vimService = new VimService
            {
                Url = serverUri.ToString(),
                Timeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds,
                CookieContainer = new CookieContainer()
            };
        }

        /// <summary>
        /// Connects to the vSphere service using the specified credentials.
        /// </summary>
        /// <param name="credential">The <see cref="NetworkCredential" /> to use to connect to vSphere.</param>
        /// <exception cref="ArgumentNullException"><paramref name="credential" /> is null.</exception>
        public void Connect(NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            PrepareToConnect();

            LogDebug($"Connecting to vSphere using user credential '{credential.UserName}'.");
            string userName = BuildUserName(credential);
            _vimService.Login(_serviceContent.sessionManager, userName, credential.Password, null);
        }

        /// <summary>
        /// Connects to the vSphere service using SSPI.
        /// </summary>
        /// <returns>The client authentication <see cref="SspiToken" />.</returns>
        public SspiToken ConnectUsingSspi()
        {
            PrepareToConnect();

            LogDebug($"Connecting to vSphere using SSPI.");
            using (ClientSecurityCredential clientCredential = new ClientSecurityCredential(SecurityPackage.Negotiate))
            {
                using (ClientSecurityContext clientContext = new ClientSecurityContext(clientCredential, null, SecurityContextAttributes.None))
                {
                    SspiToken clientToken = clientContext.Initialize();

                    try
                    {
                        // Attempt to login with the client token.  This will result in an SSPIChallenge fault the first time.
                        _vimService.LoginBySSPI(_serviceContent.sessionManager, clientToken.TokenString, null);
                    }
                    catch (SoapException ex)
                    {
                        // The SOAP exception will contain a token from the server which can be used to re-initalize the client context.
                        // After initialization, login again using the client token.
                        clientToken = clientContext.Initialize(new SspiToken(ex.Detail.InnerText));
                        _vimService.LoginBySSPI(_serviceContent.sessionManager, clientToken.TokenString, null);
                    }
                    return clientToken;
                }
            }
        }

        private void PrepareToConnect()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            _serviceContent = _vimService.RetrieveServiceContent(_serviceReference);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            if (_serviceContent != null)
            {
                _vimService.Logout(_serviceContent.sessionManager);
                _serviceContent = null;
            }
        }

        /// <summary>
        /// Retrieves a set of vSphere managed objects using the specified <see cref="VSphereSelectionSpec" /> and <see cref="VSpherePropertySpec" />.
        /// </summary>
        /// <param name="selectionSpec">The <see cref="VSphereSelectionSpec" /> that specifies which objects will be queried.</param>
        /// <param name="propertySpec">The <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A collection of <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectionSpec" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpec" /> is null.
        /// </exception>
        public IEnumerable<VSphereManagedObject> RetrieveObjects(VSphereSelectionSpec selectionSpec, VSpherePropertySpec propertySpec)
        {
            return RetrieveObjects(new VSphereManagedObject(_serviceContent.rootFolder), selectionSpec, propertySpec);
        }

        /// <summary>
        /// Retrieves a set of vSphere managed objects using the specified <see cref="VSphereSelectionSpec" /> and <see cref="VSpherePropertySpec" /> set.
        /// </summary>
        /// <param name="selectionSpec">The <see cref="VSphereSelectionSpec" /> that specifies which objects will be queried.</param>
        /// <param name="propertySpecs">The collection of <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A collection of <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectionSpec" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpecs" /> is null.
        /// </exception>
        public IEnumerable<VSphereManagedObject> RetrieveObjects(VSphereSelectionSpec selectionSpec, IEnumerable<VSpherePropertySpec> propertySpecs)
        {
            return RetrieveObjects(new VSphereManagedObject(_serviceContent.rootFolder), selectionSpec, propertySpecs);
        }

        /// <summary>
        /// Retrieves a set of vSphere managed objects using the specified <see cref="VSphereSelectionSpec" /> and <see cref="VSpherePropertySpec" />.
        /// </summary>
        /// <param name="startingObject">The <see cref="VSphereManagedObject" /> from which the query should start.</param>
        /// <param name="selectionSpec">The <see cref="VSphereSelectionSpec" /> that specifies which objects will be queried.</param>
        /// <param name="propertySpec">The <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A collection of <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="startingObject" /> is null.
        /// <para>or</para>
        /// <paramref name="selectionSpec" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpec" /> is null.
        /// </exception>
        public IEnumerable<VSphereManagedObject> RetrieveObjects(VSphereManagedObject startingObject, VSphereSelectionSpec selectionSpec, VSpherePropertySpec propertySpec)
        {
            if (propertySpec == null)
            {
                throw new ArgumentNullException(nameof(propertySpec));
            }

            return RetrieveObjects(startingObject, selectionSpec, new[] { propertySpec });
        }

        /// <summary>
        /// Retrieves a set of vSphere managed objects using the specified <see cref="VSphereSelectionSpec" /> and <see cref="VSpherePropertySpec" /> set.
        /// </summary>
        /// <param name="startingObject">The <see cref="VSphereManagedObject" /> from which the query should start.</param>
        /// <param name="selectionSpec">The <see cref="VSphereSelectionSpec" /> that specifies which objects will be queried.</param>
        /// <param name="propertySpecs">The collection of <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A collection of <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="startingObject" /> is null.
        /// <para>or</para>
        /// <paramref name="selectionSpec" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpecs" /> is null.
        /// </exception>
        public IEnumerable<VSphereManagedObject> RetrieveObjects(VSphereManagedObject startingObject, VSphereSelectionSpec selectionSpec, IEnumerable<VSpherePropertySpec> propertySpecs)
        {
            if (selectionSpec == null)
            {
                throw new ArgumentNullException(nameof(selectionSpec));
            }

            return RetrieveObjects(startingObject, new[] { selectionSpec }, propertySpecs);
        }

        /// <summary>
        /// Retrieves a set of vSphere managed objects using the specified <see cref="VSphereSelectionSpec" /> set and <see cref="VSpherePropertySpec" /> set.
        /// </summary>
        /// <param name="startingObject">The <see cref="VSphereManagedObject" /> from which the query should start.</param>
        /// <param name="selectionSpecs">The collection of <see cref="VSphereSelectionSpec" /> that specifies which objects will be queried.</param>
        /// <param name="propertySpecs">The collection of <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A collection of <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="startingObject" /> is null.
        /// <para>or</para>
        /// <paramref name="selectionSpecs" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpecs" /> is null.
        /// </exception>
        public IEnumerable<VSphereManagedObject> RetrieveObjects(VSphereManagedObject startingObject, IEnumerable<VSphereSelectionSpec> selectionSpecs, IEnumerable<VSpherePropertySpec> propertySpecs)
        {
            if (startingObject == null)
            {
                throw new ArgumentNullException(nameof(startingObject));
            }

            if (selectionSpecs == null)
            {
                throw new ArgumentNullException(nameof(selectionSpecs));
            }

            if (propertySpecs == null)
            {
                throw new ArgumentNullException(nameof(propertySpecs));
            }

            ObjectSpec objectSpec = new ObjectSpec
            {
                obj = startingObject.ManagedObjectReference,
                selectSet = selectionSpecs.Select(n => n.SelectionSpec).ToArray()
            };

            PropertyFilterSpec propertyFilterSpec = new PropertyFilterSpec
            {
                objectSet = new[] { objectSpec },
                propSet = propertySpecs.Select(n => n.PropertySpec).ToArray()
            };

            var result = _vimService.RetrieveProperties(_serviceContent.propertyCollector, new[] { propertyFilterSpec });
            return result.Select(n => new VSphereManagedObject(n));
        }

        /// <summary>
        /// Retrieves a vSphere managed object with updated property values using the specified <see cref="VSpherePropertySpec" />.
        /// </summary>
        /// <param name="managedObject">The <see cref="VSphereManagedObject" /> to retrieve.</param>
        /// <param name="propertySpec">The <see cref="VSpherePropertySpec" /> that specifies which object properties will be retrieved.</param>
        /// <returns>A <see cref="VSphereManagedObject" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="managedObject" /> is null.
        /// <para>or</para>
        /// <paramref name="propertySpec" /> is null.
        /// </exception>
        public VSphereManagedObject RetrieveObject(VSphereManagedObject managedObject, VSpherePropertySpec propertySpec)
        {
            if (managedObject == null)
            {
                throw new ArgumentNullException(nameof(managedObject));
            }

            if (propertySpec == null)
            {
                throw new ArgumentNullException(nameof(propertySpec));
            }

            ObjectSpec objectSpec = new ObjectSpec
            {
                obj = managedObject.ManagedObjectReference
            };

            PropertyFilterSpec propertyFilterSpec = new PropertyFilterSpec
            {
                objectSet = new[] { objectSpec },
                propSet = new[] { propertySpec.PropertySpec }
            };

            var result = _vimService.RetrieveProperties(_serviceContent.propertyCollector, new[] { propertyFilterSpec });
            return new VSphereManagedObject(result.Single());
        }

        /// <summary>
        /// Powers on the specified virtual machine.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <returns>A <see cref="VSphereManagedObject" /> representing the task created by the power on operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="vmObject" /> is null.</exception>
        public VSphereManagedObject PowerOnVirtualMachine(VSphereManagedObject vmObject)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            ManagedObjectReference task = _vimService.PowerOnVM_Task(vmObject.ManagedObjectReference, null);
            return new VSphereManagedObject(task);
        }

        /// <summary>
        /// Powers off the specified virtual machine.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <returns>A <see cref="VSphereManagedObject" /> representing the task created by the power on operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="vmObject" /> is null.</exception>
        public VSphereManagedObject PowerOffVirtualMachine(VSphereManagedObject vmObject)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            ManagedObjectReference task = _vimService.PowerOffVM_Task(vmObject.ManagedObjectReference);
            return new VSphereManagedObject(task);
        }

        /// <summary>
        /// Shuts down the specified virtual machine.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vmObject" /> is null.</exception>
        public void ShutdownVirtualMachine(VSphereManagedObject vmObject)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            _vimService.ShutdownGuest(vmObject.ManagedObjectReference);
        }

        /// <summary>
        /// Reboots the specified virtual machine.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vmObject" /> is null.</exception>
        public void RebootVirtualMachine(VSphereManagedObject vmObject)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            _vimService.RebootGuest(vmObject.ManagedObjectReference);
        }

        /// <summary>
        /// Reverts the specified virtual machine to the most recent snapshot.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <returns>A <see cref="VSphereManagedObject" /> representing the task created by the power on operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="vmObject" /> is null.</exception>
        public VSphereManagedObject RevertVirtualMachineToSnapshot(VSphereManagedObject vmObject)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            ManagedObjectReference task = _vimService.RevertToCurrentSnapshot_Task(vmObject.ManagedObjectReference, null, true, true);
            return new VSphereManagedObject(task);
        }

        /// <summary>
        /// Runs a guest process in the specified virtual machine.
        /// </summary>
        /// <param name="vmObject">The <see cref="VSphereManagedObject" /> representing the virtual machine.</param>
        /// <param name="fileName">The file name of the application to run.</param>
        /// <param name="arguments">The command-line arguments to pass to the application when the process starts.</param>
        /// <param name="credential">The credential to run the process as.</param>
        /// <param name="waitForExit">if set to <c>true</c> wait for the process to exit before returning.</param>
        /// <returns>The ID of the guest process that was started.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="vmObject" /> is null.
        /// <para>or</para>
        /// <paramref name="credential" /> is null.
        /// </exception>
        public long RunVirtualMachineGuestProcess(VSphereManagedObject vmObject, string fileName, string arguments, NetworkCredential credential, bool waitForExit)
        {
            if (vmObject == null)
            {
                throw new ArgumentNullException(nameof(vmObject));
            }

            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            VSpherePropertySpec spec = new VSpherePropertySpec(VSphereManagedObjectType.GuestOperationsManager, "processManager");
            VSphereManagedObject guestOperationsManager = RetrieveObject(new VSphereManagedObject(_serviceContent.guestOperationsManager), spec);
            var processManager = (ManagedObjectReference)guestOperationsManager.Properties["processManager"];

            NamePasswordAuthentication auth = new NamePasswordAuthentication
            {
                username = BuildUserName(credential),
                password = credential.Password,
                interactiveSession = true
            };
            GuestProgramSpec guestProgram = new GuestProgramSpec
            {
                programPath = fileName,
                arguments = arguments
            };

            long processId = _vimService.StartProgramInGuest(processManager, vmObject.ManagedObjectReference, auth, guestProgram);

            if (waitForExit)
            {
                while (true)
                {
                    var processInfo = _vimService.ListProcessesInGuest(processManager, vmObject.ManagedObjectReference, auth, new[] { processId }).FirstOrDefault();
                    if (processInfo?.exitCodeSpecified == true)
                    {
                        break;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            return processId;
        }

        /// <summary>
        /// Waits for the task represented by the specified <see cref="VSphereManagedObject" /> to reach a completed state.
        /// </summary>
        /// <param name="taskObject">The <see cref="VSphereManagedObject" /> representing the task.</param>
        /// <param name="timeout">The length of time to wait for the task to complete.</param>
        /// <returns>A <see cref="VSphereTaskResult" /> representing the result of the task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="taskObject" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout" /> is zero or negative.</exception>
        public VSphereTaskResult WaitForTaskCompletion(VSphereManagedObject taskObject, TimeSpan timeout)
        {
            if (taskObject == null)
            {
                throw new ArgumentNullException(nameof(taskObject));
            }

            if (timeout <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be greater than zero.");
            }

            VSpherePropertySpec getTaskState = new VSpherePropertySpec(VSphereManagedObjectType.Task, "info.state", "info.result", "info.error");

            VSphereManagedObject task = RetrieveObject(taskObject, getTaskState);
            for (int i = 0; i < (int)timeout.TotalSeconds; i++)
            {
                TaskInfoState taskState = (TaskInfoState)task.Properties["info.state"];
                if (taskState == TaskInfoState.success || taskState == TaskInfoState.error)
                {
                    return new VSphereTaskResult(task);
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    task = RetrieveObject(taskObject, getTaskState);
                }
            }
            return new VSphereTaskResult(task);
        }

        private static string BuildUserName(NetworkCredential credential)
        {
            return string.IsNullOrEmpty(credential.Domain) ? credential.UserName : $"{credential.Domain}\\{credential.UserName}";
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _serviceContent = null;
            _vimService.Dispose();
        }

        #endregion
    }
}
