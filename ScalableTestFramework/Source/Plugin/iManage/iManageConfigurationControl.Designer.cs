namespace HP.ScalableTest.Plugin.iManage
{
    partial class iManageConfigurationControl
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
            this.options_groupBox = new System.Windows.Forms.GroupBox();
            this.jobCaption_label = new System.Windows.Forms.Label();
            this.job_label = new System.Windows.Forms.Label();
            this.options_Button = new System.Windows.Forms.Button();
            this.path_label = new System.Windows.Forms.Label();
            this.path_textBox = new System.Windows.Forms.TextBox();
            this.jobBuildPageCount_label = new System.Windows.Forms.Label();
            this.jobBuildPageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pageCount_groupBox = new System.Windows.Forms.GroupBox();
            this.WorkStorage = new System.Windows.Forms.Label();
            this.destination_comboBox = new System.Windows.Forms.ComboBox();
            this.scan_RadioButton = new System.Windows.Forms.RadioButton();
            this.print_RadioButton = new System.Windows.Forms.RadioButton();
            this.jobType_GroupBox = new System.Windows.Forms.GroupBox();
            this.pwd_TextBox = new System.Windows.Forms.TextBox();
            this.id_TextBox = new System.Windows.Forms.TextBox();
            this.pwd_Label = new System.Windows.Forms.Label();
            this.id_Label = new System.Windows.Forms.Label();
            this.login_GroupBox = new System.Windows.Forms.GroupBox();
            this.label_SignOut = new System.Windows.Forms.Label();
            this.comboBox_Logout = new System.Windows.Forms.ComboBox();
            this.label_SIO = new System.Windows.Forms.Label();
            this.comboBox_SIO = new System.Windows.Forms.ComboBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.fieldValidator1 = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.options_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).BeginInit();
            this.pageCount_groupBox.SuspendLayout();
            this.jobType_GroupBox.SuspendLayout();
            this.login_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // options_groupBox
            // 
            this.options_groupBox.Controls.Add(this.jobCaption_label);
            this.options_groupBox.Controls.Add(this.job_label);
            this.options_groupBox.Controls.Add(this.options_Button);
            this.options_groupBox.Location = new System.Drawing.Point(262, 78);
            this.options_groupBox.Name = "options_groupBox";
            this.options_groupBox.Size = new System.Drawing.Size(236, 105);
            this.options_groupBox.TabIndex = 13;
            this.options_groupBox.TabStop = false;
            this.options_groupBox.Text = "Job Setting";
            // 
            // jobCaption_label
            // 
            this.jobCaption_label.AutoSize = true;
            this.jobCaption_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobCaption_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.jobCaption_label.Location = new System.Drawing.Point(47, 23);
            this.jobCaption_label.Name = "jobCaption_label";
            this.jobCaption_label.Size = new System.Drawing.Size(80, 23);
            this.jobCaption_label.TabIndex = 6;
            this.jobCaption_label.Text = "Job Type:";
            // 
            // job_label
            // 
            this.job_label.AutoSize = true;
            this.job_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.job_label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.job_label.Location = new System.Drawing.Point(140, 23);
            this.job_label.Name = "job_label";
            this.job_label.Size = new System.Drawing.Size(49, 23);
            this.job_label.TabIndex = 4;
            this.job_label.Text = "Print";
            // 
            // options_Button
            // 
            this.options_Button.Location = new System.Drawing.Point(51, 51);
            this.options_Button.Name = "options_Button";
            this.options_Button.Size = new System.Drawing.Size(130, 41);
            this.options_Button.TabIndex = 3;
            this.options_Button.Text = "Options";
            this.options_Button.UseVisualStyleBackColor = true;
            this.options_Button.Click += new System.EventHandler(this.options_Button_Click);
            // 
            // path_label
            // 
            this.path_label.AutoSize = true;
            this.path_label.Location = new System.Drawing.Point(16, 38);
            this.path_label.Name = "path_label";
            this.path_label.Size = new System.Drawing.Size(77, 20);
            this.path_label.TabIndex = 8;
            this.path_label.Text = "* File Path:";
            // 
            // path_textBox
            // 
            this.path_textBox.Location = new System.Drawing.Point(85, 35);
            this.path_textBox.Name = "path_textBox";
            this.path_textBox.Size = new System.Drawing.Size(240, 27);
            this.path_textBox.TabIndex = 7;
            // 
            // jobBuildPageCount_label
            // 
            this.jobBuildPageCount_label.AutoSize = true;
            this.jobBuildPageCount_label.Enabled = false;
            this.jobBuildPageCount_label.Location = new System.Drawing.Point(16, 66);
            this.jobBuildPageCount_label.Name = "jobBuildPageCount_label";
            this.jobBuildPageCount_label.Size = new System.Drawing.Size(195, 20);
            this.jobBuildPageCount_label.TabIndex = 1;
            this.jobBuildPageCount_label.Text = "* Job Build Page Count >= 1";
            this.jobBuildPageCount_label.Visible = false;
            // 
            // jobBuildPageCount_numericUpDown
            // 
            this.jobBuildPageCount_numericUpDown.Enabled = false;
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
            this.jobBuildPageCount_numericUpDown.Size = new System.Drawing.Size(108, 27);
            this.jobBuildPageCount_numericUpDown.TabIndex = 2;
            this.jobBuildPageCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.jobBuildPageCount_numericUpDown.Visible = false;
            // 
            // pageCount_groupBox
            // 
            this.pageCount_groupBox.Controls.Add(this.WorkStorage);
            this.pageCount_groupBox.Controls.Add(this.destination_comboBox);
            this.pageCount_groupBox.Controls.Add(this.path_label);
            this.pageCount_groupBox.Controls.Add(this.path_textBox);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_label);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_numericUpDown);
            this.pageCount_groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageCount_groupBox.Location = new System.Drawing.Point(11, 179);
            this.pageCount_groupBox.Name = "pageCount_groupBox";
            this.pageCount_groupBox.Size = new System.Drawing.Size(487, 102);
            this.pageCount_groupBox.TabIndex = 12;
            this.pageCount_groupBox.TabStop = false;
            this.pageCount_groupBox.Text = "Job Description";
            // 
            // WorkStorage
            // 
            this.WorkStorage.AutoSize = true;
            this.WorkStorage.Location = new System.Drawing.Point(362, 38);
            this.WorkStorage.Name = "WorkStorage";
            this.WorkStorage.Size = new System.Drawing.Size(132, 20);
            this.WorkStorage.TabIndex = 10;
            this.WorkStorage.Text = "* Storage Location";
            // 
            // destination_comboBox
            // 
            this.destination_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destination_comboBox.FormattingEnabled = true;
            this.destination_comboBox.Location = new System.Drawing.Point(372, 64);
            this.destination_comboBox.Name = "destination_comboBox";
            this.destination_comboBox.Size = new System.Drawing.Size(94, 28);
            this.destination_comboBox.TabIndex = 9;
            // 
            // scan_RadioButton
            // 
            this.scan_RadioButton.AutoSize = true;
            this.scan_RadioButton.Location = new System.Drawing.Point(115, 33);
            this.scan_RadioButton.Name = "scan_RadioButton";
            this.scan_RadioButton.Size = new System.Drawing.Size(61, 24);
            this.scan_RadioButton.TabIndex = 2;
            this.scan_RadioButton.Text = "Scan";
            this.scan_RadioButton.UseVisualStyleBackColor = true;
            this.scan_RadioButton.CheckedChanged += new System.EventHandler(this.scan_RadioButton_CheckedChanged);
            // 
            // print_RadioButton
            // 
            this.print_RadioButton.AutoSize = true;
            this.print_RadioButton.Checked = true;
            this.print_RadioButton.Location = new System.Drawing.Point(30, 33);
            this.print_RadioButton.Name = "print_RadioButton";
            this.print_RadioButton.Size = new System.Drawing.Size(60, 24);
            this.print_RadioButton.TabIndex = 1;
            this.print_RadioButton.TabStop = true;
            this.print_RadioButton.Text = "Print";
            this.print_RadioButton.UseVisualStyleBackColor = true;
            this.print_RadioButton.CheckedChanged += new System.EventHandler(this.print_RadioButton_CheckedChanged);
            // 
            // jobType_GroupBox
            // 
            this.jobType_GroupBox.Controls.Add(this.scan_RadioButton);
            this.jobType_GroupBox.Controls.Add(this.print_RadioButton);
            this.jobType_GroupBox.Location = new System.Drawing.Point(262, 3);
            this.jobType_GroupBox.Name = "jobType_GroupBox";
            this.jobType_GroupBox.Size = new System.Drawing.Size(233, 75);
            this.jobType_GroupBox.TabIndex = 9;
            this.jobType_GroupBox.TabStop = false;
            this.jobType_GroupBox.Text = "Jop Type";
            // 
            // pwd_TextBox
            // 
            this.pwd_TextBox.Location = new System.Drawing.Point(106, 110);
            this.pwd_TextBox.Name = "pwd_TextBox";
            this.pwd_TextBox.Size = new System.Drawing.Size(121, 27);
            this.pwd_TextBox.TabIndex = 4;
            // 
            // id_TextBox
            // 
            this.id_TextBox.Location = new System.Drawing.Point(106, 72);
            this.id_TextBox.Name = "id_TextBox";
            this.id_TextBox.Size = new System.Drawing.Size(121, 27);
            this.id_TextBox.TabIndex = 3;
            // 
            // pwd_Label
            // 
            this.pwd_Label.AutoSize = true;
            this.pwd_Label.Location = new System.Drawing.Point(18, 113);
            this.pwd_Label.Name = "pwd_Label";
            this.pwd_Label.Size = new System.Drawing.Size(87, 20);
            this.pwd_Label.TabIndex = 2;
            this.pwd_Label.Text = "Enter PWD :";
            // 
            // id_Label
            // 
            this.id_Label.AutoSize = true;
            this.id_Label.Location = new System.Drawing.Point(18, 75);
            this.id_Label.Name = "id_Label";
            this.id_Label.Size = new System.Drawing.Size(69, 20);
            this.id_Label.TabIndex = 1;
            this.id_Label.Text = "Enter ID :";
            // 
            // login_GroupBox
            // 
            this.login_GroupBox.Controls.Add(this.label_SignOut);
            this.login_GroupBox.Controls.Add(this.comboBox_Logout);
            this.login_GroupBox.Controls.Add(this.label_SIO);
            this.login_GroupBox.Controls.Add(this.comboBox_SIO);
            this.login_GroupBox.Controls.Add(this.pwd_TextBox);
            this.login_GroupBox.Controls.Add(this.id_TextBox);
            this.login_GroupBox.Controls.Add(this.pwd_Label);
            this.login_GroupBox.Controls.Add(this.id_Label);
            this.login_GroupBox.Location = new System.Drawing.Point(11, 3);
            this.login_GroupBox.Name = "login_GroupBox";
            this.login_GroupBox.Size = new System.Drawing.Size(245, 180);
            this.login_GroupBox.TabIndex = 8;
            this.login_GroupBox.TabStop = false;
            this.login_GroupBox.Text = "Login";
            // 
            // label_SignOut
            // 
            this.label_SignOut.AutoSize = true;
            this.label_SignOut.Location = new System.Drawing.Point(18, 149);
            this.label_SignOut.Name = "label_SignOut";
            this.label_SignOut.Size = new System.Drawing.Size(73, 20);
            this.label_SignOut.TabIndex = 29;
            this.label_SignOut.Text = "Sign Out :";
            // 
            // comboBox_Logout
            // 
            this.comboBox_Logout.DisplayMember = "Value";
            this.comboBox_Logout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Logout.FormattingEnabled = true;
            this.comboBox_Logout.Location = new System.Drawing.Point(106, 146);
            this.comboBox_Logout.Name = "comboBox_Logout";
            this.comboBox_Logout.Size = new System.Drawing.Size(121, 28);
            this.comboBox_Logout.TabIndex = 26;
            this.comboBox_Logout.ValueMember = "Key";
            // 
            // label_SIO
            // 
            this.label_SIO.AutoSize = true;
            this.label_SIO.Location = new System.Drawing.Point(18, 32);
            this.label_SIO.Name = "label_SIO";
            this.label_SIO.Size = new System.Drawing.Size(61, 20);
            this.label_SIO.TabIndex = 28;
            this.label_SIO.Text = "Sign In :";
            // 
            // comboBox_SIO
            // 
            this.comboBox_SIO.DisplayMember = "Value";
            this.comboBox_SIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SIO.FormattingEnabled = true;
            this.comboBox_SIO.Location = new System.Drawing.Point(106, 29);
            this.comboBox_SIO.Name = "comboBox_SIO";
            this.comboBox_SIO.Size = new System.Drawing.Size(121, 28);
            this.comboBox_SIO.TabIndex = 27;
            this.comboBox_SIO.ValueMember = "Key";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(500, 228);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(244, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(11, 287);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(734, 217);
            this.assetSelectionControl.TabIndex = 10;
            // 
            // iManageConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.options_groupBox);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.pageCount_groupBox);
            this.Controls.Add(this.jobType_GroupBox);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.login_GroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "iManageConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.options_groupBox.ResumeLayout(false);
            this.options_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).EndInit();
            this.pageCount_groupBox.ResumeLayout(false);
            this.pageCount_groupBox.PerformLayout();
            this.jobType_GroupBox.ResumeLayout(false);
            this.jobType_GroupBox.PerformLayout();
            this.login_GroupBox.ResumeLayout(false);
            this.login_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.FieldValidator fieldValidator1;
        private System.Windows.Forms.GroupBox options_groupBox;
        private System.Windows.Forms.Label jobCaption_label;
        private System.Windows.Forms.Label job_label;
        private System.Windows.Forms.Button options_Button;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.Label path_label;
        private System.Windows.Forms.TextBox path_textBox;
        private System.Windows.Forms.Label jobBuildPageCount_label;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_numericUpDown;
        private System.Windows.Forms.GroupBox pageCount_groupBox;
        private System.Windows.Forms.RadioButton scan_RadioButton;
        private System.Windows.Forms.RadioButton print_RadioButton;
        private System.Windows.Forms.GroupBox jobType_GroupBox;
        private System.Windows.Forms.TextBox pwd_TextBox;
        private System.Windows.Forms.TextBox id_TextBox;
        private System.Windows.Forms.Label pwd_Label;
        private System.Windows.Forms.Label id_Label;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox login_GroupBox;
        private System.Windows.Forms.Label WorkStorage;
        private System.Windows.Forms.ComboBox destination_comboBox;
        private System.Windows.Forms.Label label_SignOut;
        private System.Windows.Forms.ComboBox comboBox_Logout;
        private System.Windows.Forms.Label label_SIO;
        private System.Windows.Forms.ComboBox comboBox_SIO;
    }
}
