namespace HP.ScalableTest.UI
{
    partial class MobileDeviceEditForm
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.connectionId_TextBox = new System.Windows.Forms.TextBox();
            this.type_Label = new System.Windows.Forms.Label();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.description_Label = new System.Windows.Forms.Label();
            this.connectionId_Label = new System.Windows.Forms.Label();
            this.assetId_Label = new System.Windows.Forms.Label();
            this.assetId_TextBox = new System.Windows.Forms.TextBox();
            this.type_ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // connectionId_TextBox
            // 
            this.connectionId_TextBox.CausesValidation = false;
            this.connectionId_TextBox.Location = new System.Drawing.Point(177, 47);
            this.connectionId_TextBox.Name = "connectionId_TextBox";
            this.connectionId_TextBox.Size = new System.Drawing.Size(279, 20);
            this.connectionId_TextBox.TabIndex = 23;
            // 
            // type_Label
            // 
            this.type_Label.Location = new System.Drawing.Point(62, 76);
            this.type_Label.Name = "type_Label";
            this.type_Label.Size = new System.Drawing.Size(109, 24);
            this.type_Label.TabIndex = 27;
            this.type_Label.Text = "Mobile Device Type";
            this.type_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(250, 206);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 32);
            this.ok_Button.TabIndex = 28;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(356, 206);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 29;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // description_TextBox
            // 
            this.description_TextBox.CausesValidation = false;
            this.description_TextBox.Location = new System.Drawing.Point(177, 110);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(279, 85);
            this.description_TextBox.TabIndex = 25;
            // 
            // description_Label
            // 
            this.description_Label.Location = new System.Drawing.Point(75, 107);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(96, 24);
            this.description_Label.TabIndex = 22;
            this.description_Label.Text = "Description";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // connectionId_Label
            // 
            this.connectionId_Label.Location = new System.Drawing.Point(7, 44);
            this.connectionId_Label.Name = "connectionId_Label";
            this.connectionId_Label.Size = new System.Drawing.Size(164, 24);
            this.connectionId_Label.TabIndex = 26;
            this.connectionId_Label.Text = "Connection Id";
            this.connectionId_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // assetId_Label
            // 
            this.assetId_Label.Location = new System.Drawing.Point(52, 12);
            this.assetId_Label.Name = "assetId_Label";
            this.assetId_Label.Size = new System.Drawing.Size(119, 24);
            this.assetId_Label.TabIndex = 20;
            this.assetId_Label.Text = "Asset ID";
            this.assetId_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // assetId_TextBox
            // 
            this.assetId_TextBox.Location = new System.Drawing.Point(177, 15);
            this.assetId_TextBox.Name = "assetId_TextBox";
            this.assetId_TextBox.Size = new System.Drawing.Size(175, 20);
            this.assetId_TextBox.TabIndex = 21;
            // 
            // type_ComboBox
            // 
            this.type_ComboBox.FormattingEnabled = true;
            this.type_ComboBox.Location = new System.Drawing.Point(177, 79);
            this.type_ComboBox.Name = "type_ComboBox";
            this.type_ComboBox.Size = new System.Drawing.Size(279, 21);
            this.type_ComboBox.TabIndex = 30;
            // 
            // MobileDeviceEditForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(464, 244);
            this.Controls.Add(this.type_ComboBox);
            this.Controls.Add(this.connectionId_TextBox);
            this.Controls.Add(this.type_Label);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.connectionId_Label);
            this.Controls.Add(this.assetId_Label);
            this.Controls.Add(this.assetId_TextBox);
            this.Name = "MobileDeviceEditForm";
            this.Text = "MobileDeviceEditForm";
            this.Load += new System.EventHandler(this.MobileDeviceEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TextBox connectionId_TextBox;
        private System.Windows.Forms.Label type_Label;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label connectionId_Label;
        private System.Windows.Forms.Label assetId_Label;
        private System.Windows.Forms.TextBox assetId_TextBox;
        private System.Windows.Forms.ComboBox type_ComboBox;
    }
}