namespace HP.ScalableTest.Plugin.RegusKiosk.Options
{
    partial class RegusKioskPrintOptionsForm
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
            this.copies_Label = new System.Windows.Forms.Label();
            this.colormode_Label = new System.Windows.Forms.Label();
            this.duplex_Label = new System.Windows.Forms.Label();
            this.papersource_Label = new System.Windows.Forms.Label();
            this.autofit_Label = new System.Windows.Forms.Label();
            this.copies_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.colormode_ComboBox = new System.Windows.Forms.ComboBox();
            this.duplex_ComboBox = new System.Windows.Forms.ComboBox();
            this.papersource_ComboBox = new System.Windows.Forms.ComboBox();
            this.autofit_CheckBox = new System.Windows.Forms.CheckBox();
            this.ok_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.copies_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // copies_Label
            // 
            this.copies_Label.AutoSize = true;
            this.copies_Label.Location = new System.Drawing.Point(20, 17);
            this.copies_Label.Name = "copies_Label";
            this.copies_Label.Size = new System.Drawing.Size(51, 17);
            this.copies_Label.TabIndex = 0;
            this.copies_Label.Text = "Copies";
            // 
            // colormode_Label
            // 
            this.colormode_Label.AutoSize = true;
            this.colormode_Label.Location = new System.Drawing.Point(20, 47);
            this.colormode_Label.Name = "colormode_Label";
            this.colormode_Label.Size = new System.Drawing.Size(80, 17);
            this.colormode_Label.TabIndex = 1;
            this.colormode_Label.Text = "Color Mode";
            // 
            // duplex_Label
            // 
            this.duplex_Label.AutoSize = true;
            this.duplex_Label.Location = new System.Drawing.Point(20, 77);
            this.duplex_Label.Name = "duplex_Label";
            this.duplex_Label.Size = new System.Drawing.Size(51, 17);
            this.duplex_Label.TabIndex = 2;
            this.duplex_Label.Text = "Duplex";
            // 
            // papersource_Label
            // 
            this.papersource_Label.AutoSize = true;
            this.papersource_Label.Location = new System.Drawing.Point(20, 107);
            this.papersource_Label.Name = "papersource_Label";
            this.papersource_Label.Size = new System.Drawing.Size(95, 17);
            this.papersource_Label.TabIndex = 3;
            this.papersource_Label.Text = "Paper Source";
            // 
            // autofit_Label
            // 
            this.autofit_Label.AutoSize = true;
            this.autofit_Label.Location = new System.Drawing.Point(20, 137);
            this.autofit_Label.Name = "autofit_Label";
            this.autofit_Label.Size = new System.Drawing.Size(56, 17);
            this.autofit_Label.TabIndex = 4;
            this.autofit_Label.Text = "Auto Fit";
            // 
            // copies_NumericUpDown
            // 
            this.copies_NumericUpDown.Location = new System.Drawing.Point(165, 14);
            this.copies_NumericUpDown.Name = "copies_NumericUpDown";
            this.copies_NumericUpDown.Size = new System.Drawing.Size(144, 22);
            this.copies_NumericUpDown.TabIndex = 5;
            // 
            // colormode_ComboBox
            // 
            this.colormode_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colormode_ComboBox.FormattingEnabled = true;
            this.colormode_ComboBox.Location = new System.Drawing.Point(165, 44);
            this.colormode_ComboBox.Name = "colormode_ComboBox";
            this.colormode_ComboBox.Size = new System.Drawing.Size(145, 24);
            this.colormode_ComboBox.TabIndex = 6;
            // 
            // duplex_ComboBox
            // 
            this.duplex_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.duplex_ComboBox.FormattingEnabled = true;
            this.duplex_ComboBox.Location = new System.Drawing.Point(165, 74);
            this.duplex_ComboBox.Name = "duplex_ComboBox";
            this.duplex_ComboBox.Size = new System.Drawing.Size(145, 24);
            this.duplex_ComboBox.TabIndex = 7;
            // 
            // papersource_ComboBox
            // 
            this.papersource_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.papersource_ComboBox.FormattingEnabled = true;
            this.papersource_ComboBox.Location = new System.Drawing.Point(165, 104);
            this.papersource_ComboBox.Name = "papersource_ComboBox";
            this.papersource_ComboBox.Size = new System.Drawing.Size(145, 24);
            this.papersource_ComboBox.TabIndex = 8;
            // 
            // autofit_CheckBox
            // 
            this.autofit_CheckBox.AutoSize = true;
            this.autofit_CheckBox.Checked = true;
            this.autofit_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autofit_CheckBox.Location = new System.Drawing.Point(165, 136);
            this.autofit_CheckBox.Name = "autofit_CheckBox";
            this.autofit_CheckBox.Size = new System.Drawing.Size(69, 21);
            this.autofit_CheckBox.TabIndex = 9;
            this.autofit_CheckBox.Text = "Check";
            this.autofit_CheckBox.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(110, 173);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 40);
            this.ok_Button.TabIndex = 10;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // RegusKioskPrintOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 225);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.autofit_CheckBox);
            this.Controls.Add(this.papersource_ComboBox);
            this.Controls.Add(this.duplex_ComboBox);
            this.Controls.Add(this.colormode_ComboBox);
            this.Controls.Add(this.copies_NumericUpDown);
            this.Controls.Add(this.autofit_Label);
            this.Controls.Add(this.papersource_Label);
            this.Controls.Add(this.duplex_Label);
            this.Controls.Add(this.colormode_Label);
            this.Controls.Add(this.copies_Label);
            this.Name = "KioskPrintOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Options";
            ((System.ComponentModel.ISupportInitialize)(this.copies_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label copies_Label;
        private System.Windows.Forms.Label colormode_Label;
        private System.Windows.Forms.Label duplex_Label;
        private System.Windows.Forms.Label papersource_Label;
        private System.Windows.Forms.Label autofit_Label;
        private System.Windows.Forms.NumericUpDown copies_NumericUpDown;
        private System.Windows.Forms.ComboBox colormode_ComboBox;
        private System.Windows.Forms.ComboBox duplex_ComboBox;
        private System.Windows.Forms.ComboBox papersource_ComboBox;
        private System.Windows.Forms.CheckBox autofit_CheckBox;
        private System.Windows.Forms.Button ok_Button;
    }
}
