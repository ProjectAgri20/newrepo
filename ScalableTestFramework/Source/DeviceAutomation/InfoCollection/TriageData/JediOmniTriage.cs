using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on Jedi Omni based devices
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class JediOmniTriage : TriageBase, ITriage
    {
        private JediOmniDevice _jediOmniDevice;
        private JediOmniControlPanel _controlPanel;
        private DeviceMessageWarning _jediDeviceWarnings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniTriage"/> class.
        /// </summary>
        /// <param name="jediOmniDevice">The jedi omni device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public JediOmniTriage(JediOmniDevice jediOmniDevice, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _jediOmniDevice = jediOmniDevice;
            _controlPanel = _jediOmniDevice.ControlPanel;
            _jediDeviceWarnings = new DeviceMessageWarning(_jediOmniDevice);
        }

        /// <summary>
        /// Gets the control panel image.
        /// </summary>
        public override void GetControlPanelImage()
        {
            ControlPanelImage = _controlPanel.ScreenCapture();
        }

        /// <summary>
        /// Gets the current control ids.
        /// </summary>
        public override void GetCurrentControlIds()
        {
            ProcessControlIds(_controlPanel.GetIds(null, OmniIdCollectionType.Descendants));
        }

        /// <summary>
        /// Gets the device warnings.
        /// </summary>
        /// <returns></returns>
        public override string GetDeviceWarnings()
        {
            string warnings = string.Empty;

            if (_jediDeviceWarnings.IsMessageWarnings())
            {
                warnings = _jediDeviceWarnings.MessageWarnings;
            }

            return warnings;
        }
    }
}