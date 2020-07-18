namespace HP.ScalableTest.Plugin.Hpec
{
    partial class HpecConfigControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.deviceSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.skipTimeout_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.label6 = new System.Windows.Forms.Label();
            this.workflowConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDeviceSignIn = new System.Windows.Forms.RadioButton();
            this.radioButtonHpcr = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.workflowName_comboBox = new System.Windows.Forms.ComboBox();
            this.workflowName_label = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.workflowConfiguration_TabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.workflowConfiguration_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 500);
            this.tabControl.TabIndex = 6;
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.Controls.Add(this.deviceSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel2);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(667, 472);
            this.scanConfiguration_TabPage.TabIndex = 2;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // deviceSelectionControl
            // 
            this.deviceSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceSelectionControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.deviceSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deviceSelectionControl.Location = new System.Drawing.Point(3, 128);
            this.deviceSelectionControl.Name = "deviceSelectionControl";
            this.deviceSelectionControl.Size = new System.Drawing.Size(647, 335);
            this.deviceSelectionControl.TabIndex = 48;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel2.Size = new System.Drawing.Size(661, 125);
            this.panel2.TabIndex = 50;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(0, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 111);
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
    "same for all devices.";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.skipTimeout_TimeSpanControl);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(302, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(345, 111);
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
            this.label2.Size = new System.Drawing.Size(331, 57);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // skipTimeout_TimeSpanControl
            // 
            this.skipTimeout_TimeSpanControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipTimeout_TimeSpanControl.AutoSize = true;
            this.skipTimeout_TimeSpanControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.skipTimeout_TimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipTimeout_TimeSpanControl.Location = new System.Drawing.Point(135, 78);
            this.skipTimeout_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.skipTimeout_TimeSpanControl.Name = "skipTimeout_TimeSpanControl";
            this.skipTimeout_TimeSpanControl.Size = new System.Drawing.Size(100, 26);
            this.skipTimeout_TimeSpanControl.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Delay before skip";
            // 
            // workflowConfiguration_TabPage
            // 
            this.workflowConfiguration_TabPage.Controls.Add(this.groupBox1);
            this.workflowConfiguration_TabPage.Controls.Add(this.groupBox2);
            this.workflowConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.workflowConfiguration_TabPage.Name = "workflowConfiguration_TabPage";
            this.workflowConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.workflowConfiguration_TabPage.Size = new System.Drawing.Size(667, 472);
            this.workflowConfiguration_TabPage.TabIndex = 3;
            this.workflowConfiguration_TabPage.Text = "Workflow Configuration";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDeviceSignIn);
            this.groupBox1.Controls.Add(this.radioButtonHpcr);
            this.groupBox1.Location = new System.Drawing.Point(420, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 77);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication";
            // 
            // radioButtonDeviceSignIn
            // 
            this.radioButtonDeviceSignIn.AutoSize = true;
            this.radioButtonDeviceSignIn.Checked = true;
            this.radioButtonDeviceSignIn.Location = new System.Drawing.Point(22, 18);
            this.radioButtonDeviceSignIn.Name = "radioButtonDeviceSignIn";
            this.radioButtonDeviceSignIn.Size = new System.Drawing.Size(61, 19);
            this.radioButtonDeviceSignIn.TabIndex = 1;
            this.radioButtonDeviceSignIn.TabStop = true;
            this.radioButtonDeviceSignIn.Text = "Sign In";
            this.radioButtonDeviceSignIn.UseVisualStyleBackColor = true;
            // 
            // radioButtonHpcr
            // 
            this.radioButtonHpcr.AutoSize = true;
            this.radioButtonHpcr.Location = new System.Drawing.Point(22, 44);
            this.radioButtonHpcr.Name = "radioButtonHpcr";
            this.radioButtonHpcr.Size = new System.Drawing.Size(112, 19);
            this.radioButtonHpcr.TabIndex = 0;
            this.radioButtonHpcr.Text = "HP EC Workflow";
            this.radioButtonHpcr.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.workflowName_comboBox);
            this.groupBox2.Controls.Add(this.workflowName_label);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 120);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Workflow Selection";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(21, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(354, 35);
            this.label5.TabIndex = 12;
            this.label5.Text = "Note: This plugin is only applicable to Jedi physical devices currently.";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(21, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 19);
            this.label1.TabIndex = 12;
            this.label1.Text = "Select the HPEC workflow that will be invoked.";
            // 
            // workflowName_comboBox
            // 
            this.workflowName_comboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workflowName_comboBox.FormattingEnabled = true;
            this.workflowName_comboBox.Items.AddRange(new object[] {
            "Scan to Email",
            "Send to Network Folder"});
            this.workflowName_comboBox.Location = new System.Drawing.Point(120, 46);
            this.workflowName_comboBox.Name = "workflowName_comboBox";
            this.workflowName_comboBox.Size = new System.Drawing.Size(255, 23);
            this.workflowName_comboBox.TabIndex = 2;
            // 
            // workflowName_label
            // 
            this.workflowName_label.AutoSize = true;
            this.workflowName_label.Location = new System.Drawing.Point(24, 49);
            this.workflowName_label.Name = "workflowName_label";
            this.workflowName_label.Size = new System.Drawing.Size(93, 15);
            this.workflowName_label.TabIndex = 0;
            this.workflowName_label.Text = "Workflow Name";
            // 
            // HpecConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpecConfigControl";
            this.Size = new System.Drawing.Size(675, 500);
            this.tabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.workflowConfiguration_TabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage workflowConfiguration_TabPage;
        private System.Windows.Forms.Label workflowName_label;
        private HP.ScalableTest.Framework.UI.AssetSelectionControl deviceSelectionControl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private Framework.UI.TimeSpanControl skipTimeout_TimeSpanControl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox workflowName_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDeviceSignIn;
        private System.Windows.Forms.RadioButton radioButtonHpcr;
    }
}
