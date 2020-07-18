namespace HP.ScalableTest.UI
{
    partial class DatabaseMonitorConfigControl
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
            this.textBox_DbInstanceName = new System.Windows.Forms.TextBox();
            this.label_DbInstanceName = new System.Windows.Forms.Label();
            this.label_DbHostName = new System.Windows.Forms.Label();
            this.textBox_DbHostName = new System.Windows.Forms.TextBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_DbInstanceName
            // 
            this.textBox_DbInstanceName.Location = new System.Drawing.Point(3, 60);
            this.textBox_DbInstanceName.Name = "textBox_DbInstanceName";
            this.textBox_DbInstanceName.Size = new System.Drawing.Size(393, 20);
            this.textBox_DbInstanceName.TabIndex = 26;
            this.textBox_DbInstanceName.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_DbInstanceName_Validating);
            // 
            // label_DbInstanceName
            // 
            this.label_DbInstanceName.AutoSize = true;
            this.label_DbInstanceName.Location = new System.Drawing.Point(3, 44);
            this.label_DbInstanceName.Name = "label_DbInstanceName";
            this.label_DbInstanceName.Size = new System.Drawing.Size(128, 13);
            this.label_DbInstanceName.TabIndex = 25;
            this.label_DbInstanceName.Text = "Database Instance Name";
            // 
            // label_DbHostName
            // 
            this.label_DbHostName.AutoSize = true;
            this.label_DbHostName.Location = new System.Drawing.Point(3, 1);
            this.label_DbHostName.Name = "label_DbHostName";
            this.label_DbHostName.Size = new System.Drawing.Size(109, 13);
            this.label_DbHostName.TabIndex = 24;
            this.label_DbHostName.Text = "Database Host Name";
            // 
            // textBox_DbHostName
            // 
            this.textBox_DbHostName.Location = new System.Drawing.Point(3, 17);
            this.textBox_DbHostName.Name = "textBox_DbHostName";
            this.textBox_DbHostName.Size = new System.Drawing.Size(393, 20);
            this.textBox_DbHostName.TabIndex = 23;
            this.textBox_DbHostName.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_DbHostName_Validating);
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(3, 87);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(193, 13);
            this.label_Port.TabIndex = 27;
            this.label_Port.Text = "Database Port (Leave blank for default)";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(3, 103);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(60, 20);
            this.textBox_Port.TabIndex = 28;
            // 
            // DatabaseMonitorConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.textBox_DbInstanceName);
            this.Controls.Add(this.label_DbInstanceName);
            this.Controls.Add(this.label_DbHostName);
            this.Controls.Add(this.textBox_DbHostName);
            this.Name = "DatabaseMonitorConfigControl";
            this.Size = new System.Drawing.Size(400, 130);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_DbInstanceName;
        private System.Windows.Forms.Label label_DbInstanceName;
        private System.Windows.Forms.Label label_DbHostName;
        private System.Windows.Forms.TextBox textBox_DbHostName;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_Port;
    }
}
