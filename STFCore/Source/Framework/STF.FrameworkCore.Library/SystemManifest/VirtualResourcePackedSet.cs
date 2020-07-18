using System;
using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    public class VirtualResourcePackedSet : Collection<VirtualResource>
    {
        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        public VirtualResourceType ResourceType { get; private set; }

        /// <summary>
        /// Gets the max resources.
        /// </summary>
        public int MaxResources { get; private set; }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        public string Platform { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is fully packed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fully packed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullyPacked
        {
            get { return this.Count == this.MaxResources; }
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourcePackedSet" /> class.
        /// </summary>
        /// <param name="resourceType">Type of resource.</param>
        /// <param name="platform">The platform where the resource(s) will run.</param>
        /// <param name="maxResources">The max resources for this platform.</param>
        public VirtualResourcePackedSet(VirtualResource resource, string platform)
        {
            int maxResources = (int)resource.ResourcesPerVM;
            VirtualResourceType resourceType = EnumUtil.Parse<VirtualResourceType>(resource.ResourceType);

            Platform = platform;
            MaxResources = maxResources;
            ResourceType = resourceType;
            Id = SequentialGuid.NewGuid();
        }
    }
}
