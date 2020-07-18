namespace HP.ScalableTest.Plugin.ServiceStartStop
{
    partial class ServiceStartStopExecutionControl
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
            this.status_TextBox = new System.Windows.Forms.RichTextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ActivityStatus = new System.Windows.Forms.Label();
            this.Server = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_TextBox
            // 
            this.status_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_TextBox.Location = new System.Drawing.Point(6, 118);
            this.status_TextBox.Name = "status_TextBox";
            this.status_TextBox.ReadOnly = true;
            this.status_TextBox.Size = new System.Drawing.Size(428, 213);
            this.status_TextBox.TabIndex = 1;
            this.status_TextBox.Text = "";
            this.status_TextBox.WordWrap = false;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(8, 17);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(42, 15);
            this.ServerLabel.TabIndex = 2;
            this.ServerLabel.Text = "Server:";
            // 
            // ActivityStatus
            // 
            this.ActivityStatus.AutoSize = true;
            this.ActivityStatus.Location = new System.Drawing.Point(8, 100);
            this.ActivityStatus.Name = "ActivityStatus";
            this.ActivityStatus.Size = new System.Drawing.Size(82, 15);
            this.ActivityStatus.TabIndex = 3;
            this.ActivityStatus.Text = "Activity Status";
            // 
            // Server
            // 
            this.Server.AutoSize = true;
            this.Server.Location = new System.Drawing.Point(56, 17);
            this.Server.Name = "Server";
            this.Server.Size = new System.Drawing.Size(38, 15);
            this.Server.TabIndex = 4;
            this.Server.Text = "label1";
            // 
            // ServiceStartStopExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Server);
            this.Controls.Add(this.ActivityStatus);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.status_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceStartStopExecutionControl";
            this.Size = new System.Drawing.Size(437, 335);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox status_TextBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label ActivityStatus;
        private System.Windows.Forms.Label Server;
    }
}
