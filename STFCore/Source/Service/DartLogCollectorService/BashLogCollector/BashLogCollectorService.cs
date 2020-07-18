using HP.ScalableTest;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework.DartLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace DartLogCollectorService.BashLogCollector
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BashLogCollectorService : IBashLogCollectorService, IDisposable
    {
        private readonly ConcurrentDictionary<string, LowLevelParserLib> _bashLevelParserLibs = new ConcurrentDictionary<string, LowLevelParserLib>();

        public string CreateLogger(string address)
        {
            LowLevelParserLib bashParserLib;
            string listenerAddress = string.Empty;

            
            if (!address.Contains(":") && !address.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
            {
                if (_bashLevelParserLibs.ContainsKey(address))
                {
                    return address;
                }
                
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var bashLogCollector = context.BashLogCollectors.FirstOrDefault(n => n.PrinterId == address);
                    if (bashLogCollector == null)
                    {
                        TraceFactory.Logger.Error($@"No BashLog Collector found for: {address}.");
                        throw new Exception($"Unable to create a bash collector for the device: {address}");
                    }
                    if (bashLogCollector.Port == null || bashLogCollector.Port.Value == 0)
                    {
                        listenerAddress = bashLogCollector.Address;
                    }
                    else
                    {
                        if (bashLogCollector.Port != null)
                            listenerAddress = $"{bashLogCollector.Address}:{bashLogCollector.Port.Value}";
                    }
                }
            }

            if (string.IsNullOrEmpty(listenerAddress))
            {
                listenerAddress = address;
            }

            if (listenerAddress.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
                bashParserLib = new LowLevelParserLib(listenerAddress, address);
            else
            {
                var telnetAddress = listenerAddress.Split(':');
                bashParserLib = new LowLevelParserLib(IPAddress.Parse(telnetAddress[0]), Convert.ToInt32(telnetAddress[1]));
            }

            _bashLevelParserLibs.AddOrUpdate(address, bashParserLib, (key, lib) => bashParserLib);

            return address;
        }

        public List<string> GetSerialPorts()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public string CollectLog(string key)
        {
            LowLevelParserLib bashLevelParserLib;
            if (!_bashLevelParserLibs.TryGetValue(key, out bashLevelParserLib))
            {
                TraceFactory.Logger.Error("Please create a bash logger using the CreateLogger method");
                return string.Empty;
            }

            return bashLevelParserLib.Log;
        }

        /// <summary>
        /// Starts the logging for the specified device at the address
        /// </summary>
        public void StartLogging(string key)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.PollLog = true;
        }

        public void StopLogging(string key)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.PollLog = false;
        }

        public void Flush(string key)
        {
            LowLevelParserLib bashLevelParserLib;
            if (!_bashLevelParserLibs.TryGetValue(key, out bashLevelParserLib))
            {
                TraceFactory.Logger.Error("Please create a bash logger using the CreateLogger method");
                return;
            }
            bashLevelParserLib.ClearLog();
        }

        public void Dispose()
        {
            foreach (var bashLevelParserLib in _bashLevelParserLibs)
            {
                bashLevelParserLib.Value.Dispose();
            }
            _bashLevelParserLibs.Clear();
        }

        public string ReadStream(string key)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            return bashLevelParserLib.ReadStream();
        }

        public void WriteStream(string key, byte value)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.WriteStream(value);
        }

        public void WriteStream(string key, string command)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.WriteStream(command);
        }

        public void WriteStream(string key, byte[] data)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.WriteStream(data);
        }

        public void WriteToEfiShell(string key, string command)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            bashLevelParserLib.WriteToEfiShell(command);
        }

        public string WaitFor(string key, Int32 waitTimeInMSecs, Regex[] waitTerminatePatterns, Boolean verbose, Func<bool> cancelFunction)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            return bashLevelParserLib.WaitFor(waitTimeInMSecs, waitTerminatePatterns, verbose, cancelFunction);
        }

        public string WaitFor(string key, int waitTimeInMilliSeconds, string pattern, bool verbose)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            return bashLevelParserLib.WaitFor(waitTimeInMilliSeconds, pattern, verbose);
        }

        public string WaitForLine(string key, Int32 waitTimeInMSecs, string pattern)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);
            return bashLevelParserLib.WaitForLine(waitTimeInMSecs, pattern);
        }

        public void RemoveLogger(string key)
        {
            LowLevelParserLib bashLevelParserLib;
            if (!_bashLevelParserLibs.TryGetValue(key, out bashLevelParserLib))
            {
                TraceFactory.Logger.Error("Please create a bash logger using the CreateLogger method");
                return;
            }

            bashLevelParserLib.Dispose();
            _bashLevelParserLibs.TryRemove(key, out bashLevelParserLib);
        }

        private LowLevelParserLib GetLowLevelParser(string key)
        {
            LowLevelParserLib bashLevelParserLib;
            if (!_bashLevelParserLibs.TryGetValue(key, out bashLevelParserLib))
            {
                TraceFactory.Logger.Error("Please create a bash logger using the CreateLogger method");

                return null;
            }
            if (!bashLevelParserLib.IsOpen)
                bashLevelParserLib.Open();

            return bashLevelParserLib;
        }

        public string ReadCommandOutputStream(string key)
        {
            LowLevelParserLib bashLevelParserLib = GetLowLevelParser(key);

            return bashLevelParserLib.ReadCommandOutputStream();
        }
    }
}