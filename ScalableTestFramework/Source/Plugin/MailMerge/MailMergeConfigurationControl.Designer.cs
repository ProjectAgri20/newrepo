namespace HP.ScalableTest.Plugin.MailMerge
{
    partial class MailMergeConfigurationControl
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
            this.format_groupBox = new System.Windows.Forms.GroupBox();
            this.envelope_radioButton = new System.Windows.Forms.RadioButton();
            this.letter_radioButton = new System.Windows.Forms.RadioButton();
            this.jobseparator_checkBox = new System.Windows.Forms.CheckBox();
            this.messageBody_label = new System.Windows.Forms.Label();
            this.recipient_groupBox = new System.Windows.Forms.GroupBox();
            this.recipientsList_label = new System.Windows.Forms.Label();
            this.recipientsRemove_button = new System.Windows.Forms.Button();
            this.recipientsAdd_button = new System.Windows.Forms.Button();
            this.recipientsAddress_richTextBox = new System.Windows.Forms.RichTextBox();
            this.recipientsAddress_label = new System.Windows.Forms.Label();
            this.recipientsName_textBox = new System.Windows.Forms.TextBox();
            this.recipientsName_label = new System.Windows.Forms.Label();
            this.recipientsList_listBox = new System.Windows.Forms.ListBox();
            this.source_groupBox = new System.Windows.Forms.GroupBox();
            this.sourceDepartment_textBox = new System.Windows.Forms.TextBox();
            this.sourceDepartment_label = new System.Windows.Forms.Label();
            this.sourceCompany_textBox = new System.Windows.Forms.TextBox();
            this.sourceCompany_label = new System.Windows.Forms.Label();
            this.sourceDesignation_textBox = new System.Windows.Forms.TextBox();
            this.sourceDesignation_label = new System.Windows.Forms.Label();
            this.sourceName_label = new System.Windows.Forms.Label();
            this.sourceName_textBox = new System.Windows.Forms.TextBox();
            this.message_richTextBox = new System.Windows.Forms.RichTextBox();
            this.device_groupBox = new System.Windows.Forms.GroupBox();
            this.device_textBox = new System.Windows.Forms.TextBox();
            this.printerSelect_button = new System.Windows.Forms.Button();
            this.mail_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.format_groupBox.SuspendLayout();
            this.recipient_groupBox.SuspendLayout();
            this.source_groupBox.SuspendLayout();
            this.device_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // format_groupBox
            // 
            this.format_groupBox.Controls.Add(this.envelope_radioButton);
            this.format_groupBox.Controls.Add(this.letter_radioButton);
            this.format_groupBox.Location = new System.Drawing.Point(24, 70);
            this.format_groupBox.Name = "format_groupBox";
            this.format_groupBox.Size = new System.Drawing.Size(167, 42);
            this.format_groupBox.TabIndex = 10;
            this.format_groupBox.TabStop = false;
            this.format_groupBox.Text = "Mail Merge Format";
            // 
            // envelope_radioButton
            // 
            this.envelope_radioButton.AutoSize = true;
            this.envelope_radioButton.Location = new System.Drawing.Point(80, 19);
            this.envelope_radioButton.Name = "envelope_radioButton";
            this.envelope_radioButton.Size = new System.Drawing.Size(70, 17);
            this.envelope_radioButton.TabIndex = 1;
            this.envelope_radioButton.Text = "Envelope";
            this.envelope_radioButton.UseVisualStyleBackColor = true;
            // 
            // letter_radioButton
            // 
            this.letter_radioButton.AutoSize = true;
            this.letter_radioButton.Checked = true;
            this.letter_radioButton.Location = new System.Drawing.Point(9, 19);
            this.letter_radioButton.Name = "letter_radioButton";
            this.letter_radioButton.Size = new System.Drawing.Size(52, 17);
            this.letter_radioButton.TabIndex = 0;
            this.letter_radioButton.TabStop = true;
            this.letter_radioButton.Text = "Letter";
            this.letter_radioButton.UseVisualStyleBackColor = true;
            this.letter_radioButton.CheckedChanged += new System.EventHandler(this.letter_radioButton_CheckedChanged);
            // 
            // jobseparator_checkBox
            // 
            this.jobseparator_checkBox.AutoSize = true;
            this.jobseparator_checkBox.Location = new System.Drawing.Point(440, 21);
            this.jobseparator_checkBox.Name = "jobseparator_checkBox";
            this.jobseparator_checkBox.Size = new System.Drawing.Size(116, 17);
            this.jobseparator_checkBox.TabIndex = 2;
            this.jobseparator_checkBox.Text = "Print Job Separator";
            this.jobseparator_checkBox.UseVisualStyleBackColor = true;
            // 
            // messageBody_label
            // 
            this.messageBody_label.AutoSize = true;
            this.messageBody_label.Location = new System.Drawing.Point(30, 369);
            this.messageBody_label.Name = "messageBody_label";
            this.messageBody_label.Size = new System.Drawing.Size(77, 13);
            this.messageBody_label.TabIndex = 9;
            this.messageBody_label.Text = "Message Body";
            // 
            // recipient_groupBox
            // 
            this.recipient_groupBox.Controls.Add(this.recipientsList_label);
            this.recipient_groupBox.Controls.Add(this.recipientsRemove_button);
            this.recipient_groupBox.Controls.Add(this.recipientsAdd_button);
            this.recipient_groupBox.Controls.Add(this.recipientsAddress_richTextBox);
            this.recipient_groupBox.Controls.Add(this.recipientsAddress_label);
            this.recipient_groupBox.Controls.Add(this.recipientsName_textBox);
            this.recipient_groupBox.Controls.Add(this.recipientsName_label);
            this.recipient_groupBox.Controls.Add(this.recipientsList_listBox);
            this.recipient_groupBox.Location = new System.Drawing.Point(24, 202);
            this.recipient_groupBox.Name = "recipient_groupBox";
            this.recipient_groupBox.Size = new System.Drawing.Size(640, 164);
            this.recipient_groupBox.TabIndex = 8;
            this.recipient_groupBox.TabStop = false;
            this.recipient_groupBox.Text = "Recipients";
            // 
            // recipientsList_label
            // 
            this.recipientsList_label.AutoSize = true;
            this.recipientsList_label.Location = new System.Drawing.Point(345, 21);
            this.recipientsList_label.Name = "recipientsList_label";
            this.recipientsList_label.Size = new System.Drawing.Size(57, 13);
            this.recipientsList_label.TabIndex = 7;
            this.recipientsList_label.Text = "Recipients";
            // 
            // recipientsRemove_button
            // 
            this.recipientsRemove_button.Location = new System.Drawing.Point(264, 91);
            this.recipientsRemove_button.Name = "recipientsRemove_button";
            this.recipientsRemove_button.Size = new System.Drawing.Size(75, 23);
            this.recipientsRemove_button.TabIndex = 6;
            this.recipientsRemove_button.Text = "Remove";
            this.recipientsRemove_button.UseVisualStyleBackColor = true;
            this.recipientsRemove_button.Click += new System.EventHandler(this.recipientsRemove_button_Click);
            // 
            // recipientsAdd_button
            // 
            this.recipientsAdd_button.Location = new System.Drawing.Point(264, 60);
            this.recipientsAdd_button.Name = "recipientsAdd_button";
            this.recipientsAdd_button.Size = new System.Drawing.Size(75, 23);
            this.recipientsAdd_button.TabIndex = 3;
            this.recipientsAdd_button.Text = "Add";
            this.recipientsAdd_button.UseVisualStyleBackColor = true;
            this.recipientsAdd_button.Click += new System.EventHandler(this.recipientsAdd_button_Click);
            // 
            // recipientsAddress_richTextBox
            // 
            this.recipientsAddress_richTextBox.Location = new System.Drawing.Point(80, 62);
            this.recipientsAddress_richTextBox.Name = "recipientsAddress_richTextBox";
            this.recipientsAddress_richTextBox.Size = new System.Drawing.Size(178, 96);
            this.recipientsAddress_richTextBox.TabIndex = 2;
            this.recipientsAddress_richTextBox.Text = "";
            // 
            // recipientsAddress_label
            // 
            this.recipientsAddress_label.AutoSize = true;
            this.recipientsAddress_label.Location = new System.Drawing.Point(6, 62);
            this.recipientsAddress_label.Name = "recipientsAddress_label";
            this.recipientsAddress_label.Size = new System.Drawing.Size(45, 13);
            this.recipientsAddress_label.TabIndex = 3;
            this.recipientsAddress_label.Text = "Address";
            // 
            // recipientsName_textBox
            // 
            this.recipientsName_textBox.Location = new System.Drawing.Point(80, 31);
            this.recipientsName_textBox.Name = "recipientsName_textBox";
            this.recipientsName_textBox.Size = new System.Drawing.Size(178, 20);
            this.recipientsName_textBox.TabIndex = 1;
            // 
            // recipientsName_label
            // 
            this.recipientsName_label.AutoSize = true;
            this.recipientsName_label.Location = new System.Drawing.Point(6, 31);
            this.recipientsName_label.Name = "recipientsName_label";
            this.recipientsName_label.Size = new System.Drawing.Size(35, 13);
            this.recipientsName_label.TabIndex = 1;
            this.recipientsName_label.Text = "Name";
            // 
            // recipientsList_listBox
            // 
            this.recipientsList_listBox.FormattingEnabled = true;
            this.recipientsList_listBox.Location = new System.Drawing.Point(345, 37);
            this.recipientsList_listBox.Name = "recipientsList_listBox";
            this.recipientsList_listBox.Size = new System.Drawing.Size(178, 121);
            this.recipientsList_listBox.TabIndex = 0;
            // 
            // source_groupBox
            // 
            this.source_groupBox.Controls.Add(this.sourceDepartment_textBox);
            this.source_groupBox.Controls.Add(this.sourceDepartment_label);
            this.source_groupBox.Controls.Add(this.sourceCompany_textBox);
            this.source_groupBox.Controls.Add(this.sourceCompany_label);
            this.source_groupBox.Controls.Add(this.sourceDesignation_textBox);
            this.source_groupBox.Controls.Add(this.sourceDesignation_label);
            this.source_groupBox.Controls.Add(this.sourceName_label);
            this.source_groupBox.Controls.Add(this.sourceName_textBox);
            this.source_groupBox.Location = new System.Drawing.Point(24, 118);
            this.source_groupBox.Name = "source_groupBox";
            this.source_groupBox.Size = new System.Drawing.Size(640, 78);
            this.source_groupBox.TabIndex = 7;
            this.source_groupBox.TabStop = false;
            this.source_groupBox.Text = "From";
            // 
            // sourceDepartment_textBox
            // 
            this.sourceDepartment_textBox.Location = new System.Drawing.Point(345, 13);
            this.sourceDepartment_textBox.Name = "sourceDepartment_textBox";
            this.sourceDepartment_textBox.Size = new System.Drawing.Size(178, 20);
            this.sourceDepartment_textBox.TabIndex = 2;
            // 
            // sourceDepartment_label
            // 
            this.sourceDepartment_label.AutoSize = true;
            this.sourceDepartment_label.Location = new System.Drawing.Point(277, 16);
            this.sourceDepartment_label.Name = "sourceDepartment_label";
            this.sourceDepartment_label.Size = new System.Drawing.Size(62, 13);
            this.sourceDepartment_label.TabIndex = 6;
            this.sourceDepartment_label.Text = "Department";
            // 
            // sourceCompany_textBox
            // 
            this.sourceCompany_textBox.Location = new System.Drawing.Point(345, 48);
            this.sourceCompany_textBox.Name = "sourceCompany_textBox";
            this.sourceCompany_textBox.Size = new System.Drawing.Size(178, 20);
            this.sourceCompany_textBox.TabIndex = 3;
            // 
            // sourceCompany_label
            // 
            this.sourceCompany_label.AutoSize = true;
            this.sourceCompany_label.Location = new System.Drawing.Point(277, 51);
            this.sourceCompany_label.Name = "sourceCompany_label";
            this.sourceCompany_label.Size = new System.Drawing.Size(51, 13);
            this.sourceCompany_label.TabIndex = 4;
            this.sourceCompany_label.Text = "Company";
            // 
            // sourceDesignation_textBox
            // 
            this.sourceDesignation_textBox.Location = new System.Drawing.Point(80, 51);
            this.sourceDesignation_textBox.Name = "sourceDesignation_textBox";
            this.sourceDesignation_textBox.Size = new System.Drawing.Size(178, 20);
            this.sourceDesignation_textBox.TabIndex = 1;
            // 
            // sourceDesignation_label
            // 
            this.sourceDesignation_label.AutoSize = true;
            this.sourceDesignation_label.Location = new System.Drawing.Point(6, 51);
            this.sourceDesignation_label.Name = "sourceDesignation_label";
            this.sourceDesignation_label.Size = new System.Drawing.Size(63, 13);
            this.sourceDesignation_label.TabIndex = 2;
            this.sourceDesignation_label.Text = "Designation";
            // 
            // sourceName_label
            // 
            this.sourceName_label.AutoSize = true;
            this.sourceName_label.Location = new System.Drawing.Point(6, 16);
            this.sourceName_label.Name = "sourceName_label";
            this.sourceName_label.Size = new System.Drawing.Size(35, 13);
            this.sourceName_label.TabIndex = 1;
            this.sourceName_label.Text = "Name";
            // 
            // sourceName_textBox
            // 
            this.sourceName_textBox.Location = new System.Drawing.Point(80, 16);
            this.sourceName_textBox.Name = "sourceName_textBox";
            this.sourceName_textBox.Size = new System.Drawing.Size(178, 20);
            this.sourceName_textBox.TabIndex = 0;
            // 
            // message_richTextBox
            // 
            this.message_richTextBox.Location = new System.Drawing.Point(24, 385);
            this.message_richTextBox.Name = "message_richTextBox";
            this.message_richTextBox.Size = new System.Drawing.Size(640, 110);
            this.message_richTextBox.TabIndex = 6;
            this.message_richTextBox.Text = "";
            // 
            // device_groupBox
            // 
            this.device_groupBox.Controls.Add(this.jobseparator_checkBox);
            this.device_groupBox.Controls.Add(this.device_textBox);
            this.device_groupBox.Controls.Add(this.printerSelect_button);
            this.device_groupBox.Location = new System.Drawing.Point(24, 14);
            this.device_groupBox.Name = "device_groupBox";
            this.device_groupBox.Size = new System.Drawing.Size(610, 50);
            this.device_groupBox.TabIndex = 11;
            this.device_groupBox.TabStop = false;
            this.device_groupBox.Text = "Printer";
            // 
            // device_textBox
            // 
            this.device_textBox.Location = new System.Drawing.Point(6, 19);
            this.device_textBox.Name = "device_textBox";
            this.device_textBox.Size = new System.Drawing.Size(337, 20);
            this.device_textBox.TabIndex = 0;
            // 
            // printerSelect_button
            // 
            this.printerSelect_button.Location = new System.Drawing.Point(349, 19);
            this.printerSelect_button.Name = "printerSelect_button";
            this.printerSelect_button.Size = new System.Drawing.Size(75, 23);
            this.printerSelect_button.TabIndex = 1;
            this.printerSelect_button.Text = "...";
            this.printerSelect_button.UseVisualStyleBackColor = true;
            this.printerSelect_button.Click += new System.EventHandler(this.printerSelect_button_Click);
            // 
            // MailMergeConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.device_groupBox);
            this.Controls.Add(this.format_groupBox);
            this.Controls.Add(this.messageBody_label);
            this.Controls.Add(this.recipient_groupBox);
            this.Controls.Add(this.source_groupBox);
            this.Controls.Add(this.message_richTextBox);
            this.Name = "MailMergeConfigurationControl";
            this.Size = new System.Drawing.Size(695, 554);
            this.format_groupBox.ResumeLayout(false);
            this.format_groupBox.PerformLayout();
            this.recipient_groupBox.ResumeLayout(false);
            this.recipient_groupBox.PerformLayout();
            this.source_groupBox.ResumeLayout(false);
            this.source_groupBox.PerformLayout();
            this.device_groupBox.ResumeLayout(false);
            this.device_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox format_groupBox;
        private System.Windows.Forms.CheckBox jobseparator_checkBox;
        private System.Windows.Forms.RadioButton envelope_radioButton;
        private System.Windows.Forms.RadioButton letter_radioButton;
        private System.Windows.Forms.Label messageBody_label;
        private System.Windows.Forms.GroupBox recipient_groupBox;
        private System.Windows.Forms.Label recipientsList_label;
        private System.Windows.Forms.Button recipientsRemove_button;
        private System.Windows.Forms.Button recipientsAdd_button;
        private System.Windows.Forms.RichTextBox recipientsAddress_richTextBox;
        private System.Windows.Forms.Label recipientsAddress_label;
        private System.Windows.Forms.TextBox recipientsName_textBox;
        private System.Windows.Forms.Label recipientsName_label;
        private System.Windows.Forms.ListBox recipientsList_listBox;
        private System.Windows.Forms.GroupBox source_groupBox;
        private System.Windows.Forms.TextBox sourceDepartment_textBox;
        private System.Windows.Forms.Label sourceDepartment_label;
        private System.Windows.Forms.TextBox sourceCompany_textBox;
        private System.Windows.Forms.Label sourceCompany_label;
        private System.Windows.Forms.TextBox sourceDesignation_textBox;
        private System.Windows.Forms.Label sourceDesignation_label;
        private System.Windows.Forms.Label sourceName_label;
        private System.Windows.Forms.TextBox sourceName_textBox;
        private System.Windows.Forms.RichTextBox message_richTextBox;
        private System.Windows.Forms.GroupBox device_groupBox;
        private System.Windows.Forms.TextBox device_textBox;
        private System.Windows.Forms.Button printerSelect_button;
        private Framework.UI.FieldValidator mail_fieldValidator;
    }
}
