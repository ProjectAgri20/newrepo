namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class PrintServerUserControl
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
            this.queueName_TextBox = new System.Windows.Forms.TextBox();
            this.queueName_Label = new System.Windows.Forms.Label();
            this.configuration_GroupBox = new System.Windows.Forms.GroupBox();
            this.quota_CheckBox = new System.Windows.Forms.CheckBox();
            this.tracking_CheckBox = new System.Windows.Forms.CheckBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.ipm_Checkbox = new System.Windows.Forms.CheckBox();
            this.configuration_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // queueName_TextBox
            // 
            this.queueName_TextBox.Location = new System.Drawing.Point(90, 22);
            this.queueName_TextBox.Name = "queueName_TextBox";
            this.queueName_TextBox.Size = new System.Drawing.Size(374, 20);
            this.queueName_TextBox.TabIndex = 3;
            // 
            // queueName_Label
            // 
            this.queueName_Label.AutoSize = true;
            this.queueName_Label.Location = new System.Drawing.Point(6, 25);
            this.queueName_Label.Name = "queueName_Label";
            this.queueName_Label.Size = new System.Drawing.Size(70, 13);
            this.queueName_Label.TabIndex = 2;
            this.queueName_Label.Text = "Queue Name";
            // 
            // configuration_GroupBox
            // 
            this.configuration_GroupBox.Controls.Add(this.ipm_Checkbox);
            this.configuration_GroupBox.Controls.Add(this.quota_CheckBox);
            this.configuration_GroupBox.Controls.Add(this.tracking_CheckBox);
            this.configuration_GroupBox.Location = new System.Drawing.Point(6, 78);
            this.configuration_GroupBox.Name = "configuration_GroupBox";
            this.configuration_GroupBox.Size = new System.Drawing.Size(634, 82);
            this.configuration_GroupBox.TabIndex = 60;
            this.configuration_GroupBox.TabStop = false;
            this.configuration_GroupBox.Text = "Configuration";
            // 
            // quota_CheckBox
            // 
            this.quota_CheckBox.AutoSize = true;
            this.quota_CheckBox.Location = new System.Drawing.Point(102, 35);
            this.quota_CheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.quota_CheckBox.Name = "quota_CheckBox";
            this.quota_CheckBox.Size = new System.Drawing.Size(55, 17);
            this.quota_CheckBox.TabIndex = 4;
            this.quota_CheckBox.Text = "Quota";
            this.quota_CheckBox.UseVisualStyleBackColor = true;
            // 
            // tracking_CheckBox
            // 
            this.tracking_CheckBox.AutoSize = true;
            this.tracking_CheckBox.Location = new System.Drawing.Point(3, 35);
            this.tracking_CheckBox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.tracking_CheckBox.Name = "tracking_CheckBox";
            this.tracking_CheckBox.Size = new System.Drawing.Size(68, 17);
            this.tracking_CheckBox.TabIndex = 1;
            this.tracking_CheckBox.Text = "Tracking";
            this.tracking_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ipm_Checkbox
            // 
            this.ipm_Checkbox.AutoSize = true;
            this.ipm_Checkbox.Location = new System.Drawing.Point(189, 35);
            this.ipm_Checkbox.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.ipm_Checkbox.Name = "ipm_Checkbox";
            this.ipm_Checkbox.Size = new System.Drawing.Size(45, 17);
            this.ipm_Checkbox.TabIndex = 5;
            this.ipm_Checkbox.Text = "IPM";
            this.ipm_Checkbox.UseVisualStyleBackColor = true;
            // 
            // PrintServerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.configuration_GroupBox);
            this.Controls.Add(this.queueName_TextBox);
            this.Controls.Add(this.queueName_Label);
            this.Name = "PrintServerUserControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.configuration_GroupBox.ResumeLayout(false);
            this.configuration_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox queueName_TextBox;
        private System.Windows.Forms.Label queueName_Label;
        private System.Windows.Forms.GroupBox configuration_GroupBox;
        private System.Windows.Forms.CheckBox quota_CheckBox;
        private System.Windows.Forms.CheckBox tracking_CheckBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckBox ipm_Checkbox;
    }
}
