namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class JobAccountingUserControl
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
            this.quota_radioButton = new System.Windows.Forms.RadioButton();
            this.report_radioButton = new System.Windows.Forms.RadioButton();
            this.report_groupBox = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.outputFormat_Label = new System.Windows.Forms.Label();
            this.outputFormat_ComboBox = new System.Windows.Forms.ComboBox();
            this.sendReportEmail_Label = new System.Windows.Forms.Label();
            this.sendReportEmail_TextBox = new System.Windows.Forms.TextBox();
            this.reportName_TextBox = new System.Windows.Forms.TextBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.report_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // quota_radioButton
            // 
            this.quota_radioButton.AutoSize = true;
            this.quota_radioButton.Checked = true;
            this.quota_radioButton.Location = new System.Drawing.Point(30, 46);
            this.quota_radioButton.Name = "quota_radioButton";
            this.quota_radioButton.Size = new System.Drawing.Size(54, 17);
            this.quota_radioButton.TabIndex = 0;
            this.quota_radioButton.TabStop = true;
            this.quota_radioButton.Text = "Quota";
            this.quota_radioButton.UseVisualStyleBackColor = true;
            this.quota_radioButton.CheckedChanged += new System.EventHandler(this.quota_radioButton_CheckedChanged);
            // 
            // report_radioButton
            // 
            this.report_radioButton.AutoSize = true;
            this.report_radioButton.Location = new System.Drawing.Point(30, 103);
            this.report_radioButton.Name = "report_radioButton";
            this.report_radioButton.Size = new System.Drawing.Size(112, 17);
            this.report_radioButton.TabIndex = 1;
            this.report_radioButton.TabStop = true;
            this.report_radioButton.Text = "Report Generation";
            this.report_radioButton.UseVisualStyleBackColor = true;
            // 
            // report_groupBox
            // 
            this.report_groupBox.Controls.Add(this.outputFormat_Label);
            this.report_groupBox.Controls.Add(this.outputFormat_ComboBox);
            this.report_groupBox.Controls.Add(this.sendReportEmail_Label);
            this.report_groupBox.Controls.Add(this.sendReportEmail_TextBox);
            this.report_groupBox.Controls.Add(this.reportName_TextBox);
            this.report_groupBox.Controls.Add(this.name_Label);
            this.report_groupBox.Enabled = false;
            this.report_groupBox.Location = new System.Drawing.Point(30, 168);
            this.report_groupBox.Name = "report_groupBox";
            this.report_groupBox.Size = new System.Drawing.Size(441, 163);
            this.report_groupBox.TabIndex = 2;
            this.report_groupBox.TabStop = false;
            this.report_groupBox.Text = "Report Generation";
            // 
            // outputFormat_Label
            // 
            this.outputFormat_Label.AutoSize = true;
            this.outputFormat_Label.Location = new System.Drawing.Point(6, 122);
            this.outputFormat_Label.Name = "outputFormat_Label";
            this.outputFormat_Label.Size = new System.Drawing.Size(74, 13);
            this.outputFormat_Label.TabIndex = 19;
            this.outputFormat_Label.Text = "Output Format";
            // 
            // outputFormat_ComboBox
            // 
            this.outputFormat_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputFormat_ComboBox.FormattingEnabled = true;
            this.outputFormat_ComboBox.Location = new System.Drawing.Point(139, 119);
            this.outputFormat_ComboBox.Name = "outputFormat_ComboBox";
            this.outputFormat_ComboBox.Size = new System.Drawing.Size(200, 21);
            this.outputFormat_ComboBox.TabIndex = 20;
            // 
            // sendReportEmail_Label
            // 
            this.sendReportEmail_Label.AutoSize = true;
            this.sendReportEmail_Label.Location = new System.Drawing.Point(6, 80);
            this.sendReportEmail_Label.Name = "sendReportEmail_Label";
            this.sendReportEmail_Label.Size = new System.Drawing.Size(115, 13);
            this.sendReportEmail_Label.TabIndex = 17;
            this.sendReportEmail_Label.Text = "Send report by email to";
            // 
            // sendReportEmail_TextBox
            // 
            this.sendReportEmail_TextBox.Location = new System.Drawing.Point(139, 77);
            this.sendReportEmail_TextBox.Name = "sendReportEmail_TextBox";
            this.sendReportEmail_TextBox.Size = new System.Drawing.Size(200, 20);
            this.sendReportEmail_TextBox.TabIndex = 18;
            // 
            // reportName_TextBox
            // 
            this.reportName_TextBox.Location = new System.Drawing.Point(139, 35);
            this.reportName_TextBox.Name = "reportName_TextBox";
            this.reportName_TextBox.Size = new System.Drawing.Size(200, 20);
            this.reportName_TextBox.TabIndex = 16;
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Location = new System.Drawing.Point(6, 38);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(95, 13);
            this.name_Label.TabIndex = 15;
            this.name_Label.Text = "Name of the report";
            // 
            // JobAccountingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.report_groupBox);
            this.Controls.Add(this.report_radioButton);
            this.Controls.Add(this.quota_radioButton);
            this.Name = "JobAccountingUserControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.report_groupBox.ResumeLayout(false);
            this.report_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.RadioButton quota_radioButton;
        private System.Windows.Forms.RadioButton report_radioButton;
        private System.Windows.Forms.GroupBox report_groupBox;
        private System.Windows.Forms.Label outputFormat_Label;
        private System.Windows.Forms.ComboBox outputFormat_ComboBox;
        private System.Windows.Forms.Label sendReportEmail_Label;
        private System.Windows.Forms.TextBox sendReportEmail_TextBox;
        private System.Windows.Forms.TextBox reportName_TextBox;
        private System.Windows.Forms.Label name_Label;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
