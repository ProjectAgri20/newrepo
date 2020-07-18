namespace HP.ScalableTest.Development.UI
{
    partial class DataLoggerMockForm
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
            this.dataLoggerOutputTabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // dataLoggerOutputTabControl
            // 
            this.dataLoggerOutputTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLoggerOutputTabControl.Location = new System.Drawing.Point(12, 12);
            this.dataLoggerOutputTabControl.Multiline = true;
            this.dataLoggerOutputTabControl.Name = "dataLoggerOutputTabControl";
            this.dataLoggerOutputTabControl.SelectedIndex = 0;
            this.dataLoggerOutputTabControl.Size = new System.Drawing.Size(626, 369);
            this.dataLoggerOutputTabControl.TabIndex = 0;
            // 
            // DataLoggerMockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 393);
            this.Controls.Add(this.dataLoggerOutputTabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataLoggerMockForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Data Logger Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataLoggerMockForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl dataLoggerOutputTabControl;
    }
}