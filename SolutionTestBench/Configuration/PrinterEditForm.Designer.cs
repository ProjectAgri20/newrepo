namespace HP.ScalableTest
{
    partial class PrinterEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterEditForm));
            this.assetId_Label = new System.Windows.Forms.Label();
            this.manufacturer_Label = new System.Windows.Forms.Label();
            this.manufacturer_ComboBox = new System.Windows.Forms.ComboBox();
            this.modelName_Label = new System.Windows.Forms.Label();
            this.modelName_ComboBox = new System.Windows.Forms.ComboBox();
            this.address1_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.location_ComboBox = new System.Windows.Forms.ComboBox();
            this.location_Label = new System.Windows.Forms.Label();
            this.contact_Label = new System.Windows.Forms.Label();
            this.contact_ComboBox = new System.Windows.Forms.ComboBox();
            this.serialNumber_Label = new System.Windows.Forms.Label();
            this.serialNumber_TextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.modelNumber_ComboBox = new System.Windows.Forms.ComboBox();
            this.modelNumber_Label = new System.Windows.Forms.Label();
            this.assetId_TextBox = new System.Windows.Forms.TextBox();
            this.print_CheckBox = new System.Windows.Forms.CheckBox();
            this.scan_CheckBox = new System.Windows.Forms.CheckBox();
            this.controlPanel_CheckBox = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printerCapabilites_Label = new System.Windows.Forms.Label();
            this.address1_Control = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.adminPassword_Label = new System.Windows.Forms.Label();
            this.adminPassword_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.address2_Control = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.address2_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // assetId_Label
            // 
            this.assetId_Label.Location = new System.Drawing.Point(2, 32);
            this.assetId_Label.Name = "assetId_Label";
            this.assetId_Label.Size = new System.Drawing.Size(119, 24);
            this.assetId_Label.TabIndex = 0;
            this.assetId_Label.Text = "Printer Name or Id";
            this.assetId_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // manufacturer_Label
            // 
            this.manufacturer_Label.Location = new System.Drawing.Point(25, 154);
            this.manufacturer_Label.Name = "manufacturer_Label";
            this.manufacturer_Label.Size = new System.Drawing.Size(99, 24);
            this.manufacturer_Label.TabIndex = 4;
            this.manufacturer_Label.Text = "Manufacturer";
            this.manufacturer_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // manufacturer_ComboBox
            // 
            this.manufacturer_ComboBox.CausesValidation = false;
            this.manufacturer_ComboBox.FormattingEnabled = true;
            this.manufacturer_ComboBox.Location = new System.Drawing.Point(127, 154);
            this.manufacturer_ComboBox.Name = "manufacturer_ComboBox";
            this.manufacturer_ComboBox.Size = new System.Drawing.Size(331, 23);
            this.manufacturer_ComboBox.TabIndex = 5;
            // 
            // modelName_Label
            // 
            this.modelName_Label.Location = new System.Drawing.Point(28, 188);
            this.modelName_Label.Name = "modelName_Label";
            this.modelName_Label.Size = new System.Drawing.Size(96, 24);
            this.modelName_Label.TabIndex = 6;
            this.modelName_Label.Text = "Model Name";
            this.modelName_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // modelName_ComboBox
            // 
            this.modelName_ComboBox.CausesValidation = false;
            this.modelName_ComboBox.FormattingEnabled = true;
            this.modelName_ComboBox.Location = new System.Drawing.Point(127, 188);
            this.modelName_ComboBox.Name = "modelName_ComboBox";
            this.modelName_ComboBox.Size = new System.Drawing.Size(331, 23);
            this.modelName_ComboBox.TabIndex = 7;
            // 
            // address1_Label
            // 
            this.address1_Label.Location = new System.Drawing.Point(5, 254);
            this.address1_Label.Name = "address1_Label";
            this.address1_Label.Size = new System.Drawing.Size(119, 24);
            this.address1_Label.TabIndex = 10;
            this.address1_Label.Text = "Primary Address";
            this.address1_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_Label
            // 
            this.description_Label.Location = new System.Drawing.Point(25, 62);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(96, 24);
            this.description_Label.TabIndex = 2;
            this.description_Label.Text = "Description";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_TextBox
            // 
            this.description_TextBox.CausesValidation = false;
            this.description_TextBox.Location = new System.Drawing.Point(127, 62);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(331, 85);
            this.description_TextBox.TabIndex = 3;
            // 
            // location_ComboBox
            // 
            this.location_ComboBox.CausesValidation = false;
            this.location_ComboBox.FormattingEnabled = true;
            this.location_ComboBox.Location = new System.Drawing.Point(127, 323);
            this.location_ComboBox.Name = "location_ComboBox";
            this.location_ComboBox.Size = new System.Drawing.Size(331, 23);
            this.location_ComboBox.TabIndex = 13;
            // 
            // location_Label
            // 
            this.location_Label.Location = new System.Drawing.Point(43, 323);
            this.location_Label.Name = "location_Label";
            this.location_Label.Size = new System.Drawing.Size(81, 24);
            this.location_Label.TabIndex = 12;
            this.location_Label.Text = "Location";
            this.location_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contact_Label
            // 
            this.contact_Label.Location = new System.Drawing.Point(43, 357);
            this.contact_Label.Name = "contact_Label";
            this.contact_Label.Size = new System.Drawing.Size(81, 24);
            this.contact_Label.TabIndex = 14;
            this.contact_Label.Text = "Contact";
            this.contact_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contact_ComboBox
            // 
            this.contact_ComboBox.CausesValidation = false;
            this.contact_ComboBox.FormattingEnabled = true;
            this.contact_ComboBox.Location = new System.Drawing.Point(127, 357);
            this.contact_ComboBox.Name = "contact_ComboBox";
            this.contact_ComboBox.Size = new System.Drawing.Size(331, 23);
            this.contact_ComboBox.TabIndex = 15;
            // 
            // serialNumber_Label
            // 
            this.serialNumber_Label.Location = new System.Drawing.Point(2, 391);
            this.serialNumber_Label.Name = "serialNumber_Label";
            this.serialNumber_Label.Size = new System.Drawing.Size(122, 24);
            this.serialNumber_Label.TabIndex = 16;
            this.serialNumber_Label.Text = "Serial Number";
            this.serialNumber_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serialNumber_TextBox
            // 
            this.serialNumber_TextBox.CausesValidation = false;
            this.serialNumber_TextBox.Location = new System.Drawing.Point(127, 391);
            this.serialNumber_TextBox.Name = "serialNumber_TextBox";
            this.serialNumber_TextBox.Size = new System.Drawing.Size(331, 23);
            this.serialNumber_TextBox.TabIndex = 17;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(358, 491);
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
            this.okButton.Location = new System.Drawing.Point(252, 491);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 18;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // modelNumber_ComboBox
            // 
            this.modelNumber_ComboBox.CausesValidation = false;
            this.modelNumber_ComboBox.FormattingEnabled = true;
            this.modelNumber_ComboBox.Location = new System.Drawing.Point(127, 222);
            this.modelNumber_ComboBox.Name = "modelNumber_ComboBox";
            this.modelNumber_ComboBox.Size = new System.Drawing.Size(331, 23);
            this.modelNumber_ComboBox.TabIndex = 9;
            // 
            // modelNumber_Label
            // 
            this.modelNumber_Label.Location = new System.Drawing.Point(15, 220);
            this.modelNumber_Label.Name = "modelNumber_Label";
            this.modelNumber_Label.Size = new System.Drawing.Size(109, 24);
            this.modelNumber_Label.TabIndex = 8;
            this.modelNumber_Label.Text = "Model Number";
            this.modelNumber_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // assetId_TextBox
            // 
            this.assetId_TextBox.Location = new System.Drawing.Point(127, 32);
            this.assetId_TextBox.Name = "assetId_TextBox";
            this.assetId_TextBox.Size = new System.Drawing.Size(331, 23);
            this.assetId_TextBox.TabIndex = 1;
            // 
            // print_CheckBox
            // 
            this.print_CheckBox.AutoSize = true;
            this.print_CheckBox.Location = new System.Drawing.Point(139, 428);
            this.print_CheckBox.Name = "print_CheckBox";
            this.print_CheckBox.Size = new System.Drawing.Size(51, 19);
            this.print_CheckBox.TabIndex = 20;
            this.print_CheckBox.Tag = "1";
            this.print_CheckBox.Text = "Print";
            this.print_CheckBox.UseVisualStyleBackColor = true;
            // 
            // scan_CheckBox
            // 
            this.scan_CheckBox.AutoSize = true;
            this.scan_CheckBox.Location = new System.Drawing.Point(210, 428);
            this.scan_CheckBox.Name = "scan_CheckBox";
            this.scan_CheckBox.Size = new System.Drawing.Size(51, 19);
            this.scan_CheckBox.TabIndex = 21;
            this.scan_CheckBox.Tag = "2";
            this.scan_CheckBox.Text = "Scan";
            this.scan_CheckBox.UseVisualStyleBackColor = true;
            // 
            // controlPanel_CheckBox
            // 
            this.controlPanel_CheckBox.AutoSize = true;
            this.controlPanel_CheckBox.Location = new System.Drawing.Point(285, 428);
            this.controlPanel_CheckBox.Name = "controlPanel_CheckBox";
            this.controlPanel_CheckBox.Size = new System.Drawing.Size(98, 19);
            this.controlPanel_CheckBox.TabIndex = 22;
            this.controlPanel_CheckBox.Tag = "4";
            this.controlPanel_CheckBox.Text = "Control Panel";
            this.controlPanel_CheckBox.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // printerCapabilites_Label
            // 
            this.printerCapabilites_Label.AutoSize = true;
            this.printerCapabilites_Label.Location = new System.Drawing.Point(53, 428);
            this.printerCapabilites_Label.Name = "printerCapabilites_Label";
            this.printerCapabilites_Label.Size = new System.Drawing.Size(68, 15);
            this.printerCapabilites_Label.TabIndex = 25;
            this.printerCapabilites_Label.Text = "Capabilities";
            // 
            // address1_Control
            // 
            this.address1_Control.BackColor = System.Drawing.SystemColors.Window;
            this.address1_Control.CausesValidation = false;
            this.address1_Control.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.address1_Control.Location = new System.Drawing.Point(127, 256);
            this.address1_Control.MinimumSize = new System.Drawing.Size(87, 23);
            this.address1_Control.Name = "address1_Control";
            this.address1_Control.Size = new System.Drawing.Size(114, 23);
            this.address1_Control.TabIndex = 11;
            this.address1_Control.Text = "...";
            // 
            // adminPassword_Label
            // 
            this.adminPassword_Label.Location = new System.Drawing.Point(-22, 289);
            this.adminPassword_Label.Name = "adminPassword_Label";
            this.adminPassword_Label.Size = new System.Drawing.Size(146, 24);
            this.adminPassword_Label.TabIndex = 26;
            this.adminPassword_Label.Text = "Admin Password";
            this.adminPassword_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // adminPassword_TextBox
            // 
            this.adminPassword_TextBox.CausesValidation = false;
            this.adminPassword_TextBox.Location = new System.Drawing.Point(127, 289);
            this.adminPassword_TextBox.Name = "adminPassword_TextBox";
            this.adminPassword_TextBox.Size = new System.Drawing.Size(114, 23);
            this.adminPassword_TextBox.TabIndex = 27;
            // 
            // address2_Control
            // 
            this.address2_Control.BackColor = System.Drawing.SystemColors.Window;
            this.address2_Control.CausesValidation = false;
            this.address2_Control.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.address2_Control.Location = new System.Drawing.Point(344, 255);
            this.address2_Control.MinimumSize = new System.Drawing.Size(87, 23);
            this.address2_Control.Name = "address2_Control";
            this.address2_Control.Size = new System.Drawing.Size(114, 23);
            this.address2_Control.TabIndex = 28;
            this.address2_Control.Text = "...";
            // 
            // address2_Label
            // 
            this.address2_Label.Location = new System.Drawing.Point(252, 254);
            this.address2_Label.Name = "address2_Label";
            this.address2_Label.Size = new System.Drawing.Size(86, 24);
            this.address2_Label.TabIndex = 29;
            this.address2_Label.Text = "2nd Address";
            this.address2_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PrinterEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(470, 535);
            this.Controls.Add(this.address2_Label);
            this.Controls.Add(this.address2_Control);
            this.Controls.Add(this.adminPassword_TextBox);
            this.Controls.Add(this.adminPassword_Label);
            this.Controls.Add(this.printerCapabilites_Label);
            this.Controls.Add(this.controlPanel_CheckBox);
            this.Controls.Add(this.scan_CheckBox);
            this.Controls.Add(this.print_CheckBox);
            this.Controls.Add(this.modelNumber_ComboBox);
            this.Controls.Add(this.modelNumber_Label);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.serialNumber_Label);
            this.Controls.Add(this.serialNumber_TextBox);
            this.Controls.Add(this.contact_ComboBox);
            this.Controls.Add(this.contact_Label);
            this.Controls.Add(this.location_ComboBox);
            this.Controls.Add(this.location_Label);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.address1_Control);
            this.Controls.Add(this.address1_Label);
            this.Controls.Add(this.modelName_ComboBox);
            this.Controls.Add(this.modelName_Label);
            this.Controls.Add(this.manufacturer_ComboBox);
            this.Controls.Add(this.manufacturer_Label);
            this.Controls.Add(this.assetId_Label);
            this.Controls.Add(this.assetId_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Device Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrinterEditForm_FormClosing);
            this.Load += new System.EventHandler(this.PrinterEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label assetId_Label;
        private System.Windows.Forms.Label manufacturer_Label;
        private System.Windows.Forms.ComboBox manufacturer_ComboBox;
        private System.Windows.Forms.Label modelName_Label;
        private System.Windows.Forms.ComboBox modelName_ComboBox;
        private System.Windows.Forms.Label address1_Label;
        private Framework.UI.IPAddressControl address1_Control;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.ComboBox location_ComboBox;
        private System.Windows.Forms.Label location_Label;
        private System.Windows.Forms.Label contact_Label;
        private System.Windows.Forms.ComboBox contact_ComboBox;
        private System.Windows.Forms.Label serialNumber_Label;
        private System.Windows.Forms.TextBox serialNumber_TextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ComboBox modelNumber_ComboBox;
        private System.Windows.Forms.Label modelNumber_Label;
        private System.Windows.Forms.TextBox assetId_TextBox;
        private System.Windows.Forms.CheckBox print_CheckBox;
        private System.Windows.Forms.CheckBox scan_CheckBox;
        private System.Windows.Forms.CheckBox controlPanel_CheckBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.Label printerCapabilites_Label;
        private System.Windows.Forms.Label adminPassword_Label;
        private System.Windows.Forms.TextBox adminPassword_TextBox;
        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.IPAddressControl address2_Control;
        private System.Windows.Forms.Label address2_Label;
    }
}