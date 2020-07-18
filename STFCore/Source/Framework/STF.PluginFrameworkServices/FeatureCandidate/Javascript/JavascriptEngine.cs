using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HP.DeviceAutomation.Devices.Jedi;
using Microsoft.Ajax.Utilities;

namespace HP.DeviceAutomation.FeatureCandidate.Javascript
{
        /// <summary>
        /// Engine for executing JavaScript code via OXP.
        /// </summary>
    internal sealed class JavaScriptEngine : IDisposable
    {
        private HP.DeviceAutomation.Devices.Jedi.JediWindjammerDevice _device;
        private Minifier _ajaxMinifier = new Minifier();

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptEngine"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="adminPassword">The admin password.</param>
        public JavaScriptEngine(JediWindjammerDevice device)
        {
            _device = device;
        }

        /// <summary>
        /// Executes the specified JavaScript.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="allowCompression">if set to <c>true</c> allow the script to be compressed before execution.</param>
        /// <returns></returns>
        public string ExecuteJavaScript(string script, bool allowCompression = true)
        {
            // Minify script (if allowed)
            if (allowCompression)
            {
                script = _ajaxMinifier.MinifyJavaScript(script);
            }

            // Wait for an idle browser state, then execute the script
            Logger.Debug("Executing JavaScript: " + script);
            string result = _device.ControlPanel.ExecuteJavaScript(script, TimeSpan.FromSeconds(30));

            // If the JavaScript returned an exception, throw it as a managed exception
            if (result.StartsWith("Exception:", StringComparison.OrdinalIgnoreCase))
            {
                throw new JavaScriptException(result);
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Creates the specified JavaScript function if it does not already exist.
        /// </summary>
        /// <param name="function">The function.</param>
        public void CreateFunction(string function)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            // Extract the function name (minify first to remove comments and formatting)
            function = _ajaxMinifier.MinifyJavaScript(function);
            string functionName = function.Substring(0, function.IndexOf('(')).Replace("function", string.Empty).Trim();

            // Check to see if the function is already defined
            string result = ExecuteJavaScript("typeof({0})".FormatWith(functionName));
            if (result.Equals("\"undefined\"", StringComparison.OrdinalIgnoreCase))
            {
                // Execute the script to create the function (no need to minify again)
                ExecuteJavaScript(function, false);
            }
        }

        /// <summary>
        /// Clicks the HTML element with the specified ID.
        /// </summary>
        /// <param name="id">The id.</param>
        public void ClickElementById(string id)
        {
            string findById = @"document.getElementById('{0}')".FormatWith(id);

            try
            {
                string script = findById + ".click()";
                ExecuteJavaScript(script);
            }
            catch (JavaScriptException)
            {
                // Try clicking the button using a mouse event instead
                CreateFunction(@"function ClickByMouseEvent(element) { var event = document.createEvent('MouseEvents'); event.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null); element.dispatchEvent(event);}");
                string script = "ClickByMouseEvent(" + findById + ")";
                ExecuteJavaScript(script);
            }
        }

        /// <summary>
        /// Retrieves the complete HTML from the browser.
        /// </summary>
        /// <returns></returns>
        public string RetrieveHtml()
        {
            StringBuilder bodyContent = new StringBuilder();
            int bufferSize = 5000;
            int startIndex = 0;

            string response = RetrieveHtmlText(startIndex, bufferSize);
            while (!string.IsNullOrEmpty(response))
            {
                bodyContent.Append(response);
                startIndex += bufferSize;
                response = RetrieveHtmlText(startIndex, bufferSize);
            }
            return Regex.Unescape(bodyContent.ToString());
        }

        private string RetrieveHtmlText(int startIndex, int length)
        {
            int endIndex = startIndex + length;
            string script = @"document.documentElement.innerHTML.substring({0},{1});".FormatWith(startIndex, endIndex);
            string response = ExecuteJavaScript(script);
            return response.Substring(1, response.Length - 2);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }
        }

        #endregion
    }    
}
