namespace HP.ScalableTest.UI.Charting
{
    partial class BarGraphBase
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
            this.refresh2Minutes_ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh5minutes_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh10Minutes_ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.refresh_Timer = new System.Windows.Forms.Timer(this.components);
            this.zoomX_radTrackBar = new Telerik.WinControls.UI.RadTrackBar();
            this.zoomY_radTrackBar = new Telerik.WinControls.UI.RadTrackBar();
            this.chartAndZoom_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.zoom_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).BeginInit();
            this.chart_contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomX_radTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomY_radTrackBar)).BeginInit();
            this.chartAndZoom_tableLayoutPanel.SuspendLayout();
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
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
            chartArea1.AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.SmallScroll;
            chartArea1.AxisX.ScrollBar.IsPositionedInside = false;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels) 
            | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 80;
            chartArea1.AxisY.ScaleBreakStyle.MaxNumberOfBreaks = 1;
            chartArea1.AxisY.ScaleBreakStyle.StartFromZero = System.Windows.Forms.DataVisualization.Charting.StartFromZero.Yes;
            chartArea1.AxisY.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.SmallScroll;
            chartArea1.AxisY.ScrollBar.IsPositionedInside = false;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.Silver;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.Chart.ChartAreas.Add(chartArea1);
            this.Chart.ContextMenuStrip = this.chart_contextMenuStrip;
            this.Chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart.IsSoftShadows = false;
            this.Chart.Location = new System.Drawing.Point(68, 3);
            this.Chart.Name = "Chart";
            series1.BorderColor = System.Drawing.Color.Black;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series1.IsValueShownAsLabel = true;
            series1.LabelBackColor = System.Drawing.Color.Transparent;
            series1.Name = "Activity Totals";
            series1.ShadowColor = System.Drawing.Color.DimGray;
            series1.ShadowOffset = 4;
            this.Chart.Series.Add(series1);
            this.Chart.Size = new System.Drawing.Size(568, 332);
            this.Chart.TabIndex = 0;
            title1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "Title";
            title1.Text = "Graph Title";
            this.Chart.Titles.Add(title1);
            this.Chart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Chart_MouseDown);
            // 
            // chart_contextMenuStrip
            // 
            this.chart_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearFilter_ToolStripMenuItem,
            this.seriesFilter_ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.autoRefresh_ToolStripMenuItem});
            this.chart_contextMenuStrip.Name = "contextMenuStrip";
            this.chart_contextMenuStrip.Size = new System.Drawing.Size(143, 76);
            // 
            // clearFilter_ToolStripMenuItem
            // 
            this.clearFilter_ToolStripMenuItem.Enabled = false;
            this.clearFilter_ToolStripMenuItem.Name = "clearFilter_ToolStripMenuItem";
            this.clearFilter_ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.clearFilter_ToolStripMenuItem.Text = "Clear Filter";
            this.clearFilter_ToolStripMenuItem.Click += new System.EventHandler(this.clearFilter_ToolStripMenuItem_Click);
            // 
            // seriesFilter_ToolStripMenuItem
            // 
            this.seriesFilter_ToolStripMenuItem.Name = "seriesFilter_ToolStripMenuItem";
            this.seriesFilter_ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.seriesFilter_ToolStripMenuItem.Text = "Series Filter...";
            this.seriesFilter_ToolStripMenuItem.Click += new System.EventHandler(this.seriesFilter_ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(139, 6);
            // 
            // autoRefresh_ToolStripMenuItem
            // 
            this.autoRefresh_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshNone_ToolStripMenuItem,
            this.refresh1Minute_ToolStripMenuItem,
            this.refresh2Minutes_ToolStripMenuItem2,
            this.refresh5minutes_ToolStripMenuItem,
            this.refresh10Minutes_ToolStripMenuItem1});
            this.autoRefresh_ToolStripMenuItem.Name = "autoRefresh_ToolStripMenuItem";
            this.autoRefresh_ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.autoRefresh_ToolStripMenuItem.Text = "Auto Refresh";
            // 
            // refreshNone_ToolStripMenuItem
            // 
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
            // refresh2Minutes_ToolStripMenuItem2
            // 
            this.refresh2Minutes_ToolStripMenuItem2.Name = "refresh2Minutes_ToolStripMenuItem2";
            this.refresh2Minutes_ToolStripMenuItem2.Size = new System.Drawing.Size(132, 22);
            this.refresh2Minutes_ToolStripMenuItem2.Tag = "2";
            this.refresh2Minutes_ToolStripMenuItem2.Text = "2 minutes";
            this.refresh2Minutes_ToolStripMenuItem2.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh5minutes_ToolStripMenuItem
            // 
            this.refresh5minutes_ToolStripMenuItem.Name = "refresh5minutes_ToolStripMenuItem";
            this.refresh5minutes_ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.refresh5minutes_ToolStripMenuItem.Tag = "5";
            this.refresh5minutes_ToolStripMenuItem.Text = "5 minutes";
            this.refresh5minutes_ToolStripMenuItem.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh10Minutes_ToolStripMenuItem1
            // 
            this.refresh10Minutes_ToolStripMenuItem1.Name = "refresh10Minutes_ToolStripMenuItem1";
            this.refresh10Minutes_ToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.refresh10Minutes_ToolStripMenuItem1.Tag = "10";
            this.refresh10Minutes_ToolStripMenuItem1.Text = "10 minutes";
            this.refresh10Minutes_ToolStripMenuItem1.Click += new System.EventHandler(this.refreshByMinute_ToolStripMenuItem_Click);
            // 
            // refresh_Timer
            // 
            this.refresh_Timer.Tick += new System.EventHandler(this.refresh_Timer_Tick);
            // 
            // zoomX_radTrackBar
            // 
            this.zoomX_radTrackBar.BackColor = System.Drawing.Color.Transparent;
            this.zoomX_radTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomX_radTrackBar.ForeColor = System.Drawing.Color.Black;
            this.zoomX_radTrackBar.LabelStyle = Telerik.WinControls.UI.TrackBarLabelStyle.BottomRight;
            this.zoomX_radTrackBar.LargeTickFrequency = 1;
            this.zoomX_radTrackBar.Location = new System.Drawing.Point(3, 3);
            this.zoomX_radTrackBar.Minimum = 1F;
            this.zoomX_radTrackBar.Name = "zoomX_radTrackBar";
            this.zoomX_radTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // 
            // 
            this.zoomX_radTrackBar.RootElement.StretchHorizontally = false;
            this.zoomX_radTrackBar.RootElement.StretchVertically = true;
            this.zoomX_radTrackBar.Size = new System.Drawing.Size(59, 332);
            this.zoomX_radTrackBar.SmallTickFrequency = 0;
            this.zoomX_radTrackBar.TabIndex = 1;
            this.zoomX_radTrackBar.Text = "Zoom Level";
            this.zoomX_radTrackBar.TickStyle = Telerik.WinControls.Enumerations.TickStyles.BottomRight;
            this.zoomX_radTrackBar.Value = 1F;
            this.zoomX_radTrackBar.ValueChanged += new System.EventHandler(this.zoomX_radTrackBar_ValueChanged);
            this.zoomX_radTrackBar.LabelFormatting += new Telerik.WinControls.UI.LabelFormattingEventHandler(this.zoom_radTrackBar_LabelFormatting);
            // 
            // zoomY_radTrackBar
            // 
            this.zoomY_radTrackBar.BackColor = System.Drawing.Color.Transparent;
            this.zoomY_radTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomY_radTrackBar.ForeColor = System.Drawing.Color.Black;
            this.zoomY_radTrackBar.LabelStyle = Telerik.WinControls.UI.TrackBarLabelStyle.TopLeft;
            this.zoomY_radTrackBar.LargeTickFrequency = 1;
            this.zoomY_radTrackBar.Location = new System.Drawing.Point(68, 341);
            this.zoomY_radTrackBar.Minimum = 1F;
            this.zoomY_radTrackBar.Name = "zoomY_radTrackBar";
            this.zoomY_radTrackBar.Size = new System.Drawing.Size(568, 38);
            this.zoomY_radTrackBar.SmallTickFrequency = 0;
            this.zoomY_radTrackBar.TabIndex = 3;
            this.zoomY_radTrackBar.Text = "Zoom Level";
            this.zoomY_radTrackBar.TickStyle = Telerik.WinControls.Enumerations.TickStyles.TopLeft;
            this.zoomY_radTrackBar.Value = 1F;
            this.zoomY_radTrackBar.ValueChanged += new System.EventHandler(this.zoomY_radTrackBar_ValueChanged);
            this.zoomY_radTrackBar.LabelFormatting += new Telerik.WinControls.UI.LabelFormattingEventHandler(this.zoom_radTrackBar_LabelFormatting);
            // 
            // chartAndZoom_tableLayoutPanel
            // 
            this.chartAndZoom_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartAndZoom_tableLayoutPanel.ColumnCount = 2;
            this.chartAndZoom_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.18237F));
            this.chartAndZoom_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 89.81763F));
            this.chartAndZoom_tableLayoutPanel.Controls.Add(this.zoomY_radTrackBar, 1, 1);
            this.chartAndZoom_tableLayoutPanel.Controls.Add(this.zoomX_radTrackBar, 0, 0);
            this.chartAndZoom_tableLayoutPanel.Controls.Add(this.Chart, 1, 0);
            this.chartAndZoom_tableLayoutPanel.Controls.Add(this.zoom_label, 0, 1);
            this.chartAndZoom_tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.chartAndZoom_tableLayoutPanel.Name = "chartAndZoom_tableLayoutPanel";
            this.chartAndZoom_tableLayoutPanel.RowCount = 2;
            this.chartAndZoom_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.65031F));
            this.chartAndZoom_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.34969F));
            this.chartAndZoom_tableLayoutPanel.Size = new System.Drawing.Size(639, 382);
            this.chartAndZoom_tableLayoutPanel.TabIndex = 4;
            // 
            // zoom_label
            // 
            this.zoom_label.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zoom_label.AutoSize = true;
            this.zoom_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoom_label.Location = new System.Drawing.Point(9, 347);
            this.zoom_label.Name = "zoom_label";
            this.zoom_label.Size = new System.Drawing.Size(46, 26);
            this.zoom_label.TabIndex = 4;
            this.zoom_label.Text = "Adjust Zoom";
            // 
            // BarGraphBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chartAndZoom_tableLayoutPanel);
            this.Name = "BarGraphBase";
            this.Size = new System.Drawing.Size(677, 388);
            ((System.ComponentModel.ISupportInitialize)(this.Chart)).EndInit();
            this.chart_contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zoomX_radTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomY_radTrackBar)).EndInit();
            this.chartAndZoom_tableLayoutPanel.ResumeLayout(false);
            this.chartAndZoom_tableLayoutPanel.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem refresh2Minutes_ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem refresh5minutes_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refresh10Minutes_ToolStripMenuItem1;
        private System.Windows.Forms.Timer refresh_Timer;
        private Telerik.WinControls.UI.RadTrackBar zoomX_radTrackBar;
        private Telerik.WinControls.UI.RadTrackBar zoomY_radTrackBar;
        private System.Windows.Forms.TableLayoutPanel chartAndZoom_tableLayoutPanel;
        private System.Windows.Forms.Label zoom_label;
    }
}
