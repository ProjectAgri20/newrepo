using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Factory for creating <see cref="ITriage" /> objects.
    /// </summary>
    public sealed class TriageFactory : DeviceFactoryCore<ITriage>
    {
        private static TriageFactory _instance = new TriageFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="TriageFactory" /> class.
        /// </summary>
        private TriageFactory()
        {
            Add<JediOmniDevice, JediOmniTriage>();
            Add<JediWindjammerDevice, JediWindjammerTriage>();
            Add<OzWindjammerDevice, OzWindjammerTriage>();
            Add<SiriusUIv2Device, SiriusUIv2Triage>();
            Add<SiriusUIv3Device, SiriusUIv3Triage>();
            Add<PhoenixMagicFrameDevice, PhoenixMagicFrameTriage>();
            Add<PhoenixNovaDevice, PhoenixNovaTriage>();
        }

        /// <summary>
        /// Creates an <see cref="ITriage" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <returns>An <see cref="ITriage" /> for the specified device.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static ITriage Create(IDevice device, PluginExecutionData pluginExecutionData)
        {
            if (pluginExecutionData == null)
            {
                throw new ArgumentNullException(nameof(pluginExecutionData));
            }

            return _instance.FactoryCreate(device, pluginExecutionData);
        }
    }
}
