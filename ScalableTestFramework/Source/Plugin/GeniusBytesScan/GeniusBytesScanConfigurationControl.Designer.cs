namespace HP.ScalableTest.Plugin.GeniusBytesScan
{
    partial class GeniusBytesScanConfigurationControl
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
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ProximityCardLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_GuestLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_PINLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_ManualLogin = new System.Windows.Forms.RadioButton();
            this.groupBox_PullPrint = new System.Windows.Forms.GroupBox();
            this.radioButton_Scan2ME = new System.Windows.Forms.RadioButton();
            this.radioButton_Scan2Mail = new System.Windows.Forms.RadioButton();
            this.radioButton_Scan2Home = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxImagePreview = new System.Windows.Forms.CheckBox();
            this.comboBox_FileType = new System.Windows.Forms.ComboBox();
            this.label_FileType = new System.Windows.Forms.Label();
            this.jobBuildPageCount_label = new System.Windows.Forms.Label();
            this.jobBuildPageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.comboBox_Resolution = new System.Windows.Forms.ComboBox();
            this.label2_Resolution = new System.Windows.Forms.Label();
            this.comboBox_Sides = new System.Windows.Forms.ComboBox();
            this.label1_Sides = new System.Windows.Forms.Label();
            this.comboBox_Colour = new System.Windows.Forms.ComboBox();
            this.label_ColourMode = new System.Windows.Forms.Label();
            this.groupBox_Device.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_PullPrint.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Controls.Add(this.checkBox_ReleaseOnSignIn);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(678, 197);
            this.groupBox_Device.TabIndex = 4;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // checkBox_ReleaseOnSignIn
            // 
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(278, 152);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(304, 19);
            this.checkBox_ReleaseOnSignIn.TabIndex = 3;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 140);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 22);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 114);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ProximityCardLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_GuestLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_PINLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_ManualLogin);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 201);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(678, 57);
            this.groupBox_Authentication.TabIndex = 5;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // radioButton_ProximityCardLogin
            // 
            this.radioButton_ProximityCardLogin.AutoSize = true;
            this.radioButton_ProximityCardLogin.Location = new System.Drawing.Point(349, 22);
            this.radioButton_ProximityCardLogin.Name = "radioButton_ProximityCardLogin";
            this.radioButton_ProximityCardLogin.Size = new System.Drawing.Size(136, 19);
            this.radioButton_ProximityCardLogin.TabIndex = 5;
            this.radioButton_ProximityCardLogin.Text = "Proximity Card Login";
            this.radioButton_ProximityCardLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_GuestLogin
            // 
            this.radioButton_GuestLogin.AutoSize = true;
            this.radioButton_GuestLogin.Enabled = false;
            this.radioButton_GuestLogin.Location = new System.Drawing.Point(15, 22);
            this.radioButton_GuestLogin.Name = "radioButton_GuestLogin";
            this.radioButton_GuestLogin.Size = new System.Drawing.Size(88, 19);
            this.radioButton_GuestLogin.TabIndex = 4;
            this.radioButton_GuestLogin.Text = "Guest Login";
            this.radioButton_GuestLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_PINLogin
            // 
            this.radioButton_PINLogin.AutoSize = true;
            this.radioButton_PINLogin.Checked = true;
            this.radioButton_PINLogin.Location = new System.Drawing.Point(130, 22);
            this.radioButton_PINLogin.Name = "radioButton_PINLogin";
            this.radioButton_PINLogin.Size = new System.Drawing.Size(77, 19);
            this.radioButton_PINLogin.TabIndex = 3;
            this.radioButton_PINLogin.TabStop = true;
            this.radioButton_PINLogin.Text = "PIN Login";
            this.radioButton_PINLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_ManualLogin
            // 
            this.radioButton_ManualLogin.AutoSize = true;
            this.radioButton_ManualLogin.Location = new System.Drawing.Point(235, 22);
            this.radioButton_ManualLogin.Name = "radioButton_ManualLogin";
            this.radioButton_ManualLogin.Size = new System.Drawing.Size(98, 19);
            this.radioButton_ManualLogin.TabIndex = 2;
            this.radioButton_ManualLogin.Text = "Manual Login";
            this.radioButton_ManualLogin.UseVisualStyleBackColor = true;
            // 
            // groupBox_PullPrint
            // 
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Scan2ME);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Scan2Mail);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Scan2Home);
            this.groupBox_PullPrint.Location = new System.Drawing.Point(3, 257);
            this.groupBox_PullPrint.Name = "groupBox_PullPrint";
            this.groupBox_PullPrint.Size = new System.Drawing.Size(678, 67);
            this.groupBox_PullPrint.TabIndex = 6;
            this.groupBox_PullPrint.TabStop = false;
            this.groupBox_PullPrint.Text = "Scan App Selection";
            // 
            // radioButton_Scan2ME
            // 
            this.radioButton_Scan2ME.AutoSize = true;
            this.radioButton_Scan2ME.Checked = true;
            this.radioButton_Scan2ME.Location = new System.Drawing.Point(15, 28);
            this.radioButton_Scan2ME.Name = "radioButton_Scan2ME";
            this.radioButton_Scan2ME.Size = new System.Drawing.Size(73, 19);
            this.radioButton_Scan2ME.TabIndex = 4;
            this.radioButton_Scan2ME.TabStop = true;
            this.radioButton_Scan2ME.Text = "Scan2ME";
            this.radioButton_Scan2ME.UseVisualStyleBackColor = true;
            // 
            // radioButton_Scan2Mail
            // 
            this.radioButton_Scan2Mail.AutoSize = true;
            this.radioButton_Scan2Mail.Location = new System.Drawing.Point(235, 28);
            this.radioButton_Scan2Mail.Name = "radioButton_Scan2Mail";
            this.radioButton_Scan2Mail.Size = new System.Drawing.Size(79, 19);
            this.radioButton_Scan2Mail.TabIndex = 1;
            this.radioButton_Scan2Mail.TabStop = true;
            this.radioButton_Scan2Mail.Text = "Scan2Mail";
            this.radioButton_Scan2Mail.UseVisualStyleBackColor = true;
            // 
            // radioButton_Scan2Home
            // 
            this.radioButton_Scan2Home.AutoSize = true;
            this.radioButton_Scan2Home.Location = new System.Drawing.Point(129, 28);
            this.radioButton_Scan2Home.Name = "radioButton_Scan2Home";
            this.radioButton_Scan2Home.Size = new System.Drawing.Size(89, 19);
            this.radioButton_Scan2Home.TabIndex = 0;
            this.radioButton_Scan2Home.TabStop = true;
            this.radioButton_Scan2Home.Text = "Scan2Home";
            this.radioButton_Scan2Home.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxImagePreview);
            this.groupBox1.Controls.Add(this.comboBox_FileType);
            this.groupBox1.Controls.Add(this.label_FileType);
            this.groupBox1.Controls.Add(this.jobBuildPageCount_label);
            this.groupBox1.Controls.Add(this.jobBuildPageCount_numericUpDown);
            this.groupBox1.Controls.Add(this.comboBox_Resolution);
            this.groupBox1.Controls.Add(this.label2_Resolution);
            this.groupBox1.Controls.Add(this.comboBox_Sides);
            this.groupBox1.Controls.Add(this.label1_Sides);
            this.groupBox1.Controls.Add(this.comboBox_Colour);
            this.groupBox1.Controls.Add(this.label_ColourMode);
            this.groupBox1.Location = new System.Drawing.Point(4, 330);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(677, 188);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option Configuration";
            // 
            // checkBoxImagePreview
            // 
            this.checkBoxImagePreview.AutoSize = true;
            this.checkBoxImagePreview.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxImagePreview.Location = new System.Drawing.Point(390, 25);
            this.checkBoxImagePreview.Name = "checkBoxImagePreview";
            this.checkBoxImagePreview.Size = new System.Drawing.Size(109, 19);
            this.checkBoxImagePreview.TabIndex = 53;
            this.checkBoxImagePreview.Text = "Image Preview: ";
            this.checkBoxImagePreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxImagePreview.UseVisualStyleBackColor = true;
            // 
            // comboBox_FileType
            // 
            this.comboBox_FileType.DisplayMember = "Value";
            this.comboBox_FileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_FileType.FormattingEnabled = true;
            this.comboBox_FileType.Location = new System.Drawing.Point(197, 23);
            this.comboBox_FileType.Name = "comboBox_FileType";
            this.comboBox_FileType.Size = new System.Drawing.Size(176, 23);
            this.comboBox_FileType.TabIndex = 13;
            this.comboBox_FileType.ValueMember = "Key";
            // 
            // label_FileType
            // 
            this.label_FileType.AutoSize = true;
            this.label_FileType.Location = new System.Drawing.Point(14, 23);
            this.label_FileType.Name = "label_FileType";
            this.label_FileType.Size = new System.Drawing.Size(61, 15);
            this.label_FileType.TabIndex = 12;
            this.label_FileType.Text = "* File Type";
            // 
            // jobBuildPageCount_label
            // 
            this.jobBuildPageCount_label.AutoSize = true;
            this.jobBuildPageCount_label.Location = new System.Drawing.Point(17, 156);
            this.jobBuildPageCount_label.Name = "jobBuildPageCount_label";
            this.jobBuildPageCount_label.Size = new System.Drawing.Size(76, 15);
            this.jobBuildPageCount_label.TabIndex = 10;
            this.jobBuildPageCount_label.Text = "* Scan Count";
            // 
            // jobBuildPageCount_numericUpDown
            // 
            this.jobBuildPageCount_numericUpDown.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.jobBuildPageCount_numericUpDown.Location = new System.Drawing.Point(198, 154);
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
            this.jobBuildPageCount_numericUpDown.Size = new System.Drawing.Size(177, 23);
            this.jobBuildPageCount_numericUpDown.TabIndex = 11;
            this.jobBuildPageCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBox_Resolution
            // 
            this.comboBox_Resolution.DisplayMember = "Value";
            this.comboBox_Resolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Resolution.FormattingEnabled = true;
            this.comboBox_Resolution.Location = new System.Drawing.Point(198, 120);
            this.comboBox_Resolution.Name = "comboBox_Resolution";
            this.comboBox_Resolution.Size = new System.Drawing.Size(176, 23);
            this.comboBox_Resolution.TabIndex = 9;
            this.comboBox_Resolution.ValueMember = "Key";
            // 
            // label2_Resolution
            // 
            this.label2_Resolution.AutoSize = true;
            this.label2_Resolution.Location = new System.Drawing.Point(15, 123);
            this.label2_Resolution.Name = "label2_Resolution";
            this.label2_Resolution.Size = new System.Drawing.Size(71, 15);
            this.label2_Resolution.TabIndex = 8;
            this.label2_Resolution.Text = "* Resolution";
            // 
            // comboBox_Sides
            // 
            this.comboBox_Sides.DisplayMember = "Value";
            this.comboBox_Sides.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Sides.FormattingEnabled = true;
            this.comboBox_Sides.Location = new System.Drawing.Point(197, 85);
            this.comboBox_Sides.Name = "comboBox_Sides";
            this.comboBox_Sides.Size = new System.Drawing.Size(176, 23);
            this.comboBox_Sides.TabIndex = 7;
            this.comboBox_Sides.ValueMember = "Key";
            // 
            // label1_Sides
            // 
            this.label1_Sides.AutoSize = true;
            this.label1_Sides.Location = new System.Drawing.Point(14, 88);
            this.label1_Sides.Name = "label1_Sides";
            this.label1_Sides.Size = new System.Drawing.Size(42, 15);
            this.label1_Sides.TabIndex = 6;
            this.label1_Sides.Text = "* Sides";
            // 
            // comboBox_Colour
            // 
            this.comboBox_Colour.DisplayMember = "Value";
            this.comboBox_Colour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Colour.FormattingEnabled = true;
            this.comboBox_Colour.Location = new System.Drawing.Point(198, 52);
            this.comboBox_Colour.Name = "comboBox_Colour";
            this.comboBox_Colour.Size = new System.Drawing.Size(176, 23);
            this.comboBox_Colour.TabIndex = 5;
            this.comboBox_Colour.ValueMember = "Key";
            // 
            // label_ColourMode
            // 
            this.label_ColourMode.AutoSize = true;
            this.label_ColourMode.Location = new System.Drawing.Point(15, 55);
            this.label_ColourMode.Name = "label_ColourMode";
            this.label_ColourMode.Size = new System.Drawing.Size(85, 15);
            this.label_ColourMode.TabIndex = 4;
            this.label_ColourMode.Text = "* Colour Mode";
            // 
            // GeniusBytesScanConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_PullPrint);
            this.Controls.Add(this.groupBox_Authentication);
            this.Controls.Add(this.groupBox_Device);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GeniusBytesScanConfigurationControl";
            this.Size = new System.Drawing.Size(689, 553);
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_PullPrint.ResumeLayout(false);
            this.groupBox_PullPrint.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ProximityCardLogin;
        private System.Windows.Forms.RadioButton radioButton_GuestLogin;
        private System.Windows.Forms.RadioButton radioButton_PINLogin;
        private System.Windows.Forms.RadioButton radioButton_ManualLogin;
        private System.Windows.Forms.GroupBox groupBox_PullPrint;
        private System.Windows.Forms.RadioButton radioButton_Scan2ME;
        private System.Windows.Forms.RadioButton radioButton_Scan2Mail;
        private System.Windows.Forms.RadioButton radioButton_Scan2Home;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_Resolution;
        private System.Windows.Forms.Label label2_Resolution;
        private System.Windows.Forms.ComboBox comboBox_Sides;
        private System.Windows.Forms.Label label1_Sides;
        private System.Windows.Forms.ComboBox comboBox_Colour;
        private System.Windows.Forms.Label label_ColourMode;
        private System.Windows.Forms.Label jobBuildPageCount_label;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_numericUpDown;
        private System.Windows.Forms.ComboBox comboBox_FileType;
        private System.Windows.Forms.Label label_FileType;
        private System.Windows.Forms.CheckBox checkBoxImagePreview;
    }
}
