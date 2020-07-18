namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    partial class ScenarioPlatformUsageForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioPlatformUsageForm));
            this.quota_DataGridView = new System.Windows.Forms.DataGridView();
            this.verifyDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.requiredCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorizedCountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.machineQuotaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.platform_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.scenario_TextBox = new System.Windows.Forms.TextBox();
            this.user_TextBox = new System.Windows.Forms.TextBox();
            this.owner_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.submit_LinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.quota_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineQuotaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // quota_DataGridView
            // 
            this.quota_DataGridView.AllowUserToAddRows = false;
            this.quota_DataGridView.AllowUserToDeleteRows = false;
            this.quota_DataGridView.AllowUserToResizeColumns = false;
            this.quota_DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.quota_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.quota_DataGridView.AutoGenerateColumns = false;
            this.quota_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.quota_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.verifyDataGridViewImageColumn,
            this.requiredCountDataGridViewTextBoxColumn,
            this.authorizedCountDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.quota_DataGridView.DataSource = this.machineQuotaBindingSource;
            this.quota_DataGridView.Location = new System.Drawing.Point(12, 165);
            this.quota_DataGridView.Name = "quota_DataGridView";
            this.quota_DataGridView.ReadOnly = true;
            this.quota_DataGridView.RowHeadersVisible = false;
            this.quota_DataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.quota_DataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.quota_DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.quota_DataGridView.ShowCellErrors = false;
            this.quota_DataGridView.ShowCellToolTips = false;
            this.quota_DataGridView.Size = new System.Drawing.Size(573, 140);
            this.quota_DataGridView.TabIndex = 0;
            this.quota_DataGridView.SelectionChanged += new System.EventHandler(this.quota_DataGridView_SelectionChanged);
            // 
            // verifyDataGridViewImageColumn
            // 
            this.verifyDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.verifyDataGridViewImageColumn.HeaderText = "";
            this.verifyDataGridViewImageColumn.Name = "verifyDataGridViewImageColumn";
            this.verifyDataGridViewImageColumn.ReadOnly = true;
            this.verifyDataGridViewImageColumn.Width = 5;
            // 
            // requiredCountDataGridViewTextBoxColumn
            // 
            this.requiredCountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.requiredCountDataGridViewTextBoxColumn.DataPropertyName = "RequiredCount";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.requiredCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.requiredCountDataGridViewTextBoxColumn.HeaderText = "Required";
            this.requiredCountDataGridViewTextBoxColumn.Name = "requiredCountDataGridViewTextBoxColumn";
            this.requiredCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.requiredCountDataGridViewTextBoxColumn.Width = 75;
            // 
            // authorizedCountDataGridViewTextBoxColumn
            // 
            this.authorizedCountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.authorizedCountDataGridViewTextBoxColumn.DataPropertyName = "AuthorizedCount";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.authorizedCountDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.authorizedCountDataGridViewTextBoxColumn.HeaderText = "Authorized";
            this.authorizedCountDataGridViewTextBoxColumn.Name = "authorizedCountDataGridViewTextBoxColumn";
            this.authorizedCountDataGridViewTextBoxColumn.ReadOnly = true;
            this.authorizedCountDataGridViewTextBoxColumn.Width = 82;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // machineQuotaBindingSource
            // 
            this.machineQuotaBindingSource.DataSource = typeof(HP.ScalableTest.Framework.Dispatcher.ScenarioPlatformUsage);
            // 
            // platform_ImageList
            // 
            this.platform_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("platform_ImageList.ImageStream")));
            this.platform_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.platform_ImageList.Images.SetKeyName(0, "Tick");
            this.platform_ImageList.Images.SetKeyName(1, "Cross");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Required and Authorized Platform Quantities";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(576, 41);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Scenario Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "User Name";
            // 
            // scenario_TextBox
            // 
            this.scenario_TextBox.Location = new System.Drawing.Point(101, 89);
            this.scenario_TextBox.Name = "scenario_TextBox";
            this.scenario_TextBox.ReadOnly = true;
            this.scenario_TextBox.Size = new System.Drawing.Size(484, 20);
            this.scenario_TextBox.TabIndex = 5;
            // 
            // user_TextBox
            // 
            this.user_TextBox.Location = new System.Drawing.Point(101, 63);
            this.user_TextBox.Name = "user_TextBox";
            this.user_TextBox.ReadOnly = true;
            this.user_TextBox.Size = new System.Drawing.Size(157, 20);
            this.user_TextBox.TabIndex = 6;
            // 
            // owner_TextBox
            // 
            this.owner_TextBox.Location = new System.Drawing.Point(101, 115);
            this.owner_TextBox.Name = "owner_TextBox";
            this.owner_TextBox.ReadOnly = true;
            this.owner_TextBox.Size = new System.Drawing.Size(157, 20);
            this.owner_TextBox.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Scenario Owner";
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(510, 311);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 9;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // submit_LinkLabel
            // 
            this.submit_LinkLabel.AutoSize = true;
            this.submit_LinkLabel.Location = new System.Drawing.Point(10, 318);
            this.submit_LinkLabel.Name = "submit_LinkLabel";
            this.submit_LinkLabel.Size = new System.Drawing.Size(185, 13);
            this.submit_LinkLabel.TabIndex = 12;
            this.submit_LinkLabel.TabStop = true;
            this.submit_LinkLabel.Text = "Submit request for additional platforms";
            this.submit_LinkLabel.Visible = false;
            this.submit_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.submit_LinkLabel_LinkClicked);
            // 
            // ScenarioPlatformUsageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(597, 344);
            this.Controls.Add(this.submit_LinkLabel);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.owner_TextBox);
            this.Controls.Add(this.user_TextBox);
            this.Controls.Add(this.scenario_TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.quota_DataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScenarioPlatformUsageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Verify Platform Quantities";
            this.Load += new System.EventHandler(this.MachineQuotaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.quota_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.machineQuotaBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView quota_DataGridView;
        private System.Windows.Forms.BindingSource machineQuotaBindingSource;
        private System.Windows.Forms.ImageList platform_ImageList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox scenario_TextBox;
        private System.Windows.Forms.TextBox user_TextBox;
        private System.Windows.Forms.TextBox owner_TextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.DataGridViewImageColumn verifyDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requiredCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorizedCountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.LinkLabel submit_LinkLabel;
    }
}