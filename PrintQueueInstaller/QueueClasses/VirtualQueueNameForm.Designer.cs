namespace HP.ScalableTest.Print.Utility
{
    partial class VirtualQueueNameForm
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
            this.virtualQueueDataControl = new HP.ScalableTest.Print.Utility.VirtualQueueNameUserControl();
            this.SuspendLayout();
            // 
            // virtualQueueDataControl
            // 
            this.virtualQueueDataControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.virtualQueueDataControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualQueueDataControl.Location = new System.Drawing.Point(0, 0);
            this.virtualQueueDataControl.Margin = new System.Windows.Forms.Padding(4);
            this.virtualQueueDataControl.Name = "virtualQueueDataControl";
            this.virtualQueueDataControl.Size = new System.Drawing.Size(359, 321);
            this.virtualQueueDataControl.TabIndex = 0;
            this.virtualQueueDataControl.TabStop = false;
            // 
            // VirtualQueueNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 321);
            this.Controls.Add(this.virtualQueueDataControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "VirtualQueueNameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Virtual Printer Queue Configuration";
            this.Load += new System.EventHandler(this.VirtualQueueDataForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualQueueNameUserControl virtualQueueDataControl;

    }
}