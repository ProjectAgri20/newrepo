namespace HP.ScalableTest.UI.Charting
{
    partial class TimeGraphBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.Chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearFilter_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seriesFilter_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.autoRefresh_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshNone_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh1Minute_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh2Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh5minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh10Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.realTimeWindow_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowOff_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.window10Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.window30Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.window60Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.window90Minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.legendPosition_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendOff_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendBottom_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendRight_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDate_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.use24HourTime_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAMPM_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh_Timer = new System.Windows.Forms.Timer(this.components);
            this.calculatedSeries_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            this.chart_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Chart
            // 
            this.Chart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            this.Chart.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this.Chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.Chart.BorderlineWidth = 2;
            this.Chart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LabelStyle.Format = "H:mm";
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.ScaleView.SmallScrollSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.Silver;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.Chart.ChartAreas.Add(chartArea1);
            this.Chart.ContextMenuStrip = this.chart_contextMenuStrip;
            this.Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BorderColor = System.Drawing.Color.Black;
            legend1.DockedToChartArea = "ChartArea";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            legend1.IsDockedInsideChartArea = false;
            legend1.IsTextAutoFit = false;
            legend1.Name = "MainLegend";
            legend1.ShadowOffset = 5;
            this.Chart.Legends.Add(legend1);
            this.Chart.Location = new System.Drawing.Point(0, 0);
            this.Chart.Name = "Chart";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.CustomProperties = "EmptyPointValue=Zero";
            series1.Legend = "MainLegend";
            series1.MarkerSize = 10;
            series1.Name = "TemplateSeries";
            series1.ShadowColor = System.Drawing.Color.Black;
            series1.ShadowOffset = 1;
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            this.Chart.Series.Add(series1);
            this.Chart.Size = new System.Drawing.Size(450, 375);
            this.Chart.TabIndex = 1;
            title1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title";
            title1.Text = "Graph Title";
            this.Chart.Titles.Add(title1);
            // 
            // chart_contextMenuStrip
            // 
            this.chart_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearFilter_ToolStripMenuItem,
            this.seriesFilter_ToolStripMenuItem,
            this.calculatedSeries_ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.autoRefresh_ToolStripMenuItem,
            this.realTimeWindow_ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.legendPosition_ToolStripMenuItem,
            this.timeFormatToolStripMenuItem});
            this.chart_contextMenuStrip.Name = "contextMenuStrip";
            this.chart_contextMenuStrip.Size = new System.Drawing.Size(176, 192);
            // 
            // clearFilter_ToolStripMenuItem
            // 
            this.clearFilter_ToolStripMenuItem.Enabled = false;
            this.clearFilter_ToolStripMenuItem.Name = "clearFilter_ToolStripMenuItem";
            this.clearFilter_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.clearFilter_ToolStripMenuItem.Text = "Clear Filter";
            this.clearFilter_ToolStripMenuItem.Click += new System.EventHandler(this.clearFilter_ToolStripMenuItem_Click);
            // 
            // seriesFilter_ToolStripMenuItem
            // 
            this.seriesFilter_ToolStripMenuItem.Name = "seriesFilter_ToolStripMenuItem";
            this.seriesFilter_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.seriesFilter_ToolStripMenuItem.Text = "Series Filter...";
            this.seriesFilter_ToolStripMenuItem.Click += new System.EventHandler(this.seriesFilter_ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 6);
            // 
            // autoRefresh_ToolStripMenuItem
            // 
            this.autoRefresh_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshNone_ToolStripMenuItem,
            this.refresh1Minute_ToolStripMenuItem,
            this.refresh2Minutes_ToolStripMenuItem,
            this.refresh5minutes_ToolStripMenuItem,
            this.refresh10Minutes_ToolStripMenuItem});
            this.autoRefresh_ToolStripMenuItem.Name = "autoRefresh_ToolStripMenuItem";
            this.autoRefresh_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.autoRefresh_ToolStripMenuItem.Text = "Auto Refresh";
            // 
            // refreshNone_ToolStripMenuItem
            // 
            this.refreshNone_ToolStripMenuItem.Checked = true;
            this.refreshNone_ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.refreshNone_ToolStripMenuItem.Name = "refreshNone_ToolStripMenuItem";
            this.refreshNone_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refreshNone_ToolStripMenuItem.Text = "None";
            this.refreshNone_ToolStripMenuItem.Click += new System.EventHandler(this.refreshNone_ToolStripMenuItem_Click);
            // 
            // refresh1Minute_ToolStripMenuItem
            // 
            this.refresh1Minute_ToolStripMenuItem.Name = "refresh1Minute_ToolStripMenuItem";
            this.refresh1Minute_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refresh1Minute_ToolStripMenuItem.Tag = "1";
            this.refresh1Minute_ToolStripMenuItem.Text = "1 minute";
            this.refresh1Minute_ToolStripMenuItem.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh2Minutes_ToolStripMenuItem
            // 
            this.refresh2Minutes_ToolStripMenuItem.Name = "refresh2Minutes_ToolStripMenuItem";
            this.refresh2Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refresh2Minutes_ToolStripMenuItem.Tag = "2";
            this.refresh2Minutes_ToolStripMenuItem.Text = "2 minutes";
            this.refresh2Minutes_ToolStripMenuItem.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh5minutes_ToolStripMenuItem
            // 
            this.refresh5minutes_ToolStripMenuItem.Name = "refresh5minutes_ToolStripMenuItem";
            this.refresh5minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refresh5minutes_ToolStripMenuItem.Tag = "5";
            this.refresh5minutes_ToolStripMenuItem.Text = "5 minutes";
            this.refresh5minutes_ToolStripMenuItem.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh10Minutes_ToolStripMenuItem
            // 
            this.refresh10Minutes_ToolStripMenuItem.Name = "refresh10Minutes_ToolStripMenuItem";
            this.refresh10Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refresh10Minutes_ToolStripMenuItem.Tag = "10";
            this.refresh10Minutes_ToolStripMenuItem.Text = "10 minutes";
            this.refresh10Minutes_ToolStripMenuItem.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // realTimeWindow_ToolStripMenuItem
            // 
            this.realTimeWindow_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowOff_ToolStripMenuItem,
            this.window10Minutes_ToolStripMenuItem,
            this.window30Minutes_ToolStripMenuItem,
            this.window60Minutes_ToolStripMenuItem,
            this.window90Minutes_ToolStripMenuItem});
            this.realTimeWindow_ToolStripMenuItem.Name = "realTimeWindow_ToolStripMenuItem";
            this.realTimeWindow_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.realTimeWindow_ToolStripMenuItem.Text = "Real-Time Window";
            // 
            // windowOff_ToolStripMenuItem
            // 
            this.windowOff_ToolStripMenuItem.Checked = true;
            this.windowOff_ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.windowOff_ToolStripMenuItem.Name = "windowOff_ToolStripMenuItem";
            this.windowOff_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.windowOff_ToolStripMenuItem.Tag = "0";
            this.windowOff_ToolStripMenuItem.Text = "Off";
            this.windowOff_ToolStripMenuItem.Click += new System.EventHandler(this.windowMinutes_ToolStripMenuItem_Click);
            // 
            // window10Minutes_ToolStripMenuItem
            // 
            this.window10Minutes_ToolStripMenuItem.Name = "window10Minutes_ToolStripMenuItem";
            this.window10Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.window10Minutes_ToolStripMenuItem.Tag = "10";
            this.window10Minutes_ToolStripMenuItem.Text = "10 minutes";
            this.window10Minutes_ToolStripMenuItem.Click += new System.EventHandler(this.windowMinutes_ToolStripMenuItem_Click);
            // 
            // window30Minutes_ToolStripMenuItem
            // 
            this.window30Minutes_ToolStripMenuItem.Name = "window30Minutes_ToolStripMenuItem";
            this.window30Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.window30Minutes_ToolStripMenuItem.Tag = "30";
            this.window30Minutes_ToolStripMenuItem.Text = "30 minutes";
            // 
            // window60Minutes_ToolStripMenuItem
            // 
            this.window60Minutes_ToolStripMenuItem.Name = "window60Minutes_ToolStripMenuItem";
            this.window60Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.window60Minutes_ToolStripMenuItem.Tag = "60";
            this.window60Minutes_ToolStripMenuItem.Text = "60 minutes";
            // 
            // window90Minutes_ToolStripMenuItem
            // 
            this.window90Minutes_ToolStripMenuItem.Name = "window90Minutes_ToolStripMenuItem";
            this.window90Minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.window90Minutes_ToolStripMenuItem.Tag = "90";
            this.window90Minutes_ToolStripMenuItem.Text = "90 minutes";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(172, 6);
            // 
            // legendPosition_ToolStripMenuItem
            // 
            this.legendPosition_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.legendOff_ToolStripMenuItem,
            this.legendBottom_ToolStripMenuItem,
            this.legendRight_ToolStripMenuItem});
            this.legendPosition_ToolStripMenuItem.Name = "legendPosition_ToolStripMenuItem";
            this.legendPosition_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.legendPosition_ToolStripMenuItem.Text = "Legend Position";
            // 
            // legendOff_ToolStripMenuItem
            // 
            this.legendOff_ToolStripMenuItem.Name = "legendOff_ToolStripMenuItem";
            this.legendOff_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.legendOff_ToolStripMenuItem.Text = "Off";
            this.legendOff_ToolStripMenuItem.Click += new System.EventHandler(this.legendOption_ToolStripMenuItem_Click);
            // 
            // legendBottom_ToolStripMenuItem
            // 
            this.legendBottom_ToolStripMenuItem.Checked = true;
            this.legendBottom_ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.legendBottom_ToolStripMenuItem.Name = "legendBottom_ToolStripMenuItem";
            this.legendBottom_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.legendBottom_ToolStripMenuItem.Text = "Bottom";
            this.legendBottom_ToolStripMenuItem.Click += new System.EventHandler(this.legendOption_ToolStripMenuItem_Click);
            // 
            // legendRight_ToolStripMenuItem
            // 
            this.legendRight_ToolStripMenuItem.Name = "legendRight_ToolStripMenuItem";
            this.legendRight_ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.legendRight_ToolStripMenuItem.Text = "Right";
            this.legendRight_ToolStripMenuItem.Click += new System.EventHandler(this.legendOption_ToolStripMenuItem_Click);
            // 
            // timeFormatToolStripMenuItem
            // 
            this.timeFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDate_ToolStripMenuItem,
            this.use24HourTime_ToolStripMenuItem,
            this.showAMPM_ToolStripMenuItem});
            this.timeFormatToolStripMenuItem.Name = "timeFormatToolStripMenuItem";
            this.timeFormatToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.timeFormatToolStripMenuItem.Text = "Time Format";
            // 
            // showDate_ToolStripMenuItem
            // 
            this.showDate_ToolStripMenuItem.CheckOnClick = true;
            this.showDate_ToolStripMenuItem.Name = "showDate_ToolStripMenuItem";
            this.showDate_ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showDate_ToolStripMenuItem.Text = "Show Date";
            this.showDate_ToolStripMenuItem.Click += new System.EventHandler(this.timeOptions_ToolStripMenuItem_Click);
            // 
            // use24HourTime_ToolStripMenuItem
            // 
            this.use24HourTime_ToolStripMenuItem.CheckOnClick = true;
            this.use24HourTime_ToolStripMenuItem.Name = "use24HourTime_ToolStripMenuItem";
            this.use24HourTime_ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.use24HourTime_ToolStripMenuItem.Text = "Use 24-Hour Time";
            this.use24HourTime_ToolStripMenuItem.Click += new System.EventHandler(this.timeOptions_ToolStripMenuItem_Click);
            // 
            // showAMPM_ToolStripMenuItem
            // 
            this.showAMPM_ToolStripMenuItem.CheckOnClick = true;
            this.showAMPM_ToolStripMenuItem.Enabled = false;
            this.showAMPM_ToolStripMenuItem.Name = "showAMPM_ToolStripMenuItem";
            this.showAMPM_ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showAMPM_ToolStripMenuItem.Text = "Show AM/PM";
            this.showAMPM_ToolStripMenuItem.Click += new System.EventHandler(this.timeOptions_ToolStripMenuItem_Click);
            // 
            // refresh_Timer
            // 
            this.refresh_Timer.Tick += new System.EventHandler(this.refresh_Timer_Tick);
            // 
            // calculatedSeries_ToolStripMenuItem
            // 
            this.calculatedSeries_ToolStripMenuItem.Name = "calculatedSeries_ToolStripMenuItem";
            this.calculatedSeries_ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.calculatedSeries_ToolStripMenuItem.Text = "Calculated Series...";
            this.calculatedSeries_ToolStripMenuItem.Click += new System.EventHandler(this.calculatedSeries_ToolStripMenuItem_Click);
            // 
            // TimeGraphBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Chart);
            this.Name = "TimeGraphBase";
            this.Size = new System.Drawing.Size(450, 375);
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            this.chart_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Chart;
        private System.Windows.Forms.ContextMenuStrip chart_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearFilter_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seriesFilter_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem autoRefresh_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshNone_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refresh1Minute_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refresh2Minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refresh5minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refresh10Minutes_ToolStripMenuItem;
        private System.Windows.Forms.Timer refresh_Timer;
        private System.Windows.Forms.ToolStripMenuItem realTimeWindow_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowOff_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem window10Minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem window30Minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem window60Minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem window90Minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem legendPosition_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legendOff_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legendBottom_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legendRight_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDate_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem use24HourTime_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAMPM_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculatedSeries_ToolStripMenuItem;
    }
}
