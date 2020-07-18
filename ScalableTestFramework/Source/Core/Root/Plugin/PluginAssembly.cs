using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Plugin
{
    /// <summary>
    /// Creates plugin objects from a plugin assembly.  Caches reflection results to improve performance.
    /// </summary>
    public sealed class PluginAssembly
    {
        private readonly Assembly _assembly;
        private readonly ConcurrentDictionary<Type, PluginLoadResult> _typeMap = new ConcurrentDictionary<Type, PluginLoadResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAssembly" /> class.
        /// </summary>
        /// <param name="assembly">The plugin <see cref="Assembly" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly" /> is null.</exception>
        public PluginAssembly(Assembly assembly)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the Name of the Plugin Assembly.
        /// </summary>
        public string Name
        {
            get
            {
                int idx = _assembly.FullName.IndexOf(',');
                return _assembly.FullName.Substring(0, idx);
            }
        }

        /// <summary>
        /// Checks to see whether an implementation of the specified interface exists in the plugin assembly.
        /// </summary>
        /// <typeparam name="T">The interface to attempt to load from the plugin assembly.</typeparam>
        /// <returns><c>true</c> if the plugin contains an implementation of the specified interface; otherwise, <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public bool Implements<T>()
        {
            return LoadPlugin<T>().LoadSuccessful;
        }

        /// <summary>
        /// Instantiates an implementation of the specified interface from the plugin assembly.
        /// </summary>
        /// <typeparam name="T">The interface to attempt to load from the plugin assembly.</typeparam>
        /// <returns>The plugin assembly's implementation of the specified interface.</returns>
        /// <exception cref="PluginLoadException">An error occurred while loading the plugin.</exception>
        public T Create<T>() where T : class
        {
            PluginLoadResult loadResult = LoadPlugin<T>();
            if (loadResult.LoadSuccessful)
            {
                return (T)_assembly.CreateInstance(loadResult.ObjectType.FullName);
            }
            else
            {
                LogError($"Failed attempting to get types from assembly {_assembly.Location}: {loadResult.ErrorMessage}");
                throw new PluginLoadException(loadResult.ErrorMessage);
            }
        }

        private PluginLoadResult LoadPlugin<T>()
        {
            // First try loading from cache; if not present, load from assembly
            return _typeMap.GetOrAdd(typeof(T), n => LoadPlugin(_assembly, n));
        }

        /// <summary>
        /// Attempts to load the plugin implementation of the specified interface.
        /// </summary>
        /// <param name="assembly">The plugin assembly.</param>
        /// <param name="interfaceType">The interface to attempt to load from the plugin assembly.</param>
        /// <returns>A <see cref="PluginLoadResult" /> indicating the result of the plugin load attempt.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembly" /> is null.
        /// <para>or</para>
        /// <paramref name="interfaceType" /> is null.
        /// </exception>
        public static PluginLoadResult LoadPlugin(Assembly assembly, Type interfaceType)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (interfaceType == null)
            {
                throw new ArgumentNullException(nameof(interfaceType));
            }

            List<Type> objectTypes = new List<Type>();
            try
            {
                objectTypes.AddRange(assembly.GetTypes().Where(n => n.GetInterface(interfaceType.Name, true) != null));
            }
            catch (ReflectionTypeLoadException ex)
            {
                string loadErrors = string.Join(Environment.NewLine, ex.LoaderExceptions.Select(n => n.Message));
                return new PluginLoadResult(interfaceType, ex.Message + Environment.NewLine + loadErrors);
            }

            switch (objectTypes.Count)
            {
                case 1:
                    return new PluginLoadResult(interfaceType, objectTypes.Single());

                case 0:
                    return new PluginLoadResult(interfaceType, $"{assembly.Location} does not contain an implementation of {interfaceType.Name}.");

                default:
                    return new PluginLoadResult(interfaceType, $"{assembly.Location} contains {objectTypes.Count} types that implement {interfaceType.Name}.");
            }
        }
    }
}
