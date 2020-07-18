namespace HP.ScalableTest.Plugin.EPrint
{
    partial class EPrintConfigControl
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
            this.documentCount_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.docmentsToAttach_Label = new System.Windows.Forms.Label();
            this.attachSome_RadioButton = new System.Windows.Forms.RadioButton();
            this.attachAll_RadioButton = new System.Windows.Forms.RadioButton();
            this.maxDocumentSize_Label = new System.Windows.Forms.Label();
            this.device_TextBox = new System.Windows.Forms.TextBox();
            this.ePrintServer_Label = new System.Windows.Forms.Label();
            this.email_ComboBox = new System.Windows.Forms.ComboBox();
            this.email_Label = new System.Windows.Forms.Label();
            this.device_Label = new System.Windows.Forms.Label();
            this.attachment_Label = new System.Windows.Forms.Label();
            this.ePrint_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.documentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.exchange_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.exchangeServer_Label = new System.Windows.Forms.Label();
            this.documentCount_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // documentCount_GroupBox
            // 
            this.documentCount_GroupBox.Controls.Add(this.documentCount_NumericUpDown);
            this.documentCount_GroupBox.Controls.Add(this.docmentsToAttach_Label);
            this.documentCount_GroupBox.Controls.Add(this.attachSome_RadioButton);
            this.documentCount_GroupBox.Controls.Add(this.attachAll_RadioButton);
            this.documentCount_GroupBox.Location = new System.Drawing.Point(441, 12);
            this.documentCount_GroupBox.Name = "documentCount_GroupBox";
            this.documentCount_GroupBox.Size = new System.Drawing.Size(240, 101);
            this.documentCount_GroupBox.TabIndex = 33;
            this.documentCount_GroupBox.TabStop = false;
            this.documentCount_GroupBox.Text = "Document Count";
            // 
            // documentCount_NumericUpDown
            // 
            this.documentCount_NumericUpDown.Location = new System.Drawing.Point(78, 63);
            this.documentCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.documentCount_NumericUpDown.Name = "documentCount_NumericUpDown";
            this.documentCount_NumericUpDown.Size = new System.Drawing.Size(46, 20);
            this.documentCount_NumericUpDown.TabIndex = 22;
            this.documentCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // docmentsToAttach_Label
            // 
            this.docmentsToAttach_Label.AutoSize = true;
            this.docmentsToAttach_Label.Location = new System.Drawing.Point(130, 65);
            this.docmentsToAttach_Label.Name = "docmentsToAttach_Label";
            this.docmentsToAttach_Label.Size = new System.Drawing.Size(104, 13);
            this.docmentsToAttach_Label.TabIndex = 2;
            this.docmentsToAttach_Label.Text = "documents to attach";
            // 
            // attachSome_RadioButton
            // 
            this.attachSome_RadioButton.AutoSize = true;
            this.attachSome_RadioButton.Checked = true;
            this.attachSome_RadioButton.Location = new System.Drawing.Point(22, 65);
            this.attachSome_RadioButton.Name = "attachSome_RadioButton";
            this.attachSome_RadioButton.Size = new System.Drawing.Size(55, 17);
            this.attachSome_RadioButton.TabIndex = 1;
            this.attachSome_RadioButton.TabStop = true;
            this.attachSome_RadioButton.Text = "Select";
            this.attachSome_RadioButton.UseVisualStyleBackColor = true;
            // 
            // attachAll_RadioButton
            // 
            this.attachAll_RadioButton.AutoSize = true;
            this.attachAll_RadioButton.Location = new System.Drawing.Point(22, 32);
            this.attachAll_RadioButton.Name = "attachAll_RadioButton";
            this.attachAll_RadioButton.Size = new System.Drawing.Size(124, 17);
            this.attachAll_RadioButton.TabIndex = 0;
            this.attachAll_RadioButton.Text = "Attach all documents";
            this.attachAll_RadioButton.UseVisualStyleBackColor = true;
            // 
            // maxDocumentSize_Label
            // 
            this.maxDocumentSize_Label.Location = new System.Drawing.Point(119, 133);
            this.maxDocumentSize_Label.Name = "maxDocumentSize_Label";
            this.maxDocumentSize_Label.Size = new System.Drawing.Size(320, 20);
            this.maxDocumentSize_Label.TabIndex = 23;
            this.maxDocumentSize_Label.Text = "(The cumulative attachment size should not exceed 10MB)";
            // 
            // device_TextBox
            // 
            this.device_TextBox.Location = new System.Drawing.Point(122, 101);
            this.device_TextBox.Name = "device_TextBox";
            this.device_TextBox.ReadOnly = true;
            this.device_TextBox.Size = new System.Drawing.Size(308, 20);
            this.device_TextBox.TabIndex = 32;
            // 
            // ePrintServer_Label
            // 
            this.ePrintServer_Label.AutoSize = true;
            this.ePrintServer_Label.Location = new System.Drawing.Point(10, 46);
            this.ePrintServer_Label.Name = "ePrintServer_Label";
            this.ePrintServer_Label.Size = new System.Drawing.Size(71, 13);
            this.ePrintServer_Label.TabIndex = 28;
            this.ePrintServer_Label.Text = "ePrint Server:";
            // 
            // email_ComboBox
            // 
            this.email_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.email_ComboBox.FormattingEnabled = true;
            this.email_ComboBox.Location = new System.Drawing.Point(122, 69);
            this.email_ComboBox.Name = "email_ComboBox";
            this.email_ComboBox.Size = new System.Drawing.Size(308, 21);
            this.email_ComboBox.TabIndex = 31;
            // 
            // email_Label
            // 
            this.email_Label.AutoSize = true;
            this.email_Label.Location = new System.Drawing.Point(11, 72);
            this.email_Label.Name = "email_Label";
            this.email_Label.Size = new System.Drawing.Size(72, 13);
            this.email_Label.TabIndex = 29;
            this.email_Label.Text = "Device Email:";
            // 
            // device_Label
            // 
            this.device_Label.AutoSize = true;
            this.device_Label.Location = new System.Drawing.Point(10, 104);
            this.device_Label.Name = "device_Label";
            this.device_Label.Size = new System.Drawing.Size(85, 13);
            this.device_Label.TabIndex = 30;
            this.device_Label.Text = "Prints to Device:";
            // 
            // attachment_Label
            // 
            this.attachment_Label.AutoSize = true;
            this.attachment_Label.Location = new System.Drawing.Point(10, 133);
            this.attachment_Label.Name = "attachment_Label";
            this.attachment_Label.Size = new System.Drawing.Size(69, 13);
            this.attachment_Label.TabIndex = 36;
            this.attachment_Label.Text = "Attachments:";
            // 
            // ePrint_ServerComboBox
            // 
            this.ePrint_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ePrint_ServerComboBox.Location = new System.Drawing.Point(122, 41);
            this.ePrint_ServerComboBox.Name = "ePrint_ServerComboBox";
            this.ePrint_ServerComboBox.Size = new System.Drawing.Size(308, 23);
            this.ePrint_ServerComboBox.TabIndex = 35;
            // 
            // documentSelectionControl
            // 
            this.documentSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentSelectionControl.Location = new System.Drawing.Point(13, 151);
            this.documentSelectionControl.Name = "documentSelectionControl";
            this.documentSelectionControl.ShowDocumentBrowseControl = true;
            this.documentSelectionControl.ShowDocumentQueryControl = true;
            this.documentSelectionControl.ShowDocumentSetControl = true;
            this.documentSelectionControl.Size = new System.Drawing.Size(668, 217);
            this.documentSelectionControl.TabIndex = 34;
            // 
            // exchange_ServerComboBox
            // 
            this.exchange_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exchange_ServerComboBox.Location = new System.Drawing.Point(122, 12);
            this.exchange_ServerComboBox.Name = "exchange_ServerComboBox";
            this.exchange_ServerComboBox.Size = new System.Drawing.Size(308, 23);
            this.exchange_ServerComboBox.TabIndex = 37;
            // 
            // exchangeServer_Label
            // 
            this.exchangeServer_Label.AutoSize = true;
            this.exchangeServer_Label.Location = new System.Drawing.Point(11, 18);
            this.exchangeServer_Label.Name = "exchangeServer_Label";
            this.exchangeServer_Label.Size = new System.Drawing.Size(92, 13);
            this.exchangeServer_Label.TabIndex = 38;
            this.exchangeServer_Label.Text = "Exchange Server:";
            // 
            // EPrintConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exchangeServer_Label);
            this.Controls.Add(this.exchange_ServerComboBox);
            this.Controls.Add(this.attachment_Label);
            this.Controls.Add(this.ePrint_ServerComboBox);
            this.Controls.Add(this.maxDocumentSize_Label);
            this.Controls.Add(this.documentSelectionControl);
            this.Controls.Add(this.documentCount_GroupBox);
            this.Controls.Add(this.device_TextBox);
            this.Controls.Add(this.ePrintServer_Label);
            this.Controls.Add(this.email_ComboBox);
            this.Controls.Add(this.email_Label);
            this.Controls.Add(this.device_Label);
            this.Name = "EPrintConfigControl";
            this.Size = new System.Drawing.Size(690, 373);
            this.documentCount_GroupBox.ResumeLayout(false);
            this.documentCount_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox documentCount_GroupBox;
        private System.Windows.Forms.Label maxDocumentSize_Label;
        private System.Windows.Forms.NumericUpDown documentCount_NumericUpDown;
        private System.Windows.Forms.Label docmentsToAttach_Label;
        private System.Windows.Forms.RadioButton attachSome_RadioButton;
        private System.Windows.Forms.RadioButton attachAll_RadioButton;
        private System.Windows.Forms.TextBox device_TextBox;
        private System.Windows.Forms.Label ePrintServer_Label;
        private System.Windows.Forms.ComboBox email_ComboBox;
        private System.Windows.Forms.Label email_Label;
        private System.Windows.Forms.Label device_Label;
        private Framework.UI.DocumentSelectionControl documentSelectionControl;
        private Framework.UI.ServerComboBox ePrint_ServerComboBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label attachment_Label;
        private Framework.UI.ServerComboBox exchange_ServerComboBox;
        private System.Windows.Forms.Label exchangeServer_Label;
    }
}
