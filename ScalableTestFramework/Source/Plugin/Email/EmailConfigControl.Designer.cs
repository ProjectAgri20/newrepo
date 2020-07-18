namespace HP.ScalableTest.Plugin.Email
{
    partial class EmailConfigControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.emailSetup_TabPage = new System.Windows.Forms.TabPage();
            this.ccCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.toCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ccCount_Label = new System.Windows.Forms.Label();
            this.toCount_Label = new System.Windows.Forms.Label();
            this.body_TextBox = new System.Windows.Forms.TextBox();
            this.subject_TextBox = new System.Windows.Forms.TextBox();
            this.subject_Label = new System.Windows.Forms.Label();
            this.attachmentSelection_TabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maxDocumentSize_Label = new System.Windows.Forms.Label();
            this.documentCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.attachSome_RadioButton = new System.Windows.Forms.RadioButton();
            this.attachAll_RadioButton = new System.Windows.Forms.RadioButton();
            this.attachments_DocumentSelectionControl = new HP.ScalableTest.Framework.UI.DocumentSelectionControl();
            this.exchangeServer_Label = new System.Windows.Forms.Label();
            this.exchange_ServerComboBox = new HP.ScalableTest.Framework.UI.ServerComboBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl.SuspendLayout();
            this.emailSetup_TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ccCount_NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toCount_NumericUpDown)).BeginInit();
            this.attachmentSelection_TabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.emailSetup_TabPage);
            this.tabControl.Controls.Add(this.attachmentSelection_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 500);
            this.tabControl.TabIndex = 18;
            // 
            // emailSetup_TabPage
            // 
            this.emailSetup_TabPage.Controls.Add(this.exchangeServer_Label);
            this.emailSetup_TabPage.Controls.Add(this.exchange_ServerComboBox);
            this.emailSetup_TabPage.Controls.Add(this.ccCount_NumericUpDown);
            this.emailSetup_TabPage.Controls.Add(this.toCount_NumericUpDown);
            this.emailSetup_TabPage.Controls.Add(this.ccCount_Label);
            this.emailSetup_TabPage.Controls.Add(this.toCount_Label);
            this.emailSetup_TabPage.Controls.Add(this.body_TextBox);
            this.emailSetup_TabPage.Controls.Add(this.subject_TextBox);
            this.emailSetup_TabPage.Controls.Add(this.subject_Label);
            this.emailSetup_TabPage.Location = new System.Drawing.Point(4, 24);
            this.emailSetup_TabPage.Name = "emailSetup_TabPage";
            this.emailSetup_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.emailSetup_TabPage.Size = new System.Drawing.Size(667, 472);
            this.emailSetup_TabPage.TabIndex = 0;
            this.emailSetup_TabPage.Text = "Email Setup";
            // 
            // ccCount_NumericUpDown
            // 
            this.ccCount_NumericUpDown.Location = new System.Drawing.Point(428, 53);
            this.ccCount_NumericUpDown.Name = "ccCount_NumericUpDown";
            this.ccCount_NumericUpDown.Size = new System.Drawing.Size(57, 23);
            this.ccCount_NumericUpDown.TabIndex = 26;
            // 
            // toCount_NumericUpDown
            // 
            this.toCount_NumericUpDown.Location = new System.Drawing.Point(177, 50);
            this.toCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.toCount_NumericUpDown.Name = "toCount_NumericUpDown";
            this.toCount_NumericUpDown.Size = new System.Drawing.Size(57, 23);
            this.toCount_NumericUpDown.TabIndex = 26;
            this.toCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ccCount_Label
            // 
            this.ccCount_Label.Location = new System.Drawing.Point(241, 52);
            this.ccCount_Label.Name = "ccCount_Label";
            this.ccCount_Label.Size = new System.Drawing.Size(181, 20);
            this.ccCount_Label.TabIndex = 21;
            this.ccCount_Label.Text = "Number of \"Cc\" recipients:";
            this.ccCount_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toCount_Label
            // 
            this.toCount_Label.Location = new System.Drawing.Point(14, 52);
            this.toCount_Label.Name = "toCount_Label";
            this.toCount_Label.Size = new System.Drawing.Size(158, 20);
            this.toCount_Label.TabIndex = 18;
            this.toCount_Label.Text = "Number of \"To\" recipients:";
            this.toCount_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // body_TextBox
            // 
            this.body_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.body_TextBox.Location = new System.Drawing.Point(77, 124);
            this.body_TextBox.Multiline = true;
            this.body_TextBox.Name = "body_TextBox";
            this.body_TextBox.Size = new System.Drawing.Size(570, 321);
            this.body_TextBox.TabIndex = 20;
            // 
            // subject_TextBox
            // 
            this.subject_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subject_TextBox.Location = new System.Drawing.Point(77, 91);
            this.subject_TextBox.Name = "subject_TextBox";
            this.subject_TextBox.Size = new System.Drawing.Size(570, 23);
            this.subject_TextBox.TabIndex = 19;
            // 
            // subject_Label
            // 
            this.subject_Label.Location = new System.Drawing.Point(13, 94);
            this.subject_Label.Name = "subject_Label";
            this.subject_Label.Size = new System.Drawing.Size(58, 20);
            this.subject_Label.TabIndex = 25;
            this.subject_Label.Text = "Subject";
            this.subject_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // attachmentSelection_TabPage
            // 
            this.attachmentSelection_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.attachmentSelection_TabPage.Controls.Add(this.attachments_DocumentSelectionControl);
            this.attachmentSelection_TabPage.Controls.Add(this.panel1);
            this.attachmentSelection_TabPage.Location = new System.Drawing.Point(4, 24);
            this.attachmentSelection_TabPage.Name = "attachmentSelection_TabPage";
            this.attachmentSelection_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.attachmentSelection_TabPage.Size = new System.Drawing.Size(667, 472);
            this.attachmentSelection_TabPage.TabIndex = 1;
            this.attachmentSelection_TabPage.Text = "Attachment Selection";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 131);
            this.panel1.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maxDocumentSize_Label);
            this.groupBox1.Controls.Add(this.documentCount_NumericUpDown);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.attachSome_RadioButton);
            this.groupBox1.Controls.Add(this.attachAll_RadioButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 123);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Count";
            // 
            // maxDocumentSize_Label
            // 
            this.maxDocumentSize_Label.Location = new System.Drawing.Point(6, 75);
            this.maxDocumentSize_Label.Name = "maxDocumentSize_Label";
            this.maxDocumentSize_Label.Size = new System.Drawing.Size(283, 45);
            this.maxDocumentSize_Label.TabIndex = 24;
            this.maxDocumentSize_Label.Text = "Note: The cumulative attachment size should not exceed 10MB";
            // 
            // documentCount_NumericUpDown
            // 
            this.documentCount_NumericUpDown.Location = new System.Drawing.Point(121, 45);
            this.documentCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.documentCount_NumericUpDown.Name = "documentCount_NumericUpDown";
            this.documentCount_NumericUpDown.Size = new System.Drawing.Size(68, 23);
            this.documentCount_NumericUpDown.TabIndex = 22;
            this.documentCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(195, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "documents to attach";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // attachSome_RadioButton
            // 
            this.attachSome_RadioButton.Location = new System.Drawing.Point(6, 45);
            this.attachSome_RadioButton.Name = "attachSome_RadioButton";
            this.attachSome_RadioButton.Size = new System.Drawing.Size(109, 22);
            this.attachSome_RadioButton.TabIndex = 1;
            this.attachSome_RadioButton.Text = "Select up to";
            this.attachSome_RadioButton.UseVisualStyleBackColor = true;
            // 
            // attachAll_RadioButton
            // 
            this.attachAll_RadioButton.AutoSize = true;
            this.attachAll_RadioButton.Checked = true;
            this.attachAll_RadioButton.Location = new System.Drawing.Point(6, 20);
            this.attachAll_RadioButton.Name = "attachAll_RadioButton";
            this.attachAll_RadioButton.Size = new System.Drawing.Size(138, 19);
            this.attachAll_RadioButton.TabIndex = 0;
            this.attachAll_RadioButton.TabStop = true;
            this.attachAll_RadioButton.Text = "Attach all documents";
            this.attachAll_RadioButton.UseVisualStyleBackColor = true;
            // 
            // attachments_DocumentSelectionControl
            // 
            this.attachments_DocumentSelectionControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.attachments_DocumentSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.attachments_DocumentSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attachments_DocumentSelectionControl.Location = new System.Drawing.Point(3, 134);
            this.attachments_DocumentSelectionControl.Name = "attachments_DocumentSelectionControl";
            this.attachments_DocumentSelectionControl.Size = new System.Drawing.Size(661, 335);
            this.attachments_DocumentSelectionControl.TabIndex = 22;
            // 
            // exchangeServer_Label
            // 
            this.exchangeServer_Label.AutoSize = true;
            this.exchangeServer_Label.Location = new System.Drawing.Point(14, 22);
            this.exchangeServer_Label.Name = "exchangeServer_Label";
            this.exchangeServer_Label.Size = new System.Drawing.Size(95, 15);
            this.exchangeServer_Label.TabIndex = 40;
            this.exchangeServer_Label.Text = "Exchange Server:";
            // 
            // exchange_ServerComboBox
            // 
            this.exchange_ServerComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exchange_ServerComboBox.Location = new System.Drawing.Point(177, 16);
            this.exchange_ServerComboBox.Name = "exchange_ServerComboBox";
            this.exchange_ServerComboBox.Size = new System.Drawing.Size(308, 23);
            this.exchange_ServerComboBox.TabIndex = 39;
            // 
            // EmailConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EmailConfigControl";
            this.Size = new System.Drawing.Size(675, 500);
            this.tabControl.ResumeLayout(false);
            this.emailSetup_TabPage.ResumeLayout(false);
            this.emailSetup_TabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ccCount_NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toCount_NumericUpDown)).EndInit();
            this.attachmentSelection_TabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage emailSetup_TabPage;
        private System.Windows.Forms.Label ccCount_Label;
        private System.Windows.Forms.Label toCount_Label;
        private System.Windows.Forms.TextBox body_TextBox;
        private System.Windows.Forms.TextBox subject_TextBox;
        private System.Windows.Forms.Label subject_Label;
        private System.Windows.Forms.TabPage attachmentSelection_TabPage;
        private Framework.UI.DocumentSelectionControl attachments_DocumentSelectionControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton attachAll_RadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton attachSome_RadioButton;
        private System.Windows.Forms.NumericUpDown documentCount_NumericUpDown;
        private System.Windows.Forms.NumericUpDown ccCount_NumericUpDown;
        private System.Windows.Forms.NumericUpDown toCount_NumericUpDown;
        private System.Windows.Forms.Label maxDocumentSize_Label;
        private System.Windows.Forms.Label exchangeServer_Label;
        private Framework.UI.ServerComboBox exchange_ServerComboBox;
        private Framework.UI.FieldValidator fieldValidator;
    }
}
