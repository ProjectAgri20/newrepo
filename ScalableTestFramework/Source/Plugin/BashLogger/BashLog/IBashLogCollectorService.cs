using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Plugin.BashLogger.BashLog
{
    /// <summary>
    /// Interface for Bash Collector Service
    /// </summary>
    [ServiceContract]
    public interface IBashLogCollectorService
    {
        /// <summary>
        /// Create a bash logger object for the specified address
        /// </summary>
        /// <param name="address">Address can be COM port, IP:Port, or AssetId</param>
        [OperationContract]
        string CreateLogger(string address);

        /// <summary>
        /// Removes the logger object
        /// </summary>
        /// <param name="key"></param>
        [OperationContract]
        void RemoveLogger(string key);

        /// <summary>
        /// Gets the list of available serial ports on the bash collector server
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string> GetSerialPorts();

        /// <summary>
        /// Collects the logs
        /// </summary>
        [OperationContract]
        string CollectLog(string key);

        /// <summary>
        /// Starts the process of collecting logs
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <returns></returns>
        [OperationContract]
        void StartLogging(string key);


        /// <summary>
        /// Stops the collection of logs
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <returns></returns>
        [OperationContract]
        void StopLogging(string key);

        /// <summary>
        /// Flushes the logs to a file
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <returns></returns>
        [OperationContract]
        void Flush(string key);

        /// <summary>
        /// Reads the data stream from either Serial or TCP
        /// </summary>
        /// <param name="key">Device Identifier</param>
        [OperationContract]
        string ReadStream(string key);

        /// <summary>
        /// Reads the output of a previously executed command, this must be called immediately after a writestream event
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [OperationContract]
        string ReadCommandOutputStream(string key);

        /// <summary>
        /// Write the data to the Stream either Serial or TCP
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <param name="value"></param>
        [OperationContract(Name = "WriteStreamWithByte")]
        void WriteStream(string key,byte value);

        /// <summary>
        /// Writes the command to the stream either Serial or TCP
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <param name="command"></param>
        [OperationContract(Name = "WriteStreamWithCommand")]
        void WriteStream(string key,string command);

        /// <summary>
        /// Writes the bytes array to the stream either Serial or TCP
        /// <param name="key">Device Identifier</param>
        /// <param name="data"></param>
        /// </summary>
        [OperationContract(Name = "WriteStreamWithBytes")]
        void WriteStream(string key,byte[] data);

        /// <summary>
        /// Writest the command to EFI Shell
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <param name="command"></param>
        [OperationContract]
        void WriteToEfiShell(string key,string command);

        /// <summary>
        /// Waits for the pattern or string to appear on the bash prompt
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <param name="waitTimeInMSecs">Timeout in Milliseconds</param>
        /// <param name="waitTerminatePatterns">The pattern to match</param>
        /// <param name="verbose">true or false</param>
        /// <param name="cancelFunction">cancel function if needed</param>
        /// <returns></returns>
        [OperationContract(Name = "WaitForRegexPatterns")]
        string WaitFor(string key, Int32 waitTimeInMSecs, Regex[] waitTerminatePatterns, Boolean verbose,
            Func<bool> cancelFunction);

        /// <summary>
        /// Waits for the pattern string to appear on bash log/prompt 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="waitTimeInMilliSeconds"></param>
        /// <param name="pattern"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        [OperationContract(Name = "WaitForString")]
        string WaitFor(string key, Int32 waitTimeInMilliSeconds, string pattern, bool verbose);

        /// <summary>
        /// Waits for the pattern or string to appear on the bash prompt line
        /// </summary>
        /// <param name="key">Device Identifier</param>
        /// <param name="waitTimeInMSecs"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [OperationContract]
        string WaitForLine(string key, Int32 waitTimeInMSecs, string pattern);

    }
}
