using HP.ScalableTest.PluginSupport.Connectivity.UI;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    partial class ConfigurationParametersForm
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
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.info_Label = new System.Windows.Forms.Label();
            this.pruductName_Label = new System.Windows.Forms.Label();
            this.productName_ComboBox = new System.Windows.Forms.ComboBox();
            this.ipAddress_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.printer_ipAddressControl = new HP.ScalableTest.Framework.UI.IPAddressControl();
            this.details_GroupBox = new System.Windows.Forms.GroupBox();
            this.documentsPath_ComboBox = new System.Windows.Forms.ComboBox();
            this.print_PrintDriverSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.PrintDriverSelector();
            this.sitemapVersionSelector = new HP.ScalableTest.PluginSupport.Connectivity.UI.SitemapVersionSelector();
            this.details_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(174, 394);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 2;
            this.ok_Button.Text = "&OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(255, 394);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 3;
            this.cancel_Button.Text = "&Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // info_Label
            // 
            this.info_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.info_Label.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.info_Label.Location = new System.Drawing.Point(13, 13);
            this.info_Label.Name = "info_Label";
            this.info_Label.Size = new System.Drawing.Size(317, 37);
            this.info_Label.TabIndex = 2;
            this.info_Label.Text = "label1";
            // 
            // pruductName_Label
            // 
            this.pruductName_Label.AutoSize = true;
            this.pruductName_Label.Location = new System.Drawing.Point(6, 22);
            this.pruductName_Label.Name = "pruductName_Label";
            this.pruductName_Label.Size = new System.Drawing.Size(78, 13);
            this.pruductName_Label.TabIndex = 3;
            this.pruductName_Label.Text = "Product Name:";
            // 
            // productName_ComboBox
            // 
            this.productName_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productName_ComboBox.FormattingEnabled = true;
            this.productName_ComboBox.Location = new System.Drawing.Point(101, 19);
            this.productName_ComboBox.Name = "productName_ComboBox";
            this.productName_ComboBox.Size = new System.Drawing.Size(156, 21);
            this.productName_ComboBox.TabIndex = 0;
            this.productName_ComboBox.SelectedIndexChanged += new System.EventHandler(this.productName_ComboBox_SelectedIndexChanged);
            // 
            // ipAddress_Label
            // 
            this.ipAddress_Label.AutoSize = true;
            this.ipAddress_Label.Location = new System.Drawing.Point(6, 49);
            this.ipAddress_Label.Name = "ipAddress_Label";
            this.ipAddress_Label.Size = new System.Drawing.Size(61, 13);
            this.ipAddress_Label.TabIndex = 5;
            this.ipAddress_Label.Text = "IP Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Documents Path:";
            // 
            // printer_ipAddressControl
            // 
            this.printer_ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.printer_ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.printer_ipAddressControl.Location = new System.Drawing.Point(101, 46);
            this.printer_ipAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.printer_ipAddressControl.Name = "printer_ipAddressControl";
            this.printer_ipAddressControl.Size = new System.Drawing.Size(156, 20);
            this.printer_ipAddressControl.TabIndex = 1;
            this.printer_ipAddressControl.Text = "...";
            // 
            // details_GroupBox
            // 
            this.details_GroupBox.Controls.Add(this.documentsPath_ComboBox);
            this.details_GroupBox.Controls.Add(this.productName_ComboBox);
            this.details_GroupBox.Controls.Add(this.pruductName_Label);
            this.details_GroupBox.Controls.Add(this.printer_ipAddressControl);
            this.details_GroupBox.Controls.Add(this.ipAddress_Label);
            this.details_GroupBox.Controls.Add(this.label1);
            this.details_GroupBox.Location = new System.Drawing.Point(16, 53);
            this.details_GroupBox.Name = "details_GroupBox";
            this.details_GroupBox.Size = new System.Drawing.Size(314, 109);
            this.details_GroupBox.TabIndex = 0;
            this.details_GroupBox.TabStop = false;
            this.details_GroupBox.Text = "Details";
            // 
            // documentsPath_ComboBox
            // 
            this.documentsPath_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.documentsPath_ComboBox.FormattingEnabled = true;
            this.documentsPath_ComboBox.Location = new System.Drawing.Point(101, 75);
            this.documentsPath_ComboBox.Name = "documentsPath_ComboBox";
            this.documentsPath_ComboBox.Size = new System.Drawing.Size(156, 21);
            this.documentsPath_ComboBox.TabIndex = 2;
            // 
            // print_PrintDriverSelector
            // 
            this.print_PrintDriverSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.print_PrintDriverSelector.DriverModel = "";
            this.print_PrintDriverSelector.DriverPackagePath = "";
            this.print_PrintDriverSelector.Location = new System.Drawing.Point(16, 168);
            this.print_PrintDriverSelector.MinimumSize = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.Name = "print_PrintDriverSelector";
            this.print_PrintDriverSelector.PrinterFamily = null;
            this.print_PrintDriverSelector.PrinterName = null;
            this.print_PrintDriverSelector.Size = new System.Drawing.Size(314, 99);
            this.print_PrintDriverSelector.TabIndex = 1;
            // 
            // sitemapVersionSelector
            // 
            this.sitemapVersionSelector.AutoSize = true;
            this.sitemapVersionSelector.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.sitemapVersionSelector.Location = new System.Drawing.Point(16, 278);
            this.sitemapVersionSelector.Name = "sitemapVersionSelector";
            this.sitemapVersionSelector.PrinterFamily = "InkJet";
            this.sitemapVersionSelector.PrinterName = null;
            this.sitemapVersionSelector.SitemapPath = "";
            this.sitemapVersionSelector.SitemapVersion = "";
            this.sitemapVersionSelector.Size = new System.Drawing.Size(314, 102);
            this.sitemapVersionSelector.TabIndex = 4;
            // 
            // ConfigurationParametersForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(358, 430);
            this.Controls.Add(this.sitemapVersionSelector);
            this.Controls.Add(this.details_GroupBox);
            this.Controls.Add(this.print_PrintDriverSelector);
            this.Controls.Add(this.info_Label);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationParametersForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationParametersForm_FormClosing);
            this.Load += new System.EventHandler(this.ConfigurationParametersForm_Load);
            this.details_GroupBox.ResumeLayout(false);
            this.details_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
        private System.Windows.Forms.Label info_Label;
        private System.Windows.Forms.Label pruductName_Label;
        private System.Windows.Forms.ComboBox productName_ComboBox;
        private System.Windows.Forms.Label ipAddress_Label;
        private System.Windows.Forms.Label label1;
        private PrintDriverSelector print_PrintDriverSelector;
        private Framework.UI.IPAddressControl printer_ipAddressControl;
        private System.Windows.Forms.GroupBox details_GroupBox;
        private System.Windows.Forms.ComboBox documentsPath_ComboBox;
		private SitemapVersionSelector sitemapVersionSelector;
    }
}