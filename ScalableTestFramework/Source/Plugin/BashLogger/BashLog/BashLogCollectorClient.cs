using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text.RegularExpressions;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Plugin.BashLogger.BashLog
{
    /// <summary>
    /// Bash Log Collector Service
    /// </summary>
    public class BashLogCollectorClient : IBashLogCollectorService, IDisposable
    {
        private readonly string _serviceLocation;
        private readonly WcfClient<IBashLogCollectorService> _client;
        private string _key;

        /// <summary>
        /// empty constructor
        /// </summary>
        public BashLogCollectorClient()
        {
            _serviceLocation = "localhost";
            _client = new WcfClient<IBashLogCollectorService>(MessageTransferType.Http, WcfService.BashLogcollectorService.GetHttpUri(_serviceLocation));
            var binding = _client.Endpoint.Binding as WSHttpBinding;
            if (binding != null)
            {
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                _client.Endpoint.Binding = binding;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serverUri"></param>

        public BashLogCollectorClient(string serverUri)
        {
            _serviceLocation = serverUri;
            _client = new WcfClient<IBashLogCollectorService>(MessageTransferType.Http, WcfService.BashLogcollectorService.GetHttpUri(_serviceLocation));
            var binding = _client.Endpoint.Binding as WSHttpBinding;
            if (binding != null)
            {
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                _client.Endpoint.Binding = binding;
            }
        }

        /// <summary>
        /// Creates a bash logger for the specified address
        /// </summary>
        /// <param name="address"></param>
        public string CreateLogger(string address)
        {
            _key = _client.Channel.CreateLogger(address);
            return _key;
        }

        /// <summary>
        /// Collects Logs for the specified asset and for session
        /// </summary>
        public string CollectLog(string key)
        {
            return _client.Channel.CollectLog(key);
        }

        /// <summary>
        /// Starts the log collection process
        /// </summary>
        /// <returns></returns>
        public void StartLogging(string key)
        {
            _client.Channel.StartLogging(key);
        }

        /// <summary>
        /// stops the collection of logs
        /// </summary>
        /// <returns></returns>
        public void StopLogging(string key)
        {
            _client.Channel.StopLogging(key);
        }

        /// <summary>
        /// flushes the logs to a file
        /// </summary>
        /// <returns></returns>
        public void Flush(string key)
        {
            _client.Channel.Flush(key);
        }

        /// <summary>
        /// Reads the data from stream
        /// </summary>
        public string ReadStream(string key)
        {
            return _client.Channel.ReadStream(key);
        }

        /// <summary>
        /// Reads the output of a command executed previously, MUST be called after write stream
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadCommandOutputStream(string key)
        {
            return _client.Channel.ReadCommandOutputStream(key);
        }

        /// <summary>
        /// Writes the data to the stream
        /// </summary>
        public void WriteStream(string key,byte value)
        {
            _client.Channel.WriteStream(key, value);
        }

        /// <summary>
        /// Writes the command to the stream
        /// </summary>
        /// <param name="key"></param>
        /// <param name="command">the command to be executed</param>
        public void WriteStream(string key,string command)
        {
            _client.Channel.WriteStream(key, command);
        }

        /// <summary>
        /// Writes the byte array to the stream
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteStream(string key,byte[] data)
        {
            _client.Channel.WriteStream(key,data);
        }

        /// <summary>
        /// Writes the command to the EFI shell
        /// </summary>
        /// <param name="key"></param>
        /// <param name="command"></param>
        public void WriteToEfiShell(string key,string command)
        {
            _client.Channel.WriteToEfiShell(key,command);
        }

        /// <summary>
        /// Removes the logger
        /// </summary>
        /// <param name="key"></param>
        public void RemoveLogger(string key)
        {
            _client.Channel.RemoveLogger(key);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if(!string.IsNullOrEmpty(_key))
                RemoveLogger(_key);

            _client.Close();
        }

        /// <summary>
        /// Waits for the pattern to appear on the shell prompt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="waitTimeInMSecs"></param>
        /// <param name="waitTerminatePatterns"></param>
        /// <param name="verbose"></param>
        /// <param name="cancelFunction"></param>
        /// <returns></returns>
        public string WaitFor(string key, int waitTimeInMSecs, Regex[] waitTerminatePatterns, bool verbose, Func<bool> cancelFunction)
        {
            return _client.Channel.WaitFor(key, waitTimeInMSecs, waitTerminatePatterns, verbose, cancelFunction);
        }

        /// <summary>
        /// Waits for the patter to appear on the shell prompt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="waitTimeInMSecs"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string WaitForLine(string key, int waitTimeInMSecs, string pattern)
        {
            return _client.Channel.WaitForLine(key, waitTimeInMSecs, pattern);
        }

        /// <summary>
        /// Waits for the pattern to appear on the shell prompt
        /// </summary>
        /// <param name="key"></param>
        /// <param name="waitTimeInMilliSeconds"></param>
        /// <param name="pattern"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public string WaitFor(string key, int waitTimeInMilliSeconds, string pattern, bool verbose)
        {
            return _client.Channel.WaitFor(key, waitTimeInMilliSeconds, pattern, verbose);
        }

        /// <summary>
        /// Gets the list of serial ports on BashLog Collector Service Host
        /// </summary>
        /// <returns></returns>
        public List<string> GetSerialPorts()
        {
            return _client.Channel.GetSerialPorts();
        }
    }
}