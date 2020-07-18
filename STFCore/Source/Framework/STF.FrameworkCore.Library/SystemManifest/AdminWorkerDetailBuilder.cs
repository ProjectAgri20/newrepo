using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on Admin Workers for the <see cref="SystemManifest"/>
    /// </summary>
    internal class AdminWorkerDetailBuilder : OfficeWorkerDetailBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorkerDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public AdminWorkerDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.AdminWorker)
        {
        }

        /// <summary>
        /// Creates the specified resources.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            // Iterate through each resource and create the office worker process
            foreach (VirtualResource resource in resources)
            {
                // First determine if the resource already exists in the Resource collection.  If not, then
                // have VirtualResource create it, then update it with other data and add it to the list.
                var detail = manifest.Resources.GetResource<AdminWorkerDetail>(resource.VirtualResourceId);
                if (detail == null)
                {
                    detail = (AdminWorkerDetail)CreateDetail(resource);
                    detail.CommandPortOffset = 40000;

                    manifest.Resources.Add(detail);
                }

                // Use the system settings Administrator credentials for the AdminWorker
                var userCredential = new OfficeWorkerCredential();
                userCredential.UserName = GlobalSettings.Items[Setting.DomainAdminUserName];
                userCredential.Password = GlobalSettings.Items[Setting.DomainAdminPassword];
                userCredential.Domain = GlobalSettings.Items[Setting.Domain];
                userCredential.Port = detail.CommandPortOffset;
                // This only works with just the username because there is only one
                // allowed per VM.  If this ever changes then there will need to be a
                // unique suffix added.
                //userCredential.InstanceId = SystemManifestAgent.CreateUniqueId(userCredential.UserName);
                userCredential.ResourceInstanceId = userCredential.UserName;

                detail.UserCredentials.Add(userCredential);
            }
        }

        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        public override OfficeWorkerDetail CreateDetail(VirtualResource resource)
        {
            AdminWorker worker = resource as AdminWorker;

            AdminWorkerDetail detail = new AdminWorkerDetail
            {
                ResourceId = worker.VirtualResourceId,
                ResourceType = EnumUtil.Parse<VirtualResourceType>(worker.ResourceType),
                Name = worker.Name,
                Description = worker.Description,
                InstanceCount = worker.InstanceCount,
                Platform = worker.Platform,
                Enabled = worker.Enabled,
                RandomizeActivities = false,
                RandomizeStartupDelay = false,
                MinStartupDelay = 0,
                MaxStartupDelay = 0,
                RandomizeActivityDelay = false,
                MinActivityDelay = 0,
                MaxActivityDelay = 0,
                RepeatCount = 1,
                ExecutionMode = worker.ExecutionMode
            };

            CreateMetadataDetail(worker, detail);
            return detail;
        }
    }
}