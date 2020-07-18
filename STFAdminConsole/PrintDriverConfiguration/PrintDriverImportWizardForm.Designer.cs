namespace HP.ScalableTest.LabConsole
{
    partial class PrintDriverImportWizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintDriverImportWizardForm));
            this.wizardControl = new Telerik.WinControls.UI.RadWizard();
            this.wizardCompletionPage = new Telerik.WinControls.UI.WizardCompletionPage();
            this.completionPanel = new System.Windows.Forms.Panel();
            this.welcomePanel = new System.Windows.Forms.Panel();
            this.wizardWelcomePage = new Telerik.WinControls.UI.WizardWelcomePage();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).BeginInit();
            this.wizardControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl
            // 
            this.wizardControl.CompletionPage = this.wizardCompletionPage;
            this.wizardControl.Controls.Add(this.welcomePanel);
            this.wizardControl.Controls.Add(this.completionPanel);
            this.wizardControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl.Location = new System.Drawing.Point(0, 0);
            this.wizardControl.Margin = new System.Windows.Forms.Padding(4);
            this.wizardControl.Name = "wizardControl";
            this.wizardControl.PageHeaderIcon = ((System.Drawing.Image)(resources.GetObject("wizardControl.PageHeaderIcon")));
            this.wizardControl.Pages.Add(this.wizardWelcomePage);
            this.wizardControl.Pages.Add(this.wizardCompletionPage);
            this.wizardControl.Size = new System.Drawing.Size(899, 574);
            this.wizardControl.TabIndex = 0;
            this.wizardControl.Text = "wizardControl";
            this.wizardControl.WelcomePage = this.wizardWelcomePage;
            // 
            // wizardCompletionPage
            // 
            this.wizardCompletionPage.CompletionImage = ((System.Drawing.Image)(resources.GetObject("wizardCompletionPage.CompletionImage")));
            this.wizardCompletionPage.ContentArea = this.completionPanel;
            this.wizardCompletionPage.Header = "Select Driver";
            this.wizardCompletionPage.Name = "wizardCompletionPage";
            this.wizardCompletionPage.Title = "Select Driver Repository Location";
            // 
            // completionPanel
            // 
            this.completionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.completionPanel.BackColor = System.Drawing.Color.White;
            this.completionPanel.Location = new System.Drawing.Point(172, 63);
            this.completionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.completionPanel.Name = "completionPanel";
            this.completionPanel.Size = new System.Drawing.Size(727, 463);
            this.completionPanel.TabIndex = 2;
            // 
            // welcomePanel
            // 
            this.welcomePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcomePanel.BackColor = System.Drawing.Color.White;
            this.welcomePanel.Location = new System.Drawing.Point(172, 63);
            this.welcomePanel.Margin = new System.Windows.Forms.Padding(4);
            this.welcomePanel.Name = "welcomePanel";
            this.welcomePanel.Size = new System.Drawing.Size(727, 463);
            this.welcomePanel.TabIndex = 0;
            // 
            // wizardWelcomePage
            // 
            this.wizardWelcomePage.ContentArea = this.welcomePanel;
            this.wizardWelcomePage.Header = "Page header";
            this.wizardWelcomePage.Name = "wizardWelcomePage";
            this.wizardWelcomePage.Title = "Select Driver to Import";
            this.wizardWelcomePage.WelcomeImage = ((System.Drawing.Image)(resources.GetObject("wizardWelcomePage.WelcomeImage")));
            // 
            // PrintDriverImportWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 574);
            this.Controls.Add(this.wizardControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrintDriverImportWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Driver Import";
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl)).EndInit();
            this.wizardControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadWizard wizardControl;
        private Telerik.WinControls.UI.WizardCompletionPage wizardCompletionPage;
        private System.Windows.Forms.Panel completionPanel;
        private System.Windows.Forms.Panel welcomePanel;
        private Telerik.WinControls.UI.WizardWelcomePage wizardWelcomePage;
    }
}