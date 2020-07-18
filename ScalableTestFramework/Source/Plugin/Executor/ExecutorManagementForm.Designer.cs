namespace HP.ScalableTest.Plugin.Executor
{
    partial class ExecutorManagementForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutorManagementForm));
            this.copydirectory_checkBox = new System.Windows.Forms.CheckBox();
            this.arguments_textBox = new System.Windows.Forms.TextBox();
            this.ok_button = new System.Windows.Forms.Button();
            this.browse_button = new System.Windows.Forms.Button();
            this.executableFileName_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cancel_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.sessionid_checkBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // copydirectory_checkBox
            // 
            this.copydirectory_checkBox.AutoSize = true;
            this.copydirectory_checkBox.Location = new System.Drawing.Point(12, 79);
            this.copydirectory_checkBox.Name = "copydirectory_checkBox";
            this.copydirectory_checkBox.Size = new System.Drawing.Size(95, 17);
            this.copydirectory_checkBox.TabIndex = 14;
            this.copydirectory_checkBox.Text = "Copy Directory";
            this.copydirectory_checkBox.UseVisualStyleBackColor = true;
            // 
            // arguments_textBox
            // 
            this.arguments_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.arguments_textBox.Location = new System.Drawing.Point(12, 124);
            this.arguments_textBox.Name = "arguments_textBox";
            this.arguments_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.arguments_textBox.Size = new System.Drawing.Size(380, 20);
            this.arguments_textBox.TabIndex = 13;
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(297, 177);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(71, 29);
            this.ok_button.TabIndex = 12;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // browse_button
            // 
            this.browse_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse_button.Location = new System.Drawing.Point(398, 51);
            this.browse_button.Name = "browse_button";
            this.browse_button.Size = new System.Drawing.Size(48, 23);
            this.browse_button.TabIndex = 11;
            this.browse_button.Text = "...";
            this.browse_button.UseVisualStyleBackColor = true;
            this.browse_button.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // executableFileName_textBox
            // 
            this.executableFileName_textBox.BackColor = System.Drawing.Color.White;
            this.executableFileName_textBox.Location = new System.Drawing.Point(12, 53);
            this.executableFileName_textBox.Name = "executableFileName_textBox";
            this.executableFileName_textBox.ReadOnly = true;
            this.executableFileName_textBox.Size = new System.Drawing.Size(380, 20);
            this.executableFileName_textBox.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Arguments (Optional)";
            // 
            // cancel_button
            // 
            this.cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_button.Location = new System.Drawing.Point(374, 177);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(71, 29);
            this.cancel_button.TabIndex = 16;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Executable";
            // 
            // sessionid_checkBox
            // 
            this.sessionid_checkBox.AutoSize = true;
            this.sessionid_checkBox.Location = new System.Drawing.Point(12, 150);
            this.sessionid_checkBox.Name = "sessionid_checkBox";
            this.sessionid_checkBox.Size = new System.Drawing.Size(133, 17);
            this.sessionid_checkBox.TabIndex = 18;
            this.sessionid_checkBox.Text = "SessionId as argument";
            this.sessionid_checkBox.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 173);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(279, 33);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = @"NOTE: Session Id is passed as $SessionId at runtime and tagged after the above arguments are passed";
            // 
            // ExecutorManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 222);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.sessionid_checkBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.copydirectory_checkBox);
            this.Controls.Add(this.arguments_textBox);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.browse_button);
            this.Controls.Add(this.executableFileName_textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExecutorManagementForm";
            this.Text = "Add Executor";
            this.Load += new System.EventHandler(this.ExecutorManagementForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox copydirectory_checkBox;
        private System.Windows.Forms.TextBox arguments_textBox;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button browse_button;
        private System.Windows.Forms.TextBox executableFileName_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox sessionid_checkBox;
        private System.Windows.Forms.TextBox textBox1;
    }
}