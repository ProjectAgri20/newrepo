namespace HP.ScalableTest.LabConsole
{
    partial class PrintDriverAddForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintDriverAddForm));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.close_Button = new System.Windows.Forms.Button();
            this.addDrivers_Button = new System.Windows.Forms.Button();
            this.downloadControl = new HP.ScalableTest.UI.Framework.PrintDriverDownloadControl();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder");
            this.imageList.Images.SetKeyName(1, "Printer");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 66);
            this.label1.TabIndex = 4;
            this.label1.Text = "This form allows Print Drivers to be added to the STF environment from the Univer" +
    "sal Print Driver share location.  To conserve space in the environment, please a" +
    "dd only those print drivers you need.\r\n";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(52, 20);
            this.statusLabel.Text = "Status.";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 594);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(577, 25);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // close_Button
            // 
            this.close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close_Button.Location = new System.Drawing.Point(447, 532);
            this.close_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(112, 32);
            this.close_Button.TabIndex = 3;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            // 
            // addDrivers_Button
            // 
            this.addDrivers_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addDrivers_Button.Location = new System.Drawing.Point(294, 532);
            this.addDrivers_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.addDrivers_Button.Name = "addDrivers_Button";
            this.addDrivers_Button.Size = new System.Drawing.Size(144, 32);
            this.addDrivers_Button.TabIndex = 2;
            this.addDrivers_Button.Text = "Add Selected";
            this.addDrivers_Button.UseVisualStyleBackColor = true;
            this.addDrivers_Button.Click += new System.EventHandler(this.addDrivers_Button_Click);
            // 
            // downloadControl
            // 
            this.downloadControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.downloadControl.DisplayRoot = false;
            this.downloadControl.Location = new System.Drawing.Point(22, 78);
            this.downloadControl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.downloadControl.Name = "downloadControl";
            this.downloadControl.RepositoryPath = "";
            this.downloadControl.Size = new System.Drawing.Size(539, 446);
            this.downloadControl.TabIndex = 5;
            // 
            // PrintDriverAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close_Button;
            this.ClientSize = new System.Drawing.Size(577, 619);
            this.ControlBox = false;
            this.Controls.Add(this.downloadControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.close_Button);
            this.Controls.Add(this.addDrivers_Button);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PrintDriverAddForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Print Driver";
            this.Shown += new System.EventHandler(this.PrintDriverConfigForm_Shown);
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
        private HP.ScalableTest.UI.Framework.PrintDriverDownloadControl downloadControl;

    }
}