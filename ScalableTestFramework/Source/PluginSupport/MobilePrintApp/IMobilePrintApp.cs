using System;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Represent Mobile print app
    /// </summary>
    public interface IMobilePrintApp : IDisposable
    {
        /// <summary>
        /// Launch Mobile Print App
        /// </summary>
        /// <returns>false if launch failed</returns>
        bool LaunchApp();

        /// <summary>
        /// Close app
        /// </summary>
        void CloseApp();

        /// <summary>
        /// Select printer to send job
        /// </summary>
        /// <param name="printerIdentifier">Printer identifier(printer name, id) to select</param>
        void SelectPrinter(string printerIdentifier);

        /// <summary>
        /// Select target to print (i.e. file, photo, web)
        /// </summary>
        /// <param name="target">Target to print</param>
        void SelectTargetToPrint(ObjectToPrint target);

        /// <summary>
        /// Start Print Job
        /// </summary>
        /// <exception cref="MobileWorkflowException">When something goes wrong.</exception>
        void Print();

        /// <summary>
        /// Set job option
        /// </summary>
        /// <param name="option">option to set</param>
        void SetOptions(MobilePrintJobOptions option);

        /// <summary>
        /// Wait until print done just after clicking Print button
        /// </summary>
        /// <param name="waitTime">Wait time</param>
        void WaitUntilPrintDone(TimeSpan waitTime);


        /// <summary>
        /// Check lastest print job status on mobile device
        /// </summary>
        /// <returns>true if success</returns>
        bool CheckPrintStatusOnMobile();


        /// <summary>
        /// Get App name
        /// </summary>
        /// <returns>Name of App</returns>
        string GetAppName();
    }
}
