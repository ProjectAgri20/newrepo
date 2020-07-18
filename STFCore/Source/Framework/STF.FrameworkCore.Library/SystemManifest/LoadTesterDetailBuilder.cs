using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on the Load Tester for the <see cref="SystemManifest"/>
    /// </summary>
    internal class LoadTesterDetailBuilder : DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadTesterDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public LoadTesterDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.LoadTester)
        {
        }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            // There is one Load Tester per VM, so there should only be one
            // resource embedded in this manifest.
            var resource = resources.First();

            // First determine if the resource already exists in the Resource collection.  If not, then
            // have VirtualResource create it, then update it with other data and add it to the list.
            var detail = manifest.Resources.GetResource<LoadTesterDetail>(resource.VirtualResourceId);
            if (detail == null)
            {
                detail = CreateDetail(resource);
                detail.CommandPortOffset = 40000;

                manifest.Resources.Add(detail);
            }

            // Use the system settings Administrator credentials for the AdminWorker
            var userCredential = new OfficeWorkerCredential();
            userCredential.UserName = GlobalSettings.Items[Setting.DomainAdminUserName];
            userCredential.Password = GlobalSettings.Items[Setting.DomainAdminPassword];
            userCredential.Domain = GlobalSettings.Items[Setting.Domain];
            userCredential.Port = detail.CommandPortOffset;
            userCredential.ResourceInstanceId = SystemManifestAgent.CreateUniqueId(userCredential.UserName);

            detail.UserCredentials.Add(userCredential);
        }

        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        private LoadTesterDetail CreateDetail(VirtualResource resource)
        {
            LoadTester tester = resource as LoadTester;

            LoadTesterDetail detail = new LoadTesterDetail
            {
                ResourceId = tester.VirtualResourceId,
                ResourceType = EnumUtil.Parse<VirtualResourceType>(tester.ResourceType),
                Name = tester.Name,
                Description = tester.Description,
                InstanceCount = tester.InstanceCount,
                Platform = tester.Platform,
                Enabled = tester.Enabled,
                ThreadsPerVM = tester.ThreadsPerVM
            };

            CreateMetadataDetail(resource, detail);
            return detail;
        }

        private void CreateMetadataDetail(VirtualResource resource, ResourceDetailBase detail)
        {
            foreach (var data in resource.VirtualResourceMetadataSet.Where(m => m.Enabled))
            {
                LoadTesterMetadataDetail metadata = new LoadTesterMetadataDetail()
                {
                    MetadataType = data.MetadataType,
                    Data = data.Metadata,

                    Plan = data.ExecutionPlan != null
                        ? LegacySerializer.DeserializeDataContract<LoadTesterExecutionPlan>(data.ExecutionPlan)
                        : new LoadTesterExecutionPlan(),

                    Id = data.VirtualResourceMetadataId,
                    Name = data.Name,
                    MetadataVersion = data.MetadataVersion,
                    Enabled = data.Enabled,
                };
                detail.MetadataDetails.Add(metadata);
            }
        }
    }
}