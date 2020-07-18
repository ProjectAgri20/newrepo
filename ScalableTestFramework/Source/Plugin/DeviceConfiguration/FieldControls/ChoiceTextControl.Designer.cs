namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    partial class ChoiceTextControl
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
            this.onOff_CheckBox = new System.Windows.Forms.CheckBox();
            this.text_Box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // onOff_CheckBox
            // 
            this.onOff_CheckBox.AutoSize = true;
            this.onOff_CheckBox.Location = new System.Drawing.Point(268, 5);
            this.onOff_CheckBox.Name = "onOff_CheckBox";
            this.onOff_CheckBox.Size = new System.Drawing.Size(67, 17);
            this.onOff_CheckBox.TabIndex = 1;
            this.onOff_CheckBox.Text = "Set Field";
            this.onOff_CheckBox.UseVisualStyleBackColor = true;
            // 
            // text_Box
            // 
            this.text_Box.Enabled = false;
            this.text_Box.Location = new System.Drawing.Point(3, 3);
            this.text_Box.Name = "text_Box";
            this.text_Box.Size = new System.Drawing.Size(260, 20);
            this.text_Box.TabIndex = 2;
            // 
            // ChoiceTextControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.text_Box);
            this.Controls.Add(this.onOff_CheckBox);
            this.Name = "ChoiceTextControl";
            this.Size = new System.Drawing.Size(335, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.CheckBox onOff_CheckBox;
        public System.Windows.Forms.TextBox text_Box;
    }
}
