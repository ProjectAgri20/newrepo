namespace HP.ScalableTest.LabConsole
{
    partial class ServerSettingsEditForm
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
            this.components = new System.ComponentModel.Container();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.ok_Button = new System.Windows.Forms.Button();
            this.description_Label = new System.Windows.Forms.Label();
            this.value_Label = new System.Windows.Forms.Label();
            this.name_Label = new System.Windows.Forms.Label();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.value_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.SuspendLayout();
            // 
            // description_TextBox
            // 
            this.description_TextBox.Location = new System.Drawing.Point(116, 79);
            this.description_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(399, 68);
            this.description_TextBox.TabIndex = 15;
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(421, 158);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 17;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(313, 158);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 16;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.Ok_Button_Click);
            // 
            // description_Label
            // 
            this.description_Label.Location = new System.Drawing.Point(14, 79);
            this.description_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(94, 18);
            this.description_Label.TabIndex = 14;
            this.description_Label.Text = "Description";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // value_Label
            // 
            this.value_Label.Location = new System.Drawing.Point(13, 48);
            this.value_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.value_Label.Name = "value_Label";
            this.value_Label.Size = new System.Drawing.Size(95, 18);
            this.value_Label.TabIndex = 12;
            this.value_Label.Text = "Setting Value";
            this.value_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // name_Label
            // 
            this.name_Label.Location = new System.Drawing.Point(12, 13);
            this.name_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(96, 18);
            this.name_Label.TabIndex = 10;
            this.name_Label.Text = "Setting Name";
            this.name_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // name_TextBox
            // 
            this.name_TextBox.Location = new System.Drawing.Point(116, 9);
            this.name_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(268, 20);
            this.name_TextBox.TabIndex = 11;
            // 
            // value_TextBox
            // 
            this.value_TextBox.Location = new System.Drawing.Point(116, 44);
            this.value_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.value_TextBox.Name = "value_TextBox";
            this.value_TextBox.Size = new System.Drawing.Size(399, 20);
            this.value_TextBox.TabIndex = 13;
            // 
            // ServerSettingsEditForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(524, 193);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.value_Label);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.value_TextBox);
            this.Name = "ServerSettingsEditForm";
            this.Text = "Server Setting";
            this.Activated += new System.EventHandler(this.ServerSettingsEditForm_Activated);
            this.Load += new System.EventHandler(this.GlobalSettingsEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label value_Label;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.TextBox value_TextBox;
        private Framework.UI.FieldValidator fieldValidator;
    }
}