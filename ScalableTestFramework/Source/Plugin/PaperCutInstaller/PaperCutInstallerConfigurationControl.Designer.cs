namespace HP.ScalableTest.Plugin.PaperCutInstaller
{
    partial class PaperCutInstallerConfigurationControl
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
            this.paperCut_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.paperCut_groupBox = new System.Windows.Forms.GroupBox();
            this.checkBox_guest = new System.Windows.Forms.CheckBox();
            this.queue_groupBox = new System.Windows.Forms.GroupBox();
            this.label_queue = new System.Windows.Forms.Label();
            this.checkBox_AutoRelease = new System.Windows.Forms.CheckBox();
            this.textBox_QueueName = new System.Windows.Forms.TextBox();
            this.paperCut_serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.register_groupBox = new System.Windows.Forms.GroupBox();
            this.password_label = new System.Windows.Forms.Label();
            this.userLogin_label = new System.Windows.Forms.Label();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.logon_textBox = new System.Windows.Forms.TextBox();
            this.label_PaperCutServer = new System.Windows.Forms.Label();
            this.tracking_groupBox = new System.Windows.Forms.GroupBox();
            this.checkbox_TrackFax = new System.Windows.Forms.CheckBox();
            this.checkbox_TrackScan = new System.Windows.Forms.CheckBox();
            this.checkbox_TrackPrint = new System.Windows.Forms.CheckBox();
            this.login_groupBox = new System.Windows.Forms.GroupBox();
            this.radioButton_identity = new System.Windows.Forms.RadioButton();
            this.radioButton_password = new System.Windows.Forms.RadioButton();
            this.checkbox_Card = new System.Windows.Forms.CheckBox();
            this.tasks_comboBox = new System.Windows.Forms.ComboBox();
            this.tasks_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.browse_button = new System.Windows.Forms.Button();
            this.bundleFile_textBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.paperCut_groupBox.SuspendLayout();
            this.queue_groupBox.SuspendLayout();
            this.register_groupBox.SuspendLayout();
            this.tracking_groupBox.SuspendLayout();
            this.login_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // paperCut_assetSelectionControl
            // 
            this.paperCut_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paperCut_assetSelectionControl.Location = new System.Drawing.Point(0, 0);
            this.paperCut_assetSelectionControl.Name = "paperCut_assetSelectionControl";
            this.paperCut_assetSelectionControl.Size = new System.Drawing.Size(656, 163);
            this.paperCut_assetSelectionControl.TabIndex = 1;
            // 
            // paperCut_groupBox
            // 
            this.paperCut_groupBox.Controls.Add(this.queue_groupBox);
            this.paperCut_groupBox.Controls.Add(this.paperCut_serverComboBox);
            this.paperCut_groupBox.Controls.Add(this.register_groupBox);
            this.paperCut_groupBox.Controls.Add(this.label_PaperCutServer);
            this.paperCut_groupBox.Controls.Add(this.tracking_groupBox);
            this.paperCut_groupBox.Controls.Add(this.login_groupBox);
            this.paperCut_groupBox.Controls.Add(this.tasks_comboBox);
            this.paperCut_groupBox.Controls.Add(this.tasks_label);
            this.paperCut_groupBox.Controls.Add(this.label1);
            this.paperCut_groupBox.Controls.Add(this.browse_button);
            this.paperCut_groupBox.Controls.Add(this.bundleFile_textBox);
            this.paperCut_groupBox.Location = new System.Drawing.Point(3, 178);
            this.paperCut_groupBox.Name = "paperCut_groupBox";
            this.paperCut_groupBox.Size = new System.Drawing.Size(656, 388);
            this.paperCut_groupBox.TabIndex = 2;
            this.paperCut_groupBox.TabStop = false;
            this.paperCut_groupBox.Text = "PaperCut Adminstration";
            // 
            // checkBox_guest
            // 
            this.checkBox_guest.AutoSize = true;
            this.checkBox_guest.Location = new System.Drawing.Point(6, 105);
            this.checkBox_guest.Name = "checkBox_guest";
            this.checkBox_guest.Size = new System.Drawing.Size(161, 17);
            this.checkBox_guest.TabIndex = 12;
            this.checkBox_guest.Tag = "8";
            this.checkBox_guest.Text = "Enable Guest Authentication";
            this.checkBox_guest.UseVisualStyleBackColor = true;
            // 
            // queue_groupBox
            // 
            this.queue_groupBox.Controls.Add(this.label_queue);
            this.queue_groupBox.Controls.Add(this.checkBox_AutoRelease);
            this.queue_groupBox.Controls.Add(this.textBox_QueueName);
            this.queue_groupBox.Location = new System.Drawing.Point(14, 148);
            this.queue_groupBox.Name = "queue_groupBox";
            this.queue_groupBox.Size = new System.Drawing.Size(251, 142);
            this.queue_groupBox.TabIndex = 13;
            this.queue_groupBox.TabStop = false;
            this.queue_groupBox.Text = "Print Queue Settings";
            // 
            // label_queue
            // 
            this.label_queue.AutoSize = true;
            this.label_queue.Location = new System.Drawing.Point(6, 23);
            this.label_queue.Name = "label_queue";
            this.label_queue.Size = new System.Drawing.Size(94, 13);
            this.label_queue.TabIndex = 5;
            this.label_queue.Text = "Print Queue Name";
            // 
            // checkBox_AutoRelease
            // 
            this.checkBox_AutoRelease.AutoSize = true;
            this.checkBox_AutoRelease.Location = new System.Drawing.Point(9, 67);
            this.checkBox_AutoRelease.Name = "checkBox_AutoRelease";
            this.checkBox_AutoRelease.Size = new System.Drawing.Size(126, 17);
            this.checkBox_AutoRelease.TabIndex = 10;
            this.checkBox_AutoRelease.Text = "Enable Auto Release";
            this.checkBox_AutoRelease.UseVisualStyleBackColor = true;
            // 
            // textBox_QueueName
            // 
            this.textBox_QueueName.Location = new System.Drawing.Point(9, 41);
            this.textBox_QueueName.Name = "textBox_QueueName";
            this.textBox_QueueName.Size = new System.Drawing.Size(236, 20);
            this.textBox_QueueName.TabIndex = 4;
            // 
            // paperCut_serverComboBox
            // 
            this.paperCut_serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paperCut_serverComboBox.Location = new System.Drawing.Point(8, 89);
            this.paperCut_serverComboBox.Name = "paperCut_serverComboBox";
            this.paperCut_serverComboBox.Size = new System.Drawing.Size(210, 23);
            this.paperCut_serverComboBox.TabIndex = 16;
            // 
            // register_groupBox
            // 
            this.register_groupBox.Controls.Add(this.password_label);
            this.register_groupBox.Controls.Add(this.userLogin_label);
            this.register_groupBox.Controls.Add(this.password_textBox);
            this.register_groupBox.Controls.Add(this.logon_textBox);
            this.register_groupBox.Location = new System.Drawing.Point(233, 66);
            this.register_groupBox.Name = "register_groupBox";
            this.register_groupBox.Size = new System.Drawing.Size(387, 66);
            this.register_groupBox.TabIndex = 15;
            this.register_groupBox.TabStop = false;
            this.register_groupBox.Text = "Administrator Credentials";
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(184, 19);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(56, 13);
            this.password_label.TabIndex = 3;
            this.password_label.Text = "Password:";
            // 
            // userLogin_label
            // 
            this.userLogin_label.AutoSize = true;
            this.userLogin_label.Location = new System.Drawing.Point(8, 19);
            this.userLogin_label.Name = "userLogin_label";
            this.userLogin_label.Size = new System.Drawing.Size(65, 13);
            this.userLogin_label.TabIndex = 2;
            this.userLogin_label.Text = "User Logon:";
            // 
            // password_textBox
            // 
            this.password_textBox.Location = new System.Drawing.Point(187, 37);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(170, 20);
            this.password_textBox.TabIndex = 1;
            // 
            // logon_textBox
            // 
            this.logon_textBox.Location = new System.Drawing.Point(11, 37);
            this.logon_textBox.Name = "logon_textBox";
            this.logon_textBox.Size = new System.Drawing.Size(170, 20);
            this.logon_textBox.TabIndex = 0;
            // 
            // label_PaperCutServer
            // 
            this.label_PaperCutServer.AutoSize = true;
            this.label_PaperCutServer.Location = new System.Drawing.Point(11, 71);
            this.label_PaperCutServer.Name = "label_PaperCutServer";
            this.label_PaperCutServer.Size = new System.Drawing.Size(85, 13);
            this.label_PaperCutServer.TabIndex = 14;
            this.label_PaperCutServer.Text = "PaperCut Server";
            // 
            // tracking_groupBox
            // 
            this.tracking_groupBox.Controls.Add(this.checkbox_TrackFax);
            this.tracking_groupBox.Controls.Add(this.checkbox_TrackScan);
            this.tracking_groupBox.Controls.Add(this.checkbox_TrackPrint);
            this.tracking_groupBox.Location = new System.Drawing.Point(458, 148);
            this.tracking_groupBox.Name = "tracking_groupBox";
            this.tracking_groupBox.Size = new System.Drawing.Size(162, 142);
            this.tracking_groupBox.TabIndex = 7;
            this.tracking_groupBox.TabStop = false;
            this.tracking_groupBox.Text = "Tracking Settings";
            // 
            // checkbox_TrackFax
            // 
            this.checkbox_TrackFax.AutoSize = true;
            this.checkbox_TrackFax.Location = new System.Drawing.Point(6, 74);
            this.checkbox_TrackFax.Name = "checkbox_TrackFax";
            this.checkbox_TrackFax.Size = new System.Drawing.Size(74, 17);
            this.checkbox_TrackFax.TabIndex = 12;
            this.checkbox_TrackFax.Tag = "4";
            this.checkbox_TrackFax.Text = "Track Fax";
            this.checkbox_TrackFax.UseVisualStyleBackColor = true;
            // 
            // checkbox_TrackScan
            // 
            this.checkbox_TrackScan.AutoSize = true;
            this.checkbox_TrackScan.Location = new System.Drawing.Point(6, 48);
            this.checkbox_TrackScan.Name = "checkbox_TrackScan";
            this.checkbox_TrackScan.Size = new System.Drawing.Size(82, 17);
            this.checkbox_TrackScan.TabIndex = 11;
            this.checkbox_TrackScan.Tag = "2";
            this.checkbox_TrackScan.Text = "Track Scan";
            this.checkbox_TrackScan.UseVisualStyleBackColor = true;
            // 
            // checkbox_TrackPrint
            // 
            this.checkbox_TrackPrint.AutoSize = true;
            this.checkbox_TrackPrint.Location = new System.Drawing.Point(6, 22);
            this.checkbox_TrackPrint.Name = "checkbox_TrackPrint";
            this.checkbox_TrackPrint.Size = new System.Drawing.Size(78, 17);
            this.checkbox_TrackPrint.TabIndex = 10;
            this.checkbox_TrackPrint.Tag = "1";
            this.checkbox_TrackPrint.Text = "Track Print";
            this.checkbox_TrackPrint.UseVisualStyleBackColor = true;
            // 
            // login_groupBox
            // 
            this.login_groupBox.Controls.Add(this.checkBox_guest);
            this.login_groupBox.Controls.Add(this.radioButton_identity);
            this.login_groupBox.Controls.Add(this.radioButton_password);
            this.login_groupBox.Controls.Add(this.checkbox_Card);
            this.login_groupBox.Location = new System.Drawing.Point(271, 148);
            this.login_groupBox.Name = "login_groupBox";
            this.login_groupBox.Size = new System.Drawing.Size(181, 142);
            this.login_groupBox.TabIndex = 5;
            this.login_groupBox.TabStop = false;
            this.login_groupBox.Text = "Login Settings";
            // 
            // radioButton_identity
            // 
            this.radioButton_identity.AutoSize = true;
            this.radioButton_identity.Location = new System.Drawing.Point(6, 57);
            this.radioButton_identity.Name = "radioButton_identity";
            this.radioButton_identity.Size = new System.Drawing.Size(59, 17);
            this.radioButton_identity.TabIndex = 13;
            this.radioButton_identity.TabStop = true;
            this.radioButton_identity.Text = "Identity";
            this.radioButton_identity.UseVisualStyleBackColor = true;
            // 
            // radioButton_password
            // 
            this.radioButton_password.AutoSize = true;
            this.radioButton_password.Location = new System.Drawing.Point(6, 32);
            this.radioButton_password.Name = "radioButton_password";
            this.radioButton_password.Size = new System.Drawing.Size(124, 17);
            this.radioButton_password.TabIndex = 12;
            this.radioButton_password.TabStop = true;
            this.radioButton_password.Text = "Username/Password";
            this.radioButton_password.UseVisualStyleBackColor = true;
            // 
            // checkbox_Card
            // 
            this.checkbox_Card.AutoSize = true;
            this.checkbox_Card.Location = new System.Drawing.Point(6, 82);
            this.checkbox_Card.Name = "checkbox_Card";
            this.checkbox_Card.Size = new System.Drawing.Size(80, 17);
            this.checkbox_Card.TabIndex = 11;
            this.checkbox_Card.Tag = "4";
            this.checkbox_Card.Text = "Swipe Card";
            this.checkbox_Card.UseVisualStyleBackColor = true;
            // 
            // tasks_comboBox
            // 
            this.tasks_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tasks_comboBox.FormattingEnabled = true;
            this.tasks_comboBox.Location = new System.Drawing.Point(9, 37);
            this.tasks_comboBox.Name = "tasks_comboBox";
            this.tasks_comboBox.Size = new System.Drawing.Size(184, 21);
            this.tasks_comboBox.TabIndex = 4;
            this.tasks_comboBox.SelectedIndexChanged += new System.EventHandler(this.tasks_comboBox_SelectedIndexChanged);
            // 
            // tasks_label
            // 
            this.tasks_label.AutoSize = true;
            this.tasks_label.Location = new System.Drawing.Point(6, 19);
            this.tasks_label.Name = "tasks_label";
            this.tasks_label.Size = new System.Drawing.Size(99, 13);
            this.tasks_label.TabIndex = 3;
            this.tasks_label.Text = "Administration Task";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "PaperCut Bundle File";
            // 
            // browse_button
            // 
            this.browse_button.Location = new System.Drawing.Point(571, 37);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(75, 23);
            this.browse_button.TabIndex = 1;
            this.browse_button.Text = "Browse...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.browse_button_Click);
            // 
            // bundleFile_textBox
            // 
            this.bundleFile_textBox.BackColor = System.Drawing.Color.White;
            this.bundleFile_textBox.Location = new System.Drawing.Point(233, 37);
            this.bundleFile_textBox.Name = "bundleFile_textBox";
            this.bundleFile_textBox.ReadOnly = true;
            this.bundleFile_textBox.Size = new System.Drawing.Size(335, 20);
            this.bundleFile_textBox.TabIndex = 0;
            // 
            // PaperCutInstallerConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.paperCut_groupBox);
            this.Controls.Add(this.paperCut_assetSelectionControl);
            this.Name = "PaperCutInstallerConfigurationControl";
            this.Size = new System.Drawing.Size(687, 574);
            this.paperCut_groupBox.ResumeLayout(false);
            this.paperCut_groupBox.PerformLayout();
            this.queue_groupBox.ResumeLayout(false);
            this.queue_groupBox.PerformLayout();
            this.register_groupBox.ResumeLayout(false);
            this.register_groupBox.PerformLayout();
            this.tracking_groupBox.ResumeLayout(false);
            this.tracking_groupBox.PerformLayout();
            this.login_groupBox.ResumeLayout(false);
            this.login_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.AssetSelectionControl paperCut_assetSelectionControl;
        private System.Windows.Forms.GroupBox paperCut_groupBox;
        private Framework.UI.ServerComboBox paperCut_serverComboBox;
        private System.Windows.Forms.GroupBox register_groupBox;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.Label userLogin_label;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.TextBox logon_textBox;
        private System.Windows.Forms.Label label_PaperCutServer;
        private System.Windows.Forms.GroupBox tracking_groupBox;
        private System.Windows.Forms.CheckBox checkbox_TrackFax;
        private System.Windows.Forms.CheckBox checkbox_TrackScan;
        private System.Windows.Forms.CheckBox checkbox_TrackPrint;
        private System.Windows.Forms.GroupBox login_groupBox;
        private System.Windows.Forms.CheckBox checkbox_Card;
        private System.Windows.Forms.ComboBox tasks_comboBox;
        private System.Windows.Forms.Label tasks_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox bundleFile_textBox;
        private System.Windows.Forms.CheckBox checkBox_guest;
        private System.Windows.Forms.GroupBox queue_groupBox;
        private System.Windows.Forms.Label label_queue;
        private System.Windows.Forms.CheckBox checkBox_AutoRelease;
        private System.Windows.Forms.TextBox textBox_QueueName;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.RadioButton radioButton_identity;
        private System.Windows.Forms.RadioButton radioButton_password;
    }
}
