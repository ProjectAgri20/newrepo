using System;
using System.Collections.Generic;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes
{
    /// <summary>
    /// GeniusBytesAppBase class.
    /// </summary>
    public abstract class GeniusBytesAppBase : DeviceWorkflowLogSource, IGeniusBytesApp
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;

        /// <summary>
        /// Updates the status area information 
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusMessageUpdate;

        /// <summary>
        /// Gets or sets a value indicating whether [use image p review].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use image p review]; otherwise, <c>false</c>.
        /// </value>
        public bool UseImagePreview { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesAppBase" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        protected GeniusBytesAppBase(IJavaScriptExecutor controlPanel)
        {
            _engine = new OxpdBrowserEngine(controlPanel, GeniusBytesResource.GeniusBytesJavaScript);
        }

        /// <summary>
        /// Press the PrintAll
        /// </summary>
        public void PrintAll()
        {
            try
            {
                PressElementIDbyTextContains("tr", "Print All");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print All\" button");
            }
        }

        /// <summary>
        /// Press the PullPrinting(X)
        /// </summary>
        public void PullPrinting()
        {
            try
            {
                PressElementIDbyTextContains("tr", "Pull Printing(");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Pull Printing( )\" button");
            }

        }

        /// <summary>
        /// Press the ScanToHome
        /// </summary>
        public void PressAppName(string appname)
        {
            try
            {
                PressElementIDbyText("td", appname);
            }
            catch
            {
                throw new DeviceWorkflowException($"Fail to press {appname} button");
            }
        }

        /// <summary>
        /// Press the Print All and Delete
        /// </summary>
        public void PrintAllandDelete()
        {
            try
            {
                _engine.WaitForHtmlContains("Print All and Delete", TimeSpan.FromSeconds(2));
                PressElementIDbyText("td", "Print All and Delete");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print All and Delete\" button");
            }
        }

        /// <summary>
        /// Press the Print button
        /// </summary>
        public void Print()
        {
            try
            {
                _engine.WaitForHtmlContains(", selected 1", TimeSpan.FromSeconds(2));
                PressElementIDbyText("td", "Print");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print\" button");
            }
        }

        /// <summary>
        /// Press the PrintandDelete button
        /// </summary>
        public void PrintandDelete()
        {
            try
            {
                _engine.WaitForHtmlContains("Print and Delete", TimeSpan.FromSeconds(2));
                PressElementIDbyText("td", "Print and Delete");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print and Delete\" button");
            }
        }

        /// <summary>
        /// Press the Delete button
        /// </summary>
        public void Delete()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_engine.WaitForHtmlContains("Other", TimeSpan.FromMilliseconds(500)))
                    {
                        Thread.Sleep(1000);
                        PressElementIDbyText("td", "Other");
                        Thread.Sleep(2000);
                        PressElementIDbyText("td", "Delete");
                        Thread.Sleep(2000);
                        return;
                    }
                    else if (_engine.WaitForHtmlContains(">Delete<", TimeSpan.FromMilliseconds(500)))
                    {
                        Thread.Sleep(1000);
                        PressElementIDbyText("td", "Delete");
                        Thread.Sleep(2000);
                        return;
                    }
                }
                throw new DeviceWorkflowException("Can not find 'Other' or 'Delete' button");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Delete\" button");
            }

        }

        /// <summary>
        /// Press the Delete All button
        /// </summary>
        public void DeleteAll()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (_engine.WaitForHtmlContains("Other", TimeSpan.FromMilliseconds(500)))
                    {
                        Thread.Sleep(1000);
                        PressElementIDbyText("td", "Other");
                        Thread.Sleep(2000);
                        PressElementIDbyText("td", "Delete All");
                        Thread.Sleep(2000);
                        return;
                    }
                    else if (_engine.WaitForHtmlContains("Delete All", TimeSpan.FromMilliseconds(500)))
                    {
                        Thread.Sleep(1000);
                        PressElementIDbyText("td", "Delete All");
                        Thread.Sleep(2000);
                        return;
                    }
                }
                throw new DeviceWorkflowException("Can not find 'Other' or 'Delete All' button");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Delete All\" button");
            }
        }

        /// <summary>
        /// Un-selects all documents except the first.
        /// </summary>
        public virtual void SelectFirstDocument()
        {
            try
            {
                PressFirstTimeFormat("td");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press first document");
            }
        }

        /// <summary>
        /// Sets the email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <exception cref="DeviceWorkflowException">Unable to press the 'Add manually' email button.</exception>
        public abstract void SetEmailAddress(string emailAddress);


        /// <summary>
        /// Selects the first email.
        /// </summary>
        public virtual void SelectFirstEmail()
        {
            try
            {
                WaitObjectForAvailable("@", TimeSpan.FromSeconds(30));
                PressElementIDbyTextContains("td", "@");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to select first email address");
            }
        }

        /// <summary>
        /// Add Document
        /// </summary>
        public virtual void AddDocument(TimeSpan timeOut)
        {
            string elementText = "Click here to add Document";
            if (!WaitForElementReady(elementText, timeOut))
            {
                throw new DeviceWorkflowException($"'{elementText}' element was not available within {timeOut.TotalSeconds} seconds.");

            }
            PressElementIDbyText("td", elementText);
        }

        /// <summary>
        /// Add Document for OCR
        /// </summary>
        public virtual void AddDocumentforOCR()
        {
            try
            {
                PressElementIDbyText("td", "Click here to add document");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Click here to add document\"");
            }
        }

        /// <summary>
        /// Add Email
        /// </summary>
        public virtual void AddEmail()
        {
            if (WaitForElementReady("Click here to add Email address", TimeSpan.FromSeconds(5)))
            {
                PressElementIDbyText("td", "Click here to add Email address");
            }
            else
            {
                throw new DeviceWorkflowException("Fail to press \"Click here to add Email address\"");
            }
        }

        /// <summary>
        /// Confirm Email
        /// </summary>
        public virtual void ConfirmEmail()
        {
            try
            {
                WaitObjectForAvailable("Confirm", TimeSpan.FromSeconds(30));
                PressElementIDbyText("td", "Confirm");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Confirm\"");
            }
        }

        /// <summary>
        /// Select Color Mode as Print
        /// </summary>
        /// <returns></returns>
        public void SetColorModeAsPrint()
        {
            try
            {
                PressElementIDbyTextandZIndex("td", "Print", 62);
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print\" button");
            }

        }

        /// <summary>
        /// Select Color Mode as Print BW
        /// </summary>
        /// <returns></returns>
        public void SetColorModeAsPrintBW()
        {
            try
            {
                PressElementIDbyTextContains("td", "Print B");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Print BW\" button");
            }

        }

        /// <summary>
        /// Sets the enabled disabled.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="DeviceWorkflowException">Fail to press the provided button text</exception>
        public void SetEnabledDisabled(string value)
        {
            try
            {
                WaitForElementReady("td", value, 81);
                PressElementIDbyTextandZIndex("td", value, 81);
                if (!Wait.ForTrue(() => ExistElementText("Start Scan"), TimeSpan.FromSeconds(10)))
                {
                    throw new DeviceWorkflowException($"Popup menu [Enable-Disable] failed to leave within 10 seconds.");
                }
            }
            catch (DeviceWorkflowException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException($"Failed to press '{value}' button");
            }
        }

        /// <summary>
        /// Press Back key
        /// </summary>
        /// <returns></returns>
        public void PressBackKey()
        {
            try
            {
                PressElementIDbyImageName("div", "contextView-back");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press back key");
            }
        }

        /// <summary>
        /// Press OK key
        /// </summary>
        /// <returns></returns>
        public void PressOKKey()
        {
            try
            {
                PressElementIDbyText("button", "OK");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press ok button");
            }
        }

        /// <summary>
        /// Press Close Key
        /// </summary>
        /// <returns></returns>
        public void PressCloseKey()
        {
            try
            {
                PressElementLastIDbyText("td", "Close");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press close button");
            }
        }

        /// <summary>
        /// Press Cancel Key.
        /// </summary>
        public void PressCancelKey()
        {
            try
            {
                PressElementLastIDbyText("td", "Cancel");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press cancel button");
            }
        }

        /// <summary>
        /// Press Confirm button
        /// </summary>
        /// <returns></returns>
        public void Confirm()
        {
            try
            {
                PressElementIDbyText("td", "Confirm");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press Confirm button");
            }
        }

        /// <summary>
        /// Press Confirm Button on the popup
        /// </summary>
        /// <returns></returns>
        public void PressConfirmonPopup()
        {
            try
            {
                if (WaitObjectForAvailable("Confirm", TimeSpan.FromSeconds(2)))
                {
                    PressElementLastIDbyText("td", "Confirm");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press Confirm button on the Popup");
            }
        }

        /// <summary>
        /// Press Print Button on the popup
        /// </summary>
        /// <returns></returns>
        public void PressPrintonPopup()
        {
            try
            {
                if (WaitObjectForAvailable("Print B", TimeSpan.FromSeconds(5)))
                {
                    PressElementLastIDbyText("td", "Print");
                }
                else
                {
                    throw new DeviceWorkflowException("Fail to find Color mode popup");
                }
            }
            catch (DeviceWorkflowException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press Print button on the Popup");
            }
        }

        /// <summary>
        /// Sign Out for Genius Bytes
        /// </summary>
        /// <returns></returns>
        public void SignOut()
        {
            try
            {
                WaitForElementReady("mainmenu-logout-normal", TimeSpan.FromSeconds(20));
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                PressElementIDbyImageName("div", "mainmenu-logout-normal");
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press Sign Out button");
            }
        }

        /// <summary>
        /// Scroll To the page that has the given element name
        /// </summary>
        /// <param name="elementText">Name of the element.</param>
        /// <param name="isDialog">if set to <c>true</c> [is dialog].</param>
        /// <returns>
        /// true if able to scroll to the page with the given element name
        /// </returns>
        public bool ScrollToObject(string elementText, bool isDialog)
        {
            bool found = true;
            string arrowName = (isDialog == true) ? "dialog-arrow-down-normal" : "arrow-down-normal";

            while (!ExistElementText(elementText))
            {
                try
                {
                    ArrowDown(arrowName);
                }
                catch
                {
                    if (!ExistElementText(elementText))
                    {
                        found = false;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// Scrolls to object.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="elementText">The element text.</param>
        /// <param name="zIndex">Index of the z.</param>
        /// <returns></returns>
        protected bool ScrollToObject(string tag, string elementText, int zIndex)
        {
            bool found = true;
            while (!ExistElementText(tag, elementText, zIndex))
            {
                try
                {
                    ArrowDown("arrow-down-normal");
                }
                catch
                {
                    found = false;
                }
            }
            return found;
        }

        private bool ExistElementText(string tag, string elementText, int zIndex)
        {
            string existId = _engine.ExecuteFunction("getElementIdbyTextandZIndex", tag, elementText, zIndex).Trim('"');
            return !existId.Equals("undefined");
        }

        /// <summary>
        /// Checks if the given element ID exists on the current page 
        /// </summary>
        /// <param name="elementText">Name of the element.</param>
        /// <return>true if exists</return>
        public bool ExistElementText(string elementText)
        {
            bool exist = false;

            string existId = _engine.ExecuteFunction("getElementIdbyTextContains", "div", elementText).Trim('"');

            exist = (existId.Equals("undefined")) ? false : true;

            if (exist && elementText.Contains("Scan2"))
            {
                string value = string.Empty;
                try
                {
                    // Calling into the OXPD browser too quick back to back can cause unexpected results...
                    Thread.Sleep(250);
                    value = _engine.ExecuteJavaScript($"document.getElementById('{existId}').textContent");
                    if (value.Contains("Native"))
                    {
                        exist = false;
                    }
                }
                catch
                {
                    value = string.Empty;
                }
            }

            return exist;
        }

        private bool ExistObjectId(string objectId)
        {
            bool exist = false;
            string existId = _engine.ExecuteFunction("ExistElementId", objectId);

            exist = (existId.Equals("true")) ? true : false;

            return exist;
        }

        /// <summary>
        /// Press Down-Arrow Button
        /// </summary>
        /// <param name="arrowName"></param>
        /// <exception cref="DeviceWorkflowException">Fail to press \"Scroll Down\" button</exception>
        public void ArrowDown(string arrowName)
        {
            try
            {
                WaitForNotBlocked();
                PressElementIDbyImageName("div", arrowName);
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Fail to press \"Scroll Down\" button");
            }
        }
        /// <summary>
        /// Waits for the outer border of the body to have a display of not blocked.
        /// </summary>
        protected void WaitForNotBlocked()
        {
            string displayLocked = "block";
            string currentState = _engine.ExecuteFunction("getElementDisplayValue", "lock").Trim('"');
            while (currentState.Equals(displayLocked))
            {
                Thread.Sleep(250);
                currentState = _engine.ExecuteFunction("getElementDisplayValue", "lock");
            }
        }

        /// <summary>
        /// Waits for the given time for the element to become ready for use.
        /// </summary>
        /// <param name="element">The element text.</param>
        /// <param name="time">time to wait.</param>
        /// <returns>
        /// true if usable
        /// </returns>
        public bool WaitForElementReady(string element, TimeSpan time)
        {
            return Wait.ForTrue(() => ExistElementText(element), time, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Waits for element ready.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="elementText">The element text.</param>
        /// <param name="zIndex">Index of the z.</param>
        /// <exception cref="DeviceWorkflowException"></exception>
        protected void WaitForElementReady(string tag, string elementText, int zIndex)
        {
            DateTime startDt = DateTime.Now.AddSeconds(10);
            string existId = _engine.ExecuteFunction("getElementIdbyTextandZIndex", tag, elementText, zIndex).Trim('"');
            while (existId.Equals("undefined") && startDt > DateTime.Now)
            {
                Thread.Sleep(250);
                existId = _engine.ExecuteFunction("getElementIdbyTextandZIndex", tag, elementText, zIndex).Trim('"');
            }
            if (existId.Equals("undefined"))
            {
                throw new DeviceWorkflowException($"Unable to find element with text value of {elementText} within 10 seconds.");
            }
        }

        /// <summary>
        /// Waits for object to be available up to the given time
        /// </summary>
        /// <param name="objectId">The object identifier.</param>
        /// <param name="time">The time.</param>
        /// <returns>
        /// true if available
        /// </returns>
        public bool WaitForObjectAvailable(string objectId, TimeSpan time)
        {
            return Wait.ForTrue(() => ExistObjectId(objectId), time, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Verify Home for Genius Bytes
        /// </summary>
        /// <returns></returns>
        public bool VerifySignOut()
        {
            bool success = false;

            if (WaitForElementReady("Manual Login", TimeSpan.FromSeconds(10)))
            {
                success = true;
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            }

            return success;
        }

        /// <summary>
        /// Press Element by Text
        /// </summary>
        protected void PressElementIDbyText(string tag, string text)
        {
            TimeSpan interval = TimeSpan.FromSeconds(3);
            TimeSpan timeOut = TimeSpan.FromSeconds(6);

            Func<bool> action = new Func<bool>(() =>
            {
                try
                {
                    string existId = _engine.ExecuteFunction("getElementIdbyText", tag, text).Trim('"');
                    _engine.PressElementById(existId);
                    return true;
                }
                catch (JavaScriptExecutionException)
                {
                    return false;
                }
            });

            try
            {
                if (!Wait.ForTrue(action, timeOut, interval))
                {
                    throw new DeviceWorkflowException($"Did not find '{text}' element within {timeOut.TotalSeconds} seconds.");
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Did not find '{text}' element within {timeOut.TotalSeconds} seconds.", ex);
            }
        }

        /// <summary>
        /// Press Last Element by Text.
        /// </summary>
        private void PressElementLastIDbyText(string tag, string text)
        {
            TimeSpan interval = TimeSpan.FromSeconds(3);
            TimeSpan timeOut = TimeSpan.FromSeconds(6);

            Func<bool> action = new Func<bool>(() =>
            {
                try
                {
                    string existId = _engine.ExecuteFunction("getElementLastIdbyText", tag, text).Trim('"');
                    _engine.PressElementById(existId);
                    return true;
                }
                catch (JavaScriptExecutionException)
                {
                    return false;
                }
            });

            try
            {
                if (!Wait.ForTrue(action, timeOut, interval))
                {
                    throw new DeviceWorkflowException($"Did not find last {text} within {timeOut.TotalSeconds} seconds.");
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Did not find last {text} within {timeOut.TotalSeconds} seconds.", ex);
            }
        }

        /// <summary>
        /// Press Element by Text Contains
        /// </summary>
        protected void PressElementIDbyTextContains(string tag, string text)//tr
        {
            string existId = "";
            try
            {
                if (_engine.WaitForHtmlContains("Close", TimeSpan.FromSeconds(10)))
                {
                    PressCloseKey();
                }

                existId = _engine.ExecuteFunction("getElementIdbyTextContains", tag, text).Trim('"');
                _engine.PressElementById(existId);
                WaitForNotBlocked();
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getElementIdbyTextContains' at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                existId = _engine.ExecuteFunction("getElementIdbyTextContains", tag, text).Trim('"');
                _engine.PressElementById(existId);
            }
        }

        /// <summary>
        /// Press Element include the first element
        /// </summary>
        protected void PressFirstTimeFormat(string tag)
        {
            string existId = "";
            try
            {
                existId = _engine.ExecuteFunction("getElementIdofFirstTimeFormat", tag).Trim('"');
                _engine.PressElementById(existId);
                WaitForNotBlocked();
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getElementIdofFirstTimeFormat' at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                existId = _engine.ExecuteFunction("getElementIdofFirstTimeFormat", tag).Trim('"');
                _engine.PressElementById(existId);
            }
        }

        /// <summary>
        /// Press Element by Z Index
        /// </summary>
        private void PressElementIDbyZIndex(string tag, int zindex)//div
        {
            string existId = "";
            try
            {
                existId = _engine.ExecuteFunction("getElementIDbyZIndex", tag, zindex).Trim('"');
                _engine.PressElementById(existId);
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getElementIDbyZIndex' at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                existId = _engine.ExecuteFunction("getElementIDbyZIndex", tag, zindex).Trim('"');
                _engine.PressElementById(existId);
            }
        }

        /// <summary>
        /// Press Element by Z Index
        /// </summary>
        private void PressElementIDbyImageName(string tag, string imagename)
        {
            string existId = "";
            try
            {
                existId = _engine.ExecuteFunction("getElementIDbyImageName", tag, imagename).Trim('"');
                _engine.PressElementById(existId);
                WaitForNotBlocked();
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getElementIDbyImageName' at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                existId = _engine.ExecuteFunction("getElementIDbyImageName", tag, imagename).Trim('"');
                _engine.PressElementById(existId);
            }
        }

        /// <summary>
        /// Press Element ID by Text and ZIndex
        /// </summary>
        protected void PressElementIDbyTextandZIndex(string tag, string text, int zindex)
        {
            string existId = "";
            try
            {
                existId = _engine.ExecuteFunction("getElementIdbyTextandZIndex", tag, text, zindex).Trim('"');
                _engine.PressElementById(existId);
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getElementIdbyTextandZIndex' at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                existId = _engine.ExecuteFunction("getElementIdbyTextandZIndex", tag, text, zindex).Trim('"');
                _engine.PressElementById(existId);
            }
        }

        /// <summary>
        /// GetDocumentIds
        /// </summary>
        public List<string> GetDocumentIds()
        {
            List<string> result = new List<string>();

            char[] separator = { '$' };

            string idString = string.Empty;

            try
            {
                idString = _engine.ExecuteFunction("getElementIDbyZIndex", "div", 32).Trim('"', '$');
            }
            catch (JavaScriptExecutionException)
            {
                Framework.Logger.LogDebug("Retrying JavaScript 'getDocumentIds at " + DateTime.Now.ToString());
                Thread.Sleep(3000);
                idString = _engine.ExecuteFunction("getElementIDbyZIndex", "div", 32).Trim('"', '$');
            }
            if (idString.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] items = idString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Press Inner HTML by Text Contains
        /// </summary>
        public int GetDocumentsCount()
        {
            if (!WaitForElementReady("Print All", TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Fail to get home page.");
            }
            int documentsCount = 0;

            string testtext = _engine.ExecuteFunction("getInnerHTMLbyTextContainsforDocumentCount", "td", "Pull Printing(").Trim('"');
            testtext = testtext.Substring(testtext.LastIndexOf('(') + 1);
            testtext = testtext.Trim(')');

            if (!Int32.TryParse(testtext, out documentsCount))
            {
                throw new DeviceWorkflowException("Fail to get the documents count");
            }
            return documentsCount;
        }

        /// <summary>
        /// Run Click the option value for each options.
        /// </summary>
        /// <param name="value">Option Value.</param>
        public void SelectOptionValue(string value)
        {
            int count = 0;
            while (count < 5)
            {
                if (WaitObjectForAvailable(value.Replace("&", "&amp;"), TimeSpan.FromSeconds(5)))
                {
                    PressElementIDbyText("td", $"{value}");
                    break;
                }
                PressElementIDbyImageName("div", "dialog-arrow-down-normal");
                count++;
            }
            return;
        }
        /// <summary>
        /// Sets the image preview.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void SetImagePreview(bool enabled);
        /// <summary>
        /// Set FileType
        /// </summary>
        public abstract void SetFileType(GeniusByteScanFileTypeOption fileType, TimeSpan timeOut);

        /// <summary>
        /// Set FileName
        /// </summary>
        public abstract void SetFileName(string filename, GeniusBytesScanType scanType);

        /// <summary>
        /// Set color options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        public abstract void SelectColorOption(GeniusByteScanColorOption value, TimeSpan timeOut);

        /// <summary>
        /// Set sides options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        public abstract void SelectSidesOption(GeniusByteScanSidesOption value, TimeSpan timeOut);

        /// <summary>
        /// Set resolution options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        public abstract void SelectResolutionOption(GeniusByteScanResolutionOption value, TimeSpan timeOut);

        /// <summary>
        /// Start scan
        /// </summary>
        public abstract void StartScan(string sides, int pageCount);

        /// <summary>
        /// Launches GeniusBytes with the specified authenticator using given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        public abstract void Launch(IAuthenticator authenticator);

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator"></param>
        public abstract bool SignInReleaseAll(IAuthenticator authenticator);

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public abstract bool BannerErrorState();

        /// <summary>
        /// Wait specific value for available
        /// </summary>
        /// <param name="value">Value for waiting</param>
        /// <param name="time">Waiting time</param>
        /// <returns></returns>
        public bool WaitObjectForAvailable(string value, TimeSpan time)
        {
            return _engine.WaitForHtmlContains(value, time);
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public abstract bool StartedProcessingWork(TimeSpan ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public abstract bool FinishedProcessingWork();


        /// <summary>
        /// Updates the status area with job / solution information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        protected void UpdateStatus(string message, params object[] args)
        {
            var formattedMessage = string.Format(message, args);
            UpdateStatus(formattedMessage);
        }

        /// <summary>
        /// Updates the status area with job / solution information.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void UpdateStatus(string message)
        {
            OnStatusMessageChanged(this, new StatusChangedEventArgs(message));
        }

        /// <summary>
        /// Called when [status message changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
        private void OnStatusMessageChanged(object sender, StatusChangedEventArgs e)
        {
            StatusMessageUpdate?.Invoke(sender, e);
        }

    }
}
