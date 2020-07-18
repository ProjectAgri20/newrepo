using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    partial class TelnetSnmpConfigurationControl
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
            this.groupBoxTestType = new System.Windows.Forms.GroupBox();
            this.radioButtonSNMP = new System.Windows.Forms.RadioButton();
            this.radioButtonTelnet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxIP = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.Telnet_sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.groupBoxTestType.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTestType
            // 
            this.groupBoxTestType.Controls.Add(this.radioButtonSNMP);
            this.groupBoxTestType.Controls.Add(this.radioButtonTelnet);
            this.groupBoxTestType.Location = new System.Drawing.Point(402, 403);
            this.groupBoxTestType.Name = "groupBoxTestType";
            this.groupBoxTestType.Size = new System.Drawing.Size(321, 56);
            this.groupBoxTestType.TabIndex = 3;
            this.groupBoxTestType.TabStop = false;
            this.groupBoxTestType.Text = "Test Type";
            // 
            // radioButtonSNMP
            // 
            this.radioButtonSNMP.AutoSize = true;
            this.radioButtonSNMP.Enabled = false;
            this.radioButtonSNMP.Location = new System.Drawing.Point(85, 31);
            this.radioButtonSNMP.Name = "radioButtonSNMP";
            this.radioButtonSNMP.Size = new System.Drawing.Size(56, 17);
            this.radioButtonSNMP.TabIndex = 2;
            this.radioButtonSNMP.Text = "SNMP";
            this.radioButtonSNMP.UseVisualStyleBackColor = true;
            this.radioButtonSNMP.CheckedChanged += new System.EventHandler(this.radioButtonSNMP_CheckedChanged);
            // 
            // radioButtonTelnet
            // 
            this.radioButtonTelnet.AutoSize = true;
            this.radioButtonTelnet.Location = new System.Drawing.Point(12, 31);
            this.radioButtonTelnet.Name = "radioButtonTelnet";
            this.radioButtonTelnet.Size = new System.Drawing.Size(55, 17);
            this.radioButtonTelnet.TabIndex = 1;
            this.radioButtonTelnet.Text = "Telnet";
            this.radioButtonTelnet.UseVisualStyleBackColor = true;
            this.radioButtonTelnet.CheckedChanged += new System.EventHandler(this.radioButtonTelnet_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP Address:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxIP);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(0, 406);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(396, 55);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Printer Details";
            // 
            // textBoxIP
            // 
            this.textBoxIP.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxIP.Location = new System.Drawing.Point(102, 19);
            this.textBoxIP.MinimumSize = new System.Drawing.Size(87, 20);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(139, 20);
            this.textBoxIP.TabIndex = 4;
            this.textBoxIP.Text = "...";
            // 
            // Telnet_sitemapVersionSelector
            // 
            this.Telnet_sitemapVersionSelector.AutoSize = true;
            this.Telnet_sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Telnet_sitemapVersionSelector.Location = new System.Drawing.Point(0, 480);
            this.Telnet_sitemapVersionSelector.Name = "Telnet_sitemapVersionSelector";
            this.Telnet_sitemapVersionSelector.PrinterFamily = null;
            this.Telnet_sitemapVersionSelector.PrinterName = null;
            this.Telnet_sitemapVersionSelector.SitemapPath = "";
            this.Telnet_sitemapVersionSelector.SitemapVersion = "";
            this.Telnet_sitemapVersionSelector.Size = new System.Drawing.Size(396, 102);
            this.Telnet_sitemapVersionSelector.TabIndex = 8;
            // 
            // TelnetSNMPEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxTestType);
            this.Controls.Add(this.Telnet_sitemapVersionSelector);
            this.Name = "TelnetSNMPConfigurationControl";
            this.Size = new System.Drawing.Size(729, 634);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.TelnetSnmpConfiguration_Validating);
            this.Controls.SetChildIndex(this.Telnet_sitemapVersionSelector, 0);
            this.Controls.SetChildIndex(this.groupBoxTestType, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.testCaseDetails_GroupBox, 0);
            this.groupBoxTestType.ResumeLayout(false);
            this.groupBoxTestType.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTestType;
        private System.Windows.Forms.RadioButton radioButtonSNMP;
        private System.Windows.Forms.RadioButton radioButtonTelnet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private Framework.UI.IPAddressControl textBoxIP;
        private SitemapVersionSelector Telnet_sitemapVersionSelector;
    }
}
