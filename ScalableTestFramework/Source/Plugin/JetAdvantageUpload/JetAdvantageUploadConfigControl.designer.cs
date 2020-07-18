namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{

    /// <summary>
    /// 
    /// </summary>
    partial class JetAdvantageUploadConfigControl
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
            this.checkBoxShuffle = new System.Windows.Forms.CheckBox();
            this.titanPassword_TextBox = new System.Windows.Forms.TextBox();
            this.password_label = new System.Windows.Forms.Label();
            this.titanLoginId_TextBox = new System.Windows.Forms.TextBox();
            this.loginId_Label = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupCredential = new System.Windows.Forms.GroupBox();
            this.document_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.groupBoxServerInfo = new System.Windows.Forms.GroupBox();
            this.textBoxJetAdvantageProxy = new System.Windows.Forms.TextBox();
            this.textBoxJetAdvantageURL = new System.Windows.Forms.TextBox();
            this.label_JetProxy = new System.Windows.Forms.Label();
            this.label_JetURL = new System.Windows.Forms.Label();
            this.groupCredential.SuspendLayout();
            this.document_GroupBox.SuspendLayout();
            this.groupBoxServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxShuffle
            // 
            this.checkBoxShuffle.AutoSize = true;
            this.checkBoxShuffle.Location = new System.Drawing.Point(12, 98);
            this.checkBoxShuffle.Name = "checkBoxShuffle";
            this.checkBoxShuffle.Size = new System.Drawing.Size(196, 19);
            this.checkBoxShuffle.TabIndex = 58;
            this.checkBoxShuffle.Text = "Shuffle Document Upload Order";
            this.checkBoxShuffle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShuffle.UseVisualStyleBackColor = true;
            // 
            // titanPassword_TextBox
            // 
            this.titanPassword_TextBox.Location = new System.Drawing.Point(106, 50);
            this.titanPassword_TextBox.Name = "titanPassword_TextBox";
            this.titanPassword_TextBox.Size = new System.Drawing.Size(223, 23);
            this.titanPassword_TextBox.TabIndex = 57;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(9, 53);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(90, 15);
            this.password_label.TabIndex = 56;
            this.password_label.Text = "Titan Password:";
            // 
            // titanLoginId_TextBox
            // 
            this.titanLoginId_TextBox.Location = new System.Drawing.Point(106, 22);
            this.titanLoginId_TextBox.Name = "titanLoginId_TextBox";
            this.titanLoginId_TextBox.Size = new System.Drawing.Size(223, 23);
            this.titanLoginId_TextBox.TabIndex = 55;
            // 
            // loginId_Label
            // 
            this.loginId_Label.AutoSize = true;
            this.loginId_Label.Location = new System.Drawing.Point(9, 25);
            this.loginId_Label.Name = "loginId_Label";
            this.loginId_Label.Size = new System.Drawing.Size(83, 15);
            this.loginId_Label.TabIndex = 54;
            this.loginId_Label.Text = "Titan Login Id:";
            // 
            // groupCredential
            // 
            this.groupCredential.Controls.Add(this.titanPassword_TextBox);
            this.groupCredential.Controls.Add(this.titanLoginId_TextBox);
            this.groupCredential.Controls.Add(this.password_label);
            this.groupCredential.Controls.Add(this.loginId_Label);
            this.groupCredential.Location = new System.Drawing.Point(3, 3);
            this.groupCredential.Name = "groupCredential";
            this.groupCredential.Size = new System.Drawing.Size(349, 88);
            this.groupCredential.TabIndex = 59;
            this.groupCredential.TabStop = false;
            this.groupCredential.Text = "JetAdvantage Login Credential";
            // 
            // document_GroupBox
            // 
            this.document_GroupBox.Controls.Add(this.documentSelectionControl);
            this.document_GroupBox.Location = new System.Drawing.Point(3, 123);
            this.document_GroupBox.Name = "document_GroupBox";
            this.document_GroupBox.Size = new System.Drawing.Size(817, 304);
            this.document_GroupBox.TabIndex = 53;
            this.document_GroupBox.TabStop = false;
            this.document_GroupBox.Text = "Document Selection";
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(7, 16);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(804, 292);
            this.documentSelectionControl.TabIndex = 0;
            // 
            // groupBoxServerInfo
            // 
            this.groupBoxServerInfo.Controls.Add(this.textBoxJetAdvantageProxy);
            this.groupBoxServerInfo.Controls.Add(this.textBoxJetAdvantageURL);
            this.groupBoxServerInfo.Controls.Add(this.label_JetProxy);
            this.groupBoxServerInfo.Controls.Add(this.label_JetURL);
            this.groupBoxServerInfo.Location = new System.Drawing.Point(360, 3);
            this.groupBoxServerInfo.Name = "groupBoxServerInfo";
            this.groupBoxServerInfo.Size = new System.Drawing.Size(454, 88);
            this.groupBoxServerInfo.TabIndex = 60;
            this.groupBoxServerInfo.TabStop = false;
            this.groupBoxServerInfo.Text = "JetAdvantage Server Information";
            // 
            // textBoxJetAdvantageProxy
            // 
            this.textBoxJetAdvantageProxy.Location = new System.Drawing.Point(81, 53);
            this.textBoxJetAdvantageProxy.Name = "textBoxJetAdvantageProxy";
            this.textBoxJetAdvantageProxy.Size = new System.Drawing.Size(310, 23);
            this.textBoxJetAdvantageProxy.TabIndex = 58;
            this.textBoxJetAdvantageProxy.Text = "https://mfp.hpbizapps.com";
            // 
            // textBoxJetAdvantageURL
            // 
            this.textBoxJetAdvantageURL.Location = new System.Drawing.Point(81, 22);
            this.textBoxJetAdvantageURL.Name = "textBoxJetAdvantageURL";
            this.textBoxJetAdvantageURL.Size = new System.Drawing.Size(310, 23);
            this.textBoxJetAdvantageURL.TabIndex = 57;
            this.textBoxJetAdvantageURL.Text = "https://pp.hpondemand.com";
            // 
            // label_JetProxy
            // 
            this.label_JetProxy.AutoSize = true;
            this.label_JetProxy.Location = new System.Drawing.Point(6, 53);
            this.label_JetProxy.Name = "label_JetProxy";
            this.label_JetProxy.Size = new System.Drawing.Size(69, 15);
            this.label_JetProxy.TabIndex = 56;
            this.label_JetProxy.Text = "Titan Proxy:";
            // 
            // label_JetURL
            // 
            this.label_JetURL.AutoSize = true;
            this.label_JetURL.Location = new System.Drawing.Point(14, 25);
            this.label_JetURL.Name = "label_JetURL";
            this.label_JetURL.Size = new System.Drawing.Size(61, 15);
            this.label_JetURL.TabIndex = 55;
            this.label_JetURL.Text = "Titan URL:";
            // 
            // JetAdvantageUploadConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxServerInfo);
            this.Controls.Add(this.document_GroupBox);
            this.Controls.Add(this.checkBoxShuffle);
            this.Controls.Add(this.groupCredential);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "JetAdvantageUploadConfigControl";
            this.Size = new System.Drawing.Size(827, 438);
            this.groupCredential.ResumeLayout(false);
            this.groupCredential.PerformLayout();
            this.document_GroupBox.ResumeLayout(false);
            this.groupBoxServerInfo.ResumeLayout(false);
            this.groupBoxServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox titanPassword_TextBox;
        private System.Windows.Forms.Label password_label;
        private System.Windows.Forms.TextBox titanLoginId_TextBox;
        private System.Windows.Forms.Label loginId_Label;
        private System.Windows.Forms.CheckBox checkBoxShuffle;
        private System.Windows.Forms.GroupBox groupCredential;
        private System.Windows.Forms.GroupBox document_GroupBox;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBoxServerInfo;
        private System.Windows.Forms.TextBox textBoxJetAdvantageProxy;
        private System.Windows.Forms.TextBox textBoxJetAdvantageURL;
        private System.Windows.Forms.Label label_JetProxy;
        private System.Windows.Forms.Label label_JetURL;
    }
}
