using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.Common;
using System.Windows.Forms.DataVisualization.Charting;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.UI;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.LabConsole.Properties;

namespace HP.ScalableTest.LabConsole
{
    /// <summary>
    /// Control used to display Environment data for the STF
    /// </summary>
    public partial class DashboardControl : UserControl
    {
        private bool _refreshing = false;
        private ColorWheel _colorWheel = new ColorWheel();
        private Dictionary<string, Color> _assignedColors = new Dictionary<string, Color>();
        
        public DashboardControl()
        {
            InitializeComponent();
            summary_DataGrid.AutoGenerateColumns = false;
            _assignedColors.Add("Available", Color.DarkGreen);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string env = GlobalSettings.Items[Setting.Environment];
            string org = GlobalSettings.Items[Setting.Organization];

            gridCaption_Label.Text = "{0} {1} Environment Usage Summary".FormatWith(org, env);
        }

        /// <summary>
        /// Refresh all data displayed by the control
        /// </summary>
        public override void Refresh()
        {
            RefreshAll();
            base.Refresh();
        }

        public void Initialize()
        {
            client_PieChart.ConnectionString = DbConnect.AssetInventoryConnectionString.ToString();
            client_PieChart.SqlCommand = Resources.VMUsagePieChartSql;
            client_PieChart.ColorWheel = _colorWheel;
            client_PieChart.Colors = _assignedColors;

            simulator_PieChart.ConnectionString = DbConnect.AssetInventoryConnectionString.ToString();
            simulator_PieChart.SqlCommand = Resources.SimulatorUsagePieChartSql;
            simulator_PieChart.ColorWheel = _colorWheel;
            simulator_PieChart.Colors = _assignedColors;
        }

        private void RefreshAll()
        {
            if (_refreshing == false)
            {
                _refreshing = true;                

                System.Diagnostics.Debug.WriteLine("Refreshing Info");
                try
                {
                    client_PieChart.RefreshChart();
                    simulator_PieChart.RefreshChart();
                    RefreshSummaryGrid();
                }
                catch (NullReferenceException nullEx)
                {
                    TraceFactory.Logger.Error("Unable to refresh the dashboard.", nullEx);
                }

                _refreshing = false;
            }
        }

        private void RefreshSummaryGrid()
        {
            summary_DataGrid.DataSource = null;

						IEnumerable<dynamic> sessionUsages = VirtualMachine.SelectVirtualMachineSessionUsageSummary();
						summary_DataGrid.DataSource = sessionUsages;
						
            ColorTheGrid();
        }

        private void ColorTheGrid()
        {
            string key = string.Empty;
            foreach (DataGridViewRow row in summary_DataGrid.Rows)
            {
                DataGridViewCell cell = row.Cells[sessionId_Column.Index];
                key = ((string)cell.Value ?? string.Empty);

                row.HeaderCell.Style.BackColor = GetNextColor(key);
                row.HeaderCell.Style.SelectionBackColor = row.HeaderCell.Style.BackColor;
            }
        }

        protected virtual Color GetNextColor(string key)
        {
            if (_assignedColors.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase)))
            {
                return _assignedColors[key];
            }

            Color nextColor = _colorWheel.NextColor(ColorMoods.Failure);
            _assignedColors.Add(key, nextColor);
            return nextColor;
        }

        private void refresh_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Refresh();
        }
    } // DashboardControl
}
