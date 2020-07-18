using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.UI;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    /// <summary>
    /// Start page for selecting a batch of scenarios to run in order.  Contains fewer settings than the single Scenario startup page.
    /// </summary>
    public partial class WizardScenarioBatchPage : UserControl, IWizardPage
    {
        private ErrorProvider _errorProvider = null;
        private BindingList<ScenarioSelectionItem> _scenarios = null;
        private BindingSource _bindingSource = null;
        private bool _initial = true;

        private SessionTicket Ticket { get; set; }

        /// <summary>
        /// Notification to cancel the wizard.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// 
        /// </summary>
        public WizardScenarioBatchPage()
        {
            InitializeComponent();
            InitializeErrorProvider();
            scenarios_DataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="WizardConfiguration"/>.</param>
        public bool Initialize(WizardConfiguration configuration)
        {
            Ticket = configuration.Ticket;
            _scenarios = new BindingList<ScenarioSelectionItem>();
            scenarios_DataGridView.DataSource = null;

            if (STFDispatcherManager.Dispatcher == null && STFDispatcherManager.ConnectToDispatcher() == false)
            {
                //The user canceled the connect dialog
                return false;
            }

            using (new BusyCursor())
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    foreach (EnterpriseScenario scenario in EnterpriseScenario.Select(context, Ticket.ScenarioIds))
                    {
                        _scenarios.Add(new ScenarioSelectionItem(scenario));
                    }

                    LoadComboBoxes(context);
                }

                _bindingSource = new BindingSource(_scenarios, string.Empty);
                scenarios_DataGridView.DataSource = _bindingSource;
            }

            //Set data from Ticket
            if (Ticket.SessionId != null)
            {
                SetEstimatedRunTime();
                sessionName_ComboBox.Text = string.IsNullOrEmpty(Ticket.SessionName) ? "Multiple Scenarios" : Ticket.SessionName;
            }

            if (!_initial)
            {
                SessionClient.Instance.Close(Ticket.SessionId);

                sessionType_ComboBox.SelectedText = Ticket.SessionType;

                if (!string.IsNullOrEmpty(Ticket.EmailAddresses))
                {
                    runtime_NumericUpDown.Value = Math.Min(Ticket.DurationHours, runtime_NumericUpDown.Maximum);
                }
            }

            environment_Label.Text = "{0} {1} Environment".FormatWith(GlobalSettings.Items[Setting.Organization], GlobalSettings.Items[Setting.Environment]);
            dispatcher_Label.Text = STFDispatcherManager.Dispatcher.HostName;
            _initial = false;

            return true;
        }

        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>
        /// True if this page was successfully validated.
        /// </returns>
        public bool Complete()
        {
            if (!ValidateInput())
            {
                return false;
            }

            // We're gonna need a data context several times in the following lines
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                List<EnterpriseScenario> scenarios = EnterpriseScenario.Select(context, Ticket.ScenarioIds).ToList();

                // Perform a data integrity check on the scenarios
                if (PerformDataIntegrityCheck(scenarios) == false)
                {
                    return false;
                }

                //PopulateNotificationSettings() TODO: What about the email list?  The ticket has an email list.  Can we repurpose it for batch operations?

                // Populate ticket data from the UI
                Ticket.ScenarioIds = GetSelectedScenarioIds();
                Ticket.CollectEventLogs = false;
                Ticket.SessionName = sessionName_ComboBox.Text;
                Ticket.SessionType = sessionType_ComboBox.Text;
                Ticket.SessionCycle = sessionCycle_ComboBox.Text;
                Ticket.Reference = WizardPageManager.GetReferenceData(reference_TextBox);
                Ticket.SessionNotes = notes_TextBox.Text;
                Ticket.DurationHours = (int)runtime_NumericUpDown.Value;
                SessionLogRetention logRetention = EnumUtil.GetByDescription<SessionLogRetention>((string)retention_ComboBox.SelectedItem);
                Ticket.ExpirationDate = logRetention.GetExpirationDate(DateTime.Now);
                Ticket.LogLocation = GlobalSettings.WcfHosts[WcfService.DataLog];

                SetAssociatedProducts(context, scenarios);

                // Doesn't make sense to save session name for batch operation. 
                context.SaveChanges();
            }

            // Initiate the session with the dispatcher
            TraceFactory.Logger.Debug($"Calling Initiate() on {Ticket.SessionId}");
            SessionClient.Instance.InitiateSession(Ticket);

            return true;
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // STF-only
            if (GlobalSettings.IsDistributedSystem)
            {
                if (dispatcher_Label != null)
                {
                    _errorProvider.SetIconAlignment(dispatcher_Label, ErrorIconAlignment.MiddleLeft);
                }
            }

            _errorProvider.SetIconAlignment(scenarios_DataGridView, ErrorIconAlignment.TopRight);
            _errorProvider.SetIconAlignment(sessionName_ComboBox, ErrorIconAlignment.MiddleRight);
        }

        private void LoadComboBoxes(EnterpriseTestContext context)
        {
            sessionName_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionName.ToString());
            sessionName_ComboBox.SelectedIndex = -1;

            sessionType_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionType.ToString());
            sessionType_ComboBox.SelectedIndex = -1;

            sessionCycle_ComboBox.DataSource = ResourceWindowsCategory.Select(context, ResourceWindowsCategoryType.SessionCycle.ToString());
            sessionCycle_ComboBox.SelectedIndex = sessionCycle_ComboBox.FindString(Ticket.SessionCycle);

            retention_ComboBox.DataSource = SessionLogRetentionHelper.ExpirationList;
            retention_ComboBox.SelectedIndex = retention_ComboBox.FindString(EnumUtil.GetDescription(WizardPageManager.GetDefaultLogRetention()));

        }

        private List<Guid> GetSelectedScenarioIds()
        {
            List<Guid> result = new List<Guid>();

            foreach (ScenarioSelectionItem item in _scenarios)
            {
                result.Add(item.ScenarioId);
            }
            return result;
        }

        private void SetEstimatedRunTime()
        {
            int total = 0;
            foreach (ScenarioSelectionItem item in _scenarios)
            {
                total += item.EstimatedRunTime;
            }

            runtime_NumericUpDown.Value = Math.Min(total, runtime_NumericUpDown.Maximum);
        }

        /// <summary>
        /// Sets an error in the ErrorProvider if the error condition is true;
        /// </summary>
        /// <param name="control">The control to set the error.</param>
        /// <param name="setError">true if the error text is to be set.  False to remove any error text.</param>
        /// <param name="errorText">The error text.</param>
        /// <returns>true if an error was set, false otherwise.</returns>
        private bool SetError(Control control, bool setError, string errorText)
        {
            _errorProvider.SetError(control, setError ? errorText : string.Empty);
            return setError;
        }

        private void up_Button_Click(object sender, EventArgs e)
        {
            int position = _bindingSource.Position;
            if (_bindingSource.Count > 1 && position > 0)
            {
                _bindingSource.RaiseListChangedEvents = false;

                object selected = _bindingSource.Current;
                _bindingSource.Remove(selected);
                position--;
                _bindingSource.Insert(position, selected);
                _bindingSource.Position = position;

                _bindingSource.RaiseListChangedEvents = true;
                _bindingSource.ResetBindings(false);                
            }
        }

        private void down_Button_Click(object sender, EventArgs e)
        {
            int position = _bindingSource.Position;
            if (_bindingSource.Count > 1 && position < _bindingSource.Count - 1)
            {
                _bindingSource.RaiseListChangedEvents = false;

                object selected = _bindingSource.Current;
                _bindingSource.Remove(selected);
                position++;
                _bindingSource.Insert(position, selected);
                _bindingSource.Position = position;

                _bindingSource.RaiseListChangedEvents = true;
                _bindingSource.ResetBindings(false);
            }
        }

        private void add_Button_Click(object sender, EventArgs e)
        {
            using (ScenarioSelectionForm selectionForm = new ScenarioSelectionForm())
            {
                if (selectionForm.ShowDialog() == DialogResult.OK)
                {
                    EnterpriseScenario scenario = null;
                    using (new BusyCursor())
                    {
                        using (EnterpriseTestContext context = new EnterpriseTestContext())
                        {
                            scenario = EnterpriseScenario.Select(context, selectionForm.SelectedScenarioId);
                            _bindingSource.Add(new ScenarioSelectionItem(scenario));
                        }
                    }
                    
                    _bindingSource.MoveLast();
                    SetEstimatedRunTime();
                }
            }
        }

        private void remove_Button_Click(object sender, EventArgs e)
        {
            _bindingSource.Remove(_bindingSource.Current);
            _bindingSource.ResetBindings(false); //Needed to correctly refresh the grid if the last item was removed.
            SetEstimatedRunTime();
        }

        /// <summary>
        /// Validates user input.
        /// </summary>
        /// <returns>true if input is valid, false otherwise.</returns>
        private bool ValidateInput()
        {
            bool result = true;

            // STF-only
            if (GlobalSettings.IsDistributedSystem)
            {
                result &= (!SetError(dispatcher_Label, STFDispatcherManager.Dispatcher == null, "Must be connected to a dispatcher."));
            }

            result &= (!SetError(scenarios_DataGridView, _bindingSource.Count < 1, "Please select at least one scenario to run."));
            result &= (!SetError(sessionName_ComboBox, string.IsNullOrEmpty(sessionName_ComboBox.Text), "Please select or enter a Session Name."));

            return result;
        }

        private bool PerformDataIntegrityCheck(List<EnterpriseScenario> scenarios)
        {
            bool result = true;

            foreach (EnterpriseScenario scenario in scenarios)
            {
                result = result && WizardPageManager.PerformScenarioIntegrityCheck(scenario);
            }

            return result;
        }

        private void SetAssociatedProducts(EnterpriseTestContext context, IEnumerable<EnterpriseScenario> scenarios)
        {
            // We have to track what has already been added to the Ticket.  List of Guids seems the most efficient way,
            // given the fact that the list is potentially growing with each loop.  Array.Contains is faster than List.Contains, 
            // but List<> is better suited to the changing collection size.
            List<Guid> added = new List<Guid>();
            Ticket.AssociatedProductList.Clear();

            try
            {
                IEnumerable<ScenarioProduct> scenarioProducts = null;
                foreach (EnterpriseScenario scenario in scenarios)
                {
                    scenarioProducts = WizardPageManager.GetAssociatedProducts(context, scenario);
                    foreach (ScenarioProduct assocProduct in scenarioProducts)
                    {
                        if (! added.Contains(assocProduct.ProductId))
                        {
                            var item = new AssociatedProductSerializable()
                            {
                                AssociatedProductId = assocProduct.ProductId,
                                Vendor = assocProduct.Vendor,
                                Name = assocProduct.Name,
                                Version = assocProduct.Version,
                                Active = assocProduct.Active
                            };
                            Ticket.AssociatedProductList.Add(item);
                            added.Add(assocProduct.ProductId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error loading Associated Products.", ex);
            }
        }
    }

    internal class ScenarioSelectionItem
    {
        public Guid ScenarioId { get; }

        public string Name { get; }

        public int EstimatedRunTime { get; }

        public ScenarioSelectionItem(EnterpriseScenario scenario)
        {
            ScenarioId = scenario.EnterpriseScenarioId;
            Name = scenario.Name;
            EstimatedRunTime = scenario.EstimatedRuntime;
            // ScenarioSettings override the default
            if (!string.IsNullOrEmpty(scenario.ScenarioSettings))
            {
                ScenarioSettings settings = LegacySerializer.DeserializeDataContract<ScenarioSettings>(scenario.ScenarioSettings);
                EstimatedRunTime = settings.EstimatedRunTime;
            }
        }

    }
}
