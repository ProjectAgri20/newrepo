namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class JobStorageDefaultControl
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
            this.enable_label = new System.Windows.Forms.Label();
            this.retainJobs_label = new System.Windows.Forms.Label();
            this.folderName_label = new System.Windows.Forms.Label();
            this.pinRequired_label = new System.Windows.Forms.Label();
            this.EnablePinLength_label = new System.Windows.Forms.Label();
            this.Settings_Label = new System.Windows.Forms.Label();
            this.folderName_choiceTextControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.enable_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.retainJobs_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.pinRequired_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.pinLength_choiceComboControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.SuspendLayout();
            // 
            // enable_label
            // 
            this.enable_label.AutoSize = true;
            this.enable_label.Location = new System.Drawing.Point(63, 53);
            this.enable_label.Name = "enable_label";
            this.enable_label.Size = new System.Drawing.Size(67, 13);
            this.enable_label.TabIndex = 57;
            this.enable_label.Text = "Job Storage:";
            // 
            // retainJobs_label
            // 
            this.retainJobs_label.AutoSize = true;
            this.retainJobs_label.Location = new System.Drawing.Point(64, 214);
            this.retainJobs_label.Name = "retainJobs_label";
            this.retainJobs_label.Size = new System.Drawing.Size(66, 13);
            this.retainJobs_label.TabIndex = 51;
            this.retainJobs_label.Text = "Retain Jobs:";
            // 
            // folderName_label
            // 
            this.folderName_label.AutoSize = true;
            this.folderName_label.Location = new System.Drawing.Point(23, 174);
            this.folderName_label.Name = "folderName_label";
            this.folderName_label.Size = new System.Drawing.Size(107, 13);
            this.folderName_label.TabIndex = 50;
            this.folderName_label.Text = "Default Folder Name:";
            // 
            // pinRequired_label
            // 
            this.pinRequired_label.AutoSize = true;
            this.pinRequired_label.Location = new System.Drawing.Point(59, 134);
            this.pinRequired_label.Name = "pinRequired_label";
            this.pinRequired_label.Size = new System.Drawing.Size(71, 13);
            this.pinRequired_label.TabIndex = 49;
            this.pinRequired_label.Text = "Pin Required:";
            // 
            // EnablePinLength_label
            // 
            this.EnablePinLength_label.AutoSize = true;
            this.EnablePinLength_label.Location = new System.Drawing.Point(24, 94);
            this.EnablePinLength_label.Name = "EnablePinLength_label";
            this.EnablePinLength_label.Size = new System.Drawing.Size(106, 13);
            this.EnablePinLength_label.TabIndex = 45;
            this.EnablePinLength_label.Text = "Four digit PIN length:";
            // 
            // Settings_Label
            // 
            this.Settings_Label.AutoSize = true;
            this.Settings_Label.Location = new System.Drawing.Point(195, 0);
            this.Settings_Label.Name = "Settings_Label";
            this.Settings_Label.Size = new System.Drawing.Size(105, 13);
            this.Settings_Label.TabIndex = 43;
            this.Settings_Label.Text = "Job Storage Settings";
            // 
            // folderName_choiceTextControl
            // 
            this.folderName_choiceTextControl.Location = new System.Drawing.Point(134, 167);
            this.folderName_choiceTextControl.Name = "folderName_choiceTextControl";
            this.folderName_choiceTextControl.Size = new System.Drawing.Size(335, 27);
            this.folderName_choiceTextControl.TabIndex = 59;
            // 
            // enable_choiceComboControl
            // 
            this.enable_choiceComboControl.Location = new System.Drawing.Point(134, 47);
            this.enable_choiceComboControl.Name = "enable_choiceComboControl";
            this.enable_choiceComboControl.Size = new System.Drawing.Size(335, 27);
            this.enable_choiceComboControl.TabIndex = 58;
            // 
            // retainJobs_choiceComboControl
            // 
            this.retainJobs_choiceComboControl.Location = new System.Drawing.Point(134, 207);
            this.retainJobs_choiceComboControl.Name = "retainJobs_choiceComboControl";
            this.retainJobs_choiceComboControl.Size = new System.Drawing.Size(335, 27);
            this.retainJobs_choiceComboControl.TabIndex = 52;
            // 
            // pinRequired_choiceComboControl
            // 
            this.pinRequired_choiceComboControl.Location = new System.Drawing.Point(134, 127);
            this.pinRequired_choiceComboControl.Name = "pinRequired_choiceComboControl";
            this.pinRequired_choiceComboControl.Size = new System.Drawing.Size(335, 27);
            this.pinRequired_choiceComboControl.TabIndex = 47;
            // 
            // pinLength_choiceComboControl
            // 
            this.pinLength_choiceComboControl.Location = new System.Drawing.Point(134, 87);
            this.pinLength_choiceComboControl.Name = "pinLength_choiceComboControl";
            this.pinLength_choiceComboControl.Size = new System.Drawing.Size(335, 27);
            this.pinLength_choiceComboControl.TabIndex = 46;
            // 
            // JobStorageDefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.folderName_choiceTextControl);
            this.Controls.Add(this.enable_choiceComboControl);
            this.Controls.Add(this.enable_label);
            this.Controls.Add(this.retainJobs_choiceComboControl);
            this.Controls.Add(this.retainJobs_label);
            this.Controls.Add(this.folderName_label);
            this.Controls.Add(this.pinRequired_label);
            this.Controls.Add(this.pinRequired_choiceComboControl);
            this.Controls.Add(this.pinLength_choiceComboControl);
            this.Controls.Add(this.EnablePinLength_label);
            this.Controls.Add(this.Settings_Label);
            this.Name = "JobStorageDefaultControl";
            this.Size = new System.Drawing.Size(500, 260);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChoiceComboControl enable_choiceComboControl;
        private System.Windows.Forms.Label enable_label;
        private ChoiceComboControl retainJobs_choiceComboControl;
        private System.Windows.Forms.Label retainJobs_label;
        private System.Windows.Forms.Label folderName_label;
        private System.Windows.Forms.Label pinRequired_label;
        private ChoiceComboControl pinRequired_choiceComboControl;
        private ChoiceComboControl pinLength_choiceComboControl;
        private System.Windows.Forms.Label EnablePinLength_label;
        private System.Windows.Forms.Label Settings_Label;
        private ChoiceTextControl folderName_choiceTextControl;
    }
}
