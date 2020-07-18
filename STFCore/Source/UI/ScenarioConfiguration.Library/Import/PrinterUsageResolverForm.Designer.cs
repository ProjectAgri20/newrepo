namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class PrinterUsageResolverForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterUsageResolverForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.oldNameLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.oldModelValueLabel = new System.Windows.Forms.Label();
            this.oldNameValueLabel = new System.Windows.Forms.Label();
            this.oldDescriptionLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.newModelValueLabel = new System.Windows.Forms.Label();
            this.newNameValueLabel = new System.Windows.Forms.Label();
            this.newDescriptionLabel = new System.Windows.Forms.Label();
            this.newNameLabel = new System.Windows.Forms.Label();
            this.selectDeviceLinkLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(424, 305);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(84, 28);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(334, 305);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(84, 28);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // oldNameLabel
            // 
            this.oldNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.oldNameLabel.Location = new System.Drawing.Point(17, 31);
            this.oldNameLabel.Name = "oldNameLabel";
            this.oldNameLabel.Size = new System.Drawing.Size(98, 19);
            this.oldNameLabel.TabIndex = 3;
            this.oldNameLabel.Text = "Name";
            this.oldNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.oldModelValueLabel);
            this.groupBox1.Controls.Add(this.oldNameValueLabel);
            this.groupBox1.Controls.Add(this.oldDescriptionLabel);
            this.groupBox1.Controls.Add(this.oldNameLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unresolved Device";
            // 
            // oldModelValueLabel
            // 
            this.oldModelValueLabel.Location = new System.Drawing.Point(121, 60);
            this.oldModelValueLabel.Name = "oldModelValueLabel";
            this.oldModelValueLabel.Size = new System.Drawing.Size(369, 19);
            this.oldModelValueLabel.TabIndex = 6;
            // 
            // oldNameValueLabel
            // 
            this.oldNameValueLabel.Location = new System.Drawing.Point(121, 31);
            this.oldNameValueLabel.Name = "oldNameValueLabel";
            this.oldNameValueLabel.Size = new System.Drawing.Size(369, 19);
            this.oldNameValueLabel.TabIndex = 5;
            // 
            // oldDescriptionLabel
            // 
            this.oldDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.oldDescriptionLabel.Location = new System.Drawing.Point(17, 60);
            this.oldDescriptionLabel.Name = "oldDescriptionLabel";
            this.oldDescriptionLabel.Size = new System.Drawing.Size(98, 19);
            this.oldDescriptionLabel.TabIndex = 4;
            this.oldDescriptionLabel.Text = "Description";
            this.oldDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newModelValueLabel);
            this.groupBox2.Controls.Add(this.newNameValueLabel);
            this.groupBox2.Controls.Add(this.newDescriptionLabel);
            this.groupBox2.Controls.Add(this.newNameLabel);
            this.groupBox2.Controls.Add(this.selectDeviceLinkLabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(496, 148);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Replacement Device";
            // 
            // newModelValueLabel
            // 
            this.newModelValueLabel.Location = new System.Drawing.Point(121, 64);
            this.newModelValueLabel.Name = "newModelValueLabel";
            this.newModelValueLabel.Size = new System.Drawing.Size(369, 19);
            this.newModelValueLabel.TabIndex = 11;
            // 
            // newNameValueLabel
            // 
            this.newNameValueLabel.Location = new System.Drawing.Point(121, 35);
            this.newNameValueLabel.Name = "newNameValueLabel";
            this.newNameValueLabel.Size = new System.Drawing.Size(369, 19);
            this.newNameValueLabel.TabIndex = 10;
            // 
            // newDescriptionLabel
            // 
            this.newDescriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.newDescriptionLabel.Location = new System.Drawing.Point(17, 64);
            this.newDescriptionLabel.Name = "newDescriptionLabel";
            this.newDescriptionLabel.Size = new System.Drawing.Size(98, 19);
            this.newDescriptionLabel.TabIndex = 9;
            this.newDescriptionLabel.Text = "Description";
            this.newDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // newNameLabel
            // 
            this.newNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.newNameLabel.Location = new System.Drawing.Point(17, 35);
            this.newNameLabel.Name = "newNameLabel";
            this.newNameLabel.Size = new System.Drawing.Size(98, 19);
            this.newNameLabel.TabIndex = 8;
            this.newNameLabel.Text = "Name";
            this.newNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // selectDeviceLinkLabel
            // 
            this.selectDeviceLinkLabel.AutoSize = true;
            this.selectDeviceLinkLabel.Location = new System.Drawing.Point(56, 122);
            this.selectDeviceLinkLabel.Name = "selectDeviceLinkLabel";
            this.selectDeviceLinkLabel.Size = new System.Drawing.Size(385, 18);
            this.selectDeviceLinkLabel.TabIndex = 6;
            this.selectDeviceLinkLabel.TabStop = true;
            this.selectDeviceLinkLabel.Text = "Select an existing device from the Asset Inventory System";
            this.selectDeviceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.selectDeviceLinkLabel_LinkClicked);
            // 
            // PrinterUsageResolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 345);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterUsageResolverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unresolved Device";
            this.Load += new System.EventHandler(this.PrinterUsageResolverForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label oldNameLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label oldDescriptionLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel selectDeviceLinkLabel;
        private System.Windows.Forms.Label newModelValueLabel;
        private System.Windows.Forms.Label newNameValueLabel;
        private System.Windows.Forms.Label newDescriptionLabel;
        private System.Windows.Forms.Label newNameLabel;
        protected System.Windows.Forms.Label oldNameValueLabel;
        protected System.Windows.Forms.Label oldModelValueLabel;
    }
}