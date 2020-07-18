namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    partial class ScenarioImportWizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenarioImportWizardForm));
            this.importWizard = new Telerik.WinControls.UI.RadWizard();
            this.completionWizardPage = new Telerik.WinControls.UI.WizardCompletionPage();
            this.completionPanel = new System.Windows.Forms.Panel();
            this.welcomePanel = new System.Windows.Forms.Panel();
            this.resolutionPanel = new System.Windows.Forms.Panel();
            this.groupPanel = new System.Windows.Forms.Panel();
            this.documentPanel = new System.Windows.Forms.Panel();
            this.platformPanel = new System.Windows.Forms.Panel();
            this.assetPoolPanel = new System.Windows.Forms.Panel();
            this.welcomeWizardPage = new Telerik.WinControls.UI.WizardWelcomePage();
            this.assetPoolWizardPage = new Telerik.WinControls.UI.WizardPage();
            this.resolutionWizardPage = new Telerik.WinControls.UI.WizardPage();
            this.documentWizardPage = new Telerik.WinControls.UI.WizardPage();
            this.platformWizardPage = new Telerik.WinControls.UI.WizardPage();
            this.groupWizardPage = new Telerik.WinControls.UI.WizardPage();
            this.wizardImages = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.importWizard)).BeginInit();
            this.importWizard.SuspendLayout();
            this.SuspendLayout();
            // 
            // importWizard
            // 
            this.importWizard.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.importWizard.CompletionImage = ((System.Drawing.Image)(resources.GetObject("importWizard.CompletionImage")));
            this.importWizard.CompletionPage = this.completionWizardPage;
            this.importWizard.Controls.Add(this.welcomePanel);
            this.importWizard.Controls.Add(this.completionPanel);
            this.importWizard.Controls.Add(this.resolutionPanel);
            this.importWizard.Controls.Add(this.groupPanel);
            this.importWizard.Controls.Add(this.documentPanel);
            this.importWizard.Controls.Add(this.platformPanel);
            this.importWizard.Controls.Add(this.assetPoolPanel);
            this.importWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importWizard.Location = new System.Drawing.Point(0, 0);
            this.importWizard.Name = "importWizard";
            this.importWizard.PageHeaderIcon = ((System.Drawing.Image)(resources.GetObject("importWizard.PageHeaderIcon")));
            this.importWizard.Pages.Add(this.welcomeWizardPage);
            this.importWizard.Pages.Add(this.assetPoolWizardPage);
            this.importWizard.Pages.Add(this.resolutionWizardPage);
            this.importWizard.Pages.Add(this.documentWizardPage);
            this.importWizard.Pages.Add(this.platformWizardPage);
            this.importWizard.Pages.Add(this.groupWizardPage);
            this.importWizard.Pages.Add(this.completionWizardPage);
            // 
            // 
            // 
            this.importWizard.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 750, 500);
            this.importWizard.Size = new System.Drawing.Size(850, 530);
            this.importWizard.TabIndex = 0;
            this.importWizard.Text = "Scenario Import Wizard";
            this.importWizard.WelcomeImage = ((System.Drawing.Image)(resources.GetObject("importWizard.WelcomeImage")));
            this.importWizard.WelcomePage = this.welcomeWizardPage;
            // 
            // completionWizardPage
            // 
            this.completionWizardPage.ContentArea = this.completionPanel;
            this.completionWizardPage.Header = "Page header";
            this.completionWizardPage.Name = "completionWizardPage";
            this.completionWizardPage.Title = "Select Location and Import Scenario";
            // 
            // completionPanel
            // 
            this.completionPanel.BackColor = System.Drawing.Color.White;
            this.completionPanel.Location = new System.Drawing.Point(172, 63);
            this.completionPanel.Name = "completionPanel";
            this.completionPanel.Size = new System.Drawing.Size(678, 419);
            this.completionPanel.TabIndex = 2;
            // 
            // welcomePanel
            // 
            this.welcomePanel.BackColor = System.Drawing.Color.White;
            this.welcomePanel.Location = new System.Drawing.Point(172, 63);
            this.welcomePanel.Name = "welcomePanel";
            this.welcomePanel.Size = new System.Drawing.Size(678, 419);
            this.welcomePanel.TabIndex = 0;
            // 
            // resolutionPanel
            // 
            this.resolutionPanel.BackColor = System.Drawing.Color.White;
            this.resolutionPanel.Location = new System.Drawing.Point(0, 93);
            this.resolutionPanel.Name = "resolutionPanel";
            this.resolutionPanel.Size = new System.Drawing.Size(850, 389);
            this.resolutionPanel.TabIndex = 1;
            // 
            // groupPanel
            // 
            this.groupPanel.BackColor = System.Drawing.Color.White;
            this.groupPanel.Location = new System.Drawing.Point(0, 93);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(850, 389);
            this.groupPanel.TabIndex = 5;
            // 
            // documentPanel
            // 
            this.documentPanel.BackColor = System.Drawing.Color.White;
            this.documentPanel.Location = new System.Drawing.Point(0, 93);
            this.documentPanel.Name = "documentPanel";
            this.documentPanel.Size = new System.Drawing.Size(850, 389);
            this.documentPanel.TabIndex = 3;
            // 
            // platformPanel
            // 
            this.platformPanel.BackColor = System.Drawing.Color.White;
            this.platformPanel.Location = new System.Drawing.Point(0, 93);
            this.platformPanel.Name = "platformPanel";
            this.platformPanel.Size = new System.Drawing.Size(850, 389);
            this.platformPanel.TabIndex = 4;
            // 
            // assetPoolPanel
            // 
            this.assetPoolPanel.BackColor = System.Drawing.Color.White;
            this.assetPoolPanel.Location = new System.Drawing.Point(0, 93);
            this.assetPoolPanel.Name = "assetPoolPanel";
            this.assetPoolPanel.Size = new System.Drawing.Size(850, 389);
            this.assetPoolPanel.TabIndex = 6;
            // 
            // welcomeWizardPage
            // 
            this.welcomeWizardPage.ContentArea = this.welcomePanel;
            this.welcomeWizardPage.Header = "Page header";
            this.welcomeWizardPage.Name = "welcomeWizardPage";
            this.welcomeWizardPage.Title = "Scenario Import Wizard";
            // 
            // assetPoolWizardPage
            // 
            this.assetPoolWizardPage.ContentArea = this.assetPoolPanel;
            this.assetPoolWizardPage.Header = " Select the appropriate device Asset Pool for each imported device listed below";
            this.assetPoolWizardPage.Icon = null;
            this.assetPoolWizardPage.Name = "assetPoolWizardPage";
            this.assetPoolWizardPage.Title = "Select Asset Pool";
            // 
            // resolutionWizardPage
            // 
            this.resolutionWizardPage.ContentArea = this.resolutionPanel;
            this.resolutionWizardPage.Header = " Resolve each item in the list below by selecting an alternate resource from the " +
    "system targeted for import";
            this.resolutionWizardPage.HeaderVisibility = Telerik.WinControls.ElementVisibility.Visible;
            this.resolutionWizardPage.Icon = ((System.Drawing.Image)(resources.GetObject("resolutionWizardPage.Icon")));
            this.resolutionWizardPage.Image = null;
            this.resolutionWizardPage.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.resolutionWizardPage.Name = "resolutionWizardPage";
            this.resolutionWizardPage.Text = "";
            this.resolutionWizardPage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.resolutionWizardPage.TextWrap = false;
            this.resolutionWizardPage.Title = "Resolve Missing External Resource References";
            // 
            // documentWizardPage
            // 
            this.documentWizardPage.ContentArea = this.documentPanel;
            this.documentWizardPage.Header = " Resolve each Test Document in the list below by selecting an alternate document " +
    "from the target system";
            this.documentWizardPage.Name = "documentWizardPage";
            this.documentWizardPage.Title = "Resolve Missing Test Document References";
            // 
            // platformWizardPage
            // 
            this.platformWizardPage.ContentArea = this.platformPanel;
            this.platformWizardPage.Header = " Select the appropriate machine platform for each entry";
            this.platformWizardPage.Name = "platformWizardPage";
            this.platformWizardPage.Title = "Resolve Machine Platform Usage";
            // 
            // groupWizardPage
            // 
            this.groupWizardPage.ContentArea = this.groupPanel;
            this.groupWizardPage.Header = "  Select all groups that will have authorization to edit this imported scenario";
            this.groupWizardPage.Name = "groupWizardPage";
            this.groupWizardPage.Title = "Select Authorized Test Scenario Groups";
            // 
            // wizardImages
            // 
            this.wizardImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.wizardImages.ImageSize = new System.Drawing.Size(16, 16);
            this.wizardImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ScenarioImportWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 530);
            this.Controls.Add(this.importWizard);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScenarioImportWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scenario Import";
            ((System.ComponentModel.ISupportInitialize)(this.importWizard)).EndInit();
            this.importWizard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadWizard importWizard;
        private Telerik.WinControls.UI.WizardCompletionPage completionWizardPage;
        private System.Windows.Forms.Panel completionPanel;
        private System.Windows.Forms.Panel welcomePanel;
        private System.Windows.Forms.Panel resolutionPanel;
        private Telerik.WinControls.UI.WizardWelcomePage welcomeWizardPage;
        private Telerik.WinControls.UI.WizardPage resolutionWizardPage;
        private System.Windows.Forms.ImageList wizardImages;
        private System.Windows.Forms.Panel documentPanel;
        private Telerik.WinControls.UI.WizardPage documentWizardPage;
        private System.Windows.Forms.Panel platformPanel;
        private System.Windows.Forms.Panel groupPanel;
        private Telerik.WinControls.UI.WizardPage platformWizardPage;
        private Telerik.WinControls.UI.WizardPage groupWizardPage;
        private System.Windows.Forms.Panel assetPoolPanel;
        private Telerik.WinControls.UI.WizardPage assetPoolWizardPage;
    }
}