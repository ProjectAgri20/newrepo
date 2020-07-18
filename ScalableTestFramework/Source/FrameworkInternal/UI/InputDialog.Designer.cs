namespace HP.ScalableTest.Framework.UI
{
    partial class InputDialog
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
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.input_TextBox = new System.Windows.Forms.TextBox();
            this.prompt_Label = new System.Windows.Forms.Label();
            this.input_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(196, 86);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(277, 86);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // input_TextBox
            // 
            this.input_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input_TextBox.Location = new System.Drawing.Point(23, 45);
            this.input_TextBox.Name = "input_TextBox";
            this.input_TextBox.Size = new System.Drawing.Size(329, 23);
            this.input_TextBox.TabIndex = 0;
            // 
            // prompt_Label
            // 
            this.prompt_Label.AutoSize = true;
            this.prompt_Label.Location = new System.Drawing.Point(20, 18);
            this.prompt_Label.Name = "prompt_Label";
            this.prompt_Label.Size = new System.Drawing.Size(77, 15);
            this.prompt_Label.TabIndex = 4;
            this.prompt_Label.Text = "Enter a value:";
            // 
            // input_ComboBox
            // 
            this.input_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.input_ComboBox.FormattingEnabled = true;
            this.input_ComboBox.Location = new System.Drawing.Point(23, 45);
            this.input_ComboBox.Name = "input_ComboBox";
            this.input_ComboBox.Size = new System.Drawing.Size(329, 23);
            this.input_ComboBox.TabIndex = 1;
            this.input_ComboBox.Visible = false;
            // 
            // InputDialog
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(364, 121);
            this.Controls.Add(this.prompt_Label);
            this.Controls.Add(this.input_TextBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.input_ComboBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InputDialog";
            this.Text = "Input Dialog";
            this.Load += new System.EventHandler(this.InputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox input_TextBox;
        private System.Windows.Forms.Label prompt_Label;
        private System.Windows.Forms.ComboBox input_ComboBox;
    }
}