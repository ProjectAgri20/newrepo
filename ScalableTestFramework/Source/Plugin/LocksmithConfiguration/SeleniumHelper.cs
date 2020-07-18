using HP.ScalableTest.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    /// <summary>
    /// Helper class that implments methods for Selenium operations 
    /// </summary>
    public class SeleniumHelper
    {
        private const int ElementWaitDelay = 30;

        /// <summary>
        /// Checks if an element exists in specified locator <see cref="ElementExists"/> class.
        /// </summary>
        /// <param name="elementSelector">Specified the element to be checked </param>
        /// <param name="driver"> <see cref="IWebDriver"/> The browser or Web driver element executing Selnium </param>
        /// <returns></returns>
        public bool ElementExists(By elementSelector, IWebDriver driver)
        {
            try
            {
                Thread.Sleep(2000);
                driver.FindElement(elementSelector);
                return true;
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return false;
                throw new LocksmithConfigurationException("Failed as the element searched does not exist.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed while checking if the element exists.");
            }

        }

        /// <summary>
        /// Waits until an element is clickable and clicks <see cref="WaitUntilClickableAndClick"/> class. 
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium </param>
        /// <param name="elementSelector">Specifies the element which needs to be checked for clickable</param>
        /// <returns></returns>
        public void WaitUntilClickableAndClick(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elementSelector));
                element.Click();
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed while waiting for the element to  be clickable and click as the element is not found.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed while waiting for the element to  be clickable and click as the process did not complete within time out(30 seconds).");
            }
        }

        /// <summary>
        /// Moves the cursor to an element <see cref="MoveToElement"/> class. 
        /// </summary>
        /// <param name="driver"><see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element which needs to perform operation</param>
        /// <returns></returns>
        public void MoveToElement(IWebDriver driver, By elementSelector)
        {
            Actions mouseMove;
            IWebElement element;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                element = wait.Until<IWebElement>(d => d.FindElement(elementSelector));
                mouseMove = new Actions(driver);
                mouseMove.MoveToElement(element).Perform();
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to move to element as the element is not found.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to move to an element.");
            }
        }

        /// <summary>
        /// Scrolls to an element <see cref="ScrollToElement"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the Element to which it is to scrolled </param>
        /// <returns></returns>
        public void ScrollToElement(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            try
            {
                driver.SwitchTo().ActiveElement();
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                //Identify the WebElement which will appear after scrolling down
                element = driver.FindElement(elementSelector);
                // now execute query which actually will scroll until that element is not appeared on page.
                je.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed as the scroll element does not exist.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to scroll to an element.");
            }

        }

        /// <summary>
        /// Waits until an element is clickable <see cref="WaitUntilClickable"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver executing Selnium</param>
        /// <param name="elementSelector">Specifies the element for which to wait until it is clickable</param>
        /// <returns></returns>
        public void WaitUntilClickable(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(elementSelector));
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed while wait for the element to be clickable as element does not exist.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Wait for the element to click process did not complete within time out(30 seconds).");
            }
        }

        /// <summary>
        /// Waits until an element is visible <see cref="WaitUntilVisible"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to wait until it is visible</param>
        /// <returns></returns>
        public void WaitUntilVisible(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(elementSelector));
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Wait for the element process did not complete as the element does not exist.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Wait for the element process did not complete within timeout.");
            }
        }

        /// <summary>
        /// Waits until an element exists <see cref="WaitUntilExists"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to wait for until it is visible</param>
        /// <returns></returns>
        public void WaitUntilExists(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(elementSelector));
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Wait for the element process did not complete as the element does not exist.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Wait for the element process did not complete within timeout.");
            }
        }

        /// <summary>
        /// Finds an element and clicks <see cref="FindElementandClick"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to be found and then clicks it</param>
        /// <returns></returns>
        public void FindElementandClick(IWebDriver driver, By elementSelector)
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(elementSelector);
                element.Click();
            }
            catch (NoSuchElementException exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to click on an element.");
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to click on an element.");
            }
        }

        /// <summary>
        /// Passes the data using keystrokes <see cref="PassData"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element for which data needs to be passed</param>
        /// <param name="data">Specifies the data </param>
        /// <returns></returns>
        public void PassData(IWebDriver driver, By elementSelector, string data)
        {
            try
            {
                driver.FindElement(elementSelector).SendKeys(data);
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Failed to Pass the data.");
            }
        }

        /// <summary>
        /// Waits until an element is invisible <see cref="WaitUntilInvisible"/> class.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to wait for until it is visible</param>
        /// <returns></returns>
        public void WaitUntilInvisible(IWebDriver driver, By elementSelector)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(elementSelector));

            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Process did not complete within timeout (30 seconds).");
            }
        }

        /// <summary>
        /// Clears the contents of the specified selenium web element.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to clear the contents for</param>
        /// <returns></returns>
        public void ClearContent(IWebDriver driver, By elementSelector)
        {
            try
            {
                driver.FindElement(elementSelector).Clear();

            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                throw new LocksmithConfigurationException("Error trying to clear the contents of the element specified.");
            }
        }

        /// <summary>
        /// Checks if the specified Selenium element is visible.
        /// </summary>
        /// <param name="driver"> <see cref="IWebDriver"/>The browser or Web driver element executing Selnium</param>
        /// <param name="elementSelector">Specifies the element to check for the visibility</param>
        /// <returns></returns>
        public bool IsVisible(IWebDriver driver, By elementSelector)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ElementWaitDelay));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(elementSelector));
                return true;
            }
            catch (Exception exception)
            {
                ExecutionServices.SystemTrace.LogError(exception.ToString());
                return false;
            }
        }     
    }
}
