using System.Linq;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on Oz windjammer based devices
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class OzWindjammerTriage : TriageBase, ITriage
    {
        private OzWindjammerDevice _ozWindjammerDevice;
        private OzWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerTriage"/> class.
        /// </summary>
        /// <param name="ozWindjammerDevice">The oz windjammer device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public OzWindjammerTriage(OzWindjammerDevice ozWindjammerDevice, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _ozWindjammerDevice = ozWindjammerDevice;
            _controlPanel = _ozWindjammerDevice.ControlPanel;
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
            WidgetCollection wc = _controlPanel.GetWidgets();
            ControlPanelControlIds = string.Join(",", wc.Select(x => x.Id).ToArray());
        }

    }
}