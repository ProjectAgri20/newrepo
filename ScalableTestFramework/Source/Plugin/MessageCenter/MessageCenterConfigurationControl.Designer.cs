namespace HP.ScalableTest.Plugin.MessageCenter
{
    partial class MessageCenterConfigurationControl
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
            this.message_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.messageType_label = new System.Windows.Forms.Label();
            this.messageType_comboBox = new System.Windows.Forms.ComboBox();
            this.present_radioButton = new System.Windows.Forms.RadioButton();
            this.absent_radioButton = new System.Windows.Forms.RadioButton();
            this.message_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.method_groupBox = new System.Windows.Forms.GroupBox();
            this.controlpanel_radioButton = new System.Windows.Forms.RadioButton();
            this.ews_radioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.method_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageType_label
            // 
            this.messageType_label.AutoSize = true;
            this.messageType_label.Location = new System.Drawing.Point(10, 371);
            this.messageType_label.Name = "messageType_label";
            this.messageType_label.Size = new System.Drawing.Size(100, 15);
            this.messageType_label.TabIndex = 0;
            this.messageType_label.Text = "Evaluate Message";
            // 
            // messageType_comboBox
            // 
            this.messageType_comboBox.FormattingEnabled = true;
            this.messageType_comboBox.Location = new System.Drawing.Point(13, 389);
            this.messageType_comboBox.Name = "messageType_comboBox";
            this.messageType_comboBox.Size = new System.Drawing.Size(187, 23);
            this.messageType_comboBox.TabIndex = 1;
            // 
            // present_radioButton
            // 
            this.present_radioButton.AutoSize = true;
            this.present_radioButton.Checked = true;
            this.present_radioButton.Location = new System.Drawing.Point(6, 31);
            this.present_radioButton.Name = "present_radioButton";
            this.present_radioButton.Size = new System.Drawing.Size(72, 19);
            this.present_radioButton.TabIndex = 3;
            this.present_radioButton.TabStop = true;
            this.present_radioButton.Text = "Presence";
            this.present_radioButton.UseVisualStyleBackColor = true;
            // 
            // absent_radioButton
            // 
            this.absent_radioButton.AutoSize = true;
            this.absent_radioButton.Location = new System.Drawing.Point(117, 31);
            this.absent_radioButton.Name = "absent_radioButton";
            this.absent_radioButton.Size = new System.Drawing.Size(70, 19);
            this.absent_radioButton.TabIndex = 4;
            this.absent_radioButton.Text = "Absence";
            this.absent_radioButton.UseVisualStyleBackColor = true;
            // 
            // message_assetSelectionControl
            // 
            this.message_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.message_assetSelectionControl.Location = new System.Drawing.Point(13, 12);
            this.message_assetSelectionControl.Name = "message_assetSelectionControl";
            this.message_assetSelectionControl.Size = new System.Drawing.Size(577, 347);
            this.message_assetSelectionControl.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.present_radioButton);
            this.groupBox1.Controls.Add(this.absent_radioButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 419);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 74);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Check for Message";
            // 
            // method_groupBox
            // 
            this.method_groupBox.Controls.Add(this.ews_radioButton);
            this.method_groupBox.Controls.Add(this.controlpanel_radioButton);
            this.method_groupBox.Location = new System.Drawing.Point(237, 419);
            this.method_groupBox.Name = "method_groupBox";
            this.method_groupBox.Size = new System.Drawing.Size(314, 74);
            this.method_groupBox.TabIndex = 7;
            this.method_groupBox.TabStop = false;
            this.method_groupBox.Text = "Message Center Method";
            // 
            // controlpanel_radioButton
            // 
            this.controlpanel_radioButton.AutoSize = true;
            this.controlpanel_radioButton.Checked = true;
            this.controlpanel_radioButton.Location = new System.Drawing.Point(6, 22);
            this.controlpanel_radioButton.Name = "controlpanel_radioButton";
            this.controlpanel_radioButton.Size = new System.Drawing.Size(142, 19);
            this.controlpanel_radioButton.TabIndex = 0;
            this.controlpanel_radioButton.TabStop = true;
            this.controlpanel_radioButton.Text = "Control Panel (legacy)";
            this.controlpanel_radioButton.UseVisualStyleBackColor = true;
            // 
            // ews_radioButton
            // 
            this.ews_radioButton.AutoSize = true;
            this.ews_radioButton.Location = new System.Drawing.Point(6, 46);
            this.ews_radioButton.Name = "ews_radioButton";
            this.ews_radioButton.Size = new System.Drawing.Size(192, 19);
            this.ews_radioButton.TabIndex = 1;
            this.ews_radioButton.Text = "EWS (faster and recommended)";
            this.ews_radioButton.UseVisualStyleBackColor = true;
            // 
            // MessageCenterConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.method_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.message_assetSelectionControl);
            this.Controls.Add(this.messageType_comboBox);
            this.Controls.Add(this.messageType_label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MessageCenterConfigurationControl";
            this.Size = new System.Drawing.Size(599, 496);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.method_groupBox.ResumeLayout(false);
            this.method_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Framework.UI.FieldValidator message_fieldValidator;
        private System.Windows.Forms.Label messageType_label;
        private System.Windows.Forms.ComboBox messageType_comboBox;
        private System.Windows.Forms.RadioButton present_radioButton;
        private System.Windows.Forms.RadioButton absent_radioButton;
        private Framework.UI.AssetSelectionControl message_assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox method_groupBox;
        private System.Windows.Forms.RadioButton ews_radioButton;
        private System.Windows.Forms.RadioButton controlpanel_radioButton;
    }
}
