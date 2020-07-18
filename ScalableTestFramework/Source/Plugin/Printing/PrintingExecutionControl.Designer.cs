namespace HP.ScalableTest.Plugin.Printing
{
    partial class PrintingExecutionControl
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
            this.statusTitle_Label.Location = new System.Drawing.Point(4, 22);
            this.statusTitle_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusTitle_Label.Name = "statusTitle_Label";
            this.statusTitle_Label.Size = new System.Drawing.Size(113, 17);
            this.statusTitle_Label.TabIndex = 0;
            this.statusTitle_Label.Text = "Execution Status";
            // 
            // pluginStatus_TextBox
            // 
            this.pluginStatus_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginStatus_TextBox.Location = new System.Drawing.Point(7, 42);
            this.pluginStatus_TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pluginStatus_TextBox.Multiline = true;
            this.pluginStatus_TextBox.Name = "pluginStatus_TextBox";
            this.pluginStatus_TextBox.ReadOnly = true;
            this.pluginStatus_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.pluginStatus_TextBox.Size = new System.Drawing.Size(536, 315);
            this.pluginStatus_TextBox.TabIndex = 4;
            this.pluginStatus_TextBox.WordWrap = false;
            // 
            // PrintingUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pluginStatus_TextBox);
            this.Controls.Add(this.statusTitle_Label);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PrintingUserControl";
            this.Size = new System.Drawing.Size(547, 361);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusTitle_Label;
        private System.Windows.Forms.TextBox pluginStatus_TextBox;
    }
}
