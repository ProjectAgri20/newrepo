using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Core.DataLog;
using System.Collections.Generic;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Monitors a directory for output files from a solution (DSS, HPCR, etc.).
    /// </summary>
    public class OutputDirectoryMonitor: OutputMonitorBase
    {
        private FileSystemWatcher _watcher = null;
        private List<string> _filters = new List<string>();

        /// <summary>
        /// Gets the validation options.
        /// </summary>
        protected OutputMonitorConfig Options
        {
            get { return Configuration; }
        }

        /// <summary>
        /// Gets the location to monitor.
        /// </summary>
        public override string MonitorLocation
        {
            get { return Configuration.MonitorLocation; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputDirectoryMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public OutputDirectoryMonitor(MonitorConfig monitorConfig)
            : this(monitorConfig, new string[] { ".pdf", ".jpg", ".tif" })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputDirectoryMonitor" /> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        /// <param name="filePattern">The file pattern.</param>
        public OutputDirectoryMonitor(MonitorConfig monitorConfig, IEnumerable<string> filePattern) 
            : base(monitorConfig)
        {
            foreach (string filter in filePattern)
            {
                if (!_filters.Contains(filter))
                {
                    _filters.Add(filter);
                }
            }

            _watcher = new FileSystemWatcher(MonitorLocation);
            _watcher.Filter = "*.*";
            _watcher.EnableRaisingEvents = true;
            _watcher.Created += new FileSystemEventHandler(OnFileCreated);
         
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {            
            // When the Created event fires, the file may still be writing to the disk
            // Wait a moment to let the file finish writing
            Thread.Sleep(TimeSpan.FromSeconds(1));

            string ext = Path.GetExtension(e.FullPath).ToLower();

            if (_filters.Contains(ext))
            {
                ProcessFile(e.FullPath, DateTime.Now);
            }
        }

        /// <summary>
        /// Processes existing files in the destination being monitored.
        /// </summary>
        private void ProcessExisting()
        {
            TraceFactory.Logger.Debug("Processing existing files in '{0}'.".FormatWith(Configuration.MonitorLocation));
            try
            {
                foreach (string file in System.IO.Directory.GetFiles(Configuration.MonitorLocation))
                {
                    string ext = Path.GetExtension(file).ToLower();
                    
                    if (_filters.Contains(ext))
                    {
                        TraceFactory.Logger.Debug("Processing file {0} on filter {1}".FormatWith(file, ext));
                        ProcessFile(file);
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("NOT Processing file {0} on filter {1}".FormatWith(file, ext));
                    }
                }
                
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Error(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                TraceFactory.Logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Processes the located file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="createdEventTime">The created event time.</param>
        protected virtual void ProcessFile(string filePath, DateTime? createdEventTime = null)
        {
            TraceFactory.Logger.Debug("Found file: {0}".FormatWith(filePath));

            try
            {
                string fileName = Path.GetFileName(filePath);
                ScanFilePrefix filePrefix = ScanFilePrefix.Parse(fileName);

                // Create the log for this file
                DigitalSendJobOutputLogger log = new DigitalSendJobOutputLogger(fileName, filePrefix.ToString(), filePrefix.SessionId);
                log.FileSentDateTime = System.IO.Directory.GetCreationTime(filePath);
                log.FileReceivedDateTime = createdEventTime;
                log.FileLocation = "{0} - {1}".FormatWith(Environment.MachineName, Path.GetDirectoryName(filePath));

                // Validate and analyze the file
                OutputProcessor processor = new OutputProcessor(filePath);
                ValidationResult result = null;
                Retry.WhileThrowing(
                    () => result = processor.Validate(Configuration),
                    10,
                    TimeSpan.FromSeconds(2),
                    new Collection<Type>() { typeof(IOException) });

                DocumentProperties properties = processor.GetProperties();
                log.FileSizeBytes = properties.FileSize;
                log.PageCount = properties.Pages;
                log.SetErrorMessage(result);

                // Clean up the file
                processor.ApplyRetention(Configuration, result.Succeeded);

                // Send the output log
                new DataLogger(GetLogServiceHost(filePrefix.SessionId)).Submit(log);
            }
            catch (IOException ex)
            {
                LogProcessFileError(filePath, ex);
            }
            catch (FormatException ex)
            {
                LogProcessFileError(filePath, ex);
            }
        }

        private static void LogProcessFileError(string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("{0} could not be processed.".FormatWith(fileName), ex);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void StartMonitoring()
        {
            ProcessExisting();
            _watcher.EnableRaisingEvents = true;          
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void StopMonitoring()
        {
            _watcher.EnableRaisingEvents = false;
        }

        /// <summary>
        /// String representation of <see cref="OutputDirectoryMonitor"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Directory Monitor for {Configuration.MonitorLocation}";

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {                            
                if (_watcher != null)
                {
                    _watcher.Dispose();
                    _watcher = null;
                }
            }
        }

        #endregion
    }
}
