using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Automation.OfficeWorker
{
    /// <summary>
    /// Class to support the installation of local printers.  Installs both the driver and the queue.
    /// </summary>
    internal class LocalPrintQueueInstaller
    {
        private readonly List<LocalPrintDeviceInstaller> _localPrintDeviceInstallers = new List<LocalPrintDeviceInstaller>();

        /// <summary>
        /// Occurs when the print queue is installed.
        /// </summary>
        public event EventHandler<LocalPrintQueueInstalledEventArgs> PrintQueueInstalled;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPrintQueueInstaller"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public LocalPrintQueueInstaller(SystemManifest manifest)
        {
            // The manifest includes all queues required for the entire session.
            // Only install queues for activities that are part of this manifest.
            var activityIds = manifest.Resources.SelectMany(n => n.MetadataDetails).Select(n => n.Id);
            var printingActivityIds = activityIds.Where(n => manifest.ActivityPrintQueues.ContainsKey(n));
            List<DynamicLocalPrintQueueInfo> printQueues = printingActivityIds.Select(n => manifest.ActivityPrintQueues[n]).SelectMany(n => n.OfType<DynamicLocalPrintQueueInfo>()).ToList();
            foreach (DynamicLocalPrintQueueInfo printQueueInfo in printQueues)
            {
                _localPrintDeviceInstallers.Add(new LocalPrintDeviceInstaller(printQueueInfo));
            }
        }

        public void Install()
        {
            TraceFactory.Logger.Debug("Installing local print queues.");
            foreach (var printDevice in _localPrintDeviceInstallers)
            {
                TraceFactory.Logger.Info("Installing {0} with shortcut: <{1}>".FormatWith(printDevice.QueueName, printDevice.DefaultShortcut));
                printDevice.Install();
                PrintQueueInstalled?.Invoke(this, new LocalPrintQueueInstalledEventArgs(printDevice));
                TraceFactory.Logger.Debug("Installed " + printDevice.QueueName);
            }
            TraceFactory.Logger.Debug("Installing local print queues completed.");
        }

        public void ValidateShortcuts()
        {
            TraceFactory.Logger.Debug("Validating shortcuts.");
            foreach (LocalPrintDeviceInstaller printDevice in _localPrintDeviceInstallers)
            {
                printDevice.ValidatePrintingShortcut();
            }
            TraceFactory.Logger.Debug("Validating shortcuts completed.");
        }
    }

    /// <summary>
    /// <see cref="EventArgs"/> class used to support when a local printer is installed.
    /// </summary>
    internal class LocalPrintQueueInstalledEventArgs : EventArgs
    {
        public string QueueName { get; }
        public string DriverName { get; }

        public LocalPrintQueueInstalledEventArgs(LocalPrintDeviceInstaller printQueue)
        {
            QueueName = printQueue.QueueName;
            DriverName = printQueue.DriverName;
        }
    }
}
