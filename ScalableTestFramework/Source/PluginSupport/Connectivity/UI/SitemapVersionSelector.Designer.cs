namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    partial class SitemapVersionSelector
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
            this.sitemapDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.browse_Button = new System.Windows.Forms.Button();
            this.sitemapVersion_ComboBox = new System.Windows.Forms.ComboBox();
            this.sitemapVersion_Label = new System.Windows.Forms.Label();
            this.sitemapLocation_TextBox = new System.Windows.Forms.TextBox();
            this.sitemapLocation_Label = new System.Windows.Forms.Label();
            this.sitemap_FieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.sitemapDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sitemapDetails_GroupBox
            // 
            this.sitemapDetails_GroupBox.Controls.Add(this.browse_Button);
            this.sitemapDetails_GroupBox.Controls.Add(this.sitemapVersion_ComboBox);
            this.sitemapDetails_GroupBox.Controls.Add(this.sitemapVersion_Label);
            this.sitemapDetails_GroupBox.Controls.Add(this.sitemapLocation_TextBox);
            this.sitemapDetails_GroupBox.Controls.Add(this.sitemapLocation_Label);
            this.sitemapDetails_GroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sitemapDetails_GroupBox.Location = new System.Drawing.Point(0, 0);
            this.sitemapDetails_GroupBox.Name = "sitemapDetails_GroupBox";
            this.sitemapDetails_GroupBox.Size = new System.Drawing.Size(330, 100);
            this.sitemapDetails_GroupBox.TabIndex = 2;
            this.sitemapDetails_GroupBox.TabStop = false;
            this.sitemapDetails_GroupBox.Text = "Sitemap Details";
            // 
            // browse_Button
            // 
            this.browse_Button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browse_Button.AutoSize = true;
            this.browse_Button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.browse_Button.Location = new System.Drawing.Point(265, 20);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(26, 23);
            this.browse_Button.TabIndex = 3;
            this.browse_Button.Text = "...";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // sitemapVersion_ComboBox
            // 
            this.sitemapVersion_ComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sitemapVersion_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sitemapVersion_ComboBox.FormattingEnabled = true;
            this.sitemapVersion_ComboBox.Location = new System.Drawing.Point(81, 55);
            this.sitemapVersion_ComboBox.Name = "sitemapVersion_ComboBox";
            this.sitemapVersion_ComboBox.Size = new System.Drawing.Size(144, 21);
            this.sitemapVersion_ComboBox.TabIndex = 2;
            this.sitemapVersion_ComboBox.SelectedIndexChanged += new System.EventHandler(this.sitemapVersion_ComboBox_SelectedIndexChanged);
            // 
            // sitemapVersion_Label
            // 
            this.sitemapVersion_Label.AutoSize = true;
            this.sitemapVersion_Label.Location = new System.Drawing.Point(24, 58);
            this.sitemapVersion_Label.Name = "sitemapVersion_Label";
            this.sitemapVersion_Label.Size = new System.Drawing.Size(45, 13);
            this.sitemapVersion_Label.TabIndex = 2;
            this.sitemapVersion_Label.Text = "Version:";
            // 
            // sitemapLocation_TextBox
            // 
            this.sitemapLocation_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sitemapLocation_TextBox.Location = new System.Drawing.Point(81, 22);
            this.sitemapLocation_TextBox.Name = "sitemapLocation_TextBox";
            this.sitemapLocation_TextBox.ReadOnly = true;
            this.sitemapLocation_TextBox.Size = new System.Drawing.Size(168, 20);
            this.sitemapLocation_TextBox.TabIndex = 0;
            this.sitemapLocation_TextBox.TextChanged += new System.EventHandler(this.sitemapLocation_TextBox_TextChanged);
            // 
            // sitemapLocation_Label
            // 
            this.sitemapLocation_Label.AutoSize = true;
            this.sitemapLocation_Label.Location = new System.Drawing.Point(24, 25);
            this.sitemapLocation_Label.Name = "sitemapLocation_Label";
            this.sitemapLocation_Label.Size = new System.Drawing.Size(51, 13);
            this.sitemapLocation_Label.TabIndex = 0;
            this.sitemapLocation_Label.Text = "Location:";
            // 
            // SitemapVersionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.sitemapDetails_GroupBox);
            this.Name = "SitemapVersionSelector";
            this.Size = new System.Drawing.Size(330, 100);
            this.sitemapDetails_GroupBox.ResumeLayout(false);
            this.sitemapDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox sitemapDetails_GroupBox;
        private System.Windows.Forms.ComboBox sitemapVersion_ComboBox;
        private System.Windows.Forms.Label sitemapVersion_Label;
        private System.Windows.Forms.TextBox sitemapLocation_TextBox;
        private System.Windows.Forms.Label sitemapLocation_Label;
        private Framework.UI.FieldValidator sitemap_FieldValidator;
		private System.Windows.Forms.Button browse_Button;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
	}
}
