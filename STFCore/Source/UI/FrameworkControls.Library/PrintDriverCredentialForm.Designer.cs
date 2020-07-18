namespace HP.ScalableTest.UI.Framework
{
    partial class PrintDriverCredentialForm
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
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.password_TextBox = new System.Windows.Forms.TextBox();
            this.userName_TextBox = new System.Windows.Forms.TextBox();
            this.domain_TextBox = new System.Windows.Forms.TextBox();
            this.shareLocation_TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(428, 209);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(320, 209);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // password_TextBox
            // 
            this.password_TextBox.Location = new System.Drawing.Point(119, 170);
            this.password_TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.password_TextBox.Name = "password_TextBox";
            this.password_TextBox.Size = new System.Drawing.Size(152, 27);
            this.password_TextBox.TabIndex = 1;
            this.password_TextBox.UseSystemPasswordChar = true;
            // 
            // userName_TextBox
            // 
            this.userName_TextBox.Location = new System.Drawing.Point(119, 133);
            this.userName_TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.userName_TextBox.Name = "userName_TextBox";
            this.userName_TextBox.Size = new System.Drawing.Size(152, 27);
            this.userName_TextBox.TabIndex = 10;
            // 
            // domain_TextBox
            // 
            this.domain_TextBox.Location = new System.Drawing.Point(119, 96);
            this.domain_TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.domain_TextBox.Name = "domain_TextBox";
            this.domain_TextBox.Size = new System.Drawing.Size(299, 27);
            this.domain_TextBox.TabIndex = 8;
            // 
            // shareLocation_TextBox
            // 
            this.shareLocation_TextBox.Location = new System.Drawing.Point(119, 59);
            this.shareLocation_TextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.shareLocation_TextBox.Name = "shareLocation_TextBox";
            this.shareLocation_TextBox.Size = new System.Drawing.Size(407, 27);
            this.shareLocation_TextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 173);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Password:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 136);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "User Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 99);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Domain:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Remote Share:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(299, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Please provide credentials for remote share.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PrintDriverCredentialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(541, 255);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.shareLocation_TextBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.domain_TextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userName_TextBox);
            this.Controls.Add(this.password_TextBox);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PrintDriverCredentialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Credentials For Network Share";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox userName_TextBox;
        private System.Windows.Forms.TextBox password_TextBox;
        private System.Windows.Forms.TextBox shareLocation_TextBox;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox domain_TextBox;
        private System.Windows.Forms.Label label5;
    }
}