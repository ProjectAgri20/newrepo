using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace HP.ScalableTest.PluginSupport.Connectivity.Selenium
{
    /// <summary>
    /// This class derives from ISeleniumBase interface. It wraps the functionality of Selenium WebDriver.
    /// </summary>
    public class SeleniumWebDriver : ISeleniumBase
    {
        #region Private Member Variables

        private BrowserModel _browserModel;
        private IWebDriver _webDriver;
        private readonly string _driverExePath;

        #endregion Private Member Variables

        #region Constructor

        /// <summary>
        /// Create the web driver for the specified browser type.
        /// </summary>
        /// <param name="browserModel">The type of browser for which to create the web driver</param>
        /// <param name="driverExePath">Path to Driver EXE of browser</param>
        public SeleniumWebDriver(BrowserModel browserModel, string driverExePath)
        {
            _browserModel = browserModel;
            _driverExePath = driverExePath;
        }

        #endregion Constructor

        #region Public Properties

        /// <summary>
        /// Gets the title of the current page that is being displayed
        /// </summary>
        public string Title
        {
            get { return _webDriver.Title; }
        }

        /// <summary>
        /// Gets the body of the currently navigated HTML page
        /// </summary>
        public string Body
        {
            get
            {
                IWebElement bodyElement = _webDriver.FindElement(By.TagName("body"));
                return bodyElement?.Text;
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

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Launches the home page in the browser.
        /// </summary>
        /// <param name="browser">browser model</param>
        /// <param name="uri">The URI.</param>
        public void Start(BrowserModel browser, Uri uri)
        {
            _browserModel = browser;

            switch (browser)
            {
                case BrowserModel.Explorer:
                    _webDriver = new InternetExplorerDriver(_driverExePath);
                    break;

                case BrowserModel.Chrome:
                    _webDriver = new ChromeDriver(_driverExePath);
                    break;

                case BrowserModel.Firefox:
                    {
                        FirefoxProfile profile = new FirefoxProfile { AcceptUntrustedCertificates = true };
                        _webDriver = new FirefoxDriver(profile);
                        _webDriver.Manage().Window.Maximize();
                    }
                    break;

                default:
                    Debug.Assert(false, "Unsupported browser type specified");
                    break;
            }

            SetBrowserTimeOut();

            Open(uri);

            if (_webDriver.GetType() == typeof(InternetExplorerDriver) && _webDriver.Title.Contains("Certificate"))
                _webDriver.Navigate().GoToUrl("javascript:document.getElementById('overridelink').click()");
        }

        /// <summary>
        /// Sets the browser timeout to 1 minute when nothing is mentioned.
        /// </summary>
        /// <param name="span">Time out to wait for an element or operation</param>
        private void SetBrowserTimeOut(TimeSpan? span = null)
        {
            IOptions options = _webDriver?.Manage();
            ITimeouts timeouts = options?.Timeouts();
            if (timeouts != null)
            {
                TimeSpan waitAMinute = span ?? new TimeSpan(0, 1, 0);
                timeouts.ImplicitlyWait(waitAMinute);
            }
        }

        /// <summary>
        /// Quits the driver and close every associated window.
        /// </summary>
        public void Stop()
        {
            _webDriver?.Quit();
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
            NavigateTo(uri);
        }

        /// <summary>
        /// Navigates the browser to the specified URI.
        /// </summary>
        /// <param name="uri">The URL to navigate to</param>
        /// <returns></returns>
        public void NavigateTo(String uri)
        {
            Uri newUri = new Uri(uri);
            NavigateTo(newUri);
        }

        /// <summary>
        /// Navigate to specified Url. https page can also be launched
        /// </summary>
        /// <param name="uri">The URI.</param>
        public void NavigateTo(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            INavigation navigation = _webDriver.Navigate();
            string url = uri.AbsoluteUri;

            navigation.GoToUrl(new Uri(url));

            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                if (_browserModel == BrowserModel.Explorer || _browserModel == BrowserModel.Chrome)
                {
                    navigation.GoToUrl(new Uri("javascript:document.getElementById('overridelink').click()"));
                }
            }

            // wait for specified time before returning the control back
            Thread.Sleep(PageNavigationDelay);
        }

        /// <summary>
        /// Locates the element by type and wrap it to Select a Element
        /// </summary>
        /// <param name="locator">The locator to find</param>
        /// <param name="type">Type of locator</param>
        /// <returns></returns>
        public SelectElement FindSelectElement(string locator, FindType type)
        {
            SelectElement selectElement = null;
            IWebElement webElement = FindElement(locator, type);
            if (null != webElement)
            {
                selectElement = new SelectElement(webElement);
            }

            return selectElement;
        }

        /// <summary>
        /// Finds the element based on the find type for the locator specified.
        /// </summary>
        /// <param name="locator">The locator to find</param>
        /// <param name="type">Type of locator</param>
        /// <param name="retry">True to do a retry to find out the element.</param>
        /// <returns></returns>
        public IWebElement FindElement(string locator, FindType type, bool retry = true)
        {
            try
            {
                By findType = null;

                switch (type)
                {
                    case FindType.ById:
                        findType = By.Id(locator);
                        break;

                    case FindType.ByName:
                        findType = By.Name(locator);
                        break;

                    case FindType.ByTagName:
                        findType = By.TagName(locator);
                        break;

                    case FindType.ByXPath:
                        findType = By.XPath(locator);
                        break;

                    case FindType.ByClassName:
                        findType = By.ClassName(locator);
                        break;

                    case FindType.ByLinkText:
                        findType = By.LinkText(locator);
                        break;

                    default:
                        Debug.Assert(false, "Invalid find type passed to find element");
                        break;
                }

                if (retry)
                {
                    SetBrowserTimeOut();
                }
                else
                {
                    SetBrowserTimeOut(TimeSpan.FromSeconds(5));
                }

                if (null != findType)
                {
                    int retryCount = 0;
                    do
                    {
                        IWebElement element = _webDriver.FindElement(findType);
                        if (element != null && element.Enabled)
                        {
                            return element;
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        retryCount++;
                    }
                    while (retry && retryCount < 10);
                }
            }
            catch (NoSuchElementException)
            {
                throw new Exception("Element {0} could not found on the current page".FormatWith(locator));
            }
            catch (Exception e)
            {
                throw new Exception("Element {0} could not found on the current page. Exception: {1}".FormatWith(locator, e.Message));
            }

            return null;
        }

        /// <summary>
        /// Click on a button present in the page
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void Click(string locator, FindType type)
        {
            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                if (_browserModel == BrowserModel.Firefox)
                {
                    element.Click();
                }
                else
                {
                    element.SendKeys(Keys.Enter);
                }

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Click on a link present in the page using the link text
        /// </summary>
        /// <param name="linkText">Link Text</param>
        public void ClickonLink(string linkText)
        {
            Click(linkText, FindType.ByLinkText);
        }

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page using name of the control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void ClickByName(string locator, FindType type = FindType.ByName)
        {
            Click(locator, type);
        }

        /// <summary>
        /// Clicks on OK button of popup window
        /// </summary>
        public void ClickOkonAlert()
        {
            try
            {
                IAlert alert = _webDriver.SwitchTo().Alert();
                alert.Accept(); //choose the affirmative button
            }
            catch (NoAlertPresentException ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        /// <summary>
        /// Clicks on Cancel button of popup window
        /// </summary>
        public void ClickCancelonAlert()
        {
            try
            {
                IAlert alert = _webDriver.SwitchTo().Alert();
                alert.Dismiss();
            }
            catch (NoAlertPresentException ex)
            {
                Logger.LogDebug(ex.Message);
            }
        }

        /// <summary>
        /// Set the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <param name="value">The value.</param>
        /// <param name="sendTab">True to send tab, else false.</param>
        public void SetText(string locator, string value, FindType type, bool sendTab = false)
        {
            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                if (sendTab)
                {
                    element.SendKeys(Keys.Tab);
                }
                element.Clear();
                element.SendKeys(value);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Get the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns>text of the locator</returns>
        public string GetText(string locator, FindType type)
        {
            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                return element.Text;
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieves the value of the control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns>value of the locator</returns>
        public string GetValue(string locator, FindType type)
        {
            string value = string.Empty;

            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                value = element.GetAttribute("value");
            }

            return value;
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on text.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="optionLocator">Text of list item</param>
        /// <param name="type">Type of locator</param>
        public void Select(string selectLocator, string optionLocator, FindType type)
        {
            SelectElement select = FindSelectElement(selectLocator, type);
            if (null != select)
            {
                select.SelectByText(optionLocator);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on index.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="index">Index of list item</param>
        /// <param name="type">Type of locator</param>
        public void Select(string selectLocator, int index, FindType type)
        {
            SelectElement select = FindSelectElement(selectLocator, type);
            if (null != select)
            {
                select.SelectByIndex(index);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on the value.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="value">The value to be selected.</param>
        /// <param name="type">Type of locator</param>
        public void SelectByValue(string selectLocator, string value, FindType type)
        {
            SelectElement select = FindSelectElement(selectLocator, type);

            if (null != select)
            {
                select.SelectByValue(value);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Clear all selected entries from a list box. Supported onlywhen the select element supports multiple selection.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator.</param>
        public void DeselectAll(string selectLocator, FindType type)
        {
            // Deselect all only if there are items in the control
            if (GetListItems(selectLocator, type).Count() != 0)
            {
                SelectElement select = FindSelectElement(selectLocator, type);

                if (null != select)
                {
                    select.DeselectAll();

                    // wait for specified time before returning the control back
                    Thread.Sleep(ElementOperationDelay);
                }
            }
        }

        /// <summary>
        /// Get the list items from a listbox, dropdown box.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator</param>
        public IEnumerable<string> GetListItems(string selectLocator, FindType type)
        {
            List<string> rows = new List<string>();

            // The method can be applied to 'select', 'ul', 'ol' element types, So removed the code which is dependent on 'select' elements.
            IWebElement webElement = FindElement(selectLocator, type);

            if (null != webElement)
            {
                rows = webElement.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            return rows;
        }

        /// <summary>
        /// Set the value of the Browse Control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">The value to be set in browse control.</param>
        /// <param name="type">Type of locator</param>
        public void SetBrowseControlText(string locator, string value, FindType type)
        {
            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                element.SendKeys(value);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Checks a checkbox, radio button
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        public void Check(string locator, FindType type)
        {
            IWebElement element = FindElement(locator, type);
            element.Click();

            // The element is not detected after clicking on it on Omni Opus devices. So finding the element again.
            element = FindElement(locator, type);

            if (null != element && !element.Selected)
            {
                element.Click();
            }

            // wait for specified time before returning the control back
            Thread.Sleep(ElementOperationDelay);
        }

        /// <summary>
        /// Returns whether the specified checkbox/radio button is checked or not
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="type">Type of locator</param>
        /// <returns>true if checked, false otherwise</returns>
        public bool IsChecked(string locator, FindType type)
        {
            bool result = false;

            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                result = element.Selected;
            }

            return result;
        }

        /// <summary>
        /// Unchecks a specified locator
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="type">Type of locator</param>
        public void Uncheck(string locator, FindType type)
        {
            IWebElement element = FindElement(locator, type);

            if (null != element && element.Selected)
            {
                element.Click();

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Selects one or more rows in a table for given column names
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnTexts">The text for consecutive columns</param>
        /// <param name="type">Type of locator</param>
        /// <returns>True if succeeded</returns>
        public bool SelectTableRows(string locator, StringCollection columnTexts, FindType type)
        {
            if (columnTexts == null)
            {
                throw new ArgumentNullException("columnTexts");
            }

            bool succeeded = false;

            //Concat the table text columns each delimited by 2 space chars. WebDriver is providing the Row text in this
            //manner.

            StringBuilder searchColText = new StringBuilder();
            foreach (String columnText in columnTexts)
            {
                searchColText.Append(columnText);
                searchColText.Append("  ");
            }

            searchColText.Remove(searchColText.Length - 2, columnTexts.Count > 1 ? 2 : 1);

            IWebElement tableElement = FindElement(locator, type);
            if (tableElement != null) //Get the table by class name.
            {
                ReadOnlyCollection<IWebElement> tableRows = tableElement.FindElements(By.XPath("tbody/tr"));
                foreach (var row in tableRows) //Get all rows in the table.
                {
                    String rowText = row.Text.Trim();

                    bool found = (columnTexts.Count > 1) ? (0 == string.Compare(rowText, searchColText.ToString(), StringComparison.OrdinalIgnoreCase)) :
                                                               (rowText.StartsWith(searchColText.ToString(), StringComparison.OrdinalIgnoreCase));
                    if (found) //Get the complete text of the table and compare.
                    {
                        ReadOnlyCollection<IWebElement> tableColumns = row.FindElements(By.XPath("td"));

                        foreach (var tableColumn in tableColumns) //For each column in the row, click the HTML input element.
                        {
                            try
                            {
                                IWebElement inputElement = tableColumn.FindElement(By.TagName("INPUT"));

                                inputElement.Click(); //Set the focus.
                                if (!inputElement.Selected)
                                {
                                    inputElement.Click();
                                }
                                succeeded = true;
                            }
                            catch (NoSuchElementException)
                            {
                            }
                        }
                    }
                }
            }
            return succeeded;
        }

        /// <summary>
        /// Get data of all the rows whose first text column is of 'columnText'
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnText">The text for first text column in the table row</param>
        /// <param name="type">Type of locator</param>
        /// <returns>Collection of a list of rows</returns>
        public IEnumerable<StringCollection> GetTableRowsData(string locator, String columnText, FindType type)
        {
            if (columnText == null)
            {
                throw new ArgumentNullException("columnText");
            }

            List<StringCollection> listOfRows = null;

            IWebElement tableElement = FindElement(locator, type);
            if (null != tableElement) //Get the table by class name.
            {
                ReadOnlyCollection<IWebElement> tableRows = tableElement.FindElements(By.XPath("tbody/tr"));
                foreach (var row in tableRows) //Get all rows in the table.
                {
                    string rowText = row.Text.Trim();
                    if (rowText.StartsWith(columnText, StringComparison.OrdinalIgnoreCase)) //If the first column text matches with the one passed.
                    {
                        if (null == listOfRows)
                        {
                            listOfRows = new List<StringCollection>();
                        }

                        StringCollection oneRow = new StringCollection();
                        string[] columns = rowText.Split(' ');
                        foreach (string text in columns)
                        {
                            string trimmedText = text.Trim();
                            if (trimmedText.Length != 0)
                            {
                                oneRow.Add(trimmedText);
                            }
                        }
                        listOfRows.Add(oneRow);
                    }
                }
            }
            return listOfRows;
        }

        /// <summary>
        /// Get Table Rows
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="searchText">Search the row based on the text</param>
        /// <param name="type">Type of locator</param>
        /// <returns>list of rows</returns>
        public string GetTableRows(string locator, string searchText, FindType type)
        {
            IWebElement tableElement = FindElement(locator, type);
            ReadOnlyCollection<IWebElement> tableRows = tableElement.FindElements(By.XPath("tbody/tr"));
            string resultData = string.Empty;
            foreach (var row in tableRows) //Get all rows in the table.
            {
                string rowText = row.Text.Trim();
                if (rowText.Contains(searchText, StringComparison.OrdinalIgnoreCase)) //If the first column text matches with the one passed.
                {
                    resultData = resultData + '-' + rowText;
                }
            }
            return resultData;
        }

        /// <summary>
        /// Get Collection of Table details
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="type">Type of locator</param>
        /// <param name="includeHeader">true to include header details in collection, false otherwise</param>
        /// <param name="columnIndex">Column index for those element id/ name to be returned</param>
        /// <param name="elementType"></param>
        /// <param name="returnValue"></param>
        /// <returns>Table details</returns>
        public Collection<Collection<string>> GetTable(string locator, FindType type, bool includeHeader = true, int[] columnIndex = null, FindType elementType = FindType.ById, bool returnValue = true)
        {
            IWebElement tableElement = FindElement(locator, type);
            Collection<Collection<string>> table = new Collection<Collection<string>>();
            Collection<string> tableRow = new Collection<string>();

            if (includeHeader)
            {
                ReadOnlyCollection<IWebElement> tableHeaders = null;

                try
                {
                    tableHeaders = tableElement.FindElements(By.XPath("thead/tr/th"));
                }
                catch
                {
                    //TraceFactory.Logger.Debug("Error: {0}".FormatWith(ex.Message));
                }
                finally
                {
                    if (null == tableHeaders || tableHeaders.Count == 0)
                    {
                        try
                        {
                            tableHeaders = tableElement.FindElements(By.XPath("thead/tr/td"));
                        }
                        catch
                        {
                            //TraceFactory.Logger.Debug("Exception: {0}".FormatWith(ex.Message));
                        }
                    }
                }

                if (null != tableHeaders)
                {
                    foreach (var columnHeader in tableHeaders)
                    {
                        // Ignore invalid data
                        if (string.IsNullOrEmpty(columnHeader.ToString().Trim()))
                        {
                            continue;
                        }

                        tableRow.Add(columnHeader.Text);
                    }
                }
               

                // Add entry only when row(header) is found
                if (0 != tableRow.Count)
                {
                    table.Add(tableRow);
                }
            }

            // Get all data rows in the table
            ReadOnlyCollection<IWebElement> rows = tableElement.FindElements(By.XPath("tbody/tr"));

            foreach (var row in rows)
            {
                // Ignore invalid data
                if (string.IsNullOrEmpty(row.Text.Trim()))
                {
                    continue;
                }

                ReadOnlyCollection<IWebElement> columns = row.FindElements(By.XPath("td"));
                tableRow = new Collection<string>();

                for (int i = 0; i < columns.Count; i++)
                {
                    string data = string.Empty;
                    IWebElement element = null;

                    if (null != columnIndex && columnIndex.Contains(i))
                    {
                        try
                        {
                            element = columns[i].FindElement(By.TagName("input"));
                        }
                        catch
                        {
                            // If the column is not a control, FindElement will throw up an exception
                            // Do nothing
                        }

                        // By default, return value attribute.
                        if (null != element)
                        {
                            if (returnValue)
                            {
                                data = element.GetAttribute("value");
                            }
                            else if (string.IsNullOrEmpty(data) && FindType.ById.Equals(elementType) && !string.IsNullOrEmpty(element.GetAttribute("id")))
                            {
                                data = element.GetAttribute("id");
                            }
                            else if (string.IsNullOrEmpty(data) && FindType.ByName.Equals(elementType) && !string.IsNullOrEmpty(element.GetAttribute("name")))
                            {
                                data = element.GetAttribute("name");
                            }
                            else if (string.IsNullOrEmpty(data) && FindType.ByClassName.Equals(elementType) && !string.IsNullOrEmpty(element.GetAttribute("class")))
                            {
                                data = element.GetAttribute("class");
                            }
                        }
                        else
                        {
                            data = columns[i].Text;
                        }
                    }
                    else
                    {
                        data = columns[i].Text;
                    }

                    tableRow.Add(data);
                }

                // Add only valid row data
                if (0 != tableRow.Count)
                {
                    table.Add(tableRow);
                }
            }

            return table;
        }

        /// <summary>
        /// Find if element is present in page
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="type"></param>
        /// <param name="retry">True to do a retry to find out the element.</param>
        /// <returns></returns>
        public bool IsElementPresent(string locator, FindType type, bool retry = true)
        {
            try
            {
                IWebElement element = FindElement(locator, type, retry);

                if (null != element)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Waits for the specified time.
        /// </summary>
        /// <param name="timeout">The time.</param>
        public static void Wait(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        /// <summary>
        /// Close the browser.
        /// </summary>
        public void Close()
        {
            _webDriver?.Close();
        }

        #endregion Public Methods

        #region Not Supported ISelenium methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public bool IsTextPresent(string pattern)
        {
            return _webDriver.PageSource.Contains(pattern);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public void WaitForPageToLoad(string timeout)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableCellAddress"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetTableCell(string tableCellAddress, int row, int column)
        {
            throw new NotImplementedException();
        }

        ///  <summary>
        ///
        ///  </summary>
        ///  <param name="locator">The locator.</param>

        /// <returns></returns>
        public void SetFocus(string locator)
        {
            IWebElement element = FindElement(locator, FindType.ByXPath);
            element.SendKeys("");
        }

        #endregion Not Supported ISelenium methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="dispose"></param>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                Stop();
                _webDriver?.Dispose();
                _webDriver = null;
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
        /// <param name="locator">The locator</param>
        /// <param name="type">Type of locator</param>
        /// <returns>List of IWebElements</returns>
        public IList<IWebElement> GetPageElements(string locator, FindType type)
        {
            IList<IWebElement> elements = null;

            if (FindType.ByXPath == type)
            {
                elements = _webDriver.FindElements(By.XPath(locator));
            }
            else if (FindType.ById == type)
            {
                elements = _webDriver.FindElements(By.Id(locator));
            }
            else if (FindType.ByName == type)
            {
                elements = _webDriver.FindElements(By.Name(locator));
            }

            return elements;
        }

        /// TODO: Look into this method for setting date in calendar control.
        ///<summary>
        /// Sets the date on the date control.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">Date time value to be set.</param>
        /// <param name="type">Type of locator.</param>
        public void SetDateControlText(string locator, DateTime value, FindType type)
        {
            IWebElement element = FindElement(locator, type);

            element?.Click();

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(40)).Until(ExpectedConditions.ElementIsVisible(By.Id("ui-datepicker-div")));
            IWebElement dateControl = _webDriver.FindElement(By.Id("ui-datepicker-div"));

            List<IWebElement> divElements = dateControl.FindElements(By.TagName("select")).ToList();
            divElements[0].SendKeys(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value.Month));
            Thread.Sleep(TimeSpan.FromSeconds(15));
            divElements[1].SendKeys(value.Year.ToString());

            List<IWebElement> datecolumns = dateControl.FindElements(By.TagName("td")).ToList();
            datecolumns.FirstOrDefault(x => x.Text.EqualsIgnoreCase(value.Day.ToString()))?.Click();

            //_webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(10));

            //IJavaScriptExecutor exec = _webDriver as IJavaScriptExecutor;

            //exec.ExecuteScript("$('#{0}').val('{1}');".FormatWith(locator, value.Date.ToString()), "");
        }

        /// <summary>
        /// Executes java script on a page/ Form.
		/// </summary>
		/// <param name="script">Java script to be executed.</param>
		public void ExecuteScript(string script)
        {
            _webDriver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(10));

            IJavaScriptExecutor exec = _webDriver as IJavaScriptExecutor;

            exec?.ExecuteScript(script, "");
        }

        /// <summary>
        /// Send Tab on a control.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="type">Type of locator.</param>
        public void SendTab(string locator, FindType type)
        {
            IWebElement element = FindElement(locator, type);
            if (null != element)
            {
                element.SendKeys(Keys.Tab);

                // wait for specified time before returning the control back
                Thread.Sleep(ElementOperationDelay);
            }
        }

        /// <summary>
        /// Captures the screen shot for the current page.
        /// </summary>
        /// <param name="filePath">The file path to save the file</param>
        public void CaptureScreenShot(string filePath)
        {
            Screenshot shot = ((ITakesScreenshot)_webDriver).GetScreenshot();
            shot.SaveAsFile(filePath, ImageFormat.Jpeg);
        }
    }
}