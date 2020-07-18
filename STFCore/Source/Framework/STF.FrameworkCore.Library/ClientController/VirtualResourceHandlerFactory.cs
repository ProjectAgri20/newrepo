using System;
using System.Linq;
using System.Text;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Reflection;
using System.Collections.ObjectModel;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Factory class that instantiates <see cref="IVirtualResourceHandler"/> instances.
    /// </summary>
    public static class VirtualResourceHandlerFactory
    {
        /// <summary>
        /// Creates an instance of the <see cref="IVirtualResourceHandler" /> based on the type
        /// listed in the manifest.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <returns></returns>
        public static Collection<VirtualResourceHandler> Create(SystemManifest manifest)
        {
            if (manifest == null)
            {
                throw new ArgumentNullException("manifest");
            }

            // This query looks through the current assembly and gets those types with the controller
            // attribute attached, then gets the type that has the attribute value of the resource            
            // begin created and returns that type.
            var handlerTypeQuery =
                (
                    // Note that this code only looks through the currently loaded assembly where
                    // this factory class is defined. This does assume that all handlers are 
                    // present in this class, which is currently the case.  It also helps narrow
                    // the search and speed this up.  The old code is below that went through all 
                    // assemblies.  The problem here is that a Reflection Load exception will happen
                    // for some assemblies because of missing dependencies.
                    //from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                    //from t in a.GetTypes().AsParallel()
                    from t in Assembly.GetAssembly(typeof(VirtualResourceHandlerFactory)).GetTypes()
                    let attributes = t.GetCustomAttributes(typeof(VirtualResourceHandlerAttribute), false)
                    where attributes != null && attributes.Length > 0
                    from attr in attributes.Cast<VirtualResourceHandlerAttribute>()
                    where attr.ResourceType == manifest.ResourceType
                    select new { Type = t }
                ).ToList();


            if (manifest.CollectEventLogs)
            {
                var temp = (
                from t in Assembly.GetAssembly(typeof(VirtualResourceHandlerFactory)).GetTypes()
                let attributes = t.GetCustomAttributes(typeof(VirtualResourceHandlerAttribute), false)
                where attributes != null && attributes.Length > 0
                from attr in attributes.Cast<VirtualResourceHandlerAttribute>()
                where attr.ResourceType == VirtualResourceType.EventLogCollector
                select new { Type = t }
            ).FirstOrDefault();

                handlerTypeQuery.Add(temp);
            }

            TraceFactory.Logger.Debug("OriginalHandlerQuery");
            foreach (var item in handlerTypeQuery)
            {
                TraceFactory.Logger.Debug(item.ToString());

            }

            var count = handlerTypeQuery.Count();
            if (count == 0)
            {
                throw new ArgumentOutOfRangeException("No handler found for {0} resource type".FormatWith(manifest.ResourceType));
            }

            Collection<VirtualResourceHandler> handlers = new Collection<VirtualResourceHandler>();
            foreach (var q in handlerTypeQuery)
            {
                var handler = (VirtualResourceHandler)Activator.CreateInstance(q.Type, manifest);
                handlers.Add(handler);
            }

            return handlers;
        }
    }
}
