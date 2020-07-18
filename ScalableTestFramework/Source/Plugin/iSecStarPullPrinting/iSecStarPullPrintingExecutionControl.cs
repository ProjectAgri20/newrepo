using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using TopCat.TestApi.GUIAutomation;
using TopCat.TestApi.GUIAutomation.Enums;
using HP.ScalableTest.Plugin.iSecStarPullPrinting.UIMaps;
using HP.ScalableTest.Plugin.iSecStarPullPrinting.UIMap;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    [ToolboxItem(false)]
    public partial class iSecStarPullPrintingExecutionControl : UserControl, IPluginExecutionEngine
    {
        DocumentCollectionIterator _documentCollectionIterator = null;

        private static List<iSecStarPullPrintAction> _validateTargets = new List<iSecStarPullPrintAction>()
            { iSecStarPullPrintAction.Reprint, iSecStarPullPrintAction.Print, iSecStarPullPrintAction.Delete };

        private StringBuilder _logText = new StringBuilder();
        private iSecStarActivityData _activityData = null;
        private iSecStarPullPrintManager _pullPrintManager = null;
        private static PrintLogin _printLogin;
        private const int WindowsTimeout = 20;
        private const int ShortTimeout = 5;
        private static readonly TimeSpan humanTimeSpan = TimeSpan.FromSeconds(2);

        public iSecStarPullPrintingExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            iSecStarActivityData data = executionData.GetMetadata<iSecStarActivityData>();
            iSecStarPullPrintManager manager = new iSecStarPullPrintManager(executionData, data);

            if (_documentCollectionIterator == null)
            {
                CollectionSelectorMode mode = data.ShuffleDocuments ? CollectionSelectorMode.ShuffledRoundRobin : CollectionSelectorMode.RoundRobin;
                _documentCollectionIterator = new DocumentCollectionIterator(mode);
            }

            manager.StatusUpdate += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            manager.DocumentActionSelected += UpdateDocumentAction;
            manager.SessionIdUpdate += UpdateSessionId;
            manager.TimeStatusUpdate += PullPrintManager_TimeStatusUpdate;

            if (executionData.PrintQueues.Any() && executionData.Documents.Any())
            {
                try
                {
                    CloseOrphanedClientPopup();

                    manager.ExecutePrintJob(_documentCollectionIterator, data.UsePrintServerNotification, data.DelayAfterPrint);

                    if (CheckForErrorPrompt())
                    {
                        Thread.Sleep(humanTimeSpan);
                        PerformPrintTask(executionData);
                    }
                }
                catch (PrintQueueNotAvailableException ex)
                {
                    //This exception has already been logged in the call to manager.ExecutePrintJob
                    return new PluginExecutionResult(PluginResult.Failed, ex, "Print Failure.");
                }
            }
            return manager.ExecutePullPrintOperation();
        }

        /// <summary>
        /// Updates the device displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        private void UpdateDevice(object sender, StatusChangedEventArgs e)
        {
            activeDevice_Label.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }

        /// <summary>
        /// Updates the document process.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        private void UpdateDocumentAction(object sender, StatusChangedEventArgs e)
        {
            labelDocumentProcessAction.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }

        private void UpdateSessionId(object sender, StatusChangedEventArgs e)
        {
            label_sessionId.InvokeIfRequired(n => n.Text = e.StatusMessage);
        }

        private void PullPrintManager_TimeStatusUpdate(object sender, TimeStatusEventArgs e)
        {
            // Update the Time Taken when the Step has been completed
            TimeSpan duration = e.EndDateTime.Subtract(e.StartDateTime);
            UpdateStatus($"...Time Taken for Completing the  Step {e.StatusMessage} : {duration}");
        }
        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(string status)
        {
            string statusLine = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")} {status}";
            status_RichTextBox.InvokeIfRequired(n => n.AppendText(statusLine + Environment.NewLine));
        }

        /// <summary>
        ///  Performs the print dialog task
        /// </summary>
        /// <param name="executionData"></param>
        private void PerformPrintTask(PluginExecutionData executionData)
        {
            string username = executionData.Credential.UserName;
            string password = executionData.Credential.Password;
            TopCatUIAutomation.Initialize();
            _printLogin = new PrintLogin(UIAFramework.ManagedUIA);
            if (_printLogin.PrintLogin32770Window.IsAvailable())
            {
                _printLogin.Edit1000Edit.ClickWithMouse(MouseButton.Left, WindowsTimeout);
                Thread.Sleep(humanTimeSpan);
                SendKeys.SendWait(username);
                _printLogin.Edit1001Edit.ClickWithMouse(MouseButton.Left, WindowsTimeout);
                Thread.Sleep(humanTimeSpan);
                SendKeys.SendWait(password);
                _printLogin.SubmitButton1Button.WaitForAvailable(WindowsTimeout);
                _printLogin.SubmitButton1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowsTimeout));
            }
            else
            {
                UpdateStatus("Could not launch ISecStar Client Popup");
            }
        }

        /// <summary>
        /// This method is created to handle any Error prompts thrown by the ISecStar Client . 
        /// It is observed that even on entering the correct credentials the iSecStar clients throws error messages which could be timing or TopCat issues
        /// </summary>
        /// <returns></returns>
        private bool CheckForErrorPrompt()
        {
            _printLogin = new PrintLogin(UIAFramework.ManagedUIA);
            if (_printLogin.Message32770Window.IsAvailable())
            {
                _printLogin.OKButton2Button.ClickWithMouse(MouseButton.Left, WindowsTimeout);
                Thread.Sleep(humanTimeSpan);
                _printLogin.Edit1000Edit.PerformHumanAction(x => x.EnterText("", WindowsTimeout));
                Thread.Sleep(humanTimeSpan);
                _printLogin.Edit1001Edit.PerformHumanAction(x => x.EnterText("", WindowsTimeout));
                Thread.Sleep(humanTimeSpan);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is to handle any orphaned iSecStar client pop-up from the previous failures 
        /// This will close the orphaned prompts from previous iteration runs if any.
        /// </summary>
        private void CloseOrphanedClientPopup()
        {
            TopCatUIAutomation.Initialize();
            _printLogin = new PrintLogin(UIAFramework.ManagedUIA);
            if (_printLogin.PrintLogin32770Window.IsAvailable())
            {
                _printLogin.CloseButton2Button.ClickWithMouse(MouseButton.Left, WindowsTimeout);
                Thread.Sleep(humanTimeSpan);
            }
        }
    }
}
