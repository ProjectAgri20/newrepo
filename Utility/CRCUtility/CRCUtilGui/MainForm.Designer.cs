namespace PaperlessUtilityGui
{
    partial class MainForm
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
            this.onButton = new System.Windows.Forms.Button();
            this.offButton = new System.Windows.Forms.Button();
            this.textBox_Address = new System.Windows.Forms.TextBox();
            this.label_Address = new System.Windows.Forms.Label();
            this.label_AdminPwd = new System.Windows.Forms.Label();
            this.textBox_AdminPwd = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // onButton
            // 
            this.onButton.Location = new System.Drawing.Point(30, 76);
            this.onButton.Name = "onButton";
            this.onButton.Size = new System.Drawing.Size(111, 78);
            this.onButton.TabIndex = 0;
            this.onButton.Text = "Turn Paperless On";
            this.onButton.UseVisualStyleBackColor = true;
            this.onButton.Click += new System.EventHandler(this.onButton_Click);
            // 
            // offButton
            // 
            this.offButton.Location = new System.Drawing.Point(158, 76);
            this.offButton.Name = "offButton";
            this.offButton.Size = new System.Drawing.Size(111, 78);
            this.offButton.TabIndex = 1;
            this.offButton.Text = "Turn Paperless Off";
            this.offButton.UseVisualStyleBackColor = true;
            this.offButton.Click += new System.EventHandler(this.offButton_Click);
            // 
            // textBox_Address
            // 
            this.textBox_Address.Location = new System.Drawing.Point(94, 21);
            this.textBox_Address.Name = "textBox_Address";
            this.textBox_Address.Size = new System.Drawing.Size(153, 20);
            this.textBox_Address.TabIndex = 2;
            // 
            // label_Address
            // 
            this.label_Address.AutoSize = true;
            this.label_Address.Location = new System.Drawing.Point(27, 24);
            this.label_Address.Name = "label_Address";
            this.label_Address.Size = new System.Drawing.Size(61, 13);
            this.label_Address.TabIndex = 3;
            this.label_Address.Text = "IP Address:";
            // 
            // label_AdminPwd
            // 
            this.label_AdminPwd.AutoSize = true;
            this.label_AdminPwd.Location = new System.Drawing.Point(27, 50);
            this.label_AdminPwd.Name = "label_AdminPwd";
            this.label_AdminPwd.Size = new System.Drawing.Size(63, 13);
            this.label_AdminPwd.TabIndex = 5;
            this.label_AdminPwd.Text = "Admin Pwd:";
            // 
            // textBox_AdminPwd
            // 
            this.textBox_AdminPwd.Location = new System.Drawing.Point(94, 47);
            this.textBox_AdminPwd.Name = "textBox_AdminPwd";
            this.textBox_AdminPwd.Size = new System.Drawing.Size(153, 20);
            this.textBox_AdminPwd.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 162);
            this.Controls.Add(this.label_AdminPwd);
            this.Controls.Add(this.textBox_AdminPwd);
            this.Controls.Add(this.label_Address);
            this.Controls.Add(this.textBox_Address);
            this.Controls.Add(this.offButton);
            this.Controls.Add(this.onButton);
            this.Name = "MainForm";
            this.Text = "Paperless Mode Utility";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button onButton;
        private System.Windows.Forms.Button offButton;
        private System.Windows.Forms.TextBox textBox_Address;
        private System.Windows.Forms.Label label_Address;
        private System.Windows.Forms.Label label_AdminPwd;
        private System.Windows.Forms.TextBox textBox_AdminPwd;
    }
}

