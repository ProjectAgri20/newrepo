using Vim25Api;

namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Specifies a managed object type to collect information about, as well as the properties to retrieve.
    /// </summary>
    public sealed class VSpherePropertySpec
    {
        /// <summary>
        /// Gets the underlying <see cref="Vim25Api.PropertySpec" /> this instance wraps.
        /// </summary>
        internal PropertySpec PropertySpec { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSpherePropertySpec" /> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to retrieve.</param>
        public VSpherePropertySpec(VSphereManagedObjectType managedObjectType)
            : this(managedObjectType, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSpherePropertySpec"/> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to retrieve.</param>
        /// <param name="readAllProperties">if set to <c>true</c> read all properties of this object type.</param>
        public VSpherePropertySpec(VSphereManagedObjectType managedObjectType, bool readAllProperties)
            : this(managedObjectType, readAllProperties, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSpherePropertySpec" /> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to retrieve.</param>
        /// <param name="propertyPath">The path of the property to retrieve.</param>
        public VSpherePropertySpec(VSphereManagedObjectType managedObjectType, string propertyPath)
            : this(managedObjectType, false, new[] { propertyPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSpherePropertySpec"/> class.
        /// </summary>
        /// <param name="managedObjectType">The type of vSphere managed object to retrieve.</param>
        /// <param name="propertyPaths">The paths of the properties to retrieve.</param>
        public VSpherePropertySpec(VSphereManagedObjectType managedObjectType, params string[] propertyPaths)
            : this(managedObjectType, false, propertyPaths)
        {
        }

        private VSpherePropertySpec(VSphereManagedObjectType managedObjectType, bool readAllProperties, string[] propertyPaths)
        {
            PropertySpec = new PropertySpec
            {
                type = managedObjectType.ToString(),
                all = readAllProperties,
                allSpecified = readAllProperties,
                pathSet = propertyPaths
            };
        }
    }
}
