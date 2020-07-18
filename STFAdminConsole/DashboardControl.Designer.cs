namespace HP.ScalableTest.LabConsole
{
    partial class DashboardControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.summary_DataGrid = new System.Windows.Forms.DataGridView();
            this.grid_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gridCaption_Label = new System.Windows.Forms.Label();
            this.refresh_LinkLabel = new System.Windows.Forms.LinkLabel();
            this.chart_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.simulator_PieChart = new HP.ScalableTest.UI.Charting.PieChart();
            this.client_PieChart = new HP.ScalableTest.UI.Charting.PieChart();
            this.sessionId_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scenarioName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.owner_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dispatcher_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDate_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDate_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.runTime_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xpCount_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.w7Count_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.w8Count_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simCount_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.summary_DataGrid)).BeginInit();
            this.grid_TableLayoutPanel.SuspendLayout();
            this.chart_TableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // summary_DataGrid
            // 
            this.summary_DataGrid.AllowUserToAddRows = false;
            this.summary_DataGrid.AllowUserToDeleteRows = false;
            this.summary_DataGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Lavender;
            this.summary_DataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.summary_DataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.summary_DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.summary_DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sessionId_Column,
            this.scenarioName_Column,
            this.owner_Column,
            this.status_Column,
            this.dispatcher_Column,
            this.startDate_Column,
            this.endDate_Column,
            this.runTime_Column,
            this.xpCount_Column,
            this.w7Count_Column,
            this.w8Count_Column,
            this.simCount_Column});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.summary_DataGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.summary_DataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summary_DataGrid.EnableHeadersVisualStyles = false;
            this.summary_DataGrid.Location = new System.Drawing.Point(3, 25);
            this.summary_DataGrid.Name = "summary_DataGrid";
            this.summary_DataGrid.RowHeadersWidth = 20;
            this.summary_DataGrid.RowTemplate.Height = 22;
            this.summary_DataGrid.Size = new System.Drawing.Size(746, 172);
            this.summary_DataGrid.TabIndex = 0;
            // 
            // grid_TableLayoutPanel
            // 
            this.grid_TableLayoutPanel.ColumnCount = 1;
            this.grid_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grid_TableLayoutPanel.Controls.Add(this.summary_DataGrid, 0, 1);
            this.grid_TableLayoutPanel.Controls.Add(this.gridCaption_Label, 0, 0);
            this.grid_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grid_TableLayoutPanel.Location = new System.Drawing.Point(0, 382);
            this.grid_TableLayoutPanel.Name = "grid_TableLayoutPanel";
            this.grid_TableLayoutPanel.RowCount = 2;
            this.grid_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.grid_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grid_TableLayoutPanel.Size = new System.Drawing.Size(752, 200);
            this.grid_TableLayoutPanel.TabIndex = 1;
            // 
            // gridCaption_Label
            // 
            this.gridCaption_Label.BackColor = System.Drawing.Color.MediumBlue;
            this.gridCaption_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCaption_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridCaption_Label.ForeColor = System.Drawing.Color.White;
            this.gridCaption_Label.Location = new System.Drawing.Point(3, 0);
            this.gridCaption_Label.Name = "gridCaption_Label";
            this.gridCaption_Label.Size = new System.Drawing.Size(746, 22);
            this.gridCaption_Label.TabIndex = 1;
            this.gridCaption_Label.Text = "Development Environment Usage Summary";
            // 
            // refresh_LinkLabel
            // 
            this.refresh_LinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh_LinkLabel.AutoSize = true;
            this.refresh_LinkLabel.Location = new System.Drawing.Point(691, 0);
            this.refresh_LinkLabel.Name = "refresh_LinkLabel";
            this.refresh_LinkLabel.Size = new System.Drawing.Size(58, 13);
            this.refresh_LinkLabel.TabIndex = 3;
            this.refresh_LinkLabel.TabStop = true;
            this.refresh_LinkLabel.Text = "Refresh All";
            this.refresh_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refresh_LinkLabel_LinkClicked);
            // 
            // chart_TableLayoutPanel
            // 
            this.chart_TableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart_TableLayoutPanel.ColumnCount = 2;
            this.chart_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.chart_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.chart_TableLayoutPanel.Controls.Add(this.refresh_LinkLabel, 1, 0);
            this.chart_TableLayoutPanel.Controls.Add(this.simulator_PieChart, 1, 1);
            this.chart_TableLayoutPanel.Controls.Add(this.client_PieChart, 0, 1);
            this.chart_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.chart_TableLayoutPanel.Name = "chart_TableLayoutPanel";
            this.chart_TableLayoutPanel.RowCount = 2;
            this.chart_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.chart_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.chart_TableLayoutPanel.Size = new System.Drawing.Size(752, 379);
            this.chart_TableLayoutPanel.TabIndex = 4;
            // 
            // simulator_PieChart
            // 
            this.simulator_PieChart.Colors = null;
            this.simulator_PieChart.ColorWheel = null;
            this.simulator_PieChart.ConnectionString = null;
            this.simulator_PieChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simulator_PieChart.Location = new System.Drawing.Point(379, 23);
            this.simulator_PieChart.Name = "simulator_PieChart";
            this.simulator_PieChart.Size = new System.Drawing.Size(370, 353);
            this.simulator_PieChart.TabIndex = 4;
            this.simulator_PieChart.Title = "Simulator Usage";
            // 
            // client_PieChart
            // 
            this.client_PieChart.Colors = null;
            this.client_PieChart.ColorWheel = null;
            this.client_PieChart.ConnectionString = null;
            this.client_PieChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.client_PieChart.Location = new System.Drawing.Point(3, 23);
            this.client_PieChart.Name = "client_PieChart";
            this.client_PieChart.Size = new System.Drawing.Size(370, 353);
            this.client_PieChart.TabIndex = 5;
            this.client_PieChart.Title = "Client VM Usage";
            // 
            // sessionId_Column
            // 
            this.sessionId_Column.DataPropertyName = "SessionId";
            this.sessionId_Column.HeaderText = "Session ID";
            this.sessionId_Column.Name = "sessionId_Column";
            this.sessionId_Column.Width = 80;
            // 
            // scenarioName_Column
            // 
            this.scenarioName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.scenarioName_Column.DataPropertyName = "ScenarioName";
            this.scenarioName_Column.HeaderText = "Scenario Name";
            this.scenarioName_Column.Name = "scenarioName_Column";
            // 
            // owner_Column
            // 
            this.owner_Column.DataPropertyName = "Owner";
            this.owner_Column.HeaderText = "Owner";
            this.owner_Column.Name = "owner_Column";
            // 
            // status_Column
            // 
            this.status_Column.DataPropertyName = "Status";
            this.status_Column.HeaderText = "Status";
            this.status_Column.Name = "status_Column";
            // 
            // dispatcher_Column
            // 
            this.dispatcher_Column.DataPropertyName = "Dispatcher";
            this.dispatcher_Column.HeaderText = "Dispatcher";
            this.dispatcher_Column.Name = "dispatcher_Column";
            this.dispatcher_Column.Width = 85;
            // 
            // startDate_Column
            // 
            this.startDate_Column.DataPropertyName = "StartDate";
            this.startDate_Column.HeaderText = "Started";
            this.startDate_Column.Name = "startDate_Column";
            this.startDate_Column.Width = 120;
            // 
            // endDate_Column
            // 
            this.endDate_Column.DataPropertyName = "EndDate";
            this.endDate_Column.HeaderText = "Approx. End";
            this.endDate_Column.Name = "endDate_Column";
            this.endDate_Column.Width = 120;
            // 
            // runTime_Column
            // 
            this.runTime_Column.DataPropertyName = "TimeRunning";
            this.runTime_Column.HeaderText = "Run Time";
            this.runTime_Column.Name = "runTime_Column";
            // 
            // xpCount_Column
            // 
            this.xpCount_Column.DataPropertyName = "XPCount";
            this.xpCount_Column.HeaderText = "XP Count";
            this.xpCount_Column.Name = "xpCount_Column";
            this.xpCount_Column.Width = 60;
            // 
            // w7Count_Column
            // 
            this.w7Count_Column.DataPropertyName = "WSevCount";
            this.w7Count_Column.HeaderText = "Win7 Count";
            this.w7Count_Column.Name = "w7Count_Column";
            this.w7Count_Column.Width = 60;
            // 
            // w8Count_Column
            // 
            this.w8Count_Column.DataPropertyName = "W8Count";
            this.w8Count_Column.HeaderText = "Win8 Count";
            this.w8Count_Column.Name = "w8Count_Column";
            this.w8Count_Column.Width = 60;
            // 
            // simCount_Column
            // 
            this.simCount_Column.DataPropertyName = "SimulatorCount";
            this.simCount_Column.HeaderText = "Sim Count";
            this.simCount_Column.Name = "simCount_Column";
            this.simCount_Column.Width = 60;
            // 
            // DashboardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart_TableLayoutPanel);
            this.Controls.Add(this.grid_TableLayoutPanel);
            this.Name = "DashboardControl";
            this.Size = new System.Drawing.Size(752, 582);
            ((System.ComponentModel.ISupportInitialize)(this.summary_DataGrid)).EndInit();
            this.grid_TableLayoutPanel.ResumeLayout(false);
            this.chart_TableLayoutPanel.ResumeLayout(false);
            this.chart_TableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView summary_DataGrid;
        private System.Windows.Forms.TableLayoutPanel grid_TableLayoutPanel;
        private System.Windows.Forms.Label gridCaption_Label;
        private System.Windows.Forms.LinkLabel refresh_LinkLabel;
        private System.Windows.Forms.TableLayoutPanel chart_TableLayoutPanel;
        private UI.Charting.PieChart simulator_PieChart;
        private UI.Charting.PieChart client_PieChart;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionId_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn scenarioName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn owner_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn status_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn dispatcher_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDate_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDate_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn runTime_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn xpCount_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn w7Count_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn w8Count_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn simCount_Column;
    }
}
