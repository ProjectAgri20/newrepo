namespace HP.ScalableTest.Development.UI
{
    partial class PluginConfigurationDataDisplayForm
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
            this.xmlDisplayTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // xmlDisplayTextBox
            // 
            this.xmlDisplayTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlDisplayTextBox.Location = new System.Drawing.Point(0, 0);
            this.xmlDisplayTextBox.Multiline = true;
            this.xmlDisplayTextBox.Name = "xmlDisplayTextBox";
            this.xmlDisplayTextBox.ReadOnly = true;
            this.xmlDisplayTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.xmlDisplayTextBox.Size = new System.Drawing.Size(556, 465);
            this.xmlDisplayTextBox.TabIndex = 0;
            this.xmlDisplayTextBox.WordWrap = false;
            // 
            // PluginConfigurationDataDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 465);
            this.Controls.Add(this.xmlDisplayTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginConfigurationDataDisplayForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Metadata";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xmlDisplayTextBox;
    }
}