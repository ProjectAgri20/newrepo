namespace HP.ScalableTest.Plugin.JetAdvantage
{
    partial class JetAdvantageConfigControl
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
            this.deviceId_Label = new System.Windows.Forms.Label();
            this.deviceId_TextBox = new System.Windows.Forms.TextBox();
            this.deviceSelection_Button = new System.Windows.Forms.Button();
            this.titanSolution_GroupBox = new System.Windows.Forms.GroupBox();
            this.scanSend_Button = new System.Windows.Forms.RadioButton();
            this.pullPrint_RadioButton = new System.Windows.Forms.RadioButton();
            this.oxpdPassword_textBox = new System.Windows.Forms.TextBox();
            this.printAll_CheckBox = new System.Windows.Forms.CheckBox();
            this.pluginDescription_Label = new System.Windows.Forms.Label();
            this.oxpdPassword_label = new System.Windows.Forms.Label();
            this.loginId_Label = new System.Windows.Forms.Label();
            this.titanLoginId_TextBox = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.titanPassword_TextBox = new System.Windows.Forms.TextBox();
            this.deleteAfterPrint_CheckBox = new System.Windows.Forms.CheckBox();
            this.loginPin_CheckBox = new System.Windows.Forms.CheckBox();
            this.loginPin_label = new System.Windows.Forms.Label();
            this.pinDescription_label = new System.Windows.Forms.Label();
            this.loginPin_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.titanSolution_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceId_Label
            // 
            this.deviceId_Label.AutoSize = true;
            this.deviceId_Label.Location = new System.Drawing.Point(15, 19);
            this.deviceId_Label.Name = "deviceId_Label";
            this.deviceId_Label.Size = new System.Drawing.Size(98, 13);
            this.deviceId_Label.TabIndex = 0;
            this.deviceId_Label.Text = "Physical Device Id:";
            // 
            // deviceId_TextBox
            // 
            this.deviceId_TextBox.BackColor = System.Drawing.SystemColors.Control;
            this.deviceId_TextBox.Location = new System.Drawing.Point(129, 16);
            this.deviceId_TextBox.Name = "deviceId_TextBox";
            this.deviceId_TextBox.Size = new System.Drawing.Size(298, 20);
            this.deviceId_TextBox.TabIndex = 1;
            // 
            // deviceSelection_Button
            // 
            this.deviceSelection_Button.Location = new System.Drawing.Point(433, 14);
            this.deviceSelection_Button.Name = "deviceSelection_Button";
            this.deviceSelection_Button.Size = new System.Drawing.Size(28, 23);
            this.deviceSelection_Button.TabIndex = 2;
            this.deviceSelection_Button.Text = "...";
            this.deviceSelection_Button.UseVisualStyleBackColor = true;
            this.deviceSelection_Button.Click += new System.EventHandler(this.deviceSelection_Button_Click);
            // 
            // titanSolution_GroupBox
            // 
            this.titanSolution_GroupBox.Controls.Add(this.scanSend_Button);
            this.titanSolution_GroupBox.Controls.Add(this.pullPrint_RadioButton);
            this.titanSolution_GroupBox.Location = new System.Drawing.Point(18, 52);
            this.titanSolution_GroupBox.Name = "titanSolution_GroupBox";
            this.titanSolution_GroupBox.Size = new System.Drawing.Size(443, 97);
            this.titanSolution_GroupBox.TabIndex = 3;
            this.titanSolution_GroupBox.TabStop = false;
            this.titanSolution_GroupBox.Text = "HP JetAdvantage (Titan)";
            // 
            // scanSend_Button
            // 
            this.scanSend_Button.AutoSize = true;
            this.scanSend_Button.Location = new System.Drawing.Point(19, 61);
            this.scanSend_Button.Name = "scanSend_Button";
            this.scanSend_Button.Size = new System.Drawing.Size(125, 17);
            this.scanSend_Button.TabIndex = 1;
            this.scanSend_Button.Text = "Scan && Send (Helios)";
            this.scanSend_Button.UseVisualStyleBackColor = true;
            this.scanSend_Button.Visible = false;
            // 
            // pullPrint_RadioButton
            // 
            this.pullPrint_RadioButton.AutoSize = true;
            this.pullPrint_RadioButton.Checked = true;
            this.pullPrint_RadioButton.Location = new System.Drawing.Point(19, 29);
            this.pullPrint_RadioButton.Name = "pullPrint_RadioButton";
            this.pullPrint_RadioButton.Size = new System.Drawing.Size(116, 17);
            this.pullPrint_RadioButton.TabIndex = 0;
            this.pullPrint_RadioButton.TabStop = true;
            this.pullPrint_RadioButton.Text = "HP Pull Print (Atlas)";
            this.pullPrint_RadioButton.UseVisualStyleBackColor = true;
            this.pullPrint_RadioButton.CheckedChanged += new System.EventHandler(this.jetAdvantage_RbtnCheckedChanged);
            // 
            // oxpdPassword_textBox
            // 
            this.oxpdPassword_textBox.Location = new System.Drawing.Point(105, 231);
            this.oxpdPassword_textBox.Name = "oxpdPassword_textBox";
            this.oxpdPassword_textBox.Size = new System.Drawing.Size(223, 20);
            this.oxpdPassword_textBox.TabIndex = 14;
            // 
            // printAll_CheckBox
            // 
            this.printAll_CheckBox.AutoSize = true;
            this.printAll_CheckBox.Location = new System.Drawing.Point(18, 155);
            this.printAll_CheckBox.Name = "printAll_CheckBox";
            this.printAll_CheckBox.Size = new System.Drawing.Size(118, 17);
            this.printAll_CheckBox.TabIndex = 13;
            this.printAll_CheckBox.Text = "Print All Documents";
            this.printAll_CheckBox.UseVisualStyleBackColor = true;
            this.printAll_CheckBox.CheckedChanged += new System.EventHandler(this.printAll_CheckBox_CheckedChanged);
            // 
            // pluginDescription_Label
            // 
            this.pluginDescription_Label.AutoSize = true;
            this.pluginDescription_Label.Location = new System.Drawing.Point(15, 175);
            this.pluginDescription_Label.Name = "pluginDescription_Label";
            this.pluginDescription_Label.Size = new System.Drawing.Size(409, 13);
            this.pluginDescription_Label.TabIndex = 11;
            this.pluginDescription_Label.Text = "This plugin will log into a pull print enabled device, select one print job and t" +
    "hen pull it.";
            // 
            // oxpdPassword_label
            // 
            this.oxpdPassword_label.AutoSize = true;
            this.oxpdPassword_label.Location = new System.Drawing.Point(15, 234);
            this.oxpdPassword_label.Name = "oxpdPassword_label";
            this.oxpdPassword_label.Size = new System.Drawing.Size(87, 13);
            this.oxpdPassword_label.TabIndex = 10;
            this.oxpdPassword_label.Text = "OXPd Password:";
            // 
            // loginId_Label
            // 
            this.loginId_Label.AutoSize = true;
            this.loginId_Label.Location = new System.Drawing.Point(27, 269);
            this.loginId_Label.Name = "loginId_Label";
            this.loginId_Label.Size = new System.Drawing.Size(75, 13);
            this.loginId_Label.TabIndex = 15;
            this.loginId_Label.Text = "Titan Login Id:";
            // 
            // titanLoginId_TextBox
            // 
            this.titanLoginId_TextBox.Location = new System.Drawing.Point(105, 265);
            this.titanLoginId_TextBox.Name = "titanLoginId_TextBox";
            this.titanLoginId_TextBox.Size = new System.Drawing.Size(223, 20);
            this.titanLoginId_TextBox.TabIndex = 16;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(19, 297);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(83, 13);
            this.password_label.TabIndex = 17;
            this.password_label.Text = "Titan Password:";
            // 
            // titanPassword_TextBox
            // 
            this.titanPassword_TextBox.Location = new System.Drawing.Point(105, 294);
            this.titanPassword_TextBox.Name = "titanPassword_TextBox";
            this.titanPassword_TextBox.Size = new System.Drawing.Size(223, 20);
            this.titanPassword_TextBox.TabIndex = 18;
            this.titanPassword_TextBox.Text = "16CycleCross";
            // 
            // deleteAfterPrint_CheckBox
            // 
            this.deleteAfterPrint_CheckBox.AutoSize = true;
            this.deleteAfterPrint_CheckBox.Checked = true;
            this.deleteAfterPrint_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteAfterPrint_CheckBox.Location = new System.Drawing.Point(226, 155);
            this.deleteAfterPrint_CheckBox.Name = "deleteAfterPrint_CheckBox";
            this.deleteAfterPrint_CheckBox.Size = new System.Drawing.Size(106, 17);
            this.deleteAfterPrint_CheckBox.TabIndex = 19;
            this.deleteAfterPrint_CheckBox.Text = "Delete After Print";
            this.deleteAfterPrint_CheckBox.UseVisualStyleBackColor = true;
            // 
            // loginPin_CheckBox
            // 
            this.loginPin_CheckBox.AutoSize = true;
            this.loginPin_CheckBox.Location = new System.Drawing.Point(353, 234);
            this.loginPin_CheckBox.Name = "loginPin_CheckBox";
            this.loginPin_CheckBox.Size = new System.Drawing.Size(95, 17);
            this.loginPin_CheckBox.TabIndex = 20;
            this.loginPin_CheckBox.Text = "Use Login PIN";
            this.loginPin_CheckBox.UseVisualStyleBackColor = true;
            // 
            // loginPin_label
            // 
            this.loginPin_label.AutoSize = true;
            this.loginPin_label.Location = new System.Drawing.Point(353, 268);
            this.loginPin_label.Name = "loginPin_label";
            this.loginPin_label.Size = new System.Drawing.Size(60, 13);
            this.loginPin_label.TabIndex = 21;
            this.loginPin_label.Text = "Login PIN :";
            // 
            // pinDescription_label
            // 
            this.pinDescription_label.AutoSize = true;
            this.pinDescription_label.Location = new System.Drawing.Point(355, 294);
            this.pinDescription_label.Name = "pinDescription_label";
            this.pinDescription_label.Size = new System.Drawing.Size(132, 13);
            this.pinDescription_label.TabIndex = 22;
            this.pinDescription_label.Text = "numeric(0-9), 8 chars Max.";
            // 
            // loginPin_TextBox
            // 
            this.loginPin_TextBox.Enabled = false;
            this.loginPin_TextBox.Location = new System.Drawing.Point(413, 266);
            this.loginPin_TextBox.MaxLength = 8;
            this.loginPin_TextBox.Name = "loginPin_TextBox";
            this.loginPin_TextBox.ShortcutsEnabled = false;
            this.loginPin_TextBox.Size = new System.Drawing.Size(100, 20);
            this.loginPin_TextBox.TabIndex = 23;
            this.loginPin_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.loginPin_TextBox_KeyPress);
            // 
            // JetAdvantageConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loginPin_TextBox);
            this.Controls.Add(this.pinDescription_label);
            this.Controls.Add(this.loginPin_label);
            this.Controls.Add(this.loginPin_CheckBox);
            this.Controls.Add(this.deleteAfterPrint_CheckBox);
            this.Controls.Add(this.titanPassword_TextBox);
            this.Controls.Add(this.password_label);
            this.Controls.Add(this.titanLoginId_TextBox);
            this.Controls.Add(this.loginId_Label);
            this.Controls.Add(this.oxpdPassword_textBox);
            this.Controls.Add(this.printAll_CheckBox);
            this.Controls.Add(this.pluginDescription_Label);
            this.Controls.Add(this.oxpdPassword_label);
            this.Controls.Add(this.titanSolution_GroupBox);
            this.Controls.Add(this.deviceSelection_Button);
            this.Controls.Add(this.deviceId_TextBox);
            this.Controls.Add(this.deviceId_Label);
            this.Name = "JetAdvantageConfigControl";
            this.Size = new System.Drawing.Size(585, 399);
            this.titanSolution_GroupBox.ResumeLayout(false);
            this.titanSolution_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label deviceId_Label;
        private System.Windows.Forms.TextBox deviceId_TextBox;
        private System.Windows.Forms.Button deviceSelection_Button;
        private System.Windows.Forms.GroupBox titanSolution_GroupBox;
        private System.Windows.Forms.RadioButton scanSend_Button;
        private System.Windows.Forms.RadioButton pullPrint_RadioButton;
        private System.Windows.Forms.TextBox oxpdPassword_textBox;
        private System.Windows.Forms.CheckBox printAll_CheckBox;
        private System.Windows.Forms.Label pluginDescription_Label;
        private System.Windows.Forms.Label oxpdPassword_label;
        private System.Windows.Forms.Label loginId_Label;
        private System.Windows.Forms.TextBox titanLoginId_TextBox;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox titanPassword_TextBox;
        private System.Windows.Forms.CheckBox deleteAfterPrint_CheckBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckBox loginPin_CheckBox;
        private System.Windows.Forms.Label loginPin_label;
        private System.Windows.Forms.Label pinDescription_label;
        private System.Windows.Forms.TextBox loginPin_TextBox;
    }
}
