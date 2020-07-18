using System;
using System.IO;
using System.Linq;
using System.Reflection;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Service.ClientFactory
{
    internal class PluginLocator : MarshalByRefObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "This method CANNOT be marked static because it is used when constructing app domains.")]
        public PluginData GetPluginName(string assemblyLocation, VirtualResourceType resourceType)
        {
            // Anonymous method for auto-loading reflection-only assemblies.
            ResolveEventHandler handler = (notUsed, args) =>
            {
                var path = Path.GetDirectoryName(assemblyLocation);
                var fileName = args.Name.Split(',').First() + ".dll";
                var newAssemblyLocation = Path.Combine(path, fileName);
                if (File.Exists(newAssemblyLocation))
                {
                    return Assembly.ReflectionOnlyLoadFrom(newAssemblyLocation);
                }

                // The file doesn't exist.  It might be a GAC assembly.
                return Assembly.ReflectionOnlyLoad(args.Name);
            };

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += handler;
            try
            {
                var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyLocation);
                var type = FindType(assembly, resourceType);
                return type != null ? new PluginData(assemblyLocation, type.FullName) : null;
            }
            finally
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= handler;
            }
        }

        private static Type FindType(Assembly assembly, VirtualResourceType resourceType)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                foreach (var data in type.GetCustomAttributesData().Where(d => d.Constructor.DeclaringType.FullName == typeof(VirtualResourceHandlerAttribute).FullName))
                {
                    if (data.ConstructorArguments.Any(a => a.Value.ToString().Equals(resourceType.ToString())))
                    {
                        return type;
                    }
                }
            }

            return null;
        }
    }

    [Serializable]
    internal class PluginData
    {
        public string Type { get; private set; }
        public string AssemblyLocation { get; private set; }

        public PluginData(string assemblyLocation, string type)
        {
            AssemblyLocation = assemblyLocation;
            Type = type;
        }
    }

    //public static IVirtualResourceHandler Create(SystemManifest manifest)
    //{
    //    if (manifest == null)
    //    {
    //        throw new ArgumentNullException("manifest");
    //    }

    //    var pluginData = GetImplementingAssembly(manifest.ResourceType);

    //    var assembly = Assembly.LoadFrom(pluginData.AssemblyLocation);
    //    var handlerType = assembly.GetType(pluginData.Type);
    //    return (IVirtualResourceHandler)Activator.CreateInstance(handlerType, manifest);
    //}

    //private static PluginData GetImplementingAssembly(VirtualResourceType resourceType)
    //{
    //    // Look in the VirtualResource plugin directory associated with the specific resource type
    //    // for the existence of the resource plugin.
    //    string resourceDirectory = "../VirtualResourcePlugin/{0}".FormatWith(resourceType);
    //    TraceFactory.Logger.Debug("Looking in {0}".FormatWith(resourceDirectory));

    //    foreach (var file in Directory.GetFiles(resourceDirectory, "*{0}*.dll".FormatWith(resourceType), SearchOption.AllDirectories))
    //    {
    //        TraceFactory.Logger.Info("Checking {0} for resource type {1}".FormatWith(file, resourceType));
    //        var setup = AppDomain.CurrentDomain.SetupInformation;
    //        setup.PrivateBinPath = Path.GetDirectoryName(file);
    //        var domain = AppDomain.CreateDomain("Reflection: " + file, null, setup);
    //        try
    //        {
    //            var locator = (PluginLocator)domain.CreateInstanceAndUnwrap(typeof(PluginLocator).Assembly.FullName, typeof(PluginLocator).FullName);
    //            var pluginData = locator.GetPluginName(file, resourceType);
    //            if (pluginData != null)
    //            {
    //                TraceFactory.Logger.Info("{0} supports {1}".FormatWith(file, resourceType));
    //                return pluginData;
    //            }
    //        }
    //        finally
    //        {
    //            AppDomain.Unload(domain);
    //        }
    //    }
    //    throw new NotSupportedException("Unable to find handler for resource type: " + resourceType);
    //}

}
