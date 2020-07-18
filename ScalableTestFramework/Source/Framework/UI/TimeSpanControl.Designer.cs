namespace HP.ScalableTest.Framework.UI
{
    partial class TimeSpanControl
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
            this.timeSpan_DateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // timeSpan_DateTimePicker
            // 
            this.timeSpan_DateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSpan_DateTimePicker.CustomFormat = "H\'h\' mm\'m\' ss\'s\'";
            this.timeSpan_DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeSpan_DateTimePicker.Location = new System.Drawing.Point(0, 0);
            this.timeSpan_DateTimePicker.MaxDate = new System.DateTime(2000, 1, 1, 23, 59, 59, 0);
            this.timeSpan_DateTimePicker.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.timeSpan_DateTimePicker.Name = "timeSpan_DateTimePicker";
            this.timeSpan_DateTimePicker.ShowUpDown = true;
            this.timeSpan_DateTimePicker.Size = new System.Drawing.Size(95, 23);
            this.timeSpan_DateTimePicker.TabIndex = 0;
            this.timeSpan_DateTimePicker.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.timeSpan_DateTimePicker.ValueChanged += new System.EventHandler(this.timeSpan_DateTimePicker_ValueChanged);
            // 
            // TimeSpanControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeSpan_DateTimePicker);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TimeSpanControl";
            this.Size = new System.Drawing.Size(95, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker timeSpan_DateTimePicker;
    }
}
