using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Handles printing jobs.
    /// </summary>
    public class PrintController
    {
        /// <summary>
        /// Prints a document.
        /// </summary>
        /// <param name="printQueue">The printer to use.</param>
        /// <param name="fileName">File to print.</param>
        public void Print(PrintQueue printQueue, string fileName)
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException("printQueue");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            Guid fileId = Guid.NewGuid();
            string uniqueFileName = GetUniqueFileName(fileName, fileId);
            try
            {
                // Now we create the print job data with the full path and execute it
                Logger.LogDebug($"Printing {fileName} to {printQueue.FullName}");

                FilePrinter filePrinter = FilePrinterFactory.Create(new FileInfo(uniqueFileName));
                Retry.WhileThrowing(() => CreateUniqueJobFile(fileName, uniqueFileName), 2, new TimeSpan(0, 0, 2), new List<Type> { typeof(Exception) });

                filePrinter.Print(printQueue);
            }
            finally
            {
                DeleteUniqueJobFile(uniqueFileName);
            }
        }

        /// <summary>
        /// Deletes a file with a unique file name.
        /// Sometimes the file will be locked while the printing application is shutting down.
        /// For this reason, the task of deleting the file is backgrounded, so we can retry until the file deletes.
        /// </summary>
        /// <param name="uniqueJobFileName"></param>
        public void DeleteUniqueJobFile(string uniqueJobFileName)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(fileDeleteWorker_DoWork), uniqueJobFileName);
        }

        /// <summary>
        /// Keeps trying to delete a file until it succeeds.
        /// </summary>
        /// <param name="fileAsObject">Filename as a string</param>
        private void fileDeleteWorker_DoWork(object fileAsObject)
        {
            try
            {
                string filename = (string)fileAsObject;

                while (true)
                {
                    try
                    {
                        File.Delete(filename);
                        Logger.LogDebug("Deleted file: " + filename);
                        return;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Thread.Sleep(5 * 1000);
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(5 * 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                // If we simply re-throw the exception, the application will die.  Since this operation is to simply
                // delete a file, we do not want the application to die because of it.
                Logger.LogError("Ignoring exception", ex);
            }
        }

        private static void CreateUniqueJobFile(string fileName, string uniqueFileName)
        {
            Logger.LogDebug($"{fileName} -> {uniqueFileName}");
            try
            {
                File.Copy(fileName, uniqueFileName, true);
            }
            catch (Exception ex)
            {
                Logger.LogDebug($"Failed to copy temp file: {ex.Message}");
            }
        }

        private static string GetUniqueFileName(string fileName, Guid fileId)
        {
            return string.Format("{0}__{1}_-_{2}__{3}",
                Path.GetTempPath(),
                Path.GetFileNameWithoutExtension(fileName),
                fileId,
                Path.GetExtension(fileName));
        }
    }
}