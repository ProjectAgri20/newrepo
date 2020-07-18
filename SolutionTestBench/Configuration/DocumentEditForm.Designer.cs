namespace HP.ScalableTest
{
    partial class DocumentEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentEditForm));
            this.assetIdLabel = new System.Windows.Forms.Label();
            this.manufacturerLabel = new System.Windows.Forms.Label();
            this.fileTypeComboBox = new System.Windows.Forms.ComboBox();
            this.notesLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.applicationLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.fileSizeTextBox = new System.Windows.Forms.TextBox();
            this.fileSizeLabel = new System.Windows.Forms.Label();
            this.pageCountTextBox = new System.Windows.Forms.TextBox();
            this.pageCountLabel = new System.Windows.Forms.Label();
            this.applicationComboBox = new System.Windows.Forms.ComboBox();
            this.appVersionComboBox = new System.Windows.Forms.ComboBox();
            this.appVersionLabel = new System.Windows.Forms.Label();
            this.colorColorModeRadioButton = new System.Windows.Forms.RadioButton();
            this.monoColorModeRadioButton = new System.Windows.Forms.RadioButton();
            this.portraitRadioButton = new System.Windows.Forms.RadioButton();
            this.landScapeRadioButton = new System.Windows.Forms.RadioButton();
            this.fileSizeUnitLabel = new System.Windows.Forms.Label();
            this.colorModeGroupBox = new System.Windows.Forms.GroupBox();
            this.orientationGroupBox = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tag_label = new System.Windows.Forms.Label();
            this.tag_comboBox = new System.Windows.Forms.ComboBox();
            this.colorModeGroupBox.SuspendLayout();
            this.orientationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetIdLabel
            // 
            this.assetIdLabel.Location = new System.Drawing.Point(28, 32);
            this.assetIdLabel.Name = "assetIdLabel";
            this.assetIdLabel.Size = new System.Drawing.Size(93, 24);
            this.assetIdLabel.TabIndex = 0;
            this.assetIdLabel.Text = "File Name";
            this.assetIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // manufacturerLabel
            // 
            this.manufacturerLabel.Location = new System.Drawing.Point(22, 62);
            this.manufacturerLabel.Name = "manufacturerLabel";
            this.manufacturerLabel.Size = new System.Drawing.Size(99, 24);
            this.manufacturerLabel.TabIndex = 2;
            this.manufacturerLabel.Text = "File Type";
            this.manufacturerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fileTypeComboBox
            // 
            this.fileTypeComboBox.CausesValidation = false;
            this.fileTypeComboBox.FormattingEnabled = true;
            this.fileTypeComboBox.Location = new System.Drawing.Point(127, 62);
            this.fileTypeComboBox.Name = "fileTypeComboBox";
            this.fileTypeComboBox.Size = new System.Drawing.Size(279, 23);
            this.fileTypeComboBox.TabIndex = 3;
            // 
            // notesLabel
            // 
            this.notesLabel.Location = new System.Drawing.Point(25, 410);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(96, 24);
            this.notesLabel.TabIndex = 15;
            this.notesLabel.Text = "Notes";
            this.notesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notesTextBox
            // 
            this.notesTextBox.CausesValidation = false;
            this.notesTextBox.Location = new System.Drawing.Point(127, 412);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(334, 85);
            this.notesTextBox.TabIndex = 16;
            // 
            // applicationLabel
            // 
            this.applicationLabel.Location = new System.Drawing.Point(-1, 304);
            this.applicationLabel.Name = "applicationLabel";
            this.applicationLabel.Size = new System.Drawing.Size(122, 24);
            this.applicationLabel.TabIndex = 11;
            this.applicationLabel.Text = "App Name";
            this.applicationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(361, 505);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(255, 505);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 17;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(127, 32);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.ReadOnly = true;
            this.fileNameTextBox.Size = new System.Drawing.Size(334, 23);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // fileSizeTextBox
            // 
            this.fileSizeTextBox.Location = new System.Drawing.Point(127, 94);
            this.fileSizeTextBox.Name = "fileSizeTextBox";
            this.fileSizeTextBox.Size = new System.Drawing.Size(117, 23);
            this.fileSizeTextBox.TabIndex = 5;
            this.fileSizeTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.fileSizeTextBox_Validating);
            // 
            // fileSizeLabel
            // 
            this.fileSizeLabel.Location = new System.Drawing.Point(18, 94);
            this.fileSizeLabel.Name = "fileSizeLabel";
            this.fileSizeLabel.Size = new System.Drawing.Size(106, 24);
            this.fileSizeLabel.TabIndex = 4;
            this.fileSizeLabel.Text = "File Size";
            this.fileSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pageCountTextBox
            // 
            this.pageCountTextBox.Location = new System.Drawing.Point(127, 124);
            this.pageCountTextBox.Name = "pageCountTextBox";
            this.pageCountTextBox.Size = new System.Drawing.Size(66, 23);
            this.pageCountTextBox.TabIndex = 8;
            this.pageCountTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.pageCountTextBox_Validating);
            // 
            // pageCountLabel
            // 
            this.pageCountLabel.Location = new System.Drawing.Point(15, 124);
            this.pageCountLabel.Name = "pageCountLabel";
            this.pageCountLabel.Size = new System.Drawing.Size(106, 24);
            this.pageCountLabel.TabIndex = 7;
            this.pageCountLabel.Text = "Page Count";
            this.pageCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // applicationComboBox
            // 
            this.applicationComboBox.CausesValidation = false;
            this.applicationComboBox.FormattingEnabled = true;
            this.applicationComboBox.Location = new System.Drawing.Point(127, 304);
            this.applicationComboBox.Name = "applicationComboBox";
            this.applicationComboBox.Size = new System.Drawing.Size(334, 23);
            this.applicationComboBox.TabIndex = 12;
            // 
            // appVersionComboBox
            // 
            this.appVersionComboBox.CausesValidation = false;
            this.appVersionComboBox.FormattingEnabled = true;
            this.appVersionComboBox.Location = new System.Drawing.Point(127, 336);
            this.appVersionComboBox.Name = "appVersionComboBox";
            this.appVersionComboBox.Size = new System.Drawing.Size(334, 23);
            this.appVersionComboBox.TabIndex = 14;
            // 
            // appVersionLabel
            // 
            this.appVersionLabel.Location = new System.Drawing.Point(-1, 336);
            this.appVersionLabel.Name = "appVersionLabel";
            this.appVersionLabel.Size = new System.Drawing.Size(122, 24);
            this.appVersionLabel.TabIndex = 13;
            this.appVersionLabel.Text = "App Version";
            this.appVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // colorColorModeRadioButton
            // 
            this.colorColorModeRadioButton.AutoSize = true;
            this.colorColorModeRadioButton.Location = new System.Drawing.Point(128, 32);
            this.colorColorModeRadioButton.Name = "colorColorModeRadioButton";
            this.colorColorModeRadioButton.Size = new System.Drawing.Size(54, 19);
            this.colorColorModeRadioButton.TabIndex = 1;
            this.colorColorModeRadioButton.Text = "Color";
            this.colorColorModeRadioButton.UseVisualStyleBackColor = true;
            this.colorColorModeRadioButton.CheckedChanged += new System.EventHandler(this.colorColorModeRadioButton_CheckedChanged);
            // 
            // monoColorModeRadioButton
            // 
            this.monoColorModeRadioButton.AutoSize = true;
            this.monoColorModeRadioButton.Checked = true;
            this.monoColorModeRadioButton.Location = new System.Drawing.Point(34, 32);
            this.monoColorModeRadioButton.Name = "monoColorModeRadioButton";
            this.monoColorModeRadioButton.Size = new System.Drawing.Size(57, 19);
            this.monoColorModeRadioButton.TabIndex = 0;
            this.monoColorModeRadioButton.TabStop = true;
            this.monoColorModeRadioButton.Text = "Mono";
            this.monoColorModeRadioButton.UseVisualStyleBackColor = true;
            this.monoColorModeRadioButton.CheckedChanged += new System.EventHandler(this.monoColorModeRadioButton_CheckedChanged);
            // 
            // portraitRadioButton
            // 
            this.portraitRadioButton.AutoSize = true;
            this.portraitRadioButton.Checked = true;
            this.portraitRadioButton.Location = new System.Drawing.Point(34, 32);
            this.portraitRadioButton.Name = "portraitRadioButton";
            this.portraitRadioButton.Size = new System.Drawing.Size(64, 19);
            this.portraitRadioButton.TabIndex = 0;
            this.portraitRadioButton.TabStop = true;
            this.portraitRadioButton.Text = "Portrait";
            this.portraitRadioButton.UseVisualStyleBackColor = true;
            this.portraitRadioButton.CheckedChanged += new System.EventHandler(this.portraitRadioButton_CheckedChanged);
            // 
            // landScapeRadioButton
            // 
            this.landScapeRadioButton.AutoSize = true;
            this.landScapeRadioButton.Location = new System.Drawing.Point(128, 32);
            this.landScapeRadioButton.Name = "landScapeRadioButton";
            this.landScapeRadioButton.Size = new System.Drawing.Size(81, 19);
            this.landScapeRadioButton.TabIndex = 1;
            this.landScapeRadioButton.Text = "Landscape";
            this.landScapeRadioButton.UseVisualStyleBackColor = true;
            this.landScapeRadioButton.CheckedChanged += new System.EventHandler(this.landScapeRadioButton_CheckedChanged);
            // 
            // fileSizeUnitLabel
            // 
            this.fileSizeUnitLabel.Location = new System.Drawing.Point(250, 94);
            this.fileSizeUnitLabel.Name = "fileSizeUnitLabel";
            this.fileSizeUnitLabel.Size = new System.Drawing.Size(98, 24);
            this.fileSizeUnitLabel.TabIndex = 6;
            this.fileSizeUnitLabel.Text = "in Bytes";
            this.fileSizeUnitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // colorModeGroupBox
            // 
            this.colorModeGroupBox.Controls.Add(this.colorColorModeRadioButton);
            this.colorModeGroupBox.Controls.Add(this.monoColorModeRadioButton);
            this.colorModeGroupBox.Location = new System.Drawing.Point(127, 154);
            this.colorModeGroupBox.Name = "colorModeGroupBox";
            this.colorModeGroupBox.Size = new System.Drawing.Size(279, 69);
            this.colorModeGroupBox.TabIndex = 9;
            this.colorModeGroupBox.TabStop = false;
            this.colorModeGroupBox.Text = "Color Mode";
            // 
            // orientationGroupBox
            // 
            this.orientationGroupBox.Controls.Add(this.portraitRadioButton);
            this.orientationGroupBox.Controls.Add(this.landScapeRadioButton);
            this.orientationGroupBox.Location = new System.Drawing.Point(127, 229);
            this.orientationGroupBox.Name = "orientationGroupBox";
            this.orientationGroupBox.Size = new System.Drawing.Size(279, 69);
            this.orientationGroupBox.TabIndex = 10;
            this.orientationGroupBox.TabStop = false;
            this.orientationGroupBox.Text = "Orientation";
            // 
            // tag_label
            // 
            this.tag_label.Location = new System.Drawing.Point(-1, 371);
            this.tag_label.Name = "tag_label";
            this.tag_label.Size = new System.Drawing.Size(122, 24);
            this.tag_label.TabIndex = 19;
            this.tag_label.Text = "Tag";
            this.tag_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tag_comboBox
            // 
            this.tag_comboBox.CausesValidation = false;
            this.tag_comboBox.FormattingEnabled = true;
            this.tag_comboBox.Location = new System.Drawing.Point(127, 371);
            this.tag_comboBox.Name = "tag_comboBox";
            this.tag_comboBox.Size = new System.Drawing.Size(334, 23);
            this.tag_comboBox.TabIndex = 20;
            // 
            // DocumentEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(473, 549);
            this.Controls.Add(this.tag_label);
            this.Controls.Add(this.tag_comboBox);
            this.Controls.Add(this.orientationGroupBox);
            this.Controls.Add(this.colorModeGroupBox);
            this.Controls.Add(this.fileSizeUnitLabel);
            this.Controls.Add(this.appVersionLabel);
            this.Controls.Add(this.appVersionComboBox);
            this.Controls.Add(this.applicationComboBox);
            this.Controls.Add(this.pageCountLabel);
            this.Controls.Add(this.pageCountTextBox);
            this.Controls.Add(this.fileSizeLabel);
            this.Controls.Add(this.fileSizeTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applicationLabel);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.fileTypeComboBox);
            this.Controls.Add(this.manufacturerLabel);
            this.Controls.Add(this.assetIdLabel);
            this.Controls.Add(this.fileNameTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DocumentEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Document Properties";
            this.Load += new System.EventHandler(this.DocumentEditForm_Load);
            this.colorModeGroupBox.ResumeLayout(false);
            this.colorModeGroupBox.PerformLayout();
            this.orientationGroupBox.ResumeLayout(false);
            this.orientationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label assetIdLabel;
        private System.Windows.Forms.Label manufacturerLabel;
        private System.Windows.Forms.ComboBox fileTypeComboBox;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.Label applicationLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.TextBox fileSizeTextBox;
        private System.Windows.Forms.Label fileSizeLabel;
        private System.Windows.Forms.TextBox pageCountTextBox;
        private System.Windows.Forms.Label pageCountLabel;
        private System.Windows.Forms.ComboBox applicationComboBox;
        private System.Windows.Forms.ComboBox appVersionComboBox;
        private System.Windows.Forms.Label appVersionLabel;
        private System.Windows.Forms.RadioButton colorColorModeRadioButton;
        private System.Windows.Forms.RadioButton monoColorModeRadioButton;
        private System.Windows.Forms.RadioButton portraitRadioButton;
        private System.Windows.Forms.RadioButton landScapeRadioButton;
        private System.Windows.Forms.Label fileSizeUnitLabel;
        private System.Windows.Forms.GroupBox colorModeGroupBox;
        private System.Windows.Forms.GroupBox orientationGroupBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label tag_label;
        private System.Windows.Forms.ComboBox tag_comboBox;
    }
}