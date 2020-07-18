namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    partial class PrinterOnMobileConfigurationControl
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
            this.options_groupBox = new System.Windows.Forms.GroupBox();
            this.paperSize_comboBox = new System.Windows.Forms.ComboBox();
            this.duplex_comboBox = new System.Windows.Forms.ComboBox();
            this.orientation_comboBox = new System.Windows.Forms.ComboBox();
            this.paperSize_label = new System.Windows.Forms.Label();
            this.duplex_label = new System.Windows.Forms.Label();
            this.orientation_label = new System.Windows.Forms.Label();
            this.Pages_groupBox = new System.Windows.Forms.GroupBox();
            this.pages_radioButton = new System.Windows.Forms.RadioButton();
            this.allPages_radioButton = new System.Windows.Forms.RadioButton();
            this.pages_textBox = new System.Windows.Forms.TextBox();
            this.copies_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.copies_label = new System.Windows.Forms.Label();
            this.filePath_label = new System.Windows.Forms.Label();
            this.filePath_textBox = new System.Windows.Forms.TextBox();
            this.printerId_label = new System.Windows.Forms.Label();
            this.printerId_textBox = new System.Windows.Forms.TextBox();
            this.accountInfo_groupBox = new System.Windows.Forms.GroupBox();
            this.accountInfoCaption_label = new System.Windows.Forms.Label();
            this.email_textBox = new System.Windows.Forms.TextBox();
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.email_label = new System.Windows.Forms.Label();
            this.name_label = new System.Windows.Forms.Label();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.color_label = new System.Windows.Forms.Label();
            this.color_comboBox = new System.Windows.Forms.ComboBox();
            this.options_groupBox.SuspendLayout();
            this.Pages_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.copies_numericUpDown)).BeginInit();
            this.accountInfo_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // options_groupBox
            // 
            this.options_groupBox.Controls.Add(this.color_comboBox);
            this.options_groupBox.Controls.Add(this.color_label);
            this.options_groupBox.Controls.Add(this.paperSize_comboBox);
            this.options_groupBox.Controls.Add(this.duplex_comboBox);
            this.options_groupBox.Controls.Add(this.orientation_comboBox);
            this.options_groupBox.Controls.Add(this.paperSize_label);
            this.options_groupBox.Controls.Add(this.duplex_label);
            this.options_groupBox.Controls.Add(this.orientation_label);
            this.options_groupBox.Controls.Add(this.Pages_groupBox);
            this.options_groupBox.Controls.Add(this.copies_numericUpDown);
            this.options_groupBox.Controls.Add(this.copies_label);
            this.options_groupBox.Location = new System.Drawing.Point(246, 49);
            this.options_groupBox.Name = "options_groupBox";
            this.options_groupBox.Size = new System.Drawing.Size(496, 143);
            this.options_groupBox.TabIndex = 4;
            this.options_groupBox.TabStop = false;
            this.options_groupBox.Text = "Options";
            // 
            // paperSize_comboBox
            // 
            this.paperSize_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paperSize_comboBox.FormattingEnabled = true;
            this.paperSize_comboBox.Location = new System.Drawing.Point(311, 103);
            this.paperSize_comboBox.Name = "paperSize_comboBox";
            this.paperSize_comboBox.Size = new System.Drawing.Size(162, 23);
            this.paperSize_comboBox.TabIndex = 9;
            // 
            // duplex_comboBox
            // 
            this.duplex_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.duplex_comboBox.FormattingEnabled = true;
            this.duplex_comboBox.Location = new System.Drawing.Point(311, 50);
            this.duplex_comboBox.Name = "duplex_comboBox";
            this.duplex_comboBox.Size = new System.Drawing.Size(162, 23);
            this.duplex_comboBox.TabIndex = 8;
            // 
            // orientation_comboBox
            // 
            this.orientation_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orientation_comboBox.FormattingEnabled = true;
            this.orientation_comboBox.Location = new System.Drawing.Point(311, 23);
            this.orientation_comboBox.Name = "orientation_comboBox";
            this.orientation_comboBox.Size = new System.Drawing.Size(161, 23);
            this.orientation_comboBox.TabIndex = 7;
            // 
            // paperSize_label
            // 
            this.paperSize_label.AutoSize = true;
            this.paperSize_label.Location = new System.Drawing.Point(218, 106);
            this.paperSize_label.Name = "paperSize_label";
            this.paperSize_label.Size = new System.Drawing.Size(63, 15);
            this.paperSize_label.TabIndex = 6;
            this.paperSize_label.Text = "Paper Size:";
            // 
            // duplex_label
            // 
            this.duplex_label.AutoSize = true;
            this.duplex_label.Location = new System.Drawing.Point(218, 53);
            this.duplex_label.Name = "duplex_label";
            this.duplex_label.Size = new System.Drawing.Size(46, 15);
            this.duplex_label.TabIndex = 5;
            this.duplex_label.Text = "Duplex:";
            // 
            // orientation_label
            // 
            this.orientation_label.AutoSize = true;
            this.orientation_label.Location = new System.Drawing.Point(218, 26);
            this.orientation_label.Name = "orientation_label";
            this.orientation_label.Size = new System.Drawing.Size(70, 15);
            this.orientation_label.TabIndex = 4;
            this.orientation_label.Text = "Orientation:";
            // 
            // Pages_groupBox
            // 
            this.Pages_groupBox.Controls.Add(this.pages_radioButton);
            this.Pages_groupBox.Controls.Add(this.allPages_radioButton);
            this.Pages_groupBox.Controls.Add(this.pages_textBox);
            this.Pages_groupBox.Location = new System.Drawing.Point(13, 52);
            this.Pages_groupBox.Name = "Pages_groupBox";
            this.Pages_groupBox.Size = new System.Drawing.Size(179, 86);
            this.Pages_groupBox.TabIndex = 3;
            this.Pages_groupBox.TabStop = false;
            this.Pages_groupBox.Text = "Pages";
            // 
            // pages_radioButton
            // 
            this.pages_radioButton.AutoSize = true;
            this.pages_radioButton.Location = new System.Drawing.Point(17, 58);
            this.pages_radioButton.Name = "pages_radioButton";
            this.pages_radioButton.Size = new System.Drawing.Size(14, 13);
            this.pages_radioButton.TabIndex = 6;
            this.pages_radioButton.TabStop = true;
            this.pages_radioButton.UseVisualStyleBackColor = true;
            // 
            // allPages_radioButton
            // 
            this.allPages_radioButton.AutoSize = true;
            this.allPages_radioButton.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allPages_radioButton.Location = new System.Drawing.Point(17, 24);
            this.allPages_radioButton.Name = "allPages_radioButton";
            this.allPages_radioButton.Size = new System.Drawing.Size(71, 17);
            this.allPages_radioButton.TabIndex = 5;
            this.allPages_radioButton.TabStop = true;
            this.allPages_radioButton.Text = "All Pages";
            this.allPages_radioButton.UseVisualStyleBackColor = true;
            this.allPages_radioButton.CheckedChanged += new System.EventHandler(this.allPages_radioButton_CheckedChanged);
            // 
            // pages_textBox
            // 
            this.pages_textBox.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pages_textBox.Location = new System.Drawing.Point(40, 53);
            this.pages_textBox.Name = "pages_textBox";
            this.pages_textBox.Size = new System.Drawing.Size(100, 22);
            this.pages_textBox.TabIndex = 4;
            this.pages_textBox.Text = "1-2";
            // 
            // copies_numericUpDown
            // 
            this.copies_numericUpDown.Location = new System.Drawing.Point(80, 23);
            this.copies_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.copies_numericUpDown.Name = "copies_numericUpDown";
            this.copies_numericUpDown.Size = new System.Drawing.Size(106, 23);
            this.copies_numericUpDown.TabIndex = 2;
            this.copies_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // copies_label
            // 
            this.copies_label.AutoSize = true;
            this.copies_label.Location = new System.Drawing.Point(17, 25);
            this.copies_label.Name = "copies_label";
            this.copies_label.Size = new System.Drawing.Size(46, 15);
            this.copies_label.TabIndex = 0;
            this.copies_label.Text = "Copies:";
            // 
            // filePath_label
            // 
            this.filePath_label.AutoSize = true;
            this.filePath_label.Location = new System.Drawing.Point(255, 19);
            this.filePath_label.Name = "filePath_label";
            this.filePath_label.Size = new System.Drawing.Size(55, 15);
            this.filePath_label.TabIndex = 19;
            this.filePath_label.Text = "File Path:";
            // 
            // filePath_textBox
            // 
            this.filePath_textBox.Location = new System.Drawing.Point(326, 16);
            this.filePath_textBox.Name = "filePath_textBox";
            this.filePath_textBox.Size = new System.Drawing.Size(386, 23);
            this.filePath_textBox.TabIndex = 18;
            // 
            // printerId_label
            // 
            this.printerId_label.AutoSize = true;
            this.printerId_label.Location = new System.Drawing.Point(26, 19);
            this.printerId_label.Name = "printerId_label";
            this.printerId_label.Size = new System.Drawing.Size(59, 15);
            this.printerId_label.TabIndex = 20;
            this.printerId_label.Text = "Printer ID:";
            // 
            // printerId_textBox
            // 
            this.printerId_textBox.Location = new System.Drawing.Point(97, 16);
            this.printerId_textBox.Name = "printerId_textBox";
            this.printerId_textBox.Size = new System.Drawing.Size(125, 23);
            this.printerId_textBox.TabIndex = 21;
            // 
            // accountInfo_groupBox
            // 
            this.accountInfo_groupBox.Controls.Add(this.accountInfoCaption_label);
            this.accountInfo_groupBox.Controls.Add(this.email_textBox);
            this.accountInfo_groupBox.Controls.Add(this.name_textBox);
            this.accountInfo_groupBox.Controls.Add(this.email_label);
            this.accountInfo_groupBox.Controls.Add(this.name_label);
            this.accountInfo_groupBox.Location = new System.Drawing.Point(20, 49);
            this.accountInfo_groupBox.Name = "accountInfo_groupBox";
            this.accountInfo_groupBox.Size = new System.Drawing.Size(220, 144);
            this.accountInfo_groupBox.TabIndex = 22;
            this.accountInfo_groupBox.TabStop = false;
            this.accountInfo_groupBox.Text = "Account Info";
            // 
            // accountInfoCaption_label
            // 
            this.accountInfoCaption_label.AutoSize = true;
            this.accountInfoCaption_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountInfoCaption_label.Location = new System.Drawing.Point(6, 23);
            this.accountInfoCaption_label.Name = "accountInfoCaption_label";
            this.accountInfoCaption_label.Size = new System.Drawing.Size(201, 45);
            this.accountInfoCaption_label.TabIndex = 5;
            this.accountInfoCaption_label.Text = "This printer has some required or \r\noptional information to enter before \r\nprinti" +
    "ng continues.";
            // 
            // email_textBox
            // 
            this.email_textBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email_textBox.Location = new System.Drawing.Point(64, 109);
            this.email_textBox.Name = "email_textBox";
            this.email_textBox.Size = new System.Drawing.Size(138, 23);
            this.email_textBox.TabIndex = 3;
            // 
            // name_textBox
            // 
            this.name_textBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_textBox.Location = new System.Drawing.Point(64, 80);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(138, 23);
            this.name_textBox.TabIndex = 2;
            // 
            // email_label
            // 
            this.email_label.AutoSize = true;
            this.email_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email_label.Location = new System.Drawing.Point(6, 112);
            this.email_label.Name = "email_label";
            this.email_label.Size = new System.Drawing.Size(39, 15);
            this.email_label.TabIndex = 1;
            this.email_label.Text = "Email:";
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_label.Location = new System.Drawing.Point(6, 83);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(42, 15);
            this.name_label.TabIndex = 0;
            this.name_label.Text = "Name:";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(20, 200);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(722, 176);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // color_label
            // 
            this.color_label.AutoSize = true;
            this.color_label.Location = new System.Drawing.Point(218, 79);
            this.color_label.Name = "color_label";
            this.color_label.Size = new System.Drawing.Size(39, 15);
            this.color_label.TabIndex = 10;
            this.color_label.Text = "Color:";
            // 
            // color_comboBox
            // 
            this.color_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.color_comboBox.FormattingEnabled = true;
            this.color_comboBox.Location = new System.Drawing.Point(311, 76);
            this.color_comboBox.Name = "color_comboBox";
            this.color_comboBox.Size = new System.Drawing.Size(162, 23);
            this.color_comboBox.TabIndex = 11;
            // 
            // PrinterOnMobileConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountInfo_groupBox);
            this.Controls.Add(this.printerId_textBox);
            this.Controls.Add(this.printerId_label);
            this.Controls.Add(this.filePath_label);
            this.Controls.Add(this.filePath_textBox);
            this.Controls.Add(this.options_groupBox);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrinterOnMobileConfigurationControl";
            this.Size = new System.Drawing.Size(765, 396);
            this.options_groupBox.ResumeLayout(false);
            this.options_groupBox.PerformLayout();
            this.Pages_groupBox.ResumeLayout(false);
            this.Pages_groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.copies_numericUpDown)).EndInit();
            this.accountInfo_groupBox.ResumeLayout(false);
            this.accountInfo_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox options_groupBox;
        private System.Windows.Forms.Label copies_label;
        private System.Windows.Forms.NumericUpDown copies_numericUpDown;
        private System.Windows.Forms.GroupBox Pages_groupBox;
        private System.Windows.Forms.TextBox pages_textBox;
        private System.Windows.Forms.RadioButton pages_radioButton;
        private System.Windows.Forms.RadioButton allPages_radioButton;
        private System.Windows.Forms.Label paperSize_label;
        private System.Windows.Forms.Label duplex_label;
        private System.Windows.Forms.Label orientation_label;
        private System.Windows.Forms.ComboBox orientation_comboBox;
        private System.Windows.Forms.ComboBox duplex_comboBox;
        private System.Windows.Forms.ComboBox paperSize_comboBox;
        private System.Windows.Forms.Label filePath_label;
        private System.Windows.Forms.TextBox filePath_textBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label printerId_label;
        private System.Windows.Forms.TextBox printerId_textBox;
        private System.Windows.Forms.GroupBox accountInfo_groupBox;
        private System.Windows.Forms.Label accountInfoCaption_label;
        private System.Windows.Forms.TextBox email_textBox;
        private System.Windows.Forms.TextBox name_textBox;
        private System.Windows.Forms.Label email_label;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.ComboBox color_comboBox;
        private System.Windows.Forms.Label color_label;
    }
}
