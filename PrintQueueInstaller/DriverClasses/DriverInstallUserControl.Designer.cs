namespace HP.ScalableTest.Print.Utility
{
    partial class DriverInstallUserControl
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
            this.downloadDriver_Button = new System.Windows.Forms.Button();
            this.driverPackagePath_ComboBox = new System.Windows.Forms.ComboBox();
            this.browse_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.model_Label = new System.Windows.Forms.Label();
            this.inboxDriver_Button = new System.Windows.Forms.Button();
            this.includeAllArchitectures_CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // downloadDriver_Button
            // 
            this.downloadDriver_Button.Location = new System.Drawing.Point(262, 83);
            this.downloadDriver_Button.Name = "downloadDriver_Button";
            this.downloadDriver_Button.Size = new System.Drawing.Size(122, 23);
            this.downloadDriver_Button.TabIndex = 53;
            this.downloadDriver_Button.Text = "Download Driver";
            this.downloadDriver_Button.UseVisualStyleBackColor = true;
            this.downloadDriver_Button.Click += new System.EventHandler(this.downloadDriver_Button_Click);
            // 
            // driverPackagePath_ComboBox
            // 
            this.driverPackagePath_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driverPackagePath_ComboBox.FormattingEnabled = true;
            this.driverPackagePath_ComboBox.Location = new System.Drawing.Point(134, 5);
            this.driverPackagePath_ComboBox.Name = "driverPackagePath_ComboBox";
            this.driverPackagePath_ComboBox.Size = new System.Drawing.Size(495, 21);
            this.driverPackagePath_ComboBox.TabIndex = 0;
            this.driverPackagePath_ComboBox.SelectedIndexChanged += new System.EventHandler(this.driverPackagePath_ComboBox_SelectedIndexChanged);
            this.driverPackagePath_ComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.driverPackagePath_ComboBox_KeyDown);
            // 
            // browse_Button
            // 
            this.browse_Button.Location = new System.Drawing.Point(134, 83);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(122, 23);
            this.browse_Button.TabIndex = 5;
            this.browse_Button.Text = "Browse Locally...";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.browse_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Local Driver Package";
            // 
            // model_Label
            // 
            this.model_Label.AutoSize = true;
            this.model_Label.Location = new System.Drawing.Point(61, 35);
            this.model_Label.Name = "model_Label";
            this.model_Label.Size = new System.Drawing.Size(67, 13);
            this.model_Label.TabIndex = 22;
            this.model_Label.Text = "Driver Model";
            // 
            // inboxDriver_Button
            // 
            this.inboxDriver_Button.Location = new System.Drawing.Point(390, 83);
            this.inboxDriver_Button.Name = "inboxDriver_Button";
            this.inboxDriver_Button.Size = new System.Drawing.Size(122, 23);
            this.inboxDriver_Button.TabIndex = 54;
            this.inboxDriver_Button.Text = "Select In-box Driver";
            this.inboxDriver_Button.UseVisualStyleBackColor = true;
            this.inboxDriver_Button.Click += new System.EventHandler(this.inboxDriver_Button_Click);
            // 
            // includeAllArchitectures_CheckBox
            // 
            this.includeAllArchitectures_CheckBox.AutoSize = true;
            this.includeAllArchitectures_CheckBox.Location = new System.Drawing.Point(134, 60);
            this.includeAllArchitectures_CheckBox.Name = "includeAllArchitectures_CheckBox";
            this.includeAllArchitectures_CheckBox.Size = new System.Drawing.Size(138, 17);
            this.includeAllArchitectures_CheckBox.TabIndex = 56;
            this.includeAllArchitectures_CheckBox.Text = "Include all architectures";
            this.includeAllArchitectures_CheckBox.UseVisualStyleBackColor = true;
            this.includeAllArchitectures_CheckBox.CheckedChanged += new System.EventHandler(this.includeAllArchitectures_CheckBox_CheckedChanged);
            // 
            // DriverInstallUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.includeAllArchitectures_CheckBox);
            this.Controls.Add(this.inboxDriver_Button);
            this.Controls.Add(this.downloadDriver_Button);
            this.Controls.Add(this.driverPackagePath_ComboBox);
            this.Controls.Add(this.browse_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.model_Label);
            this.Name = "DriverInstallUserControl";
            this.Size = new System.Drawing.Size(632, 117);
            this.Load += new System.EventHandler(this.DriverInstallUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button downloadDriver_Button;
        private System.Windows.Forms.ComboBox driverPackagePath_ComboBox;
        private System.Windows.Forms.Button browse_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label model_Label;
        private System.Windows.Forms.Button inboxDriver_Button;
        private System.Windows.Forms.CheckBox includeAllArchitectures_CheckBox;
    }
}
