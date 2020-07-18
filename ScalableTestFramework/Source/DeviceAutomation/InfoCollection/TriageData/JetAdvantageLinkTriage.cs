using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.SPS.SES.Helper;
using System;
using System.Drawing;
using System.IO;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// Used to collect triage data on JetAdvantageLink platform
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.TriageBase" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData.ITriage" />
    public sealed class JetAdvantageLinkTriage : TriageBase, ITriage
    {
        private JetAdvantageLinkUI _linkUI;
        private JediOmniControlPanel _omniUI;

        private Image _linkImg;
        private Image _omniImg;
        private byte[] _linkImgByte;
        private string _uiDumpXML;
        private UiDump _uiDump;

        private TriageDataLog _logger;
        private PluginExecutionData _pluginExecutionData;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageLinkTriage"/> class.
        /// </summary>
        /// <param name="linkUI">JetAdantageLink UI</param>
        /// <param name="device">Target device</param>
        /// <param name="pluginExecutionData"></param>
        public JetAdvantageLinkTriage(IDevice device, JetAdvantageLinkUI linkUI, PluginExecutionData pluginExecutionData) : base(pluginExecutionData)
        {
            _linkUI = linkUI;
            _omniUI = ((JediOmniDevice)device).ControlPanel;
            _pluginExecutionData = pluginExecutionData;
            _logger = new TriageDataLog(_pluginExecutionData);
        }

        /// <summary>
        /// Submit JetAdvantageLink Triage Data
        /// </summary>
        public override void Submit()
        {
            ExecutionServices.SystemTrace.LogDebug($"Logging Triage data with JetAdvantageLink dump data for session {_pluginExecutionData.SessionId} and activity {_pluginExecutionData.ActivityExecutionId}.");
            
            GetCurrentControlIds();
            _uiDump = new UiDump(_linkImgByte, _uiDumpXML);
            using (var ms = new MemoryStream())
            {
                _uiDump.Save(ms);
                ExecutionServices.SystemTrace.LogDebug($"Ui Dump with length {ms.Length} is saved.");
                UIDumpData = System.Text.Encoding.Default.GetString(ms.ToArray());
            }
            base.Submit();
        }

        /// <summary>
        /// Gets the control panel image both Omni and Link
        /// </summary>
        public override void GetControlPanelImage()
        {
            Image screenshot;
            try
            {
                _linkImgByte = _linkUI.Controller.GetScreenCapture();
            }
            catch
            {
                _linkImgByte = null;
            }

            _uiDumpXML = _linkUI.Controller.GetUIDump();
            _omniImg = _omniUI.ScreenCapture();

            if (_linkImgByte != null)
            {
                using (var ms = new MemoryStream(_linkImgByte))
                {
                    screenshot = Image.FromStream(ms);
                }
                _linkImg = screenshot;

                Bitmap bitmap = new Bitmap(_linkImg.Width + _omniImg.Width, Math.Max(_linkImg.Height, _omniImg.Height));
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(_linkImg, 0, 0);
                    g.DrawImage(_omniImg, _linkImg.Width, 0);
                }
                ControlPanelImage = bitmap;
            }
            else
            {
                ControlPanelImage = _omniImg;
            }
        }
        /// <summary>
        /// Gets ADB logcat data
        /// </summary>
        public override void GetCurrentControlIds()
        {
            ControlPanelControlIds =_linkUI.Controller.ExecuteADBCommand("logcat -d -v threadtime", 30);
       }
    }
}
