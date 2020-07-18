namespace HP.ScalableTest.Framework.UI
{
    partial class LockTimeoutControl
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
            this.acquireTimeout_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.holdTimeout_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // acquireTimeout_TimeSpanControl
            // 
            this.acquireTimeout_TimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.acquireTimeout_TimeSpanControl.Location = new System.Drawing.Point(130, 0);
            this.acquireTimeout_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.acquireTimeout_TimeSpanControl.Name = "acquireTimeout_TimeSpanControl";
            this.acquireTimeout_TimeSpanControl.Size = new System.Drawing.Size(111, 27);
            this.acquireTimeout_TimeSpanControl.TabIndex = 1;
            // 
            // holdTimeout_TimeSpanControl
            // 
            this.holdTimeout_TimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.holdTimeout_TimeSpanControl.Location = new System.Drawing.Point(130, 27);
            this.holdTimeout_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.holdTimeout_TimeSpanControl.Name = "holdTimeout_TimeSpanControl";
            this.holdTimeout_TimeSpanControl.Size = new System.Drawing.Size(111, 27);
            this.holdTimeout_TimeSpanControl.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Acquire Lock Timeout";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Hold Lock Timeout";
            // 
            // LockTimeoutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.holdTimeout_TimeSpanControl);
            this.Controls.Add(this.acquireTimeout_TimeSpanControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LockTimeoutControl";
            this.Size = new System.Drawing.Size(241, 53);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSpanControl acquireTimeout_TimeSpanControl;
        private TimeSpanControl holdTimeout_TimeSpanControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
