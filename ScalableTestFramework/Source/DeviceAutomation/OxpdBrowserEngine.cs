using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Engine for interacting with the OXPd solution browser.
    /// </summary>
    /// <remarks>
    /// This class encapsulates common JavaScript operations used when interacting with OXPd-based solutions.
    /// It also allows the consumer to define functions that are injected into the browser upon first use
    /// and then execute those functions via a simple syntax.
    /// </remarks>
    public sealed class OxpdBrowserEngine
    {
        private readonly JavaScriptEngine _javaScriptEngine;
        private readonly Action<Coordinate> _pressScreen;
        private readonly Lazy<int> _browserOffset;
        private readonly string _backingScript;
        private readonly DeviceScreen _deviceScreen;
        private readonly Lazy<BoundingBox> _oxpdBodyBox;

        // JavaScript class names of objects that use the Oxpd Zoom style
        private string[] _classNames = { "xl hp-textbox", "hp-textbox-input xl", "xl hp-list", "hp-listitem xl", "xl hp-numberbox", "hp-textbox xl" }; 


        /// <summary>
        /// Initializes a new instance of the <see cref="OxpdBrowserEngine"/> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <exception cref="ArgumentNullException"><paramref name="controlPanel" /> is null.</exception>
        public OxpdBrowserEngine(IJavaScriptExecutor controlPanel)
        {            
            if (controlPanel == null)
            {
                throw new ArgumentNullException(nameof(controlPanel));
            }

            _javaScriptEngine = new JavaScriptEngine(controlPanel);
            _deviceScreen = new DeviceScreen(controlPanel);
            _oxpdBodyBox = new Lazy<BoundingBox>(() => ParseBoundingArea(ExecuteJavaScript($"document.querySelector(\'body\').getBoundingClientRect()")));
            // JavaScript bounding boxes return coordinates relative to the browser window.
            // To map those coordinates to the device control panel, we must apply an offset
            // to handle the space taken up by the masthead.
            if (controlPanel is JediOmniControlPanel omni)
            {
                _pressScreen = omni.PressScreen;
                _browserOffset = new Lazy<int>(() => GetBrowserOffset(omni));
            }

            if (controlPanel is JediWindjammerControlPanel windjammer)
            {
                _pressScreen = windjammer.PressScreen;
                _browserOffset = new Lazy<int>(() => GetBrowserOffset(windjammer));
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OxpdBrowserEngine" /> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <param name="backingScript">The backing script that will be used by browser interaction.</param>
        /// <remarks>
        /// The backing script provided to this method will be injected into the browser for use by the
        /// <see cref="ExecuteFunction" /> method.  This script is lazy-loaded, and will automatically
        /// be re-injected if the browser engine ever finds it has been unloaded.
        /// </remarks>
        public OxpdBrowserEngine(IJavaScriptExecutor controlPanel, string backingScript)
            : this(controlPanel)
        {
            _backingScript = backingScript;
        }

        /// <summary>
        /// Executes the specified JavaScript in the OXPd browser.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        /// <returns>The result of the JavaScript execution.</returns>
        /// <remarks>
        /// If the backing script is required the catch will cause the backing script that was provided to the constructor to be loaded.
        /// It will attempt to execute a function defined in the backing script; if the backing script is not loaded
        /// into the browser, it will inject that script and then execute it.
        /// </remarks>
        public string ExecuteJavaScript(string script)
        {
            string result = string.Empty;
            try
            {
                result = _javaScriptEngine.ExecuteJavaScript(script);
            }
            catch (JavaScriptExecutionException) when (!string.IsNullOrWhiteSpace(_backingScript))
            {
                // Backing script has either been unloaded or has not yet been injected.
                // Inject the script and then run again.
                _javaScriptEngine.ExecuteJavaScript(_backingScript);
                result = _javaScriptEngine.ExecuteJavaScript(script);
            }
            return result;
        }

        /// <summary>
        /// Executes the specified JavaScript function in the OXPd browser, defining the functions if necessary.
        /// </summary>
        /// <param name="function">The function to execute.</param>
        /// <param name="parameters">The parameters to pass to the function.</param>
        /// <returns>The result of the JavaScript execution.</returns>
        /// <remarks>
        /// This method depends on the backing script that was provided to the constructor for this instance.
        /// It will attempt to execute a function defined in the backing script; if the backing script is not loaded
        /// into the browser, it will inject that script and then execute it.
        /// </remarks>
        public string ExecuteFunction(string function, params object[] parameters)
        {
            // Build the equivalent JavaScript to call the specified function
            string parameterList = parameters.Any() ? $"'{string.Join("','", parameters)}'" : string.Empty;
            string script = $"{function}({parameterList})";

            return ExecuteJavaScript(script);
        }

        /// <summary>
        /// Retrieves the complete HTML DOM from the OXPd browser.
        /// </summary>
        /// <returns>The HTML content of the browser.</returns>
        /// <remarks>
        /// This method is resource-intensive for the target device browser, and is provided mostly for use during development.
        /// Avoid using this method in production code - try to inject JavaScript that will return just the data needed.
        /// </remarks>
        public string GetBrowserHtml()
        {
            StringBuilder content = new StringBuilder();
            int bufferSize = 5000;
            int startIndex = 0;

            bool done = false;
            while (!done)
            {
                string script = $"document.documentElement.innerHTML.substring({startIndex},{startIndex + bufferSize});";
                string response = ExecuteJavaScript(script);
                string unquotedResponse = response.Substring(1, response.Length - 2);
                content.Append(unquotedResponse);

                startIndex += bufferSize;
                done = unquotedResponse.Length < bufferSize;
            }
            return Regex.Unescape(content.ToString());
        }

        /// <summary>
        /// Determines whether the OXPd browser HTML contains the specified value (case-sensitive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the HTML contains the specified value, <c>false</c> otherwise.</returns>
        public bool HtmlContains(string value)
        {
            string script = $"document.documentElement.innerHTML.indexOf('{value}')";
            return int.Parse(ExecuteJavaScript(script)) > -1;
        }

        /// <summary>
        /// Waits for the OXPd browser HTML to contain the specified value (case-sensitive).
        /// </summary>
        /// <param name="value">The value to wait for.</param>
        /// <param name="waitTime">The amount of time to wait for the value.</param>
        /// <returns><c>true</c> if the HTML contains the value within the specified timespan, <c>false</c> otherwise.</returns>
        public bool WaitForHtmlContains(string value, TimeSpan waitTime)
        {
            return Wait.ForTrue(() => HtmlContains(value), waitTime);
        }
        /// <summary>
        /// Returns true if the given identifier exists within the OXPD browser
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>bool</returns>
        public bool ExistElementId(string elementId)
        {
            bool existId = true;

            string script = "function ExistElementById(){ var buttonElement = document.getElementById('" + elementId + "');var result =  buttonElement == null ?'false':'true'; return result;}ExistElementById();";
            try
            {
                existId = ExecuteJavaScript(script).Replace("\"", string.Empty).Trim().Equals("true") ? true : false;
                if (existId)
                {
                    // sometimes the javaScript ID is embed in the header.
                    BoundingBox bb = GetBoundingAreaById(elementId);
                    if (bb.Left.Equals(0) && bb.Top.Equals(0))
                    {
                        existId = false;
                    }
                }
            }
            catch
            {
                existId = false;
            }

            return existId;
        }

        /// <summary>Waits for upto the given wait time for the passed in element to exist on the Oxpd form.</summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="waitTime">The wait time.</param>
        /// <returns>true if exists</returns>
        public bool WaitToExistElementId(string elementId, TimeSpan waitTime)
        {
            return Wait.ForTrue(() => ExistElementId(elementId), waitTime);
        }

        /// <summary>
        /// Gets the bottom location by identifier.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        public int GetBottomLocationById(string elementId)
        {
            double zoomMultiplier = 1;

            if (_deviceScreen.IsHighDef && elementId.Length > 0)
            {
                zoomMultiplier = GetZoomRequired(elementId);
            }
            double loc = GetBoundingAreaById(elementId).Bottom * zoomMultiplier + _browserOffset.Value;
            return Convert.ToInt32(loc);
        }

        /// <summary>
        /// Gets the bounding area of the element with the specified ID.
        /// </summary>
        /// <param name="elementId">The element ID.</param>
        /// <returns>A <see cref="BoundingBox" /> for the element located by the specified script.</returns>
        /// <exception cref="DeviceWorkflowException">No element with the specified ID was found.</exception>
        public BoundingBox GetBoundingAreaById(string elementId)
        {
            try
            {
                string script = $"document.getElementById('{elementId}')";
                return GetElementBoundingArea(script);
            }
            catch (JavaScriptExecutionException)
            {
                throw new DeviceWorkflowException($"Could not find OXPd element with ID '{elementId}'.");
            }
        }

        /// <summary>
        /// Gets the bounding area of the element with the specified class name and index.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="BoundingBox" /> for the element located by the specified script.</returns>
        /// <exception cref="DeviceWorkflowException">No element with the specified class and index was found.</exception>
        public BoundingBox GetBoundingAreaByClassIndex(string className, int index)
        {
            try
            {
                string script = $"document.getElementsByClassName('{className}')[{index}]";
                return GetElementBoundingArea(script);
            }
            catch (JavaScriptExecutionException)
            {
                throw new DeviceWorkflowException($"Could not find OXPd element with class '{className}' and index {index}.");
            }
        }

        /// <summary>
        /// Gets the bounding area of an OXPd browser element.
        /// </summary>
        /// <param name="elementSelectionScript">A script that selects the specified element.</param>
        /// <returns>A <see cref="BoundingBox" /> for the element located by the specified script.</returns>
        public BoundingBox GetElementBoundingArea(string elementSelectionScript)
        {
            BoundingBox boundingBox = new BoundingBox();
            void getBoundingBox()
            {
                string boundingArea = ExecuteJavaScript($"{elementSelectionScript}.getBoundingClientRect()");
                boundingBox = ParseBoundingArea(boundingArea);
            }
            Retry.WhileThrowing<JavaScriptExecutionException>(getBoundingBox, 5, TimeSpan.FromSeconds(1));
            return boundingBox;
        }

        /// <summary>
        /// Presses the OXPd browser element with the specified ID.
        /// </summary>
        /// <param name="elementId">The element ID.</param>
        /// <exception cref="DeviceWorkflowException">No element with the specified ID was found.</exception>
        public void PressElementById(string elementId)
        {
            BoundingBox initialBoundingArea = GetBoundingAreaById(elementId);
             // does the oxpdbodybox completely contain the element we want to press
             // oxpdBodyBox uses relative positioning (the top is always 0)
            if (!BoundingAreaFoundWithin(_oxpdBodyBox.Value, initialBoundingArea))
            {
                //if the top of footer is > than the bottom of the buttonId
                //controlPanel.GetBoundingBox("#hpid-oxpd-scroll-pane")
                ExecuteJavaScript($"document.getElementById(\"{elementId}\").scrollIntoView()");
            }
            Coordinate pressCoordinate = GetCenterCoordinate(GetBoundingAreaById(elementId), elementId);
            _pressScreen?.Invoke(pressCoordinate);
        }

        private static bool BoundingAreaFoundWithin(BoundingBox outerBox, BoundingBox innerBox)
        {
            return outerBox.Top <= innerBox.Top &&
                outerBox.Bottom >= innerBox.Bottom;
        }

        /// <summary>
        /// Presses the OXPd browser element with the specified class and index.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="DeviceWorkflowException">No element with the specified class and index was found.</exception>
        /// <exception cref="ArgumentException">Coordinates outside the screen area boundary.</exception>
        public void PressElementByClassIndex(string className, int index)
        {
            BoundingBox boundingBox = GetBoundingAreaByClassIndex(className, index);
            PressElementByBoundingArea(boundingBox);
        }

        /// <summary>
        /// Presses the OXPd browser element with the specified bounding area.
        /// </summary>
        /// <param name="boundingBox">The <see cref="BoundingBox" /> of the element to press.</param>
        /// <exception cref="ArgumentException">Coordinates outside the screen area boundary.</exception>
        public void PressElementByBoundingArea(BoundingBox boundingBox)
        {
            Coordinate pressCoordinate = GetCenterCoordinate(boundingBox);
            if (! _deviceScreen.Contains(pressCoordinate))
            {
                throw new ArgumentException($"One or more components of coordinate (X={pressCoordinate.X},Y={pressCoordinate.Y}) is outside the screen area boundary (W={_deviceScreen.Size.Width},H={_deviceScreen.Size.Height}).");
            }

            _pressScreen?.Invoke(pressCoordinate);
        }

        /// <summary>
        /// Presses the OXPd browser element with the specified bounding area utilizing the given zoom multiplier.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="zoomMultiplier">The zoom multiplier.</param>
        public void PressElementByBoundingArea(BoundingBox boundingBox, double zoomMultiplier)
        {
            Coordinate pressCoordinate = GetCenterCoordinate(boundingBox, zoomMultiplier);
            _pressScreen?.Invoke(pressCoordinate);
        }

        private Coordinate GetCenterCoordinate(BoundingBox boundingBox , string elementId = "")
        {
            double zoomMultiplier = 1;

            if (_deviceScreen.IsHighDef && elementId.Length > 0)
            {
                zoomMultiplier = GetZoomRequired(elementId);
            }

            return GetCenterCoordinate(boundingBox, zoomMultiplier); 
        }

        private Coordinate GetCenterCoordinate(BoundingBox boundingBox, double zoomMultiplier)
        {
            double xCenter = (boundingBox.Left + (boundingBox.Width / 2)) * zoomMultiplier;
            double yCenter = (boundingBox.Top + (boundingBox.Height / 2)) * zoomMultiplier;

            int coordX = Convert.ToInt32(xCenter);
            int coordY = Convert.ToInt32(yCenter + _browserOffset.Value);

            Coordinate pressCoordinate = new Coordinate(coordX, coordY);
            return pressCoordinate;
        }

        /// <summary>
        /// Gets the zoom multiplier if it is required.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>double</returns>
        public double GetZoomRequired(string elementId)
        {
            double zoomMultipler = 1;

            string style = $"document.getElementById('{elementId}').className";

            string className = ExecuteJavaScript($"{style}");
            if (_classNames.Any(n => className.Contains(n)))
            {
                zoomMultipler = 1.28;
            }

            return zoomMultipler;
        }

        /// <summary>
        /// Parses the specified JavaScript bounding area string into a <see cref="BoundingBox" />.
        /// </summary>
        /// <param name="boundingArea">A string representing the bounding area, as returned from getBoundingClientRect().</param>
        /// <returns>A <see cref="BoundingBox" /> representing the specified bounding area.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="boundingArea" /> is null.</exception>
        /// <exception cref="FormatException"><paramref name="boundingArea" /> is not a valid bounding area string.</exception>
        private static BoundingBox ParseBoundingArea(string boundingArea)
        {
            if (boundingArea == null)
            {
                throw new ArgumentNullException(nameof(boundingArea));
            }

            try
            {
                Dictionary<string, int> properties = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Expected format for bounding area string:
                // {"left":20,"right":380,"top":54,"height":35,"bottom":89,"width":360}
                string[] pieces = boundingArea.Split("{},:\"".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < pieces.Length; i += 2)
                {
                    // For some solutions, the bounding area properties may be decimals
                    properties.Add(pieces[i], (int)decimal.Parse(pieces[i + 1]));
                }

                Coordinate location = new Coordinate(properties["left"], properties["top"]);
                Size size = new Size(properties["width"], properties["height"]);
                return new BoundingBox(location, size);
            }
            catch (Exception ex) when (ex is FormatException || ex is KeyNotFoundException || ex is IndexOutOfRangeException)
            {
                throw new FormatException($"'{boundingArea}' is not a valid bounding area string.");
            }
        }

        /// <summary>
        /// Gets the Y-offset of the OXPd web browser for the specified <see cref="JediOmniControlPanel" />.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <returns>The Y-offset of the OXPd web browser control.</returns>
        /// <exception cref="ElementNotFoundException">The OXPd browser could not be found.</exception>
        public static int GetBrowserOffset(JediOmniControlPanel controlPanel)
        {
            return controlPanel.GetBoundingBox("#hpid-oxpd-scroll-pane").Top;
        }

        /// <summary>
        /// Gets the Y-offset of the OXPd web browser for the specified <see cref="JediWindjammerControlPanel" />.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <returns>The Y-offset of the OXPd web browser control.</returns>
        /// <exception cref="DeviceInvalidOperationException">An OXPd browser control could not be found.</exception>
        public static int GetBrowserOffset(JediWindjammerControlPanel controlPanel)
        {
            var browsers = new[] { "mWebBrowser", "WebKitBrowser" };
            var controls = controlPanel.GetControls();
            foreach (string browser in browsers)
            {
                if (controls.Contains(browser, StringComparer.OrdinalIgnoreCase))
                {
                    return controlPanel.GetProperty<int>(browser, "Top");
                }
            }
            throw new DeviceInvalidOperationException("No known OXPd browser control was found on the control panel.");
        }
    }
}
