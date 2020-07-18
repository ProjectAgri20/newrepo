namespace HP.ScalableTest.LabConsole
{
    partial class MonitorConfigDetailForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.label_MonitorType = new System.Windows.Forms.Label();
            this.monitorType_ComboBox = new System.Windows.Forms.ComboBox();
            this.label_Server = new System.Windows.Forms.Label();
            this.serverComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(335, 261);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 10;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(254, 261);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 9;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // label_MonitorType
            // 
            this.label_MonitorType.AutoSize = true;
            this.label_MonitorType.Location = new System.Drawing.Point(12, 60);
            this.label_MonitorType.Name = "label_MonitorType";
            this.label_MonitorType.Size = new System.Drawing.Size(69, 13);
            this.label_MonitorType.TabIndex = 11;
            this.label_MonitorType.Text = "Monitor Type";
            this.toolTip.SetToolTip(this.label_MonitorType, "Warning!  Changing the Monitor Type could result in the loss of data.");
            // 
            // monitorType_ComboBox
            // 
            this.monitorType_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monitorType_ComboBox.DisplayMember = "Value";
            this.monitorType_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monitorType_ComboBox.FormattingEnabled = true;
            this.monitorType_ComboBox.Location = new System.Drawing.Point(87, 57);
            this.monitorType_ComboBox.Name = "monitorType_ComboBox";
            this.monitorType_ComboBox.Size = new System.Drawing.Size(323, 21);
            this.monitorType_ComboBox.TabIndex = 12;
            this.monitorType_ComboBox.ValueMember = "Key";
            // 
            // label_Server
            // 
            this.label_Server.AutoSize = true;
            this.label_Server.Location = new System.Drawing.Point(12, 12);
            this.label_Server.Name = "label_Server";
            this.label_Server.Size = new System.Drawing.Size(136, 13);
            this.label_Server.TabIndex = 13;
            this.label_Server.Text = "Output Monitor Server Host";
            this.toolTip.SetToolTip(this.label_Server, "The host server where the monitoring service is running.");
            // 
            // serverComboBox
            // 
            this.serverComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serverComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBox.Location = new System.Drawing.Point(12, 28);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(398, 23);
            this.serverComboBox.TabIndex = 19;
            // 
            // MonitorConfigDetailForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(417, 287);
            this.Controls.Add(this.serverComboBox);
            this.Controls.Add(this.monitorType_ComboBox);
            this.Controls.Add(this.label_MonitorType);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.label_Server);
            this.Controls.Add(this.ok_Button);
            this.Name = "MonitorConfigDetailForm";
            this.Text = "Output Monitor Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label label_MonitorType;
        private System.Windows.Forms.ComboBox monitorType_ComboBox;
        private System.Windows.Forms.Label label_Server;
        private Framework.UI.ServerComboBox serverComboBox;
        private System.Windows.Forms.ToolTip toolTip;
    }
}