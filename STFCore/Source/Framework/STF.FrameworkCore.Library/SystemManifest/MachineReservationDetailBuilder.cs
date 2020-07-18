using System;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on Machine Reservations for the <see cref="SystemManifest"/>
    /// </summary>
    internal class MachineReservationDetailBuilder : DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MachineReservationDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public MachineReservationDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.MachineReservation)
        {
        }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            var resource = resources.First();

            MachineReservationDetail detail = manifest.Resources.GetResource<MachineReservationDetail>(resource.VirtualResourceId);
            if (detail == null)
            {
                detail = CreateDetail(resource);
                manifest.Resources.Add(detail);
            }

            // Add installers to the manifest that are specific to this instance of the Machine Reservation
            MachineReservationMetadata metadata = LegacySerializer.DeserializeXml<MachineReservationMetadata>(resource.VirtualResourceMetadataSet.First().Metadata);
            if (metadata.PackageId != Guid.Empty)
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    int i = 1;
                    foreach (var installer in SelectSoftwareInstallers(context, metadata.PackageId))
                    {
                        TraceFactory.Logger.Debug("Adding {0}".FormatWith(installer.Description));
                        manifest.SoftwareInstallers.Add(CreateSoftwareInstallerDetail(installer, i++));
                    }
                }
            }
        }

        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        private MachineReservationDetail CreateDetail(VirtualResource resource)
        {
            return new MachineReservationDetail
            {
                ResourceId = resource.VirtualResourceId,
                ResourceType = EnumUtil.Parse<VirtualResourceType>(resource.ResourceType),
                Name = resource.Name,
                Description = resource.Description,
                InstanceCount = resource.InstanceCount,
                Platform = resource.Platform,
                Enabled = resource.Enabled
            };
        }

        private static IQueryable<SoftwareInstaller> SelectSoftwareInstallers(EnterpriseTestEntities entities, Guid packageId)
        {
            return from p in entities.SoftwareInstallerPackages
                   where p.PackageId == packageId
                   from s in p.SoftwareInstallerSettings
                   orderby s.InstallOrderNumber
                   select s.SoftwareInstaller;
        }

        private static SoftwareInstallerDetail CreateSoftwareInstallerDetail(SoftwareInstaller installer, int orderNumber)
        {
            return new SoftwareInstallerDetail()
            {
                InstallerId = installer.InstallerId,
                Path = installer.FilePath,
                Arguments = installer.Arguments,
                RebootMode = EnumUtil.Parse<RebootMode>(installer.RebootSetting),
                CopyDirectory = installer.CopyDirectory,
                Description = installer.Description,
                InstallOrderNumber = orderNumber,
            };
        }
    }
}