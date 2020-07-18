namespace HP.ScalableTest.UI.Framework
{
    partial class MainFormLogOnDialog
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
            this.password_Label = new System.Windows.Forms.Label();
            this.domain_Label = new System.Windows.Forms.Label();
            this.username_TextBox = new System.Windows.Forms.TextBox();
            this.exit_Button = new System.Windows.Forms.Button();
            this.logIn_Button = new System.Windows.Forms.Button();
            this.username_Label = new System.Windows.Forms.Label();
            this.guest_CheckBox = new System.Windows.Forms.CheckBox();
            this.password_TextBox = new System.Windows.Forms.MaskedTextBox();
            this.domain_ComboBox = new System.Windows.Forms.ComboBox();
            this.connection_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // password_Label
            // 
            this.password_Label.AutoSize = true;
            this.password_Label.Location = new System.Drawing.Point(8, 92);
            this.password_Label.Name = "password_Label";
            this.password_Label.Size = new System.Drawing.Size(60, 15);
            this.password_Label.TabIndex = 4;
            this.password_Label.Text = "Password:";
            // 
            // domain_Label
            // 
            this.domain_Label.AutoSize = true;
            this.domain_Label.Location = new System.Drawing.Point(8, 36);
            this.domain_Label.Name = "domain_Label";
            this.domain_Label.Size = new System.Drawing.Size(52, 15);
            this.domain_Label.TabIndex = 0;
            this.domain_Label.Text = "Domain:";
            // 
            // username_TextBox
            // 
            this.username_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.username_TextBox.Location = new System.Drawing.Point(92, 61);
            this.username_TextBox.Name = "username_TextBox";
            this.username_TextBox.Size = new System.Drawing.Size(214, 23);
            this.username_TextBox.TabIndex = 3;
            this.username_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Control_HasValue);
            // 
            // exit_Button
            // 
            this.exit_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exit_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exit_Button.Location = new System.Drawing.Point(219, 143);
            this.exit_Button.Name = "exit_Button";
            this.exit_Button.Size = new System.Drawing.Size(87, 27);
            this.exit_Button.TabIndex = 8;
            this.exit_Button.Text = "&Cancel";
            this.exit_Button.UseVisualStyleBackColor = true;
            // 
            // logIn_Button
            // 
            this.logIn_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.logIn_Button.Location = new System.Drawing.Point(125, 143);
            this.logIn_Button.Name = "logIn_Button";
            this.logIn_Button.Size = new System.Drawing.Size(87, 27);
            this.logIn_Button.TabIndex = 7;
            this.logIn_Button.Text = "&Log In";
            this.logIn_Button.UseVisualStyleBackColor = true;
            this.logIn_Button.Click += new System.EventHandler(this.logIn_Button_Click);
            // 
            // username_Label
            // 
            this.username_Label.AutoSize = true;
            this.username_Label.Location = new System.Drawing.Point(8, 64);
            this.username_Label.Name = "username_Label";
            this.username_Label.Size = new System.Drawing.Size(63, 15);
            this.username_Label.TabIndex = 2;
            this.username_Label.Text = "Username:";
            // 
            // guest_CheckBox
            // 
            this.guest_CheckBox.AutoSize = true;
            this.guest_CheckBox.Location = new System.Drawing.Point(92, 117);
            this.guest_CheckBox.Name = "guest_CheckBox";
            this.guest_CheckBox.Size = new System.Drawing.Size(106, 19);
            this.guest_CheckBox.TabIndex = 6;
            this.guest_CheckBox.Text = "Log in as Guest";
            this.guest_CheckBox.UseVisualStyleBackColor = true;
            this.guest_CheckBox.CheckedChanged += new System.EventHandler(this.guest_CheckBox_CheckedChanged);
            // 
            // password_TextBox
            // 
            this.password_TextBox.Location = new System.Drawing.Point(92, 89);
            this.password_TextBox.Name = "password_TextBox";
            this.password_TextBox.Size = new System.Drawing.Size(214, 23);
            this.password_TextBox.TabIndex = 5;
            this.password_TextBox.UseSystemPasswordChar = true;
            this.password_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.Control_HasValue);
            // 
            // domain_ComboBox
            // 
            this.domain_ComboBox.FormattingEnabled = true;
            this.domain_ComboBox.ItemHeight = 15;
            this.domain_ComboBox.Location = new System.Drawing.Point(92, 33);
            this.domain_ComboBox.Name = "domain_ComboBox";
            this.domain_ComboBox.Size = new System.Drawing.Size(214, 23);
            this.domain_ComboBox.TabIndex = 1;
            this.domain_ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.Control_HasValue);
            // 
            // connection_Label
            // 
            this.connection_Label.AutoSize = true;
            this.connection_Label.Location = new System.Drawing.Point(8, 9);
            this.connection_Label.Name = "connection_Label";
            this.connection_Label.Size = new System.Drawing.Size(173, 15);
            this.connection_Label.TabIndex = 9;
            this.connection_Label.Text = "Connecting to: <Environment>";
            // 
            // MainFormLogOnDialog
            // 
            this.AcceptButton = this.logIn_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exit_Button;
            this.ClientSize = new System.Drawing.Size(314, 173);
            this.Controls.Add(this.connection_Label);
            this.Controls.Add(this.domain_ComboBox);
            this.Controls.Add(this.password_TextBox);
            this.Controls.Add(this.guest_CheckBox);
            this.Controls.Add(this.password_Label);
            this.Controls.Add(this.domain_Label);
            this.Controls.Add(this.username_TextBox);
            this.Controls.Add(this.exit_Button);
            this.Controls.Add(this.logIn_Button);
            this.Controls.Add(this.username_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainFormLogOnDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log In";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label password_Label;
        private System.Windows.Forms.Label domain_Label;
        private System.Windows.Forms.TextBox username_TextBox;
        private System.Windows.Forms.Button exit_Button;
        private System.Windows.Forms.Button logIn_Button;
        private System.Windows.Forms.Label username_Label;
        private System.Windows.Forms.CheckBox guest_CheckBox;
        private System.Windows.Forms.MaskedTextBox password_TextBox;
        private System.Windows.Forms.ComboBox domain_ComboBox;
        private System.Windows.Forms.Label connection_Label;
    }
}