namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ImportCompositeControl
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
            this.scenarioLabel = new System.Windows.Forms.Label();
            this.descLabel = new System.Windows.Forms.Label();
            this.importMainPanel = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.scenarioNameTextBox = new System.Windows.Forms.TextBox();
            this.splitterPanel = new System.Windows.Forms.Panel();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scenarioLabel
            // 
            this.scenarioLabel.Location = new System.Drawing.Point(10, 11);
            this.scenarioLabel.Name = "scenarioLabel";
            this.scenarioLabel.Size = new System.Drawing.Size(114, 25);
            this.scenarioLabel.TabIndex = 0;
            this.scenarioLabel.Text = "Scenario Name";
            this.scenarioLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descLabel
            // 
            this.descLabel.Location = new System.Drawing.Point(13, 44);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(111, 25);
            this.descLabel.TabIndex = 2;
            this.descLabel.Text = "Description";
            this.descLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // importMainPanel
            // 
            this.importMainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.importMainPanel.Location = new System.Drawing.Point(0, 88);
            this.importMainPanel.Name = "importMainPanel";
            this.importMainPanel.Size = new System.Drawing.Size(774, 339);
            this.importMainPanel.TabIndex = 6;
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.Controls.Add(this.splitterPanel);
            this.headerPanel.Controls.Add(this.descriptionTextBox);
            this.headerPanel.Controls.Add(this.scenarioNameTextBox);
            this.headerPanel.Controls.Add(this.scenarioLabel);
            this.headerPanel.Controls.Add(this.descLabel);
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(774, 87);
            this.headerPanel.TabIndex = 7;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(130, 44);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(604, 27);
            this.descriptionTextBox.TabIndex = 7;
            // 
            // scenarioNameTextBox
            // 
            this.scenarioNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioNameTextBox.Location = new System.Drawing.Point(130, 11);
            this.scenarioNameTextBox.Name = "scenarioNameTextBox";
            this.scenarioNameTextBox.Size = new System.Drawing.Size(604, 27);
            this.scenarioNameTextBox.TabIndex = 6;
            this.scenarioNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.scenarioNameTextBox_Validating);
            // 
            // splitterPanel
            // 
            this.splitterPanel.BackColor = System.Drawing.Color.Gainsboro;
            this.splitterPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterPanel.Location = new System.Drawing.Point(0, 85);
            this.splitterPanel.Name = "splitterPanel";
            this.splitterPanel.Size = new System.Drawing.Size(774, 2);
            this.splitterPanel.TabIndex = 7;
            // 
            // ImportCompositeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.importMainPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ImportCompositeControl";
            this.Size = new System.Drawing.Size(774, 427);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label scenarioLabel;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Panel importMainPanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel splitterPanel;
        private System.Windows.Forms.TextBox scenarioNameTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
    }
}
