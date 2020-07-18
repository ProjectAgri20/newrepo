namespace HP.ScalableTest
{
    partial class DomainAccountEditForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DomainAccountEditForm));
            this.poolNameLabel = new System.Windows.Forms.Label();
            this.formatLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.endTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.poolNameTextBox = new System.Windows.Forms.TextBox();
            this.startLabel = new System.Windows.Forms.Label();
            this.formatTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.poolFormat_PictureBox = new System.Windows.Forms.PictureBox();
            this.startTextBox = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.poolFormat_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // poolNameLabel
            // 
            this.poolNameLabel.Location = new System.Drawing.Point(43, 22);
            this.poolNameLabel.Name = "poolNameLabel";
            this.poolNameLabel.Size = new System.Drawing.Size(96, 24);
            this.poolNameLabel.TabIndex = 0;
            this.poolNameLabel.Text = "Pool Id";
            this.poolNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formatLabel
            // 
            this.formatLabel.Location = new System.Drawing.Point(2, 52);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(137, 24);
            this.formatLabel.TabIndex = 2;
            this.formatLabel.Text = "Username Format";
            this.formatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endLabel
            // 
            this.endLabel.Location = new System.Drawing.Point(17, 112);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(122, 24);
            this.endLabel.TabIndex = 6;
            this.endLabel.Text = "End Index";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // endTextBox
            // 
            this.endTextBox.Location = new System.Drawing.Point(145, 112);
            this.endTextBox.Name = "endTextBox";
            this.endTextBox.Size = new System.Drawing.Size(62, 23);
            this.endTextBox.TabIndex = 7;
            this.endTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.EndTextBox_Validating);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(331, 227);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(225, 227);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 10;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // poolNameTextBox
            // 
            this.poolNameTextBox.Location = new System.Drawing.Point(145, 22);
            this.poolNameTextBox.Name = "poolNameTextBox";
            this.poolNameTextBox.Size = new System.Drawing.Size(180, 23);
            this.poolNameTextBox.TabIndex = 1;
            this.poolNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PoolNameTextBox_Validating);
            // 
            // startLabel
            // 
            this.startLabel.Location = new System.Drawing.Point(17, 82);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(122, 24);
            this.startLabel.TabIndex = 4;
            this.startLabel.Text = "Start Index";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formatTextBox
            // 
            this.formatTextBox.Location = new System.Drawing.Point(145, 52);
            this.formatTextBox.Name = "formatTextBox";
            this.formatTextBox.Size = new System.Drawing.Size(180, 23);
            this.formatTextBox.TabIndex = 3;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.CausesValidation = false;
            this.descriptionTextBox.Location = new System.Drawing.Point(147, 142);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(284, 23);
            this.descriptionTextBox.TabIndex = 9;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(19, 142);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(122, 24);
            this.descriptionLabel.TabIndex = 8;
            this.descriptionLabel.Text = "Pool Name";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // poolFormat_PictureBox
            // 
            this.poolFormat_PictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("poolFormat_PictureBox.BackgroundImage")));
            this.poolFormat_PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.poolFormat_PictureBox.Location = new System.Drawing.Point(331, 57);
            this.poolFormat_PictureBox.Name = "poolFormat_PictureBox";
            this.poolFormat_PictureBox.Size = new System.Drawing.Size(20, 18);
            this.poolFormat_PictureBox.TabIndex = 107;
            this.poolFormat_PictureBox.TabStop = false;
            this.poolFormat_PictureBox.Click += new System.EventHandler(this.virtualWorkerFormat_Click);
            // 
            // startTextBox
            // 
            this.startTextBox.Location = new System.Drawing.Point(145, 82);
            this.startTextBox.Name = "startTextBox";
            this.startTextBox.Size = new System.Drawing.Size(62, 23);
            this.startTextBox.TabIndex = 5;
            // 
            // DomainAccountEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(443, 271);
            this.Controls.Add(this.startTextBox);
            this.Controls.Add(this.poolFormat_PictureBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.formatTextBox);
            this.Controls.Add(this.startLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.endLabel);
            this.Controls.Add(this.endTextBox);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.poolNameLabel);
            this.Controls.Add(this.poolNameTextBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DomainAccountEditForm";
            this.Text = "Virtual Worker Account Pool";
            this.Load += new System.EventHandler(this.PrinterEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.poolFormat_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label poolNameLabel;
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.TextBox endTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox poolNameTextBox;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.TextBox formatTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.PictureBox poolFormat_PictureBox;
        private System.Windows.Forms.TextBox startTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}