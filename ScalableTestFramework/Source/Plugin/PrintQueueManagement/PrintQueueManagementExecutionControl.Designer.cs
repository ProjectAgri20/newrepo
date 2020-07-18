namespace HP.ScalableTest.Plugin.PrintQueueManagement
{
    partial class PrintQueueManagementExecutionControl
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
            this.pqm_groupBox = new System.Windows.Forms.GroupBox();
            this.tasks_groupBox = new System.Windows.Forms.GroupBox();
            this.activityStatus_dataGridView = new System.Windows.Forms.DataGridView();
            this.device_groupBox = new System.Windows.Forms.GroupBox();
            this.device_textBox = new System.Windows.Forms.TextBox();
            this.operationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pqm_groupBox.SuspendLayout();
            this.tasks_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activityStatus_dataGridView)).BeginInit();
            this.device_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pqm_groupBox
            // 
            this.pqm_groupBox.Controls.Add(this.tasks_groupBox);
            this.pqm_groupBox.Controls.Add(this.device_groupBox);
            this.pqm_groupBox.Location = new System.Drawing.Point(19, 21);
            this.pqm_groupBox.Name = "pqm_groupBox";
            this.pqm_groupBox.Size = new System.Drawing.Size(521, 351);
            this.pqm_groupBox.TabIndex = 0;
            this.pqm_groupBox.TabStop = false;
            this.pqm_groupBox.Text = "Print Queue Management";
            // 
            // tasks_groupBox
            // 
            this.tasks_groupBox.Controls.Add(this.activityStatus_dataGridView);
            this.tasks_groupBox.Location = new System.Drawing.Point(30, 129);
            this.tasks_groupBox.Name = "tasks_groupBox";
            this.tasks_groupBox.Size = new System.Drawing.Size(456, 196);
            this.tasks_groupBox.TabIndex = 2;
            this.tasks_groupBox.TabStop = false;
            this.tasks_groupBox.Text = "Activity Status";
            // 
            // activityStatus_dataGridView
            // 
            this.activityStatus_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.activityStatus_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.operationColumn,
            this.descriptionColumn,
            this.statusColumn});
            this.activityStatus_dataGridView.Location = new System.Drawing.Point(13, 19);
            this.activityStatus_dataGridView.Name = "activityStatus_dataGridView";
            this.activityStatus_dataGridView.RowHeadersVisible = false;
            this.activityStatus_dataGridView.Size = new System.Drawing.Size(429, 160);
            this.activityStatus_dataGridView.TabIndex = 0;
            // 
            // device_groupBox
            // 
            this.device_groupBox.Controls.Add(this.device_textBox);
            this.device_groupBox.Location = new System.Drawing.Point(30, 33);
            this.device_groupBox.Name = "device_groupBox";
            this.device_groupBox.Size = new System.Drawing.Size(456, 67);
            this.device_groupBox.TabIndex = 1;
            this.device_groupBox.TabStop = false;
            this.device_groupBox.Text = "Printer";
            // 
            // device_textBox
            // 
            this.device_textBox.Location = new System.Drawing.Point(13, 29);
            this.device_textBox.Name = "device_textBox";
            this.device_textBox.Size = new System.Drawing.Size(429, 20);
            this.device_textBox.TabIndex = 0;
            // 
            // operationColumn
            // 
            this.operationColumn.DataPropertyName = "Operation";
            this.operationColumn.HeaderText = "Activity";
            this.operationColumn.Name = "operationColumn";
            this.operationColumn.ReadOnly = true;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.DataPropertyName = "Description";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.ReadOnly = true;
            this.descriptionColumn.Width = 225;
            // 
            // statusColumn
            // 
            this.statusColumn.DataPropertyName = "Status";
            this.statusColumn.HeaderText = "Status";
            this.statusColumn.Name = "statusColumn";
            this.statusColumn.ReadOnly = true;
            // 
            // PrintQueueManagementExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pqm_groupBox);
            this.Name = "PrintQueueManagementExecutionControl";
            this.Size = new System.Drawing.Size(554, 389);
            this.pqm_groupBox.ResumeLayout(false);
            this.tasks_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.activityStatus_dataGridView)).EndInit();
            this.device_groupBox.ResumeLayout(false);
            this.device_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pqm_groupBox;
        private System.Windows.Forms.GroupBox device_groupBox;
        private System.Windows.Forms.TextBox device_textBox;
        private System.Windows.Forms.GroupBox tasks_groupBox;
        private System.Windows.Forms.DataGridView activityStatus_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn operationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusColumn;
    }
}
