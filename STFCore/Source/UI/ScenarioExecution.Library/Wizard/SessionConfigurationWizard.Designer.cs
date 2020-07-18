using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    partial class SessionConfigurationWizard
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

                // Dispose of all wizard pages as well
                foreach (WizardPage page in main_RadWizard.Pages)
                {
                    page.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionConfigurationWizard));
            this.main_RadWizard = new Telerik.WinControls.UI.RadWizard();
            this.summary_WizardPage = new Telerik.WinControls.UI.WizardCompletionPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.scenarioSelection_WizardPage = new Telerik.WinControls.UI.WizardWelcomePage();
            this.assetReservation_WizardPage = new Telerik.WinControls.UI.WizardPage();
            this.deviceSetup_WizardPage = new Telerik.WinControls.UI.WizardPage();
            ((System.ComponentModel.ISupportInitialize)(this.main_RadWizard)).BeginInit();
            this.main_RadWizard.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_RadWizard
            // 
            this.main_RadWizard.CompletionPage = this.summary_WizardPage;
            this.main_RadWizard.Controls.Add(this.panel1);
            this.main_RadWizard.Controls.Add(this.panel2);
            this.main_RadWizard.Controls.Add(this.panel5);
            this.main_RadWizard.Controls.Add(this.panel3);
            this.main_RadWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_RadWizard.Location = new System.Drawing.Point(0, 0);
            this.main_RadWizard.Name = "main_RadWizard";
            this.main_RadWizard.PageHeaderIcon = ((System.Drawing.Image)(resources.GetObject("main_RadWizard.PageHeaderIcon")));
            this.main_RadWizard.Pages.Add(this.scenarioSelection_WizardPage);
            this.main_RadWizard.Pages.Add(this.assetReservation_WizardPage);
            this.main_RadWizard.Pages.Add(this.deviceSetup_WizardPage);
            this.main_RadWizard.Pages.Add(this.summary_WizardPage);
            // 
            // 
            // 
            this.main_RadWizard.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 600, 400);
            this.main_RadWizard.Size = new System.Drawing.Size(940, 590);
            this.main_RadWizard.TabIndex = 0;
            this.main_RadWizard.Text = "radWizard";
            this.main_RadWizard.WelcomePage = this.scenarioSelection_WizardPage;
            this.main_RadWizard.Next += new Telerik.WinControls.UI.WizardCancelEventHandler(this.main_Wizard_Next);
            this.main_RadWizard.Previous += new Telerik.WinControls.UI.WizardCancelEventHandler(this.main_Wizard_Previous);
            this.main_RadWizard.Finish += new System.EventHandler(this.main_Wizard_Finish);
            this.main_RadWizard.Cancel += new System.EventHandler(this.main_Wizard_Cancel);
            this.main_RadWizard.SelectedPageChanging += new Telerik.WinControls.UI.SelectedPageChangingEventHandler(this.main_Wizard_SelectedPageChanging);
            this.main_RadWizard.SelectedPageChanged += new Telerik.WinControls.UI.SelectedPageChangedEventHandler(this.main_Wizard_SelectedPageChanged);
            // 
            // summary_WizardPage
            // 
            this.summary_WizardPage.CompletionImage = ((System.Drawing.Image)(resources.GetObject("summary_WizardPage.CompletionImage")));
            this.summary_WizardPage.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.summary_WizardPage.ContentArea = this.panel5;
            this.summary_WizardPage.Header = "Page header";
            this.summary_WizardPage.Name = "summary_WizardPage";
            this.summary_WizardPage.Title = "Session Initialization";
            this.summary_WizardPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(177, 63);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(763, 479);
            this.panel5.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(177, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 479);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(940, 449);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(0, 93);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(940, 449);
            this.panel3.TabIndex = 3;
            // 
            // scenarioSelection_WizardPage
            // 
            this.scenarioSelection_WizardPage.ContentArea = this.panel1;
            this.scenarioSelection_WizardPage.Header = "Page header";
            this.scenarioSelection_WizardPage.Name = "scenarioSelection_WizardPage";
            this.scenarioSelection_WizardPage.Title = "Scenario and Machine Selection";
            this.scenarioSelection_WizardPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.scenarioSelection_WizardPage.WelcomeImage = ((System.Drawing.Image)(resources.GetObject("scenarioSelection_WizardPage.WelcomeImage")));
            this.scenarioSelection_WizardPage.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            // 
            // assetReservation_WizardPage
            // 
            this.assetReservation_WizardPage.ContentArea = this.panel2;
            this.assetReservation_WizardPage.Header = "View availability or apply a reservation key for the assets requested for use in " +
    "this scenario.";
            this.assetReservation_WizardPage.Name = "assetReservation_WizardPage";
            this.assetReservation_WizardPage.Title = "Asset Reservation";
            this.assetReservation_WizardPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // deviceSetup_WizardPage
            // 
            this.deviceSetup_WizardPage.ContentArea = this.panel3;
            this.deviceSetup_WizardPage.Header = "Select options for devices used in this scenario.";
            this.deviceSetup_WizardPage.Name = "deviceSetup_WizardPage";
            this.deviceSetup_WizardPage.Title = "Device Setup";
            this.deviceSetup_WizardPage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // SessionConfigurationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 590);
            this.Controls.Add(this.main_RadWizard);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SessionConfigurationWizard";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Session Configuration Wizard";
            this.Shown += new System.EventHandler(this.SessionConfigurationWizard_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.main_RadWizard)).EndInit();
            this.main_RadWizard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadWizard main_RadWizard;
        private Telerik.WinControls.UI.WizardCompletionPage summary_WizardPage;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Telerik.WinControls.UI.WizardWelcomePage scenarioSelection_WizardPage;
        private Telerik.WinControls.UI.WizardPage assetReservation_WizardPage;
        private System.Windows.Forms.Panel panel3;
        private Telerik.WinControls.UI.WizardPage deviceSetup_WizardPage;
    }
}