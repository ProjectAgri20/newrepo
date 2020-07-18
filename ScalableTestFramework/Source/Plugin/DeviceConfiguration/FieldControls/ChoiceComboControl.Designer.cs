namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    partial class ChoiceComboControl
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
            this.choice_Combo = new System.Windows.Forms.ComboBox();
            this.onOff_CheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // choice_Combo
            // 
            this.choice_Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.choice_Combo.Enabled = false;
            this.choice_Combo.FormattingEnabled = true;
            this.choice_Combo.Location = new System.Drawing.Point(3, 3);
            this.choice_Combo.Name = "choice_Combo";
            this.choice_Combo.Size = new System.Drawing.Size(260, 21);
            this.choice_Combo.TabIndex = 0;
            // 
            // onOff_CheckBox
            // 
            this.onOff_CheckBox.AutoSize = true;
            this.onOff_CheckBox.Location = new System.Drawing.Point(268, 5);
            this.onOff_CheckBox.Name = "onOff_CheckBox";
            this.onOff_CheckBox.Size = new System.Drawing.Size(67, 17);
            this.onOff_CheckBox.TabIndex = 2;
            this.onOff_CheckBox.Text = "Set Field";
            this.onOff_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ChoiceComboControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.onOff_CheckBox);
            this.Controls.Add(this.choice_Combo);
            this.Name = "ChoiceComboControl";
            this.Size = new System.Drawing.Size(335, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox choice_Combo;
        public System.Windows.Forms.CheckBox onOff_CheckBox;
    }
}
