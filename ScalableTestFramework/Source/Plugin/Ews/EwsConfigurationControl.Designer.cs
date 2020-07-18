using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.Ews
{
    partial class EwsConfigurationControl
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
            this.groupBoxBrowser = new System.Windows.Forms.GroupBox();
            this.radioButtonChrome = new System.Windows.Forms.RadioButton();
            this.radioButtonOpera = new System.Windows.Forms.RadioButton();
            this.radioButtonFirefox = new System.Windows.Forms.RadioButton();
            this.radioButtonSafari = new System.Windows.Forms.RadioButton();
            this.radioButtonIE = new System.Windows.Forms.RadioButton();
            this.groupBoxPrinterDetails = new System.Windows.Forms.GroupBox();
            this.textBoxIP = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.label2 = new System.Windows.Forms.Label();
            this.EWS_sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.groupBoxBrowser.SuspendLayout();
            this.groupBoxPrinterDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxBrowser
            // 
            this.groupBoxBrowser.Controls.Add(this.radioButtonChrome);
            this.groupBoxBrowser.Controls.Add(this.radioButtonOpera);
            this.groupBoxBrowser.Controls.Add(this.radioButtonFirefox);
            this.groupBoxBrowser.Controls.Add(this.radioButtonSafari);
            this.groupBoxBrowser.Controls.Add(this.radioButtonIE);
            this.groupBoxBrowser.Location = new System.Drawing.Point(3, 510);
            this.groupBoxBrowser.Name = "groupBoxBrowser";
            this.groupBoxBrowser.Size = new System.Drawing.Size(720, 59);
            this.groupBoxBrowser.TabIndex = 3;
            this.groupBoxBrowser.TabStop = false;
            this.groupBoxBrowser.Text = "Browser Type";
            // 
            // radioButtonChrome
            // 
            this.radioButtonChrome.AutoSize = true;
            this.radioButtonChrome.Location = new System.Drawing.Point(262, 26);
            this.radioButtonChrome.Name = "radioButtonChrome";
            this.radioButtonChrome.Size = new System.Drawing.Size(61, 17);
            this.radioButtonChrome.TabIndex = 4;
            this.radioButtonChrome.Text = "Chrome";
            this.radioButtonChrome.UseVisualStyleBackColor = true;
            this.radioButtonChrome.CheckedChanged += new System.EventHandler(this.radioButtonChrome_CheckedChanged);
            // 
            // radioButtonOpera
            // 
            this.radioButtonOpera.AutoSize = true;
            this.radioButtonOpera.Location = new System.Drawing.Point(451, 26);
            this.radioButtonOpera.Name = "radioButtonOpera";
            this.radioButtonOpera.Size = new System.Drawing.Size(54, 17);
            this.radioButtonOpera.TabIndex = 3;
            this.radioButtonOpera.Text = "Opera";
            this.radioButtonOpera.UseVisualStyleBackColor = true;
            this.radioButtonOpera.CheckedChanged += new System.EventHandler(this.radioButtonOpera_CheckedChanged);
            // 
            // radioButtonFirefox
            // 
            this.radioButtonFirefox.AutoSize = true;
            this.radioButtonFirefox.Location = new System.Drawing.Point(168, 24);
            this.radioButtonFirefox.Name = "radioButtonFirefox";
            this.radioButtonFirefox.Size = new System.Drawing.Size(56, 17);
            this.radioButtonFirefox.TabIndex = 2;
            this.radioButtonFirefox.Text = "Firefox";
            this.radioButtonFirefox.UseVisualStyleBackColor = true;
            this.radioButtonFirefox.CheckedChanged += new System.EventHandler(this.radioButtonFirefox_CheckedChanged);
            // 
            // radioButtonSafari
            // 
            this.radioButtonSafari.AutoSize = true;
            this.radioButtonSafari.Location = new System.Drawing.Point(361, 26);
            this.radioButtonSafari.Name = "radioButtonSafari";
            this.radioButtonSafari.Size = new System.Drawing.Size(52, 17);
            this.radioButtonSafari.TabIndex = 1;
            this.radioButtonSafari.Text = "Safari";
            this.radioButtonSafari.UseVisualStyleBackColor = true;
            this.radioButtonSafari.CheckedChanged += new System.EventHandler(this.radioButtonSafari_CheckedChanged);
            // 
            // radioButtonIE
            // 
            this.radioButtonIE.AutoSize = true;
            this.radioButtonIE.Checked = true;
            this.radioButtonIE.Location = new System.Drawing.Point(28, 24);
            this.radioButtonIE.Name = "radioButtonIE";
            this.radioButtonIE.Size = new System.Drawing.Size(102, 17);
            this.radioButtonIE.TabIndex = 0;
            this.radioButtonIE.TabStop = true;
            this.radioButtonIE.Text = "Internet Explorer";
            this.radioButtonIE.UseVisualStyleBackColor = true;
            this.radioButtonIE.CheckedChanged += new System.EventHandler(this.radioButtonIE_CheckedChanged);
            // 
            // groupBoxPrinterDetails
            // 
            this.groupBoxPrinterDetails.Controls.Add(this.textBoxIP);
            this.groupBoxPrinterDetails.Controls.Add(this.label2);
            this.groupBoxPrinterDetails.Location = new System.Drawing.Point(3, 402);
            this.groupBoxPrinterDetails.Name = "groupBoxPrinterDetails";
            this.groupBoxPrinterDetails.Size = new System.Drawing.Size(334, 102);
            this.groupBoxPrinterDetails.TabIndex = 4;
            this.groupBoxPrinterDetails.TabStop = false;
            this.groupBoxPrinterDetails.Text = "PrinterDetails";
            // 
            // textBoxIP
            // 
            this.textBoxIP.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxIP.Location = new System.Drawing.Point(99, 16);
            this.textBoxIP.MinimumSize = new System.Drawing.Size(87, 20);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(127, 20);
            this.textBoxIP.TabIndex = 6;
            this.textBoxIP.Text = "...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IPv4 Address:";
            // 
            // EWS_sitemapVersionSelector
            // 
            this.EWS_sitemapVersionSelector.AutoSize = true;
            this.EWS_sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.EWS_sitemapVersionSelector.Location = new System.Drawing.Point(343, 402);
            this.EWS_sitemapVersionSelector.Name = "EWS_sitemapVersionSelector";
            this.EWS_sitemapVersionSelector.PrinterFamily = null;
            this.EWS_sitemapVersionSelector.PrinterName = null;
            this.EWS_sitemapVersionSelector.SitemapPath = "";
            this.EWS_sitemapVersionSelector.SitemapVersion = "";
            this.EWS_sitemapVersionSelector.Size = new System.Drawing.Size(380, 102);
            this.EWS_sitemapVersionSelector.TabIndex = 6;
            // 
            // EWSEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EWS_sitemapVersionSelector);
            this.Controls.Add(this.groupBoxPrinterDetails);
            this.Controls.Add(this.groupBoxBrowser);
            this.Name = "EWSConfigurationControl";
            this.Size = new System.Drawing.Size(729, 624);
            this.Controls.SetChildIndex(this.groupBoxBrowser, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.Controls.SetChildIndex(this.groupBoxPrinterDetails, 0);
            this.Controls.SetChildIndex(this.EWS_sitemapVersionSelector, 0);
            //((System.ComponentModel.ISupportInitialize)(this.fieldValidator)).EndInit();
            this.groupBoxBrowser.ResumeLayout(false);
            this.groupBoxBrowser.PerformLayout();
            this.groupBoxPrinterDetails.ResumeLayout(false);
            this.groupBoxPrinterDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBrowser;
        private System.Windows.Forms.GroupBox groupBoxPrinterDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonChrome;
        private System.Windows.Forms.RadioButton radioButtonOpera;
        private System.Windows.Forms.RadioButton radioButtonFirefox;
        private System.Windows.Forms.RadioButton radioButtonSafari;
        private System.Windows.Forms.RadioButton radioButtonIE;
        private Framework.UI.IPAddressControl textBoxIP;
        private SitemapVersionSelector EWS_sitemapVersionSelector;
    }
}
