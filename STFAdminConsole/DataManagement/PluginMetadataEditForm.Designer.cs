namespace HP.ScalableTest.LabConsole
{
    partial class PluginMetadataEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pluginName_TextBox = new System.Windows.Forms.TextBox();
            this.pluginTitle_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.group_ComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.resourceTypes_CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pluginAssemblyName_TextBox = new System.Windows.Forms.TextBox();
            this.icon_PictureBox = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.browseImages_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.icon_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(62, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plugin Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pluginName_TextBox
            // 
            this.pluginName_TextBox.Location = new System.Drawing.Point(169, 17);
            this.pluginName_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.pluginName_TextBox.Name = "pluginName_TextBox";
            this.pluginName_TextBox.ReadOnly = true;
            this.pluginName_TextBox.Size = new System.Drawing.Size(268, 27);
            this.pluginName_TextBox.TabIndex = 0;
            // 
            // pluginTitle_TextBox
            // 
            this.pluginTitle_TextBox.Location = new System.Drawing.Point(169, 87);
            this.pluginTitle_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.pluginTitle_TextBox.Name = "pluginTitle_TextBox";
            this.pluginTitle_TextBox.Size = new System.Drawing.Size(268, 27);
            this.pluginTitle_TextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(73, 91);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Plugin Title";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // group_ComboBox
            // 
            this.group_ComboBox.FormattingEnabled = true;
            this.group_ComboBox.Location = new System.Drawing.Point(169, 122);
            this.group_ComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.group_ComboBox.Name = "group_ComboBox";
            this.group_ComboBox.Size = new System.Drawing.Size(268, 28);
            this.group_ComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(38, 126);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "Category/Group";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 162);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 22);
            this.label4.TabIndex = 5;
            this.label4.Text = "Applicable Resources";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(205, 373);
            this.ok_button.Margin = new System.Windows.Forms.Padding(4);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(112, 32);
            this.ok_button.TabIndex = 7;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.Location = new System.Drawing.Point(327, 373);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(112, 32);
            this.cancel_Button.TabIndex = 8;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            this.cancel_Button.Click += new System.EventHandler(this.cancel_Button_Click);
            // 
            // resourceTypes_CheckedListBox
            // 
            this.resourceTypes_CheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.resourceTypes_CheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resourceTypes_CheckedListBox.CheckOnClick = true;
            this.resourceTypes_CheckedListBox.FormattingEnabled = true;
            this.resourceTypes_CheckedListBox.Location = new System.Drawing.Point(169, 162);
            this.resourceTypes_CheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.resourceTypes_CheckedListBox.Name = "resourceTypes_CheckedListBox";
            this.resourceTypes_CheckedListBox.Size = new System.Drawing.Size(268, 88);
            this.resourceTypes_CheckedListBox.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 52);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 27);
            this.label5.TabIndex = 9;
            this.label5.Text = "Assembly Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pluginAssemblyName_TextBox
            // 
            this.pluginAssemblyName_TextBox.Location = new System.Drawing.Point(169, 52);
            this.pluginAssemblyName_TextBox.Margin = new System.Windows.Forms.Padding(4);
            this.pluginAssemblyName_TextBox.Name = "pluginAssemblyName_TextBox";
            this.pluginAssemblyName_TextBox.Size = new System.Drawing.Size(268, 27);
            this.pluginAssemblyName_TextBox.TabIndex = 10;
            // 
            // icon_PictureBox
            // 
            this.icon_PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.icon_PictureBox.Location = new System.Drawing.Point(168, 286);
            this.icon_PictureBox.Name = "icon_PictureBox";
            this.icon_PictureBox.Size = new System.Drawing.Size(20, 20);
            this.icon_PictureBox.TabIndex = 11;
            this.icon_PictureBox.TabStop = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 284);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 22);
            this.label6.TabIndex = 12;
            this.label6.Text = "Icon";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // browseImages_Button
            // 
            this.browseImages_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseImages_Button.Location = new System.Drawing.Point(305, 279);
            this.browseImages_Button.Margin = new System.Windows.Forms.Padding(4);
            this.browseImages_Button.Name = "browseImages_Button";
            this.browseImages_Button.Size = new System.Drawing.Size(134, 32);
            this.browseImages_Button.TabIndex = 13;
            this.browseImages_Button.Text = "Browse Images";
            this.browseImages_Button.UseVisualStyleBackColor = true;
            this.browseImages_Button.Click += new System.EventHandler(this.browseImages_Button_Click);
            // 
            // PluginMetadataEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 421);
            this.ControlBox = false;
            this.Controls.Add(this.browseImages_Button);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.icon_PictureBox);
            this.Controls.Add(this.pluginAssemblyName_TextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.resourceTypes_CheckedListBox);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.group_ComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pluginTitle_TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pluginName_TextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "PluginMetadataEditForm";
            this.Text = "Plugin Reference ";
            this.Load += new System.EventHandler(this.PluginMetadataEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.icon_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pluginName_TextBox;
        private System.Windows.Forms.TextBox pluginTitle_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox group_ComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.CheckedListBox resourceTypes_CheckedListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox pluginAssemblyName_TextBox;
        private System.Windows.Forms.PictureBox icon_PictureBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button browseImages_Button;
    }
}