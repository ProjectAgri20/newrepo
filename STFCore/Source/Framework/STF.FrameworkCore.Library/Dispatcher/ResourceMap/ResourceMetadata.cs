using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines the metadata that will be used by the <see cref="ResourceName"/>.
    /// </summary>
    public class ResourceMetadata : ISessionMapElement
    {
        /// <summary>
        /// Gets the detail information from the <see cref="SystemManifest"/>.
        /// </summary>
        public ResourceMetadataDetail Detail { get; private set; }

        /// <summary>
        /// Gets the <see cref="SessionMapElement"/> for this item.
        /// </summary>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the resource name associated with this metadata.
        /// </summary>
        public string ResourceName { get; private set; }

        public void Validate(ParallelLoopState loopState, string resourceName, SystemManifest manifest, string machineName)
        {
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceProfile" /> class.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public ResourceMetadata(string resourceName, ResourceMetadataDetail detail)
        {
            ResourceName = resourceName;
            Detail = detail;
            //MapElement = new SessionMapElement("{0}:{1}".FormatWith(resourceName, detail.Name), ElementType.ResourceMetadata, detail.MetadataType);
            MapElement = new SessionMapElement(detail.Name, ElementType.Activity, detail.MetadataType);
        }
    }
}
