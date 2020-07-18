using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HP.ScalableTest.WindowsAutomation;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Service.Monitor.Directory
{
    /// <summary>
    /// Monitors a directory and logs the number and size of files over time.
    /// </summary>
    public sealed class DirectoryMonitor : StfMonitor
    {
        private volatile bool _scanning;
        private Dictionary<string, long> _files;
        private FileSystemWatcher _fileWatcher;
        private DirectoryMonitorConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryMonitor" /> class.
        /// </summary>
        /// <param name="monitorConfig">The configuration data needed to start this instance.</param>
        public DirectoryMonitor(MonitorConfig monitorConfig) 
            : base(monitorConfig)
        {
            RefreshConfig(monitorConfig);

            TraceFactory.Logger.Info($"Pointing to {MonitorLocation}");
            TraceFactory.Logger.Info($"Logging to {_config.LogServiceHostName}");
            _files = new Dictionary<string, long>();

            _fileWatcher = new FileSystemWatcher(MonitorLocation);
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;

            _fileWatcher.Created += (s, e) => { AddOrUpdateFile(e.FullPath); LogUpdate(); };
            _fileWatcher.Changed += (s, e) => { AddOrUpdateFile(e.FullPath); LogUpdate(); };
            _fileWatcher.Deleted += (s, e) => { DeleteFile(e.FullPath); LogUpdate(); };
            _fileWatcher.Renamed += (s, e) => { RenameFile(e.OldFullPath, e.FullPath); LogUpdate(); };

        }

        /// <summary>
        /// Gets the location to monitor.
        /// </summary>
        public override string MonitorLocation
        {
            get { return _config.MonitorLocation; }
        }

        /// <summary>
        /// Gets the number of files in the directory.
        /// </summary>
        /// <value>The file count.</value>
        public int FileCount
        {
            get { return _files.Count; }
        }

        /// <summary>
        /// Gets the total size of all files in the directory, in bytes.
        /// </summary>
        /// <value>The directory size, in bytes.</value>
        public long ByteCount
        {
            get { return _files.Values.Sum(); }
        }

        /// <summary>
        /// Starts monitoring.
        /// </summary>
        public override void StartMonitoring()
        {
            _files.Clear();
            _fileWatcher.EnableRaisingEvents = true;
            TraceFactory.Logger.Info("Directory monitor started.");

            ThreadPool.QueueUserWorkItem(o => ScanDirectory());
        }

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        public override void StopMonitoring()
        {
            _fileWatcher.EnableRaisingEvents = false;
            TraceFactory.Logger.Info("Directory monitor stopped.");
        }

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        /// <param name="monitorConfig"></param>
        public override void RefreshConfig(MonitorConfig monitorConfig)
        {
            _config = LegacySerializer.DeserializeXml<DirectoryMonitorConfig>(monitorConfig.Configuration);
        }

        private void LogUpdate()
        {
            if (!_scanning)
            {
                DirectorySnapshotLogger logger = new DirectorySnapshotLogger()
                {
                    DirectoryName = _fileWatcher.Path,
                    TotalFiles = this.FileCount,
                    TotalBytes = this.ByteCount
                };
                new DataLogger(_config.LogServiceHostName).SubmitAsync(logger);

                TraceFactory.Logger.Info($"Directory contains {logger.TotalFiles} files totaling {logger.TotalBytes} bytes");
            }
        }

        private void AddOrUpdateFile(string filePath)
        {
            TraceFactory.Logger.Debug("File updated: " + filePath);
            lock (_files)
            {
                _files[filePath] = GetFileSize(filePath);
            }
        }

        private void DeleteFile(string filePath)
        {
            TraceFactory.Logger.Debug("File deleted: " + filePath);
            lock (_files)
            {
                _files.Remove(filePath);
            }
        }

        private void RenameFile(string oldFilePath, string newFilePath)
        {
            TraceFactory.Logger.Debug("File renamed: " + oldFilePath + " -> " + newFilePath);
            lock (_files)
            {
                _files.Remove(oldFilePath);
                _files[newFilePath] = GetFileSize(newFilePath);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static int GetFileSize(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return FileSystem.GetFileSize(filePath);
                }
            }
            catch
            {
                // Don't crash here
            }
            return 0;
        }

        private void ScanDirectory()
        {
            TraceFactory.Logger.Info("Building directory cache...");
            _scanning = true;
            LoadDirectory(_fileWatcher.Path);
            _scanning = false;
            LogUpdate();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void LoadDirectory(string path)
        {
            // Get all the files in this directory
            bool done = false;
            string[] files = null;
            while (!done)
            {
                try
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        files = System.IO.Directory.GetFiles(path);
                        done = true;
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }

            // Process each of the files
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    AddOrUpdateFile(file);
                }
            }

            // Get the subdirectories of this directory
            done = false;
            string[] directories = null;
            while (!done)
            {
                try
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        directories = System.IO.Directory.GetDirectories(path);
                        done = true;
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }

            // Recursively scan the rest of the directories
            foreach (string directory in directories)
            {
                LoadDirectory(directory);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_fileWatcher != null)
            {
                _fileWatcher.Dispose();
                _fileWatcher = null;
            }
        }

        #endregion
    }
}
