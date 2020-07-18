namespace HP.ScalableTest.LabConsole
{
    partial class CitrixPublishedAppsForm
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
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }

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
            this.citrixServer_ComboBox = new System.Windows.Forms.ComboBox();
            this.citrixServer_Label = new System.Windows.Forms.Label();
            this.publishedApp_Label = new System.Windows.Forms.Label();
            this.publishedApp_OK = new System.Windows.Forms.Button();
            this.publishedApp_Apply = new System.Windows.Forms.Button();
            this.publishedApp_Cancel = new System.Windows.Forms.Button();
            this.stringValueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.publishedApp_DataGridView = new System.Windows.Forms.DataGridView();
            this.nameValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enterpriseScenarioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.systemSettingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.scenarioSessionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.stringValueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.publishedApp_DataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enterpriseScenarioBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioSessionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // citrixServer_ComboBox
            // 
            this.citrixServer_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.citrixServer_ComboBox.FormattingEnabled = true;
            this.citrixServer_ComboBox.Location = new System.Drawing.Point(12, 33);
            this.citrixServer_ComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.citrixServer_ComboBox.Name = "citrixServer_ComboBox";
            this.citrixServer_ComboBox.Size = new System.Drawing.Size(344, 24);
            this.citrixServer_ComboBox.TabIndex = 0;
            this.citrixServer_ComboBox.SelectedIndexChanged += new System.EventHandler(this.citrixServer_ComboBox_SelectedIndexChanged);
            // 
            // citrixServer_Label
            // 
            this.citrixServer_Label.AutoSize = true;
            this.citrixServer_Label.Location = new System.Drawing.Point(13, 14);
            this.citrixServer_Label.Name = "citrixServer_Label";
            this.citrixServer_Label.Size = new System.Drawing.Size(84, 17);
            this.citrixServer_Label.TabIndex = 1;
            this.citrixServer_Label.Text = "Citrix Server";
            // 
            // publishedApp_Label
            // 
            this.publishedApp_Label.AutoSize = true;
            this.publishedApp_Label.Location = new System.Drawing.Point(13, 73);
            this.publishedApp_Label.Name = "publishedApp_Label";
            this.publishedApp_Label.Size = new System.Drawing.Size(118, 17);
            this.publishedApp_Label.TabIndex = 3;
            this.publishedApp_Label.Text = "Application Name";
            // 
            // publishedApp_OK
            // 
            this.publishedApp_OK.Location = new System.Drawing.Point(45, 303);
            this.publishedApp_OK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.publishedApp_OK.Name = "publishedApp_OK";
            this.publishedApp_OK.Size = new System.Drawing.Size(100, 28);
            this.publishedApp_OK.TabIndex = 4;
            this.publishedApp_OK.Text = "OK";
            this.publishedApp_OK.UseVisualStyleBackColor = true;
            this.publishedApp_OK.Click += new System.EventHandler(this.publishedApp_OK_Click);
            // 
            // publishedApp_Apply
            // 
            this.publishedApp_Apply.Location = new System.Drawing.Point(257, 303);
            this.publishedApp_Apply.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.publishedApp_Apply.Name = "publishedApp_Apply";
            this.publishedApp_Apply.Size = new System.Drawing.Size(100, 28);
            this.publishedApp_Apply.TabIndex = 5;
            this.publishedApp_Apply.Text = "Apply";
            this.publishedApp_Apply.UseVisualStyleBackColor = true;
            this.publishedApp_Apply.Click += new System.EventHandler(this.publishedApp_Apply_Click);
            // 
            // publishedApp_Cancel
            // 
            this.publishedApp_Cancel.Location = new System.Drawing.Point(151, 303);
            this.publishedApp_Cancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.publishedApp_Cancel.Name = "publishedApp_Cancel";
            this.publishedApp_Cancel.Size = new System.Drawing.Size(100, 28);
            this.publishedApp_Cancel.TabIndex = 6;
            this.publishedApp_Cancel.Text = "Cancel";
            this.publishedApp_Cancel.UseVisualStyleBackColor = true;
            this.publishedApp_Cancel.Click += new System.EventHandler(this.publishedApp_Cancel_Click);
            // 
            // stringValueBindingSource
            // 
            this.stringValueBindingSource.DataSource = typeof(HP.ScalableTest.Framework.StringValue);
            // 
            // publishedApp_DataGridView
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.publishedApp_DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.publishedApp_DataGridView.AutoGenerateColumns = false;
            this.publishedApp_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.publishedApp_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameValue});
            this.publishedApp_DataGridView.DataSource = this.stringValueBindingSource;
            this.publishedApp_DataGridView.Location = new System.Drawing.Point(12, 92);
            this.publishedApp_DataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.publishedApp_DataGridView.Name = "publishedApp_DataGridView";
            this.publishedApp_DataGridView.RowHeadersWidth = 25;
            this.publishedApp_DataGridView.RowTemplate.Height = 28;
            this.publishedApp_DataGridView.Size = new System.Drawing.Size(345, 195);
            this.publishedApp_DataGridView.TabIndex = 7;
            this.publishedApp_DataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.productName_dataGridView_CellValidating);
            // 
            // nameValue
            // 
            this.nameValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameValue.DataPropertyName = "Value";
            this.nameValue.HeaderText = "Name";
            this.nameValue.Name = "nameValue";
            // 
            // enterpriseScenarioBindingSource
            // 
            this.enterpriseScenarioBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.EnterpriseScenario);
            // 
            // systemSettingBindingSource
            // 
            this.systemSettingBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.SystemSetting);
            // 
            // scenarioSessionBindingSource
            // 
            this.scenarioSessionBindingSource.DataSource = typeof(HP.ScalableTest.Data.EnterpriseTest.Model.ScenarioSession);
            // 
            // CitrixPublishedAppsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 342);
            this.Controls.Add(this.publishedApp_DataGridView);
            this.Controls.Add(this.publishedApp_Cancel);
            this.Controls.Add(this.publishedApp_Apply);
            this.Controls.Add(this.publishedApp_OK);
            this.Controls.Add(this.publishedApp_Label);
            this.Controls.Add(this.citrixServer_Label);
            this.Controls.Add(this.citrixServer_ComboBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CitrixPublishedAppsForm";
            this.Text = "Citrix Published Applications";
            this.Load += new System.EventHandler(this.CitrixPublishedAppsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.stringValueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.publishedApp_DataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enterpriseScenarioBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.systemSettingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenarioSessionBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox citrixServer_ComboBox;
        private System.Windows.Forms.Label citrixServer_Label;
        private System.Windows.Forms.Label publishedApp_Label;
        private System.Windows.Forms.Button publishedApp_OK;
        private System.Windows.Forms.Button publishedApp_Apply;
        private System.Windows.Forms.Button publishedApp_Cancel;
        private System.Windows.Forms.BindingSource stringValueBindingSource;
        private System.Windows.Forms.DataGridView publishedApp_DataGridView;
        private System.Windows.Forms.BindingSource enterpriseScenarioBindingSource;
        private System.Windows.Forms.BindingSource systemSettingBindingSource;
        private System.Windows.Forms.BindingSource scenarioSessionBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameValue;
    }
}