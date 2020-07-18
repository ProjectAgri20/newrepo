namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportWelcomeControl
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
            this.importFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileOpenButton = new System.Windows.Forms.Button();
            this.welcomeRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // importFileTextBox
            // 
            this.importFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importFileTextBox.Location = new System.Drawing.Point(98, 287);
            this.importFileTextBox.Name = "importFileTextBox";
            this.importFileTextBox.Size = new System.Drawing.Size(503, 24);
            this.importFileTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(15, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Import File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // fileOpenButton
            // 
            this.fileOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fileOpenButton.Location = new System.Drawing.Point(607, 285);
            this.fileOpenButton.Name = "fileOpenButton";
            this.fileOpenButton.Size = new System.Drawing.Size(38, 28);
            this.fileOpenButton.TabIndex = 3;
            this.fileOpenButton.Text = "...";
            this.fileOpenButton.UseVisualStyleBackColor = true;
            this.fileOpenButton.Click += new System.EventHandler(this.fileOpenButton_Click);
            // 
            // welcomeRichTextBox
            // 
            this.welcomeRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcomeRichTextBox.BackColor = System.Drawing.Color.White;
            this.welcomeRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.welcomeRichTextBox.Location = new System.Drawing.Point(18, 16);
            this.welcomeRichTextBox.Name = "welcomeRichTextBox";
            this.welcomeRichTextBox.Size = new System.Drawing.Size(626, 263);
            this.welcomeRichTextBox.TabIndex = 4;
            this.welcomeRichTextBox.Text = "";
            // 
            // ImportWelcomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.welcomeRichTextBox);
            this.Controls.Add(this.fileOpenButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.importFileTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportWelcomeControl";
            this.Size = new System.Drawing.Size(662, 367);
            this.Load += new System.EventHandler(this.OpenFileControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox importFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button fileOpenButton;
        private System.Windows.Forms.RichTextBox welcomeRichTextBox;

    }
}
