namespace HP.ScalableTest.LabConsole
{
    partial class GlobalSettingsEditForm
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
            this.components = new System.ComponentModel.Container();
            this.type_Label = new System.Windows.Forms.Label();
            this.value_TextBox = new System.Windows.Forms.TextBox();
            this.name_TextBox = new System.Windows.Forms.TextBox();
            this.name_Label = new System.Windows.Forms.Label();
            this.value_Label = new System.Windows.Forms.Label();
            this.description_Label = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.subType_ListBox = new System.Windows.Forms.CheckedListBox();
            this.instruction_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // type_Label
            // 
            this.type_Label.Location = new System.Drawing.Point(32, 163);
            this.type_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.type_Label.Name = "type_Label";
            this.type_Label.Size = new System.Drawing.Size(84, 18);
            this.type_Label.TabIndex = 0;
            this.type_Label.Text = "Type";
            this.type_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // value_TextBox
            // 
            this.value_TextBox.Location = new System.Drawing.Point(124, 46);
            this.value_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.value_TextBox.Name = "value_TextBox";
            this.value_TextBox.Size = new System.Drawing.Size(399, 23);
            this.value_TextBox.TabIndex = 5;
            // 
            // name_TextBox
            // 
            this.name_TextBox.Location = new System.Drawing.Point(124, 11);
            this.name_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.name_TextBox.Name = "name_TextBox";
            this.name_TextBox.Size = new System.Drawing.Size(268, 23);
            this.name_TextBox.TabIndex = 3;
            // 
            // name_Label
            // 
            this.name_Label.Location = new System.Drawing.Point(20, 15);
            this.name_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.name_Label.Name = "name_Label";
            this.name_Label.Size = new System.Drawing.Size(96, 18);
            this.name_Label.TabIndex = 2;
            this.name_Label.Text = "Setting Name";
            this.name_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // value_Label
            // 
            this.value_Label.Location = new System.Drawing.Point(13, 50);
            this.value_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.value_Label.Name = "value_Label";
            this.value_Label.Size = new System.Drawing.Size(103, 18);
            this.value_Label.TabIndex = 4;
            this.value_Label.Text = "Setting Value";
            this.value_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_Label
            // 
            this.description_Label.Location = new System.Drawing.Point(22, 81);
            this.description_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(94, 18);
            this.description_Label.TabIndex = 6;
            this.description_Label.Text = "Description";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(315, 299);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(100, 32);
            this.ok_button.TabIndex = 8;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(423, 299);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 32);
            this.cancel_Button.TabIndex = 9;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // description_TextBox
            // 
            this.description_TextBox.Location = new System.Drawing.Point(124, 81);
            this.description_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.description_TextBox.Multiline = true;
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(399, 68);
            this.description_TextBox.TabIndex = 7;
            // 
            // subType_ListBox
            // 
            this.subType_ListBox.CheckOnClick = true;
            this.subType_ListBox.FormattingEnabled = true;
            this.subType_ListBox.Location = new System.Drawing.Point(124, 163);
            this.subType_ListBox.Name = "subType_ListBox";
            this.subType_ListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.subType_ListBox.Size = new System.Drawing.Size(291, 94);
            this.subType_ListBox.TabIndex = 12;
            this.subType_ListBox.ThreeDCheckBoxes = true;
            this.subType_ListBox.Click += new System.EventHandler(this.subType_ListBox_Click);
            // 
            // instruction_Label
            // 
            this.instruction_Label.Location = new System.Drawing.Point(121, 260);
            this.instruction_Label.Name = "instruction_Label";
            this.instruction_Label.Size = new System.Drawing.Size(402, 35);
            this.instruction_Label.TabIndex = 13;
            this.instruction_Label.Text = "Select additional...";
            this.instruction_Label.Visible = false;
            // 
            // GlobalSettingsEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 344);
            this.ControlBox = false;
            this.Controls.Add(this.instruction_Label);
            this.Controls.Add(this.subType_ListBox);
            this.Controls.Add(this.description_TextBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.description_Label);
            this.Controls.Add(this.value_Label);
            this.Controls.Add(this.name_Label);
            this.Controls.Add(this.name_TextBox);
            this.Controls.Add(this.type_Label);
            this.Controls.Add(this.value_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "GlobalSettingsEditForm";
            this.Text = "System Setting";
            this.Load += new System.EventHandler(this.GlobalSettingsEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label type_Label;
        private System.Windows.Forms.TextBox value_TextBox;
        private System.Windows.Forms.TextBox name_TextBox;
        private System.Windows.Forms.Label name_Label;
        private System.Windows.Forms.Label value_Label;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TextBox description_TextBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckedListBox subType_ListBox;
        private System.Windows.Forms.Label instruction_Label;
    }
}