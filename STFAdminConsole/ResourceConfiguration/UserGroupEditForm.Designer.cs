namespace HP.ScalableTest.LabConsole
{
    partial class UserGroupEditForm
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
            this.groupNameLabel = new System.Windows.Forms.Label();
            this.groupName_TextBox = new System.Windows.Forms.TextBox();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // groupNameLabel
            // 
            this.groupNameLabel.AutoSize = true;
            this.groupNameLabel.Location = new System.Drawing.Point(8, 20);
            this.groupNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.groupNameLabel.Name = "groupNameLabel";
            this.groupNameLabel.Size = new System.Drawing.Size(94, 20);
            this.groupNameLabel.TabIndex = 0;
            this.groupNameLabel.Text = "Group Name";
            this.groupNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupName_TextBox
            // 
            this.groupName_TextBox.Location = new System.Drawing.Point(110, 17);
            this.groupName_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.groupName_TextBox.Name = "groupName_TextBox";
            this.groupName_TextBox.Size = new System.Drawing.Size(298, 27);
            this.groupName_TextBox.TabIndex = 0;
            // 
            // description_TextBox
            // 
            this.description_TextBox.Location = new System.Drawing.Point(110, 53);
            this.description_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(298, 65);
            this.description_TextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(17, 56);
            this.descriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(85, 20);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(200, 151);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(100, 32);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(308, 151);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 5;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // UserGroupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 196);
            this.ControlBox = false;
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.groupNameLabel);
            this.Controls.Add(this.groupName_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "UserGroupEditForm";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.UserEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label groupNameLabel;
        private System.Windows.Forms.TextBox groupName_TextBox;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_Button;
    }
}