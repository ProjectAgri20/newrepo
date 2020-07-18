namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    partial class ChoiceTimePicker
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
            this.dateTime = new System.Windows.Forms.DateTimePicker();
            this.onOff_CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dateTime
            // 
            this.dateTime.Enabled = false;
            this.dateTime.Location = new System.Drawing.Point(4, 4);
            this.dateTime.Name = "dateTime";
            this.dateTime.Size = new System.Drawing.Size(200, 20);
            this.dateTime.TabIndex = 0;
            // 
            // onOff_CheckBox
            // 
            this.onOff_CheckBox.AutoSize = true;
            this.onOff_CheckBox.Location = new System.Drawing.Point(210, 7);
            this.onOff_CheckBox.Name = "onOff_CheckBox";
            this.onOff_CheckBox.Size = new System.Drawing.Size(68, 17);
            this.onOff_CheckBox.TabIndex = 3;
            this.onOff_CheckBox.Text = "Get Field";
            this.onOff_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ChoiceTimePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.onOff_CheckBox);
            this.Controls.Add(this.dateTime);
            this.Name = "ChoiceTimePicker";
            this.Size = new System.Drawing.Size(283, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DateTimePicker dateTime;
        public System.Windows.Forms.CheckBox onOff_CheckBox;
    }
}
