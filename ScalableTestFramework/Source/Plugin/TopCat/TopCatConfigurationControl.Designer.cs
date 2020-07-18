namespace HP.ScalableTest.Plugin.TopCat
{
    partial class TopCatConfigurationControl
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
            this.scriptpath_textBox = new System.Windows.Forms.TextBox();
            this.browse_button = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.labelScriptFiles = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.script_groupBox = new System.Windows.Forms.GroupBox();
            this.update_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.properties_comboBox = new System.Windows.Forms.ComboBox();
            this.properties_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.scripts_listBox = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.copydir_checkBox = new System.Windows.Forms.CheckBox();
            this.browseInstaller_button = new System.Windows.Forms.Button();
            this.setupPath_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.runonce_checkBox = new System.Windows.Forms.CheckBox();
            this.test_groupBox = new System.Windows.Forms.GroupBox();
            this.alltests_checkBox = new System.Windows.Forms.CheckBox();
            this.topcattests_listBox = new System.Windows.Forms.ListBox();
            this.topcat_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBox1.SuspendLayout();
            this.script_groupBox.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.test_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // scriptpath_textBox
            // 
            this.scriptpath_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptpath_textBox.BackColor = System.Drawing.Color.White;
            this.scriptpath_textBox.Location = new System.Drawing.Point(12, 32);
            this.scriptpath_textBox.Name = "scriptpath_textBox";
            this.scriptpath_textBox.ReadOnly = true;
            this.scriptpath_textBox.Size = new System.Drawing.Size(429, 20);
            this.scriptpath_textBox.TabIndex = 0;
            // 
            // browse_button
            // 
            this.browse_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browse_button.Location = new System.Drawing.Point(447, 29);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(40, 23);
            this.browse_button.TabIndex = 1;
            this.browse_button.Text = "...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Enabled = false;
            this.buttonUpload.Location = new System.Drawing.Point(366, 58);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(75, 23);
            this.buttonUpload.TabIndex = 2;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // labelScriptFiles
            // 
            this.labelScriptFiles.AutoSize = true;
            this.labelScriptFiles.Location = new System.Drawing.Point(7, 10);
            this.labelScriptFiles.Name = "labelScriptFiles";
            this.labelScriptFiles.Size = new System.Drawing.Size(96, 13);
            this.labelScriptFiles.TabIndex = 3;
            this.labelScriptFiles.Text = "TopCat Script Files";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "New Script";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonUpload);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.scriptpath_textBox);
            this.groupBox1.Controls.Add(this.browse_button);
            this.groupBox1.Location = new System.Drawing.Point(10, 416);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 87);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Upload New Script";
            // 
            // script_groupBox
            // 
            this.script_groupBox.Controls.Add(this.update_button);
            this.script_groupBox.Controls.Add(this.label3);
            this.script_groupBox.Controls.Add(this.properties_comboBox);
            this.script_groupBox.Controls.Add(this.properties_textBox);
            this.script_groupBox.Controls.Add(this.label2);
            this.script_groupBox.Location = new System.Drawing.Point(344, 173);
            this.script_groupBox.Name = "script_groupBox";
            this.script_groupBox.Size = new System.Drawing.Size(285, 142);
            this.script_groupBox.TabIndex = 8;
            this.script_groupBox.TabStop = false;
            this.script_groupBox.Text = "Script Properties";
            // 
            // update_button
            // 
            this.update_button.Location = new System.Drawing.Point(204, 100);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(75, 23);
            this.update_button.TabIndex = 5;
            this.update_button.Text = "Update";
            this.update_button.UseVisualStyleBackColor = true;
            this.update_button.Click += new System.EventHandler(this.update_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value";
            // 
            // properties_comboBox
            // 
            this.properties_comboBox.FormattingEnabled = true;
            this.properties_comboBox.Location = new System.Drawing.Point(9, 34);
            this.properties_comboBox.Name = "properties_comboBox";
            this.properties_comboBox.Size = new System.Drawing.Size(267, 21);
            this.properties_comboBox.TabIndex = 3;
            this.properties_comboBox.SelectedIndexChanged += new System.EventHandler(this.properties_comboBox_SelectedIndexChanged);
            // 
            // properties_textBox
            // 
            this.properties_textBox.Location = new System.Drawing.Point(6, 74);
            this.properties_textBox.Name = "properties_textBox";
            this.properties_textBox.Size = new System.Drawing.Size(270, 20);
            this.properties_textBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Property";
            // 
            // scripts_listBox
            // 
            this.scripts_listBox.FormattingEnabled = true;
            this.scripts_listBox.Location = new System.Drawing.Point(10, 27);
            this.scripts_listBox.Name = "scripts_listBox";
            this.scripts_listBox.Size = new System.Drawing.Size(328, 212);
            this.scripts_listBox.TabIndex = 9;
            this.scripts_listBox.SelectedIndexChanged += new System.EventHandler(this.scripts_listBox_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.copydir_checkBox);
            this.groupBox4.Controls.Add(this.browseInstaller_button);
            this.groupBox4.Controls.Add(this.setupPath_textBox);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(10, 316);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(619, 94);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Dependent Software";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "NOTE:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(512, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Ensure that the installer is a batch file, which installs the required AUT to the" +
    " specified directory unattended.";
            // 
            // copydir_checkBox
            // 
            this.copydir_checkBox.AutoSize = true;
            this.copydir_checkBox.Location = new System.Drawing.Point(74, 46);
            this.copydir_checkBox.Name = "copydir_checkBox";
            this.copydir_checkBox.Size = new System.Drawing.Size(101, 17);
            this.copydir_checkBox.TabIndex = 3;
            this.copydir_checkBox.Text = "Copy Directory?";
            this.copydir_checkBox.UseVisualStyleBackColor = true;
            // 
            // browseInstaller_button
            // 
            this.browseInstaller_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseInstaller_button.Location = new System.Drawing.Point(438, 19);
            this.browseInstaller_button.Name = "browseInstaller_button";
            this.browseInstaller_button.Size = new System.Drawing.Size(40, 23);
            this.browseInstaller_button.TabIndex = 2;
            this.browseInstaller_button.Text = "...";
            this.browseInstaller_button.UseVisualStyleBackColor = true;
            this.browseInstaller_button.Click += new System.EventHandler(this.browseInstaller_button_Click);
            // 
            // setupPath_textBox
            // 
            this.setupPath_textBox.BackColor = System.Drawing.Color.White;
            this.setupPath_textBox.Location = new System.Drawing.Point(74, 20);
            this.setupPath_textBox.Name = "setupPath_textBox";
            this.setupPath_textBox.Size = new System.Drawing.Size(358, 20);
            this.setupPath_textBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Installer File";
            // 
            // runonce_checkBox
            // 
            this.runonce_checkBox.AutoSize = true;
            this.runonce_checkBox.Checked = true;
            this.runonce_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runonce_checkBox.Location = new System.Drawing.Point(10, 250);
            this.runonce_checkBox.Name = "runonce_checkBox";
            this.runonce_checkBox.Size = new System.Drawing.Size(95, 17);
            this.runonce_checkBox.TabIndex = 11;
            this.runonce_checkBox.Text = "Run at Startup";
            this.runonce_checkBox.UseVisualStyleBackColor = true;
            // 
            // test_groupBox
            // 
            this.test_groupBox.Controls.Add(this.alltests_checkBox);
            this.test_groupBox.Controls.Add(this.topcattests_listBox);
            this.test_groupBox.Location = new System.Drawing.Point(344, 27);
            this.test_groupBox.Name = "test_groupBox";
            this.test_groupBox.Size = new System.Drawing.Size(285, 140);
            this.test_groupBox.TabIndex = 12;
            this.test_groupBox.TabStop = false;
            this.test_groupBox.Text = "Select Tests";
            // 
            // alltests_checkBox
            // 
            this.alltests_checkBox.AutoSize = true;
            this.alltests_checkBox.Checked = true;
            this.alltests_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alltests_checkBox.Location = new System.Drawing.Point(9, 117);
            this.alltests_checkBox.Name = "alltests_checkBox";
            this.alltests_checkBox.Size = new System.Drawing.Size(79, 17);
            this.alltests_checkBox.TabIndex = 1;
            this.alltests_checkBox.Text = "Execute All";
            this.alltests_checkBox.UseVisualStyleBackColor = true;
            this.alltests_checkBox.CheckedChanged += new System.EventHandler(this.alltests_checkBox_CheckedChanged);
            // 
            // topcattests_listBox
            // 
            this.topcattests_listBox.Enabled = false;
            this.topcattests_listBox.FormattingEnabled = true;
            this.topcattests_listBox.Location = new System.Drawing.Point(9, 16);
            this.topcattests_listBox.Name = "topcattests_listBox";
            this.topcattests_listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.topcattests_listBox.Size = new System.Drawing.Size(193, 95);
            this.topcattests_listBox.TabIndex = 0;
            // 
            // TopCatConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.test_groupBox);
            this.Controls.Add(this.runonce_checkBox);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.scripts_listBox);
            this.Controls.Add(this.script_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelScriptFiles);
            this.Name = "TopCatConfigurationControl";
            this.Size = new System.Drawing.Size(651, 506);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.script_groupBox.ResumeLayout(false);
            this.script_groupBox.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.test_groupBox.ResumeLayout(false);
            this.test_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox scriptpath_textBox;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Label labelScriptFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox script_groupBox;
        private System.Windows.Forms.Button update_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox properties_comboBox;
        private System.Windows.Forms.TextBox properties_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox scripts_listBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button browseInstaller_button;
        private System.Windows.Forms.TextBox setupPath_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox copydir_checkBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox runonce_checkBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox test_groupBox;
        private System.Windows.Forms.ListBox topcattests_listBox;
        private System.Windows.Forms.CheckBox alltests_checkBox;
        private Framework.UI.FieldValidator topcat_fieldValidator;
    }
}
