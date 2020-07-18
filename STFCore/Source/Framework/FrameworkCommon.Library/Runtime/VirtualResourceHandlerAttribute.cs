using System;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Declares a class is a Virtual Resource Controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class VirtualResourceHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the virtual resource this handler can handle.
        /// </summary>
        public VirtualResourceType ResourceType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceHandlerAttribute" /> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        public VirtualResourceHandlerAttribute(VirtualResourceType resourceType)
        {
            ResourceType = resourceType;
        }
    }
}
