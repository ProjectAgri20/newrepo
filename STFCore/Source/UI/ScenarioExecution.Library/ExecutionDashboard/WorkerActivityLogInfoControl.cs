using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    public partial class WorkerActivityLogInfoControl : ElementInfoControlBase
    {
        protected delegate void UpdateControlTextHandler(Control control, string text);

        public WorkerActivityLogInfoControl()
        {
            InitializeComponent();
        }

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                using (var context = DbConnect.DataLogContext())
                {
                    var activities = GetActivityExecutionData(context);
                    activityExecutionBindingSource.DataSource = activities;
                }

                radGridView1.MasterTemplate.BestFitColumns(BestFitColumnMode.AllCells);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("RefreshData: " + ex.ToString());
            }
        }

        protected virtual List<SessionActivityData> GetActivityExecutionData(DataLogContext context)
        {
            throw new NotImplementedException();
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
