using System;
using System.Threading;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage
{
    /// <summary>
    /// Omni version for JetAdvantage application
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage.JetAdvantageAppBase" />
    public class JediOmniJetAdvantageApp : JetAdvantageAppBase
    {
        private readonly JediOmniDevice _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniJetAdvantageApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniJetAdvantageApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _device = device;
        }

        /// <summary>
        /// Does the Initial operations/Device management required for Launching the Jet Advantage App From Home Screen
        /// </summary>
        public override void Launch()
        {
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HP JetAdvantage");
            _device.ControlPanel.ScrollPressWait("#hpid-hpJetAdvantage-homescreen-button", ".hp-oxpd-app-screen");
        }

        /// <summary>
        /// Handles the PullPrinting of Documents
        /// </summary>
        private void PullPrintDocuments(bool printAll, bool delete)
        {
            SelectDocuments(printAll);

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            OnActivityStatusUpdate("Pressing the print button.");
            PressButton("print-settings-button");

            Thread.Sleep(2000);

            PressButton("print-button");
            OnActivityStatusUpdate("Printing the selected document.");
            CheckForPrintingDialog();
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);
            if (delete)
            {
                DeleteDocuments();
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            // wait while in the process of printing the selected document
            FinishPullPrint(DateTime.Now, 4);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
        }

        private void SelectDocuments(bool printAll)
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            if (printAll)
            {
                SelectAllDocuments();
            }
            else
            {
                SelectFirstDocument();
            }
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        /// <summary>
        /// Abstract function for HP Jet Advantage pull printing Operation
        /// </summary>
        /// <param name="printAll">if set to <c>true</c> [print all].</param>
        /// <param name="deleteDocuments">if set to <c>true</c> [delete].</param>
        public override void RunHPJetAdvantage(bool printAll, bool deleteDocuments)
        {
            RecordEvent(DeviceWorkflowMarker.AppShown);
            // press the pull print button
            OnActivityStatusUpdate("Preparing for Atlas Pull Print.");
            PressButton("hp-pullprint");
            DocumentPrinted = true;
            Engine.WaitForHtmlContains("select-all-checkbox", TimeSpan.FromSeconds(25));
            string htmlString = Engine.GetBrowserHtml();
            if (HasFilesToPrint(htmlString))
            {
                PullPrintDocuments(printAll, deleteDocuments);
            }
            else
            {
                OnActivityStatusUpdate("No documents available, logging out.");
                DocumentPrinted = false;
            }
        }

        /// <summary>
        /// Click on Select-all Checkbox to selct all documents in the Pull Printing list
        /// </summary>
        private void SelectAllDocuments()
        {
            PressButton("queue-header", "select-all-checkbox-area");
        }

        /// <summary>
        /// selects the first document in the list for Pull Printing
        /// </summary>
        private void SelectFirstDocument()
        {
            PressButton("queue", "queue-item-details");
        }

        /// <summary>
        /// Uses the jet pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        protected override void LoginViaPin(string jetPin)
        {
            _device.ControlPanel.TypeOnVirtualKeyboard(jetPin);
            _device.ControlPanel.Press("#hpid-keyboard-key-done");

            PressButton("dialog normal show", "dialog-confirm-button button-keypad primary done");
        }

        /// <summary>
        /// Creates the jet pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        protected override void TypePin(string jetPin)
        {
            _device.ControlPanel.Type(jetPin);
        }

        /// <summary>
        /// Does the text entry using Control Panel Keyboard
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="textToType"></param>
        protected override void PlayKeyboard(string objectId, string textToType)
        {
            PressElement(objectId);
            _device.ControlPanel.TypeOnVirtualKeyboard(textToType);

            _device.ControlPanel.Press("#hpid-keyboard-key-done");
        }

        /// <summary>
        /// Press Button on the Jet Advantage Browser based on Button ID
        /// </summary>
        /// <param name="buttonId"></param>
        protected override void PressButton(string buttonId)
        {
            PressElement(buttonId);
            Thread.Sleep(200);
        }

        /// <summary>
        ///  Press Button on the Jet Advantage Browser based on Class Name while ID is not available for the button
        /// </summary>
        /// <param name="className1"></param>
        /// <param name="className2"></param>
        protected override void PressButton(string className1, string className2)
        {
            PressElement(className1, className2);
            Thread.Sleep(200);
        }

        /// <summary>
        /// Select the User in the Jet Advantage Screen based on the Email
        /// </summary>
        /// <param name="objectId"></param>
        protected override void SelectUser(string objectId)
        {
            PressElement(objectId);
            Thread.Sleep(2000);
        }
    }
}
