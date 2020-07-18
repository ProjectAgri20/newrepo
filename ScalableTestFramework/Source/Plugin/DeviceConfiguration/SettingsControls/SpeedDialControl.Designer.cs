namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class SpeedDialControl
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
            this.defaultWindows_Label = new System.Windows.Forms.Label();
            this.passwordLength_Label = new System.Windows.Forms.Label();
            this.windowsDomain_Label = new System.Windows.Forms.Label();
            this.speedDial_ChoiceControl = new HP.ScalableTest.Plugin.DeviceConfiguration.FieldControls.ThreeChoiceControl();
            this.SuspendLayout();
            // 
            // defaultWindows_Label
            // 
            this.defaultWindows_Label.AutoSize = true;
            this.defaultWindows_Label.Location = new System.Drawing.Point(16, 104);
            this.defaultWindows_Label.Name = "defaultWindows_Label";
            this.defaultWindows_Label.Size = new System.Drawing.Size(64, 13);
            this.defaultWindows_Label.TabIndex = 43;
            this.defaultWindows_Label.Text = "Fax Number";
            // 
            // passwordLength_Label
            // 
            this.passwordLength_Label.AutoSize = true;
            this.passwordLength_Label.Location = new System.Drawing.Point(16, 52);
            this.passwordLength_Label.Name = "passwordLength_Label";
            this.passwordLength_Label.Size = new System.Drawing.Size(35, 13);
            this.passwordLength_Label.TabIndex = 42;
            this.passwordLength_Label.Text = "Name";
            // 
            // windowsDomain_Label
            // 
            this.windowsDomain_Label.AutoSize = true;
            this.windowsDomain_Label.Location = new System.Drawing.Point(16, 79);
            this.windowsDomain_Label.Name = "windowsDomain_Label";
            this.windowsDomain_Label.Size = new System.Drawing.Size(99, 13);
            this.windowsDomain_Label.TabIndex = 41;
            this.windowsDomain_Label.Text = "Speed Dial Number";
            // 
            // speedDial_ChoiceControl
            // 
            this.speedDial_ChoiceControl.Location = new System.Drawing.Point(173, 27);
            this.speedDial_ChoiceControl.Name = "speedDial_ChoiceControl";
            this.speedDial_ChoiceControl.Size = new System.Drawing.Size(347, 129);
            this.speedDial_ChoiceControl.TabIndex = 44;
            // 
            // SpeedDialControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.speedDial_ChoiceControl);
            this.Controls.Add(this.defaultWindows_Label);
            this.Controls.Add(this.passwordLength_Label);
            this.Controls.Add(this.windowsDomain_Label);
            this.Name = "SpeedDialControl";
            this.Size = new System.Drawing.Size(705, 177);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label defaultWindows_Label;
        private System.Windows.Forms.Label passwordLength_Label;
        private System.Windows.Forms.Label windowsDomain_Label;
        private FieldControls.ThreeChoiceControl speedDial_ChoiceControl;
    }
}
