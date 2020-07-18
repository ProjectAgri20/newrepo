namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class HpacServerConfigurationExecutionControl
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
            this.hpacServer_Label = new System.Windows.Forms.Label();
            this.hpacServerText_Label = new System.Windows.Forms.Label();
            this.status_Label = new System.Windows.Forms.Label();
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.sessionId_Label = new System.Windows.Forms.Label();
            this.sessionIdText_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hpacServer_Label
            // 
            this.hpacServer_Label.AutoSize = true;
            this.hpacServer_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpacServer_Label.Location = new System.Drawing.Point(125, 24);
            this.hpacServer_Label.Name = "hpacServer_Label";
            this.hpacServer_Label.Size = new System.Drawing.Size(27, 13);
            this.hpacServer_Label.TabIndex = 14;
            this.hpacServer_Label.Text = "N/A";
            // 
            // hpacServerText_Label
            // 
            this.hpacServerText_Label.AutoSize = true;
            this.hpacServerText_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpacServerText_Label.Location = new System.Drawing.Point(34, 24);
            this.hpacServerText_Label.Name = "hpacServerText_Label";
            this.hpacServerText_Label.Size = new System.Drawing.Size(85, 13);
            this.hpacServerText_Label.TabIndex = 13;
            this.hpacServerText_Label.Text = "HPAC Server:";
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(1, 78);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(43, 13);
            this.status_Label.TabIndex = 12;
            this.status_Label.Text = "Status";
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.Location = new System.Drawing.Point(3, 95);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(497, 211);
            this.status_RichTextBox.TabIndex = 9;
            this.status_RichTextBox.Text = "";
            // 
            // sessionId_Label
            // 
            this.sessionId_Label.AutoSize = true;
            this.sessionId_Label.Location = new System.Drawing.Point(125, 4);
            this.sessionId_Label.Name = "sessionId_Label";
            this.sessionId_Label.Size = new System.Drawing.Size(51, 13);
            this.sessionId_Label.TabIndex = 24;
            this.sessionId_Label.Text = "12345AB";
            // 
            // sessionIdText_Label
            // 
            this.sessionIdText_Label.AutoSize = true;
            this.sessionIdText_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionIdText_Label.Location = new System.Drawing.Point(53, 4);
            this.sessionIdText_Label.Name = "sessionIdText_Label";
            this.sessionIdText_Label.Size = new System.Drawing.Size(66, 13);
            this.sessionIdText_Label.TabIndex = 23;
            this.sessionIdText_Label.Text = "SessionId:";
            // 
            // HpacSimulationExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sessionId_Label);
            this.Controls.Add(this.sessionIdText_Label);
            this.Controls.Add(this.hpacServer_Label);
            this.Controls.Add(this.hpacServerText_Label);
            this.Controls.Add(this.status_Label);
            this.Controls.Add(this.status_RichTextBox);
            this.Name = "HpacServerConfigurationExecutionControl";
            this.Size = new System.Drawing.Size(504, 310);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label hpacServer_Label;
        private System.Windows.Forms.Label hpacServerText_Label;
        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label sessionId_Label;
        private System.Windows.Forms.Label sessionIdText_Label;
    }
}
