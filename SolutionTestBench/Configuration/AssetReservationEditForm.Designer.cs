namespace HP.ScalableTest
{
    partial class AssetReservationEditForm<T>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyForm));
            this.assetIdLabel = new System.Windows.Forms.Label();
            this.reservedForLabel = new System.Windows.Forms.Label();
            this.notesLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.startLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.reservedForComboBox = new System.Windows.Forms.ComboBox();
            this.reservedByComboBox = new System.Windows.Forms.ComboBox();
            this.reservedByLabel = new System.Windows.Forms.Label();
            this.permanentCheckBox = new System.Windows.Forms.CheckBox();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.permanentLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // assetIdLabel
            // 
            this.assetIdLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.assetIdLabel.Location = new System.Drawing.Point(124, 27);
            this.assetIdLabel.Name = "assetIdLabel";
            this.assetIdLabel.Size = new System.Drawing.Size(282, 24);
            this.assetIdLabel.TabIndex = 0;
            this.assetIdLabel.Text = "Device";
            this.assetIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // reservedForLabel
            // 
            this.reservedForLabel.Location = new System.Drawing.Point(2, 150);
            this.reservedForLabel.Name = "reservedForLabel";
            this.reservedForLabel.Size = new System.Drawing.Size(119, 24);
            this.reservedForLabel.TabIndex = 5;
            this.reservedForLabel.Text = "Reserved For";
            this.reservedForLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notesLabel
            // 
            this.notesLabel.Location = new System.Drawing.Point(25, 214);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(96, 24);
            this.notesLabel.TabIndex = 10;
            this.notesLabel.Text = "Notes";
            this.notesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // notesTextBox
            // 
            this.notesTextBox.Location = new System.Drawing.Point(127, 214);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.Size = new System.Drawing.Size(279, 85);
            this.notesTextBox.TabIndex = 11;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(309, 323);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(203, 323);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.CustomFormat = "dd-MMM-yyyy HH:mm:ss ";
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(127, 92);
            this.endDateTimePicker.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(196, 24);
            this.endDateTimePicker.TabIndex = 4;
            this.endDateTimePicker.Value = new System.DateTime(2015, 4, 9, 0, 0, 0, 0);
            this.endDateTimePicker.Validating += new System.ComponentModel.CancelEventHandler(this.endDateTimePicker_Validating);
            // 
            // startLabel
            // 
            this.startLabel.Location = new System.Drawing.Point(25, 64);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(96, 24);
            this.startLabel.TabIndex = 1;
            this.startLabel.Text = "Start";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endLabel
            // 
            this.endLabel.Location = new System.Drawing.Point(25, 94);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(96, 24);
            this.endLabel.TabIndex = 3;
            this.endLabel.Text = "End";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // reservedForComboBox
            // 
            this.reservedForComboBox.FormattingEnabled = true;
            this.reservedForComboBox.Location = new System.Drawing.Point(127, 150);
            this.reservedForComboBox.Name = "reservedForComboBox";
            this.reservedForComboBox.Size = new System.Drawing.Size(279, 26);
            this.reservedForComboBox.TabIndex = 6;
            this.reservedForComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.reservedForComboBox_Validating);
            // 
            // reservedByComboBox
            // 
            this.reservedByComboBox.FormattingEnabled = true;
            this.reservedByComboBox.Location = new System.Drawing.Point(127, 182);
            this.reservedByComboBox.Name = "reservedByComboBox";
            this.reservedByComboBox.Size = new System.Drawing.Size(279, 26);
            this.reservedByComboBox.TabIndex = 8;
            this.reservedByComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.reservedByComboBox_Validating);
            // 
            // reservedByLabel
            // 
            this.reservedByLabel.Location = new System.Drawing.Point(2, 182);
            this.reservedByLabel.Name = "reservedByLabel";
            this.reservedByLabel.Size = new System.Drawing.Size(119, 24);
            this.reservedByLabel.TabIndex = 7;
            this.reservedByLabel.Text = "Reserved By";
            this.reservedByLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // permanentCheckBox
            // 
            this.permanentCheckBox.AutoSize = true;
            this.permanentCheckBox.Location = new System.Drawing.Point(127, 122);
            this.permanentCheckBox.Name = "permanentCheckBox";
            this.permanentCheckBox.Size = new System.Drawing.Size(185, 22);
            this.permanentCheckBox.TabIndex = 9;
            this.permanentCheckBox.Text = "Permanent Reservation";
            this.permanentCheckBox.UseVisualStyleBackColor = true;
            this.permanentCheckBox.CheckedChanged += new System.EventHandler(this.permanentCheckBox_CheckedChanged);
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.CustomFormat = "dd-MMM-yyyy HH:mm:ss ";
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTimePicker.Location = new System.Drawing.Point(127, 62);
            this.startDateTimePicker.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(196, 24);
            this.startDateTimePicker.TabIndex = 2;
            this.startDateTimePicker.Value = new System.DateTime(2015, 4, 9, 0, 0, 0, 0);
            this.startDateTimePicker.Validating += new System.ComponentModel.CancelEventHandler(this.startDateTimePicker_Validating);
            // 
            // permanentLabel
            // 
            this.permanentLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.permanentLabel.Location = new System.Drawing.Point(127, 92);
            this.permanentLabel.Name = "permanentLabel";
            this.permanentLabel.Size = new System.Drawing.Size(279, 24);
            this.permanentLabel.TabIndex = 30;
            this.permanentLabel.Text = "Permanent Reservation";
            this.permanentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.permanentLabel.Visible = false;
            // 
            // AssetReservationEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(421, 367);
            this.Controls.Add(this.startDateTimePicker);
            this.Controls.Add(this.permanentCheckBox);
            this.Controls.Add(this.reservedByLabel);
            this.Controls.Add(this.reservedByComboBox);
            this.Controls.Add(this.reservedForComboBox);
            this.Controls.Add(this.endLabel);
            this.Controls.Add(this.startLabel);
            this.Controls.Add(this.endDateTimePicker);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.reservedForLabel);
            this.Controls.Add(this.assetIdLabel);
            this.Controls.Add(this.permanentLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssetReservationEditForm";
            this.Text = "Asset Reservation";
            this.Load += new System.EventHandler(this.AssetReservationEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label assetIdLabel;
        private System.Windows.Forms.Label reservedForLabel;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.ComboBox reservedForComboBox;
        private System.Windows.Forms.ComboBox reservedByComboBox;
        private System.Windows.Forms.Label reservedByLabel;
        private System.Windows.Forms.CheckBox permanentCheckBox;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.Label permanentLabel;
    }
}