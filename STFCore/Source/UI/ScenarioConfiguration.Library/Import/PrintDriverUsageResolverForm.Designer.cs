namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class PrintDriverUsageResolverForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exportNameValue = new System.Windows.Forms.Label();
            this.exportPackageLabel = new System.Windows.Forms.Label();
            this.exportVersionValue = new System.Windows.Forms.Label();
            this.exportModelLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.printDriverSelectionControl = new HP.ScalableTest.UI.Framework.PrintDriverSelectionControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.exportNameValue);
            this.groupBox1.Controls.Add(this.exportPackageLabel);
            this.groupBox1.Controls.Add(this.exportVersionValue);
            this.groupBox1.Controls.Add(this.exportModelLabel);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 111);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Unresolved Device Driver";
            // 
            // exportPackageValue
            // 
            this.exportNameValue.Location = new System.Drawing.Point(136, 31);
            this.exportNameValue.Name = "exportPackageValue";
            this.exportNameValue.Size = new System.Drawing.Size(415, 27);
            this.exportNameValue.TabIndex = 10;
            // 
            // exportPackageLabel
            // 
            this.exportPackageLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.exportPackageLabel.Location = new System.Drawing.Point(19, 31);
            this.exportPackageLabel.Name = "exportPackageLabel";
            this.exportPackageLabel.Size = new System.Drawing.Size(110, 27);
            this.exportPackageLabel.TabIndex = 9;
            this.exportPackageLabel.Text = "Driver Package";
            this.exportPackageLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // exportModelValue
            // 
            this.exportVersionValue.Location = new System.Drawing.Point(136, 58);
            this.exportVersionValue.Name = "exportModelValue";
            this.exportVersionValue.Size = new System.Drawing.Size(415, 27);
            this.exportVersionValue.TabIndex = 5;
            // 
            // exportModelLabel
            // 
            this.exportModelLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.exportModelLabel.Location = new System.Drawing.Point(20, 58);
            this.exportModelLabel.Name = "exportModelLabel";
            this.exportModelLabel.Size = new System.Drawing.Size(110, 27);
            this.exportModelLabel.TabIndex = 3;
            this.exportModelLabel.Text = "Driver Model";
            this.exportModelLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(401, 296);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(84, 28);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(491, 296);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(84, 28);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // printDriverSelectionControl
            // 
            this.printDriverSelectionControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.printDriverSelectionControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.printDriverSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.printDriverSelectionControl.Location = new System.Drawing.Point(13, 38);
            this.printDriverSelectionControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.printDriverSelectionControl.Name = "printDriverSelectionControl";
            this.printDriverSelectionControl.Size = new System.Drawing.Size(529, 72);
            this.printDriverSelectionControl.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.printDriverSelectionControl);
            this.groupBox2.Location = new System.Drawing.Point(14, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(558, 128);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Replacement Device Driver";
            // 
            // PrintDriverUsageResolverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 336);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "PrintDriverUsageResolverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unresolved Device Driver";
            this.Load += new System.EventHandler(this.PrintDriverUsageResolverForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label exportVersionValue;
        private System.Windows.Forms.Label exportModelLabel;
        private System.Windows.Forms.Label exportNameValue;
        private System.Windows.Forms.Label exportPackageLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Framework.PrintDriverSelectionControl printDriverSelectionControl;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}