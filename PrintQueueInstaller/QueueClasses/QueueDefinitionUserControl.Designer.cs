namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Form to define the queue definitions
    /// </summary>
    partial class QueueDefinitionUserControl
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
            this.queueDefinitions_GroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.adHocQueue_Button = new System.Windows.Forms.Button();
            this.pendingVirtualQueues_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.definePhysicalQueues_Button = new System.Windows.Forms.Button();
            this.queueCount_Label = new System.Windows.Forms.Label();
            this.queueCount_TextBox = new System.Windows.Forms.TextBox();
            this.pendingPhysicalQueues_TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.installTimeout_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.configDefaults_GroupBox = new System.Windows.Forms.GroupBox();
            this.configFileBrowse_Button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dcuSetup_Button = new System.Windows.Forms.Button();
            this.configFileName_TextBox = new System.Windows.Forms.TextBox();
            this.useConfigurationFile_CheckBox = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.additionalDescriptionText_TextBox = new System.Windows.Forms.TextBox();
            this.driver_GroupBox = new System.Windows.Forms.GroupBox();
            this.nameExtension_GroupBox = new System.Windows.Forms.GroupBox();
            this.fullName_CheckBox = new System.Windows.Forms.CheckBox();
            this.queueDefinitions_GroupBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installTimeout_NumericUpDown)).BeginInit();
            this.configDefaults_GroupBox.SuspendLayout();
            this.nameExtension_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // queueDefinitions_GroupBox
            // 
            this.queueDefinitions_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.queueDefinitions_GroupBox.Controls.Add(this.groupBox5);
            this.queueDefinitions_GroupBox.Controls.Add(this.label2);
            this.queueDefinitions_GroupBox.Controls.Add(this.groupBox4);
            this.queueDefinitions_GroupBox.Controls.Add(this.label1);
            this.queueDefinitions_GroupBox.Controls.Add(this.installTimeout_NumericUpDown);
            this.queueDefinitions_GroupBox.Location = new System.Drawing.Point(3, 153);
            this.queueDefinitions_GroupBox.Name = "queueDefinitions_GroupBox";
            this.queueDefinitions_GroupBox.Size = new System.Drawing.Size(711, 127);
            this.queueDefinitions_GroupBox.TabIndex = 55;
            this.queueDefinitions_GroupBox.TabStop = false;
            this.queueDefinitions_GroupBox.Text = "Print Queue Definitions";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.adHocQueue_Button);
            this.groupBox5.Controls.Add(this.pendingVirtualQueues_TextBox);
            this.groupBox5.Location = new System.Drawing.Point(487, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(215, 99);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Queues from Virtual Devices";
            // 
            // adHocQueue_Button
            // 
            this.adHocQueue_Button.Location = new System.Drawing.Point(6, 23);
            this.adHocQueue_Button.Name = "adHocQueue_Button";
            this.adHocQueue_Button.Size = new System.Drawing.Size(150, 23);
            this.adHocQueue_Button.TabIndex = 45;
            this.adHocQueue_Button.Text = "Create Ad-hoc Definitions";
            this.adHocQueue_Button.UseVisualStyleBackColor = true;
            this.adHocQueue_Button.Click += new System.EventHandler(this.adHocQueue_Button_Click);
            // 
            // pendingVirtualQueues_TextBox
            // 
            this.pendingVirtualQueues_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pendingVirtualQueues_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pendingVirtualQueues_TextBox.Location = new System.Drawing.Point(6, 78);
            this.pendingVirtualQueues_TextBox.Name = "pendingVirtualQueues_TextBox";
            this.pendingVirtualQueues_TextBox.Size = new System.Drawing.Size(203, 13);
            this.pendingVirtualQueues_TextBox.TabIndex = 47;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "minutes";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.definePhysicalQueues_Button);
            this.groupBox4.Controls.Add(this.queueCount_Label);
            this.groupBox4.Controls.Add(this.queueCount_TextBox);
            this.groupBox4.Controls.Add(this.pendingPhysicalQueues_TextBox);
            this.groupBox4.Location = new System.Drawing.Point(266, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 99);
            this.groupBox4.TabIndex = 57;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Queues From Physical Devices";
            // 
            // definePhysicalQueues_Button
            // 
            this.definePhysicalQueues_Button.Location = new System.Drawing.Point(6, 23);
            this.definePhysicalQueues_Button.Name = "definePhysicalQueues_Button";
            this.definePhysicalQueues_Button.Size = new System.Drawing.Size(150, 23);
            this.definePhysicalQueues_Button.TabIndex = 1;
            this.definePhysicalQueues_Button.Text = "Create From Inventory";
            this.definePhysicalQueues_Button.UseVisualStyleBackColor = true;
            this.definePhysicalQueues_Button.Click += new System.EventHandler(this.definePhysicalQueues_Button_Click);
            // 
            // queueCount_Label
            // 
            this.queueCount_Label.AutoSize = true;
            this.queueCount_Label.Location = new System.Drawing.Point(6, 55);
            this.queueCount_Label.Name = "queueCount_Label";
            this.queueCount_Label.Size = new System.Drawing.Size(100, 13);
            this.queueCount_Label.TabIndex = 54;
            this.queueCount_Label.Text = "Queues Per Device";
            // 
            // queueCount_TextBox
            // 
            this.queueCount_TextBox.Location = new System.Drawing.Point(112, 52);
            this.queueCount_TextBox.Name = "queueCount_TextBox";
            this.queueCount_TextBox.Size = new System.Drawing.Size(31, 20);
            this.queueCount_TextBox.TabIndex = 53;
            this.queueCount_TextBox.Text = "1";
            this.queueCount_TextBox.TextChanged += new System.EventHandler(this.queueCount_TextBox_TextChanged);
            this.queueCount_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.queueCount_TextBox_KeyDown);
            this.queueCount_TextBox.Leave += new System.EventHandler(this.queueCount_TextBox_Leave);
            this.queueCount_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.queueCount_TextBox_Validating);
            // 
            // pendingPhysicalQueues_TextBox
            // 
            this.pendingPhysicalQueues_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pendingPhysicalQueues_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pendingPhysicalQueues_TextBox.Location = new System.Drawing.Point(9, 78);
            this.pendingPhysicalQueues_TextBox.Name = "pendingPhysicalQueues_TextBox";
            this.pendingPhysicalQueues_TextBox.Size = new System.Drawing.Size(200, 13);
            this.pendingPhysicalQueues_TextBox.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Installation Timeout";
            // 
            // installTimeout_NumericUpDown
            // 
            this.installTimeout_NumericUpDown.Location = new System.Drawing.Point(110, 24);
            this.installTimeout_NumericUpDown.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.installTimeout_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.installTimeout_NumericUpDown.Name = "installTimeout_NumericUpDown";
            this.installTimeout_NumericUpDown.Size = new System.Drawing.Size(41, 20);
            this.installTimeout_NumericUpDown.TabIndex = 55;
            this.installTimeout_NumericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // configDefaults_GroupBox
            // 
            this.configDefaults_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configDefaults_GroupBox.Controls.Add(this.configFileBrowse_Button);
            this.configDefaults_GroupBox.Controls.Add(this.label5);
            this.configDefaults_GroupBox.Controls.Add(this.dcuSetup_Button);
            this.configDefaults_GroupBox.Controls.Add(this.configFileName_TextBox);
            this.configDefaults_GroupBox.Controls.Add(this.useConfigurationFile_CheckBox);
            this.configDefaults_GroupBox.Controls.Add(this.label9);
            this.configDefaults_GroupBox.Location = new System.Drawing.Point(3, 286);
            this.configDefaults_GroupBox.Name = "configDefaults_GroupBox";
            this.configDefaults_GroupBox.Size = new System.Drawing.Size(711, 116);
            this.configDefaults_GroupBox.TabIndex = 54;
            this.configDefaults_GroupBox.TabStop = false;
            this.configDefaults_GroupBox.Text = "Queue Configuration Defaults (optional)";
            // 
            // configFileBrowse_Button
            // 
            this.configFileBrowse_Button.Location = new System.Drawing.Point(627, 44);
            this.configFileBrowse_Button.Name = "configFileBrowse_Button";
            this.configFileBrowse_Button.Size = new System.Drawing.Size(75, 23);
            this.configFileBrowse_Button.TabIndex = 32;
            this.configFileBrowse_Button.Text = "Browse...";
            this.configFileBrowse_Button.UseVisualStyleBackColor = true;
            this.configFileBrowse_Button.Click += new System.EventHandler(this.configFileBrowse_Button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "CFM File Name";
            // 
            // dcuSetup_Button
            // 
            this.dcuSetup_Button.Location = new System.Drawing.Point(591, 85);
            this.dcuSetup_Button.Name = "dcuSetup_Button";
            this.dcuSetup_Button.Size = new System.Drawing.Size(111, 23);
            this.dcuSetup_Button.TabIndex = 3;
            this.dcuSetup_Button.Text = "Launch DCU...";
            this.dcuSetup_Button.UseVisualStyleBackColor = true;
            this.dcuSetup_Button.Click += new System.EventHandler(this.dcuSetup_Button_Click);
            // 
            // configFileName_TextBox
            // 
            this.configFileName_TextBox.Location = new System.Drawing.Point(156, 47);
            this.configFileName_TextBox.Name = "configFileName_TextBox";
            this.configFileName_TextBox.Size = new System.Drawing.Size(465, 20);
            this.configFileName_TextBox.TabIndex = 52;
            this.configFileName_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.configFileName_TextBox_Validating);
            // 
            // useConfigurationFile_CheckBox
            // 
            this.useConfigurationFile_CheckBox.AutoSize = true;
            this.useConfigurationFile_CheckBox.Location = new System.Drawing.Point(156, 27);
            this.useConfigurationFile_CheckBox.Name = "useConfigurationFile_CheckBox";
            this.useConfigurationFile_CheckBox.Size = new System.Drawing.Size(15, 14);
            this.useConfigurationFile_CheckBox.TabIndex = 2;
            this.useConfigurationFile_CheckBox.UseVisualStyleBackColor = true;
            this.useConfigurationFile_CheckBox.CheckedChanged += new System.EventHandler(this.useConfigurationFile_CheckBox_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(66, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Enable CFM File";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Additional Queue Description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(473, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "(will be added to the queue name)";
            // 
            // additionalDescriptionText_TextBox
            // 
            this.additionalDescriptionText_TextBox.Location = new System.Drawing.Point(175, 25);
            this.additionalDescriptionText_TextBox.Name = "additionalDescriptionText_TextBox";
            this.additionalDescriptionText_TextBox.Size = new System.Drawing.Size(292, 20);
            this.additionalDescriptionText_TextBox.TabIndex = 4;
            this.additionalDescriptionText_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.additionalDescriptionText_TextBox_Validating);
            // 
            // driver_GroupBox
            // 
            this.driver_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driver_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.driver_GroupBox.Name = "driver_GroupBox";
            this.driver_GroupBox.Size = new System.Drawing.Size(711, 144);
            this.driver_GroupBox.TabIndex = 53;
            this.driver_GroupBox.TabStop = false;
            this.driver_GroupBox.Text = "Print Driver Package";
            // 
            // nameExtension_GroupBox
            // 
            this.nameExtension_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameExtension_GroupBox.Controls.Add(this.label6);
            this.nameExtension_GroupBox.Controls.Add(this.additionalDescriptionText_TextBox);
            this.nameExtension_GroupBox.Controls.Add(this.label8);
            this.nameExtension_GroupBox.Location = new System.Drawing.Point(3, 408);
            this.nameExtension_GroupBox.Name = "nameExtension_GroupBox";
            this.nameExtension_GroupBox.Size = new System.Drawing.Size(711, 60);
            this.nameExtension_GroupBox.TabIndex = 56;
            this.nameExtension_GroupBox.TabStop = false;
            this.nameExtension_GroupBox.Text = "Queue Name Extension (optional)";
            // 
            // fullName_CheckBox
            // 
            this.fullName_CheckBox.AutoSize = true;
            this.fullName_CheckBox.Checked = true;
            this.fullName_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fullName_CheckBox.Location = new System.Drawing.Point(3, 474);
            this.fullName_CheckBox.Name = "fullName_CheckBox";
            this.fullName_CheckBox.Size = new System.Drawing.Size(270, 17);
            this.fullName_CheckBox.TabIndex = 53;
            this.fullName_CheckBox.Text = "Option Appends Driver Information To Queue Name";
            this.fullName_CheckBox.UseVisualStyleBackColor = true;
            // 
            // QueueDefinitionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fullName_CheckBox);
            this.Controls.Add(this.nameExtension_GroupBox);
            this.Controls.Add(this.queueDefinitions_GroupBox);
            this.Controls.Add(this.configDefaults_GroupBox);
            this.Controls.Add(this.driver_GroupBox);
            this.Name = "QueueDefinitionUserControl";
            this.Size = new System.Drawing.Size(717, 502);
            this.Load += new System.EventHandler(this.QueueDefinitionControl_Load);
            this.queueDefinitions_GroupBox.ResumeLayout(false);
            this.queueDefinitions_GroupBox.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installTimeout_NumericUpDown)).EndInit();
            this.configDefaults_GroupBox.ResumeLayout(false);
            this.configDefaults_GroupBox.PerformLayout();
            this.nameExtension_GroupBox.ResumeLayout(false);
            this.nameExtension_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox queueDefinitions_GroupBox;
        private System.Windows.Forms.Button adHocQueue_Button;
        private System.Windows.Forms.Button definePhysicalQueues_Button;
        private System.Windows.Forms.GroupBox configDefaults_GroupBox;
        private System.Windows.Forms.Button configFileBrowse_Button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button dcuSetup_Button;
        private System.Windows.Forms.TextBox configFileName_TextBox;
        private System.Windows.Forms.TextBox additionalDescriptionText_TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox useConfigurationFile_CheckBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox driver_GroupBox;
        private System.Windows.Forms.TextBox pendingPhysicalQueues_TextBox;
        private System.Windows.Forms.TextBox pendingVirtualQueues_TextBox;
        private System.Windows.Forms.Label queueCount_Label;
        private System.Windows.Forms.TextBox queueCount_TextBox;
        private System.Windows.Forms.GroupBox nameExtension_GroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown installTimeout_NumericUpDown;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox fullName_CheckBox;
    }
}
