namespace HP.ScalableTest.Plugin.DriverlessPrinting
{
    partial class DriverlessPrintingExecutionControl
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
            this.pluginStatus_TextBox.Location = new System.Drawing.Point(12, 23);
            this.pluginStatus_TextBox.Multiline = true;
            this.pluginStatus_TextBox.Name = "pluginStatus_TextBox";
            this.pluginStatus_TextBox.ReadOnly = true;
            this.pluginStatus_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.pluginStatus_TextBox.Size = new System.Drawing.Size(408, 238);
            this.pluginStatus_TextBox.TabIndex = 6;
            this.pluginStatus_TextBox.WordWrap = false;
            // 
            // statusTitle_Label
            // 
            this.statusTitle_Label.AutoSize = true;
            this.statusTitle_Label.Location = new System.Drawing.Point(10, 7);
            this.statusTitle_Label.Name = "statusTitle_Label";
            this.statusTitle_Label.Size = new System.Drawing.Size(93, 15);
            this.statusTitle_Label.TabIndex = 5;
            this.statusTitle_Label.Text = "Execution Status";
            // 
            // DriverlessPrintingExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pluginStatus_TextBox);
            this.Controls.Add(this.statusTitle_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DriverlessPrintingExecutionControl";
            this.Size = new System.Drawing.Size(423, 261);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pluginStatus_TextBox;
        private System.Windows.Forms.Label statusTitle_Label;
    }
}
