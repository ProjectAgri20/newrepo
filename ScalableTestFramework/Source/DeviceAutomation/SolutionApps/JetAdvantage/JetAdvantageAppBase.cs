using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage
{
    /// <summary>
    /// JetAdvantage application base
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowLogSource" />
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage.IJetAdvantageApp" />
    public abstract class JetAdvantageAppBase : DeviceWorkflowLogSource, IJetAdvantageApp
    {
        /// <summary>
        /// Gets the JavaScript engine.
        /// </summary>
        /// <value>
        /// The engine.
        /// </value>
        protected OxpdBrowserEngine Engine { get; }

        /// <summary>
        /// Occurs when [activity status change].
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChange;

        /// <summary>
        /// check whether documents are available for print
        /// </summary>
        public bool DocumentPrinted { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageAppBase"/> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        protected JetAdvantageAppBase(IJavaScriptExecutor controlPanel)
        {
            Engine = new OxpdBrowserEngine(controlPanel);
        }

        /// <summary>
        /// Abstract function for HP Jet Advantage Launch
        /// </summary>
        public abstract void Launch();

        /// <summary>
        /// Abstract function for HP Jet Advantage SignIn
        /// </summary>
        /// <param name="jetLoginId">The jet login identifier.</param>
        /// <param name="jetPassword">The jet password.</param>
        /// <param name="useJetPin">if set to <c>true</c> [use jet pin].</param>
        /// <param name="jetPin">The jet pin.</param>
        public void SignIn(string jetLoginId, string jetPassword, bool useJetPin, string jetPin)
        {
            // if the email text box is available than able to access the server
            if (Engine.WaitForHtmlContains("email-textbox", TimeSpan.FromSeconds(15)))
            {
                OnActivityStatusUpdate("Selecting User.");
                SelectUser("different-user");

                // set the login ID and password
                OnActivityStatusUpdate("Filling out HP JetAdvantage login and password data.");
                EnterLoginPassword(jetLoginId, jetPassword);

                if (useJetPin)
                {
                    RecordInfo(DeviceWorkflowMarker.AuthType, "JetAdvantage Pin");
                    //click create pin
                    CreateJetPin(jetPin);
                }
                else
                {
                    OnActivityStatusUpdate("Pressing the HP JetAdvantage login Button");
                    RecordInfo(DeviceWorkflowMarker.AuthType, "JetAdvantage");
                    PressButton("login-button");
                }
                Engine.WaitForHtmlContains("hp-pullprint", TimeSpan.FromSeconds(15));

                if (useJetPin)
                {
                    //logout
                    UseJetPin(jetLoginId, jetPin);
                }
            }
            else
            {
                throw new DeviceWorkflowException("Unable to sign in to HP JetAdvantage.");
            }
        }

        /// <summary>
        /// Creates the jet pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        private void CreateJetPin(string jetPin)
        {
            //click create pin
            OnActivityStatusUpdate("Selecting Create PIN.");
            PressButton("create-pin-checkbox");

            //Login to JetAdvantage
            OnActivityStatusUpdate("Pressing the HP JetAdvantage login Button");
            PressButton("login-button");

            Engine.WaitForHtmlContains("keypad", TimeSpan.FromSeconds(30));
            TypePin(jetPin);
            PressButton("dialog normal show", "dialog-confirm-button button-keypad primary done");
        }

        /// <summary>
        /// Uses the JetAdvantage pin for login purposes.
        /// </summary>
        /// <param name="jetLoginId">The jet login identifier.</param>
        /// <param name="jetPin">The jet pin.</param>
        private void UseJetPin(string jetLoginId, string jetPin)
        {
            //logout
            OnActivityStatusUpdate("Pressing the Log Out button.");
            PressButton("id-button-logout");

            //different user
            Engine.WaitForHtmlContains("email-textbox", TimeSpan.FromSeconds(60));
            OnActivityStatusUpdate("Selecting previously logged in user.");
            string userName = jetLoginId.Replace("@", "_");
            userName = userName.Replace(".", "_");
            SelectUser(userName);

            //EnterLogin with PIN created again
            OnActivityStatusUpdate("Enter Login PIN.");
            PressButton("pin-textbox");
            Engine.WaitForHtmlContains("keypad", TimeSpan.FromSeconds(60));

            LoginViaPin(jetPin);

            Engine.WaitForHtmlContains("hp-pullprint", TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Abstract function for HP Jet Advantage pull printing Operation
        /// </summary>
        /// <param name="printAll">if set to <c>true</c> [print all].</param>
        /// <param name="deleteDocuments">if set to <c>true</c> [delete].</param>
        public abstract void RunHPJetAdvantage(bool printAll, bool deleteDocuments);

        /// <summary>
        /// Abstract function for HP Jet Advantage Sign out
        /// </summary>
        public void Logout()
        {
            OnActivityStatusUpdate("Pressing the Back button.");
            PressButton("back-button");

            OnActivityStatusUpdate("Pressing the Log Out button.");
            PressButton("id-button-logout");

            Engine.WaitForHtmlContains("email-textbox", TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// Checks for printing dialog popup. Will continue waiting until the popup goes away.
        /// </summary>
        protected void CheckForPrintingDialog()
        {
            bool found = Engine.WaitForHtmlContains("progress-dialog-template-inst9", TimeSpan.FromSeconds(10));
            while (found)
            {
                found = Engine.WaitForHtmlContains("progress-dialog-template-inst9", TimeSpan.FromSeconds(10));
            }
        }

        /// <summary>
        /// Handles Delete Document operation for JetAdvantage
        /// </summary>
        protected void DeleteDocuments()
        {
            OnActivityStatusUpdate("Pressing the delete button.");
            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            PressButton("delete-button");
            Engine.WaitForHtmlContains("delete these documents?", TimeSpan.FromSeconds(15));

            PressButton("dialog info show", "dialog-confirm-button primary");

            string spinner = "id=\"modal-spinner\" class=\"hidden\"";
            Engine.WaitForHtmlContains(spinner, TimeSpan.FromSeconds(120));

            RecordEvent(DeviceWorkflowMarker.DeleteEnd);

            //string html = Engine.GetBrowserHtml();
        }

        /// <summary>
        /// Enters the login ID and password. Uses the child's PlayKeyBoard method.
        /// </summary>
        /// <param name="jetLoginId">The jet login identifier.</param>
        /// <param name="jetPassword">The jet password.</param>
        private void EnterLoginPassword(string jetLoginId, string jetPassword)
        {
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
            PlayKeyboard("email-textbox", jetLoginId);
            Thread.Sleep(200);

            PlayKeyboard("password-textbox", jetPassword);
            Thread.Sleep(200);
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
        }

        /// <summary>
        /// Pauses the HP JetAdvantage process while waiting up to the specified number of minutes for the device to finish printing
        /// </summary>
        /// <param name="startDt">DateTime</param>
        /// <param name="minutesToWait">int</param>
        protected void FinishPullPrint(DateTime startDt, int minutesToWait)
        {
            string htmlString = Engine.GetBrowserHtml();
            int idx = htmlString.IndexOf("progress-dialog-template-inst8", StringComparison.Ordinal);
            if (idx <= 0) { idx = 1; }
            int end = htmlString.IndexOf("</dialog>", idx, StringComparison.Ordinal);
            if (end > idx)
            {
                string line = htmlString.Substring(idx, end - idx);
                if (line.Contains("Printing"))
                {
                    TimeSpan ts = DateTime.Now.Subtract(startDt);
                    if (ts.TotalMinutes < minutesToWait)
                    {
                        FinishPullPrint(startDt, minutesToWait);
                    }
                }
            }
        }

        /// <summary>
        /// Logins the via pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        protected abstract void LoginViaPin(string jetPin);

        /// <summary>
        /// Plays the key board. Virtual method, must be over ridden in child class. 
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="textToType">Type of the text to.</param>
        protected abstract void PlayKeyboard(string objectId, string textToType);

        /// <summary>
        /// Presses the button.
        /// </summary>
        /// <param name="buttonId">The button identifier.</param>
        protected abstract void PressButton(string buttonId);

        /// <summary>
        /// Presses the button.
        /// </summary>
        /// <param name="className1">The class name1.</param>
        /// <param name="className2">The class name2.</param>
        protected abstract void PressButton(string className1, string className2);

        /// <summary>
        /// Selects the user.
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        protected abstract void SelectUser(string objectId);

        /// <summary>
        /// Types the pin.
        /// </summary>
        /// <param name="jetPin">The jet pin.</param>
        protected abstract void TypePin(string jetPin);

        /// <summary>
        /// Returns true if the given string contains files to print.
        /// </summary>
        /// <param name="htmlString">string</param>
        /// <returns>bool</returns>
        protected static bool HasFilesToPrint(string htmlString) => htmlString.ToLower().Contains("queue-item-details");

        /// <summary>
        /// Presses the given control with nested class names
        /// </summary>
        /// <param name="className1">string</param>
        /// <param name="className2"></param>
        protected void PressElement(string className1, string className2)
        {
            string script = $"document.getElementsByClassName('{className1}')[0].getElementsByClassName('{className2}')[0]";

            BoundingBox boundingBox = Engine.GetElementBoundingArea(script);
            if (boundingBox.Left == 0 && boundingBox.Right == 0)
            {
                boundingBox = Engine.GetElementBoundingArea(script);
            }
            Engine.PressElementByBoundingArea(boundingBox);
        }

        /// <summary>
        /// Presses the given control.
        /// </summary>
        /// <param name="objectId">string</param>
        /// <returns>Coordinate</returns>
        protected void PressElement(string objectId)
        {
            BoundingBox boundingBox = Engine.GetBoundingAreaById(objectId);
            if (boundingBox.Left == 0 && boundingBox.Right == 0)
            {
                boundingBox = Engine.GetBoundingAreaById(objectId);
            }
            Engine.PressElementByBoundingArea(boundingBox);
        }

        /// <summary>
        /// Event to letting know that a status may be updated
        /// </summary>
        /// <param name="status"></param>
        protected void OnActivityStatusUpdate(string status)
        {
            ActivityStatusChange?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
