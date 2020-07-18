using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.SessionExecution.Wizard;

namespace HP.SolutionTest.WorkBench
{
    /// <summary>
    /// Wizard for setting session configuration.
    /// </summary>
    internal partial class SessionConfigurationWizard : Form
    {
        private bool _movingNext = false;
        private WizardConfiguration _configuration;

        /// <summary>
        /// Gets the <see cref="SessionTicket"/> containing the wizard configuration.
        /// </summary>
        public SessionTicket Ticket
        {
            get { return _configuration.Ticket; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionConfigurationWizard"/> class.
        /// </summary>
        public SessionConfigurationWizard(List<Guid> scenarioIds)
        {
            InitializeComponent();

            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            main_RadWizard.HelpButton.Visibility = ElementVisibility.Hidden;
            main_RadWizard.FinishButton.Text = "Start Session";

            // Add controls to the wizard pages
            if (scenarioIds.Count > 1)
            {
                AddPage<WizardScenarioBatchPage>(scenarioSelection_WizardPage);
            }
            else
            {
                AddPage<WizardScenarioSelectionPage>(scenarioSelection_WizardPage);
            }
            AddPage<WizardAssetReservationPage>(assetReservation_WizardPage);
            AddPage<WizardDeviceSetupPage>(deviceSetup_WizardPage);
            AddPage<WizardSessionInitializationPage>(summary_WizardPage);

            // Initialize the configuration
            _configuration = new WizardConfiguration();
            _configuration.Ticket.ScenarioIds = scenarioIds;
        }

        private void AddPage<T>(WizardPage wizardPage) where T : UserControl, IWizardPage, new()
        {
            UserControl wizardControl = new T();
            wizardPage.ContentArea.Controls.Add(wizardControl);
            wizardControl.Dock = DockStyle.Fill;

            ((IWizardPage)wizardControl).Cancel += main_Wizard_Cancel;

            // Special case: let the initialization page enable/disable the Next button
            var initializationPage = wizardControl as WizardSessionInitializationPage;
            if (initializationPage != null)
            {
                initializationPage.SetFinishButton(main_RadWizard.FinishButton);
            }
        }

        private void SessionConfigurationWizard_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            if (! GetWizardPage(main_RadWizard.Pages[0]).Initialize(_configuration))
            {
                // Initialize returned false.  User canceled the connect dialog.  Do not continue with the wizard.
                main_Wizard_Cancel(sender, EventArgs.Empty);
            }
        }

        private void main_Wizard_SelectedPageChanging(object sender, SelectedPageChangingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Validate the current page (if moving forward) and make sure we can proceed
                if (_movingNext && !GetWizardPage(e.SelectedPage).Complete())
                {
                    e.Cancel = true;
                    return;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void main_Wizard_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // Determine whether we should display the next page
                if (!GetWizardPage(main_RadWizard.SelectedPage).Initialize(_configuration))
                {
                    int index = main_RadWizard.Pages.IndexOf(main_RadWizard.SelectedPage);
                    main_RadWizard.SelectedPage = main_RadWizard.Pages[index + 1];

                    // Changing the page will fire the event again, which will
                    // call this method again - don't do anything else here
                    return;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            // If this is the summary page, don't let the user move backward
            main_RadWizard.BackButton.Visibility = (main_RadWizard.SelectedPage == summary_WizardPage) ?
                                            ElementVisibility.Hidden :
                                            ElementVisibility.Visible;
        }

        private void main_Wizard_Finish(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                GetWizardPage(main_RadWizard.SelectedPage).Complete();
                this.DialogResult = DialogResult.OK;

                SessionClient.Instance.PowerUp(Ticket.SessionId);
            }
            finally
            {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void main_Wizard_Next(object sender, WizardCancelEventArgs e)
        {
            _movingNext = true;
        }

        private void main_Wizard_Previous(object sender, WizardCancelEventArgs e)
        {
            _movingNext = false;
        }

        private void main_Wizard_Cancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private static IWizardPage GetWizardPage(WizardPage page)
        {
            return page.ContentArea.Controls[0] as IWizardPage;
        }

    }
}
