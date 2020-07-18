namespace HP.ScalableTest
{
    partial class CameraEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraEditForm));
            this.assetId_Label = new System.Windows.Forms.Label();
            this.ipAddress_Label = new System.Windows.Forms.Label();
            this.cameraServer_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.printerId_Label = new System.Windows.Forms.Label();
            this.assetId_TextBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.cameraServer_TextBox = new System.Windows.Forms.TextBox();
            this.printerId_TextBox = new System.Windows.Forms.TextBox();
            this.ipAddress_Control = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.SuspendLayout();
            // 
            // assetId_Label
            // 
            this.assetId_Label.Location = new System.Drawing.Point(2, 9);
            this.assetId_Label.Name = "assetId_Label";
            this.assetId_Label.Size = new System.Drawing.Size(119, 24);
            this.assetId_Label.TabIndex = 0;
            this.assetId_Label.Text = "Asset ID";
            this.assetId_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ipAddress_Label
            // 
            this.ipAddress_Label.Location = new System.Drawing.Point(22, 41);
            this.ipAddress_Label.Name = "ipAddress_Label";
            this.ipAddress_Label.Size = new System.Drawing.Size(99, 24);
            this.ipAddress_Label.TabIndex = 4;
            this.ipAddress_Label.Text = "IP Address";
            this.ipAddress_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cameraServer_Label
            // 
            this.cameraServer_Label.Location = new System.Drawing.Point(25, 73);
            this.cameraServer_Label.Name = "cameraServer_Label";
            this.cameraServer_Label.Size = new System.Drawing.Size(96, 24);
            this.cameraServer_Label.TabIndex = 6;
            this.cameraServer_Label.Text = "Camera Server";
            this.cameraServer_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_Label
            // 
            this.description_Label.Location = new System.Drawing.Point(25, 143);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(96, 24);
            this.description_Label.TabIndex = 2;
            this.description_Label.Text = "Description";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_TextBox
            // 
            this.description_TextBox.CausesValidation = false;
            this.description_TextBox.Location = new System.Drawing.Point(127, 139);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(279, 85);
            this.description_TextBox.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(306, 235);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 19;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(200, 235);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 18;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // printerId_Label
            // 
            this.printerId_Label.Location = new System.Drawing.Point(12, 105);
            this.printerId_Label.Name = "printerId_Label";
            this.printerId_Label.Size = new System.Drawing.Size(109, 24);
            this.printerId_Label.TabIndex = 8;
            this.printerId_Label.Text = "Printer Id";
            this.printerId_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // assetId_TextBox
            // 
            this.assetId_TextBox.Location = new System.Drawing.Point(127, 9);
            this.assetId_TextBox.Name = "assetId_TextBox";
            this.assetId_TextBox.Size = new System.Drawing.Size(279, 23);
            this.assetId_TextBox.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // cameraServer_TextBox
            // 
            this.cameraServer_TextBox.CausesValidation = false;
            this.cameraServer_TextBox.Location = new System.Drawing.Point(127, 73);
            this.cameraServer_TextBox.Name = "cameraServer_TextBox";
            this.cameraServer_TextBox.Size = new System.Drawing.Size(279, 23);
            this.cameraServer_TextBox.TabIndex = 3;
            // 
            // printerId_TextBox
            // 
            this.printerId_TextBox.CausesValidation = false;
            this.printerId_TextBox.Location = new System.Drawing.Point(127, 105);
            this.printerId_TextBox.Name = "printerId_TextBox";
            this.printerId_TextBox.Size = new System.Drawing.Size(279, 23);
            this.printerId_TextBox.TabIndex = 4;
            // 
            // ipAddress_Control
            // 
            this.ipAddress_Control.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddress_Control.CausesValidation = false;
            this.ipAddress_Control.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddress_Control.Location = new System.Drawing.Point(127, 41);
            this.ipAddress_Control.MinimumSize = new System.Drawing.Size(87, 23);
            this.ipAddress_Control.Name = "ipAddress_Control";
            this.ipAddress_Control.Size = new System.Drawing.Size(114, 23);
            this.ipAddress_Control.TabIndex = 2;
            // 
            // CameraEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(418, 279);
            this.Controls.Add(this.ipAddress_Control);
            this.Controls.Add(this.printerId_TextBox);
            this.Controls.Add(this.cameraServer_TextBox);
            this.Controls.Add(this.printerId_Label);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.cameraServer_Label);
            this.Controls.Add(this.ipAddress_Label);
            this.Controls.Add(this.assetId_Label);
            this.Controls.Add(this.assetId_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CameraEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Camera Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraEditForm_FormClosing);
            this.Load += new System.EventHandler(this.CameraEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label assetId_Label;
        private System.Windows.Forms.Label ipAddress_Label;
        private System.Windows.Forms.Label cameraServer_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label printerId_Label;
        private System.Windows.Forms.TextBox assetId_TextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TextBox cameraServer_TextBox;
        private System.Windows.Forms.TextBox printerId_TextBox;
        private Framework.UI.IPAddressControl ipAddress_Control;
    }
}