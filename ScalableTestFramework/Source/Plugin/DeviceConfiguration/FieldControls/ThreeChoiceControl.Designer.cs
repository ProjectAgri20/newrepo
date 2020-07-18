namespace HP.ScalableTest.Plugin.DeviceConfiguration.FieldControls
{
    partial class ThreeChoiceControl
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
            this.field1_TextBox = new System.Windows.Forms.TextBox();
            this.field3_TextBox = new System.Windows.Forms.MaskedTextBox();
            this.field2_TextBox = new System.Windows.Forms.TextBox();
            this.generic_GroupBox = new System.Windows.Forms.GroupBox();
            this.onOff_CheckBox = new System.Windows.Forms.CheckBox();
            this.generic_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // field1_TextBox
            // 
            this.field1_TextBox.Location = new System.Drawing.Point(6, 19);
            this.field1_TextBox.Name = "field1_TextBox";
            this.field1_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field1_TextBox.TabIndex = 0;
            // 
            // field3_TextBox
            // 
            this.field3_TextBox.Location = new System.Drawing.Point(6, 71);
            this.field3_TextBox.Name = "field3_TextBox";
            this.field3_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field3_TextBox.TabIndex = 2;
            // 
            // field2_TextBox
            // 
            this.field2_TextBox.Location = new System.Drawing.Point(6, 45);
            this.field2_TextBox.Name = "field2_TextBox";
            this.field2_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field2_TextBox.TabIndex = 1;
            // 
            // generic_GroupBox
            // 
            this.generic_GroupBox.Controls.Add(this.field1_TextBox);
            this.generic_GroupBox.Controls.Add(this.field3_TextBox);
            this.generic_GroupBox.Controls.Add(this.field2_TextBox);
            this.generic_GroupBox.Location = new System.Drawing.Point(3, 3);
            this.generic_GroupBox.Name = "generic_GroupBox";
            this.generic_GroupBox.Size = new System.Drawing.Size(240, 102);
            this.generic_GroupBox.TabIndex = 3;
            this.generic_GroupBox.TabStop = false;
            this.generic_GroupBox.Text = "Grouping";
            // 
            // onOff_CheckBox
            // 
            this.onOff_CheckBox.AutoSize = true;
            this.onOff_CheckBox.Location = new System.Drawing.Point(272, 6);
            this.onOff_CheckBox.Name = "onOff_CheckBox";
            this.onOff_CheckBox.Size = new System.Drawing.Size(72, 17);
            this.onOff_CheckBox.TabIndex = 4;
            this.onOff_CheckBox.Text = "Set Fields";
            this.onOff_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ThreeChoiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.onOff_CheckBox);
            this.Controls.Add(this.generic_GroupBox);
            this.Name = "ThreeChoiceControl";
            this.Size = new System.Drawing.Size(347, 112);
            this.generic_GroupBox.ResumeLayout(false);
            this.generic_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox field1_TextBox;
        public System.Windows.Forms.MaskedTextBox field3_TextBox;
        public System.Windows.Forms.TextBox field2_TextBox;
        private System.Windows.Forms.GroupBox generic_GroupBox;
        public System.Windows.Forms.CheckBox onOff_CheckBox;
    }
}
