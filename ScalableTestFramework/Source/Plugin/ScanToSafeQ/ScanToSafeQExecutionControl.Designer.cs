namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    partial class ScanToSafeQExecutionControl
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
            this.device_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.device_Label.Location = new System.Drawing.Point(3, 14);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(47, 13);
            this.device_Label.TabIndex = 6;
            this.device_Label.Text = "Device";
            // 
            // ScanToHpcrExecControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.device_Label);
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "ScanToHpcrExecControl";            
            this.Controls.SetChildIndex(this.device_Label, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label device_Label;
    }
}
