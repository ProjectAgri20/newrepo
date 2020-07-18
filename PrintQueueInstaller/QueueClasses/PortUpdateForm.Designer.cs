namespace HP.ScalableTest.Print.Utility
{
    partial class PortUpdateForm
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
            this.ports_GroupBox = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.update_Button = new System.Windows.Forms.Button();
            this.close_Button = new System.Windows.Forms.Button();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selected_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.queue_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portAddress_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newPortAddress_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portNumber_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ports_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ports_GroupBox
            // 
            this.ports_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ports_GroupBox.Controls.Add(this.dataGridView);
            this.ports_GroupBox.Controls.Add(this.update_Button);
            this.ports_GroupBox.Location = new System.Drawing.Point(5, 7);
            this.ports_GroupBox.Name = "ports_GroupBox";
            this.ports_GroupBox.Size = new System.Drawing.Size(668, 259);
            this.ports_GroupBox.TabIndex = 3;
            this.ports_GroupBox.TabStop = false;
            this.ports_GroupBox.Text = "Port Settings";
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selected_Column,
            this.queue_Column,
            this.portName_Column,
            this.portAddress_Column,
            this.newPortAddress_Column,
            this.portNumber_Column});
            this.dataGridView.Location = new System.Drawing.Point(7, 19);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(655, 205);
            this.dataGridView.TabIndex = 5;
            // 
            // update_Button
            // 
            this.update_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.update_Button.Location = new System.Drawing.Point(550, 230);
            this.update_Button.Name = "update_Button";
            this.update_Button.Size = new System.Drawing.Size(113, 23);
            this.update_Button.TabIndex = 0;
            this.update_Button.Text = "Update Selected";
            this.update_Button.UseVisualStyleBackColor = true;
            this.update_Button.Click += new System.EventHandler(this.update_Button_Click);
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(603, 272);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 1;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.IndeterminateValue = "False";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "QueueName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Queue Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PortName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Port Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 130;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "PortAddress";
            this.dataGridViewTextBoxColumn3.HeaderText = "Port Address";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // selected_Column
            // 
            this.selected_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.selected_Column.HeaderText = "";
            this.selected_Column.IndeterminateValue = "False";
            this.selected_Column.Name = "selected_Column";
            this.selected_Column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.selected_Column.Width = 30;
            // 
            // queue_Column
            // 
            this.queue_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.queue_Column.DataPropertyName = "QueueName";
            this.queue_Column.HeaderText = "Queue Name";
            this.queue_Column.Name = "queue_Column";
            this.queue_Column.ReadOnly = true;
            // 
            // portName_Column
            // 
            this.portName_Column.DataPropertyName = "PortName";
            this.portName_Column.HeaderText = "Port Name";
            this.portName_Column.Name = "portName_Column";
            this.portName_Column.ReadOnly = true;
            this.portName_Column.Width = 130;
            // 
            // portAddress_Column
            // 
            this.portAddress_Column.DataPropertyName = "PortAddress";
            this.portAddress_Column.HeaderText = "Port Address";
            this.portAddress_Column.Name = "portAddress_Column";
            this.portAddress_Column.ReadOnly = true;
            this.portAddress_Column.Width = 120;
            // 
            // newPortAddress_Column
            // 
            this.newPortAddress_Column.DataPropertyName = "NewPortAddress";
            this.newPortAddress_Column.HeaderText = "New Address";
            this.newPortAddress_Column.Name = "newPortAddress_Column";
            this.newPortAddress_Column.Width = 120;
            // 
            // portNumber_Column
            // 
            this.portNumber_Column.DataPropertyName = "PortNumber";
            this.portNumber_Column.HeaderText = "Port Number";
            this.portNumber_Column.Name = "portNumber_Column";
            // 
            // PortUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(680, 297);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.ports_GroupBox);
            this.Name = "PortUpdateForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Print Queue Port Address";
            this.ports_GroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ports_GroupBox;
        private System.Windows.Forms.Button update_Button;
        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn queue_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn portName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn portAddress_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn newPortAddress_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn portNumber_Column;
    }
}