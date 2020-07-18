namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class SettingsUserControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.Option_TabPage = new System.Windows.Forms.TabPage();
            this.AddMultipleServer_groupBox = new System.Windows.Forms.GroupBox();
            this.ServerName_TextBox = new System.Windows.Forms.TextBox();
            this.ServerURI_label = new System.Windows.Forms.Label();
            this.OtherOptions_GroupBox = new System.Windows.Forms.GroupBox();
            this.Encryption_CheckBox = new System.Windows.Forms.CheckBox();
            this.ProtocolOptions_label = new System.Windows.Forms.Label();
            this.ProtocolOptions_comboBox = new System.Windows.Forms.ComboBox();
            this.quota_TabPage = new System.Windows.Forms.TabPage();
            this.snmpTracking_GroupBox = new System.Windows.Forms.GroupBox();
            this.digitalSending_CheckBox = new System.Windows.Forms.CheckBox();
            this.copies_CheckBox = new System.Windows.Forms.CheckBox();
            this.agentGroupBox = new System.Windows.Forms.GroupBox();
            this.agent_CheckBox = new System.Windows.Forms.CheckBox();
            this.sPPEnterpriseGroupBox = new System.Windows.Forms.GroupBox();
            this.purgedJobs_CheckBox = new System.Windows.Forms.CheckBox();
            this.queueNameTabPage = new System.Windows.Forms.TabPage();
            this.deleteRadioButton = new System.Windows.Forms.RadioButton();
            this.addRadioButton = new System.Windows.Forms.RadioButton();
            this.queueName_TextBox = new System.Windows.Forms.TextBox();
            this.queueName_Label = new System.Windows.Forms.Label();
            this.queueTabControl = new System.Windows.Forms.TabControl();
            this.ServerIPAddress_label = new System.Windows.Forms.Label();
            this.IPAddress_textBox = new System.Windows.Forms.TextBox();
            this.Option_TabPage.SuspendLayout();
            this.AddMultipleServer_groupBox.SuspendLayout();
            this.OtherOptions_GroupBox.SuspendLayout();
            this.quota_TabPage.SuspendLayout();
            this.snmpTracking_GroupBox.SuspendLayout();
            this.agentGroupBox.SuspendLayout();
            this.sPPEnterpriseGroupBox.SuspendLayout();
            this.queueNameTabPage.SuspendLayout();
            this.queueTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // Option_TabPage
            // 
            this.Option_TabPage.Controls.Add(this.AddMultipleServer_groupBox);
            this.Option_TabPage.Controls.Add(this.OtherOptions_GroupBox);
            this.Option_TabPage.Controls.Add(this.ProtocolOptions_label);
            this.Option_TabPage.Controls.Add(this.ProtocolOptions_comboBox);
            this.Option_TabPage.Location = new System.Drawing.Point(4, 22);
            this.Option_TabPage.Name = "Option_TabPage";
            this.Option_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.Option_TabPage.Size = new System.Drawing.Size(646, 322);
            this.Option_TabPage.TabIndex = 4;
            this.Option_TabPage.Text = "Options";
            this.Option_TabPage.UseVisualStyleBackColor = true;
            // 
            // AddMultipleServer_groupBox
            // 
            this.AddMultipleServer_groupBox.Controls.Add(this.IPAddress_textBox);
            this.AddMultipleServer_groupBox.Controls.Add(this.ServerIPAddress_label);
            this.AddMultipleServer_groupBox.Controls.Add(this.ServerName_TextBox);
            this.AddMultipleServer_groupBox.Controls.Add(this.ServerURI_label);
            this.AddMultipleServer_groupBox.Location = new System.Drawing.Point(77, 140);
            this.AddMultipleServer_groupBox.Name = "AddMultipleServer_groupBox";
            this.AddMultipleServer_groupBox.Size = new System.Drawing.Size(460, 161);
            this.AddMultipleServer_groupBox.TabIndex = 5;
            this.AddMultipleServer_groupBox.TabStop = false;
            this.AddMultipleServer_groupBox.Text = "AddMultipleServer";
            // 
            // ServerName_TextBox
            // 
            this.ServerName_TextBox.Location = new System.Drawing.Point(116, 39);
            this.ServerName_TextBox.Name = "ServerName_TextBox";
            this.ServerName_TextBox.Size = new System.Drawing.Size(344, 20);
            this.ServerName_TextBox.TabIndex = 1;
            // 
            // ServerURI_label
            // 
            this.ServerURI_label.AutoSize = true;
            this.ServerURI_label.Location = new System.Drawing.Point(6, 42);
            this.ServerURI_label.Name = "ServerURI_label";
            this.ServerURI_label.Size = new System.Drawing.Size(60, 13);
            this.ServerURI_label.TabIndex = 0;
            this.ServerURI_label.Text = "Server URI";
            // 
            // OtherOptions_GroupBox
            // 
            this.OtherOptions_GroupBox.Controls.Add(this.Encryption_CheckBox);
            this.OtherOptions_GroupBox.Location = new System.Drawing.Point(77, 56);
            this.OtherOptions_GroupBox.Name = "OtherOptions_GroupBox";
            this.OtherOptions_GroupBox.Size = new System.Drawing.Size(460, 78);
            this.OtherOptions_GroupBox.TabIndex = 2;
            this.OtherOptions_GroupBox.TabStop = false;
            this.OtherOptions_GroupBox.Text = "Other Options";
            // 
            // Encryption_CheckBox
            // 
            this.Encryption_CheckBox.AutoSize = true;
            this.Encryption_CheckBox.Location = new System.Drawing.Point(17, 34);
            this.Encryption_CheckBox.Name = "Encryption_CheckBox";
            this.Encryption_CheckBox.Size = new System.Drawing.Size(113, 17);
            this.Encryption_CheckBox.TabIndex = 0;
            this.Encryption_CheckBox.Text = "Encryption at Rest";
            this.Encryption_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ProtocolOptions_label
            // 
            this.ProtocolOptions_label.AutoSize = true;
            this.ProtocolOptions_label.Location = new System.Drawing.Point(74, 29);
            this.ProtocolOptions_label.Name = "ProtocolOptions_label";
            this.ProtocolOptions_label.Size = new System.Drawing.Size(85, 13);
            this.ProtocolOptions_label.TabIndex = 1;
            this.ProtocolOptions_label.Text = "Protocol Options";
            // 
            // ProtocolOptions_comboBox
            // 
            this.ProtocolOptions_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProtocolOptions_comboBox.FormattingEnabled = true;
            this.ProtocolOptions_comboBox.Items.AddRange(new object[] {
            "SOCK",
            "PJL",
            "IPPS"});
            this.ProtocolOptions_comboBox.Location = new System.Drawing.Point(222, 29);
            this.ProtocolOptions_comboBox.Name = "ProtocolOptions_comboBox";
            this.ProtocolOptions_comboBox.Size = new System.Drawing.Size(315, 21);
            this.ProtocolOptions_comboBox.TabIndex = 0;
            this.ProtocolOptions_comboBox.SelectedIndexChanged += new System.EventHandler(this.ProtocolOptions_comboBox_SelectedIndexChanged);
            // 
            // quota_TabPage
            // 
            this.quota_TabPage.Controls.Add(this.snmpTracking_GroupBox);
            this.quota_TabPage.Controls.Add(this.agentGroupBox);
            this.quota_TabPage.Controls.Add(this.sPPEnterpriseGroupBox);
            this.quota_TabPage.Location = new System.Drawing.Point(4, 22);
            this.quota_TabPage.Name = "quota_TabPage";
            this.quota_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.quota_TabPage.Size = new System.Drawing.Size(646, 322);
            this.quota_TabPage.TabIndex = 3;
            this.quota_TabPage.Text = " Quota Settings";
            this.quota_TabPage.UseVisualStyleBackColor = true;
            // 
            // snmpTracking_GroupBox
            // 
            this.snmpTracking_GroupBox.Controls.Add(this.digitalSending_CheckBox);
            this.snmpTracking_GroupBox.Controls.Add(this.copies_CheckBox);
            this.snmpTracking_GroupBox.Location = new System.Drawing.Point(3, 215);
            this.snmpTracking_GroupBox.Name = "snmpTracking_GroupBox";
            this.snmpTracking_GroupBox.Size = new System.Drawing.Size(436, 66);
            this.snmpTracking_GroupBox.TabIndex = 8;
            this.snmpTracking_GroupBox.TabStop = false;
            this.snmpTracking_GroupBox.Text = "Snmp Tracking";
            // 
            // digitalSending_CheckBox
            // 
            this.digitalSending_CheckBox.AutoSize = true;
            this.digitalSending_CheckBox.Location = new System.Drawing.Point(111, 31);
            this.digitalSending_CheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.digitalSending_CheckBox.Name = "digitalSending_CheckBox";
            this.digitalSending_CheckBox.Size = new System.Drawing.Size(124, 17);
            this.digitalSending_CheckBox.TabIndex = 1;
            this.digitalSending_CheckBox.Text = "Track digital sending";
            this.digitalSending_CheckBox.UseVisualStyleBackColor = true;
            // 
            // copies_CheckBox
            // 
            this.copies_CheckBox.AutoSize = true;
            this.copies_CheckBox.Location = new System.Drawing.Point(10, 31);
            this.copies_CheckBox.Name = "copies_CheckBox";
            this.copies_CheckBox.Size = new System.Drawing.Size(88, 17);
            this.copies_CheckBox.TabIndex = 0;
            this.copies_CheckBox.Text = "Track copies";
            this.copies_CheckBox.UseVisualStyleBackColor = true;
            // 
            // agentGroupBox
            // 
            this.agentGroupBox.Controls.Add(this.agent_CheckBox);
            this.agentGroupBox.Location = new System.Drawing.Point(3, 120);
            this.agentGroupBox.Name = "agentGroupBox";
            this.agentGroupBox.Size = new System.Drawing.Size(436, 66);
            this.agentGroupBox.TabIndex = 7;
            this.agentGroupBox.TabStop = false;
            this.agentGroupBox.Text = "Agent";
            // 
            // agent_CheckBox
            // 
            this.agent_CheckBox.AutoSize = true;
            this.agent_CheckBox.Location = new System.Drawing.Point(10, 31);
            this.agent_CheckBox.Name = "agent_CheckBox";
            this.agent_CheckBox.Size = new System.Drawing.Size(138, 17);
            this.agent_CheckBox.TabIndex = 0;
            this.agent_CheckBox.Text = "Enable quota for copies";
            this.agent_CheckBox.UseVisualStyleBackColor = true;
            // 
            // sPPEnterpriseGroupBox
            // 
            this.sPPEnterpriseGroupBox.Controls.Add(this.purgedJobs_CheckBox);
            this.sPPEnterpriseGroupBox.Location = new System.Drawing.Point(3, 25);
            this.sPPEnterpriseGroupBox.Name = "sPPEnterpriseGroupBox";
            this.sPPEnterpriseGroupBox.Size = new System.Drawing.Size(436, 66);
            this.sPPEnterpriseGroupBox.TabIndex = 6;
            this.sPPEnterpriseGroupBox.TabStop = false;
            this.sPPEnterpriseGroupBox.Text = "SPP Enterprise";
            // 
            // purgedJobs_CheckBox
            // 
            this.purgedJobs_CheckBox.AutoSize = true;
            this.purgedJobs_CheckBox.Location = new System.Drawing.Point(10, 31);
            this.purgedJobs_CheckBox.Name = "purgedJobs_CheckBox";
            this.purgedJobs_CheckBox.Size = new System.Drawing.Size(174, 17);
            this.purgedJobs_CheckBox.TabIndex = 0;
            this.purgedJobs_CheckBox.Text = "Enable Tracking of purged jobs";
            this.purgedJobs_CheckBox.UseVisualStyleBackColor = true;
            // 
            // queueNameTabPage
            // 
            this.queueNameTabPage.Controls.Add(this.deleteRadioButton);
            this.queueNameTabPage.Controls.Add(this.addRadioButton);
            this.queueNameTabPage.Controls.Add(this.queueName_TextBox);
            this.queueNameTabPage.Controls.Add(this.queueName_Label);
            this.queueNameTabPage.Location = new System.Drawing.Point(4, 22);
            this.queueNameTabPage.Name = "queueNameTabPage";
            this.queueNameTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.queueNameTabPage.Size = new System.Drawing.Size(646, 322);
            this.queueNameTabPage.TabIndex = 0;
            this.queueNameTabPage.Text = "Queue Name";
            this.queueNameTabPage.UseVisualStyleBackColor = true;
            // 
            // deleteRadioButton
            // 
            this.deleteRadioButton.AutoSize = true;
            this.deleteRadioButton.Location = new System.Drawing.Point(303, 66);
            this.deleteRadioButton.Name = "deleteRadioButton";
            this.deleteRadioButton.Size = new System.Drawing.Size(91, 17);
            this.deleteRadioButton.TabIndex = 3;
            this.deleteRadioButton.Text = "Delete Queue";
            this.deleteRadioButton.UseVisualStyleBackColor = true;
            // 
            // addRadioButton
            // 
            this.addRadioButton.AutoSize = true;
            this.addRadioButton.Checked = true;
            this.addRadioButton.Location = new System.Drawing.Point(143, 66);
            this.addRadioButton.Name = "addRadioButton";
            this.addRadioButton.Size = new System.Drawing.Size(79, 17);
            this.addRadioButton.TabIndex = 2;
            this.addRadioButton.TabStop = true;
            this.addRadioButton.Text = "Add Queue";
            this.addRadioButton.UseVisualStyleBackColor = true;
            this.addRadioButton.CheckedChanged += new System.EventHandler(this.addRadioButton_CheckedChanged);
            // 
            // queueName_TextBox
            // 
            this.queueName_TextBox.Location = new System.Drawing.Point(87, 22);
            this.queueName_TextBox.Name = "queueName_TextBox";
            this.queueName_TextBox.Size = new System.Drawing.Size(374, 20);
            this.queueName_TextBox.TabIndex = 1;
            // 
            // queueName_Label
            // 
            this.queueName_Label.AutoSize = true;
            this.queueName_Label.Location = new System.Drawing.Point(3, 25);
            this.queueName_Label.Name = "queueName_Label";
            this.queueName_Label.Size = new System.Drawing.Size(70, 13);
            this.queueName_Label.TabIndex = 0;
            this.queueName_Label.Text = "Queue Name";
            // 
            // queueTabControl
            // 
            this.queueTabControl.Controls.Add(this.queueNameTabPage);
            this.queueTabControl.Controls.Add(this.quota_TabPage);
            this.queueTabControl.Controls.Add(this.Option_TabPage);
            this.queueTabControl.Location = new System.Drawing.Point(3, 3);
            this.queueTabControl.Name = "queueTabControl";
            this.queueTabControl.SelectedIndex = 0;
            this.queueTabControl.Size = new System.Drawing.Size(654, 348);
            this.queueTabControl.TabIndex = 0;
            this.queueTabControl.SelectedIndexChanged += new System.EventHandler(this.queueTabControl_SelectedIndexChanged);
            // 
            // ServerIPAddress_label
            // 
            this.ServerIPAddress_label.AutoSize = true;
            this.ServerIPAddress_label.Location = new System.Drawing.Point(6, 88);
            this.ServerIPAddress_label.Name = "ServerIPAddress_label";
            this.ServerIPAddress_label.Size = new System.Drawing.Size(92, 13);
            this.ServerIPAddress_label.TabIndex = 2;
            this.ServerIPAddress_label.Text = "Server IP Address";
            // 
            // IPAddress_textBox
            // 
            this.IPAddress_textBox.Location = new System.Drawing.Point(116, 81);
            this.IPAddress_textBox.Name = "IPAddress_textBox";
            this.IPAddress_textBox.Size = new System.Drawing.Size(343, 20);
            this.IPAddress_textBox.TabIndex = 3;
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.queueTabControl);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.Load += new System.EventHandler(this.SettingsUserControl_Load);
            this.Option_TabPage.ResumeLayout(false);
            this.Option_TabPage.PerformLayout();
            this.AddMultipleServer_groupBox.ResumeLayout(false);
            this.AddMultipleServer_groupBox.PerformLayout();
            this.OtherOptions_GroupBox.ResumeLayout(false);
            this.OtherOptions_GroupBox.PerformLayout();
            this.quota_TabPage.ResumeLayout(false);
            this.snmpTracking_GroupBox.ResumeLayout(false);
            this.snmpTracking_GroupBox.PerformLayout();
            this.agentGroupBox.ResumeLayout(false);
            this.agentGroupBox.PerformLayout();
            this.sPPEnterpriseGroupBox.ResumeLayout(false);
            this.sPPEnterpriseGroupBox.PerformLayout();
            this.queueNameTabPage.ResumeLayout(false);
            this.queueNameTabPage.PerformLayout();
            this.queueTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabPage Option_TabPage;
        private System.Windows.Forms.GroupBox OtherOptions_GroupBox;
        private System.Windows.Forms.CheckBox Encryption_CheckBox;
        private System.Windows.Forms.Label ProtocolOptions_label;
        private System.Windows.Forms.ComboBox ProtocolOptions_comboBox;
        private System.Windows.Forms.TabPage quota_TabPage;
        private System.Windows.Forms.GroupBox snmpTracking_GroupBox;
        private System.Windows.Forms.CheckBox digitalSending_CheckBox;
        private System.Windows.Forms.CheckBox copies_CheckBox;
        private System.Windows.Forms.GroupBox agentGroupBox;
        private System.Windows.Forms.CheckBox agent_CheckBox;
        private System.Windows.Forms.GroupBox sPPEnterpriseGroupBox;
        private System.Windows.Forms.CheckBox purgedJobs_CheckBox;
        private System.Windows.Forms.TabPage queueNameTabPage;
        private System.Windows.Forms.RadioButton deleteRadioButton;
        private System.Windows.Forms.RadioButton addRadioButton;
        private System.Windows.Forms.TextBox queueName_TextBox;
        private System.Windows.Forms.Label queueName_Label;
        private System.Windows.Forms.TabControl queueTabControl;
        private System.Windows.Forms.GroupBox AddMultipleServer_groupBox;
        private System.Windows.Forms.TextBox ServerName_TextBox;
        private System.Windows.Forms.Label ServerURI_label;
        private System.Windows.Forms.TextBox IPAddress_textBox;
        private System.Windows.Forms.Label ServerIPAddress_label;
    }
}
