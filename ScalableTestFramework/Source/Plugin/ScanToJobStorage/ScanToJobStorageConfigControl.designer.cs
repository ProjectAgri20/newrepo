namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    partial class ScanToJobStorageConfigControl
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
            this.scanToJobStorage_GroupBox = new System.Windows.Forms.GroupBox();
            this.scanoptions_groupBox = new System.Windows.Forms.GroupBox();
            this.ScanOptions_Button = new System.Windows.Forms.Button();
            this.pin_Label = new System.Windows.Forms.Label();
            this.pinDescription_label = new System.Windows.Forms.Label();
            this.pinRequired_CheckBox = new System.Windows.Forms.CheckBox();
            this.pin_TextBox = new System.Windows.Forms.TextBox();
            this.scanDetails_Panel = new System.Windows.Forms.Panel();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ScanToJobStorage = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.skipDelay_GroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pageCount_GroupBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.scanToJobStorage_GroupBox.SuspendLayout();
            this.scanoptions_groupBox.SuspendLayout();
            this.scanDetails_Panel.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.skipDelay_GroupBox.SuspendLayout();
            this.pageCount_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // scanToJobStorage_GroupBox
            // 
            this.scanToJobStorage_GroupBox.Controls.Add(this.scanoptions_groupBox);
            this.scanToJobStorage_GroupBox.Controls.Add(this.pin_Label);
            this.scanToJobStorage_GroupBox.Controls.Add(this.pinDescription_label);
            this.scanToJobStorage_GroupBox.Controls.Add(this.pinRequired_CheckBox);
            this.scanToJobStorage_GroupBox.Controls.Add(this.pin_TextBox);
            this.scanToJobStorage_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.scanToJobStorage_GroupBox.Name = "scanToJobStorage_GroupBox";
            this.scanToJobStorage_GroupBox.Size = new System.Drawing.Size(666, 119);
            this.scanToJobStorage_GroupBox.TabIndex = 54;
            this.scanToJobStorage_GroupBox.TabStop = false;
            this.scanToJobStorage_GroupBox.Text = "Scan To JobStorage";
            // 
            // scanoptions_groupBox
            // 
            this.scanoptions_groupBox.Controls.Add(this.ScanOptions_Button);
            this.scanoptions_groupBox.Location = new System.Drawing.Point(315, 12);
            this.scanoptions_groupBox.Name = "scanoptions_groupBox";
            this.scanoptions_groupBox.Size = new System.Drawing.Size(227, 62);
            this.scanoptions_groupBox.TabIndex = 59;
            this.scanoptions_groupBox.TabStop = false;
            this.scanoptions_groupBox.Text = "Scan Options";
            // 
            // ScanOptions_Button
            // 
            this.ScanOptions_Button.Location = new System.Drawing.Point(63, 22);
            this.ScanOptions_Button.Name = "ScanOptions_Button";
            this.ScanOptions_Button.Size = new System.Drawing.Size(114, 34);
            this.ScanOptions_Button.TabIndex = 0;
            this.ScanOptions_Button.Text = "Scan Options";
            this.ScanOptions_Button.UseVisualStyleBackColor = true;
            this.ScanOptions_Button.Click += new System.EventHandler(this.ScanOptions_Button_Click);
            // 
            // pin_Label
            // 
            this.pin_Label.AutoSize = true;
            this.pin_Label.Location = new System.Drawing.Point(6, 54);
            this.pin_Label.Name = "pin_Label";
            this.pin_Label.Size = new System.Drawing.Size(26, 15);
            this.pin_Label.TabIndex = 58;
            this.pin_Label.Text = "PIN";
            // 
            // pinDescription_label
            // 
            this.pinDescription_label.AutoSize = true;
            this.pinDescription_label.Location = new System.Drawing.Point(35, 77);
            this.pinDescription_label.Name = "pinDescription_label";
            this.pinDescription_label.Size = new System.Drawing.Size(147, 15);
            this.pinDescription_label.TabIndex = 57;
            this.pinDescription_label.Text = "numeric(0-9), 4 chars Max.";
            // 
            // pinRequired_CheckBox
            // 
            this.pinRequired_CheckBox.AutoSize = true;
            this.pinRequired_CheckBox.Location = new System.Drawing.Point(9, 22);
            this.pinRequired_CheckBox.Name = "pinRequired_CheckBox";
            this.pinRequired_CheckBox.Size = new System.Drawing.Size(93, 19);
            this.pinRequired_CheckBox.TabIndex = 56;
            this.pinRequired_CheckBox.Text = "Pin Required";
            this.pinRequired_CheckBox.UseVisualStyleBackColor = true;
            this.pinRequired_CheckBox.CheckedChanged += new System.EventHandler(this.pinRequired_CheckBox_CheckedChanged);
            // 
            // pin_TextBox
            // 
            this.pin_TextBox.Enabled = false;
            this.pin_TextBox.Location = new System.Drawing.Point(38, 51);
            this.pin_TextBox.MaxLength = 4;
            this.pin_TextBox.Name = "pin_TextBox";
            this.pin_TextBox.Size = new System.Drawing.Size(129, 23);
            this.pin_TextBox.TabIndex = 53;
            this.pin_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pin_TextBox_KeyPress);
            // 
            // scanDetails_Panel
            // 
            this.scanDetails_Panel.Controls.Add(this.groupBox_Authentication);
            this.scanDetails_Panel.Controls.Add(this.skipDelay_GroupBox);
            this.scanDetails_Panel.Controls.Add(this.pageCount_GroupBox);
            this.scanDetails_Panel.Controls.Add(this.scanToJobStorage_GroupBox);
            this.scanDetails_Panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.scanDetails_Panel.Location = new System.Drawing.Point(0, 0);
            this.scanDetails_Panel.Name = "scanDetails_Panel";
            this.scanDetails_Panel.Size = new System.Drawing.Size(675, 296);
            this.scanDetails_Panel.TabIndex = 59;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ScanToJobStorage);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(4, 113);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(666, 49);
            this.groupBox_Authentication.TabIndex = 97;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_ScanToJobStorage
            // 
            this.radioButton_ScanToJobStorage.AutoSize = true;
            this.radioButton_ScanToJobStorage.Checked = true;
            this.radioButton_ScanToJobStorage.Location = new System.Drawing.Point(116, 20);
            this.radioButton_ScanToJobStorage.Name = "radioButton_ScanToJobStorage";
            this.radioButton_ScanToJobStorage.Size = new System.Drawing.Size(121, 19);
            this.radioButton_ScanToJobStorage.TabIndex = 93;
            this.radioButton_ScanToJobStorage.TabStop = true;
            this.radioButton_ScanToJobStorage.Text = "ScanToJobStorage";
            this.radioButton_ScanToJobStorage.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(61, 19);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(447, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(307, 21);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // skipDelay_GroupBox
            // 
            this.skipDelay_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipDelay_GroupBox.Controls.Add(this.lockTimeoutControl);
            this.skipDelay_GroupBox.Controls.Add(this.label2);
            this.skipDelay_GroupBox.Location = new System.Drawing.Point(308, 168);
            this.skipDelay_GroupBox.Name = "skipDelay_GroupBox";
            this.skipDelay_GroupBox.Size = new System.Drawing.Size(359, 125);
            this.skipDelay_GroupBox.TabIndex = 56;
            this.skipDelay_GroupBox.TabStop = false;
            this.skipDelay_GroupBox.Text = "Skip Delay";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 57);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // pageCount_GroupBox
            // 
            this.pageCount_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_GroupBox.Controls.Add(this.label4);
            this.pageCount_GroupBox.Controls.Add(this.pageCount_NumericUpDown);
            this.pageCount_GroupBox.Controls.Add(this.label3);
            this.pageCount_GroupBox.Location = new System.Drawing.Point(6, 168);
            this.pageCount_GroupBox.Name = "pageCount_GroupBox";
            this.pageCount_GroupBox.Size = new System.Drawing.Size(296, 122);
            this.pageCount_GroupBox.TabIndex = 55;
            this.pageCount_GroupBox.TabStop = false;
            this.pageCount_GroupBox.Text = "Page Count";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 91);
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
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(0, 296);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(675, 216);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 69);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // ScanToJobStorageConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.scanDetails_Panel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToJobStorageConfigControl";
            this.Size = new System.Drawing.Size(675, 512);
            this.scanToJobStorage_GroupBox.ResumeLayout(false);
            this.scanToJobStorage_GroupBox.PerformLayout();
            this.scanoptions_groupBox.ResumeLayout(false);
            this.scanDetails_Panel.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.skipDelay_GroupBox.ResumeLayout(false);
            this.pageCount_GroupBox.ResumeLayout(false);
            this.pageCount_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox scanToJobStorage_GroupBox;
        private System.Windows.Forms.Label pinDescription_label;
        private System.Windows.Forms.CheckBox pinRequired_CheckBox;
        private System.Windows.Forms.TextBox pin_TextBox;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.Panel scanDetails_Panel;
        private System.Windows.Forms.Label pin_Label;
        private System.Windows.Forms.GroupBox pageCount_GroupBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox skipDelay_GroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox scanoptions_groupBox;
        private System.Windows.Forms.Button ScanOptions_Button;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ScanToJobStorage;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
