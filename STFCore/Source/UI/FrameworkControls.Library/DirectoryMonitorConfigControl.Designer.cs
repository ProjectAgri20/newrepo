namespace HP.ScalableTest.UI
{
    partial class DirectoryMonitorConfigControl
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
            this.label_Destination = new System.Windows.Forms.Label();
            this.textBox_Destination = new System.Windows.Forms.TextBox();
            this.textBox_DataLogHost = new System.Windows.Forms.TextBox();
            this.label_DataLogHost = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_Destination
            // 
            this.label_Destination.AutoSize = true;
            this.label_Destination.Location = new System.Drawing.Point(4, 5);
            this.label_Destination.Name = "label_Destination";
            this.label_Destination.Size = new System.Drawing.Size(95, 13);
            this.label_Destination.TabIndex = 20;
            this.label_Destination.Text = "Output Destination";
            // 
            // textBox_Destination
            // 
            this.textBox_Destination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Destination.Location = new System.Drawing.Point(4, 21);
            this.textBox_Destination.Name = "textBox_Destination";
            this.textBox_Destination.Size = new System.Drawing.Size(393, 20);
            this.textBox_Destination.TabIndex = 19;
            this.textBox_Destination.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_Destination_Validating);
            // 
            // textBox_DataLogHost
            // 
            this.textBox_DataLogHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_DataLogHost.Location = new System.Drawing.Point(4, 64);
            this.textBox_DataLogHost.Name = "textBox_DataLogHost";
            this.textBox_DataLogHost.Size = new System.Drawing.Size(393, 20);
            this.textBox_DataLogHost.TabIndex = 22;
            this.textBox_DataLogHost.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox_DataLogHost_Validating);
            // 
            // label_DataLogHost
            // 
            this.label_DataLogHost.AutoSize = true;
            this.label_DataLogHost.Location = new System.Drawing.Point(4, 48);
            this.label_DataLogHost.Name = "label_DataLogHost";
            this.label_DataLogHost.Size = new System.Drawing.Size(120, 13);
            this.label_DataLogHost.TabIndex = 21;
            this.label_DataLogHost.Text = "Log Service Host Name";
            // 
            // DirectoryMonitorConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_DataLogHost);
            this.Controls.Add(this.label_DataLogHost);
            this.Controls.Add(this.label_Destination);
            this.Controls.Add(this.textBox_Destination);
            this.Name = "DirectoryMonitorConfigControl";
            this.Size = new System.Drawing.Size(400, 90);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Destination;
        private System.Windows.Forms.TextBox textBox_Destination;
        private System.Windows.Forms.TextBox textBox_DataLogHost;
        private System.Windows.Forms.Label label_DataLogHost;
    }
}
