using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using OpenQA.Selenium;
using Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.Selenium
{
    /// <summary>
    /// Web automation client layered over Selenium
    /// </summary>
    public class SeleniumServer : ISeleniumBase, IDisposable
    {
        private ISelenium _selenium;
        private string _jarFile = string.Empty;
        private string _serverHost = string.Empty;
        private int _serverPort = 4444;

        private const string _seleniumJar = "Selenium.jar";

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumServer" /> class.
        /// </summary>
        /// <param name="jarFile">The jar file.</param>
        /// <param name="serverHost">The server host.</param>
        /// <param name="serverPort">The server port.</param>
        public SeleniumServer(string jarFile, string serverHost, int serverPort = 4444)
        {
            _jarFile = jarFile;
            _serverHost = serverHost;
            _serverPort = serverPort;
        }

        private static bool SeleniumIsRunning()
        {
            return
                (
                    from c in GetCommandLines("java.exe")
                    where c.Contains(_seleniumJar, StringComparison.OrdinalIgnoreCase)
                    select c
                ).Any();
        }

        private static IEnumerable<string> GetCommandLines(string processName)
        {
            List<string> results = new List<string>();

            string wmiQuery = string.Format(CultureInfo.InvariantCulture, "select CommandLine from Win32_Process where Name='{0}'", processName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery))
            {
                using (ManagementObjectCollection retObjectCollection = searcher.Get())
                {
                    foreach (ManagementObject retObject in retObjectCollection)
                    {
                        results.Add((string)retObject["CommandLine"]);
                    }
                }
            }
            return results;
        }

        private void StartServer()
        {
            if (SeleniumIsRunning())
            {
                return;
            }

            using (Process process = new Process())
            {
                Logger.LogDebug("Starting Selenium Remote Control Server");
                process.StartInfo.FileName = JavaUtil.JavaExePath;
                process.StartInfo.Arguments = "-jar \"{0}\" -trustAllSSLCertificates".FormatWith(_jarFile);
                process.Start();
            }
            Thread.Sleep(120000);
            //WaitOnStart();
        }

        private void WaitOnStart()
        {
            // Try to establish connection with the Selenium server using an HTTP Response
            // object and then query its status object looking for an OK status code.
            // This will cause a block until the server starts or a timeout is hit.
            int attempts = 0;
            int maxAttempts = 5;
            int secondsBetweenAttempts = 5;

            do
            {
                attempts++;
                var url = "http://{0}:{1}/wd/hub/status".FormatWith(_serverHost, _serverPort);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";

                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        break;
                    }
                }
                catch (WebException ex)
                {
                    Logger.LogDebug(ex.Message);
                }

                Thread.Sleep(TimeSpan.FromSeconds(secondsBetweenAttempts));

            } while (attempts <= maxAttempts);

            if (attempts > maxAttempts)
            {
                throw new WebException("Unable to communicate with Selenium Server");
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets the title of the current page that is being displayed
        /// </summary>
        public string Title
        {
            get
            {
                return _selenium.GetTitle();
            }
        }

        /// <summary>
        /// Gets the body of the currently navigated HTML page
        /// </summary>
        public string Body
        {
            get
            {
                return _selenium.GetBodyText();
            }
        }

        /// <summary>
        /// Gets or sets the page navigation delay
        /// </summary>
        public TimeSpan PageNavigationDelay { get; set; }

        /// <summary>
        /// Gets or sets the element operation delay 
        /// </summary>
        public TimeSpan ElementOperationDelay { get; set; }

        #endregion

        /// <summary>
        /// Launches the browser.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="uri">The URI.</param>
        public void Start(BrowserModel browser, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            StartServer();

            string browserStr = Enum<BrowserModel>.Value(browser);
            _selenium = new DefaultSelenium(_serverHost, _serverPort, browserStr, uri.AbsoluteUri);

            _selenium.Start();
            _selenium.WindowFocus();
            //_selenium.WindowMaximize();
            _selenium.Open("/");
            _selenium.WaitForPageToLoad("50000");
        }

        /// <summary>
        /// Waits the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public static void Wait(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public void Open(string uri)
        {
            Uri newUri = new Uri(uri);
            Open(newUri);
        }

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public void Open(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            _selenium.Open(uri.AbsoluteUri);
        }

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void Click(string locator, FindType type = FindType.ById)
        {
            _selenium.ChooseOkOnNextConfirmation();
            _selenium.Click(locator);
            if (_selenium.IsConfirmationPresent())
            {
                _selenium.GetConfirmation();
            }
        }

        /// <summary>
        /// Clicks on OK button of popup window
        /// </summary>
        public void ClickOkonAlert()
        {
            _selenium.ChooseOkOnNextConfirmation();
        }

        /// <summary>
        /// Clicks on Cancel button of popup window
        /// </summary>
        public void ClickCancelonAlert()
        {
            _selenium.ChooseCancelOnNextConfirmation();
        }

        /// <summary>
        /// Set the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">Type of locator</param>
        public void SetText(string locator, string value, FindType type = FindType.ById, bool sendTab = false)
        {
            _selenium.Type(locator, value);
        }

        /// <summary>
        /// Get the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns></returns>
        public string GetText(string locator, FindType type = FindType.ById)
        {
            return _selenium.GetText(locator);
        }

        /// <summary>
        /// Selects the from a dropdown.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="optionLocator">The option locator.</param>
        /// <param name="type">Type of locator</param>
        public void Select(string selectLocator, string optionLocator, FindType type = FindType.ById)
        {
            _selenium.Select(selectLocator, optionLocator);
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on index.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="index">Index of list item</param>
        /// <param name="type">Type of locator</param>
        public void Select(string selectLocator, int index, FindType type = FindType.ById)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the list items from a listbox, dropdown box.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator</param>        
        public IEnumerable<string> GetListItems(string selectLocator, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns whether the specified checkbox/ radio button is checked or not
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns>
        /// true if checked, false otherwise
        /// </returns>
        public bool IsChecked(string locator, FindType type = FindType.ById)
        {
            return _selenium.IsChecked(locator);
        }

        /// <summary>
        /// Navigate to specified Url
        /// </summary>
        /// <param name="uri">The URL.</param>
        public void NavigateTo(string uri)
        {
            NavigateTo(new Uri(uri));
        }

        /// <summary>
        /// Navigate to specified Url
        /// </summary>
        /// <param name="uri">The URI.</param>
        public void NavigateTo(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            _selenium.Open(uri.AbsoluteUri);
        }

        /// <summary>
        /// Search for the specified string/text in the current page
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>
        /// true if string found, false otherwise
        /// </returns>
        public bool IsTextPresent(string pattern)
        {
            return _selenium.IsTextPresent(pattern);
        }

        /// <summary>
        /// Check a checkbox
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void Check(string locator, FindType type = FindType.ById)
        {
            _selenium.Check(locator);
        }

        /// <summary>
        /// Unchecks the specified locator.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void Uncheck(string locator, FindType type = FindType.ById)
        {
            _selenium.Uncheck(locator);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            var path = "http://{0}:{1}/selenium-server/driver/?cmd=shutDownSeleniumServer".FormatWith(_serverHost, _serverPort);
            var request = (HttpWebRequest)WebRequest.Create(new Uri(path));
            request.GetResponse();
        }

        /// <summary>
        /// Gets the (whitespace-trimmed) value of an input field (or anything else with a value parameter).
        /// For checkbox/radio elements, the value will be "on" or "off" depending on
        /// whether the element is checked or not.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns></returns>
        public string GetValue(string locator, FindType type = FindType.ById)
        {
            return _selenium.GetValue(locator);
        }

        /// <summary>
        /// Ends the test session, killing the browser
        /// </summary>
        public void Close()
        {
            _selenium.Stop();
        }

        /// <summary>
        /// Waits for a new page to load.
        /// </summary>
        /// <param name="timeout">a timeout in milliseconds, after which this command will return with an error</param>
        public void WaitForPageToLoad(String timeout)
        {
            _selenium.WaitForPageToLoad(timeout);
        }

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page using name of the control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void ClickByName(string locator, FindType type = FindType.ByName)
        {
            _selenium.Click("name=" + locator);
        }

        /// <summary>
        /// Gets the text from a cell of a table.
        /// </summary>
        /// <param name="tableCellAddress">table id</param>
        /// <param name="row">row value of the cell</param>
        /// <param name="column">column value of the cell</param>
        /// <returns>the text from the specified cell</returns>
        public string GetTableCell(string tableCellAddress, int row, int column)
        {
            string cellValue = string.Empty;
            cellValue = _selenium.GetTable(@"{0}.{1}.{2}".FormatWith(tableCellAddress, row, column));
            return cellValue;
        }

        /// <summary>
        /// Set the focus for control
        /// </summary>
        /// <param name="locator">The locator.</param>        
        public void SetFocus(string locator)
        {
            _selenium.Focus(locator);
        }

        /// <summary>
        /// Checks whether the element exists or not
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <returns>True if the element present else false</returns>
        public bool IsElementExists(string locator)
        {
            return _selenium.IsElementPresent(locator);
        }

        /// <summary>
        /// Set the value of the Browse Control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">The value to be set in browse control.</param>
        /// <param name="type">Type of locator</param>
        public void SetBrowseControlText(string locator, string value, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for SeleniumRC
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnTexts">The text for consecutive columns</param>
        /// <param name="type">Type of locator</param>
        /// <returns>True if succeeded</returns>
        public bool SelectTableRows(string locator, StringCollection columnTexts, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for SeleniumRC
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnText">The text for first text column in the table row</param>
        /// <param name="type">Type of locator</param>
        /// <returns>Collection of a list of rows</returns>
        public IEnumerable<StringCollection> GetTableRowsData(string locator, String columnText, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposed"></param>
        protected virtual void Dispose(bool disposed)
        {
            if (!disposed)
            {
                _selenium = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This function is not implemented for selenium RC
        /// </summary>
        /// <param name="linkText">The link text</param>
        public void ClickonLink(string linkText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for selenium RC
        /// </summary>
        /// <param name="linkText">The link text</param>
        public string GetTableRows(string controlId, string searchText, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for selenium RC
        /// </summary>
        /// <param name="selectLocator"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void SelectByValue(string selectLocator, string value, FindType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for Selenium RC
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="type"></param>
        /// <returns></returns>        
        public Collection<Collection<string>> GetTable(string locator, FindType type, bool includeHeader = true, int[] columnIndex = null, FindType elementType = FindType.ById, bool returnValue = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for Selenium RC
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="type"></param>
        /// <param name="retry">True to do a retry to find out the element.</param>
        /// <returns></returns>
        public bool IsElementPresent(string locator, FindType type, bool retry = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented for Selenium RC
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<IWebElement> GetPageElements(string locator, FindType type)
        {
            throw new NotImplementedException();
        }


        public void SetDateControlText(string locator, DateTime value, FindType type)
        {
            throw new NotImplementedException();
        }


        public void ExecuteScript(string script)
        {
            throw new NotImplementedException();
        }

        public void SendTab(string locator, FindType type)
        {
            throw new NotImplementedException();
        }

        public void DeselectAll(string selectLocator, FindType type)
        {
            throw new NotImplementedException();
        }

        public void CaptureScreenShot(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
