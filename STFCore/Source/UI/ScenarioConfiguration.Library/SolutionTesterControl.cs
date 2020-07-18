using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring a Solution Tester virtual resource.
    /// </summary>
    public class SolutionTesterControl : OfficeWorkerControl
    {
        private SolutionTester _tester = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionTesterControl"/> class.
        /// </summary>
        public SolutionTesterControl()
            : base()
        {
        }

        /// <summary>
        /// Gets the resource title.
        /// </summary>
        public override string EditFormTitle
        {
            get { return "Solution Tester Configuration"; }
        }

        /// <summary>
        /// Gets the type of the worker.
        /// </summary>
        /// <value>
        /// The type of the worker.
        /// </value>
        protected override VirtualResourceType WorkerType
        {
            get { return VirtualResourceType.SolutionTester; }
        }

        public override EntityObject EntityObject
        {
            get { return _tester; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Initialize(new SolutionTester());
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="entity"></param>
        public override void Initialize(object entity)
        {
            _tester = entity as SolutionTester;

            base.Initialize(entity);

            if (_tester.ResourcesPerVM != int.MaxValue)
            {
                // Force resources per VM to the max value to avoid
                // splitting manifests when running the test.
                _tester.ResourcesPerVM = int.MaxValue;
            }

            usernameTextBox.DataBindings.Add("Text", _tester, "Username");
            passwordTextBox.DataBindings.Add("Text", _tester, "Password");
            domainTextBox.DataBindings.Add("Text", _tester, "Domain");

            if (_tester.UseCredential)
            {
                solutionTesterUseDesktopRadioButton.Checked = false;
                solutionTesterUseSelectedRadioButton.Checked = true;
            }
            else
            {
                solutionTesterUseDesktopRadioButton.Checked = true;
                solutionTesterUseSelectedRadioButton.Checked = false;
            }

            switch (_tester.AccountType)
            {
                case SolutionTesterCredentialType.AccountPool:
                    poolCredentialRadioButton.Checked = true;
                    break;
                case SolutionTesterCredentialType.DefaultDesktop:
                    desktopCredentialRadioButton.Checked = true;
                    solutionTesterUseDesktopRadioButton.Checked = true;
                    solutionTesterUseDesktopRadioButton.Enabled = false;
                    solutionTesterUseSelectedRadioButton.Enabled = false;
                    break;
                default:
                    manualCredentialRadioButton.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Sets up any child controls.
        /// </summary>
        /// <param name="officeWorker">The office worker.</param>
        protected override void SetupChildControls(OfficeWorker officeWorker)
        {
            var yOffset = 120;

            var location = workflow_GroupBox.Location;
            workflow_GroupBox.Location = new Point(location.X, location.Y - yOffset);

            location = workerQuantity_GroupBox.Location;
            workerQuantity_GroupBox.Location = new Point(location.X, location.Y - yOffset);

            location = startup_TimeDelayControl.Location;
            startup_TimeDelayControl.Location = new Point(location.X, location.Y - yOffset);

            virtualMachinePlatform_ComboBox.Visible = false;
            platform_Label.Visible = false;

            instanceCount_Label.Text = "Total Testers";
            workerQuantity_GroupBox.Text = "Solution Testers Count";

            workersPerVM_Label.Visible = workersPerVM_UpDown.Visible = false;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                bool enablePools = context.DomainAccountPools.Count() > 0;
                poolCredentialRadioButton.Enabled = enablePools;
                userPool_ComboBox.Enabled = enablePools;
                userPool_Label.Enabled = enablePools;
            }

            configuration_TabControl.TabPages.Remove(userGroups_TabPage);

            Worker.Platform = "LOCAL";

            userPool_ComboBox.Location = new Point(127, 132);
            userPool_ComboBox.Visible = true;
            solutionTesterActivityAccountsGroupBox.Controls.Add(userPool_ComboBox);
            userPool_Label.Location = new Point(40, 135);
            userPool_Label.Visible = true;
            solutionTesterActivityAccountsGroupBox.Controls.Add(userPool_Label);

            manualCredentialRadioButton.CheckedChanged += manualCredentialRadioButton_CheckedChanged;
            desktopCredentialRadioButton.CheckedChanged += desktopCredentialRadioButton_CheckedChanged;
            poolCredentialRadioButton.CheckedChanged += poolCredentialRadioButton_CheckedChanged;
            solutionTesterUseDesktopRadioButton.CheckedChanged += solutionTesterUseDesktopRadioButton_CheckedChanged;
            solutionTesterUseSelectedRadioButton.CheckedChanged += solutionTesterUseSelectedRadioButton_CheckedChanged;
        }

        protected override void LoadUserPools()
        {
            userPool_ComboBox.DisplayMember = "Description";
            DomainAccountPool selectedPool = null;

            List<DomainAccountPool> userPools = null;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                userPools = context.DomainAccountPools.OrderBy(x => x.Description).ToList();
            }

            if (userPools.Count > 0)
            {
                bool testerHasUserPool = !string.IsNullOrEmpty(_tester.UserPool);
                if (testerHasUserPool)
                {
                    //If tester has a User Pool setting, select it if it's in the list
                    if (userPools.Any(x => x.DomainAccountKey.Equals(_tester.UserPool)))
                    {
                        selectedPool = userPools.First(x => x.DomainAccountKey.Equals(_tester.UserPool));
                    }
                }
                else
                {
                    selectedPool = userPools.First();
                }
            }

            userPool_ComboBox.DataSource = userPools;
            userPool_ComboBox.SelectedItem = selectedPool;
        }

        protected override void InitializePlatforms()
        {
            // Intentionally left blank to avoid failure in configuration platforms
            // as the the Solution Test Bench does not use them.
        }

        protected override void ReadPlatform()
        {
            // Change the platform to LOCAL since the job will run on the local machine
            Worker.Platform = "LOCAL";
        }

        protected override void instanceCount_TextBox_Validating(object sender, CancelEventArgs e)
        {
            base.instanceCount_TextBox_Validating(sender, e);
            if (e.Cancel == false)
            {
                bool inBounds = false;
                int instanceCountVal = 0;
                string errorMessage = string.Empty;
                if (int.TryParse(instanceCount_TextBox.Text, out instanceCountVal))
                {
                    inBounds = (instanceCountVal <= MaxResourcesPerVM);
                    if (!inBounds)
                    {
                        errorMessage = $"Solution Tester Count must not exceed {MaxResourcesPerVM} for the entire scenario.";
                    }
                }

                fieldValidator.SetError(instanceCount_TextBox, errorMessage);
                e.Cancel = !inBounds;
            }
        }

        private void ValidateUserPool()
        {
            List<DomainAccountPool> userPools = (List<DomainAccountPool>)userPool_ComboBox.DataSource;
            DomainAccountPool selectedPool = null;

            if (userPools.Any(x => x.DomainAccountKey.Equals(_tester.UserPool, StringComparison.OrdinalIgnoreCase)))
            {
                selectedPool = userPools.First(x => x.DomainAccountKey.Equals(_tester.UserPool, StringComparison.OrdinalIgnoreCase));
                userPool_ComboBox.SelectedItem = selectedPool;
            }
            else if (userPools.Count > 0) //No user pools match the tester's pool, but there are pools available.  Assign the first one.
            {
                selectedPool = userPools.First();
                userPool_ComboBox.SelectedItem = selectedPool;
            }
            // We're not worried about the case of no pools in the database because the User Pools radio button
            // is not enabled unless there is at least one pool in the system.
        }

            private void manualCredentialRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ToggleManualControls(true);
            TogglePoolControls(false);
            _tester.AccountType = SolutionTesterCredentialType.ManuallyEntered;

            solutionTesterUseDesktopRadioButton.Enabled = true;
            solutionTesterUseSelectedRadioButton.Enabled = true;
        }

        private void desktopCredentialRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ToggleManualControls(false);
            TogglePoolControls(false);
            _tester.AccountType = SolutionTesterCredentialType.DefaultDesktop;

            solutionTesterUseDesktopRadioButton.Checked = true;
            solutionTesterUseDesktopRadioButton.Enabled = false;
            solutionTesterUseSelectedRadioButton.Enabled = false;
        }

        private void poolCredentialRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ToggleManualControls(false);
            TogglePoolControls(true);
            _tester.AccountType = SolutionTesterCredentialType.AccountPool;
            ValidateUserPool();

            solutionTesterUseDesktopRadioButton.Enabled = true;
            solutionTesterUseSelectedRadioButton.Enabled = true;
        }

        private void TogglePoolControls(bool enabled)
        {
            userPool_ComboBox.Enabled = enabled;
            userPool_Label.Enabled = enabled;
        }

        private void ToggleManualControls(bool enabled)
        {
            usernameLabel.Enabled = usernameTextBox.Enabled = enabled;
            revealPictureBox.Enabled = enabled;
            revealPictureBox.BackColor = enabled ? Color.White : Color.Transparent;
            domainLabel.Enabled = domainTextBox.Enabled = enabled;
            passwordLabel.Enabled = passwordTextBox.Enabled = enabled;
        }

        private void solutionTesterUseDesktopRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _tester.UseCredential = false;
        }

        private void solutionTesterUseSelectedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _tester.UseCredential = true;
        }
    }
}
