namespace HP.ScalableTest.Print.Utility
{
    partial class MainEditControl
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
                
                if (_manager != null)
                {
                    _manager.Dispose();
                }

                if (_upgradeForm != null)
                {
                    _upgradeForm.Dispose();
                }
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.start_button = new System.Windows.Forms.Button();
            this.queues_DataGridView = new System.Windows.Forms.DataGridView();
            this.Progress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.queueNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.queueTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.useConfigurationFileDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.configurationFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.snmpEnabledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sharedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clientRenderDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printQueueInstallDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.queues_GroupBox = new System.Windows.Forms.GroupBox();
            this.clearSelected_Button = new System.Windows.Forms.Button();
            this.addQueue_Button = new System.Windows.Forms.Button();
            this.queueCount_Label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.abort_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueueInstallDataBindingSource)).BeginInit();
            this.queues_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // start_button
            // 
            this.start_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.start_button.Location = new System.Drawing.Point(705, 421);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(130, 23);
            this.start_button.TabIndex = 7;
            this.start_button.Text = "Install Queues";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // queues_DataGridView
            // 
            this.queues_DataGridView.AllowUserToAddRows = false;
            this.queues_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.queues_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.queues_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queues_DataGridView.AutoGenerateColumns = false;
            this.queues_DataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.queues_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.queues_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.queues_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.queues_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Progress,
            this.queueNameDataGridViewTextBoxColumn,
            this.driverNameDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.addressDataGridViewTextBoxColumn,
            this.portDataGridViewTextBoxColumn,
            this.queueTypeDataGridViewTextBoxColumn,
            this.useConfigurationFileDataGridViewCheckBoxColumn,
            this.configurationFileDataGridViewTextBoxColumn,
            this.snmpEnabledDataGridViewCheckBoxColumn,
            this.sharedDataGridViewCheckBoxColumn,
            this.clientRenderDataGridViewCheckBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.queues_DataGridView.DataSource = this.printQueueInstallDataBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.queues_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.queues_DataGridView.Location = new System.Drawing.Point(6, 34);
            this.queues_DataGridView.Name = "queues_DataGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.queues_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.queues_DataGridView.RowHeadersVisible = false;
            this.queues_DataGridView.RowHeadersWidth = 30;
            this.queues_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.queues_DataGridView.Size = new System.Drawing.Size(965, 381);
            this.queues_DataGridView.TabIndex = 2;
            this.queues_DataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.queues_DataGridView_CellContentClick);
            this.queues_DataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.queues_DataGridView_RowsRemoved);
            // 
            // Progress
            // 
            this.Progress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Progress.DataPropertyName = "Progress";
            this.Progress.HeaderText = "Progress";
            this.Progress.MinimumWidth = 50;
            this.Progress.Name = "Progress";
            this.Progress.ReadOnly = true;
            this.Progress.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Progress.Width = 73;
            // 
            // queueNameDataGridViewTextBoxColumn
            // 
            this.queueNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.queueNameDataGridViewTextBoxColumn.DataPropertyName = "QueueName";
            this.queueNameDataGridViewTextBoxColumn.HeaderText = "Queue";
            this.queueNameDataGridViewTextBoxColumn.Name = "queueNameDataGridViewTextBoxColumn";
            this.queueNameDataGridViewTextBoxColumn.Width = 64;
            // 
            // driverNameDataGridViewTextBoxColumn
            // 
            this.driverNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.driverNameDataGridViewTextBoxColumn.DataPropertyName = "DriverName";
            this.driverNameDataGridViewTextBoxColumn.HeaderText = "Driver";
            this.driverNameDataGridViewTextBoxColumn.Name = "driverNameDataGridViewTextBoxColumn";
            this.driverNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverNameDataGridViewTextBoxColumn.Width = 60;
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset Id";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Width = 70;
            // 
            // addressDataGridViewTextBoxColumn
            // 
            this.addressDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.addressDataGridViewTextBoxColumn.DataPropertyName = "Address";
            this.addressDataGridViewTextBoxColumn.HeaderText = "Address";
            this.addressDataGridViewTextBoxColumn.Name = "addressDataGridViewTextBoxColumn";
            this.addressDataGridViewTextBoxColumn.Width = 70;
            // 
            // portDataGridViewTextBoxColumn
            // 
            this.portDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.portDataGridViewTextBoxColumn.DataPropertyName = "Port";
            this.portDataGridViewTextBoxColumn.HeaderText = "Port";
            this.portDataGridViewTextBoxColumn.Name = "portDataGridViewTextBoxColumn";
            this.portDataGridViewTextBoxColumn.Width = 51;
            // 
            // queueTypeDataGridViewTextBoxColumn
            // 
            this.queueTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.queueTypeDataGridViewTextBoxColumn.DataPropertyName = "QueueType";
            this.queueTypeDataGridViewTextBoxColumn.HeaderText = "Queue Type";
            this.queueTypeDataGridViewTextBoxColumn.Name = "queueTypeDataGridViewTextBoxColumn";
            this.queueTypeDataGridViewTextBoxColumn.Width = 91;
            // 
            // useConfigurationFileDataGridViewCheckBoxColumn
            // 
            this.useConfigurationFileDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.useConfigurationFileDataGridViewCheckBoxColumn.DataPropertyName = "UseConfigurationFile";
            this.useConfigurationFileDataGridViewCheckBoxColumn.HeaderText = "CFM";
            this.useConfigurationFileDataGridViewCheckBoxColumn.Name = "useConfigurationFileDataGridViewCheckBoxColumn";
            this.useConfigurationFileDataGridViewCheckBoxColumn.Width = 35;
            // 
            // configurationFileDataGridViewTextBoxColumn
            // 
            this.configurationFileDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.configurationFileDataGridViewTextBoxColumn.DataPropertyName = "ConfigurationFile";
            this.configurationFileDataGridViewTextBoxColumn.HeaderText = "CFM File";
            this.configurationFileDataGridViewTextBoxColumn.Name = "configurationFileDataGridViewTextBoxColumn";
            this.configurationFileDataGridViewTextBoxColumn.ReadOnly = true;
            this.configurationFileDataGridViewTextBoxColumn.Width = 73;
            // 
            // snmpEnabledDataGridViewCheckBoxColumn
            // 
            this.snmpEnabledDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.snmpEnabledDataGridViewCheckBoxColumn.DataPropertyName = "SnmpEnabled";
            this.snmpEnabledDataGridViewCheckBoxColumn.HeaderText = "SNMP";
            this.snmpEnabledDataGridViewCheckBoxColumn.Name = "snmpEnabledDataGridViewCheckBoxColumn";
            this.snmpEnabledDataGridViewCheckBoxColumn.Width = 44;
            // 
            // sharedDataGridViewCheckBoxColumn
            // 
            this.sharedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sharedDataGridViewCheckBoxColumn.DataPropertyName = "Shared";
            this.sharedDataGridViewCheckBoxColumn.HeaderText = "Shared";
            this.sharedDataGridViewCheckBoxColumn.Name = "sharedDataGridViewCheckBoxColumn";
            this.sharedDataGridViewCheckBoxColumn.Width = 47;
            // 
            // clientRenderDataGridViewCheckBoxColumn
            // 
            this.clientRenderDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clientRenderDataGridViewCheckBoxColumn.DataPropertyName = "ClientRender";
            this.clientRenderDataGridViewCheckBoxColumn.HeaderText = "Client";
            this.clientRenderDataGridViewCheckBoxColumn.Name = "clientRenderDataGridViewCheckBoxColumn";
            this.clientRenderDataGridViewCheckBoxColumn.Width = 39;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // printQueueInstallDataBindingSource
            // 
            this.printQueueInstallDataBindingSource.DataSource = typeof(HP.ScalableTest.Print.Utility.QueueInstallationData);
            // 
            // queues_GroupBox
            // 
            this.queues_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queues_GroupBox.Controls.Add(this.clearSelected_Button);
            this.queues_GroupBox.Controls.Add(this.addQueue_Button);
            this.queues_GroupBox.Controls.Add(this.queueCount_Label);
            this.queues_GroupBox.Controls.Add(this.label2);
            this.queues_GroupBox.Controls.Add(this.queues_DataGridView);
            this.queues_GroupBox.Controls.Add(this.start_button);
            this.queues_GroupBox.Controls.Add(this.abort_Button);
            this.queues_GroupBox.Location = new System.Drawing.Point(12, 15);
            this.queues_GroupBox.Name = "queues_GroupBox";
            this.queues_GroupBox.Size = new System.Drawing.Size(977, 450);
            this.queues_GroupBox.TabIndex = 28;
            this.queues_GroupBox.TabStop = false;
            this.queues_GroupBox.Text = "Print Queue Definitions";
            // 
            // clearSelected_Button
            // 
            this.clearSelected_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearSelected_Button.Location = new System.Drawing.Point(142, 421);
            this.clearSelected_Button.Name = "clearSelected_Button";
            this.clearSelected_Button.Size = new System.Drawing.Size(130, 23);
            this.clearSelected_Button.TabIndex = 49;
            this.clearSelected_Button.Text = "Clear Definitions";
            this.clearSelected_Button.UseVisualStyleBackColor = true;
            this.clearSelected_Button.Click += new System.EventHandler(this.clearSelected_Button_Click);
            // 
            // addQueue_Button
            // 
            this.addQueue_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addQueue_Button.Location = new System.Drawing.Point(6, 421);
            this.addQueue_Button.Name = "addQueue_Button";
            this.addQueue_Button.Size = new System.Drawing.Size(130, 23);
            this.addQueue_Button.TabIndex = 48;
            this.addQueue_Button.Text = "Add Definitions";
            this.addQueue_Button.UseVisualStyleBackColor = true;
            this.addQueue_Button.Click += new System.EventHandler(this.addQueue_Button_Click);
            // 
            // queueCount_Label
            // 
            this.queueCount_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.queueCount_Label.AutoSize = true;
            this.queueCount_Label.Location = new System.Drawing.Point(905, 16);
            this.queueCount_Label.Name = "queueCount_Label";
            this.queueCount_Label.Size = new System.Drawing.Size(66, 13);
            this.queueCount_Label.TabIndex = 47;
            this.queueCount_Label.Text = "0000 / 0000";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(812, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Queues Created:";
            // 
            // abort_Button
            // 
            this.abort_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abort_Button.Location = new System.Drawing.Point(841, 421);
            this.abort_Button.Name = "abort_Button";
            this.abort_Button.Size = new System.Drawing.Size(130, 23);
            this.abort_Button.TabIndex = 8;
            this.abort_Button.Text = "Cancel Installation";
            this.abort_Button.UseVisualStyleBackColor = true;
            this.abort_Button.Click += new System.EventHandler(this.abort_Button_Click);
            // 
            // MainEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queues_GroupBox);
            this.Name = "MainEditControl";
            this.Size = new System.Drawing.Size(1002, 478);
            this.Load += new System.EventHandler(this.MainEditControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.queues_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printQueueInstallDataBindingSource)).EndInit();
            this.queues_GroupBox.ResumeLayout(false);
            this.queues_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.DataGridView queues_DataGridView;
        private System.Windows.Forms.GroupBox queues_GroupBox;
        private System.Windows.Forms.Button abort_Button;
        private System.Windows.Forms.Label queueCount_Label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button clearSelected_Button;
        private System.Windows.Forms.Button addQueue_Button;
        private System.Windows.Forms.BindingSource printQueueInstallDataBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Progress;
        private System.Windows.Forms.DataGridViewTextBoxColumn queueNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn addressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn queueTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn useConfigurationFileDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn configurationFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn snmpEnabledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn sharedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clientRenderDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
    }
}
