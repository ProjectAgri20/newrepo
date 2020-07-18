namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    partial class ChoiceNumericControl
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
            this.choice_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.onOff_CheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.choice_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // choice_numericUpDown
            // 
            this.choice_numericUpDown.AutoSize = true;
            this.choice_numericUpDown.Location = new System.Drawing.Point(3, 3);
            this.choice_numericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.choice_numericUpDown.Name = "choice_numericUpDown";
            this.choice_numericUpDown.Size = new System.Drawing.Size(260, 20);
            this.choice_numericUpDown.TabIndex = 0;
            // 
            // onOff_CheckBox
            // 
            this.onOff_CheckBox.AutoSize = true;
            this.onOff_CheckBox.Location = new System.Drawing.Point(268, 4);
            this.onOff_CheckBox.Name = "onOff_CheckBox";
            this.onOff_CheckBox.Size = new System.Drawing.Size(68, 17);
            this.onOff_CheckBox.TabIndex = 3;
            this.onOff_CheckBox.Text = "Get Field";
            this.onOff_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ChoiceNumericControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.onOff_CheckBox);
            this.Controls.Add(this.choice_numericUpDown);
            this.Name = "ChoiceNumericControl";
            this.Size = new System.Drawing.Size(335, 27);
            ((System.ComponentModel.ISupportInitialize)(this.choice_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.CheckBox onOff_CheckBox;
        public System.Windows.Forms.NumericUpDown choice_numericUpDown;
    }
}
