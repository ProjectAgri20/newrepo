namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    partial class SafeComSimulationConfigControl
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
            this.printAll_CheckBox = new System.Windows.Forms.CheckBox();
            this.address_Label = new System.Windows.Forms.Label();
            this.address_TextBox = new System.Windows.Forms.TextBox();
            this.server_Label = new System.Windows.Forms.Label();
            this.authentication_GroupBox = new System.Windows.Forms.GroupBox();
            this.card_TextBox = new System.Windows.Forms.TextBox();
            this.card_Label = new System.Windows.Forms.Label();
            this.windows_RadioButton = new System.Windows.Forms.RadioButton();
            this.cardPin_RadioButton = new System.Windows.Forms.RadioButton();
            this.userPin_RadioButton = new System.Windows.Forms.RadioButton();
            this.userPassword_RadioButton = new System.Windows.Forms.RadioButton();
            this.pullAllInfo_Label = new System.Windows.Forms.Label();
            this.asset_Label = new System.Windows.Forms.Label();
            this.selectAsset_Button = new System.Windows.Forms.Button();
            this.asset_TextBox = new System.Windows.Forms.TextBox();
            this.safecom_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.authentication_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // printAll_CheckBox
            // 
            this.printAll_CheckBox.AutoSize = true;
            this.printAll_CheckBox.Location = new System.Drawing.Point(117, 221);
            this.printAll_CheckBox.Name = "printAll_CheckBox";
            this.printAll_CheckBox.Size = new System.Drawing.Size(114, 17);
            this.printAll_CheckBox.TabIndex = 57;
            this.printAll_CheckBox.Text = "Pull All Documents";
            this.printAll_CheckBox.UseVisualStyleBackColor = true;
            // 
            // address_Label
            // 
            this.address_Label.AutoSize = true;
            this.address_Label.Location = new System.Drawing.Point(8, 251);
            this.address_Label.Name = "address_Label";
            this.address_Label.Size = new System.Drawing.Size(103, 13);
            this.address_Label.TabIndex = 55;
            this.address_Label.Text = "Asset MAC Address:";
            // 
            // address_TextBox
            // 
            this.address_TextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.address_TextBox.Location = new System.Drawing.Point(117, 248);
            this.address_TextBox.MaxLength = 17;
            this.address_TextBox.Name = "address_TextBox";
            this.address_TextBox.Size = new System.Drawing.Size(249, 20);
            this.address_TextBox.TabIndex = 54;
            // 
            // server_Label
            // 
            this.server_Label.AutoSize = true;
            this.server_Label.Location = new System.Drawing.Point(8, 10);
            this.server_Label.Name = "server_Label";
            this.server_Label.Size = new System.Drawing.Size(87, 13);
            this.server_Label.TabIndex = 53;
            this.server_Label.Text = "SafeCom Server:";
            // 
            // authentication_GroupBox
            // 
            this.authentication_GroupBox.Controls.Add(this.card_TextBox);
            this.authentication_GroupBox.Controls.Add(this.card_Label);
            this.authentication_GroupBox.Controls.Add(this.windows_RadioButton);
            this.authentication_GroupBox.Controls.Add(this.cardPin_RadioButton);
            this.authentication_GroupBox.Controls.Add(this.userPin_RadioButton);
            this.authentication_GroupBox.Controls.Add(this.userPassword_RadioButton);
            this.authentication_GroupBox.Location = new System.Drawing.Point(117, 38);
            this.authentication_GroupBox.Name = "authentication_GroupBox";
            this.authentication_GroupBox.Size = new System.Drawing.Size(418, 168);
            this.authentication_GroupBox.TabIndex = 52;
            this.authentication_GroupBox.TabStop = false;
            this.authentication_GroupBox.Text = "Authentication Method";
            // 
            // card_TextBox
            // 
            this.card_TextBox.Enabled = false;
            this.card_TextBox.Location = new System.Drawing.Point(133, 101);
            this.card_TextBox.Name = "card_TextBox";
            this.card_TextBox.Size = new System.Drawing.Size(158, 20);
            this.card_TextBox.TabIndex = 7;
            // 
            // card_Label
            // 
            this.card_Label.AutoSize = true;
            this.card_Label.Enabled = false;
            this.card_Label.Location = new System.Drawing.Point(71, 104);
            this.card_Label.Name = "card_Label";
            this.card_Label.Size = new System.Drawing.Size(49, 13);
            this.card_Label.TabIndex = 6;
            this.card_Label.Text = "Card No.";
            // 
            // windows_RadioButton
            // 
            this.windows_RadioButton.AutoSize = true;
            this.windows_RadioButton.Enabled = false;
            this.windows_RadioButton.Location = new System.Drawing.Point(28, 130);
            this.windows_RadioButton.Name = "windows_RadioButton";
            this.windows_RadioButton.Size = new System.Drawing.Size(124, 17);
            this.windows_RadioButton.TabIndex = 5;
            this.windows_RadioButton.Text = "Windows Credentials";
            this.windows_RadioButton.UseVisualStyleBackColor = true;
            this.windows_RadioButton.CheckedChanged += new System.EventHandler(this.Authentication_RadioButton_CheckedChanged);
            // 
            // cardPin_RadioButton
            // 
            this.cardPin_RadioButton.AutoSize = true;
            this.cardPin_RadioButton.Enabled = false;
            this.cardPin_RadioButton.Location = new System.Drawing.Point(28, 80);
            this.cardPin_RadioButton.Name = "cardPin_RadioButton";
            this.cardPin_RadioButton.Size = new System.Drawing.Size(121, 17);
            this.cardPin_RadioButton.TabIndex = 4;
            this.cardPin_RadioButton.Text = "Proximity Card && PIN";
            this.cardPin_RadioButton.UseVisualStyleBackColor = true;
            this.cardPin_RadioButton.CheckedChanged += new System.EventHandler(this.Authentication_RadioButton_CheckedChanged);
            // 
            // userPin_RadioButton
            // 
            this.userPin_RadioButton.AutoSize = true;
            this.userPin_RadioButton.Enabled = false;
            this.userPin_RadioButton.Location = new System.Drawing.Point(28, 55);
            this.userPin_RadioButton.Name = "userPin_RadioButton";
            this.userPin_RadioButton.Size = new System.Drawing.Size(103, 17);
            this.userPin_RadioButton.TabIndex = 3;
            this.userPin_RadioButton.Text = "Username && PIN";
            this.userPin_RadioButton.UseVisualStyleBackColor = true;
            this.userPin_RadioButton.CheckedChanged += new System.EventHandler(this.Authentication_RadioButton_CheckedChanged);
            // 
            // userPassword_RadioButton
            // 
            this.userPassword_RadioButton.AutoSize = true;
            this.userPassword_RadioButton.Checked = true;
            this.userPassword_RadioButton.Location = new System.Drawing.Point(28, 30);
            this.userPassword_RadioButton.Name = "userPassword_RadioButton";
            this.userPassword_RadioButton.Size = new System.Drawing.Size(131, 17);
            this.userPassword_RadioButton.TabIndex = 2;
            this.userPassword_RadioButton.TabStop = true;
            this.userPassword_RadioButton.Text = "Username && Password";
            this.userPassword_RadioButton.UseVisualStyleBackColor = true;
            this.userPassword_RadioButton.CheckedChanged += new System.EventHandler(this.Authentication_RadioButton_CheckedChanged);
            // 
            // pullAllInfo_Label
            // 
            this.pullAllInfo_Label.AutoSize = true;
            this.pullAllInfo_Label.Location = new System.Drawing.Point(237, 222);
            this.pullAllInfo_Label.Name = "pullAllInfo_Label";
            this.pullAllInfo_Label.Size = new System.Drawing.Size(313, 13);
            this.pullAllInfo_Label.TabIndex = 59;
            this.pullAllInfo_Label.Text = "(Pulls all documents found in the print queue during each activity)";
            // 
            // asset_Label
            // 
            this.asset_Label.AutoSize = true;
            this.asset_Label.Location = new System.Drawing.Point(75, 277);
            this.asset_Label.Name = "asset_Label";
            this.asset_Label.Size = new System.Drawing.Size(36, 13);
            this.asset_Label.TabIndex = 62;
            this.asset_Label.Text = "Asset:";
            // 
            // selectAsset_Button
            // 
            this.selectAsset_Button.Location = new System.Drawing.Point(370, 272);
            this.selectAsset_Button.Name = "selectAsset_Button";
            this.selectAsset_Button.Size = new System.Drawing.Size(38, 27);
            this.selectAsset_Button.TabIndex = 61;
            this.selectAsset_Button.Text = "...";
            this.selectAsset_Button.UseVisualStyleBackColor = true;
            this.selectAsset_Button.Click += new System.EventHandler(this.selectAsset_Button_Click);
            // 
            // asset_TextBox
            // 
            this.asset_TextBox.Location = new System.Drawing.Point(117, 274);
            this.asset_TextBox.Name = "asset_TextBox";
            this.asset_TextBox.ReadOnly = true;
            this.asset_TextBox.Size = new System.Drawing.Size(249, 20);
            this.asset_TextBox.TabIndex = 60;
            // 
            // safecom_ServerComboBox
            // 
            this.safecom_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.safecom_ServerComboBox.Location = new System.Drawing.Point(117, 3);
            this.safecom_ServerComboBox.Name = "safecom_ServerComboBox";
            this.safecom_ServerComboBox.Size = new System.Drawing.Size(418, 23);
            this.safecom_ServerComboBox.TabIndex = 58;
            // 
            // SafeComSimulationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.asset_Label);
            this.Controls.Add(this.selectAsset_Button);
            this.Controls.Add(this.asset_TextBox);
            this.Controls.Add(this.pullAllInfo_Label);
            this.Controls.Add(this.safecom_ServerComboBox);
            this.Controls.Add(this.printAll_CheckBox);
            this.Controls.Add(this.address_Label);
            this.Controls.Add(this.address_TextBox);
            this.Controls.Add(this.server_Label);
            this.Controls.Add(this.authentication_GroupBox);
            this.Name = "SafeComSimulationConfigControl";
            this.Size = new System.Drawing.Size(629, 316);
            this.authentication_GroupBox.ResumeLayout(false);
            this.authentication_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox printAll_CheckBox;
        private System.Windows.Forms.Label address_Label;
        private System.Windows.Forms.TextBox address_TextBox;
        private System.Windows.Forms.Label server_Label;
        private System.Windows.Forms.GroupBox authentication_GroupBox;
        private System.Windows.Forms.TextBox card_TextBox;
        private System.Windows.Forms.Label card_Label;
        private System.Windows.Forms.RadioButton windows_RadioButton;
        private System.Windows.Forms.RadioButton cardPin_RadioButton;
        private System.Windows.Forms.RadioButton userPin_RadioButton;
        private System.Windows.Forms.RadioButton userPassword_RadioButton;
        private Framework.UI.ServerComboBox safecom_ServerComboBox;
        private System.Windows.Forms.Label pullAllInfo_Label;
        private System.Windows.Forms.Label asset_Label;
        private System.Windows.Forms.Button selectAsset_Button;
        private System.Windows.Forms.TextBox asset_TextBox;
        private Framework.UI.FieldValidator fieldValidator;
    }
}
