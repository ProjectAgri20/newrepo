namespace HP.ScalableTest.LabConsole
{
    partial class PrintQueueRefreshForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.printQueue_DataGridView = new System.Windows.Forms.DataGridView();
            this.select_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.queueName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scenarioName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.instructions_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // printQueue_DataGridView
            // 
            this.printQueue_DataGridView.AllowUserToAddRows = false;
            this.printQueue_DataGridView.AllowUserToDeleteRows = false;
            this.printQueue_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printQueue_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.printQueue_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select_Column,
            this.queueName_Column,
            this.scenarioName_Column});
            this.printQueue_DataGridView.Location = new System.Drawing.Point(12, 89);
            this.printQueue_DataGridView.MultiSelect = false;
            this.printQueue_DataGridView.Name = "printQueue_DataGridView";
            this.printQueue_DataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.printQueue_DataGridView.RowHeadersVisible = false;
            this.printQueue_DataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.printQueue_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.printQueue_DataGridView.ShowEditingIcon = false;
            this.printQueue_DataGridView.Size = new System.Drawing.Size(417, 217);
            this.printQueue_DataGridView.TabIndex = 0;
            // 
            // select_Column
            // 
            this.select_Column.HeaderText = "Select";
            this.select_Column.Name = "select_Column";
            this.select_Column.Width = 50;
            // 
            // queueName_Column
            // 
            this.queueName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.queueName_Column.HeaderText = "Queue Name";
            this.queueName_Column.Name = "queueName_Column";
            this.queueName_Column.ReadOnly = true;
            // 
            // scenarioName_Column
            // 
            this.scenarioName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.scenarioName_Column.HeaderText = "Scenario Name";
            this.scenarioName_Column.Name = "scenarioName_Column";
            this.scenarioName_Column.ReadOnly = true;
            this.scenarioName_Column.Visible = false;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(249, 312);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(87, 27);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(342, 312);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(87, 27);
            this.cancel_Button.TabIndex = 1;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // instructions_Label
            // 
            this.instructions_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instructions_Label.Location = new System.Drawing.Point(0, 0);
            this.instructions_Label.Name = "instructions_Label";
            this.instructions_Label.Padding = new System.Windows.Forms.Padding(8, 15, 0, 0);
            this.instructions_Label.Size = new System.Drawing.Size(441, 351);
            this.instructions_Label.TabIndex = 2;
            this.instructions_Label.Text = "Instructions";
            // 
            // PrintQueueRefreshForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(441, 351);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.printQueue_DataGridView);
            this.Controls.Add(this.instructions_Label);
            this.Name = "PrintQueueRefreshForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Queue Refresh Form";
            ((System.ComponentModel.ISupportInitialize)(this.printQueue_DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView printQueue_DataGridView;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label instructions_Label;
        private System.Windows.Forms.DataGridViewCheckBoxColumn select_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn queueName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn scenarioName_Column;
    }
}