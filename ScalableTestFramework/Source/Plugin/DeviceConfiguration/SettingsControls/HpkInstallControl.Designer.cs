namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class HpkInstallControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.field1_Label = new System.Windows.Forms.Label();
            this.field2_Label = new System.Windows.Forms.Label();
            this.field3_Label = new System.Windows.Forms.Label();
            this.field1_Textbox = new System.Windows.Forms.TextBox();
            this.field2_Textbox = new System.Windows.Forms.TextBox();
            this.field3_Textbox = new System.Windows.Forms.TextBox();
            this.open_Button = new System.Windows.Forms.Button();
            this.add_button = new System.Windows.Forms.Button();
            this.hpkInstallGridView = new System.Windows.Forms.DataGridView();
            this.delete_button = new System.Windows.Forms.Button();
            this.field4_Label = new System.Windows.Forms.Label();
            this.retryCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.retry_CheckBox = new System.Windows.Forms.CheckBox();
            this.skip_CheckBox = new System.Windows.Forms.CheckBox();
            this.packageNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uuidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hpkInstallTableDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.hpkInstallGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.retryCount_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpkInstallTableDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // field1_Label
            // 
            this.field1_Label.AutoSize = true;
            this.field1_Label.Location = new System.Drawing.Point(28, 44);
            this.field1_Label.Name = "field1_Label";
            this.field1_Label.Size = new System.Drawing.Size(81, 13);
            this.field1_Label.TabIndex = 0;
            this.field1_Label.Text = "Package Name";
            // 
            // field2_Label
            // 
            this.field2_Label.AutoSize = true;
            this.field2_Label.Location = new System.Drawing.Point(28, 70);
            this.field2_Label.Name = "field2_Label";
            this.field2_Label.Size = new System.Drawing.Size(29, 13);
            this.field2_Label.TabIndex = 1;
            this.field2_Label.Text = "Uuid";
            // 
            // field3_Label
            // 
            this.field3_Label.AutoSize = true;
            this.field3_Label.Location = new System.Drawing.Point(28, 96);
            this.field3_Label.Name = "field3_Label";
            this.field3_Label.Size = new System.Drawing.Size(45, 13);
            this.field3_Label.TabIndex = 2;
            this.field3_Label.Text = "FilePath";
            // 
            // field1_Textbox
            // 
            this.field1_Textbox.Location = new System.Drawing.Point(128, 41);
            this.field1_Textbox.Name = "field1_Textbox";
            this.field1_Textbox.ReadOnly = true;
            this.field1_Textbox.Size = new System.Drawing.Size(346, 20);
            this.field1_Textbox.TabIndex = 3;
            // 
            // field2_Textbox
            // 
            this.field2_Textbox.Location = new System.Drawing.Point(128, 67);
            this.field2_Textbox.Name = "field2_Textbox";
            this.field2_Textbox.ReadOnly = true;
            this.field2_Textbox.Size = new System.Drawing.Size(346, 20);
            this.field2_Textbox.TabIndex = 4;
            // 
            // field3_Textbox
            // 
            this.field3_Textbox.Location = new System.Drawing.Point(128, 93);
            this.field3_Textbox.Name = "field3_Textbox";
            this.field3_Textbox.ReadOnly = true;
            this.field3_Textbox.Size = new System.Drawing.Size(346, 20);
            this.field3_Textbox.TabIndex = 5;
            // 
            // open_Button
            // 
            this.open_Button.Location = new System.Drawing.Point(504, 40);
            this.open_Button.Name = "open_Button";
            this.open_Button.Size = new System.Drawing.Size(116, 23);
            this.open_Button.TabIndex = 6;
            this.open_Button.Text = "Open HPK file";
            this.open_Button.UseVisualStyleBackColor = true;
            this.open_Button.Click += new System.EventHandler(this.open_Button_Click);
            // 
            // add_button
            // 
            this.add_button.Location = new System.Drawing.Point(504, 92);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(116, 23);
            this.add_button.TabIndex = 7;
            this.add_button.Text = "Add";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // hpkInstallGridView
            // 
            this.hpkInstallGridView.AutoGenerateColumns = false;
            this.hpkInstallGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hpkInstallGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.packageNameDataGridViewTextBoxColumn,
            this.uuidDataGridViewTextBoxColumn,
            this.filePathDataGridViewTextBoxColumn});
            this.hpkInstallGridView.DataSource = this.hpkInstallTableDataBindingSource;
            this.hpkInstallGridView.Location = new System.Drawing.Point(22, 167);
            this.hpkInstallGridView.Name = "hpkInstallGridView";
            this.hpkInstallGridView.Size = new System.Drawing.Size(598, 304);
            this.hpkInstallGridView.TabIndex = 8;
            // 
            // delete_button
            // 
            this.delete_button.Location = new System.Drawing.Point(504, 141);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(116, 23);
            this.delete_button.TabIndex = 9;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // field4_Label
            // 
            this.field4_Label.AutoSize = true;
            this.field4_Label.Location = new System.Drawing.Point(28, 121);
            this.field4_Label.Name = "field4_Label";
            this.field4_Label.Size = new System.Drawing.Size(82, 13);
            this.field4_Label.TabIndex = 10;
            this.field4_Label.Text = "Retry limit count";
            // 
            // retryCount_numericUpDown
            // 
            this.retryCount_numericUpDown.Enabled = false;
            this.retryCount_numericUpDown.Location = new System.Drawing.Point(128, 119);
            this.retryCount_numericUpDown.Name = "retryCount_numericUpDown";
            this.retryCount_numericUpDown.ReadOnly = true;
            this.retryCount_numericUpDown.Size = new System.Drawing.Size(200, 20);
            this.retryCount_numericUpDown.TabIndex = 11;
            this.retryCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.retryCount_numericUpDown.ValueChanged += new System.EventHandler(this.retryCount_numericUpDown_ValueChanged);
            // 
            // retry_CheckBox
            // 
            this.retry_CheckBox.AutoSize = true;
            this.retry_CheckBox.Enabled = false;
            this.retry_CheckBox.Location = new System.Drawing.Point(334, 121);
            this.retry_CheckBox.Name = "retry_CheckBox";
            this.retry_CheckBox.Size = new System.Drawing.Size(140, 17);
            this.retry_CheckBox.TabIndex = 12;
            this.retry_CheckBox.Text = "Enable Installation Retry";
            this.retry_CheckBox.UseVisualStyleBackColor = true;
            this.retry_CheckBox.CheckedChanged += new System.EventHandler(this.retry_CheckBox_CheckedChanged);
            // 
            // skip_CheckBox
            // 
            this.skip_CheckBox.AutoSize = true;
            this.skip_CheckBox.Enabled = false;
            this.skip_CheckBox.Location = new System.Drawing.Point(334, 145);
            this.skip_CheckBox.Name = "skip_CheckBox";
            this.skip_CheckBox.Size = new System.Drawing.Size(114, 17);
            this.skip_CheckBox.TabIndex = 13;
            this.skip_CheckBox.Text = "Skip Installed HPK";
            this.skip_CheckBox.UseVisualStyleBackColor = true;
            this.skip_CheckBox.CheckedChanged += new System.EventHandler(this.skip_CheckBox_CheckedChanged);
            // 
            // packageNameDataGridViewTextBoxColumn
            // 
            this.packageNameDataGridViewTextBoxColumn.DataPropertyName = "PackageName";
            this.packageNameDataGridViewTextBoxColumn.HeaderText = "PackageName";
            this.packageNameDataGridViewTextBoxColumn.Name = "packageNameDataGridViewTextBoxColumn";
            this.packageNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // uuidDataGridViewTextBoxColumn
            // 
            this.uuidDataGridViewTextBoxColumn.DataPropertyName = "Uuid";
            this.uuidDataGridViewTextBoxColumn.HeaderText = "Uuid";
            this.uuidDataGridViewTextBoxColumn.Name = "uuidDataGridViewTextBoxColumn";
            this.uuidDataGridViewTextBoxColumn.Width = 150;
            // 
            // filePathDataGridViewTextBoxColumn
            // 
            this.filePathDataGridViewTextBoxColumn.DataPropertyName = "FilePath";
            this.filePathDataGridViewTextBoxColumn.HeaderText = "FilePath";
            this.filePathDataGridViewTextBoxColumn.Name = "filePathDataGridViewTextBoxColumn";
            this.filePathDataGridViewTextBoxColumn.Width = 300;
            // 
            // hpkInstallTableDataBindingSource
            // 
            this.hpkInstallTableDataBindingSource.DataSource = typeof(HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.HpkInstallTableData);
            // 
            // HpkInstallControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skip_CheckBox);
            this.Controls.Add(this.retry_CheckBox);
            this.Controls.Add(this.retryCount_numericUpDown);
            this.Controls.Add(this.field4_Label);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.hpkInstallGridView);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.open_Button);
            this.Controls.Add(this.field3_Textbox);
            this.Controls.Add(this.field2_Textbox);
            this.Controls.Add(this.field1_Textbox);
            this.Controls.Add(this.field3_Label);
            this.Controls.Add(this.field2_Label);
            this.Controls.Add(this.field1_Label);
            this.Name = "HpkInstallControl";
            this.Size = new System.Drawing.Size(639, 485);
            ((System.ComponentModel.ISupportInitialize)(this.hpkInstallGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.retryCount_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpkInstallTableDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label field1_Label;
        private System.Windows.Forms.Label field2_Label;
        private System.Windows.Forms.Label field3_Label;
        private System.Windows.Forms.TextBox field1_Textbox;
        private System.Windows.Forms.TextBox field2_Textbox;
        private System.Windows.Forms.TextBox field3_Textbox;
        private System.Windows.Forms.Button open_Button;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.DataGridView hpkInstallGridView;
        private System.Windows.Forms.BindingSource hpkInstallTableDataBindingSource;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn packageNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uuidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label field4_Label;
        private System.Windows.Forms.NumericUpDown retryCount_numericUpDown;
        private System.Windows.Forms.CheckBox retry_CheckBox;
        private System.Windows.Forms.CheckBox skip_CheckBox;
    }
}
