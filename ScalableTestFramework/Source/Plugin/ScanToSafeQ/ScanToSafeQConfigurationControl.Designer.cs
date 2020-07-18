namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    partial class ScanToSafeQConfigurationControl
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
            this.pageCount_groupBox = new System.Windows.Forms.GroupBox();
            this.path_label = new System.Windows.Forms.Label();
            this.path_textBox = new System.Windows.Forms.TextBox();
            this.jobBuildPageCount_label = new System.Windows.Forms.Label();
            this.jobBuildPageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox_Advanced = new System.Windows.Forms.GroupBox();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMode = new System.Windows.Forms.Label();
            this.radioButton_SafeQScan = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator();
            this.pageCount_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).BeginInit();
            this.groupBox_Advanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageCount_groupBox
            // 
            this.pageCount_groupBox.Controls.Add(this.path_label);
            this.pageCount_groupBox.Controls.Add(this.path_textBox);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_label);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_numericUpDown);
            this.pageCount_groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageCount_groupBox.Location = new System.Drawing.Point(11, 157);
            this.pageCount_groupBox.Name = "pageCount_groupBox";
            this.pageCount_groupBox.Size = new System.Drawing.Size(484, 102);
            this.pageCount_groupBox.TabIndex = 18;
            this.pageCount_groupBox.TabStop = false;
            this.pageCount_groupBox.Text = "Job Description";
            // 
            // path_label
            // 
            this.path_label.AutoSize = true;
            this.path_label.Location = new System.Drawing.Point(16, 38);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(107, 15);
            this.path_label.TabIndex = 8;
            this.path_label.Text = "* Workflow Name :";
            // 
            // path_textBox
            // 
            this.path_textBox.Location = new System.Drawing.Point(141, 35);
            this.path_textBox.Name = "path_textBox";
            this.path_textBox.Size = new System.Drawing.Size(184, 23);
            this.path_textBox.TabIndex = 7;
            // 
            // jobBuildPageCount_label
            // 
            this.jobBuildPageCount_label.AutoSize = true;
            this.jobBuildPageCount_label.Location = new System.Drawing.Point(16, 66);
            this.jobBuildPageCount_label.Name = "jobBuildPageCount_label";
            this.jobBuildPageCount_label.Size = new System.Drawing.Size(82, 15);
            this.jobBuildPageCount_label.TabIndex = 1;
            this.jobBuildPageCount_label.Text = "* Scan Count :";
            // 
            // jobBuildPageCount_numericUpDown
            // 
            this.jobBuildPageCount_numericUpDown.Location = new System.Drawing.Point(141, 64);
            this.jobBuildPageCount_numericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.jobBuildPageCount_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.jobBuildPageCount_numericUpDown.Name = "jobBuildPageCount_numericUpDown";
            this.jobBuildPageCount_numericUpDown.Size = new System.Drawing.Size(184, 23);
            this.jobBuildPageCount_numericUpDown.TabIndex = 2;
            this.jobBuildPageCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox_Advanced
            // 
            this.groupBox_Advanced.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Advanced.Controls.Add(this.label_AuthMode);
            this.groupBox_Advanced.Controls.Add(this.radioButton_SafeQScan);
            this.groupBox_Advanced.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Advanced.Location = new System.Drawing.Point(11, 3);
            this.groupBox_Advanced.Name = "groupBox_Advanced";
            this.groupBox_Advanced.Size = new System.Drawing.Size(484, 148);
            this.groupBox_Advanced.TabIndex = 19;
            this.groupBox_Advanced.TabStop = false;
            this.groupBox_Advanced.Text = "Authentication Configuration";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(198, 57);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(176, 23);
            this.comboBox_AuthProvider.TabIndex = 5;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMode
            // 
            this.label_AuthMode.AutoSize = true;
            this.label_AuthMode.Location = new System.Drawing.Point(15, 60);
            this.label_AuthMode.Name = "label_AuthMode";
            this.label_AuthMode.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMode.TabIndex = 4;
            this.label_AuthMode.Text = "Authentication Method";
            // 
            // radioButton_SafeQScan
            // 
            this.radioButton_SafeQScan.AutoSize = true;
            this.radioButton_SafeQScan.Location = new System.Drawing.Point(198, 31);
            this.radioButton_SafeQScan.Name = "radioButton_SafeQScan";
            this.radioButton_SafeQScan.Size = new System.Drawing.Size(84, 19);
            this.radioButton_SafeQScan.TabIndex = 2;
            this.radioButton_SafeQScan.Text = "SafeQ Scan";
            this.radioButton_SafeQScan.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 31);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(100, 19);
            this.radioButton_SignInButton.TabIndex = 1;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(500, 206);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(244, 53);
            this.lockTimeoutControl.TabIndex = 17;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(11, 268);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(734, 220);
            this.assetSelectionControl.TabIndex = 16;
            // 
            // ScanToSafeQConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Advanced);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.pageCount_groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToSafeQConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.pageCount_groupBox.ResumeLayout(false);
            this.pageCount_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).EndInit();
            this.groupBox_Advanced.ResumeLayout(false);
            this.groupBox_Advanced.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox pageCount_groupBox;
        private System.Windows.Forms.Label path_label;
        private System.Windows.Forms.TextBox path_textBox;
        private System.Windows.Forms.Label jobBuildPageCount_label;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_numericUpDown;
        private System.Windows.Forms.GroupBox groupBox_Advanced;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMode;
        private System.Windows.Forms.RadioButton radioButton_SafeQScan;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        //private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
    }
}
