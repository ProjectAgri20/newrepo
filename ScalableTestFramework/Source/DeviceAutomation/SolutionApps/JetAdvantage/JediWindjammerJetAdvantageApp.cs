using System;
using System.Threading;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage
{
    /// <summary>
    /// Jet Advantage Controller Class for Windjammer Specific Device Operations
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage.JetAdvantageAppBase" />
    public class JediWindjammerJetAdvantageApp : JetAdvantageAppBase
    {
        private readonly JediWindjammerDevice _device;
        //The latest JetAdvantage Main Form Name is "HPForm". Old form name "OxpUIAppMainForm"
        private const string _oxpMainForm = "HPForm";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerJetAdvantageApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerJetAdvantageApp(JediWindjammerDevice device)
            : base(device.ControlPanel)
        {
            _device = device;
        }

        /// <summary>
        /// Abstract function for HP Jet Advantage Launch
        /// </summary>
        public override void Launch()
        {
            // Press the HP JetAdvantage (Titan) button
            OnActivityStatusUpdate("Press the HP JetAdvantage (Titan) button");
            string scrollControlName = "mAccessPointDisplay";
            _device.ControlPanel.WaitForControl(scrollControlName, TimeSpan.FromSeconds(200));

            RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HP JetAdvantage");
            _device.ControlPanel.ScrollPressNavigate(scrollControlName, "MoreApps", _oxpMainForm, true);
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
            Engine.WaitForHtmlContains("select-all-checkbox", TimeSpan.FromSeconds(30));
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
        /// Click on Select-all Checkbox to select all documents in the Pull Printing list
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
        /// Logins via pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        protected override void LoginViaPin(string jetPin)
        {
            _device.ControlPanel.Type(jetPin);
            PressButton("dialog normal show", "dialog-confirm-button button-keypad primary done");
        }

        /// <summary>
        /// Types the pin.
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
            _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", textToType);
            Thread.Sleep(2000);
            _device.ControlPanel.Press("ok");
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
        /// Handles the PullPrinting of Documents
        /// </summary>
        private void PullPrintDocuments(bool printAll, bool delete)
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

        /// <summary>
        /// Select the user for the previous users list based on Login ID
        /// </summary>
        /// <param name="objectId"></param>
        protected override void SelectUser(string objectId)
        {
            PressElement(objectId);
            Thread.Sleep(2000);
        }
    }
}
