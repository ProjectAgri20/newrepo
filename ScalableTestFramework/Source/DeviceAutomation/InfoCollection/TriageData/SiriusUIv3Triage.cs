using System.Linq;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on Sirius UIv3 based devices
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class SiriusUIv3Triage : TriageBase, ITriage
    {
        private readonly SiriusUIv3ControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3Triage"/> class.
        /// </summary>
        /// <param name="siriusUIv3Device">The Sirius u iv3 device.</param>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        public SiriusUIv3Triage(SiriusUIv3Device siriusUIv3Device, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _controlPanel = siriusUIv3Device.ControlPanel;
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