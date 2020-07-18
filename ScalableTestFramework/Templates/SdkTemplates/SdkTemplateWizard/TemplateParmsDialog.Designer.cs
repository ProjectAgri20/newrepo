namespace SdkTemplateWizard
{
    partial class TemplateParmsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateParmsDialog));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.defaultNamespaceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.librarySearchButton = new System.Windows.Forms.Button();
            this.frameworkLibraryLocationTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.findFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(504, 79);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(423, 79);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // defaultNamespaceTextBox
            // 
            this.defaultNamespaceTextBox.Location = new System.Drawing.Point(229, 12);
            this.defaultNamespaceTextBox.Name = "defaultNamespaceTextBox";
            this.defaultNamespaceTextBox.Size = new System.Drawing.Size(350, 23);
            this.defaultNamespaceTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default namespace:";
            // 
            // librarySearchButton
            // 
            this.librarySearchButton.AutoSize = true;
            this.librarySearchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.librarySearchButton.Location = new System.Drawing.Point(553, 41);
            this.librarySearchButton.Name = "librarySearchButton";
            this.librarySearchButton.Size = new System.Drawing.Size(26, 25);
            this.librarySearchButton.TabIndex = 4;
            this.librarySearchButton.Text = "...";
            this.librarySearchButton.UseVisualStyleBackColor = true;
            this.librarySearchButton.Click += new System.EventHandler(this.librarySearchButton_Click);
            // 
            // frameworkLibraryLocationTextBox
            // 
            this.frameworkLibraryLocationTextBox.Location = new System.Drawing.Point(229, 43);
            this.frameworkLibraryLocationTextBox.Name = "frameworkLibraryLocationTextBox";
            this.frameworkLibraryLocationTextBox.Size = new System.Drawing.Size(318, 23);
            this.frameworkLibraryLocationTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Location of STF.Framework.dll library:";
            // 
            // findFileDialog
            // 
            this.findFileDialog.DefaultExt = "dll";
            this.findFileDialog.FileName = "STF.Framework.dll";
            this.findFileDialog.Filter = "STF Library|STF.Framework.dll";
            this.findFileDialog.Title = "Location of STF Framework library";
            // 
            // TemplateParmsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(591, 114);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.frameworkLibraryLocationTextBox);
            this.Controls.Add(this.librarySearchButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.defaultNamespaceTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateParmsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HP Inc. STB Plugin Template Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        internal System.Windows.Forms.TextBox defaultNamespaceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button librarySearchButton;
        internal System.Windows.Forms.TextBox frameworkLibraryLocationTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog findFileDialog;
    }
}