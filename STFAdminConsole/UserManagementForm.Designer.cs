namespace HP.ScalableTest.LabConsole
{
    partial class UserManagementForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.userName_TextBox = new System.Windows.Forms.TextBox();
            this.domain_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.role_ComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.groups_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userName_TextBox
            // 
            this.userName_TextBox.Location = new System.Drawing.Point(130, 17);
            this.userName_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.userName_TextBox.Name = "userName_TextBox";
            this.userName_TextBox.Size = new System.Drawing.Size(268, 24);
            this.userName_TextBox.TabIndex = 0;
            // 
            // domain_TextBox
            // 
            this.domain_TextBox.Location = new System.Drawing.Point(130, 53);
            this.domain_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.domain_TextBox.Name = "domain_TextBox";
            this.domain_TextBox.Size = new System.Drawing.Size(268, 24);
            this.domain_TextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Domain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // role_ComboBox
            // 
            this.role_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.role_ComboBox.FormattingEnabled = true;
            this.role_ComboBox.Location = new System.Drawing.Point(130, 89);
            this.role_ComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.role_ComboBox.Name = "role_ComboBox";
            this.role_ComboBox.Size = new System.Drawing.Size(268, 26);
            this.role_ComboBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "User Role";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "User Groups";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(166, 291);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(112, 36);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(286, 291);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(112, 36);
            this.cancel_Button.TabIndex = 5;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // groups_CheckedListBox
            // 
            this.groups_CheckedListBox.BackColor = System.Drawing.Color.White;
            this.groups_CheckedListBox.CheckOnClick = true;
            this.groups_CheckedListBox.FormattingEnabled = true;
            this.groups_CheckedListBox.Location = new System.Drawing.Point(130, 130);
            this.groups_CheckedListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groups_CheckedListBox.Name = "groups_CheckedListBox";
            this.groups_CheckedListBox.Size = new System.Drawing.Size(268, 137);
            this.groups_CheckedListBox.TabIndex = 3;
            // 
            // UserManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 342);
            this.ControlBox = false;
            this.Controls.Add(this.groups_CheckedListBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.role_ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.domain_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userName_TextBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "UserManagementForm";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.UserEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox userName_TextBox;
        private System.Windows.Forms.TextBox domain_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox role_ComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.CheckedListBox groups_CheckedListBox;
    }
}