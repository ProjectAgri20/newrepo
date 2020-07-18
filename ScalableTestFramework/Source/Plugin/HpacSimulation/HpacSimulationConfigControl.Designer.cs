namespace HP.ScalableTest.Plugin.HpacSimulation
{
    partial class HpacSimulationConfigControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pullAllInfo_Label = new System.Windows.Forms.Label();
            this.printAll_CheckBox = new System.Windows.Forms.CheckBox();
            this.hpacServer_Label = new System.Windows.Forms.Label();
            this.authentication_GroupBox = new System.Windows.Forms.GroupBox();
            this.domain_RadioButton = new System.Windows.Forms.RadioButton();
            this.pic_RadioButton = new System.Windows.Forms.RadioButton();
            this.smartCard_RadioButton = new System.Windows.Forms.RadioButton();
            this.asset_Label = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Plugin.HpacSimulation.VirtualPrinterSelectionControl();
            this.hpac_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.authentication_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pullAllInfo_Label
            // 
            this.pullAllInfo_Label.AutoSize = true;
            this.pullAllInfo_Label.Location = new System.Drawing.Point(134, 165);
            this.pullAllInfo_Label.Name = "pullAllInfo_Label";
            this.pullAllInfo_Label.Size = new System.Drawing.Size(313, 13);
            this.pullAllInfo_Label.TabIndex = 55;
            this.pullAllInfo_Label.Text = "(Pulls all documents found in the print queue during each activity)";
            // 
            // printAll_CheckBox
            // 
            this.printAll_CheckBox.AutoSize = true;
            this.printAll_CheckBox.Location = new System.Drawing.Point(14, 164);
            this.printAll_CheckBox.Name = "printAll_CheckBox";
            this.printAll_CheckBox.Size = new System.Drawing.Size(114, 17);
            this.printAll_CheckBox.TabIndex = 54;
            this.printAll_CheckBox.Text = "Pull All Documents";
            this.printAll_CheckBox.UseVisualStyleBackColor = true;
            // 
            // hpacServer_Label
            // 
            this.hpacServer_Label.AutoSize = true;
            this.hpacServer_Label.Location = new System.Drawing.Point(11, 7);
            this.hpacServer_Label.Name = "hpacServer_Label";
            this.hpacServer_Label.Size = new System.Drawing.Size(70, 13);
            this.hpacServer_Label.TabIndex = 52;
            this.hpacServer_Label.Text = "HPAC Server";
            this.hpacServer_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // authentication_GroupBox
            // 
            this.authentication_GroupBox.Controls.Add(this.domain_RadioButton);
            this.authentication_GroupBox.Controls.Add(this.pic_RadioButton);
            this.authentication_GroupBox.Controls.Add(this.smartCard_RadioButton);
            this.authentication_GroupBox.Location = new System.Drawing.Point(14, 42);
            this.authentication_GroupBox.Name = "authentication_GroupBox";
            this.authentication_GroupBox.Size = new System.Drawing.Size(456, 116);
            this.authentication_GroupBox.TabIndex = 53;
            this.authentication_GroupBox.TabStop = false;
            this.authentication_GroupBox.Text = "Authentication Method";
            // 
            // domain_RadioButton
            // 
            this.domain_RadioButton.AutoSize = true;
            this.domain_RadioButton.Checked = true;
            this.domain_RadioButton.Location = new System.Drawing.Point(135, 29);
            this.domain_RadioButton.Name = "domain_RadioButton";
            this.domain_RadioButton.Size = new System.Drawing.Size(124, 17);
            this.domain_RadioButton.TabIndex = 2;
            this.domain_RadioButton.TabStop = true;
            this.domain_RadioButton.Text = "Windows Credentials";
            this.domain_RadioButton.UseVisualStyleBackColor = true;
            this.domain_RadioButton.Click += new System.EventHandler(this.HpacAuthenticationMode_RadioButton_Click);
            // 
            // pic_RadioButton
            // 
            this.pic_RadioButton.AutoSize = true;
            this.pic_RadioButton.Location = new System.Drawing.Point(135, 54);
            this.pic_RadioButton.Name = "pic_RadioButton";
            this.pic_RadioButton.Size = new System.Drawing.Size(183, 17);
            this.pic_RadioButton.TabIndex = 0;
            this.pic_RadioButton.Text = "PIC (Personal Identification Code)";
            this.pic_RadioButton.UseVisualStyleBackColor = true;
            this.pic_RadioButton.CheckedChanged += new System.EventHandler(this.HpacAuthenticationMode_RadioButton_Click);
            // 
            // smartCard_RadioButton
            // 
            this.smartCard_RadioButton.AutoSize = true;
            this.smartCard_RadioButton.Enabled = false;
            this.smartCard_RadioButton.Location = new System.Drawing.Point(135, 79);
            this.smartCard_RadioButton.Name = "smartCard_RadioButton";
            this.smartCard_RadioButton.Size = new System.Drawing.Size(74, 17);
            this.smartCard_RadioButton.TabIndex = 1;
            this.smartCard_RadioButton.Text = "SmartCard";
            this.smartCard_RadioButton.UseVisualStyleBackColor = true;
            this.smartCard_RadioButton.CheckedChanged += new System.EventHandler(this.HpacAuthenticationMode_RadioButton_Click);
            // 
            // asset_Label
            // 
            this.asset_Label.AutoSize = true;
            this.asset_Label.Location = new System.Drawing.Point(11, 190);
            this.asset_Label.Name = "asset_Label";
            this.asset_Label.Size = new System.Drawing.Size(92, 13);
            this.asset_Label.TabIndex = 57;
            this.asset_Label.Text = "Selected Asset(s):";
            this.asset_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 208);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(662, 212);
            this.assetSelectionControl.TabIndex = 56;
            // 
            // hpac_ServerComboBox
            // 
            this.hpac_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpac_ServerComboBox.Location = new System.Drawing.Point(96, 3);
            this.hpac_ServerComboBox.Name = "hpac_ServerComboBox";
            this.hpac_ServerComboBox.Size = new System.Drawing.Size(374, 23);
            this.hpac_ServerComboBox.TabIndex = 36;
            // 
            // HpacSimulationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.asset_Label);
            this.Controls.Add(this.assetSelectionControl);
            this.Controls.Add(this.pullAllInfo_Label);
            this.Controls.Add(this.printAll_CheckBox);
            this.Controls.Add(this.hpacServer_Label);
            this.Controls.Add(this.authentication_GroupBox);
            this.Controls.Add(this.hpac_ServerComboBox);
            this.Name = "HpacSimulationConfigControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.authentication_GroupBox.ResumeLayout(false);
            this.authentication_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UI.ServerComboBox hpac_ServerComboBox;
        private System.Windows.Forms.Label pullAllInfo_Label;
        private System.Windows.Forms.CheckBox printAll_CheckBox;
        private System.Windows.Forms.Label hpacServer_Label;
        private System.Windows.Forms.GroupBox authentication_GroupBox;
        private System.Windows.Forms.RadioButton domain_RadioButton;
        private System.Windows.Forms.RadioButton pic_RadioButton;
        private System.Windows.Forms.RadioButton smartCard_RadioButton;
        private VirtualPrinterSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label asset_Label;
    }
}
