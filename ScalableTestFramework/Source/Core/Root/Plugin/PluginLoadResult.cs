using System;

namespace HP.ScalableTest.Core.Plugin
{
    /// <summary>
    /// The cached result of a plugin load operation.
    /// </summary>
    public sealed class PluginLoadResult
    {
        /// <summary>
        /// Gets a value indicating whether the plugin load was successful.
        /// </summary>
        public bool LoadSuccessful
        {
            get { return ObjectType != null; }
        }

        /// <summary>
        /// Gets the type of the interface that was attempted to load.
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// Gets the type of the plugin class that implements <see cref="InterfaceType" />.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        /// Gets the error message resulting from an unsuccessful load.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadResult" /> class.
        /// </summary>
        /// <param name="interfaceType">The interface that was attempted to load.</param>
        /// <param name="objectType">The type of the plugin class that implements the interface.</param>
        public PluginLoadResult(Type interfaceType, Type objectType)
        {
            InterfaceType = interfaceType;
            ObjectType = objectType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadResult"/> class.
        /// </summary>
        /// <param name="interfaceType">The interface that was attempted to load.</param>
        /// <param name="errorMessage">The error message resulting from an unsuccessful load.</param>
        public PluginLoadResult(Type interfaceType, string errorMessage)
        {
            InterfaceType = interfaceType;
            ErrorMessage = errorMessage;
        }
    }
}
