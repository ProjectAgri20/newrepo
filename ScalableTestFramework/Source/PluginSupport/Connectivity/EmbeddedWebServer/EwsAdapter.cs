using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using OpenQA.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    /// <summary>
    /// Supported Adapter types
    /// </summary>
    public enum EwsAdapterType
    {
        /// <summary>
        /// The SeleniumRC adapter type
        /// </summary>
        SeleniumServerRC,
        /// <summary>
        /// Web Driver Type
        /// </summary>
        WebDriverAdapter
    }

    /// <summary>
    /// Adapter that supports starting and communicating with an EWS instance
    /// </summary>
    public class EwsAdapter : IDisposable
    {
        private EwsSiteMapData _siteMapData = null;
        private ISeleniumBase _client = null;
        private EwsSettings _settings = null;

        /// <summary>
        /// Initializes a new instance of the class.<see cref="EwsAdapter" /> 
        /// </summary>
        /// <param name="settings">The settings.</param>
        public EwsAdapter(EwsSettings settings, EwsSeleniumSettings seleniumSettings, string siteMapPath)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            _settings = settings;

            if (settings.AdapterType == EwsAdapterType.SeleniumServerRC)
            {

                _client = new SeleniumServer
                    (
                        CopyServerJarFile(seleniumSettings),
                        settings.HttpRemoteControlHost,
                        settings.HttpRemoteControlPort
                    );

            }
            else if (settings.AdapterType == EwsAdapterType.WebDriverAdapter)
            {
                _client = new SeleniumWebDriver(settings.Browser, CopyWebDriverEXEFiles(settings.Browser, seleniumSettings));
                _client.PageNavigationDelay = settings.PageNavigationDelay;
                _client.ElementOperationDelay = settings.ElementOperationDelay;
            }

            _siteMapData = new EwsSiteMapData(settings, siteMapPath);
        }

        private static string CopyServerJarFile(EwsSeleniumSettings seleniumSettings)
        {
            // This ensures the Selenium RC jar file is on the local machine.  Get the file from the
            // location specified in the settings and copy it to the location defined in the AppData directory.
            // Return this value as it is sent to the SeleniumServer class to tell it where to go to start the
            // server file.

            string source = seleniumSettings.SeleniumServerJarFile;

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SeleniumRC");
            string destination = Path.Combine(directory, "SeleniumServer.jar");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(destination))
            {
                File.Copy(source, destination, true);
            }

            return destination;
        }

        /// <summary>
        /// Reads the file path from database and copy over the driver files for IE and Chrome.
        /// For IE, copies over the Driver file based on the processor type.
        /// For Chrome, copies over the 32-bit driver file as driver support is available only for 32-bit.
        /// </summary>
        /// <param name="browser">Browser model</param>
        /// <returns>Destination directory</returns>
        public static string CopyWebDriverEXEFiles(BrowserModel browser, EwsSeleniumSettings seleniumSettings)
        {
            if (browser == BrowserModel.Firefox)
            {
                return string.Empty;
            }

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string destDirectory = Path.Combine(appDataPath, "SeleniumWebDriver");

            string source = string.Empty;
            string destination = string.Empty;

            if (browser == BrowserModel.Explorer)
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    source = seleniumSettings.SeleniumIEDriverPath64;
                }
                else
                {
                    source = seleniumSettings.SeleniumIEDriverPath32;
                }
                destination = Path.Combine(destDirectory, "IEDriverServer.exe");
            }
            else if (browser == BrowserModel.Chrome)
            {
                source = seleniumSettings.SeleniumChromeDriverPath;
                destination = Path.Combine(destDirectory, Path.GetFileName(source));
            }

            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            File.Copy(source, destination, true);

            return destDirectory;
        }


        /// <summary>
        /// Gets the site map data.
        /// </summary>
        /// <value>
        /// The site map data.
        /// </value>
        public EwsSiteMapData SiteMapData
        {
            get { return _siteMapData; }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public EwsSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the title of the current page that is being displayed
        /// </summary>
        public string Title
        {
            get
            {
                return _client.Title;
            }
        }

        /// <summary>
        /// Gets the body of the currently navigated HTML page
        /// </summary>
        public string Body
        {
            get
            {
                return _client.Body;
            }
        }

        /// <summary>
        /// Navigate to specified Url
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hypertext">protocol: http or https</param>
        public void Navigate(string path, string hypertext = "https")
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            string url = path;
            if (!path.StartsWith("http", StringComparison.OrdinalIgnoreCase) ||
                !path.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                _siteMapData.SelectPage(path, hypertext);
                url = _siteMapData.DeviceUrl.AbsoluteUri;
            }

            _client.Open(new Uri(url));
        }

        /// <summary>
        /// Launches the browser based on configuration settings.
        /// </summary>
        /// <param name="hypertext">The hypertext "http" or "https"</param>
        public void Start(string hypertext = "https")
        {
            Start(_settings.Browser, hypertext);
        }

        /// <summary>
        /// Launches the selected browser overriding configuration settings.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="hypertext">The hypertext "http" or "https"</param>
        public void Start(BrowserModel browser, string hypertext)
        {
            _client.Start(browser, new Uri("{0}://{1}".FormatWith(hypertext, _settings.DeviceAddress)));
        }

        /// <summary>
        /// Waits the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void Wait(TimeSpan timeout)
        {
            SeleniumServer.Wait(timeout);
        }

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page
        /// </summary>
        /// <param name="controlId">Id of the control on which click has to be performed</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void Click(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Click(locator, sourceType);
        }

        /// <summary>
        /// Click on hyperlink present in the page
        /// </summary>
        /// <param name="linkText">Text displayed on the hyperlink on which click has to be performed</param>
        public void ClickonLink(string linkText)
        {
            FindType sourceType = FindType.ByLinkText;
            _client.Click(linkText, sourceType);
        }

        /// <summary>
        /// Clicks on OK button of popup window
        /// </summary>
        public void ClickOkonAlert()
        {
            _client.ClickOkonAlert();
        }

        /// <summary>
        /// Clicks on Cancel button of popup window
        /// </summary>
        public void ClickCancelonAlert()
        {
            _client.ClickCancelonAlert();
        }

        /// <summary>
        /// Set the value of the text-box
        /// </summary>
        /// <param name="controlId">Id of the text-box button</param>
        /// <param name="value">Text value to be set</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <param name="sendTab">True to send tab, else false.</param>
        public void SetText(string controlId, string value, bool useSitemapId = true, FindType sourceType = FindType.ById, bool sendTab = false)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.SetText(locator, value, sourceType, sendTab);
        }

        /// <summary>
        /// Get the value of the text-box
        /// </summary>
        /// <param name="controlId">Id of the text-box button</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns></returns>
        public string GetText(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetText(locator, sourceType);
        }

        /// <summary>
        /// Returns whether the specified checkbox/ radio button is checked or not
        /// </summary>
        /// <param name="controlId">Id of the button</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>true if checked, false otherwise</returns>
        public bool IsChecked(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.IsChecked(locator, sourceType);
        }

        /// <summary>
        /// Search for the specified string/text in the current page
        /// </summary>
        /// <param name="pattern">string to be searched</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>
        /// true if string found, false otherwise
        /// </returns>
        public bool SearchText(string pattern, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            return _client.IsTextPresent(pattern);
        }

        /// <summary>
        /// Select an option in the drop-down box
        /// </summary>
        /// <param name="controlId">ID of the drop-down box</param>
        /// <param name="optionId">ID of the option to be selected</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void SelectDropDown(string controlId, string optionId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Select(locator, optionId, sourceType);
        }

        /// <summary>
        /// Check a checkbox
        /// </summary>
        /// <param name="controlId">ID of the checkbox</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void Check(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Check(locator, sourceType);
        }

        /// <summary>
        /// Uncheck a checkbox
        /// </summary>
        /// <param name="controlId">ID of the checkbox</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void Uncheck(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Uncheck(locator, sourceType);
        }

        /// <summary>
        /// Gets the (whitespace-trimmed) value of an input field (or anything else with a value parameter).
        /// For checkbox/radio elements, the value will be "on" or "off" depending on
        /// whether the element is checked or not.
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public string GetValue(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetValue(locator, sourceType);
        }

        /// <summary>
        /// Ends the test session, killing the browser
        /// </summary>
        public void Stop()
        {
            _client.Stop();
        }

        /// <summary>
        /// Waits for a new page to load.
        /// </summary>
        /// <param name="timeout">a timeout in milliseconds, after which this command will return with an error</param>
        public void WaitForPageToLoad(string timeout)
        {
            _client.WaitForPageToLoad(timeout);
        }

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page using name of the control
        /// </summary>
        /// <param name="controlId">Id of the control on which click has to be performed</param>
        public void ClickByName(string controlId)
        {
            _client.Click("name={0}".FormatWith(controlId), FindType.ByName);
        }

        /// <summary>
        /// Gets the text from a cell of a table.
        /// </summary>
        /// <param name="tableCellAddress">table id</param>
        /// <param name="row">row value of the cell</param>
        /// <param name="column">column value of the cell</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>the text from the specified cell</returns>
        //public string GetTableCell(string tableCellAddress, int row, int column)
        //{
        //    return _client.GetTableCell(tableCellAddress, row, column);
        //}
        public string GetTableCell(string controlId, int row, int column, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetTableCell(locator, row, column);
        }

        /// <summary>
        /// Set focus of the control
        /// </summary>
        /// <param name="controlId">Id of the text-box button</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns></returns>
        public void SetFocus(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.SetFocus(locator);
        }

        /// <summary>
        /// Selects the item from a listbox.
        /// </summary>
        /// <param name="controlId">Id of the listbox</param>
        /// <param name="controlItem">Text of list item</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void Select(string controlId, string controlItem, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Select(locator, controlItem, sourceType);
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on index.
        /// </summary>
        /// <param name="controlId">Id of the listbox</param>
        /// <param name="index">Index of list item</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void Select(string controlId, int index, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.Select(locator, index, sourceType);
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on the value.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="value">The value to be selected.</param>
        /// <param name="type">Type of locator</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void SelectByValue(string controlId, string value, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.SelectByValue(locator, value, sourceType);
        }

        /// <summary>
        /// Clear all selected entries from a list box. Supported onlywhen the select element supports multiple selection.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator.</param>
        public void DeselectAll(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.DeselectAll(locator, sourceType);
        }

        /// <summary>
        /// Get the list items from a listbox, dropdown box.
        /// </summary>
        /// <param name="controlId">The select locator.</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public List<string> GetListItems(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetListItems(locator, sourceType).ToList();
        }

        /// <summary>
        /// Set the value of the Browse Control
        /// </summary>
        /// <param name="controlId">The control unique identifier.</param>
        /// <param name="value">Value.</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public void SetBrowseControlText(string controlId, string value, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            _client.SetBrowseControlText(locator, value, sourceType);

        }

        /// <summary>
        /// Selects one or more rows in a table for given column names
        /// </summary>
        /// <param name="controlId">The locator</param>
        /// <param name="columnTexts">The text for consecutive columns</param>
        /// <returns>True if succeeded</returns>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        public bool SelectTableRows(string controlId, StringCollection columnTexts, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.SelectTableRows(locator, columnTexts, sourceType);
        }

        /// <summary>
        /// Get data of all the rows whose first text column is of 'columnText'
        /// </summary>
        /// <param name="controlId">The locator</param>
        /// <param name="columnText">The text for first text column in the table row</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>Collection of a list of rows</returns>
        public List<StringCollection> GetTableRowsData(string controlId, String columnText, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetTableRowsData(locator, columnText, sourceType).ToList();
        }

        /// <summary>
        /// Get Table Rows
        /// </summary>
        /// <param name="controlId">The locator</param>        
        /// <param name="searchText">Select the row based on the text provided</param> 
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>list of rows</returns>
        public string GetTableRows(string controlId, string searchText, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetTableRows(locator, searchText, sourceType);
        }

        /// <summary>
        /// Get Table Details
        /// </summary>
        /// <param name="controlId">The locator</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <returns>Collection of table details</returns>
        public Collection<Collection<string>> GetTable(string controlId, bool includeHeader = true, int[] columnIndex = null, FindType elementType = FindType.ById, bool returnValue = true, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

            return _client.GetTable(locator, type: sourceType, includeHeader: includeHeader, columnIndex: columnIndex, elementType: elementType, returnValue: returnValue);
        }

        /// <summary>
        /// Navigate to specified IPV6 Url
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="hypertext">protocol: http or https</param>
        public void NavigateIPv6(string path, string hypertext = "http")
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            path = "https://[" + path + "]";
            _client.Open(new Uri(path));
        }

        /// <summary>
        /// Check if element is present in page
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <param name="sourceType">Default mentions the element is of Id type</param>
        /// <param name="retry">True to do a retry to find out the element.</param>
        /// <returns></returns>
        public bool IsElementPresent(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById, bool retry = true)
        {
            try
            {
                string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;

                return _client.IsElementPresent(locator, sourceType, retry);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposed"></param>
        protected virtual void Dispose(bool disposed)
        {
            if (!disposed)
            {
                _client.Dispose();
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
        /// Get IWebElements of a html page
        /// </summary>
        /// <param name="controlId">The locator</param>
        /// <param name="sourceType">Type of locator</param>
        /// <param name="useSitemapId">By default element id will be picked from sitemaps, false to use the control Id directly</param>
        /// <returns>List of IWebElements</returns>
        public IList<IWebElement> GetPageElements(string controlId, bool useSitemapId = true, FindType sourceType = FindType.ById)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(controlId, out sourceType) : controlId;
            return _client.GetPageElements(locator, sourceType);
        }

        /// <summary>
        /// Sets the date on the date control.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">Date time value to be set.</param>
        /// <param name="type">Type of locator.</param>
        /// <param name="sendTab">True to send tab, else false.</param>
        public void SetDateControlText(string associatedControl, DateTime value, FindType sourceType = FindType.ById, bool useSitemapId = true)
        {
            string locator = useSitemapId == true ? _siteMapData.GetLocator(associatedControl, out sourceType) : associatedControl;
            _client.SetDateControlText(locator, value, sourceType);
        }

        /// <summary>
        /// Executes java script on a page/ Form. Pass the control names enclosed in $${ControlName}&&. ControlName is the key available from sitemaps.
        /// e.g.: If the JavaScript is "UsernameTextBox.Val('admin'), pass the script as "$$UserName$$.val('admin'). 
        /// If multiple controls needs to be used enclose each control in $${ControlName}&&.
        /// <param name="script">Java script to be executed.</param>
        public void ExecuteScript(string script)
        {
            FindType sourceType = FindType.ById;
            string locator = string.Empty;

            foreach (var item in Regex.Matches(script, "~[^~]+~"))
            {
                locator = _siteMapData.GetLocator(item.ToString().Trim('~', '~'), out sourceType);
                script = script.Replace(item.ToString(), locator);
            }

            _client.ExecuteScript(script);
        }

        /// <summary>
        /// Send Tab on a control.
        /// </summary>
        /// <param name="locator"><The locator.s/param>
        /// <param name="type">Type of locator.</param>
        public void SendTab(string control, FindType type = FindType.ById)
        {
            string locator = _siteMapData.GetLocator(control, out type);
            _client.SendTab(locator, type);
        }

        /// <summary>
        /// Captures the screen shot for the current page.
        /// </summary>
        /// <param name="filePath">The file path to save the file</param>
        public void CaptureScreenShot(string filePath)
        {
            _client.CaptureScreenShot(filePath);
        }
    }
}