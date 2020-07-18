using System.Linq;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on Sirius UIv2 based devices
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class SiriusUIv2Triage : TriageBase, ITriage
    {
        private readonly SiriusUIv2ControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2Triage"/> class.
        /// </summary>
        /// <param name="siriusUIv2Device">The Sirius u iv2 device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public SiriusUIv2Triage(SiriusUIv2Device siriusUIv2Device, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _controlPanel = siriusUIv2Device.ControlPanel;
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
            WidgetCollection wc = _controlPanel.GetScreenInfo().Widgets;

            ControlPanelControlIds = string.Join(",", wc.Select(x => x.Id).ToArray());
        }

    }
}