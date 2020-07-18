using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Implementation of <see cref="IDssWorkflowApp" /> for an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerDssWorkflowApp : DeviceWorkflowLogSource, IDssWorkflowApp
    {
        private const int _workflowMain = 54;
        private const int _workflowSelection = 673;
        private const int _workflowKeyboard = 32;
        private const int _homeScreen = 567;
        private const int _signInScreen = 18;

        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;
        private readonly OzWindjammerDssWorkflowJobOptions _optionsManager;
        private readonly OzWindjammerJobExecutionManager _executionManager;
        private Pacekeeper _pacekeeper;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper
        {
            get
            {
                return _pacekeeper;
            }
            set
            {
                _pacekeeper = value;
                _optionsManager.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerDssWorkflowApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerDssWorkflowApp(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new OzWindjammerDssWorkflowJobOptions(device);
            _executionManager = new OzWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Workflow application on the device.
        /// </summary>
        public void Launch()
        {
            Widget appButton = _controlPanel.ScrollToItem("Title", "Workflow");
            if (appButton != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press(appButton);
                bool success = _controlPanel.WaitForScreen(_workflowSelection, TimeSpan.FromSeconds(30));
                if (!success)
                {
                    int activeScreen = _controlPanel.ActiveScreenId();
                    if (activeScreen == _signInScreen)
                    {
                        throw new DeviceWorkflowException("Sign-in required to launch the Workflow application.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Could not launch Workflow application. Active screen: {activeScreen}");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            else
            {
                if (_controlPanel.ActiveScreenId() == _homeScreen)
                {
                    throw new DeviceWorkflowException("Workflow application button was not found on device home screen.");
                }
                else
                {
                    throw new DeviceWorkflowException("Cannot launch the Workflow application: Not at device home screen.");
                }
            }
        }

        /// <summary>
        /// Selects the workflow with the specified name from the default workflow menu.
        /// </summary>
        /// <param name="workflowName">The workflow name.</param>
        public void SelectWorkflow(string workflowName)
        {
            Widget groupWidget = _controlPanel.GetWidgets().Find("1.", StringMatch.StartsWith);
            _controlPanel.Press(groupWidget);

            // Wait for the workflows to load, then let it settle for a moment
            Wait.ForNotNull(() => _controlPanel.GetWidgets().Find("Workflow >", StringMatch.StartsWith), TimeSpan.FromSeconds(20));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Pacekeeper.Sync();

            // Find the workflow and select it
            WidgetCollection workflowWidgets = _controlPanel.GetWidgets().OfType(WidgetType.HelpListItem);
            string workflowMatch = StringMatcher.FindBestMatch(workflowName, workflowWidgets.Select(n => n.Text), StringMatch.Contains, ignoreCase: true);
            if (workflowMatch != null)
            {
                Widget workflow = _controlPanel.ScrollToItem("StringContent1", workflowMatch);
                _controlPanel.Press(workflow);
            }
            else
            {
                throw new DeviceWorkflowException($"Could not find workflow named '{workflowName}'.");
            }

            // Wait for the parameters to load, then let it settle for a moment
            _controlPanel.WaitForScreen(_workflowMain, TimeSpan.FromSeconds(20));
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Enters the specified value for the specified workflow prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="value">The value.</param>
        public void EnterPromptValue(string prompt, string value)
        {
            WidgetCollection widgets = _controlPanel.GetWidgets();
            IEnumerable<string> prompts = widgets.OfType(WidgetType.Prompt).Select(n => n.Text);

            // Find the text box for the prompt and press it to navigate to the keyboard screen
            string promptMatch = StringMatcher.FindBestMatch(prompt, prompts, StringMatch.Contains, ignoreCase: true, ignoreWhiteSpace: true);
            if (promptMatch != null)
            {
                Widget label = widgets.Find(promptMatch);
                PressPrompt(widgets, label);
                _controlPanel.WaitForScreen(_workflowKeyboard, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();

                // Enter the parameter text, then navigate back to the parameter screen
                _controlPanel.TypeOnVirtualKeyboard(value);
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                _controlPanel.WaitForScreen(_workflowMain, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException($"Could not find workflow prompt '{prompt}'.");
            }
        }

        private void PressPrompt(WidgetCollection widgets, Widget label)
        {
            if (ArePromptLocationsCorrect(widgets))
            {
                Widget promptBox = widgets.FindByLabel(label, WidgetType.Button);
                _controlPanel.Press(promptBox);
            }
            else
            {
                if (AreTextBoxLocationsCorrect(widgets))
                {
                    // For some devices, the workflow parameters screen reports the same bounding box for each label, which
                    // makes it impossible to determine which label corresponds to which text box by comparing their position.
                    // Instead, we are forced to make an assumption based on the way the controls are dynamically created:
                    // each text box corresponds to the prompt with an ID exactly 1 greater than the box's ID.
                    _controlPanel.Press(label.Id - 1);
                }
                else
                {
                    // On the workflow parameters screen, the bounding boxes are the same for all the labels AND text boxes.
                    // We'll need to make some assumptions about the way the controls are dynamically created:
                    // Each text box corresponds to the prompt with an ID exactly 1 greater than the box's ID,
                    // AND the text box IDs are in ascending order as we move down the screen.
                    int textBoxId = label.Id - 1;

                    // Calculate the y offset from the top box's press coordinate.
                    // There are 50 pixels between each edit box and the ID increments by 2 for each box.
                    // The top box always has ID 1500
                    Widget topBox = widgets.Find(1500);
                    var topBoxCenter = new Coordinate(topBox.Location.X + topBox.Size.Width / 2, topBox.Location.Y + topBox.Size.Height / 2);
                    int offset = (textBoxId - topBox.Id) / 2 * 50;
                    _controlPanel.PressScreen(new Coordinate(topBoxCenter.X, topBoxCenter.Y + offset));
                }
            }
        }

        private static bool ArePromptLocationsCorrect(WidgetCollection widgets)
        {
            WidgetCollection prompts = widgets.OfType(WidgetType.Prompt);
            int distinctLocations = prompts.Select(n => n.Location).Distinct().Count();
            return distinctLocations == prompts.Count;
        }

        private static bool AreTextBoxLocationsCorrect(WidgetCollection widgets)
        {
            WidgetCollection textBoxes = widgets.OfType(WidgetType.EditBox);
            int distinctLocations = textBoxes.Select(n => n.Location).Distinct().Count();
            return distinctLocations == textBoxes.Count;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool jobCompleted = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            jobCompleted = _executionManager.ExecuteScanJob(executionOptions);
            return jobCompleted;
        }

        /// <summary>
        /// Signs into Oz device using IAuthenticator object using given AuthenticationMode
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="authenticationMode"></param>
        /// <returns>NotImplementedException</returns>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="IDssWorkflowJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IDssWorkflowJobOptions Options => _optionsManager;

        private class OzWindjammerDssWorkflowJobOptions : OzWindjammerJobOptionsManager, IDssWorkflowJobOptions
        {
            public OzWindjammerDssWorkflowJobOptions(OzWindjammerDevice device)
                : base(device)
            {
            }
        }
    }
}
