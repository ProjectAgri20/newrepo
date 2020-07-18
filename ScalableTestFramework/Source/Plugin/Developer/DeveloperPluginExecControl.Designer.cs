namespace HP.ScalableTest.Plugin.Developer
{
    partial class DeveloperPluginExecControl
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
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.sessionIdLabel = new System.Windows.Forms.Label();
            this.sessionIdTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Location = new System.Drawing.Point(0, 38);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTextBox.Size = new System.Drawing.Size(456, 249);
            this.outputTextBox.TabIndex = 0;
            // 
            // sessionIdLabel
            // 
            this.sessionIdLabel.AutoSize = true;
            this.sessionIdLabel.Location = new System.Drawing.Point(3, 9);
            this.sessionIdLabel.Name = "sessionIdLabel";
            this.sessionIdLabel.Size = new System.Drawing.Size(60, 15);
            this.sessionIdLabel.TabIndex = 1;
            this.sessionIdLabel.Text = "Session ID";
            // 
            // sessionIdTextBox
            // 
            this.sessionIdTextBox.Location = new System.Drawing.Point(69, 6);
            this.sessionIdTextBox.Name = "sessionIdTextBox";
            this.sessionIdTextBox.ReadOnly = true;
            this.sessionIdTextBox.Size = new System.Drawing.Size(104, 23);
            this.sessionIdTextBox.TabIndex = 2;
            // 
            // DeveloperPluginExecControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sessionIdTextBox);
            this.Controls.Add(this.sessionIdLabel);
            this.Controls.Add(this.outputTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeveloperPluginExecControl";
            this.Size = new System.Drawing.Size(456, 287);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Label sessionIdLabel;
        private System.Windows.Forms.TextBox sessionIdTextBox;
    }
}
