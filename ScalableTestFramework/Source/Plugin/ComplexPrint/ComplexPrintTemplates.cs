using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Print.Monitor;
using HP.ScalableTest.WindowsAutomation;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    /// <summary>
    /// Templates for Print
    /// </summary>
    internal class ComplexPrintTemplates
    {
        #region Local Variable

        ComplexPrintActivityData _activityData;
        string PRINTER_ERROR_WINDOW_TITLE = "Printer Error";
        bool _abort = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to load <see cref=" ComplexPrintActivityData"/>
        /// </summary>
        /// <param name="activityData"><see cref=" ComplexPrintActivityData"/></param>
        public ComplexPrintTemplates(ComplexPrintActivityData activityData)
        {
            _activityData = activityData;
        }

        #endregion

        #region Templates

        /// <summary>
        /// Print all files in specified folder
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="printProtocol"><see cref=" Printer.Printer.PrintProtocol"/></param>
        /// <param name="portNo">Port number for installation</param>
        /// <param name="testNo">Test number</param>        
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool ComplexPrinting(string ipAddress, Printer.Printer.PrintProtocol printProtocol, int portNo, int testNo)
        {
            string[] files = Directory.GetFiles(_activityData.DocumentsPath);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Complex Printing failed. No files available.");
                return false;
            }

            string hostname = string.Empty;

            if (Printer.Printer.PrintProtocol.IPPS.Equals(printProtocol))
            {
                if (!CtcUtility.IPPS_Prerequisite(IPAddress.Parse(ipAddress), out hostname))
                {
                    TraceFactory.Logger.Info("Failed to perform pre-requisites for IPPS.");
                    return false;
                }
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.Install(IPAddress.Parse(ipAddress), printProtocol, _activityData.DriverPackagePath, _activityData.DriverModel, portNo, hostname))
            {
                printer.Print(CtcUtility.CreateFile("Test {0} started.".FormatWith(testNo)));
                // Subscribing for Print Queue Event
                printer.PrintQueueError += printer_PrintQueueError;

                if (!printer.Print(files))
                {
                    TraceFactory.Logger.Info("Complex Printing failed. All jobs didn't print.");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("Complex Printing failed.");
                return false;
            }

            TraceFactory.Logger.Info("All jobs for Complex Printing printed successfully.");
            return true;
        }

        /// <summary>
        /// Print all files in specified folder
        /// </summary>
        /// <param name="ipAddress">IP Address of Printer</param>
        /// <param name="testNo">Test number</param>        
        /// <param name="isPassiveMode">Passive/ Active mode for FTP</param>
        /// <returns>true if all files printed successfully, false otherwise</returns>
        public bool ComplexPrinting(string ipAddress, int testNo, bool isPassiveMode = false)
        {
            string[] files = Directory.GetFiles(_activityData.DocumentsPath);
            if (null == files || 0 == files.Length)
            {
                TraceFactory.Logger.Info("Complex Printing failed. No files available.");
                return false;
            }

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(_activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(ipAddress));

            if (printer.PrintWithFtp(IPAddress.Parse(ipAddress), string.Empty, string.Empty, files, isPassiveMode: isPassiveMode))
            {
                TraceFactory.Logger.Info("All jobs for Complex Printing printed successfully.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Complex Printing failed. All jobs didn't print successfully.");
                return false;
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Notify user on PrintQueue error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printer_PrintQueueError(object sender, PrintJobDataEventArgs e)
        {
            // Check added for object type
            if (null != sender)
            {
                Printer.Printer printer = sender as Printer.Printer;
                Thread confirmationMsgBoxThread = new Thread(new ThreadStart(CheckUserConfirmation));
                TimeSpan timeOut = new TimeSpan(0, 30, 0);

                confirmationMsgBoxThread.Start();
                DateTime startTime = DateTime.Now;

                do
                {
                    // Case 1: Wait for timeOut and kill the thread if no response from the user
                    if (startTime.Add(timeOut) <= DateTime.Now)
                    {
                        // Used to notify that PrintQueue error was not cleared
                        _abort = true;

                        KillMessageBox(confirmationMsgBoxThread);
                        return;
                    }

                    // Wait from 30 seconds and check the status of PrintQueue
                    Thread.Sleep(TimeSpan.FromSeconds(30));

                    // Case 2: If PrintQueue error is rectified from user or if PrintQueue recovers from error, kill message box
                    if (!printer.IsPrintQueueInError)
                    {
                        KillMessageBox(confirmationMsgBoxThread);
                        return;
                    }

                } while (!_abort);

                // If user has clicked Cancel button, abort the thread
                if (!_abort)
                {
                    confirmationMsgBoxThread.Abort();
                }
            }
        }

        /// <summary>
        /// Get user input on PrintQueue error
        /// </summary>
        private void CheckUserConfirmation()
        {
            DialogResult result = MessageBox.Show("Make sure Printer is in Ready state without any warnings.\nSelect one of the options below: \n 1. Retry to try once again.\n 2. Cancel to Fail the current test.\n ", PRINTER_ERROR_WINDOW_TITLE, System.Windows.Forms.MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

            if (result == DialogResult.Retry)
            {
                _abort = false;
            }

            if (result == DialogResult.Cancel)
            {
                _abort = true;
            }

        }

        /// <summary>
        /// Kill the Windows message box and stop the thread
        /// </summary>
        /// <param name="confirmationMsgBoxThread"></param>
        private void KillMessageBox(Thread confirmationMsgBoxThread)
        {
            PopupAssassin.KillWindow(PRINTER_ERROR_WINDOW_TITLE);
            confirmationMsgBoxThread.Abort();
        }

        #endregion
    }
}
