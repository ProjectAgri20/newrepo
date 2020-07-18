namespace HP.ScalableTest.Plugin.GFriendExecution
{
    partial class EditScriptForm
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
            this.text_Label = new System.Windows.Forms.Label();
            this.typeValue_Label = new System.Windows.Forms.Label();
            this.type_Label = new System.Windows.Forms.Label();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.name_Label = new System.Windows.Forms.Label();
            this.fileText_richTextBox = new System.Windows.Forms.RichTextBox();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // text_Label
            // 
            this.text_Label.AutoSize = true;
            this.text_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_Label.Location = new System.Drawing.Point(7, 65);
            this.text_Label.Name = "text_Label";
            this.text_Label.Size = new System.Drawing.Size(49, 15);
            this.text_Label.TabIndex = 15;
            this.text_Label.Text = "File Text";
            // 
            // typeValue_Label
            // 
            this.typeValue_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.typeValue_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeValue_Label.Location = new System.Drawing.Point(470, 13);
            this.typeValue_Label.Name = "typeValue_Label";
            this.typeValue_Label.Size = new System.Drawing.Size(98, 18);
            this.typeValue_Label.TabIndex = 16;
            this.typeValue_Label.Text = "GF Variable";
            this.typeValue_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // type_Label
            // 
            this.type_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.type_Label.AutoSize = true;
            this.type_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.type_Label.Location = new System.Drawing.Point(408, 14);
            this.type_Label.Name = "type_Label";
            this.type_Label.Size = new System.Drawing.Size(56, 15);
            this.type_Label.TabIndex = 17;
            this.type_Label.Text = "File Type:";
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_Button.Location = new System.Drawing.Point(493, 439);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 18;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok_Button.Location = new System.Drawing.Point(411, 439);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 19;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.Ok_Button_Click);
            // 
            // name_Label
            // 
            this.name_Label.AutoSize = true;
            this.name_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_Label.Location = new System.Drawing.Point(12, 14);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(63, 15);
            this.name_Label.TabIndex = 20;
            this.name_Label.Text = "File Name:";
            // 
            // fileText_richTextBox
            // 
            this.fileText_richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileText_richTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileText_richTextBox.Location = new System.Drawing.Point(3, 83);
            this.fileText_richTextBox.Name = "fileText_richTextBox";
            this.fileText_richTextBox.Size = new System.Drawing.Size(565, 350);
            this.fileText_richTextBox.TabIndex = 21;
            this.fileText_richTextBox.Text = "";
            // 
            // name_TextBox
            // 
            this.name_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name_TextBox.Location = new System.Drawing.Point(90, 12);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(306, 22);
            this.name_TextBox.TabIndex = 22;
            // 
            // EditScriptForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(571, 465);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.fileText_richTextBox);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.type_Label);
            this.Controls.Add(this.typeValue_Label);
            this.Controls.Add(this.text_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EditScriptForm";
            this.Text = "Edit GFriend Script";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label text_Label;
        private System.Windows.Forms.Label typeValue_Label;
        private System.Windows.Forms.Label type_Label;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.RichTextBox fileText_richTextBox;
        private System.Windows.Forms.TextBox name_TextBox;
    }
}