namespace HP.ScalableTest.LabConsole
{
    partial class SoftwareInstallerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareInstallerForm));
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.reboot_ComboBox = new System.Windows.Forms.ComboBox();
            this.arguments_TextBox = new System.Windows.Forms.TextBox();
            this.filePath_TextBox = new System.Windows.Forms.TextBox();
            this.description_Label = new System.Windows.Forms.Label();
            this.filePath_Label = new System.Windows.Forms.Label();
            this.arguments_Label = new System.Windows.Forms.Label();
            this.reboot_Label = new System.Windows.Forms.Label();
            this.description_TextBox = new System.Windows.Forms.TextBox();
            this.copyDirectory_CheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.Location = new System.Drawing.Point(349, 155);
            this.ok_Button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(100, 28);
            this.ok_Button.TabIndex = 20;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(457, 155);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(100, 28);
            this.cancel_Button.TabIndex = 21;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.reboot_ComboBox, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.arguments_TextBox, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.filePath_TextBox, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.description_Label, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.filePath_Label, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.arguments_Label, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.reboot_Label, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.description_TextBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.copyDirectory_CheckBox, 1, 4);
            this.tableLayoutPanel.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(553, 144);
            this.tableLayoutPanel.TabIndex = 22;
            // 
            // reboot_ComboBox
            // 
            this.reboot_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reboot_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reboot_ComboBox.FormattingEnabled = true;
            this.reboot_ComboBox.Location = new System.Drawing.Point(153, 94);
            this.reboot_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.reboot_ComboBox.Name = "reboot_ComboBox";
            this.reboot_ComboBox.Size = new System.Drawing.Size(396, 24);
            this.reboot_ComboBox.TabIndex = 11;
            this.reboot_ComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.reboot_ComboBox_Validating);
            // 
            // arguments_TextBox
            // 
            this.arguments_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.arguments_TextBox.Location = new System.Drawing.Point(153, 64);
            this.arguments_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.arguments_TextBox.Name = "arguments_TextBox";
            this.arguments_TextBox.Size = new System.Drawing.Size(396, 22);
            this.arguments_TextBox.TabIndex = 8;
            // 
            // filePath_TextBox
            // 
            this.filePath_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePath_TextBox.Location = new System.Drawing.Point(153, 34);
            this.filePath_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.filePath_TextBox.Name = "filePath_TextBox";
            this.filePath_TextBox.Size = new System.Drawing.Size(396, 22);
            this.filePath_TextBox.TabIndex = 7;
            this.filePath_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.filePath_TextBox_Validating);
            // 
            // description_Label
            // 
            this.description_Label.AutoSize = true;
            this.description_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.description_Label.Location = new System.Drawing.Point(4, 0);
            this.description_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.description_Label.Name = "description_Label";
            this.description_Label.Size = new System.Drawing.Size(141, 30);
            this.description_Label.TabIndex = 0;
            this.description_Label.Text = "File Name:";
            this.description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filePath_Label
            // 
            this.filePath_Label.AutoSize = true;
            this.filePath_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filePath_Label.Location = new System.Drawing.Point(4, 30);
            this.filePath_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.filePath_Label.Name = "filePath_Label";
            this.filePath_Label.Size = new System.Drawing.Size(141, 30);
            this.filePath_Label.TabIndex = 1;
            this.filePath_Label.Text = "File Path:";
            this.filePath_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // arguments_Label
            // 
            this.arguments_Label.AutoSize = true;
            this.arguments_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arguments_Label.Location = new System.Drawing.Point(4, 60);
            this.arguments_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.arguments_Label.Name = "arguments_Label";
            this.arguments_Label.Size = new System.Drawing.Size(141, 30);
            this.arguments_Label.TabIndex = 2;
            this.arguments_Label.Text = "Arguments:";
            this.arguments_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // reboot_Label
            // 
            this.reboot_Label.AutoSize = true;
            this.reboot_Label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reboot_Label.Location = new System.Drawing.Point(4, 90);
            this.reboot_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.reboot_Label.Name = "reboot_Label";
            this.reboot_Label.Size = new System.Drawing.Size(141, 30);
            this.reboot_Label.TabIndex = 3;
            this.reboot_Label.Text = "Reboot Mode:";
            this.reboot_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // description_TextBox
            // 
            this.description_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description_TextBox.Location = new System.Drawing.Point(153, 4);
            this.description_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.description_TextBox.Name = "description_TextBox";
            this.description_TextBox.Size = new System.Drawing.Size(396, 22);
            this.description_TextBox.TabIndex = 6;
            this.description_TextBox.Validating += new System.ComponentModel.CancelEventHandler(this.description_TextBox_Validating);
            // 
            // copyDirectory_CheckBox
            // 
            this.copyDirectory_CheckBox.AutoSize = true;
            this.copyDirectory_CheckBox.Location = new System.Drawing.Point(153, 124);
            this.copyDirectory_CheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.copyDirectory_CheckBox.Name = "copyDirectory_CheckBox";
            this.copyDirectory_CheckBox.Size = new System.Drawing.Size(163, 21);
            this.copyDirectory_CheckBox.TabIndex = 12;
            this.copyDirectory_CheckBox.Text = "Copy Install Directory";
            this.copyDirectory_CheckBox.UseVisualStyleBackColor = true;
            // 
            // SoftwareInstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(559, 186);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.cancel_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SoftwareInstallerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Software Installer";
            this.Load += new System.EventHandler(this.SoftwareInstallerForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox reboot_ComboBox;
        private System.Windows.Forms.TextBox arguments_TextBox;
        private System.Windows.Forms.TextBox filePath_TextBox;
        private System.Windows.Forms.Label description_Label;
        private System.Windows.Forms.Label filePath_Label;
        private System.Windows.Forms.Label arguments_Label;
        private System.Windows.Forms.Label reboot_Label;
        private System.Windows.Forms.TextBox description_TextBox;
        private System.Windows.Forms.CheckBox copyDirectory_CheckBox;
    }
}