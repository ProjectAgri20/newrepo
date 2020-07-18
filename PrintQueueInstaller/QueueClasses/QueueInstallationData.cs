using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Defines data associated with installing a print queue
    /// </summary>
    [Serializable]
    public class QueueInstallationData
    {
        /// <summary>
        /// Gets or sets the driver package.
        /// </summary>
        /// <value>
        /// The driver package.
        /// </value>
        public PrintDeviceDriver Driver { get; set; }

        /// <summary>
        /// Gets the print driver.
        /// </summary>
        /// <value>
        /// The print driver.
        /// </value>
        public string DriverName
        {
            get { return Driver.Name; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to install the additional driver.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if install additional driver; otherwise, <c>false</c>.
        /// </value>
        public bool InstallAdditionalDriver { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the config file path.
        /// </summary>
        /// <value>
        /// The config file path.
        /// </value>
        public string ConfigurationFilePath { get; set; }

        /// <summary>
        /// Gets the configuration file.
        /// </summary>
        public string ConfigurationFile
        {
            get { return Path.GetFileName(ConfigurationFilePath); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [use configuration file].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use configuration file]; otherwise, <c>false</c>.
        /// </value>
        public bool UseConfigurationFile { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        /// <value>
        /// The asset id.
        /// </value>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SNMP enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [SNMP enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="QueueInstallationData"/> is shared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if shared; otherwise, <c>false</c>.
        /// </value>
        public bool Shared { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the type of the queue.
        /// </summary>
        /// <value>
        /// The type of the queue.
        /// </value>
        public string QueueType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [client render].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [client render]; otherwise, <c>false</c>.
        /// </value>
        public bool ClientRender { get; set; }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        public string Progress { get; set; }

        /// <summary>
        /// Gets a value indicating whether the queue is installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if queue is installed; otherwise, <c>false</c>.
        /// </value>
        public bool QueueIsInstalled
        {
            get { return Regex.Match(Progress, @"[0-9]+:[0-9]+\.[0-9]+").Success; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueInstallationData"/> class.
        /// </summary>
        public QueueInstallationData()
        {
            Progress = string.Empty;
            Port = 9100;
        }
    }
}
