namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ExportOptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportOptionsForm));
            this.includePrintersCheckbox = new System.Windows.Forms.CheckBox();
            this.includeDocumentsCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scenarioNameLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // includePrintersCheckbox
            // 
            this.includePrintersCheckbox.AutoSize = true;
            this.includePrintersCheckbox.Location = new System.Drawing.Point(23, 78);
            this.includePrintersCheckbox.Name = "includePrintersCheckbox";
            this.includePrintersCheckbox.Size = new System.Drawing.Size(456, 24);
            this.includePrintersCheckbox.TabIndex = 0;
            this.includePrintersCheckbox.Text = "Include Printer Definitions used by this scenario in the export file";
            this.includePrintersCheckbox.UseVisualStyleBackColor = true;
            // 
            // includeDocumentsCheckbox
            // 
            this.includeDocumentsCheckbox.AutoSize = true;
            this.includeDocumentsCheckbox.Location = new System.Drawing.Point(23, 109);
            this.includeDocumentsCheckbox.Name = "includeDocumentsCheckbox";
            this.includeDocumentsCheckbox.Size = new System.Drawing.Size(443, 24);
            this.includeDocumentsCheckbox.TabIndex = 1;
            this.includeDocumentsCheckbox.Text = "Include Test Documents used by this scenario in the export file";
            this.includeDocumentsCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 33);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scenario:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // scenarioNameLabel
            // 
            this.scenarioNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic);
            this.scenarioNameLabel.Location = new System.Drawing.Point(96, 20);
            this.scenarioNameLabel.Name = "scenarioNameLabel";
            this.scenarioNameLabel.Size = new System.Drawing.Size(336, 33);
            this.scenarioNameLabel.TabIndex = 3;
            this.scenarioNameLabel.Text = "<Scenario Name>";
            this.scenarioNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(396, 155);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(290, 155);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(20, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(476, 2);
            this.panel1.TabIndex = 6;
            // 
            // ExportOptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 199);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.scenarioNameLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.includeDocumentsCheckbox);
            this.Controls.Add(this.includePrintersCheckbox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Options";
            this.Load += new System.EventHandler(this.ExportOptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox includePrintersCheckbox;
        private System.Windows.Forms.CheckBox includeDocumentsCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label scenarioNameLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel panel1;
    }
}