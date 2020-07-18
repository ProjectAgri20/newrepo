namespace HP.ScalableTest.UI
{
    partial class TimeDelayControl
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.maxDelay_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.minDelay_TimeSpanControl = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.max_Label = new System.Windows.Forms.Label();
            this.randomDelay_CheckBox = new System.Windows.Forms.CheckBox();
            this.min_Label = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.maxDelay_TimeSpanControl);
            this.groupBox.Controls.Add(this.minDelay_TimeSpanControl);
            this.groupBox.Controls.Add(this.max_Label);
            this.groupBox.Controls.Add(this.randomDelay_CheckBox);
            this.groupBox.Controls.Add(this.min_Label);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(236, 127);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Delay";
            // 
            // maxDelay_TimeSpanControl
            // 
            this.maxDelay_TimeSpanControl.AutoSize = true;
            this.maxDelay_TimeSpanControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.maxDelay_TimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxDelay_TimeSpanControl.Location = new System.Drawing.Point(69, 87);
            this.maxDelay_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.maxDelay_TimeSpanControl.Name = "maxDelay_TimeSpanControl";
            this.maxDelay_TimeSpanControl.Size = new System.Drawing.Size(157, 26);
            this.maxDelay_TimeSpanControl.TabIndex = 6;
            this.maxDelay_TimeSpanControl.ValueChanged += new System.EventHandler(this.maxDelay_TimeSpanControl_ValueChanged);
            // 
            // minDelay_TimeSpanControl
            // 
            this.minDelay_TimeSpanControl.AutoSize = true;
            this.minDelay_TimeSpanControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.minDelay_TimeSpanControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minDelay_TimeSpanControl.Location = new System.Drawing.Point(69, 56);
            this.minDelay_TimeSpanControl.Margin = new System.Windows.Forms.Padding(0);
            this.minDelay_TimeSpanControl.Name = "minDelay_TimeSpanControl";
            this.minDelay_TimeSpanControl.Size = new System.Drawing.Size(157, 26);
            this.minDelay_TimeSpanControl.TabIndex = 5;
            this.minDelay_TimeSpanControl.ValueChanged += new System.EventHandler(this.minDelay_TimeSpanControl_ValueChanged);
            // 
            // max_Label
            // 
            this.max_Label.Location = new System.Drawing.Point(20, 88);
            this.max_Label.Name = "max_Label";
            this.max_Label.Size = new System.Drawing.Size(46, 25);
            this.max_Label.TabIndex = 8;
            this.max_Label.Text = "Max";
            this.max_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // randomDelay_CheckBox
            // 
            this.randomDelay_CheckBox.AutoSize = true;
            this.randomDelay_CheckBox.Checked = true;
            this.randomDelay_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.randomDelay_CheckBox.Location = new System.Drawing.Point(10, 25);
            this.randomDelay_CheckBox.Name = "randomDelay_CheckBox";
            this.randomDelay_CheckBox.Size = new System.Drawing.Size(85, 19);
            this.randomDelay_CheckBox.TabIndex = 4;
            this.randomDelay_CheckBox.Text = "Randomize";
            this.randomDelay_CheckBox.UseVisualStyleBackColor = true;
            this.randomDelay_CheckBox.CheckedChanged += new System.EventHandler(this.RandomDelayCheckBox_CheckedChanged);
            // 
            // min_Label
            // 
            this.min_Label.Location = new System.Drawing.Point(6, 57);
            this.min_Label.Name = "min_Label";
            this.min_Label.Size = new System.Drawing.Size(60, 25);
            this.min_Label.TabIndex = 7;
            this.min_Label.Text = "Min";
            this.min_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TimeDelayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TimeDelayControl";
            this.Size = new System.Drawing.Size(236, 127);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.TimeDelayControl_Validating);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label min_Label;
        private ScalableTest.Framework.UI.TimeSpanControl maxDelay_TimeSpanControl;
        private ScalableTest.Framework.UI.TimeSpanControl minDelay_TimeSpanControl;
        private System.Windows.Forms.CheckBox randomDelay_CheckBox;
        private System.Windows.Forms.Label max_Label;
    }
}
