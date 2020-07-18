namespace HP.ScalableTest.Plugin.RegusKiosk
{
    partial class RegusKioskConfigurationControl
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
            this.components = new System.ComponentModel.Container();
            this.login_GroupBox = new System.Windows.Forms.GroupBox();
            this.authtype_ComboBox = new System.Windows.Forms.ComboBox();
            this.pwd_TextBox = new System.Windows.Forms.TextBox();
            this.id_TextBox = new System.Windows.Forms.TextBox();
            this.pwd_Label = new System.Windows.Forms.Label();
            this.id_Label = new System.Windows.Forms.Label();
            this.authtype_Label = new System.Windows.Forms.Label();
            this.jobType_GroupBox = new System.Windows.Forms.GroupBox();
            this.scanDestination_comboBox = new System.Windows.Forms.ComboBox();
            this.printSource_comboBox = new System.Windows.Forms.ComboBox();
            this.scan_RadioButton = new System.Windows.Forms.RadioButton();
            this.print_RadioButton = new System.Windows.Forms.RadioButton();
            this.copy_RadioButton = new System.Windows.Forms.RadioButton();
            this.options_Button = new System.Windows.Forms.Button();
            this.jobBuildPageCountCaption_label = new System.Windows.Forms.Label();
            this.pageCount_groupBox = new System.Windows.Forms.GroupBox();
            this.path_label = new System.Windows.Forms.Label();
            this.path_textBox = new System.Windows.Forms.TextBox();
            this.jobBuildPageCount_label = new System.Windows.Forms.Label();
            this.jobBuildPageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.job_label = new System.Windows.Forms.Label();
            this.sourceDestination_label = new System.Windows.Forms.Label();
            this.jobCaption_label = new System.Windows.Forms.Label();
            this.sourceDestinationCaption_label = new System.Windows.Forms.Label();
            this.options_groupBox = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.pin_TextBox = new System.Windows.Forms.TextBox();
            this.pin_Label = new System.Windows.Forms.Label();
            this.login_GroupBox.SuspendLayout();
            this.jobType_GroupBox.SuspendLayout();
            this.pageCount_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).BeginInit();
            this.options_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // login_GroupBox
            // 
            this.login_GroupBox.Controls.Add(this.pin_TextBox);
            this.login_GroupBox.Controls.Add(this.pin_Label);
            this.login_GroupBox.Controls.Add(this.authtype_ComboBox);
            this.login_GroupBox.Controls.Add(this.pwd_TextBox);
            this.login_GroupBox.Controls.Add(this.id_TextBox);
            this.login_GroupBox.Controls.Add(this.pwd_Label);
            this.login_GroupBox.Controls.Add(this.id_Label);
            this.login_GroupBox.Controls.Add(this.authtype_Label);
            this.login_GroupBox.Location = new System.Drawing.Point(20, 23);
            this.login_GroupBox.Name = "login_GroupBox";
            this.login_GroupBox.Size = new System.Drawing.Size(245, 151);
            this.login_GroupBox.TabIndex = 0;
            this.login_GroupBox.TabStop = false;
            this.login_GroupBox.Text = "Login";
            // 
            // authtype_ComboBox
            // 
            this.authtype_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.authtype_ComboBox.FormattingEnabled = true;
            this.authtype_ComboBox.Location = new System.Drawing.Point(102, 27);
            this.authtype_ComboBox.Name = "authtype_ComboBox";
            this.authtype_ComboBox.Size = new System.Drawing.Size(124, 23);
            this.authtype_ComboBox.TabIndex = 6;
            this.authtype_ComboBox.SelectedIndexChanged += new System.EventHandler(this.authtype_ComboBox_SelectedIndexChanged);
            // 
            // pwd_TextBox
            // 
            this.pwd_TextBox.Location = new System.Drawing.Point(102, 87);
            this.pwd_TextBox.Name = "pwd_TextBox";
            this.pwd_TextBox.Size = new System.Drawing.Size(124, 23);
            this.pwd_TextBox.TabIndex = 4;
            // 
            // id_TextBox
            // 
            this.id_TextBox.Location = new System.Drawing.Point(102, 57);
            this.id_TextBox.Name = "id_TextBox";
            this.id_TextBox.Size = new System.Drawing.Size(124, 23);
            this.id_TextBox.TabIndex = 3;
            // 
            // pwd_Label
            // 
            this.pwd_Label.AutoSize = true;
            this.pwd_Label.Location = new System.Drawing.Point(15, 90);
            this.pwd_Label.Name = "pwd_Label";
            this.pwd_Label.Size = new System.Drawing.Size(69, 15);
            this.pwd_Label.TabIndex = 2;
            this.pwd_Label.Text = "Enter PWD :";
            // 
            // id_Label
            // 
            this.id_Label.AutoSize = true;
            this.id_Label.Location = new System.Drawing.Point(16, 60);
            this.id_Label.Name = "id_Label";
            this.id_Label.Size = new System.Drawing.Size(54, 15);
            this.id_Label.TabIndex = 1;
            this.id_Label.Text = "Enter ID :";
            // 
            // authtype_Label
            // 
            this.authtype_Label.AutoSize = true;
            this.authtype_Label.Location = new System.Drawing.Point(16, 27);
            this.authtype_Label.Name = "authtype_Label";
            this.authtype_Label.Size = new System.Drawing.Size(70, 15);
            this.authtype_Label.TabIndex = 0;
            this.authtype_Label.Text = "Auth Type : ";
            // 
            // jobType_GroupBox
            // 
            this.jobType_GroupBox.Controls.Add(this.scanDestination_comboBox);
            this.jobType_GroupBox.Controls.Add(this.printSource_comboBox);
            this.jobType_GroupBox.Controls.Add(this.scan_RadioButton);
            this.jobType_GroupBox.Controls.Add(this.print_RadioButton);
            this.jobType_GroupBox.Controls.Add(this.copy_RadioButton);
            this.jobType_GroupBox.Location = new System.Drawing.Point(271, 23);
            this.jobType_GroupBox.Name = "jobType_GroupBox";
            this.jobType_GroupBox.Size = new System.Drawing.Size(233, 151);
            this.jobType_GroupBox.TabIndex = 1;
            this.jobType_GroupBox.TabStop = false;
            this.jobType_GroupBox.Text = "Jop Type";
            // 
            // scanDestination_comboBox
            // 
            this.scanDestination_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scanDestination_comboBox.Enabled = false;
            this.scanDestination_comboBox.FormattingEnabled = true;
            this.scanDestination_comboBox.Location = new System.Drawing.Point(98, 99);
            this.scanDestination_comboBox.Name = "scanDestination_comboBox";
            this.scanDestination_comboBox.Size = new System.Drawing.Size(111, 23);
            this.scanDestination_comboBox.TabIndex = 8;
            this.scanDestination_comboBox.SelectedIndexChanged += new System.EventHandler(this.ScanDestination_comboBox_SelectedIndexChanged);
            // 
            // printSource_comboBox
            // 
            this.printSource_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printSource_comboBox.Enabled = false;
            this.printSource_comboBox.FormattingEnabled = true;
            this.printSource_comboBox.Location = new System.Drawing.Point(98, 67);
            this.printSource_comboBox.Name = "printSource_comboBox";
            this.printSource_comboBox.Size = new System.Drawing.Size(111, 23);
            this.printSource_comboBox.TabIndex = 7;
            this.printSource_comboBox.SelectedIndexChanged += new System.EventHandler(this.PrintSource_comboBox_SelectedIndexChanged);
            // 
            // scan_RadioButton
            // 
            this.scan_RadioButton.AutoSize = true;
            this.scan_RadioButton.Location = new System.Drawing.Point(18, 100);
            this.scan_RadioButton.Name = "scan_RadioButton";
            this.scan_RadioButton.Size = new System.Drawing.Size(50, 19);
            this.scan_RadioButton.TabIndex = 2;
            this.scan_RadioButton.TabStop = true;
            this.scan_RadioButton.Text = "Scan";
            this.scan_RadioButton.UseVisualStyleBackColor = true;
            this.scan_RadioButton.CheckedChanged += new System.EventHandler(this.scan_RadioButton_CheckedChanged);
            // 
            // print_RadioButton
            // 
            this.print_RadioButton.AutoSize = true;
            this.print_RadioButton.Location = new System.Drawing.Point(18, 68);
            this.print_RadioButton.Name = "print_RadioButton";
            this.print_RadioButton.Size = new System.Drawing.Size(50, 19);
            this.print_RadioButton.TabIndex = 1;
            this.print_RadioButton.TabStop = true;
            this.print_RadioButton.Text = "Print";
            this.print_RadioButton.UseVisualStyleBackColor = true;
            this.print_RadioButton.CheckedChanged += new System.EventHandler(this.print_RadioButton_CheckedChanged);
            // 
            // copy_RadioButton
            // 
            this.copy_RadioButton.AutoSize = true;
            this.copy_RadioButton.Checked = true;
            this.copy_RadioButton.Location = new System.Drawing.Point(18, 38);
            this.copy_RadioButton.Name = "copy_RadioButton";
            this.copy_RadioButton.Size = new System.Drawing.Size(53, 19);
            this.copy_RadioButton.TabIndex = 0;
            this.copy_RadioButton.TabStop = true;
            this.copy_RadioButton.Text = "Copy";
            this.copy_RadioButton.UseVisualStyleBackColor = true;
            this.copy_RadioButton.CheckedChanged += new System.EventHandler(this.copy_RadioButton_CheckedChanged);
            // 
            // options_Button
            // 
            this.options_Button.Location = new System.Drawing.Point(49, 96);
            this.options_Button.Name = "options_Button";
            this.options_Button.Size = new System.Drawing.Size(130, 41);
            this.options_Button.TabIndex = 3;
            this.options_Button.Text = "Options";
            this.options_Button.UseVisualStyleBackColor = true;
            this.options_Button.Click += new System.EventHandler(this.options_Button_Click);
            // 
            // jobBuildPageCountCaption_label
            // 
            this.jobBuildPageCountCaption_label.AutoSize = true;
            this.jobBuildPageCountCaption_label.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.jobBuildPageCountCaption_label.Location = new System.Drawing.Point(14, 32);
            this.jobBuildPageCountCaption_label.Name = "jobBuildPageCountCaption_label";
            this.jobBuildPageCountCaption_label.Size = new System.Drawing.Size(300, 15);
            this.jobBuildPageCountCaption_label.TabIndex = 6;
            this.jobBuildPageCountCaption_label.Text = "Select the number of pages for the scanned document. ";
            // 
            // pageCount_groupBox
            // 
            this.pageCount_groupBox.Controls.Add(this.path_label);
            this.pageCount_groupBox.Controls.Add(this.path_textBox);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_label);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_numericUpDown);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCountCaption_label);
            this.pageCount_groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageCount_groupBox.Location = new System.Drawing.Point(20, 180);
            this.pageCount_groupBox.Name = "pageCount_groupBox";
            this.pageCount_groupBox.Size = new System.Drawing.Size(484, 102);
            this.pageCount_groupBox.TabIndex = 6;
            this.pageCount_groupBox.TabStop = false;
            this.pageCount_groupBox.Text = "Job Build Page Count";
            // 
            // path_label
            // 
            this.path_label.AutoSize = true;
            this.path_label.Enabled = false;
            this.path_label.Location = new System.Drawing.Point(16, 66);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(63, 15);
            this.path_label.TabIndex = 8;
            this.path_label.Text = "* File Path:";
            this.path_label.Visible = false;
            // 
            // path_textBox
            // 
            this.path_textBox.Enabled = false;
            this.path_textBox.Location = new System.Drawing.Point(85, 63);
            this.path_textBox.Name = "path_textBox";
            this.path_textBox.Size = new System.Drawing.Size(271, 23);
            this.path_textBox.TabIndex = 7;
            this.path_textBox.Visible = false;
            // 
            // jobBuildPageCount_label
            // 
            this.jobBuildPageCount_label.AutoSize = true;
            this.jobBuildPageCount_label.Location = new System.Drawing.Point(16, 66);
            this.jobBuildPageCount_label.Name = "jobBuildPageCount_label";
            this.jobBuildPageCount_label.Size = new System.Drawing.Size(156, 15);
            this.jobBuildPageCount_label.TabIndex = 1;
            this.jobBuildPageCount_label.Text = "* Job Build Page Count >= 1";
            // 
            // jobBuildPageCount_numericUpDown
            // 
            this.jobBuildPageCount_numericUpDown.Location = new System.Drawing.Point(217, 64);
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
            this.jobBuildPageCount_numericUpDown.Size = new System.Drawing.Size(108, 23);
            this.jobBuildPageCount_numericUpDown.TabIndex = 2;
            this.jobBuildPageCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // job_label
            // 
            this.job_label.AutoSize = true;
            this.job_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.job_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.job_label.Location = new System.Drawing.Point(116, 31);
            this.job_label.Name = "job_label";
            this.job_label.Size = new System.Drawing.Size(44, 19);
            this.job_label.TabIndex = 4;
            this.job_label.Text = "Copy";
            // 
            // sourceDestination_label
            // 
            this.sourceDestination_label.AutoSize = true;
            this.sourceDestination_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceDestination_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sourceDestination_label.Location = new System.Drawing.Point(116, 60);
            this.sourceDestination_label.Name = "sourceDestination_label";
            this.sourceDestination_label.Size = new System.Drawing.Size(50, 19);
            this.sourceDestination_label.TabIndex = 5;
            this.sourceDestination_label.Text = "label1";
            this.sourceDestination_label.Visible = false;
            // 
            // jobCaption_label
            // 
            this.jobCaption_label.AutoSize = true;
            this.jobCaption_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobCaption_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.jobCaption_label.Location = new System.Drawing.Point(15, 31);
            this.jobCaption_label.Name = "jobCaption_label";
            this.jobCaption_label.Size = new System.Drawing.Size(65, 19);
            this.jobCaption_label.TabIndex = 6;
            this.jobCaption_label.Text = "Job Type:";
            // 
            // sourceDestinationCaption_label
            // 
            this.sourceDestinationCaption_label.AutoSize = true;
            this.sourceDestinationCaption_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceDestinationCaption_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.sourceDestinationCaption_label.Location = new System.Drawing.Point(15, 60);
            this.sourceDestinationCaption_label.Name = "sourceDestinationCaption_label";
            this.sourceDestinationCaption_label.Size = new System.Drawing.Size(90, 19);
            this.sourceDestinationCaption_label.TabIndex = 7;
            this.sourceDestinationCaption_label.Text = "Destionation:";
            this.sourceDestinationCaption_label.Visible = false;
            // 
            // options_groupBox
            // 
            this.options_groupBox.Controls.Add(this.sourceDestinationCaption_label);
            this.options_groupBox.Controls.Add(this.jobCaption_label);
            this.options_groupBox.Controls.Add(this.sourceDestination_label);
            this.options_groupBox.Controls.Add(this.job_label);
            this.options_groupBox.Controls.Add(this.options_Button);
            this.options_groupBox.Location = new System.Drawing.Point(510, 23);
            this.options_groupBox.Name = "options_groupBox";
            this.options_groupBox.Size = new System.Drawing.Size(236, 151);
            this.options_groupBox.TabIndex = 7;
            this.options_groupBox.TabStop = false;
            this.options_groupBox.Text = "Job Setting";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(502, 229);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(244, 53);
            this.lockTimeoutControl.TabIndex = 3;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(20, 285);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(734, 233);
            this.assetSelectionControl.TabIndex = 2;
            // 
            // pin_TextBox
            // 
            this.pin_TextBox.Location = new System.Drawing.Point(102, 117);
            this.pin_TextBox.Name = "pin_TextBox";
            this.pin_TextBox.Size = new System.Drawing.Size(124, 23);
            this.pin_TextBox.TabIndex = 8;
            // 
            // pin_Label
            // 
            this.pin_Label.AutoSize = true;
            this.pin_Label.Location = new System.Drawing.Point(15, 120);
            this.pin_Label.Name = "pin_Label";
            this.pin_Label.Size = new System.Drawing.Size(62, 15);
            this.pin_Label.TabIndex = 7;
            this.pin_Label.Text = "Enter PIN :";
            // 
            // RegusKioskConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.options_groupBox);
            this.Controls.Add(this.pageCount_groupBox);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.jobType_GroupBox);
            this.Controls.Add(this.login_GroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RegusKioskConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.login_GroupBox.ResumeLayout(false);
            this.login_GroupBox.PerformLayout();
            this.jobType_GroupBox.ResumeLayout(false);
            this.jobType_GroupBox.PerformLayout();
            this.pageCount_groupBox.ResumeLayout(false);
            this.pageCount_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).EndInit();
            this.options_groupBox.ResumeLayout(false);
            this.options_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox login_GroupBox;
        private System.Windows.Forms.GroupBox jobType_GroupBox;
        private System.Windows.Forms.Label id_Label;
        private System.Windows.Forms.Label authtype_Label;
        private System.Windows.Forms.Label pwd_Label;
        private System.Windows.Forms.TextBox id_TextBox;
        private System.Windows.Forms.TextBox pwd_TextBox;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.ComboBox authtype_ComboBox;
        private System.Windows.Forms.RadioButton print_RadioButton;
        private System.Windows.Forms.RadioButton copy_RadioButton;
        private System.Windows.Forms.RadioButton scan_RadioButton;
        private System.Windows.Forms.Button options_Button;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.Label jobBuildPageCountCaption_label;
        private System.Windows.Forms.GroupBox pageCount_groupBox;
        private System.Windows.Forms.Label jobBuildPageCount_label;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_numericUpDown;
        private System.Windows.Forms.ComboBox scanDestination_comboBox;
        private System.Windows.Forms.ComboBox printSource_comboBox;
        private System.Windows.Forms.Label job_label;
        private System.Windows.Forms.Label sourceDestination_label;
        private System.Windows.Forms.Label jobCaption_label;
        private System.Windows.Forms.Label sourceDestinationCaption_label;
        private System.Windows.Forms.TextBox path_textBox;
        private System.Windows.Forms.Label path_label;
        private System.Windows.Forms.GroupBox options_groupBox;
        private System.Windows.Forms.TextBox pin_TextBox;
        private System.Windows.Forms.Label pin_Label;
    }
}
