namespace HP.ScalableTest.Plugin.ePrintAdmin
{
    partial class ePrintAdminExecutionControl
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
            this.ePrintExecution_groupBox = new System.Windows.Forms.GroupBox();
            this.tasks_groupBox = new System.Windows.Forms.GroupBox();
            this.activityStatus_dataGridView = new System.Windows.Forms.DataGridView();
            this.operationColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ePrintExecution_groupBox.SuspendLayout();
            this.tasks_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.activityStatus_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ePrintExecution_groupBox
            // 
            this.ePrintExecution_groupBox.Controls.Add(this.tasks_groupBox);
            this.ePrintExecution_groupBox.Location = new System.Drawing.Point(23, 27);
            this.ePrintExecution_groupBox.Name = "ePrintExecution_groupBox";
            this.ePrintExecution_groupBox.Size = new System.Drawing.Size(523, 285);
            this.ePrintExecution_groupBox.TabIndex = 4;
            this.ePrintExecution_groupBox.TabStop = false;
            this.ePrintExecution_groupBox.Text = "ePrint Operations";
            // 
            // tasks_groupBox
            // 
            this.tasks_groupBox.Controls.Add(this.activityStatus_dataGridView);
            this.tasks_groupBox.Location = new System.Drawing.Point(17, 19);
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
            // ePrintAdminExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ePrintExecution_groupBox);
            this.Name = "ePrintAdminExecutionControl";
            this.Size = new System.Drawing.Size(685, 412);
            this.ePrintExecution_groupBox.ResumeLayout(false);
            this.tasks_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.activityStatus_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ePrintExecution_groupBox;
        private System.Windows.Forms.GroupBox tasks_groupBox;
        private System.Windows.Forms.DataGridView activityStatus_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn operationColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusColumn;
    }
}
