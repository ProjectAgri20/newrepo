namespace HP.ScalableTest.UI
{
    partial class MonitorConfigControl
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
            this.textBox_MonitorLocation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_Destination
            // 
            this.label_Destination.AutoSize = true;
            this.label_Destination.Location = new System.Drawing.Point(3, 3);
            this.label_Destination.Name = "label_Destination";
            this.label_Destination.Size = new System.Drawing.Size(95, 13);
            this.label_Destination.TabIndex = 18;
            this.label_Destination.Text = "Output Destination";
            // 
            // textBox_MonitorLocation
            // 
            this.textBox_MonitorLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_MonitorLocation.Location = new System.Drawing.Point(3, 19);
            this.textBox_MonitorLocation.Name = "textBox_MonitorLocation";
            this.textBox_MonitorLocation.Size = new System.Drawing.Size(393, 20);
            this.textBox_MonitorLocation.TabIndex = 17;
            this.textBox_MonitorLocation.Validating += new System.ComponentModel.CancelEventHandler(this.destination_TextBox_Validating);
            // 
            // MonitorConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_Destination);
            this.Controls.Add(this.textBox_MonitorLocation);
            this.Name = "MonitorConfigControl";
            this.Size = new System.Drawing.Size(400, 47);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Destination;
        private System.Windows.Forms.TextBox textBox_MonitorLocation;
    }
}
