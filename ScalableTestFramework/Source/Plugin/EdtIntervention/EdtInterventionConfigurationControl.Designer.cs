namespace HP.ScalableTest.Plugin.EdtIntervention
{
    partial class EdtInterventionConfigurationControl
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
            this.AlertTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.edtTestType_comboBox = new System.Windows.Forms.ComboBox();
            this.edtTestType_label = new System.Windows.Forms.Label();
            this.testMethod_label = new System.Windows.Forms.Label();
            this.testMethod_comboBox = new System.Windows.Forms.ComboBox();
            this.wakeMethod_comboBox = new System.Windows.Forms.ComboBox();
            this.wakeMethod_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AlertTextBox
            // 
            this.AlertTextBox.Location = new System.Drawing.Point(63, 37);
            this.AlertTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.AlertTextBox.Multiline = true;
            this.AlertTextBox.Name = "AlertTextBox";
            this.AlertTextBox.Size = new System.Drawing.Size(688, 105);
            this.AlertTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Alert Message:";
            // 
            // edtTestType_comboBox
            // 
            this.edtTestType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.edtTestType_comboBox.FormattingEnabled = true;
            this.edtTestType_comboBox.Location = new System.Drawing.Point(63, 187);
            this.edtTestType_comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.edtTestType_comboBox.Name = "edtTestType_comboBox";
            this.edtTestType_comboBox.Size = new System.Drawing.Size(197, 24);
            this.edtTestType_comboBox.TabIndex = 2;
            this.edtTestType_comboBox.SelectedIndexChanged += new System.EventHandler(this.edtTestType_comboBox_SelectedIndexChanged);
            // 
            // edtTestType_label
            // 
            this.edtTestType_label.AutoSize = true;
            this.edtTestType_label.Location = new System.Drawing.Point(59, 167);
            this.edtTestType_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.edtTestType_label.Name = "edtTestType_label";
            this.edtTestType_label.Size = new System.Drawing.Size(108, 17);
            this.edtTestType_label.TabIndex = 3;
            this.edtTestType_label.Text = "EDT Test Type:";
            // 
            // testMethod_label
            // 
            this.testMethod_label.AutoSize = true;
            this.testMethod_label.Location = new System.Drawing.Point(292, 167);
            this.testMethod_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.testMethod_label.Name = "testMethod_label";
            this.testMethod_label.Size = new System.Drawing.Size(91, 17);
            this.testMethod_label.TabIndex = 5;
            this.testMethod_label.Text = "Test Method:";
            // 
            // testMethod_comboBox
            // 
            this.testMethod_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.testMethod_comboBox.FormattingEnabled = true;
            this.testMethod_comboBox.Location = new System.Drawing.Point(296, 187);
            this.testMethod_comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.testMethod_comboBox.Name = "testMethod_comboBox";
            this.testMethod_comboBox.Size = new System.Drawing.Size(197, 24);
            this.testMethod_comboBox.TabIndex = 4;
            this.testMethod_comboBox.SelectedIndexChanged += new System.EventHandler(this.testMethod_comboBox_SelectedIndexChanged);
            // 
            // wakeMethod_comboBox
            // 
            this.wakeMethod_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wakeMethod_comboBox.FormattingEnabled = true;
            this.wakeMethod_comboBox.Location = new System.Drawing.Point(554, 187);
            this.wakeMethod_comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.wakeMethod_comboBox.Name = "wakeMethod_comboBox";
            this.wakeMethod_comboBox.Size = new System.Drawing.Size(197, 24);
            this.wakeMethod_comboBox.TabIndex = 6;
            // 
            // wakeMethod_label
            // 
            this.wakeMethod_label.AutoSize = true;
            this.wakeMethod_label.Location = new System.Drawing.Point(551, 166);
            this.wakeMethod_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.wakeMethod_label.Name = "wakeMethod_label";
            this.wakeMethod_label.Size = new System.Drawing.Size(99, 17);
            this.wakeMethod_label.TabIndex = 7;
            this.wakeMethod_label.Text = "Wake Method:";
            // 
            // EdtInterventionConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wakeMethod_label);
            this.Controls.Add(this.wakeMethod_comboBox);
            this.Controls.Add(this.testMethod_label);
            this.Controls.Add(this.testMethod_comboBox);
            this.Controls.Add(this.edtTestType_label);
            this.Controls.Add(this.edtTestType_comboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AlertTextBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EdtInterventionConfigurationControl";
            this.Size = new System.Drawing.Size(812, 316);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AlertTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox edtTestType_comboBox;
        private System.Windows.Forms.Label edtTestType_label;
        private System.Windows.Forms.Label testMethod_label;
        private System.Windows.Forms.ComboBox testMethod_comboBox;
        private System.Windows.Forms.ComboBox wakeMethod_comboBox;
        private System.Windows.Forms.Label wakeMethod_label;
    }
}
