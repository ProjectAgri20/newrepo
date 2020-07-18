using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using OpenQA.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.Selenium
{
    /// <summary>
    /// Enum for type of locator
    /// </summary>
    public enum FindType
    {
        /// <summary>
        /// locator type is name
        /// </summary>
        ByName,
        /// <summary>
        /// locator type is Id
        /// </summary>
        ById,
        /// <summary>
        /// locator type is Tagname
        /// </summary>
        ByTagName,
        /// <summary>
        /// locator type is XPath
        /// </summary>
        ByXPath,
        /// <summary>
        /// locator type is class name
        /// </summary>
        ByClassName,
        /// <summary>
        /// locator type is link text
        /// </summary>
        ByLinkText
    }

    /// <summary>
    /// This interface is used to run a Selenium based test
    /// </summary>
    public interface ISeleniumBase : IDisposable
    {
        /// <summary>
        /// Gets or sets the page navigation delay
        /// </summary>
        TimeSpan PageNavigationDelay { get; set; }

        /// <summary>
        /// Gets or sets the element operation delay 
        /// </summary>
        TimeSpan ElementOperationDelay { get; set; }

        /// <summary>
        /// Gets the title of the current page that is being displayed
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the body of the currently navigated HTML page
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Launches the browser.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="uri">The URI.</param>
        void Start(BrowserModel browser, Uri uri);

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        void Open(string uri);

        /// <summary>
        /// Opens the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        void Open(Uri uri);

        /// <summary>
        /// Clicks on a button/ radio-button/ check-box present in the page
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        void Click(string locator, FindType type);

        /// <summary>
        /// Clicks on link present in the page
        /// </summary>
        /// <param name="linkText">The link text.</param>
        void ClickonLink(string linkText);

        /// <summary>
        /// Clicks on OK button of popup window
        /// </summary>
        void ClickOkonAlert();

        /// <summary>
        /// Clicks on Cancel button of popup window
        /// </summary>
        void ClickCancelonAlert();

        /// <summary>
        /// Set the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">Type of locator</param>
        /// <param name="sendTab">True to send tab, else false.</param>
        void SetText(string locator, string value, FindType type, bool sendTab = false);

        /// <summary>
        /// Get the value of the text-box
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns></returns>
        string GetText(string locator, FindType type);

        /// <summary>
        /// Selects the item from a listbox, dropdown box based on text.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator</param>
        /// <param name="optionLocator">The option locator.</param>
        void Select(string selectLocator, string optionLocator, FindType type);

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on index.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="index">Index of list item</param>
        /// <param name="type">Type of locator</param>
        void Select(string selectLocator, int index, FindType type);

        /// <summary>
        /// Selects the list item from a listbox, dropdown box based on the value.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="value">The value to be selected.</param>
        /// <param name="type">Type of locator</param>
        void SelectByValue(string selectLocator, string value, FindType type);

        /// <summary>
        /// Clear all selected entries from a list box. Supported onlywhen the select element supports multiple selection.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator.</param>
        void DeselectAll(string selectLocator, FindType type);

        /// <summary>
        /// Get the list items from a listbox, dropdown box.
        /// </summary>
        /// <param name="selectLocator">The select locator.</param>
        /// <param name="type">Type of locator</param>        
        IEnumerable<string> GetListItems(string selectLocator, FindType type);

        /// <summary>
        /// Returns whether the specified checkbox/ radio button is checked or not
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns>
        /// true if checked, false otherwise
        /// </returns>
        bool IsChecked(string locator, FindType type);

        /// <summary>
        /// Navigate to specified Url
        /// </summary>
        /// <param name="uri">The URL.</param>
        void NavigateTo(string uri);

        /// <summary>
        /// Navigate to specified Url
        /// </summary>
        /// <param name="uri">The URI.</param>
        void NavigateTo(Uri uri);

        /// <summary>
        /// Search for the specified string/text in the current page
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>
        /// true if string found, false otherwise
        /// </returns>
        bool IsTextPresent(string pattern);

        /// <summary>
        /// Check a checkbox
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        void Check(string locator, FindType type);

        /// <summary>
        /// Unchecks the specified locator.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        void Uncheck(string locator, FindType type);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets the (whitespace-trimmed) value of an input field (or anything else with a value parameter).
        /// For checkbox/radio elements, the value will be "on" or "off" depending on
        /// whether the element is checked or not.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        /// <returns></returns>
        string GetValue(string locator, FindType type);

        /// <summary>
        /// Ends the test session, killing the browser
        /// </summary>
        void Close();

        /// <summary>
        /// Waits for a new page to load.
        /// </summary>
        /// <param name="timeout">a timeout in milliseconds, after which this command will return with an error</param>
        void WaitForPageToLoad(string timeout);

        /// <summary>
        /// Click on a button/ radio-button/ check-box present in the page using name of the control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="type">Type of locator</param>
        void ClickByName(string locator, FindType type);

        /// <summary>
        /// Gets the text from a cell of a table.
        /// </summary>
        /// <param name="tableCellAddress">table id</param>
        /// <param name="row">row value of the cell</param>
        /// <param name="column">column value of the cell</param>
        /// <returns>the text from the specified cell</returns>
        string GetTableCell(string tableCellAddress, int row, int column);

        /// <summary>
        /// Set the focus for control
        /// </summary>
        /// <param name="locator">The locator.</param> 
        void SetFocus(string locator);

        /// <summary>
        /// Set the value of the Browse Control
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">The value to be set in browse control.</param>
        /// <param name="type">Type of locator</param>
        void SetBrowseControlText(string locator, string value, FindType type);

        /// <summary>
        /// Selects one or more rows in a table for given column names
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnTexts">The text for consecutive columns</param>
        /// <param name="type">Type of locator</param>
        /// <returns>True if succeeded</returns>
        bool SelectTableRows(string locator, StringCollection columnTexts, FindType type);

        /// <summary>
        /// Get data of all the rows whose first text column is of 'columnText'
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="columnText">The text for first text column in the table row</param>
        /// <param name="type">Type of locator</param>
        /// <returns>Collection of a list of rows</returns>
        IEnumerable<StringCollection> GetTableRowsData(string locator, String columnText, FindType type);

        /// <summary>
        /// Get Table Rows
        /// </summary>
        /// <param name="locator">The locator</param>        
        /// <param name="type">Type of locator</param>
        /// <returns>Collection of a list of rows</returns>
        string GetTableRows(string locator, string searchText, FindType type);

        /// <summary>
        /// Get Table Details
        /// </summary>
        /// <param name="locator">The locator</param>        
        /// <param name="type">Type of locator</param>
        /// <param name="includeHeader">true to include header details in collection, false otherwise</param>
        /// <param name="columnIndex">Column index for those element id/ name to be returned</param>
        /// <param name="elementType">Element type <see cref=" FindType"/>. If Id is not available, name will be returned</param>
        /// <param name="returnValue">Return Element value by default, if <paramref name="elementType"/> is mentioned, use id/ name/ class</param>
        /// <returns>Collection of Table details</returns>
        Collection<Collection<string>> GetTable(string locator, FindType type, bool includeHeader = true, int[] columnIndex = null, FindType elementType = FindType.ById, bool returnValue = true);

        /// <summary>
        /// Check if element is present on current page
        /// </summary>
        /// <param name="locator">The locator</param>        
        /// <param name="type">Type of locator</param>
        /// <param name="retry">True to do a retry to find out the element.</param>
        /// <returns>true if element found, false otherwise</returns>
        bool IsElementPresent(string locator, FindType type, bool retry = true);

        /// <summary>
        /// Get IWebElements of a html page
        /// </summary>
        /// <param name="locator">The locator</param>
        /// <param name="type">Type of locator</param>
        /// <returns>List of IWebElements</returns>
        IList<IWebElement> GetPageElements(string locator, FindType type);

        /// <summary>
        /// Sets the date on the date control.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <param name="value">Date time value to be set.</param>
        /// <param name="type">Type of locator.</param>
        void SetDateControlText(string locator, DateTime value, FindType type);

        /// <summary>
        /// Executes java script on a page/ Form. 
        /// </summary>
        /// <param name="script">Java script to be executed.</param>
        void ExecuteScript(string script);

        /// <summary>
        /// Send Tab on a control.
        /// </summary>
        /// <param name="locator"><The locator.s/param>
        /// <param name="type">Type of locator.</param>
        void SendTab(string locator, FindType type);

        /// <summary>
        /// Captures the screen shot for the current page.
        /// </summary>
        /// <param name="filePath">The file path to save the file</param>
        void CaptureScreenShot(string filePath);
    }
}
