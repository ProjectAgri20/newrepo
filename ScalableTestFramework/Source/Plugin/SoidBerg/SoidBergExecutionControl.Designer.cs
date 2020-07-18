namespace HP.ScalableTest.Plugin.SoidBerg
{
    partial class SoidBergExecutionControl
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
            this.pluginStatus_TextBox = new System.Windows.Forms.TextBox();
            this.statusTitle_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pluginStatus_TextBox
            // 
            this.pluginStatus_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginStatus_TextBox.Location = new System.Drawing.Point(16, 16);
            this.pluginStatus_TextBox.Multiline = true;
            this.pluginStatus_TextBox.Name = "pluginStatus_TextBox";
            this.pluginStatus_TextBox.ReadOnly = true;
            this.pluginStatus_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.pluginStatus_TextBox.Size = new System.Drawing.Size(465, 307);
            this.pluginStatus_TextBox.TabIndex = 7;
            this.pluginStatus_TextBox.WordWrap = false;
            // 
            // statusTitle_Label
            // 
            this.statusTitle_Label.AutoSize = true;
            this.statusTitle_Label.Location = new System.Drawing.Point(13, 0);
            this.statusTitle_Label.Name = "statusTitle_Label";
            this.statusTitle_Label.Size = new System.Drawing.Size(87, 13);
            this.statusTitle_Label.TabIndex = 8;
            this.statusTitle_Label.Text = "Execution Status";
            // 
            // SoidBergExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusTitle_Label);
            this.Controls.Add(this.pluginStatus_TextBox);
            this.Name = "SoidBergExecutionControl";
            this.Size = new System.Drawing.Size(493, 340);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pluginStatus_TextBox;
        private System.Windows.Forms.Label statusTitle_Label;
    }
}
