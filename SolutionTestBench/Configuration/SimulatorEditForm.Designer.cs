namespace HP.ScalableTest.UI
{
    partial class SimulatorEditForm
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
            this.textBox_Address = new System.Windows.Forms.TextBox();
            this.label_Address = new System.Windows.Forms.Label();
            this.label_Product = new System.Windows.Forms.Label();
            this.textBox_Product = new System.Windows.Forms.TextBox();
            this.label_AssetId = new System.Windows.Forms.Label();
            this.textBox_AssetId = new System.Windows.Forms.TextBox();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Firmware = new System.Windows.Forms.Label();
            this.textBox_Firmware = new System.Windows.Forms.TextBox();
            this.label_VmName = new System.Windows.Forms.Label();
            this.textBox_VmName = new System.Windows.Forms.TextBox();
            this.comboBox_Type = new System.Windows.Forms.ComboBox();
            this.label_Type = new System.Windows.Forms.Label();
            this.button_Ok = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.label_Capabilites = new System.Windows.Forms.Label();
            this.checkBox_ControlPanel = new System.Windows.Forms.CheckBox();
            this.checkBox_Scan = new System.Windows.Forms.CheckBox();
            this.checkBox_Mobile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox_Address
            // 
            this.textBox_Address.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Address.CausesValidation = false;
            this.textBox_Address.Location = new System.Drawing.Point(154, 57);
            this.textBox_Address.Name = "textBox_Address";
            this.textBox_Address.Size = new System.Drawing.Size(279, 20);
            this.textBox_Address.TabIndex = 6;
            // 
            // label_Address
            // 
            this.label_Address.Location = new System.Drawing.Point(55, 57);
            this.label_Address.Name = "label_Address";
            this.label_Address.Size = new System.Drawing.Size(96, 24);
            this.label_Address.TabIndex = 5;
            this.label_Address.Text = "Address";
            this.label_Address.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Product
            // 
            this.label_Product.Location = new System.Drawing.Point(29, 31);
            this.label_Product.Name = "label_Product";
            this.label_Product.Size = new System.Drawing.Size(122, 24);
            this.label_Product.TabIndex = 3;
            this.label_Product.Text = "Product";
            this.label_Product.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_Product
            // 
            this.textBox_Product.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Product.CausesValidation = false;
            this.textBox_Product.Location = new System.Drawing.Point(154, 31);
            this.textBox_Product.Name = "textBox_Product";
            this.textBox_Product.Size = new System.Drawing.Size(279, 20);
            this.textBox_Product.TabIndex = 4;
            // 
            // label_AssetId
            // 
            this.label_AssetId.Location = new System.Drawing.Point(29, 5);
            this.label_AssetId.Name = "label_AssetId";
            this.label_AssetId.Size = new System.Drawing.Size(119, 24);
            this.label_AssetId.TabIndex = 1;
            this.label_AssetId.Text = "Simulator Id";
            this.label_AssetId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_AssetId
            // 
            this.textBox_AssetId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_AssetId.Location = new System.Drawing.Point(154, 5);
            this.textBox_AssetId.Name = "textBox_AssetId";
            this.textBox_AssetId.Size = new System.Drawing.Size(279, 20);
            this.textBox_AssetId.TabIndex = 2;
            // 
            // textBox_Password
            // 
            this.textBox_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Password.CausesValidation = false;
            this.textBox_Password.Location = new System.Drawing.Point(154, 135);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.Size = new System.Drawing.Size(279, 20);
            this.textBox_Password.TabIndex = 12;
            // 
            // label_Password
            // 
            this.label_Password.Location = new System.Drawing.Point(55, 135);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(96, 24);
            this.label_Password.TabIndex = 11;
            this.label_Password.Text = "Admin Password";
            this.label_Password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Firmware
            // 
            this.label_Firmware.Location = new System.Drawing.Point(29, 109);
            this.label_Firmware.Name = "label_Firmware";
            this.label_Firmware.Size = new System.Drawing.Size(122, 24);
            this.label_Firmware.TabIndex = 9;
            this.label_Firmware.Text = "Firmware Version";
            this.label_Firmware.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_Firmware
            // 
            this.textBox_Firmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Firmware.CausesValidation = false;
            this.textBox_Firmware.Location = new System.Drawing.Point(154, 109);
            this.textBox_Firmware.Name = "textBox_Firmware";
            this.textBox_Firmware.Size = new System.Drawing.Size(279, 20);
            this.textBox_Firmware.TabIndex = 10;
            // 
            // label_VmName
            // 
            this.label_VmName.Location = new System.Drawing.Point(29, 83);
            this.label_VmName.Name = "label_VmName";
            this.label_VmName.Size = new System.Drawing.Size(119, 24);
            this.label_VmName.TabIndex = 7;
            this.label_VmName.Text = "VM or Host Name";
            this.label_VmName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_VmName
            // 
            this.textBox_VmName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_VmName.Location = new System.Drawing.Point(154, 83);
            this.textBox_VmName.Name = "textBox_VmName";
            this.textBox_VmName.Size = new System.Drawing.Size(279, 20);
            this.textBox_VmName.TabIndex = 8;
            // 
            // comboBox_Type
            // 
            this.comboBox_Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Type.CausesValidation = false;
            this.comboBox_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Type.FormattingEnabled = true;
            this.comboBox_Type.Items.AddRange(new object[] {
            "Jedi",
            "Sirius",
            "Mobile"});
            this.comboBox_Type.Location = new System.Drawing.Point(154, 162);
            this.comboBox_Type.Name = "comboBox_Type";
            this.comboBox_Type.Size = new System.Drawing.Size(279, 21);
            this.comboBox_Type.TabIndex = 14;
            // 
            // label_Type
            // 
            this.label_Type.Location = new System.Drawing.Point(70, 162);
            this.label_Type.Name = "label_Type";
            this.label_Type.Size = new System.Drawing.Size(81, 24);
            this.label_Type.TabIndex = 13;
            this.label_Type.Text = "Simulator Type";
            this.label_Type.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.Location = new System.Drawing.Point(229, 226);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(100, 32);
            this.button_Ok.TabIndex = 20;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(335, 226);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(100, 32);
            this.button_Cancel.TabIndex = 21;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // label_Capabilites
            // 
            this.label_Capabilites.AutoSize = true;
            this.label_Capabilites.Location = new System.Drawing.Point(88, 189);
            this.label_Capabilites.Name = "label_Capabilites";
            this.label_Capabilites.Size = new System.Drawing.Size(60, 13);
            this.label_Capabilites.TabIndex = 15;
            this.label_Capabilites.Text = "Capabilities";
            // 
            // checkBox_ControlPanel
            // 
            this.checkBox_ControlPanel.AutoSize = true;
            this.checkBox_ControlPanel.Location = new System.Drawing.Point(220, 189);
            this.checkBox_ControlPanel.Name = "checkBox_ControlPanel";
            this.checkBox_ControlPanel.Size = new System.Drawing.Size(89, 17);
            this.checkBox_ControlPanel.TabIndex = 17;
            this.checkBox_ControlPanel.Tag = "4";
            this.checkBox_ControlPanel.Text = "Control Panel";
            this.checkBox_ControlPanel.UseVisualStyleBackColor = true;
            // 
            // checkBox_Scan
            // 
            this.checkBox_Scan.AutoSize = true;
            this.checkBox_Scan.Location = new System.Drawing.Point(154, 189);
            this.checkBox_Scan.Name = "checkBox_Scan";
            this.checkBox_Scan.Size = new System.Drawing.Size(51, 17);
            this.checkBox_Scan.TabIndex = 16;
            this.checkBox_Scan.Tag = "2";
            this.checkBox_Scan.Text = "Scan";
            this.checkBox_Scan.UseVisualStyleBackColor = true;
            // 
            // checkBox_Mobile
            // 
            this.checkBox_Mobile.AutoSize = true;
            this.checkBox_Mobile.Location = new System.Drawing.Point(326, 189);
            this.checkBox_Mobile.Name = "checkBox_Mobile";
            this.checkBox_Mobile.Size = new System.Drawing.Size(57, 17);
            this.checkBox_Mobile.TabIndex = 18;
            this.checkBox_Mobile.Tag = "8";
            this.checkBox_Mobile.Text = "Mobile";
            this.checkBox_Mobile.UseVisualStyleBackColor = true;
            // 
            // SimulatorEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(438, 261);
            this.Controls.Add(this.label_Capabilites);
            this.Controls.Add(this.checkBox_ControlPanel);
            this.Controls.Add(this.checkBox_Scan);
            this.Controls.Add(this.checkBox_Mobile);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.comboBox_Type);
            this.Controls.Add(this.label_Type);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.label_Password);
            this.Controls.Add(this.label_Firmware);
            this.Controls.Add(this.textBox_Firmware);
            this.Controls.Add(this.label_VmName);
            this.Controls.Add(this.textBox_VmName);
            this.Controls.Add(this.textBox_Address);
            this.Controls.Add(this.label_Address);
            this.Controls.Add(this.label_Product);
            this.Controls.Add(this.textBox_Product);
            this.Controls.Add(this.label_AssetId);
            this.Controls.Add(this.textBox_AssetId);
            this.MinimumSize = new System.Drawing.Size(454, 269);
            this.Name = "SimulatorEditForm";
            this.Text = "Simulator Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimulatorEditForm_FormClosing);
            this.Load += new System.EventHandler(this.SimulatorEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Address;
        private System.Windows.Forms.Label label_Address;
        private System.Windows.Forms.Label label_Product;
        private System.Windows.Forms.TextBox textBox_Product;
        private System.Windows.Forms.Label label_AssetId;
        private System.Windows.Forms.TextBox textBox_AssetId;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Firmware;
        private System.Windows.Forms.TextBox textBox_Firmware;
        private System.Windows.Forms.Label label_VmName;
        private System.Windows.Forms.TextBox textBox_VmName;
        private System.Windows.Forms.ComboBox comboBox_Type;
        private System.Windows.Forms.Label label_Type;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.Button button_Cancel;
        private ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label label_Capabilites;
        private System.Windows.Forms.CheckBox checkBox_ControlPanel;
        private System.Windows.Forms.CheckBox checkBox_Scan;
        private System.Windows.Forms.CheckBox checkBox_Mobile;
    }
}