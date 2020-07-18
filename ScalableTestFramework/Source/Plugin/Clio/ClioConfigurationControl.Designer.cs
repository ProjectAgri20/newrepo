namespace HP.ScalableTest.Plugin.Clio
{
    partial class ClioConfigurationControl
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
            this.label_SignOut = new System.Windows.Forms.Label();
            this.comboBox_Logout = new System.Windows.Forms.ComboBox();
            this.label_SIO = new System.Windows.Forms.Label();
            this.comboBox_SIO = new System.Windows.Forms.ComboBox();
            this.pwd_TextBox = new System.Windows.Forms.TextBox();
            this.id_TextBox = new System.Windows.Forms.TextBox();
            this.pwd_Label = new System.Windows.Forms.Label();
            this.id_Label = new System.Windows.Forms.Label();
            this.jobType_GroupBox = new System.Windows.Forms.GroupBox();
            this.scan_RadioButton = new System.Windows.Forms.RadioButton();
            this.print_RadioButton = new System.Windows.Forms.RadioButton();
            this.options_groupBox = new System.Windows.Forms.GroupBox();
            this.jobCaption_Label = new System.Windows.Forms.Label();
            this.job_Label = new System.Windows.Forms.Label();
            this.options_Button = new System.Windows.Forms.Button();
            this.pageCount_groupBox = new System.Windows.Forms.GroupBox();
            this.matter_TextBox = new System.Windows.Forms.TextBox();
            this.matter_Label = new System.Windows.Forms.Label();
            this.path_Label = new System.Windows.Forms.Label();
            this.path_TextBox = new System.Windows.Forms.TextBox();
            this.jobBuildPageCount_Label = new System.Windows.Forms.Label();
            this.jobBuildPageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.WorkStorage = new System.Windows.Forms.Label();
            this.destination_comboBox = new System.Windows.Forms.ComboBox();
            this.login_GroupBox.SuspendLayout();
            this.jobType_GroupBox.SuspendLayout();
            this.options_groupBox.SuspendLayout();
            this.pageCount_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // login_GroupBox
            // 
            this.login_GroupBox.Controls.Add(this.destination_comboBox);
            this.login_GroupBox.Controls.Add(this.WorkStorage);
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
            this.login_GroupBox.Size = new System.Drawing.Size(245, 244);
            this.login_GroupBox.TabIndex = 1;
            this.login_GroupBox.TabStop = false;
            this.login_GroupBox.Text = "Sign In / Sign Out";
            // 
            // label_SignOut
            // 
            this.label_SignOut.AutoSize = true;
            this.label_SignOut.Location = new System.Drawing.Point(11, 168);
            this.label_SignOut.Name = "label_SignOut";
            this.label_SignOut.Size = new System.Drawing.Size(59, 15);
            this.label_SignOut.TabIndex = 25;
            this.label_SignOut.Text = "Sign Out :";
            // 
            // comboBox_Logout
            // 
            this.comboBox_Logout.DisplayMember = "Value";
            this.comboBox_Logout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Logout.FormattingEnabled = true;
            this.comboBox_Logout.Location = new System.Drawing.Point(87, 160);
            this.comboBox_Logout.Name = "comboBox_Logout";
            this.comboBox_Logout.Size = new System.Drawing.Size(121, 23);
            this.comboBox_Logout.TabIndex = 22;
            this.comboBox_Logout.ValueMember = "Key";
            // 
            // label_SIO
            // 
            this.label_SIO.AutoSize = true;
            this.label_SIO.Location = new System.Drawing.Point(11, 40);
            this.label_SIO.Name = "label_SIO";
            this.label_SIO.Size = new System.Drawing.Size(49, 15);
            this.label_SIO.TabIndex = 24;
            this.label_SIO.Text = "Sign In :";
            // 
            // comboBox_SIO
            // 
            this.comboBox_SIO.DisplayMember = "Value";
            this.comboBox_SIO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SIO.FormattingEnabled = true;
            this.comboBox_SIO.Location = new System.Drawing.Point(87, 37);
            this.comboBox_SIO.Name = "comboBox_SIO";
            this.comboBox_SIO.Size = new System.Drawing.Size(121, 23);
            this.comboBox_SIO.TabIndex = 23;
            this.comboBox_SIO.ValueMember = "Key";
            this.comboBox_SIO.SelectedIndexChanged += new System.EventHandler(this.comboBox_SIO_SelectedIndexChanged);
            // 
            // pwd_TextBox
            // 
            this.pwd_TextBox.Enabled = false;
            this.pwd_TextBox.Location = new System.Drawing.Point(87, 118);
            this.pwd_TextBox.Name = "pwd_TextBox";
            this.pwd_TextBox.Size = new System.Drawing.Size(121, 23);
            this.pwd_TextBox.TabIndex = 21;
            // 
            // id_TextBox
            // 
            this.id_TextBox.Enabled = false;
            this.id_TextBox.Location = new System.Drawing.Point(87, 78);
            this.id_TextBox.Name = "id_TextBox";
            this.id_TextBox.Size = new System.Drawing.Size(121, 23);
            this.id_TextBox.TabIndex = 20;
            // 
            // pwd_Label
            // 
            this.pwd_Label.AutoSize = true;
            this.pwd_Label.Location = new System.Drawing.Point(9, 121);
            this.pwd_Label.Name = "pwd_Label";
            this.pwd_Label.Size = new System.Drawing.Size(63, 15);
            this.pwd_Label.TabIndex = 1;
            this.pwd_Label.Text = "Password :";
            // 
            // id_Label
            // 
            this.id_Label.AutoSize = true;
            this.id_Label.Location = new System.Drawing.Point(9, 83);
            this.id_Label.Name = "id_Label";
            this.id_Label.Size = new System.Drawing.Size(24, 15);
            this.id_Label.TabIndex = 1;
            this.id_Label.Text = "ID :";
            // 
            // jobType_GroupBox
            // 
            this.jobType_GroupBox.Controls.Add(this.scan_RadioButton);
            this.jobType_GroupBox.Controls.Add(this.print_RadioButton);
            this.jobType_GroupBox.Location = new System.Drawing.Point(262, 3);
            this.jobType_GroupBox.Name = "jobType_GroupBox";
            this.jobType_GroupBox.Size = new System.Drawing.Size(200, 113);
            this.jobType_GroupBox.TabIndex = 2;
            this.jobType_GroupBox.TabStop = false;
            this.jobType_GroupBox.Text = "Jop Type";
            // 
            // scan_RadioButton
            // 
            this.scan_RadioButton.AutoSize = true;
            this.scan_RadioButton.Location = new System.Drawing.Point(20, 71);
            this.scan_RadioButton.Name = "scan_RadioButton";
            this.scan_RadioButton.Size = new System.Drawing.Size(50, 19);
            this.scan_RadioButton.TabIndex = 23;
            this.scan_RadioButton.Text = "Scan";
            this.scan_RadioButton.UseVisualStyleBackColor = true;
            this.scan_RadioButton.CheckedChanged += new System.EventHandler(this.scan_RadioButton_CheckedChanged);
            // 
            // print_RadioButton
            // 
            this.print_RadioButton.AutoSize = true;
            this.print_RadioButton.Checked = true;
            this.print_RadioButton.Location = new System.Drawing.Point(20, 33);
            this.print_RadioButton.Name = "print_RadioButton";
            this.print_RadioButton.Size = new System.Drawing.Size(50, 19);
            this.print_RadioButton.TabIndex = 22;
            this.print_RadioButton.TabStop = true;
            this.print_RadioButton.Text = "Print";
            this.print_RadioButton.UseVisualStyleBackColor = true;
            this.print_RadioButton.CheckedChanged += new System.EventHandler(this.print_RadioButton_CheckedChanged);
            // 
            // options_groupBox
            // 
            this.options_groupBox.Controls.Add(this.jobCaption_Label);
            this.options_groupBox.Controls.Add(this.job_Label);
            this.options_groupBox.Controls.Add(this.options_Button);
            this.options_groupBox.Location = new System.Drawing.Point(468, 3);
            this.options_groupBox.Name = "options_groupBox";
            this.options_groupBox.Size = new System.Drawing.Size(269, 113);
            this.options_groupBox.TabIndex = 3;
            this.options_groupBox.TabStop = false;
            this.options_groupBox.Text = "Job Setting";
            // 
            // jobCaption_Label
            // 
            this.jobCaption_Label.AutoSize = true;
            this.jobCaption_Label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobCaption_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.jobCaption_Label.Location = new System.Drawing.Point(15, 33);
            this.jobCaption_Label.Name = "jobCaption_Label";
            this.jobCaption_Label.Size = new System.Drawing.Size(65, 19);
            this.jobCaption_Label.TabIndex = 1;
            this.jobCaption_Label.Text = "Job Type:";
            // 
            // job_Label
            // 
            this.job_Label.AutoSize = true;
            this.job_Label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.job_Label.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.job_Label.Location = new System.Drawing.Point(114, 33);
            this.job_Label.Name = "job_Label";
            this.job_Label.Size = new System.Drawing.Size(41, 19);
            this.job_Label.TabIndex = 1;
            this.job_Label.Text = "Print";
            // 
            // options_Button
            // 
            this.options_Button.Location = new System.Drawing.Point(19, 64);
            this.options_Button.Name = "options_Button";
            this.options_Button.Size = new System.Drawing.Size(130, 41);
            this.options_Button.TabIndex = 24;
            this.options_Button.Text = "Options";
            this.options_Button.UseVisualStyleBackColor = true;
            this.options_Button.Click += new System.EventHandler(this.options_Button_Click);
            // 
            // pageCount_groupBox
            // 
            this.pageCount_groupBox.Controls.Add(this.matter_TextBox);
            this.pageCount_groupBox.Controls.Add(this.matter_Label);
            this.pageCount_groupBox.Controls.Add(this.path_Label);
            this.pageCount_groupBox.Controls.Add(this.path_TextBox);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_Label);
            this.pageCount_groupBox.Controls.Add(this.jobBuildPageCount_NumericUpDown);
            this.pageCount_groupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageCount_groupBox.Location = new System.Drawing.Point(262, 117);
            this.pageCount_groupBox.Name = "pageCount_groupBox";
            this.pageCount_groupBox.Size = new System.Drawing.Size(475, 130);
            this.pageCount_groupBox.TabIndex = 4;
            this.pageCount_groupBox.TabStop = false;
            this.pageCount_groupBox.Text = "Job Description";
            // 
            // matter_TextBox
            // 
            this.matter_TextBox.Location = new System.Drawing.Point(231, 27);
            this.matter_TextBox.Name = "matter_TextBox";
            this.matter_TextBox.Size = new System.Drawing.Size(180, 23);
            this.matter_TextBox.TabIndex = 25;
            // 
            // matter_Label
            // 
            this.matter_Label.AutoSize = true;
            this.matter_Label.Location = new System.Drawing.Point(16, 30);
            this.matter_Label.Name = "matter_Label";
            this.matter_Label.Size = new System.Drawing.Size(88, 15);
            this.matter_Label.TabIndex = 1;
            this.matter_Label.Text = "*Matter Name :";
            // 
            // path_Label
            // 
            this.path_Label.AutoSize = true;
            this.path_Label.Location = new System.Drawing.Point(16, 60);
            this.path_Label.Name = "path_Label";
            this.path_Label.Size = new System.Drawing.Size(63, 15);
            this.path_Label.TabIndex = 1;
            this.path_Label.Text = "* File Path:";
            // 
            // path_TextBox
            // 
            this.path_TextBox.Location = new System.Drawing.Point(232, 60);
            this.path_TextBox.Name = "path_TextBox";
            this.path_TextBox.Size = new System.Drawing.Size(180, 23);
            this.path_TextBox.TabIndex = 26;
            // 
            // jobBuildPageCount_Label
            // 
            this.jobBuildPageCount_Label.AutoSize = true;
            this.jobBuildPageCount_Label.Enabled = false;
            this.jobBuildPageCount_Label.Location = new System.Drawing.Point(16, 90);
            this.jobBuildPageCount_Label.Name = "jobBuildPageCount_Label";
            this.jobBuildPageCount_Label.Size = new System.Drawing.Size(156, 15);
            this.jobBuildPageCount_Label.TabIndex = 1;
            this.jobBuildPageCount_Label.Text = "* Job Build Page Count >= 1";
            // 
            // jobBuildPageCount_NumericUpDown
            // 
            this.jobBuildPageCount_NumericUpDown.Enabled = false;
            this.jobBuildPageCount_NumericUpDown.Location = new System.Drawing.Point(232, 90);
            this.jobBuildPageCount_NumericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.jobBuildPageCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.jobBuildPageCount_NumericUpDown.Name = "jobBuildPageCount_NumericUpDown";
            this.jobBuildPageCount_NumericUpDown.Size = new System.Drawing.Size(180, 23);
            this.jobBuildPageCount_NumericUpDown.TabIndex = 27;
            this.jobBuildPageCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(11, 314);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(726, 183);
            this.assetSelectionControl.TabIndex = 30;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(493, 255);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(244, 53);
            this.lockTimeoutControl.TabIndex = 29;
            // 
            // WorkStorage
            // 
            this.WorkStorage.AutoSize = true;
            this.WorkStorage.Location = new System.Drawing.Point(11, 204);
            this.WorkStorage.Name = "WorkStorage";
            this.WorkStorage.Size = new System.Drawing.Size(59, 15);
            this.WorkStorage.TabIndex = 26;
            this.WorkStorage.Text = "Location :";
            // 
            // destination_comboBox
            // 
            this.destination_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destination_comboBox.FormattingEnabled = true;
            this.destination_comboBox.Location = new System.Drawing.Point(87, 201);
            this.destination_comboBox.Name = "destination_comboBox";
            this.destination_comboBox.Size = new System.Drawing.Size(121, 23);
            this.destination_comboBox.TabIndex = 27;
            // 
            // ClioConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.pageCount_groupBox);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.options_groupBox);
            this.Controls.Add(this.jobType_GroupBox);
            this.Controls.Add(this.login_GroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ClioConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.login_GroupBox.ResumeLayout(false);
            this.login_GroupBox.PerformLayout();
            this.jobType_GroupBox.ResumeLayout(false);
            this.jobType_GroupBox.PerformLayout();
            this.options_groupBox.ResumeLayout(false);
            this.options_groupBox.PerformLayout();
            this.pageCount_groupBox.ResumeLayout(false);
            this.pageCount_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox login_GroupBox;
        private System.Windows.Forms.TextBox pwd_TextBox;
        private System.Windows.Forms.TextBox id_TextBox;
        private System.Windows.Forms.Label pwd_Label;
        private System.Windows.Forms.Label id_Label;
        private System.Windows.Forms.GroupBox jobType_GroupBox;
        private System.Windows.Forms.RadioButton scan_RadioButton;
        private System.Windows.Forms.RadioButton print_RadioButton;
        private System.Windows.Forms.GroupBox options_groupBox;
        private System.Windows.Forms.Label jobCaption_Label;
        private System.Windows.Forms.Label job_Label;
        private System.Windows.Forms.Button options_Button;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox pageCount_groupBox;
        private System.Windows.Forms.Label path_Label;
        private System.Windows.Forms.TextBox path_TextBox;
        private System.Windows.Forms.Label jobBuildPageCount_Label;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_NumericUpDown;
        private System.Windows.Forms.TextBox matter_TextBox;
        private System.Windows.Forms.Label matter_Label;
        private System.Windows.Forms.Label label_SignOut;
        private System.Windows.Forms.ComboBox comboBox_Logout;
        private System.Windows.Forms.Label label_SIO;
        private System.Windows.Forms.ComboBox comboBox_SIO;
        private System.Windows.Forms.Label WorkStorage;
        private System.Windows.Forms.ComboBox destination_comboBox;
    }
}
