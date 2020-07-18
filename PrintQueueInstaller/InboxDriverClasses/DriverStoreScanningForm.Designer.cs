namespace HP.ScalableTest.Print.Utility
{
    partial class DriverStoreScanningForm
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
            this.loading_ProgressBar = new System.Windows.Forms.ProgressBar();
            this.scanning_Label = new System.Windows.Forms.Label();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.driver_Label = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // loading_ProgressBar
            // 
            this.loading_ProgressBar.Location = new System.Drawing.Point(12, 27);
            this.loading_ProgressBar.Name = "loading_ProgressBar";
            this.loading_ProgressBar.Size = new System.Drawing.Size(609, 23);
            this.loading_ProgressBar.TabIndex = 0;
            // 
            // scanning_Label
            // 
            this.scanning_Label.AutoSize = true;
            this.scanning_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanning_Label.Location = new System.Drawing.Point(233, 9);
            this.scanning_Label.Name = "scanning_Label";
            this.scanning_Label.Size = new System.Drawing.Size(171, 13);
            this.scanning_Label.TabIndex = 1;
            this.scanning_Label.Text = "Scanning for in-box drivers...";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Location = new System.Drawing.Point(280, 80);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 2;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // driver_Label
            // 
            this.driver_Label.BackColor = System.Drawing.SystemColors.Info;
            this.driver_Label.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.driver_Label.Location = new System.Drawing.Point(12, 56);
            this.driver_Label.Name = "driver_Label";
            this.driver_Label.Size = new System.Drawing.Size(609, 13);
            this.driver_Label.TabIndex = 4;
            this.driver_Label.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DriverStoreScanningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(633, 115);
            this.Controls.Add(this.driver_Label);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.scanning_Label);
            this.Controls.Add(this.loading_ProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DriverStoreScanningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Driver Store Scanning Form";
            this.Load += new System.EventHandler(this.DriverStoreScanningForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar loading_ProgressBar;
        private System.Windows.Forms.Label scanning_Label;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox driver_Label;
    }
}