using System;
using System.Collections.ObjectModel;
using System.IO;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Monitors a directory for LAN Fax output files.
    /// </summary>
    internal class LANFaxMonitor : OutputDirectoryMonitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LANFaxMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public LANFaxMonitor(MonitorConfig monitorConfig) 
            : base(monitorConfig, new string[] { "*.tif" })
        {
            // For the LAN Fax monitoring, we are always interested in the HPF metadata
            // file that is created. Without it, we cannot process the file correctly.
            Configuration.LookForMetadataFile = true;
            Configuration.MetadataFileExtension = "hpf";
        }

        /// <summary>
        /// Processes the located file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="createdEventTime">The created event time.</param>
        protected override void ProcessFile(string filePath, DateTime? createdEventTime = null)
        {
            string fileName = Path.GetFileName(filePath);
            TraceFactory.Logger.Debug("Found file: {0}".FormatWith(fileName));

            // Check to see if we have the associated HPF file - without it, we can't process this file
            string hpfFile = Path.ChangeExtension(filePath, ".hpf");
            if (!Retry.UntilTrue(() => File.Exists(filePath), 30, TimeSpan.FromSeconds(2)))
            {
                TraceFactory.Logger.Debug("No associated HPF file - cannot process.");
                return;
            }

            // Extract the file prefix from the HPF metadata file
            ScanFilePrefix filePrefix = null;
            try
            {
                Retry.WhileThrowing(
                    () => filePrefix = ExtractFileName(hpfFile),
                    10,
                    TimeSpan.FromSeconds(2),
                    new Collection<Type>() { typeof(IOException) });
            }
            catch (IOException ex)
            {
                LogProcessFileError(fileName + " HPF file ", ex);
                return;
            }
            catch (FormatException ex)
            {
                LogProcessFileError(fileName + " HPF file ", ex);
                return;
            }

            try
            {
                // Create the log for this file
                DigitalSendJobOutputLogger log = new DigitalSendJobOutputLogger(filePrefix, Path.GetExtension(filePath));
                log.FileName = Path.GetFileName(filePath);
                log.FileSentDateTime = System.IO.Directory.GetCreationTime(filePath);
                log.FileReceivedDateTime = createdEventTime;
                log.FileLocation = "{0} - {1}".FormatWith(Environment.MachineName, Path.GetDirectoryName(filePath));

                // Validate and analyze the file
                OutputProcessor processor = new OutputProcessor(filePath);
                ValidationResult result = null;
                Retry.WhileThrowing(
                    () => result = processor.Validate(Options),
                    10,
                    TimeSpan.FromSeconds(2),
                    new Collection<Type>() { typeof(IOException) });

                DocumentProperties properties = processor.GetProperties();
                log.FileSizeBytes = properties.FileSize;
                log.PageCount = properties.Pages;
                log.SetErrorMessage(result);

                // Clean up the file
                Options.RetentionFileName = filePrefix.ToString();
                processor.ApplyRetention(Options, result.Succeeded);

                // Send the output log
                new DataLogger(GetLogServiceHost(filePrefix.SessionId)).Submit(log);
            }
            catch (IOException ex)
            {
                LogProcessFileError(fileName, ex);
            }
            catch (FormatException ex)
            {
                LogProcessFileError(fileName, ex);
            }

            // Create a notification file for the fax
            try
            {
                CreateNotificationFile(hpfFile, filePrefix);
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Error(ex);
            }
        }

        private static void LogProcessFileError(string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("{0} could not be processed.".FormatWith(fileName), ex);
        }

        private static ScanFilePrefix ExtractFileName(string hpfFile)
        {
            string faxCode = string.Empty;
            string user = string.Empty;

            foreach (string line in File.ReadLines(hpfFile))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(' ');
                    switch (parts[0])
                    {
                        case "##UserName":
                            user = parts[1];
                            break;

                        case "##dial":
                            faxCode = parts[1];
                            break;
                    }
                }
            }
            
            return ScanFilePrefix.ParseFromFax(faxCode, user);
        }

        private static void CreateNotificationFile(string hpfFile, ScanFilePrefix filePrefix)
        {
            using (StreamWriter file = new StreamWriter(Path.ChangeExtension(hpfFile, "000")))
            {
                file.WriteLine("Status: OK");
                file.WriteLine("Fax_number: " + filePrefix.ToFaxCode());
            }
        }
    }
}
