using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Helper class for the OXP-based DSS Enhanced Workflow app.
    /// </summary>
    internal sealed class DssEnhancedWorkflowApp : DeviceWorkflowLogSource, IDssWorkflowJobOptions
    {
        private readonly OxpdBrowserEngine _engine;

        public DssEnhancedWorkflowApp(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel);

        }

        public void SelectWorkflow(string workflowName)
        {
            _engine.ExecuteJavaScript(DssWorkflowResource.DssWorkflowJavaScript);

            // Select the first menu (arbitrarily)
            string firstMenu = "document.getElementById('rootAppListBox').getElementsByTagName('li')[0]";
            BoundingBox boundingBox = _engine.GetElementBoundingArea(firstMenu);
            _engine.PressElementByBoundingArea(boundingBox);

            // Select the workflow
            try
            {
                DssWorkflowsReady(workflowName);

                _engine.ExecuteJavaScript(DssWorkflowResource.DssWorkflowJavaScript);
                PressElementByText("rootAppListBox", "li", workflowName);
            }
            catch (JavaScriptExecutionException)
            {
                throw new DeviceWorkflowException($"Unable to select workflow named '{workflowName}'.");
            }

            // Wait for the prompts to load
            _engine.WaitForHtmlContains("promptListBox", TimeSpan.FromSeconds(30));
        }

        private void DssWorkflowsReady(string workflowName)
        {
            int idx = 0;
            bool serverReady = false;

            while (!serverReady && idx < 3)
            {
                idx++;
                serverReady = _engine.WaitForHtmlContains(workflowName, TimeSpan.FromSeconds(6));

            }
            if (!serverReady)
            {
                throw new DeviceCommunicationException("Unable to display the DSS Workflow options within 18 seconds.");
            }
        }

        public void SelectPrompt(string prompt)
        {
            _engine.ExecuteJavaScript(DssWorkflowResource.DssWorkflowJavaScript);

            try
            {
                PressElementByText("promptListBox", "span", prompt);
                _engine.PressElementById(prompt);
            }
            catch (JavaScriptExecutionException)
            {
                throw new DeviceWorkflowException($"Unable to select prompt named '{prompt}'.");
            }
        }

        public void SetJobBuildState(bool enable)
        {
            _engine.ExecuteJavaScript(DssWorkflowResource.DssWorkflowJavaScript);
            string desiredState = enable ? "Job Build" : "Off";

            //If no OCR Prompts are configured on the DSS Server then Done button will not be available.Hence this check.
            if (_engine.HtmlContains("promptList"))
            {
                // Close out the prompt view to get to the options menu
                _engine.PressElementById("workflowFormCloseButton");
            }

            PressElementByText("mainFooter", "div", "Options");
            PressElementByText("moreoptionsListItemsBox", "span", desiredState);
            PressElementByText("mainFooter", "div", "Hide Options");
        }

        public bool ExecuteJob()
        {
            _engine.PressElementById("startBtn");

            return true;
        }

        public bool ExecuteJob(JediWindjammerControlPanel controlPanel)
        {
            _engine.PressElementById("startBtn");

            string script = "document.getElementById('statusPopDown').getElementsByTagName('label')[0].innerHTML";
            string status = controlPanel.ExecuteJavaScript(script);


            while (!status.Contains("Scanning"))
            {
                Thread.Sleep(100);
                status = controlPanel.ExecuteJavaScript(script);
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            while (status.Contains("Scanning"))
            {
                Thread.Sleep(100);
                status = controlPanel.ExecuteJavaScript(script);
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
            while (status.Contains("Sending"))
            {
                Thread.Sleep(100);
                status = controlPanel.ExecuteJavaScript(script);
            }
            RecordEvent(DeviceWorkflowMarker.SendingJobEnd);

            return true;
        }

        private void PressElementByText(string parentId, string tag, string text)
        {
            string elementWithText = $"getElementByText('{parentId}','{tag}','{text}')";
            BoundingBox boundingBox = _engine.GetElementBoundingArea(elementWithText);

            _engine.PressElementByBoundingArea(boundingBox);
        }
    }
}
