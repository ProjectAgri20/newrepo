using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    public static class VirtualResourceDetailBuilder
    {
        public static ResourceDetailBase CreateBaseDetail(VirtualResource resource)
        {
            var resourceType = EnumUtil.Parse<VirtualResourceType>(resource.ResourceType);
            DetailBuilderBase builder = CreateDetailBuilder(resourceType, null, null);
            return builder.CreateBaseDetail(resource);
        }

        internal static DetailBuilderBase CreateDetailBuilder(VirtualResourceType resourceType, SystemManifestAgent systemManifestAgent, VirtualResourcePacker packer)
        {
            string className = "{0}.{1}DetailBuilder".FormatWith(typeof(SystemManifestAgent).Namespace, resourceType);
            return Activator.CreateInstance(Type.GetType(className), systemManifestAgent, packer) as DetailBuilderBase;
        }
    }
}
