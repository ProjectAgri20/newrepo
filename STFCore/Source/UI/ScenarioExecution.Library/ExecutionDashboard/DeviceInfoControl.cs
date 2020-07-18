using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Device)]
    public partial class DeviceInfoControl : ElementInfoControlBase
    {
        string _sessionId = null;
        string _deviceId = null;

        protected delegate void UpdateControlTextHandler(Control control, string text);

        public DeviceInfoControl()
        {
            InitializeComponent();
        }

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            _sessionId = sessionInfo.SessionId;
            _deviceId = element.Name;
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                var data = DeviceReportData.GetReportData(_sessionId, _deviceId);
                deviceActivityLogBindingSource.DataSource = data;
                radGridView1.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("RefreshData: " + ex.ToString());
            }
        }

        private void UpdateControlText(Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new UpdateControlTextHandler(UpdateControlText), control, text);
                return;
            }

            control.Text = text;
        }
    }
}
