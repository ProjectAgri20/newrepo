using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.Discovery
{
    partial class DiscoveryConfigurationControl
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
            this.ipv4Address_IPAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.printerDetails_GroupBox = new System.Windows.Forms.GroupBox();
            this.ipv4Address_Label = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.discoveryPlugin_SitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.printDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.printerDetails_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipv4Address_IPAddressControl
            // 
            this.ipv4Address_IPAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipv4Address_IPAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipv4Address_IPAddressControl.Location = new System.Drawing.Point(103, 21);
            this.ipv4Address_IPAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipv4Address_IPAddressControl.Name = "ipv4Address_IPAddressControl";
            this.ipv4Address_IPAddressControl.Size = new System.Drawing.Size(140, 20);
            this.ipv4Address_IPAddressControl.TabIndex = 0;
            this.ipv4Address_IPAddressControl.Text = "...";
            // 
            // printerDetails_GroupBox
            // 
            this.printerDetails_GroupBox.Controls.Add(this.ipv4Address_Label);
            this.printerDetails_GroupBox.Controls.Add(this.ipv4Address_IPAddressControl);
            this.printerDetails_GroupBox.Location = new System.Drawing.Point(7, 386);
            this.printerDetails_GroupBox.Name = "printerDetails_GroupBox";
            this.printerDetails_GroupBox.Size = new System.Drawing.Size(314, 62);
            this.printerDetails_GroupBox.TabIndex = 0;
            this.printerDetails_GroupBox.TabStop = false;
            this.printerDetails_GroupBox.Text = "Printer Details";
            // 
            // ipv4Address_Label
            // 
            this.ipv4Address_Label.AutoSize = true;
            this.ipv4Address_Label.Location = new System.Drawing.Point(24, 25);
            this.ipv4Address_Label.Name = "ipv4Address_Label";
            this.ipv4Address_Label.Size = new System.Drawing.Size(73, 13);
            this.ipv4Address_Label.TabIndex = 7;
            this.ipv4Address_Label.Text = "IPv4 Address:";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // discoveryPlugin_SitemapVersionSelector
            // 
            this.discoveryPlugin_SitemapVersionSelector.AutoSize = true;
            this.discoveryPlugin_SitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.discoveryPlugin_SitemapVersionSelector.Location = new System.Drawing.Point(349, 386);
            this.discoveryPlugin_SitemapVersionSelector.Name = "discoveryPlugin_SitemapVersionSelector";
            this.discoveryPlugin_SitemapVersionSelector.PrinterFamily = null;
            this.discoveryPlugin_SitemapVersionSelector.PrinterName = null;
            this.discoveryPlugin_SitemapVersionSelector.SitemapPath = "";
            this.discoveryPlugin_SitemapVersionSelector.SitemapVersion = "";
            this.discoveryPlugin_SitemapVersionSelector.Size = new System.Drawing.Size(374, 100);
            this.discoveryPlugin_SitemapVersionSelector.TabIndex = 55;
            // 
            // printDriverSelector
            // 
            this.printDriverSelector.DriverModel = "";
            this.printDriverSelector.DriverPackagePath = "";
            this.printDriverSelector.Location = new System.Drawing.Point(7, 454);
            this.printDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.printDriverSelector.Name = "printDriverSelector";
            this.printDriverSelector.PrinterFamily = null;
            this.printDriverSelector.PrinterName = null;
            this.printDriverSelector.Size = new System.Drawing.Size(314, 99);
            this.printDriverSelector.TabIndex = 56;
            this.printDriverSelector.Load += new System.EventHandler(this.printDriverSelector_Load);
            // 
            // DiscoveryConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printDriverSelector);
            this.Controls.Add(this.discoveryPlugin_SitemapVersionSelector);
            this.Controls.Add(this.printerDetails_GroupBox);
            this.Name = "DiscoveryConfigurationControl";
            this.Size = new System.Drawing.Size(750, 564);
            this.Controls.SetChildIndex(this.printerDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.discoveryPlugin_SitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.printDriverSelector, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.printerDetails_GroupBox.ResumeLayout(false);
            this.printerDetails_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        
        HP.ScalableTest.Framework.UI.IPAddressControl ipv4Address_IPAddressControl;
        private System.Windows.Forms.GroupBox printerDetails_GroupBox;
		private System.Windows.Forms.Label ipv4Address_Label;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private SitemapVersionSelector discoveryPlugin_SitemapVersionSelector;
        private PrintDriverSelector printDriverSelector;
    }
}
