namespace HP.ScalableTest.Print.Utility
{
    partial class InboxDriverSelectionForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.inboxDriver_DataGridView = new System.Windows.Forms.DataGridView();
            this.Ok_Button = new System.Windows.Forms.Button();
            this.reload_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.viewInf_Button = new System.Windows.Forms.Button();
            this.driversFound_Label = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printDriverPropertiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.providerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.versionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printProcessorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.architectureDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verifyPdlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.releaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.configurationFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.helpFileDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.monitorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.inboxDriver_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverPropertiesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // inboxDriver_DataGridView
            // 
            this.inboxDriver_DataGridView.AllowUserToAddRows = false;
            this.inboxDriver_DataGridView.AllowUserToDeleteRows = false;
            this.inboxDriver_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.inboxDriver_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.inboxDriver_DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inboxDriver_DataGridView.AutoGenerateColumns = false;
            this.inboxDriver_DataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.inboxDriver_DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.inboxDriver_DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.inboxDriver_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inboxDriver_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.providerDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.versionDataGridViewTextBoxColumn,
            this.driverDateDataGridViewTextBoxColumn,
            this.printProcessorDataGridViewTextBoxColumn,
            this.idDataGridViewTextBoxColumn,
            this.architectureDataGridViewTextBoxColumn,
            this.driverTypeDataGridViewTextBoxColumn,
            this.verifyPdlDataGridViewTextBoxColumn,
            this.releaseDataGridViewTextBoxColumn,
            this.driverDataGridViewTextBoxColumn,
            this.configurationFileDataGridViewTextBoxColumn,
            this.dataFileDataGridViewTextBoxColumn,
            this.infPathDataGridViewTextBoxColumn,
            this.helpFileDataGridViewTextBoxColumn,
            this.locationDataGridViewTextBoxColumn,
            this.monitorDataGridViewTextBoxColumn});
            this.inboxDriver_DataGridView.DataSource = this.printDriverPropertiesBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.inboxDriver_DataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.inboxDriver_DataGridView.Location = new System.Drawing.Point(12, 30);
            this.inboxDriver_DataGridView.MultiSelect = false;
            this.inboxDriver_DataGridView.Name = "inboxDriver_DataGridView";
            this.inboxDriver_DataGridView.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.inboxDriver_DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.inboxDriver_DataGridView.RowHeadersVisible = false;
            this.inboxDriver_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inboxDriver_DataGridView.Size = new System.Drawing.Size(961, 437);
            this.inboxDriver_DataGridView.TabIndex = 0;
            this.inboxDriver_DataGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.inboxDriver_DataGridView_CellEnter);
            this.inboxDriver_DataGridView.DoubleClick += new System.EventHandler(this.inboxDriver_DataGridView_DoubleClick);
            // 
            // Ok_Button
            // 
            this.Ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok_Button.Location = new System.Drawing.Point(817, 473);
            this.Ok_Button.Name = "Ok_Button";
            this.Ok_Button.Size = new System.Drawing.Size(75, 23);
            this.Ok_Button.TabIndex = 1;
            this.Ok_Button.Text = "OK";
            this.Ok_Button.UseVisualStyleBackColor = true;
            this.Ok_Button.Click += new System.EventHandler(this.Ok_Button_Click);
            // 
            // reload_Button
            // 
            this.reload_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reload_Button.Location = new System.Drawing.Point(12, 473);
            this.reload_Button.Name = "reload_Button";
            this.reload_Button.Size = new System.Drawing.Size(75, 23);
            this.reload_Button.TabIndex = 2;
            this.reload_Button.Text = "Reload";
            this.reload_Button.UseVisualStyleBackColor = true;
            this.reload_Button.Click += new System.EventHandler(this.reload_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(898, 473);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // viewInf_Button
            // 
            this.viewInf_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.viewInf_Button.Location = new System.Drawing.Point(93, 473);
            this.viewInf_Button.Name = "viewInf_Button";
            this.viewInf_Button.Size = new System.Drawing.Size(75, 23);
            this.viewInf_Button.TabIndex = 4;
            this.viewInf_Button.Text = "View INF";
            this.viewInf_Button.UseVisualStyleBackColor = true;
            this.viewInf_Button.Click += new System.EventHandler(this.viewInf_Button_Click);
            // 
            // driversFound_Label
            // 
            this.driversFound_Label.AutoSize = true;
            this.driversFound_Label.Location = new System.Drawing.Point(12, 14);
            this.driversFound_Label.Name = "driversFound_Label";
            this.driversFound_Label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.driversFound_Label.Size = new System.Drawing.Size(0, 13);
            this.driversFound_Label.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Version";
            this.dataGridViewTextBoxColumn1.HeaderText = "Version";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 67;
            // 
            // printDriverPropertiesBindingSource
            // 
            this.printDriverPropertiesBindingSource.DataSource = typeof(HP.ScalableTest.Print.Utility.PrintDeviceDriver);
            // 
            // providerDataGridViewTextBoxColumn
            // 
            this.providerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.providerDataGridViewTextBoxColumn.DataPropertyName = "Provider";
            this.providerDataGridViewTextBoxColumn.HeaderText = "Provider";
            this.providerDataGridViewTextBoxColumn.Name = "providerDataGridViewTextBoxColumn";
            this.providerDataGridViewTextBoxColumn.ReadOnly = true;
            this.providerDataGridViewTextBoxColumn.Width = 71;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // versionDataGridViewTextBoxColumn
            // 
            this.versionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.versionDataGridViewTextBoxColumn.DataPropertyName = "Version";
            this.versionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.versionDataGridViewTextBoxColumn.Name = "versionDataGridViewTextBoxColumn";
            this.versionDataGridViewTextBoxColumn.ReadOnly = true;
            this.versionDataGridViewTextBoxColumn.Width = 67;
            // 
            // driverDateDataGridViewTextBoxColumn
            // 
            this.driverDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.driverDateDataGridViewTextBoxColumn.DataPropertyName = "DriverDate";
            this.driverDateDataGridViewTextBoxColumn.HeaderText = "Date";
            this.driverDateDataGridViewTextBoxColumn.Name = "driverDateDataGridViewTextBoxColumn";
            this.driverDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverDateDataGridViewTextBoxColumn.Width = 55;
            // 
            // printProcessorDataGridViewTextBoxColumn
            // 
            this.printProcessorDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.printProcessorDataGridViewTextBoxColumn.DataPropertyName = "PrintProcessor";
            this.printProcessorDataGridViewTextBoxColumn.HeaderText = "Processor";
            this.printProcessorDataGridViewTextBoxColumn.Name = "printProcessorDataGridViewTextBoxColumn";
            this.printProcessorDataGridViewTextBoxColumn.ReadOnly = true;
            this.printProcessorDataGridViewTextBoxColumn.Width = 79;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // architectureDataGridViewTextBoxColumn
            // 
            this.architectureDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.architectureDataGridViewTextBoxColumn.DataPropertyName = "Architecture";
            this.architectureDataGridViewTextBoxColumn.HeaderText = "Architecture";
            this.architectureDataGridViewTextBoxColumn.Name = "architectureDataGridViewTextBoxColumn";
            this.architectureDataGridViewTextBoxColumn.ReadOnly = true;
            this.architectureDataGridViewTextBoxColumn.Width = 89;
            // 
            // driverTypeDataGridViewTextBoxColumn
            // 
            this.driverTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.driverTypeDataGridViewTextBoxColumn.DataPropertyName = "DriverType";
            this.driverTypeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.driverTypeDataGridViewTextBoxColumn.Name = "driverTypeDataGridViewTextBoxColumn";
            this.driverTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverTypeDataGridViewTextBoxColumn.Width = 56;
            // 
            // verifyPdlDataGridViewTextBoxColumn
            // 
            this.verifyPdlDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.verifyPdlDataGridViewTextBoxColumn.DataPropertyName = "VerifyPdl";
            this.verifyPdlDataGridViewTextBoxColumn.HeaderText = "PDL";
            this.verifyPdlDataGridViewTextBoxColumn.Name = "verifyPdlDataGridViewTextBoxColumn";
            this.verifyPdlDataGridViewTextBoxColumn.ReadOnly = true;
            this.verifyPdlDataGridViewTextBoxColumn.Width = 53;
            // 
            // releaseDataGridViewTextBoxColumn
            // 
            this.releaseDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.releaseDataGridViewTextBoxColumn.DataPropertyName = "Release";
            this.releaseDataGridViewTextBoxColumn.HeaderText = "Release";
            this.releaseDataGridViewTextBoxColumn.Name = "releaseDataGridViewTextBoxColumn";
            this.releaseDataGridViewTextBoxColumn.ReadOnly = true;
            this.releaseDataGridViewTextBoxColumn.Visible = false;
            this.releaseDataGridViewTextBoxColumn.Width = 71;
            // 
            // driverDataGridViewTextBoxColumn
            // 
            this.driverDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.driverDataGridViewTextBoxColumn.DataPropertyName = "Driver";
            this.driverDataGridViewTextBoxColumn.HeaderText = "Driver";
            this.driverDataGridViewTextBoxColumn.Name = "driverDataGridViewTextBoxColumn";
            this.driverDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverDataGridViewTextBoxColumn.Visible = false;
            this.driverDataGridViewTextBoxColumn.Width = 60;
            // 
            // configurationFileDataGridViewTextBoxColumn
            // 
            this.configurationFileDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.configurationFileDataGridViewTextBoxColumn.DataPropertyName = "ConfigurationFile";
            this.configurationFileDataGridViewTextBoxColumn.HeaderText = "Configuration File";
            this.configurationFileDataGridViewTextBoxColumn.Name = "configurationFileDataGridViewTextBoxColumn";
            this.configurationFileDataGridViewTextBoxColumn.ReadOnly = true;
            this.configurationFileDataGridViewTextBoxColumn.Visible = false;
            this.configurationFileDataGridViewTextBoxColumn.Width = 113;
            // 
            // dataFileDataGridViewTextBoxColumn
            // 
            this.dataFileDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataFileDataGridViewTextBoxColumn.DataPropertyName = "DataFile";
            this.dataFileDataGridViewTextBoxColumn.HeaderText = "Data File";
            this.dataFileDataGridViewTextBoxColumn.Name = "dataFileDataGridViewTextBoxColumn";
            this.dataFileDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // infPathDataGridViewTextBoxColumn
            // 
            this.infPathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.infPathDataGridViewTextBoxColumn.DataPropertyName = "InfPath";
            this.infPathDataGridViewTextBoxColumn.HeaderText = "INF Path";
            this.infPathDataGridViewTextBoxColumn.Name = "infPathDataGridViewTextBoxColumn";
            this.infPathDataGridViewTextBoxColumn.ReadOnly = true;
            this.infPathDataGridViewTextBoxColumn.Visible = false;
            // 
            // helpFileDataGridViewTextBoxColumn
            // 
            this.helpFileDataGridViewTextBoxColumn.DataPropertyName = "HelpFile";
            this.helpFileDataGridViewTextBoxColumn.HeaderText = "Help File";
            this.helpFileDataGridViewTextBoxColumn.Name = "helpFileDataGridViewTextBoxColumn";
            this.helpFileDataGridViewTextBoxColumn.ReadOnly = true;
            this.helpFileDataGridViewTextBoxColumn.Visible = false;
            // 
            // locationDataGridViewTextBoxColumn
            // 
            this.locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            this.locationDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            this.locationDataGridViewTextBoxColumn.ReadOnly = true;
            this.locationDataGridViewTextBoxColumn.Visible = false;
            // 
            // monitorDataGridViewTextBoxColumn
            // 
            this.monitorDataGridViewTextBoxColumn.DataPropertyName = "Monitor";
            this.monitorDataGridViewTextBoxColumn.HeaderText = "Monitor";
            this.monitorDataGridViewTextBoxColumn.Name = "monitorDataGridViewTextBoxColumn";
            this.monitorDataGridViewTextBoxColumn.ReadOnly = true;
            this.monitorDataGridViewTextBoxColumn.Visible = false;
            // 
            // InboxDriverSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 508);
            this.Controls.Add(this.driversFound_Label);
            this.Controls.Add(this.viewInf_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.reload_Button);
            this.Controls.Add(this.Ok_Button);
            this.Controls.Add(this.inboxDriver_DataGridView);
            this.Name = "InboxDriverSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "In-box Driver Selection";
            this.Load += new System.EventHandler(this.InboxDriverSelectionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxDriver_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printDriverPropertiesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView inboxDriver_DataGridView;
        private System.Windows.Forms.Button Ok_Button;
        private System.Windows.Forms.Button reload_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button viewInf_Button;
        private System.Windows.Forms.Label driversFound_Label;
        private System.Windows.Forms.BindingSource printDriverPropertiesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn providerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn versionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printProcessorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn architectureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn verifyPdlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn releaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn configurationFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn infPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn helpFileDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn monitorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}