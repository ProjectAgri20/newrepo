namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    partial class ScanToWorkflowConfigControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanToWorkflowConfigControl));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ScanToWorkflow = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.workflowConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.excludeFileNamePrompt_checkBox = new System.Windows.Forms.CheckBox();
            this.workflowPrompts_DataGridView = new System.Windows.Forms.DataGridView();
            this.prompt_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value_DataGridViewColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.workflowName_Label = new System.Windows.Forms.Label();
            this.workflowName_TextBox = new System.Windows.Forms.TextBox();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.logOcr_CheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.folderDestination_RadioButton = new System.Windows.Forms.RadioButton();
            this.sharepointDestination_RadioButton = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.tabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.workflowConfiguration_TabPage.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workflowPrompts_DataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.workflowConfiguration_TabPage);
            this.tabControl.Controls.Add(this.loggingOptions_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 500);
            this.tabControl.TabIndex = 6;
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel2);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(667, 472);
            this.scanConfiguration_TabPage.TabIndex = 2;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 209);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(661, 260);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox_Authentication);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel2.Size = new System.Drawing.Size(661, 206);
            this.panel2.TabIndex = 50;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ScanToWorkflow);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(651, 49);
            this.groupBox_Authentication.TabIndex = 95;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_ScanToWorkflow
            // 
            this.radioButton_ScanToWorkflow.AutoSize = true;
            this.radioButton_ScanToWorkflow.Location = new System.Drawing.Point(116, 20);
            this.radioButton_ScanToWorkflow.Name = "radioButton_ScanToWorkflow";
            this.radioButton_ScanToWorkflow.Size = new System.Drawing.Size(114, 19);
            this.radioButton_ScanToWorkflow.TabIndex = 93;
            this.radioButton_ScanToWorkflow.Text = "ScanToWorkflow";
            this.radioButton_ScanToWorkflow.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(61, 19);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(439, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(299, 22);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(3, 58);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 142);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Page Count";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 80);
            this.pageCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pageCount_NumericUpDown.Name = "pageCount_NumericUpDown";
            this.pageCount_NumericUpDown.Size = new System.Drawing.Size(107, 23);
            this.pageCount_NumericUpDown.TabIndex = 12;
            this.pageCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 57);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.lockTimeoutControl);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(302, 67);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(352, 133);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Skip Delay";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 57);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // workflowConfiguration_TabPage
            // 
            this.workflowConfiguration_TabPage.Controls.Add(this.groupBox6);
            this.workflowConfiguration_TabPage.Controls.Add(this.groupBox2);
            this.workflowConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.workflowConfiguration_TabPage.Name = "workflowConfiguration_TabPage";
            this.workflowConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.workflowConfiguration_TabPage.Size = new System.Drawing.Size(667, 472);
            this.workflowConfiguration_TabPage.TabIndex = 3;
            this.workflowConfiguration_TabPage.Text = "Workflow Configuration";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.excludeFileNamePrompt_checkBox);
            this.groupBox6.Controls.Add(this.workflowPrompts_DataGridView);
            this.groupBox6.Controls.Add(this.textBox5);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 69);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(661, 400);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Workflow Prompts";
            // 
            // excludeFileNamePrompt_checkBox
            // 
            this.excludeFileNamePrompt_checkBox.AutoSize = true;
            this.excludeFileNamePrompt_checkBox.Location = new System.Drawing.Point(24, 59);
            this.excludeFileNamePrompt_checkBox.Name = "excludeFileNamePrompt_checkBox";
            this.excludeFileNamePrompt_checkBox.Size = new System.Drawing.Size(324, 19);
            this.excludeFileNamePrompt_checkBox.TabIndex = 95;
            this.excludeFileNamePrompt_checkBox.Text = "Exclude the file name prompt (will disable file validation)";
            this.excludeFileNamePrompt_checkBox.UseVisualStyleBackColor = true;
            // 
            // workflowPrompts_DataGridView
            // 
            this.workflowPrompts_DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workflowPrompts_DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.prompt_DataGridViewColumn,
            this.value_DataGridViewColumn});
            this.workflowPrompts_DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workflowPrompts_DataGridView.Location = new System.Drawing.Point(3, 95);
            this.workflowPrompts_DataGridView.Name = "workflowPrompts_DataGridView";
            this.workflowPrompts_DataGridView.Size = new System.Drawing.Size(655, 302);
            this.workflowPrompts_DataGridView.TabIndex = 2;
            // 
            // prompt_DataGridViewColumn
            // 
            this.prompt_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.prompt_DataGridViewColumn.HeaderText = "Prompt Text";
            this.prompt_DataGridViewColumn.Name = "prompt_DataGridViewColumn";
            // 
            // value_DataGridViewColumn
            // 
            this.value_DataGridViewColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.value_DataGridViewColumn.HeaderText = "Prompt Value";
            this.value_DataGridViewColumn.Name = "value_DataGridViewColumn";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Control;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox5.Location = new System.Drawing.Point(3, 19);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(655, 76);
            this.textBox5.TabIndex = 48;
            this.textBox5.Text = resources.GetString("textBox5.Text");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.workflowName_Label);
            this.groupBox2.Controls.Add(this.workflowName_TextBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(661, 66);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Workflow Selection";
            // 
            // workflowName_Label
            // 
            this.workflowName_Label.AutoSize = true;
            this.workflowName_Label.Location = new System.Drawing.Point(21, 29);
            this.workflowName_Label.Name = "workflowName_Label";
            this.workflowName_Label.Size = new System.Drawing.Size(93, 15);
            this.workflowName_Label.TabIndex = 0;
            this.workflowName_Label.Text = "Workflow Name";
            // 
            // workflowName_TextBox
            // 
            this.workflowName_TextBox.Location = new System.Drawing.Point(125, 25);
            this.workflowName_TextBox.Name = "workflowName_TextBox";
            this.workflowName_TextBox.Size = new System.Drawing.Size(308, 23);
            this.workflowName_TextBox.TabIndex = 1;
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.groupBox7);
            this.loggingOptions_TabPage.Controls.Add(this.groupBox4);
            this.loggingOptions_TabPage.Controls.Add(this.groupBox1);
            this.loggingOptions_TabPage.Controls.Add(this.label7);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 24);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(667, 472);
            this.loggingOptions_TabPage.TabIndex = 4;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.digitalSendServer_TextBox);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Location = new System.Drawing.Point(9, 155);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(317, 104);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Digital Send Service";
            // 
            // digitalSendServer_TextBox
            // 
            this.digitalSendServer_TextBox.Location = new System.Drawing.Point(98, 25);
            this.digitalSendServer_TextBox.Name = "digitalSendServer_TextBox";
            this.digitalSendServer_TextBox.Size = new System.Drawing.Size(202, 23);
            this.digitalSendServer_TextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Server Name";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.logOcr_CheckBox);
            this.groupBox4.Location = new System.Drawing.Point(332, 43);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(317, 106);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Document Options";
            // 
            // logOcr_CheckBox
            // 
            this.logOcr_CheckBox.AutoSize = true;
            this.logOcr_CheckBox.Location = new System.Drawing.Point(17, 28);
            this.logOcr_CheckBox.Name = "logOcr_CheckBox";
            this.logOcr_CheckBox.Size = new System.Drawing.Size(146, 19);
            this.logOcr_CheckBox.TabIndex = 7;
            this.logOcr_CheckBox.Text = "Workflow will use OCR";
            this.logOcr_CheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.folderDestination_RadioButton);
            this.groupBox1.Controls.Add(this.sharepointDestination_RadioButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 106);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination Type";
            // 
            // folderDestination_RadioButton
            // 
            this.folderDestination_RadioButton.AutoSize = true;
            this.folderDestination_RadioButton.Checked = true;
            this.folderDestination_RadioButton.Location = new System.Drawing.Point(21, 27);
            this.folderDestination_RadioButton.Name = "folderDestination_RadioButton";
            this.folderDestination_RadioButton.Size = new System.Drawing.Size(112, 19);
            this.folderDestination_RadioButton.TabIndex = 6;
            this.folderDestination_RadioButton.TabStop = true;
            this.folderDestination_RadioButton.Text = "Workflow Folder";
            this.folderDestination_RadioButton.UseVisualStyleBackColor = true;
            // 
            // sharepointDestination_RadioButton
            // 
            this.sharepointDestination_RadioButton.AutoSize = true;
            this.sharepointDestination_RadioButton.Location = new System.Drawing.Point(21, 64);
            this.sharepointDestination_RadioButton.Name = "sharepointDestination_RadioButton";
            this.sharepointDestination_RadioButton.Size = new System.Drawing.Size(180, 19);
            this.sharepointDestination_RadioButton.TabIndex = 5;
            this.sharepointDestination_RadioButton.Text = "SharePoint Document Library";
            this.sharepointDestination_RadioButton.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(649, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(10, 71);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // ScanToWorkflowConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToWorkflowConfigControl";
            this.Size = new System.Drawing.Size(675, 500);
            this.tabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.workflowConfiguration_TabPage.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workflowPrompts_DataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage workflowConfiguration_TabPage;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.DataGridView workflowPrompts_DataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn prompt_DataGridViewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn value_DataGridViewColumn;
        private System.Windows.Forms.TextBox workflowName_TextBox;
        private System.Windows.Forms.Label workflowName_Label;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox logOcr_CheckBox;
        private System.Windows.Forms.RadioButton sharepointDestination_RadioButton;
        private System.Windows.Forms.RadioButton folderDestination_RadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label label5;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ScanToWorkflow;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.CheckBox excludeFileNamePrompt_checkBox;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}