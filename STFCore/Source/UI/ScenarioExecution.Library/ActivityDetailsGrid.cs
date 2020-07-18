using System;
using System.Drawing;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// This form uses a Singleton pattern to ensure that only a single instance can be displayed
    /// at any given time.
    /// </summary>
    public partial class ActivityDetailsGrid : Form
    {
        private static ActivityDetailsGrid instance;
        private static object syncRoot = new object();

        /// <summary>
        /// Returns a single instance of this form.
        /// </summary>
        public static ActivityDetailsGrid Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    lock (syncRoot)
                    {
                        if (instance == null || instance.IsDisposed)
                        {
                            instance = new ActivityDetailsGrid();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// The constructor is marked as private so that the user cannot create a duplicate
        /// instance of this form. To get an instance of this form then call
        /// ActivityDetailsGrid.Instance.
        /// </summary>
        private ActivityDetailsGrid()
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(activityDetails_RadGridView, GridViewStyle.ReadOnly);
        }

        private void close_Button_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void activityDetails_RadGridView_GroupSummaryEvaluate(object sender, GroupSummaryEvaluationEventArgs e)
        {
            if (e.SummaryItem.Name.Contains("Status", StringComparison.OrdinalIgnoreCase))
            {
                e.FormatString = "{0}: ({1} Activities)".FormatWith(e.Value, e.Group.ItemCount);
            }
        }

        public void RefreshDetailsGrid()
        {
            activityDetails_RadGridView.MasterView.Refresh();
        }

        private void activityDetails_RadGridView_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo is GridViewGroupRowInfo)
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = Color.DarkGray;
                e.CellElement.GradientStyle = GradientStyles.Solid;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            }
        }
    }
}
