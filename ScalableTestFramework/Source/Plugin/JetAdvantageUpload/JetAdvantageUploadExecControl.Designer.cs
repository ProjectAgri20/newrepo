namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    partial class JetAdvantageUploadExecControl
    {
        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusTitle_Label = new System.Windows.Forms.Label();
            this.pluginStatus_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // statusTitle_Label
            // 
            this.statusTitle_Label.AutoSize = true;
            this.statusTitle_Label.Location = new System.Drawing.Point(3, 18);
            this.statusTitle_Label.Name = "statusTitle_Label";
            this.statusTitle_Label.Size = new System.Drawing.Size(87, 13);
            this.statusTitle_Label.TabIndex = 0;
            this.statusTitle_Label.Text = "Execution Status";
            // 
            // pluginStatus_TextBox
            // 
            this.pluginStatus_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginStatus_TextBox.Location = new System.Drawing.Point(3, 34);
            this.pluginStatus_TextBox.Multiline = true;
            this.pluginStatus_TextBox.Name = "pluginStatus_TextBox";
            this.pluginStatus_TextBox.ReadOnly = true;
            this.pluginStatus_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.pluginStatus_TextBox.Size = new System.Drawing.Size(404, 256);
            this.pluginStatus_TextBox.TabIndex = 4;
            this.pluginStatus_TextBox.WordWrap = false;
            // 
            // PrintingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pluginStatus_TextBox);
            this.Controls.Add(this.statusTitle_Label);
            this.Name = "PrintingUserControl";
            this.Size = new System.Drawing.Size(410, 293);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusTitle_Label;
        private System.Windows.Forms.TextBox pluginStatus_TextBox;
    }
}
