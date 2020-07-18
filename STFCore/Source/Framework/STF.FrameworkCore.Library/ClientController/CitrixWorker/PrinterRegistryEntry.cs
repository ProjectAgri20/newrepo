using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// </summary>
    public class PrinterRegistryEntry
    {
        private const string _queueRegistryPath = @"SYSTEM\CurrentControlSet\Control\Print\Printers\{0}";

        private readonly CitrixQueueClientData _clientData = null;
        private string _fullQueueName = string.Empty;
        private UniversalPrintDriverType _driverType = UniversalPrintDriverType.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterRegistryEntry"/> class.
        /// </summary>
        public PrinterRegistryEntry(CitrixQueueClientData data)
        {
            _clientData = data;
        }

        /// <summary>
        /// Gets the type of the driver.
        /// </summary>
        /// <value>
        /// The type of the driver.
        /// </value>
        public UniversalPrintDriverType DriverType
        {
            get { return _driverType; }
        }

        /// <summary>
        /// Gets the client data.
        /// </summary>
        /// <value>
        /// The client data.
        /// </value>
        public CitrixQueueClientData ClientData
        {
            get { return _clientData; }
        }

        /// <summary>
        /// Gets the full name of the queue.
        /// </summary>
        /// <value>
        /// The full name of the queue.
        /// </value>
        public string FullQueueName
        {
            get { return _fullQueueName; }
        }

        /// <summary>
        /// Gets the printer entries from the registry.
        /// </summary>
        public static Collection<string> PrinterEntries
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Print\Printers"))
                {
                    return new Collection<string>(key.GetSubKeyNames());
                }
            }
        }

        /// <summary>
        /// Checks to see if the queue creation is complete.
        /// </summary>
        /// <param name="queueNames">The queue names.</param>
        /// <returns></returns>
        public bool CheckQueueCreationComplete(Collection<string> queueNames)
        {
            // First look in DSSpooler to determine if the driver type is HP UPD
            // or Citrix UPD.  If it's HP, then you want to look at the InstallationComplete
            // flag to make sure it's done.  Otherwise just consider the queue creation 
            // complete.

            string queueName =
                (
                    from q in queueNames
                    where q.StartsWith(_clientData.QueueName, StringComparison.OrdinalIgnoreCase) && q.Contains(_clientData.HostName)
                    select q
                ).FirstOrDefault();

            if (string.IsNullOrEmpty(queueName))
            {
                // The queue is not found for this entry, so return false.
                return false;
            }

            _fullQueueName = queueName;

            bool isCitrixUpd = false;
            string queueRegistryPath = _queueRegistryPath.FormatWith(queueName);

            string dsSpoolerPath = Path.Combine(queueRegistryPath, "DsSpooler");
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(dsSpoolerPath))
            {
                if (key == null)
                {
                    return false;
                }

                object driverNameObj = (string)key.GetValue("driverName");

                if (driverNameObj == null)
                {
                    return false;
                }
                else
                {
                    string driverName = (string)driverNameObj;

                    if (driverName.Equals("Citrix Universal Printer", StringComparison.OrdinalIgnoreCase))
                    {
                        _driverType = UniversalPrintDriverType.Citrix;
                        isCitrixUpd = true;
                    }
                    else
                    {
                        _driverType = UniversalPrintDriverType.HP;
                    }
                }
            }

            bool returnStatus = false;

            if (isCitrixUpd)
            {
                returnStatus = true;
            }
            else
            {
                string driverRegistryPath = Path.Combine(queueRegistryPath, "PrinterDriverData");

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(driverRegistryPath))
                {
                    if (key == null)
                    {
                        returnStatus = false;
                    }
                    else
                    {
                        object installationCompleteObj = key.GetValue("InstallationComplete");
                        if (installationCompleteObj != null)
                        {
                            if ((int)installationCompleteObj == 0)
                            {
                                returnStatus = true;
                            }
                        }
                    }
                }
            }

            return returnStatus;
        }
    }

    /// <summary>
    /// Defines different driver types used on Citrix
    /// </summary>
    public enum UniversalPrintDriverType
    {
        /// <summary>
        /// No driver type
        /// </summary>
        None,
        /// <summary>
        /// the Citrix UPD
        /// </summary>
        Citrix,
        /// <summary>
        /// the HP UPD
        /// </summary>
        HP
    }
}