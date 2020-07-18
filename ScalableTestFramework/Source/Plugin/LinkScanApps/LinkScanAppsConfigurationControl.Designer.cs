namespace HP.ScalableTest.Plugin.LinkScanApps
{
    partial class LinkScanAppsConfigurationControl
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
            this.destination_label = new System.Windows.Forms.Label();
            this.destination_comboBox = new System.Windows.Forms.ComboBox();
            this.scantoEmail_groupBox = new System.Windows.Forms.GroupBox();
            this.message_textBox = new System.Windows.Forms.TextBox();
            this.emailfrom_label = new System.Windows.Forms.Label();
            this.subject_textBox = new System.Windows.Forms.TextBox();
            this.emailTo_label = new System.Windows.Forms.Label();
            this.bcc_textBox = new System.Windows.Forms.TextBox();
            this.mailcc_label = new System.Windows.Forms.Label();
            this.cc_textBox = new System.Windows.Forms.TextBox();
            this.bcc_label = new System.Windows.Forms.Label();
            this.to_textBox = new System.Windows.Forms.TextBox();
            this.from_textBox = new System.Windows.Forms.TextBox();
            this.fileName_textBox = new System.Windows.Forms.TextBox();
            this.buildPageCount_groupBox = new System.Windows.Forms.GroupBox();
            this.pageCountCaption_label = new System.Windows.Forms.Label();
            this.pageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pageCount_label = new System.Windows.Forms.Label();
            this.options_button = new System.Windows.Forms.Button();
            this.scantoNetwork_groupBox = new System.Windows.Forms.GroupBox();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.domainPort_label = new System.Windows.Forms.Label();
            this.password_label = new System.Windows.Forms.Label();
            this.password_textBox = new System.Windows.Forms.TextBox();
            this.folderPath_textBox = new System.Windows.Forms.TextBox();
            this.userName_textBox = new System.Windows.Forms.TextBox();
            this.folderpath_label = new System.Windows.Forms.Label();
            this.userName_label = new System.Windows.Forms.Label();
            this.server_textBox = new System.Windows.Forms.TextBox();
            this.server_label = new System.Windows.Forms.Label();
            this.fileName_checkBox = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.subject_checkBox = new System.Windows.Forms.CheckBox();
            this.message_checkBox = new System.Windows.Forms.CheckBox();
            this.scantoEmail_groupBox.SuspendLayout();
            this.buildPageCount_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_numericUpDown)).BeginInit();
            this.scantoNetwork_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // destination_label
            // 
            this.destination_label.AutoSize = true;
            this.destination_label.Location = new System.Drawing.Point(33, 13);
            this.destination_label.Name = "destination_label";
            this.destination_label.Size = new System.Drawing.Size(88, 20);
            this.destination_label.TabIndex = 0;
            this.destination_label.Text = "Destination:";
            // 
            // destination_comboBox
            // 
            this.destination_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.destination_comboBox.FormattingEnabled = true;
            this.destination_comboBox.Location = new System.Drawing.Point(137, 10);
            this.destination_comboBox.Name = "destination_comboBox";
            this.destination_comboBox.Size = new System.Drawing.Size(185, 28);
            this.destination_comboBox.TabIndex = 1;
            this.destination_comboBox.SelectedIndexChanged += new System.EventHandler(this.destination_comboBox_SelectedIndexChanged);
            // 
            // scantoEmail_groupBox
            // 
            this.scantoEmail_groupBox.Controls.Add(this.message_checkBox);
            this.scantoEmail_groupBox.Controls.Add(this.subject_checkBox);
            this.scantoEmail_groupBox.Controls.Add(this.message_textBox);
            this.scantoEmail_groupBox.Controls.Add(this.emailfrom_label);
            this.scantoEmail_groupBox.Controls.Add(this.subject_textBox);
            this.scantoEmail_groupBox.Controls.Add(this.emailTo_label);
            this.scantoEmail_groupBox.Controls.Add(this.bcc_textBox);
            this.scantoEmail_groupBox.Controls.Add(this.mailcc_label);
            this.scantoEmail_groupBox.Controls.Add(this.cc_textBox);
            this.scantoEmail_groupBox.Controls.Add(this.bcc_label);
            this.scantoEmail_groupBox.Controls.Add(this.to_textBox);
            this.scantoEmail_groupBox.Controls.Add(this.from_textBox);
            this.scantoEmail_groupBox.Location = new System.Drawing.Point(15, 74);
            this.scantoEmail_groupBox.Name = "scantoEmail_groupBox";
            this.scantoEmail_groupBox.Size = new System.Drawing.Size(443, 157);
            this.scantoEmail_groupBox.TabIndex = 3;
            this.scantoEmail_groupBox.TabStop = false;
            this.scantoEmail_groupBox.Text = "Scan to Email";
            // 
            // message_textBox
            // 
            this.message_textBox.Enabled = false;
            this.message_textBox.Location = new System.Drawing.Point(266, 53);
            this.message_textBox.Multiline = true;
            this.message_textBox.Name = "message_textBox";
            this.message_textBox.Size = new System.Drawing.Size(169, 93);
            this.message_textBox.TabIndex = 26;
            // 
            // emailfrom_label
            // 
            this.emailfrom_label.AutoSize = true;
            this.emailfrom_label.Location = new System.Drawing.Point(1, 23);
            this.emailfrom_label.Name = "emailfrom_label";
            this.emailfrom_label.Size = new System.Drawing.Size(46, 20);
            this.emailfrom_label.TabIndex = 15;
            this.emailfrom_label.Text = "From:";
            // 
            // subject_textBox
            // 
            this.subject_textBox.Enabled = false;
            this.subject_textBox.Location = new System.Drawing.Point(265, 20);
            this.subject_textBox.Name = "subject_textBox";
            this.subject_textBox.Size = new System.Drawing.Size(169, 27);
            this.subject_textBox.TabIndex = 25;
            // 
            // emailTo_label
            // 
            this.emailTo_label.AutoSize = true;
            this.emailTo_label.Location = new System.Drawing.Point(2, 56);
            this.emailTo_label.Name = "emailTo_label";
            this.emailTo_label.Size = new System.Drawing.Size(28, 20);
            this.emailTo_label.TabIndex = 16;
            this.emailTo_label.Text = "To:";
            // 
            // bcc_textBox
            // 
            this.bcc_textBox.Location = new System.Drawing.Point(47, 119);
            this.bcc_textBox.Name = "bcc_textBox";
            this.bcc_textBox.Size = new System.Drawing.Size(120, 27);
            this.bcc_textBox.TabIndex = 24;
            // 
            // mailcc_label
            // 
            this.mailcc_label.AutoSize = true;
            this.mailcc_label.Location = new System.Drawing.Point(2, 89);
            this.mailcc_label.Name = "mailcc_label";
            this.mailcc_label.Size = new System.Drawing.Size(28, 20);
            this.mailcc_label.TabIndex = 17;
            this.mailcc_label.Text = "Cc:";
            // 
            // cc_textBox
            // 
            this.cc_textBox.Location = new System.Drawing.Point(47, 86);
            this.cc_textBox.Name = "cc_textBox";
            this.cc_textBox.Size = new System.Drawing.Size(120, 27);
            this.cc_textBox.TabIndex = 23;
            // 
            // bcc_label
            // 
            this.bcc_label.AutoSize = true;
            this.bcc_label.Location = new System.Drawing.Point(2, 122);
            this.bcc_label.Name = "bcc_label";
            this.bcc_label.Size = new System.Drawing.Size(35, 20);
            this.bcc_label.TabIndex = 18;
            this.bcc_label.Text = "Bcc:";
            // 
            // to_textBox
            // 
            this.to_textBox.Location = new System.Drawing.Point(47, 53);
            this.to_textBox.Name = "to_textBox";
            this.to_textBox.Size = new System.Drawing.Size(120, 27);
            this.to_textBox.TabIndex = 22;
            // 
            // from_textBox
            // 
            this.from_textBox.Location = new System.Drawing.Point(47, 20);
            this.from_textBox.Name = "from_textBox";
            this.from_textBox.Size = new System.Drawing.Size(120, 27);
            this.from_textBox.TabIndex = 21;
            // 
            // fileName_textBox
            // 
            this.fileName_textBox.Enabled = false;
            this.fileName_textBox.Location = new System.Drawing.Point(137, 44);
            this.fileName_textBox.Name = "fileName_textBox";
            this.fileName_textBox.Size = new System.Drawing.Size(287, 27);
            this.fileName_textBox.TabIndex = 2;
            // 
            // buildPageCount_groupBox
            // 
            this.buildPageCount_groupBox.Controls.Add(this.pageCountCaption_label);
            this.buildPageCount_groupBox.Controls.Add(this.pageCount_numericUpDown);
            this.buildPageCount_groupBox.Controls.Add(this.pageCount_label);
            this.buildPageCount_groupBox.Location = new System.Drawing.Point(464, 1);
            this.buildPageCount_groupBox.Name = "buildPageCount_groupBox";
            this.buildPageCount_groupBox.Size = new System.Drawing.Size(234, 136);
            this.buildPageCount_groupBox.TabIndex = 40;
            this.buildPageCount_groupBox.TabStop = false;
            this.buildPageCount_groupBox.Text = "Job Build page Count";
            // 
            // pageCountCaption_label
            // 
            this.pageCountCaption_label.AutoSize = true;
            this.pageCountCaption_label.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pageCountCaption_label.Location = new System.Drawing.Point(20, 23);
            this.pageCountCaption_label.Name = "pageCountCaption_label";
            this.pageCountCaption_label.Size = new System.Drawing.Size(193, 40);
            this.pageCountCaption_label.TabIndex = 2;
            this.pageCountCaption_label.Text = "Select the number of Pages \r\nfor the scanned document.";
            // 
            // pageCount_numericUpDown
            // 
            this.pageCount_numericUpDown.Location = new System.Drawing.Point(95, 99);
            this.pageCount_numericUpDown.Name = "pageCount_numericUpDown";
            this.pageCount_numericUpDown.Size = new System.Drawing.Size(120, 27);
            this.pageCount_numericUpDown.TabIndex = 1;
            // 
            // pageCount_label
            // 
            this.pageCount_label.AutoSize = true;
            this.pageCount_label.Location = new System.Drawing.Point(20, 76);
            this.pageCount_label.Name = "pageCount_label";
            this.pageCount_label.Size = new System.Drawing.Size(195, 20);
            this.pageCount_label.TabIndex = 0;
            this.pageCount_label.Text = "* Job Build Page Count >= 1";
            // 
            // options_button
            // 
            this.options_button.Location = new System.Drawing.Point(328, 10);
            this.options_button.Name = "options_button";
            this.options_button.Size = new System.Drawing.Size(96, 28);
            this.options_button.TabIndex = 41;
            this.options_button.Text = "Options";
            this.options_button.UseVisualStyleBackColor = true;
            this.options_button.Click += new System.EventHandler(this.options_button_Click);
            // 
            // scantoNetwork_groupBox
            // 
            this.scantoNetwork_groupBox.Controls.Add(this.port_textBox);
            this.scantoNetwork_groupBox.Controls.Add(this.domainPort_label);
            this.scantoNetwork_groupBox.Controls.Add(this.password_label);
            this.scantoNetwork_groupBox.Controls.Add(this.password_textBox);
            this.scantoNetwork_groupBox.Controls.Add(this.folderPath_textBox);
            this.scantoNetwork_groupBox.Controls.Add(this.userName_textBox);
            this.scantoNetwork_groupBox.Controls.Add(this.folderpath_label);
            this.scantoNetwork_groupBox.Controls.Add(this.userName_label);
            this.scantoNetwork_groupBox.Controls.Add(this.server_textBox);
            this.scantoNetwork_groupBox.Controls.Add(this.server_label);
            this.scantoNetwork_groupBox.Enabled = false;
            this.scantoNetwork_groupBox.Location = new System.Drawing.Point(15, 74);
            this.scantoNetwork_groupBox.Name = "scantoNetwork_groupBox";
            this.scantoNetwork_groupBox.Size = new System.Drawing.Size(433, 188);
            this.scantoNetwork_groupBox.TabIndex = 42;
            this.scantoNetwork_groupBox.TabStop = false;
            this.scantoNetwork_groupBox.Text = "Scan to Network Folder";
            this.scantoNetwork_groupBox.Visible = false;
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(122, 118);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(287, 27);
            this.port_textBox.TabIndex = 23;
            // 
            // domainPort_label
            // 
            this.domainPort_label.AutoSize = true;
            this.domainPort_label.Location = new System.Drawing.Point(18, 122);
            this.domainPort_label.Name = "domainPort_label";
            this.domainPort_label.Size = new System.Drawing.Size(38, 20);
            this.domainPort_label.TabIndex = 49;
            this.domainPort_label.Text = "Port:";
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(18, 91);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(73, 20);
            this.password_label.TabIndex = 48;
            this.password_label.Text = "Password:";
            // 
            // password_textBox
            // 
            this.password_textBox.Location = new System.Drawing.Point(122, 88);
            this.password_textBox.Name = "password_textBox";
            this.password_textBox.Size = new System.Drawing.Size(287, 27);
            this.password_textBox.TabIndex = 22;
            // 
            // folderPath_textBox
            // 
            this.folderPath_textBox.Location = new System.Drawing.Point(122, 151);
            this.folderPath_textBox.Name = "folderPath_textBox";
            this.folderPath_textBox.Size = new System.Drawing.Size(287, 27);
            this.folderPath_textBox.TabIndex = 24;
            // 
            // userName_textBox
            // 
            this.userName_textBox.Location = new System.Drawing.Point(122, 57);
            this.userName_textBox.Name = "userName_textBox";
            this.userName_textBox.Size = new System.Drawing.Size(287, 27);
            this.userName_textBox.TabIndex = 21;
            // 
            // folderpath_label
            // 
            this.folderpath_label.AutoSize = true;
            this.folderpath_label.Location = new System.Drawing.Point(18, 154);
            this.folderpath_label.Name = "folderpath_label";
            this.folderpath_label.Size = new System.Drawing.Size(86, 20);
            this.folderpath_label.TabIndex = 44;
            this.folderpath_label.Text = "Folder Path:";
            // 
            // userName_label
            // 
            this.userName_label.AutoSize = true;
            this.userName_label.Location = new System.Drawing.Point(18, 60);
            this.userName_label.Name = "userName_label";
            this.userName_label.Size = new System.Drawing.Size(85, 20);
            this.userName_label.TabIndex = 43;
            this.userName_label.Text = "User Name:";
            // 
            // server_textBox
            // 
            this.server_textBox.Location = new System.Drawing.Point(122, 24);
            this.server_textBox.Name = "server_textBox";
            this.server_textBox.Size = new System.Drawing.Size(287, 27);
            this.server_textBox.TabIndex = 20;
            // 
            // server_label
            // 
            this.server_label.AutoSize = true;
            this.server_label.Location = new System.Drawing.Point(18, 27);
            this.server_label.Name = "server_label";
            this.server_label.Size = new System.Drawing.Size(53, 20);
            this.server_label.TabIndex = 0;
            this.server_label.Text = "Server:";
            // 
            // fileName_checkBox
            // 
            this.fileName_checkBox.AutoSize = true;
            this.fileName_checkBox.Location = new System.Drawing.Point(34, 44);
            this.fileName_checkBox.Name = "fileName_checkBox";
            this.fileName_checkBox.Size = new System.Drawing.Size(97, 24);
            this.fileName_checkBox.TabIndex = 43;
            this.fileName_checkBox.Text = "FileName:";
            this.fileName_checkBox.UseVisualStyleBackColor = true;
            this.fileName_checkBox.CheckedChanged += new System.EventHandler(this.checkBox_FileName_CheckedChanged);
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(457, 192);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 50);
            this.lockTimeoutControl.TabIndex = 27;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(10, 271);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(689, 115);
            this.assetSelectionControl.TabIndex = 2;
            // 
            // subject_checkBox
            // 
            this.subject_checkBox.AutoSize = true;
            this.subject_checkBox.Location = new System.Drawing.Point(174, 22);
            this.subject_checkBox.Name = "subject_checkBox";
            this.subject_checkBox.Size = new System.Drawing.Size(83, 24);
            this.subject_checkBox.TabIndex = 27;
            this.subject_checkBox.Text = "Subject:";
            this.subject_checkBox.UseVisualStyleBackColor = true;
            this.subject_checkBox.CheckedChanged += new System.EventHandler(this.subject_checkBox_CheckedChanged);
            // 
            // message_checkBox
            // 
            this.message_checkBox.AutoSize = true;
            this.message_checkBox.Location = new System.Drawing.Point(174, 53);
            this.message_checkBox.Name = "message_checkBox";
            this.message_checkBox.Size = new System.Drawing.Size(92, 24);
            this.message_checkBox.TabIndex = 28;
            this.message_checkBox.Text = "Message:";
            this.message_checkBox.UseVisualStyleBackColor = true;
            this.message_checkBox.CheckedChanged += new System.EventHandler(this.message_checkBox_CheckedChanged);
            // 
            // LinkScanAppsConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileName_checkBox);
            this.Controls.Add(this.options_button);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.buildPageCount_groupBox);
            this.Controls.Add(this.fileName_textBox);
            this.Controls.Add(this.scantoEmail_groupBox);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.destination_comboBox);
            this.Controls.Add(this.destination_label);
            this.Controls.Add(this.scantoNetwork_groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LinkScanAppsConfigurationControl";
            this.Size = new System.Drawing.Size(710, 400);
            this.scantoEmail_groupBox.ResumeLayout(false);
            this.scantoEmail_groupBox.PerformLayout();
            this.buildPageCount_groupBox.ResumeLayout(false);
            this.buildPageCount_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_numericUpDown)).EndInit();
            this.scantoNetwork_groupBox.ResumeLayout(false);
            this.scantoNetwork_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label destination_label;
        private System.Windows.Forms.ComboBox destination_comboBox;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox scantoEmail_groupBox;
        private System.Windows.Forms.TextBox message_textBox;
        private System.Windows.Forms.Label emailfrom_label;
        private System.Windows.Forms.TextBox subject_textBox;
        private System.Windows.Forms.Label emailTo_label;
        private System.Windows.Forms.TextBox bcc_textBox;
        private System.Windows.Forms.Label mailcc_label;
        private System.Windows.Forms.TextBox cc_textBox;
        private System.Windows.Forms.Label bcc_label;
        private System.Windows.Forms.TextBox to_textBox;
        private System.Windows.Forms.TextBox from_textBox;
        private System.Windows.Forms.TextBox fileName_textBox;
        private System.Windows.Forms.GroupBox buildPageCount_groupBox;
        private System.Windows.Forms.NumericUpDown pageCount_numericUpDown;
        private System.Windows.Forms.Label pageCount_label;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.Button options_button;
        private System.Windows.Forms.GroupBox scantoNetwork_groupBox;
        private System.Windows.Forms.Label server_label;
        private System.Windows.Forms.TextBox server_textBox;
        private System.Windows.Forms.TextBox password_textBox;
        private System.Windows.Forms.TextBox folderPath_textBox;
        private System.Windows.Forms.TextBox userName_textBox;
        private System.Windows.Forms.Label folderpath_label;
        private System.Windows.Forms.Label userName_label;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.Label pageCountCaption_label;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Label domainPort_label;
        private System.Windows.Forms.CheckBox fileName_checkBox;
        private System.Windows.Forms.CheckBox message_checkBox;
        private System.Windows.Forms.CheckBox subject_checkBox;
    }
}
