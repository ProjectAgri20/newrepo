using System.Linq;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Triage collection for Phoenix Magic Frame
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public class PhoenixMagicFrameTriage : TriageBase, ITriage
    {
        private readonly PhoenixMagicFrameControlPanel _phoenixMagicFrameControlPanel;
        /// <summary>
        /// Triage Collection for PhoenixMagicFrame
        /// </summary>
        /// <param name="phoenixMagicFrameDevice"></param>
        /// <param name="pluginExecutionData"></param>
        public PhoenixMagicFrameTriage(PhoenixMagicFrameDevice phoenixMagicFrameDevice, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _phoenixMagicFrameControlPanel = phoenixMagicFrameDevice.ControlPanel;
        }

        /// <summary>
        /// Empty method since the Triage collection for Phoenix is not enabled at this time.
        /// </summary>
        public override void Submit()
        {

        }
        /// <summary>
        /// Empty method since the Triage collection for Phoenix is not enabled at this time.
        /// </summary>
        public override void GetControlPanelImage()
        {
            ControlPanelImage = _phoenixMagicFrameControlPanel.ScreenCapture();
        }

        /// <summary>
        /// Empty method since the Triage collection for Phoenix is not enabled at this time.
        /// </summary>
        public override void GetCurrentControlIds()
        {
            var virtualButtons = _phoenixMagicFrameControlPanel.GetVirtualButtons();
            ProcessControlIds(virtualButtons.Select(x => x.Name));
        }
    }

    /// <summary>
    /// Triage collection for Phoenix Nova device
    /// </summary>
    public class PhoenixNovaTriage : TriageBase, ITriage
    {
        private readonly PhoenixNovaControlPanel _phoenixNovaControlPanel;

        /// <summary>
        /// Triage Collection for PhoenixNova 
        /// </summary>
        /// <param name="phoenixNovaDevice"></param>
        /// <param name="pluginExecutionData"></param>
        public PhoenixNovaTriage(PhoenixNovaDevice phoenixNovaDevice, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _phoenixNovaControlPanel = phoenixNovaDevice.ControlPanel;
        }

        /// <summary>
        /// Virtual method for retrieving the control panel image.
        /// </summary>
        public override void GetControlPanelImage()
        {
            ControlPanelImage = _phoenixNovaControlPanel.ScreenCapture();
        }

        /// <summary>
        /// Virtual method for retrieving the current control ids.
        /// </summary>
        public override void GetCurrentControlIds()
        {
            var virtualButtons = _phoenixNovaControlPanel.GetVirtualButtons();
            ProcessControlIds(virtualButtons.Select(x => x.Name));
        }
    }
}
