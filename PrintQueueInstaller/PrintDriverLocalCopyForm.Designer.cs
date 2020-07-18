namespace HP.ScalableTest.Print.Utility
{
    partial class PrintDriverLocalCopyForm
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
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.close_Button = new System.Windows.Forms.Button();
            this.addDrivers_Button = new System.Windows.Forms.Button();
            this.printDriverAddControl = new HP.ScalableTest.UI.Framework.PrintDriverDownloadControl();
            this.copyTo_TextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.browse_Button = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 33);
            this.label1.TabIndex = 4;
            this.label1.Text = "This form allows Print Drivers to be copied locally from the Universal Print Driv" +
    "er share location.";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(43, 17);
            this.statusLabel.Text = "Status.";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 425);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(488, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // close_Button
            // 
            this.close_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(401, 384);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(75, 23);
            this.close_Button.TabIndex = 3;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // addDrivers_Button
            // 
            this.addDrivers_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addDrivers_Button.Location = new System.Drawing.Point(299, 384);
            this.addDrivers_Button.Name = "addDrivers_Button";
            this.addDrivers_Button.Size = new System.Drawing.Size(96, 23);
            this.addDrivers_Button.TabIndex = 2;
            this.addDrivers_Button.Text = "Add Selected";
            this.addDrivers_Button.UseVisualStyleBackColor = true;
            this.addDrivers_Button.Click += new System.EventHandler(this.addDrivers_Button_Click);
            // 
            // printDriverAddControl
            // 
            this.printDriverAddControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printDriverAddControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.printDriverAddControl.Location = new System.Drawing.Point(15, 69);
            this.printDriverAddControl.Name = "printDriverAddControl";
            this.printDriverAddControl.Size = new System.Drawing.Size(461, 309);
            this.printDriverAddControl.TabIndex = 5;
            // 
            // copyTo_TextBox
            // 
            this.copyTo_TextBox.Location = new System.Drawing.Point(65, 43);
            this.copyTo_TextBox.Name = "copyTo_TextBox";
            this.copyTo_TextBox.Size = new System.Drawing.Size(330, 20);
            this.copyTo_TextBox.TabIndex = 6;
            this.copyTo_TextBox.Text = "<Filled in from Resource file.>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Copy To";
            // 
            // browse_Button
            // 
            this.browse_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browse_Button.Location = new System.Drawing.Point(401, 41);
            this.browse_Button.Name = "browse_Button";
            this.browse_Button.Size = new System.Drawing.Size(75, 23);
            this.browse_Button.TabIndex = 8;
            this.browse_Button.Text = "Browse...";
            this.browse_Button.UseVisualStyleBackColor = true;
            this.browse_Button.Click += new System.EventHandler(this.browse_Button_Click);
            // 
            // PrintDriverLocalCopyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(488, 447);
            this.ControlBox = false;
            this.Controls.Add(this.browse_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.copyTo_TextBox);
            this.Controls.Add(this.printDriverAddControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.addDrivers_Button);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PrintDriverLocalCopyForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Download Print Driver";
            this.Shown += new System.EventHandler(this.PrintDriverLocalCopyForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.Button addDrivers_Button;
        private HP.ScalableTest.UI.Framework.PrintDriverDownloadControl printDriverAddControl;
        private System.Windows.Forms.TextBox copyTo_TextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browse_Button;
    }
}