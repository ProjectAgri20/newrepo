using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Print.Utility
{
    internal class QueueInstallationDataFactory
    {
        private readonly Dictionary<string, int> _queueIndex = new Dictionary<string, int>();

        public void Reset()
        {
            _queueIndex.Clear();
        }

        /// <summary>
        /// Constructs the queue definitions based on the provided list of printer Ids.
        /// </summary>
        /// <param name="printerIds">The printer ids.</param>
        /// <param name="sourceDriver">The driver.</param>
        /// <param name="currentDriver">The driver.</param>
        /// <param name="description">The additional description.</param>
        /// <param name="queueCount">The queue count.</param>
        /// <returns></returns>
        public Collection<QueueInstallationData> Create
            (
                Collection<string> printerIds,
                PrintDeviceDriver sourceDriver,
                PrintDeviceDriver currentDriver,
                string description,
                int queueCount = 1,
                bool fullName = true
            )
        {
            Collection<QueueInstallationData> queueData = new Collection<QueueInstallationData>();
            StringBuilder queueName = new StringBuilder();

            if (printerIds.Count == 0)
            {
                return queueData;
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (Asset asset in context.Assets.Where(n => printerIds.Contains(n.AssetId)))
                {
                    for (int i = 0; i < queueCount; i++)
                    {
                        QueueInstallationData data = new QueueInstallationData();

                        // Assign the driver install data from a collected list of loaded package data
                        // based on the version.
                        data.Driver = sourceDriver;

                        data.AssetId = asset.AssetId;
                        data.QueueType = asset.AssetType;
                        data.Shared = true;

                        queueName.Clear();

                        if (asset.AssetType == "Printer")
                        {
                            Printer printer = (Printer)asset;

                            data.Port = printer.PortNumber;
                            data.Address = printer.Address1;
                            data.SnmpEnabled = true;
                            data.ClientRender = false;
                            data.Shared = true;
                            data.Description = printer.Description;

                            queueName.Append(printer.Product);
                            queueName.Append(" ").Append(data.AssetId);

                            if (fullName)
                            {
                                queueName.Append(" ").Append(currentDriver.DriverType);
                                queueName.Append(" ").Append(Regex.Replace(currentDriver.VerifyPdl, @"\s+", " "));
                            }

                            if (!string.IsNullOrEmpty(currentDriver.Release))
                            {
                                queueName.Append(" ").Append(Regex.Replace(currentDriver.Release, @"\s+", " "));
                            }

                            // Append the additional description data if it exists
                            if (!string.IsNullOrEmpty(description))
                            {
                                queueName.Append(" ").Append(Regex.Replace(description, @"\s+", " "));
                            }
                        }
                        else if (asset.AssetType == "VirtualPrinter")
                        {
                            VirtualPrinter virtualPrinter = (VirtualPrinter)asset;

                            //Build a shortened version of the asset ID
                            string addressCode = asset.AssetId.Split('-')[0];
                            AddressParser assetIP = new AddressParser(virtualPrinter.Address);

                            data.Port = virtualPrinter.PortNumber;
                            data.Address = virtualPrinter.Address;
                            data.SnmpEnabled = virtualPrinter.SnmpEnabled;
                            data.Description = "Virtual Printer";
                            data.ClientRender = false;
                            data.Shared = true;

                            queueName.Append(addressCode);
                            queueName.Append("-").Append(assetIP.GetOctet(2));
                            queueName.Append("-").Append(assetIP.GetOctet(3));
                            queueName.Append(" ").Append(currentDriver.DriverType);
                            queueName.Append(" ").Append(Regex.Replace(currentDriver.VerifyPdl, @"\s+", " "));

                            if (!string.IsNullOrEmpty(currentDriver.Release))
                            {
                                queueName.Append(" ").Append(Regex.Replace(currentDriver.Release, @"\s+", " "));
                            }

                            // Append the additional description data if it exists
                            if (!string.IsNullOrEmpty(description))
                            {
                                queueName.Append(" ").Append(Regex.Replace(description, @"\s+", " "));
                            }
                        }

                        data.Key = queueName.ToString();
                        IncrementQueueIndex(data);

                        if (queueCount > 1)
                        {
                            queueName.Append(" ").Append(_queueIndex[data.Key].ToString("D3"));
                        }

                        data.QueueName = queueName.ToString();
                        queueData.Add(data);
                    }
                }
            }

            return queueData;
        }

        /// <summary>
        /// Constructs the queue definitions based on user provided configuration.
        /// </summary>
        /// <param name="properties">The driver.</param>
        /// <param name="additionalDescription">The additional description.</param>
        /// <param name="ipStartValue">The start value of the last IP octet.</param>
        /// <param name="ipEndValue">The end value of the last IP octet.</param>
        /// <param name="hostName">The hostname.</param>
        /// <param name="numberOfQueues">The number of queues.</param>
        /// <param name="addressCode">The address code.</param>
        /// <param name="incrementIP">if set to <c>true</c> increment IP octet values.</param>
        /// <param name="enableSnmp">if set to <c>true</c> SNMP will be enable on the port.</param>
        /// <param name="renderOnClient">if set to <c>true</c> the render on client option will be set on the queue.</param>
        /// <param name="shareQueues">if set to <c>true</c> the queue will be shared.</param>
        /// <returns>Collection of QueueInstallationData</returns>
        public Collection<QueueInstallationData> Create
            (
                PrintDeviceDriver properties,
                string additionalDescription,
                int ipStartValue,
                int ipEndValue,
                string hostName,
                int numberOfQueues,
                string addressCode,
                bool incrementIP,
                bool enableSnmp,
                bool renderOnClient,
                bool shareQueues
            )
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }
            if (numberOfQueues < 1)
            {
                throw new ArgumentException("numberOfQueues must be a positive, non-zero integer.");
            }
            if (ipStartValue < 1)
            {
                throw new ArgumentException("ipStartValue must be a positive, non-zero integer.");
            }
            
            Collection<QueueInstallationData> queueData = new Collection<QueueInstallationData>();
            int currentIPNumber = ipStartValue;
            FrameworkServer vPrintServer = null;
            
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                vPrintServer = context.FrameworkServers.FirstOrDefault(n => n.HostName.StartsWith(hostName));
            }

            AddressParser serverIP = ParseAddress(vPrintServer);
            StringBuilder queueName = new StringBuilder();

            for (int i = 0; i < numberOfQueues; i++)
            {
                queueName.Clear();

                QueueInstallationData data = new QueueInstallationData();
                data.Driver = properties;
                data.QueueType = "VirtualPrinter";
                data.AssetId = "{0}-{1:D5}".FormatWith(addressCode, currentIPNumber);
                data.Address = serverIP.Prefix + currentIPNumber.ToString();
                data.SnmpEnabled = enableSnmp;
                data.ClientRender = renderOnClient;
                data.Shared = shareQueues;
                
                queueName.Append(addressCode);
                queueName.Append("-").Append(serverIP.GetOctet(2));
                queueName.Append("-").Append(currentIPNumber.ToString("D3"));
                queueName.Append(" ").Append(properties.DriverType);
                queueName.Append(" ").Append(Regex.Replace(properties.VerifyPdl, @"\s+", " "));

                if (!string.IsNullOrEmpty(properties.Release))
                {
                    queueName.Append(" ").Append(Regex.Replace(properties.Release, @"\s+", " "));
                }

                // Append the additional description data if it exists
                if (!string.IsNullOrEmpty(additionalDescription))
                {
                    queueName.Append(" ").Append(Regex.Replace(additionalDescription, @"\s+", " "));
                }

                // Append a queue index if we're reusing the Virtual Printer for multiple queues
                if (incrementIP == false || numberOfQueues > 255)
                {
                    data.Key = queueName.ToString();
                    IncrementQueueIndex(data);
                    queueName.Append(" ").Append(_queueIndex[data.Key].ToString("D3"));
                }

                data.QueueName = queueName.ToString();
                queueData.Add(data);

                if (currentIPNumber == ipEndValue)
                {
                    //Restart the IP number, don't increment
                    currentIPNumber = ipStartValue;
                }
                else if (incrementIP)
                {
                    currentIPNumber++;
                }
            }

            return queueData;
        }

        private void IncrementQueueIndex(QueueInstallationData data)
        {
            if (!_queueIndex.ContainsKey(data.Key))
            {
                _queueIndex.Add(data.Key, 0);
            }

            _queueIndex[data.Key]++;
        }

        private AddressParser ParseAddress(FrameworkServer vPrintServer)
        {
            if (string.IsNullOrEmpty(vPrintServer.IPAddress))
            {
                throw new MissingFieldException("IP Address is not set for {0}".FormatWith(vPrintServer.HostName));
            }

            return new AddressParser(vPrintServer.IPAddress);
        }
    }

    /// <summary>
    /// The purpose of this class is to provide various substrings of a string-based IP Address to be used for 
    /// describing dynamically-generated print queue names.  The intent is to parse the IP string once, then cache
    /// the parsed information for later use.  For example, when building ad-hoc print queue names, part of the
    /// name is the third and fourth octet values.  The IP address is parsed once before the loop begins creating
    /// the names, then the individual pieces are available for use in a StringBuilder.  The Prefix property is 
    /// saved as is to avoid the overhead of concatenating the first 3 octet values each time it is accessed.
    /// This class was not created in the common namespace for the reason that its usage is specific to print queue
    /// naming and is written to be optimized for string manipulation not optimized for byte arrays.
    /// </summary>
    internal class AddressParser
    {
        private string _prefix = string.Empty;
        private string[] _parsed = null;

        public AddressParser(string ipAddress)
        {
            if (! string.IsNullOrEmpty(ipAddress))
            {
                _prefix = ipAddress.Substring(0, ipAddress.LastIndexOf('.') + 1);
                _parsed = ipAddress.Split('.');
            }
        }

        /// <summary>
        /// The first 3 octet values (including the last ".") of the IP Address.
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
        }

        /// <summary>
        /// Returns the specified octet by index value.
        /// </summary>
        /// <param name="zeroBasedIndex">The index of the desired octet</param>
        /// <returns></returns>
        public string GetOctet(int zeroBasedIndex)
        {
            try
            {
                return _parsed[zeroBasedIndex].PadLeft(3, '0');
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
