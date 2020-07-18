using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher
{
    internal abstract class DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailBuilderBase"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="packer">The packer.</param>
        /// <param name="type">The type.</param>
        public DetailBuilderBase(SystemManifestAgent config, VirtualResourcePacker packer, VirtualResourceType type)
        {
            ManifestAgent = config;
            ResourcePacker = packer;
            ResourceType = type;
        }

        /// <summary>
        /// Gets the manifest builder.
        /// </summary>
        internal SystemManifestAgent ManifestAgent { get; private set; }

        /// <summary>
        /// Gets the resource packer.
        /// </summary>
        internal VirtualResourcePacker ResourcePacker { get; private set; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        internal VirtualResourceType ResourceType { get; private set; }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal abstract void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest);

        public abstract ResourceDetailBase CreateBaseDetail(VirtualResource resource);
    }
}