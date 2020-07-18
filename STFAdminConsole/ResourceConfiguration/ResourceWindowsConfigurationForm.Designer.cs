namespace HP.ScalableTest.LabConsole.ResourceConfiguration
{
    partial class ResourceWindowsConfigurationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.services_GridView = new System.Windows.Forms.DataGridView();
            this.servicesCheckBox_Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.displayName_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_Column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resource_GroupBox = new System.Windows.Forms.GroupBox();
            this.serverName_TextBox = new System.Windows.Forms.TextBox();
            this.resourceType_TextBox = new System.Windows.Forms.TextBox();
            this.serverValidation_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.add_Button = new System.Windows.Forms.Button();
            this.customSelection_GroupBox = new System.Windows.Forms.GroupBox();
            this.serviceName_Label = new System.Windows.Forms.Label();
            this.serviceName_TextBox = new System.Windows.Forms.TextBox();
            this.retrieveServicesList_RadioButton = new System.Windows.Forms.RadioButton();
            this.addCustomService_RadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.services_GridView)).BeginInit();
            this.resource_GroupBox.SuspendLayout();
            this.customSelection_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Resource Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Resource / Server";
            // 
            // services_GridView
            // 
            this.services_GridView.AllowUserToAddRows = false;
            this.services_GridView.AllowUserToDeleteRows = false;
            this.services_GridView.AllowUserToOrderColumns = true;
            this.services_GridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.services_GridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.services_GridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.services_GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.services_GridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.servicesCheckBox_Column,
            this.displayName_Column,
            this.name_Column});
            this.services_GridView.Location = new System.Drawing.Point(6, 142);
            this.services_GridView.Name = "services_GridView";
            this.services_GridView.RowHeadersVisible = false;
            this.services_GridView.RowTemplate.Height = 24;
            this.services_GridView.Size = new System.Drawing.Size(665, 236);
            this.services_GridView.TabIndex = 4;
            // 
            // servicesCheckBox_Column
            // 
            this.servicesCheckBox_Column.HeaderText = "Select";
            this.servicesCheckBox_Column.Name = "servicesCheckBox_Column";
            this.servicesCheckBox_Column.Width = 60;
            // 
            // displayName_Column
            // 
            this.displayName_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.displayName_Column.DataPropertyName = "DisplayName";
            this.displayName_Column.FillWeight = 7.299271F;
            this.displayName_Column.HeaderText = "Display Name";
            this.displayName_Column.Name = "displayName_Column";
            this.displayName_Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.displayName_Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // name_Column
            // 
            this.name_Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.name_Column.DataPropertyName = "ServiceName";
            this.name_Column.FillWeight = 192.7008F;
            this.name_Column.HeaderText = "Name";
            this.name_Column.Name = "name_Column";
            this.name_Column.ReadOnly = true;
            this.name_Column.Width = 250;
            // 
            // resource_GroupBox
            // 
            this.resource_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resource_GroupBox.Controls.Add(this.serverName_TextBox);
            this.resource_GroupBox.Controls.Add(this.resourceType_TextBox);
            this.resource_GroupBox.Controls.Add(this.serverValidation_Button);
            this.resource_GroupBox.Controls.Add(this.label1);
            this.resource_GroupBox.Controls.Add(this.label2);
            this.resource_GroupBox.Location = new System.Drawing.Point(12, 22);
            this.resource_GroupBox.Name = "resource_GroupBox";
            this.resource_GroupBox.Size = new System.Drawing.Size(677, 139);
            this.resource_GroupBox.TabIndex = 6;
            this.resource_GroupBox.TabStop = false;
            this.resource_GroupBox.Text = "Resource Selection";
            // 
            // serverName_TextBox
            // 
            this.serverName_TextBox.Location = new System.Drawing.Point(138, 65);
            this.serverName_TextBox.Name = "serverName_TextBox";
            this.serverName_TextBox.Size = new System.Drawing.Size(327, 22);
            this.serverName_TextBox.TabIndex = 11;
            // 
            // resourceType_TextBox
            // 
            this.resourceType_TextBox.Location = new System.Drawing.Point(138, 33);
            this.resourceType_TextBox.Name = "resourceType_TextBox";
            this.resourceType_TextBox.Size = new System.Drawing.Size(327, 22);
            this.resourceType_TextBox.TabIndex = 10;
            // 
            // serverValidation_Button
            // 
            this.serverValidation_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.serverValidation_Button.Location = new System.Drawing.Point(558, 105);
            this.serverValidation_Button.Name = "serverValidation_Button";
            this.serverValidation_Button.Size = new System.Drawing.Size(113, 28);
            this.serverValidation_Button.TabIndex = 9;
            this.serverValidation_Button.Text = "Validate Server";
            this.serverValidation_Button.UseVisualStyleBackColor = true;
            this.serverValidation_Button.Click += new System.EventHandler(this.serverValidation_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(590, 585);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(93, 28);
            this.cancel_Button.TabIndex = 8;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // add_Button
            // 
            this.add_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.add_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.add_Button.Location = new System.Drawing.Point(486, 585);
            this.add_Button.Name = "add_Button";
            this.add_Button.Size = new System.Drawing.Size(93, 28);
            this.add_Button.TabIndex = 9;
            this.add_Button.Text = "Add";
            this.add_Button.UseVisualStyleBackColor = true;
            this.add_Button.Click += new System.EventHandler(this.add_Button_Click);
            // 
            // customSelection_GroupBox
            // 
            this.customSelection_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customSelection_GroupBox.Controls.Add(this.serviceName_Label);
            this.customSelection_GroupBox.Controls.Add(this.services_GridView);
            this.customSelection_GroupBox.Controls.Add(this.serviceName_TextBox);
            this.customSelection_GroupBox.Controls.Add(this.retrieveServicesList_RadioButton);
            this.customSelection_GroupBox.Controls.Add(this.addCustomService_RadioButton);
            this.customSelection_GroupBox.Location = new System.Drawing.Point(12, 182);
            this.customSelection_GroupBox.Name = "customSelection_GroupBox";
            this.customSelection_GroupBox.Size = new System.Drawing.Size(677, 384);
            this.customSelection_GroupBox.TabIndex = 10;
            this.customSelection_GroupBox.TabStop = false;
            this.customSelection_GroupBox.Text = "Custom Selection";
            // 
            // serviceName_Label
            // 
            this.serviceName_Label.AutoSize = true;
            this.serviceName_Label.Location = new System.Drawing.Point(82, 69);
            this.serviceName_Label.Name = "serviceName_Label";
            this.serviceName_Label.Size = new System.Drawing.Size(45, 16);
            this.serviceName_Label.TabIndex = 5;
            this.serviceName_Label.Text = "Name";
            // 
            // serviceName_TextBox
            // 
            this.serviceName_TextBox.Enabled = false;
            this.serviceName_TextBox.Location = new System.Drawing.Point(138, 69);
            this.serviceName_TextBox.Name = "serviceName_TextBox";
            this.serviceName_TextBox.Size = new System.Drawing.Size(327, 22);
            this.serviceName_TextBox.TabIndex = 2;
            // 
            // retrieveServicesList_RadioButton
            // 
            this.retrieveServicesList_RadioButton.AutoSize = true;
            this.retrieveServicesList_RadioButton.Checked = true;
            this.retrieveServicesList_RadioButton.Location = new System.Drawing.Point(11, 106);
            this.retrieveServicesList_RadioButton.Name = "retrieveServicesList_RadioButton";
            this.retrieveServicesList_RadioButton.Size = new System.Drawing.Size(175, 20);
            this.retrieveServicesList_RadioButton.TabIndex = 1;
            this.retrieveServicesList_RadioButton.TabStop = true;
            this.retrieveServicesList_RadioButton.Text = "Select services from host";
            this.retrieveServicesList_RadioButton.UseVisualStyleBackColor = true;
            this.retrieveServicesList_RadioButton.CheckedChanged += new System.EventHandler(this.retrieveServicesList_RadioButton_CheckedChanged);
            // 
            // addCustomService_RadioButton
            // 
            this.addCustomService_RadioButton.AutoSize = true;
            this.addCustomService_RadioButton.Location = new System.Drawing.Point(11, 42);
            this.addCustomService_RadioButton.Name = "addCustomService_RadioButton";
            this.addCustomService_RadioButton.Size = new System.Drawing.Size(148, 20);
            this.addCustomService_RadioButton.TabIndex = 0;
            this.addCustomService_RadioButton.Text = "Add Custom Service";
            this.addCustomService_RadioButton.UseVisualStyleBackColor = true;
            this.addCustomService_RadioButton.CheckedChanged += new System.EventHandler(this.addCustomService_RadioButton_CheckedChanged);
            // 
            // ResourceWindowsConfigurationForm
            // 
            this.AcceptButton = this.add_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(705, 625);
            this.Controls.Add(this.customSelection_GroupBox);
            this.Controls.Add(this.add_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.resource_GroupBox);
            this.Name = "ResourceWindowsConfigurationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Resource Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.services_GridView)).EndInit();
            this.resource_GroupBox.ResumeLayout(false);
            this.resource_GroupBox.PerformLayout();
            this.customSelection_GroupBox.ResumeLayout(false);
            this.customSelection_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView services_GridView;
        private System.Windows.Forms.GroupBox resource_GroupBox;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button add_Button;
        private System.Windows.Forms.Button serverValidation_Button;
        private System.Windows.Forms.TextBox serverName_TextBox;
        private System.Windows.Forms.TextBox resourceType_TextBox;
        private System.Windows.Forms.GroupBox customSelection_GroupBox;
        private System.Windows.Forms.TextBox serviceName_TextBox;
        private System.Windows.Forms.RadioButton retrieveServicesList_RadioButton;
        private System.Windows.Forms.RadioButton addCustomService_RadioButton;
        private System.Windows.Forms.Label serviceName_Label;
        private System.Windows.Forms.DataGridViewCheckBoxColumn servicesCheckBox_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayName_Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_Column;
    }
}