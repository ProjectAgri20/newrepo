namespace HP.ScalableTest.Framework.UI
{
    partial class TextDisplayDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextDisplayDialog));
            this.close_Button = new System.Windows.Forms.Button();
            this.display_TextBox = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.save_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copy_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(846, 484);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 2;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // display_TextBox
            // 
            this.display_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.display_TextBox.Location = new System.Drawing.Point(14, 32);
            this.display_TextBox.Multiline = true;
            this.display_TextBox.Name = "display_TextBox";
            this.display_TextBox.Size = new System.Drawing.Size(907, 439);
            this.display_TextBox.TabIndex = 1;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.save_ToolStripButton,
            this.copy_ToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(933, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // save_ToolStripButton
            // 
            this.save_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("save_ToolStripButton.Image")));
            this.save_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.save_ToolStripButton.Name = "save_ToolStripButton";
            this.save_ToolStripButton.Size = new System.Drawing.Size(86, 22);
            this.save_ToolStripButton.Text = "Save to File";
            this.save_ToolStripButton.Click += new System.EventHandler(this.save_ToolStripButton_Click);
            // 
            // copy_ToolStripButton
            // 
            this.copy_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copy_ToolStripButton.Image")));
            this.copy_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copy_ToolStripButton.Name = "copy_ToolStripButton";
            this.copy_ToolStripButton.Size = new System.Drawing.Size(124, 22);
            this.copy_ToolStripButton.Text = "Copy to Clipboard";
            this.copy_ToolStripButton.Click += new System.EventHandler(this.copy_ToolStripButton_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "Save Text Data";
            // 
            // TextDisplayDialog
            // 
            this.AcceptButton = this.close_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.display_TextBox);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TextDisplayDialog";
            this.Text = "Text Viewer";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.TextBox display_TextBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton save_ToolStripButton;
        private System.Windows.Forms.ToolStripButton copy_ToolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}