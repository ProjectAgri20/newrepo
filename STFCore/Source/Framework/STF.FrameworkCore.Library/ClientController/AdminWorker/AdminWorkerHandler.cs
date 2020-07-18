using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Controller that handles setting up AdminWorkers on a VM Client.
    /// </summary>
    [VirtualResourceHandler(VirtualResourceType.AdminWorker)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = true)]
    internal class AdminWorkerHandler : OfficeWorkerHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorkerHandler"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public AdminWorkerHandler(SystemManifest manifest)
            : base(manifest)
        {            
        }

        /// <summary>
        /// Creates the AdminWorker resources.
        /// </summary>
        public override void Start()
        {
            ChangeMachineStatusMessage("Starting Admin Workers");

            OfficeWorkerCredential credential = SystemManifest.Resources.OfType<AdminWorkerDetail>().First().UserCredentials.First();
            ConfigureLocalUserGroups(credential);
            ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Starting);

            try
            {
                StartUserProcess(credential, Directory.GetCurrentDirectory());
            }
            catch
            {
                ChangeMachineStatusMessage("Error starting worker {0}".FormatWith(credential.UserName));
                ChangeResourceState(credential.ResourceInstanceId, RuntimeState.Error);
            }
        }
    }
}
