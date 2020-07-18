using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Plugin
{
    /// <summary>
    /// Loads plugin assemblies in the form of <see cref="PluginAssembly" /> objects.
    /// </summary>
    public static class PluginFactory
    {
        private static readonly Dictionary<string, PluginDefinition> _pluginAssemblies = new Dictionary<string, PluginDefinition>();
        private static readonly ConcurrentDictionary<string, PluginAssembly> _assemblyCache = new ConcurrentDictionary<string, PluginAssembly>();

        /// <summary>
        /// Gets or sets the relative path from the executing assembly to the folder containing the plugins.
        /// </summary>
        public static string PluginRelativePath { get; set; }

        /// <summary>
        /// Initializes the <see cref="PluginFactory" /> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static PluginFactory()
        {
            // Subscribe to assembly load failure events 
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// Registers a plugin with the factory.
        /// </summary>
        /// <param name="pluginDefinition">The <see cref="PluginDefinition" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pluginDefinition" /> is null.</exception>
        public static void Register(PluginDefinition pluginDefinition)
        {
            if (pluginDefinition == null)
            {
                throw new ArgumentNullException(nameof(pluginDefinition));
            }

            _pluginAssemblies[pluginDefinition.Name] = pluginDefinition;
        }

        /// <summary>
        /// Registers the specified plugins with the factory.
        /// </summary>
        /// <param name="pluginDefinitions">The <see cref="PluginDefinition" /> collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pluginDefinitions" /> is null.</exception>
        public static void Register(IEnumerable<PluginDefinition> pluginDefinitions)
        {
            if (pluginDefinitions == null)
            {
                throw new ArgumentNullException(nameof(pluginDefinitions));
            }

            foreach (PluginDefinition definition in pluginDefinitions)
            {
                Register(definition);
            }
        }

        /// <summary>
        /// Clears the plugin definitions.
        /// </summary>
        public static void ClearPluginDefinitions()
        {
            _pluginAssemblies.Clear();
        }

        /// <summary>
        /// Gets a <see cref="PluginAssembly" /> for the registered plugin with the specified name.
        /// </summary>
        /// <param name="pluginName">The plugin name.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"><paramref name="pluginName" /> is not a registered plugin.</exception>
        public static PluginAssembly GetPlugin(string pluginName)
        {
            if (_pluginAssemblies.TryGetValue(pluginName, out PluginDefinition definition))
            {
                return GetPluginByAssemblyName(definition.AssemblyName);
            }
            else
            {
                throw new InvalidOperationException($"Plugin {pluginName} is not registered with the plugin factory.");
            }
        }

        /// <summary>
        /// Gets a <see cref="PluginAssembly" /> object for the specified assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>A <see cref="PluginAssembly" /> object for the specified assembly.</returns>
        public static PluginAssembly GetPluginByAssemblyName(string assemblyName)
        {
            return _assemblyCache.GetOrAdd(assemblyName, n => LoadAssembly(n));
        }

        private static PluginAssembly LoadAssembly(string assemblyName)
        {
            string exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyPath;
            if (!string.IsNullOrEmpty(PluginRelativePath))
            {
                assemblyPath = Path.Combine(exeLocation, PluginRelativePath, assemblyName);
            }
            else
            {
                assemblyPath = Path.Combine(exeLocation, assemblyName);
            }

            try
            {
                LogDebug($"Loading plugin assembly at {assemblyName}");
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                return new PluginAssembly(assembly);
            }
            catch (Exception ex)
            {
                LogError($"Error loading plugin assembly: {ex.Message}", ex);
                throw new PluginLoadException($"Error loading plugin assembly {assemblyPath}.", ex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Attempt to locate and load an assembly from the Plugin folder if we failed to load the first time.
            // Example:  Plugin being loaded has a dependency that also lives in the plugin folder;
            // the default behavior is to first try loading from the executing folder, which will fail.
            Assembly result = null;
            try
            {
                // Look for the failed assembly in the Plugin folder
                if (!string.IsNullOrEmpty(PluginRelativePath))
                {
                    string exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string assemblyFileName = args.Name.Split(',')[0] + ".dll";
                    string assemblyPath = Path.Combine(exeLocation, PluginRelativePath, assemblyFileName);
                    if (File.Exists(assemblyPath))
                    {
                        result = Assembly.LoadFrom(assemblyPath);
                    }
                }
            }
            catch
            {
                // do nothing
            }
            return result;
        }
    }
}
