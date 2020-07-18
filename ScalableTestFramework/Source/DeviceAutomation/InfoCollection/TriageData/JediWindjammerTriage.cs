using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on Jedi windjammer based devices
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class JediWindjammerTriage : TriageBase, ITriage
    {
        private JediWindjammerDevice _jediWindjammerDevice;
        private JediWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerTriage"/> class.
        /// </summary>
        /// <param name="jediWindjammerDevice">The jedi windjammer device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public JediWindjammerTriage(JediWindjammerDevice jediWindjammerDevice, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _jediWindjammerDevice = jediWindjammerDevice;
            _controlPanel = _jediWindjammerDevice.ControlPanel;
        }

        /// <summary>
        /// Gets the control panel image.
        /// </summary>
        public override void GetControlPanelImage()
        {
            ControlPanelImage = _controlPanel.ScreenCapture();
        }

        /// <summary>
        /// Gets the current control ids located on the device panel.
        /// </summary>
        public override void GetCurrentControlIds()
        {
            ProcessControlIds(_controlPanel.GetControls());
        }

    }
}