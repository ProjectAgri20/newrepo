using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.UI;
using Telerik.WinControls.UI;
using HP.ScalableTest;
using HP.ScalableTest.UI.SessionExecution.Wizard;

namespace HP.SolutionTest.WorkBench
{
    internal partial class WizardSessionInitializationPage : UserControl, IWizardPage
    {
        private string _sessionId;
        private RadButtonElement _finishButton;
        private BindingList<AssetStatusRow> _statusRows;
        private readonly string[] _separator = new string[] { Environment.NewLine };

        /// <summary>
        /// Notification to cancel the wizard.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardSessionInitializationPage"/> class.
        /// </summary>
        public WizardSessionInitializationPage()
        {
            InitializeComponent();
            AssetStatusRow.Icons = availabilityIcons_ImageList;
            UserInterfaceStyler.Configure(assetStatus_GridView, GridViewStyle.ReadOnly);
            assetStatus_GridView.MasterTemplate.SelectLastAddedRow = false;
        }

        /// <summary>
        /// Initializes the wizard page with the specified <see cref="WizardConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public bool Initialize(WizardConfiguration configuration)
        {
            _sessionId = configuration.Ticket.SessionId;
            _statusRows = new BindingList<AssetStatusRow>();
            assetStatus_GridView.DataSource = _statusRows;

            SessionClient.Instance.SessionStateReceived += SessionStateReceived;
            SessionClient.Instance.SessionStartupTransitionReceived += SessionStartupTransitionReceived;
            SessionClient.Instance.SessionMapElementReceived += SessionMapElementReceived;

            if (_finishButton != null)
            {
                _finishButton.Enabled = false;
            }

            TraceFactory.Logger.Debug("Calling Stage");
            SessionClient.Instance.Stage(_sessionId, configuration.SessionAssets);

            return true;
        }

        public void SetFinishButton(RadButtonElement button)
        {
            _finishButton = button;
        }

        private void SessionStateReceived(object sender, SessionStateEventArgs e)
        {
            if (initializationStatus_Label.InvokeRequired)
            {
                initializationStatus_Label.Invoke(new Action(() => SessionStateReceived(sender, e)));
                return;
            }

            // If we have received a session state change, we're not ready to power up
            retry_Button.Visible = false;
            _finishButton.Enabled = false;

            TraceFactory.Logger.Debug("STATE: {0}, MESSAGE: <{1}>".FormatWith(e.State, e.Message));
            // Change message based on 
            switch (e.State)
            {
                case SessionState.Staging:
                    initializationStatus_Label.Text = "Initializing...";
                    break;
                case SessionState.Validating:
                    initializationStatus_Label.Text = "Validating environment configuration...";
                    break;
                case SessionState.Error:
                    initializationStatus_Label.Text = "Setup Error: {0}".FormatWith(e.Message);
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        MessageBox.Show(e.Message, "Session Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;               
            }
        }

        private void SessionStartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            if (initializationStatus_Label.InvokeRequired)
            {
                initializationStatus_Label.Invoke(new Action(() => SessionStartupTransitionReceived(sender, e)));
                return;
            }

            // This special event handler isolates this specific client as the one to handle
            // this next transition.  Without it, every client monitoring the dispatcher
            // would respond to this event and try to validate, which would be a problem.
            switch (e.Transition)
            {
                case SessionStartupTransition.ReadyToValidate:
                    SessionClient.Instance.Validate(_sessionId);
                    break;

                case SessionStartupTransition.ReadyToRevalidate:
                    retry_Button.Visible = true;
                    _finishButton.Enabled = false;
                    initializationStatus_Label.Text = "Validation failed. Fix the specified errors and press Retry to validate again.";
                    break;

                case SessionStartupTransition.ReadyToPowerUp:
                    retry_Button.Visible = true;
                    _finishButton.Enabled = true;
                    initializationStatus_Label.Text = "Validation complete. Press Start Session to begin test execution.";
                    break;
            }

            // When a transition comes in, force the grid view to refresh completely
            // This takes care of any stragglers that haven't refreshed in the UI
            assetStatus_GridView.DataSource = null;
            assetStatus_GridView.DataSource = _statusRows;
        }

        private void SessionMapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            // We only care about updates that pertain to the current session ID
            SessionMapElement element = e.MapElement;
            if (element.SessionId != _sessionId)
            {
                return;
            }

            if (assetStatus_GridView.InvokeRequired)
            {
                assetStatus_GridView.Invoke(new Action(() => SessionMapElementReceived(sender, e)));
                return;
            }

            switch (element.ElementType)
            {
                case ElementType.Machine:
                case ElementType.Device:
                case ElementType.RemotePrintQueue:
                    UpdateRow(element);
                    break;

                default:
                    if (element.State == RuntimeState.Error || element.State == RuntimeState.Warning)
                    {
                        UpdateRow(element);
                    }
                    break;
            }
        }

        private void UpdateRow(SessionMapElement element)
        {
            AssetStatusRow row = _statusRows.FirstOrDefault(n => n.Id == element.Id);
            if (row != null)
            {
                row.Update(element);
            }
            else
            {
                AssetStatusRow newRow = new AssetStatusRow(element);
                newRow.Update(element);
                _statusRows.Add(newRow);
            }
        }

        /// <summary>
        /// Performs final validation before allowing the user to navigate away from the page.
        /// </summary>
        /// <returns>
        /// True if this page was successfully validated.
        /// </returns>
        public bool Complete()
        {
            return true;
        }

        private void retry_Button_Click(object sender, EventArgs e)
        {
            _statusRows.Clear();
            retry_Button.Visible = false;
            _finishButton.Enabled = false;
            SessionClient.Instance.Revalidate(_sessionId);
            initializationStatus_Label.Text = "Validating environment configuration...";
        }

        /// <summary>
        /// Helper class used to display information in the DataGridView.
        /// </summary>
        private class AssetStatusRow
        {
            public static ImageList Icons { get; set; }

            public Guid Id { get; private set; }
            public RuntimeState State { get; private set; }
            public string Name { get; set; }
            public string ElementType { get; set; }
            public string Details { get; set; }
            public Image Icon { get; set; }

            public AssetStatusRow(SessionMapElement element)
            {
                Id = element.Id;
            }

            public void Update(SessionMapElement element)
            {
                Name = element.Name;
                ElementType = element.ElementType.ToString();
                Details = element.Message;
                State = element.State;
                switch (element.State)
                {
                    case RuntimeState.Validated:
                        Icon = Icons.Images["Available"];
                        break;

                    case RuntimeState.Warning:
                        Icon = Icons.Images["Warning"];
                        break;

                    case RuntimeState.Error:
                        Icon = Icons.Images["Unavailable"];
                        break;

                    case RuntimeState.Validating:
                    default:
                        Icon = Icons.Images["Unknown"];
                        break;
                }
            }
        }

        private void assetStatus_GridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.Value != null && e.ColumnIndex == 3)
            {
                int lineCount = e.CellElement.Value.ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries).Length;
                if (lineCount > 1)
                {
                    int height = (lineCount + 1) * 18;
                    assetStatus_GridView.ChildRows[e.RowIndex].Height = height;
                }

                e.CellElement.ToolTipText = e.CellElement.Text;
            } 
        }
    }
}
