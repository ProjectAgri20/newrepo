using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Interface for a simulated version of the framework for use during plugin development.
    /// </summary>
    public interface IPluginFrameworkSimulator
    {
        /// <summary>
        /// Gets the <see cref="IAssetInventory" /> for the simulator.
        /// </summary>
        IAssetInventory AssetInventory { get; }

        /// <summary>
        /// Gets the <see cref="ICriticalSection" /> for the simulator.
        /// </summary>
        ICriticalSection CriticalSection { get; }

        /// <summary>
        /// Gets the <see cref="IDataLogger" /> for the simulator.
        /// </summary>
        IDataLogger DataLogger { get; }

        /// <summary>
        /// Gets the <see cref="IDocumentLibrary" /> for the simulator.
        /// </summary>
        IDocumentLibrary DocumentLibrary { get; }

        /// <summary>
        /// Gets the <see cref="IEnvironmentConfiguration" /> for the simulator.
        /// </summary>
        IEnvironmentConfiguration EnvironmentConfiguration { get; }

        /// <summary>
        /// Gets the <see cref="IFileRepository" /> for the simulator.
        /// </summary>
        IFileRepository FileRepository { get; }

        /// <summary>
        /// Gets the <see cref="ISessionRuntime" /> for the simulator.
        /// </summary>
        ISessionRuntime SessionRuntime { get; }

        /// <summary>
        /// Gets the <see cref="ISystemTrace" /> for the simulator.
        /// </summary>
        ISystemTrace SystemTrace { get; }

        /// <summary>
        /// Gets the Name of the loaded Plugin Assembly.
        /// </summary>
        string PluginAssemblyName { get; }

        /// <summary>
        /// Gets the type of <see cref="IPluginConfigurationControl" /> to use.
        /// </summary>
        Type ConfigurationControlType { get; }

        /// <summary>
        /// Gets the type of <see cref="IPluginExecutionEngine" /> to use.
        /// </summary>
        Type ExecutionEngineType { get; }

        /// <summary>
        /// Gets the <see cref="PluginEnvironment" /> to use.
        /// </summary>
        PluginEnvironment Environment { get; }

        /// <summary>
        /// Creates <see cref="PluginExecutionData" /> based on the specified <see cref="PluginConfigurationData" />.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>A new <see cref="PluginExecutionData" /> based on the specified <see cref="PluginConfigurationData" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configurationData" /> is null.</exception>
        PluginExecutionData CreateExecutionData(PluginConfigurationData configurationData);
    }

    /// <summary>
    /// Provides extension methods for <see cref="IPluginFrameworkSimulator" />.
    /// </summary>
    public static class PluginFrameworkSimulatorExtension
    {
        /// <summary>
        /// Creates an <see cref="IPluginConfigurationControl" /> from an <see cref="IPluginFrameworkSimulator" />.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" />.</param>
        /// <returns>A new <see cref="IPluginConfigurationControl" />, or null if the configuration control type is not set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public static IPluginConfigurationControl CreateConfigurationControl(this IPluginFrameworkSimulator simulator)
        {
            if (simulator == null)
            {
                throw new ArgumentNullException(nameof(simulator));
            }

            if (simulator.ConfigurationControlType != null)
            {
                return (IPluginConfigurationControl)Activator.CreateInstance(simulator.ConfigurationControlType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates an <see cref="IPluginExecutionEngine" /> from an <see cref="IPluginFrameworkSimulator" />.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" />.</param>
        /// <returns>A new <see cref="IPluginExecutionEngine" />, or null if the execution engine type is not set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public static IPluginExecutionEngine CreateExecutionEngine(this IPluginFrameworkSimulator simulator)
        {
            if (simulator == null)
            {
                throw new ArgumentNullException(nameof(simulator));
            }

            if (simulator.ExecutionEngineType != null)
            {
                return (IPluginExecutionEngine)Activator.CreateInstance(simulator.ExecutionEngineType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Enables and disables framework services according to the specified <see cref="FrameworkServicesContext" />.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" />.</param>
        /// <param name="context">The <see cref="FrameworkServicesContext" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public static void SetServiceContext(this IPluginFrameworkSimulator simulator, FrameworkServicesContext context)
        {
            if (simulator == null)
            {
                throw new ArgumentNullException(nameof(simulator));
            }

            ClearServices();

            switch (context)
            {
                case FrameworkServicesContext.Configuration:
                    EnableConfigurationServices(simulator);
                    break;

                case FrameworkServicesContext.Execution:
                    EnableExecutionServices(simulator);
                    break;

                case FrameworkServicesContext.ConfigurationConstructor:
                    ConfigurationServices.SystemTrace = simulator.SystemTrace;
                    break;

                case FrameworkServicesContext.ExecutionConstructor:
                    ExecutionServices.SystemTrace = simulator.SystemTrace;
                    break;

                case FrameworkServicesContext.All:
                    EnableConfigurationServices(simulator);
                    EnableExecutionServices(simulator);
                    break;
            }
        }

        private static void ClearServices()
        {
            ConfigurationServices.AssetInventory = null;
            ConfigurationServices.DocumentLibrary = null;
            ConfigurationServices.EnvironmentConfiguration = null;
            ConfigurationServices.SystemTrace = null;

            ExecutionServices.CriticalSection = null;
            ExecutionServices.DataLogger = null;
            ExecutionServices.FileRepository = null;
            ExecutionServices.SessionRuntime = null;
            ExecutionServices.SystemTrace = null;
        }

        private static void EnableConfigurationServices(IPluginFrameworkSimulator simulator)
        {
            ConfigurationServices.AssetInventory = simulator.AssetInventory;
            ConfigurationServices.DocumentLibrary = simulator.DocumentLibrary;
            ConfigurationServices.EnvironmentConfiguration = simulator.EnvironmentConfiguration;
            ConfigurationServices.SystemTrace = simulator.SystemTrace;
        }

        private static void EnableExecutionServices(IPluginFrameworkSimulator simulator)
        {
            ExecutionServices.CriticalSection = simulator.CriticalSection;
            ExecutionServices.DataLogger = simulator.DataLogger;
            ExecutionServices.FileRepository = simulator.FileRepository;
            ExecutionServices.SessionRuntime = simulator.SessionRuntime;
            ExecutionServices.SystemTrace = simulator.SystemTrace;
        }
    }
}
