using HP.ScalableTest.Framework;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace HP.ScalableTest.Plugin.BashLogger
{
    /// <summary>
    /// Library of low level functionalities that Read/Write and Parse NormalizedData from a device
    /// </summary>

    //public class LowLevelParserLib : IParser, IDisposable

    public class LowLevelParserLib : IParser, IDisposable
    {
        /*
                private static readonly string ClassName = typeof(LowLevelParserLib).Name;
        */

        private readonly string _comPort;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private static readonly string ClassName = typeof(LowLevelParserLib).Name;
        private TcpClient _tcpClientInstance;
        private NetworkStream _networkStreamInstance;
        private SerialPort _serialPort;

        private string _serialData;

        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        private ISystemTrace PreBootLogger => ExecutionServices.SystemTrace;

        #region Constructor

        /// <summary>
        /// LLPaserLib constructor initialize private fields as well as establishes a TcpClient instance connection
        /// (which includes allocating a socket) and constructs a NetworkStream instance and associates it with the
        /// aforementioned TcpClient instance. Both instances must be closed and/or disposed of by calling Close()
        /// method of the LLParseLib class instance.
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="deviceName"></param>
        public LowLevelParserLib(string comPort, string deviceName)
        {
            _comPort = comPort;
            _hostName = deviceName;
            Open();
        }

        public LowLevelParserLib(IPAddress address, int port)
        {
            _ipAddress = address;
            _port = port;
            Open(_ipAddress, _port);
        }

        public LowLevelParserLib()
        {
        }

        #endregion Constructor

        /// <summary>
        /// Null character
        /// </summary>
        public static readonly Byte bNUL = 0x00;

        /// <summary>
        /// Escape character
        /// </summary>
        public static readonly Byte bESC = 0x1b;

        /// <summary>
        /// End of Text - ctrl-c character
        /// </summary>
        public static readonly Byte bETX = 0x03;

        /// <summary>
        /// Acknowledge - ctrl-f character
        /// </summary>
        public static readonly Byte bACK = 0x06;

        /// <summary>
        /// Carriage Return character
        /// </summary>
        public static readonly Byte bCR = 0x0D;

        /// <summary>
        /// New Line character
        /// </summary>
        public static readonly Byte bLF = 0x0A;

        /// <summary>
        /// Back Space character
        /// </summary>
        public static readonly Byte bBS = 0x08;

        /// <summary>
        /// Space bar character
        /// </summary>
        public static readonly Byte bSP = 0x20;

        /// <summary>
        /// F2 character
        /// </summary>
        public static readonly Byte bF2 = 0x3c;

        /// <summary>
        ///F10, Save character
        ///</summary>
        public static readonly Byte bF10 = 0x5b;

        #region Public Instance Properties

        // ASCII to HEX table http://www.asciitable.com

        /// <summary>
        /// The default EFI shell timeout
        /// </summary>
        public int DefaultTimeout { get { return DefaultTimeoutInMsecs; } }

        /// <summary>
        /// Returns whether or not data is available on the underlying NetworStream instance.
        /// Property catches exceptions and instead returns false.
        /// </summary>
        public Boolean DataAvailable
        {
            get
            {
                try
                {
                    return _serialPort.BytesToRead > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Port Number
        /// </summary>
        public string PortNum
        {
            get { return _comPort; }
        }

        /// <summary>
        /// Name of the device connected (Emulator | formatter | etc.)
        /// </summary>
        public string HostName
        {
            get { return _hostName; }
        }

        /// <summary>
        /// Read/Write timeout for the socket
        /// </summary>
        public int SocketTimeout { get; set; } = 180000;

        #endregion Public Instance Properties

        /// <summary>
        /// Opens a TcpClient socket with a Networkstream wrapper
        /// </summary>
        /// <param name="ipAddress">Opens connection to this IP address</param>
        /// <param name="port">Opens connection to this port</param>
        public void Open(IPAddress ipAddress, Int32 port)
        {
            PreBootLogger.LogDebug("Connecting to " + ipAddress + ':' + port);
            _tcpClientInstance = new TcpClient();

            TimeSpan timeout = TimeSpan.FromMinutes(6);
            TimeSpan retrySleep = TimeSpan.FromSeconds(10);
            Stopwatch timer = new Stopwatch();
            int attempts = 0;
            timer.Start();
            do
            {
                try
                {
                    attempts++;
                    _tcpClientInstance.Connect(ipAddress, port);
                    _networkStreamInstance = new NetworkStream(_tcpClientInstance.Client) { ReadTimeout = 180000 };
                }
                catch (Exception ex)
                {
                    if (timer.Elapsed < timeout)
                    {
                        //Log as a warning, then wait a bit before retrying.
                        PreBootLogger.LogWarn($"TcpClient instance unable to connect after attempt {attempts}: {ex.Message}");
                        Thread.Sleep(retrySleep);
                    }
                    else
                    {
                        //We hit our timeout. Log as an error and give up.
                        string message =
                            $"Unable to connect to {_hostName} on port {port} after {timer.Elapsed:c} ({attempts} attempts).";
                        PreBootLogger.LogError(message + "\n\n" + ex);
                        this.Close();
                        throw new TimeoutException(message, ex);
                    }
                }
            }
            while (!_tcpClientInstance.Connected);

            //If we got out here, the TCP client connected.
            ClearStream();
            PreBootLogger.LogDebug($"Connection established with {ipAddress}:{port}.");
        }

        public void Open()
        {
            _serialPort = new SerialPort(_comPort, 115200);
            _serialPort.Open();
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        private void _serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            _serialData = serialPort?.ReadExisting();

            SerialDataReceivedEventArgs args = new SerialDataReceivedEventArgs
            {
                SerialData = _serialData,
                TimeReceived = DateTime.Now
            };
            OnDataReceived(args);
        }

        protected virtual void OnDataReceived(SerialDataReceivedEventArgs e)
        {
            EventHandler<SerialDataReceivedEventArgs> handler = DataReceived;
            handler?.Invoke(this, e);
        }

        public string ReadSerialData()
        {
            return !string.IsNullOrEmpty(_serialData) ? _serialData : ReadStream();
        }

        /// <summary>
        /// Read from a NetworkStream instance most likely associated with a Telnet TcpClient session.
        /// </summary>
        /// <returns>A string of contents read from the stream</returns>
        public string ReadStream()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                return ReadSerialStream();

            return ReadTcpStream();
        }

        private string ReadSerialStream()
        {
            StringBuilder readAsciiContent = new StringBuilder();

            try
            {
                if (_serialPort.IsOpen)
                {
                    var buffer = new byte[4096];
                    while (_serialPort.BytesToRead > 0)
                    {
                        int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);
                        readAsciiContent.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    }
                }
                else
                    throw new ApplicationException("Serial Port instance not connected or not readable.");
            }
            catch (Exception e)
            {
                String ErrMsg = "Unable to read the stream. Check if socket is connected.";
                //PrebootLogger.Error("LLParser", "ReadStream()",  ErrMsg + " Throwing exception.");
                throw new ParserException(ErrMsg, e);
            }
            if (0 != readAsciiContent.Length)
            {
            }
            //PrebootLogger.Debug(CLASS_NAME, "ReadStream()", "Called/Exited after reading " + ReadASCIIContent.Length + " bytes.");

            return readAsciiContent.ToString();
        }

        private string ReadTcpStream()
        {
            StringBuilder readAsciiContent = new StringBuilder();

            try
            {
                if (_tcpClientInstance.Connected && _networkStreamInstance.CanRead)
                {
                    var buffer = new byte[4096];
                    while (_networkStreamInstance.DataAvailable)
                    {
                        int bytesRead = _networkStreamInstance.Read(buffer, 0, buffer.Length);
                        readAsciiContent.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    }
                }
                else
                    throw new ApplicationException("TcpClient instance not connected or NetworkStream instance not readable.");
            }
            catch
            {
                String ErrMsg = "Unable to read the stream. Check if socket is connected.";
                PreBootLogger.LogError($"ReadStream: {ErrMsg}. Throwing exception.");
                throw new ParserException(ErrMsg);
            }
            if (0 != readAsciiContent.Length)
                PreBootLogger.LogDebug($"ReadStream: Called/Exited after reading {readAsciiContent.Length} bytes.");

            return (readAsciiContent.ToString());
        }

        public string ReadBufferedStream()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                return ReadSerialBufferedStream();

            return ReadTcpBufferedStream();
        }

        /// <summary>
        /// Read from stream the lenghth of the specified buffer
        /// </summary>
        /// <returns>A string of contents read from the stream</returns>
        private string ReadSerialBufferedStream()
        {
            //PrebootLogger.Debug(CLASS_NAME, "ReadBufferedStream", "Reading buffer stream...");
            StringBuilder content = new StringBuilder();
            if (_serialPort.IsOpen && _serialPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[1000];
                int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);
                content.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "ReadBufferedStream()", "Unable to read the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to read the stream. Check if serial port is connected");
            }
            return content.ToString();
            //  return Normalize(content);
        }

        private string ReadTcpBufferedStream()
        {
            PreBootLogger.LogDebug("ReadBufferedStream: Reading buffer stream...");
            StringBuilder content = new StringBuilder();
            if (_tcpClientInstance.Connected && _networkStreamInstance.CanRead)
            {
                byte[] Buffer = new byte[1000];
                if (_networkStreamInstance.DataAvailable)
                {
                    int bytesRead = _networkStreamInstance.Read(Buffer, 0, Buffer.Length);
                    content.Append(Encoding.ASCII.GetString(Buffer, 0, bytesRead));
                    //Console.WriteLine("bytesRead = {0},  {1}", bytesRead, Normalize(content));
                }
            }
            else
            {
                PreBootLogger.LogError("ReadBufferedStream: Unable to read the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to read the stream. Check if socket is connected");
            }
            return content.ToString();
            //  return Normalize(content);
        }

        /// <summary>
        /// Write to a stream
        /// </summary>
        ///<param name="data">binary data to send to the stream</param>
        public void WriteStream(byte[] data)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                WriteSerialStream(data);
            else
                WriteTcpStream(data);
        }

        private void WriteSerialStream(byte[] data)
        {
            //PrebootLogger.Debug("LLParser", "WriteStream", "Writing " + data.Length + " bytes of data to stream...");
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(data, 0, data.Length);
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "WriteStream(byte[])", "Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if serial port is connected");
            }
        }

        private void WriteTcpStream(byte[] data)
        {
            PreBootLogger.LogDebug($"WriteStream: Writing {data.Length} bytes of data to stream...");
            if (_tcpClientInstance.Connected && _networkStreamInstance.CanWrite)
            {
                _networkStreamInstance.Write(data, 0, data.Length);
            }
            else
            {
                PreBootLogger.LogError("WriteStream: Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if socket is connected");
            }
        }

        /// <summary>
        /// Write to a stream
        /// </summary>
        /// <param name="command">the string to write to a stream</param>
        public void WriteStream(string command)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                WriteSerialStream(command);
            else
                WriteTcpStream(command);
        }

       
        public void WriteSerialStream(string command)
        {
            //PrebootLogger.Debug("LLParser", "WriteStream", "Writing \"" + command + "\" to stream...");
            //if (_TcpClientInstance.Connected && _NetworkStreamInstance.CanWrite)
            if (_serialPort.IsOpen)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                _serialPort.Write(buffer, 0, buffer.Length);
                WriteStream(bCR);
                WriteStream(bLF);
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "WriteStream(string)", "Unable to send " + command + ". Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if serial port is connected");
            }
        }

        public void WriteTcpStream(string command)
        {
            PreBootLogger.LogDebug($"WriteStream: Writing {command} to stream...");
            if (_tcpClientInstance.Connected && _networkStreamInstance.CanWrite)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                _networkStreamInstance.Write(buffer, 0, buffer.Length);
                WriteStream(bCR);
                WriteStream(bLF);
            }
            else
            {
                PreBootLogger.LogError($"WriteStream: Unable to send {command}. Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if socket is connected");
            }
        }

        /// <summary>
        /// Write to a stream
        /// </summary>
        /// <param name="value">the byte to write to a stream</param>
        /// 
        public void WriteStream(byte value)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                WriteSerialStream(value);
            else
                WriteTcpStream(value);

        }
        private void WriteSerialStream(byte value)
        {
            ////PrebootLogger.Debug("LLParser", "WriteStream", "Writing byte \"" + value + "\" to stream...");
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(new[] { value }, 0, 1);
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "WriteStream", "Unable to write byte to the sream. Check if socket is connected. Threw an exception.");
                throw new ParserException("Unable to write to the stream. Check if serial port is connected");
            }
        }

        private void WriteTcpStream(byte value)
        {
            //PrebootLogger.Debug("LLParser", "WriteStream", "Writing byte \"" + value + "\" to stream...");
            if (_tcpClientInstance.Connected && _networkStreamInstance.CanWrite)
            {
                _networkStreamInstance.WriteByte(value);
            }
            else
            {
                PreBootLogger.LogError("WriteStream: Unable to write byte to the sream. Check if socket is connected. Threw an exception.");
                throw new ParserException("Unable to write to the stream. Check if socket is connected");
            }
        }

        /// <summary>
        /// Write to a stream
        /// </summary>
        /// <param name="command">the string to write to a stream</param>
        /// 
        public void WriteToEfiShell(string command)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                WriteToSerialEfiShell(command);
            else
                WriteToTcpEfiShell(command);

        }
        private void WriteToSerialEfiShell(string command)
        {
            //PrebootLogger.Debug(CLASS_NAME, "WriteToEfiShell", "Writing \"" + command + "\" to efi shell...");
            //if (_TcpClientInstance.Connected && _NetworkStreamInstance.CanWrite)
            if (_serialPort.IsOpen)
            {
                //clear any data that might be on the stream
                ClearStream();

                byte[] buffer = Encoding.ASCII.GetBytes(command);
                _serialPort.Write(buffer, 0, buffer.Length);
                _StringWaitFor(command);
                WriteStream(bCR);
                WriteStream(bLF);
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "WriteToEfiShell", "Unable to send " + command + ". Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if socket is connected");
            }
        }

        private void WriteToTcpEfiShell(string command)
        {
            PreBootLogger.LogDebug("WriteToEfiShell: Writing \"" + command + "\" to efi shell...");
            if (_tcpClientInstance.Connected && _networkStreamInstance.CanWrite)
            {
                //clear any data that might be on the stream
                ClearStream();

                byte[] buffer = Encoding.ASCII.GetBytes(command);
                _networkStreamInstance.Write(buffer, 0, buffer.Length);
                _StringWaitFor(command);
                WriteStream(bCR);
                WriteStream(bLF);
            }
            else
            {
                PreBootLogger.LogError("WriteToEfiShell: Unable to send " + command + ". Unable to write to the stream. Check if socket is connected. Threw an exception");
                throw new ParserException("Unable to write to the stream. Check if socket is connected");
            }
        }

        /// <summary>
        /// Clear the stream
        /// </summary>
        public void ClearStream()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                ClearSerialStream();
            else
               ClearTcpStream();

        }
        private void ClearSerialStream()
        {
            ////PrebootLogger.Debug(CLASS_NAME, "ClearStream", "Clearing the stream...");
            if (_serialPort.IsOpen)
            {
                byte[] buf = new byte[256];

                for (int i = 0; i < 2; i++)
                {
                    //Sleep so the socket can collect all the data to be cleared.
                    Thread.Sleep(5);

                    while (_serialPort.BytesToRead > 0)
                    {
                        _serialPort.Read(buf, 0, buf.Length);
                    }
                }
            }
            else
            {
                //PrebootLogger.Error(CLASS_NAME, "ClearStream()", "TcpClient instance connection not established.");
                throw new ParserException("Serial Port instance connection not established.");
            }
        }

        private void ClearTcpStream()
        {
            //PrebootLogger.Debug(CLASS_NAME, "ClearStream", "Clearing the stream...");
            if (_tcpClientInstance.Connected)
            {
                byte[] buf = new byte[256];

                for (int i = 0; i < 2; i++)
                {
                    //Sleep so the socket can collect all the data to be cleared.
                    Thread.Sleep(5);

                    while (_networkStreamInstance.DataAvailable)
                    {
                        _networkStreamInstance.Read(buf, 0, buf.Length);
                    }
                }
            }
            else
            {
                PreBootLogger.LogError("ClearStream(): TcpClient instance connection not established.");
                throw new ParserException("TcpClient instance connection not established.");
            }
        }

        /// <summary>
        /// Closes the socket connection.
        /// </summary>

        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                CloseSerial();
            else
                CloseTcp();

        }
        private void CloseSerial()
        {
            //PrebootLogger.Debug(CLASS_NAME, "Close()", "Called. Closing NetworkStream and TcpClient instances, \"" + _HostIP + "\":\"" + _PortNum + "\"");

            try
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }
            catch
            {
                // ignored
            }

            _hostName = string.Empty;
        }

        private void CloseTcp()
        {
            PreBootLogger.LogDebug($"Close: Called. Closing NetworkStream and TcpClient instances, {_ipAddress}: {_port}");
            try
            {
                if (null != _networkStreamInstance)
                {
                    _networkStreamInstance.Close();
                    _networkStreamInstance.Dispose();
                    _networkStreamInstance = null;
                }
            }
            catch
            {
                // ignored
            }

            try
            {
                if (_tcpClientInstance?.Client != null)
                {
                    _tcpClientInstance.Client.Shutdown(SocketShutdown.Both);
                    _tcpClientInstance.Client.Close();
                    _tcpClientInstance.Client = null;
                }
            }
            catch
            {
                // ignored
            }

            try
            {
                if (null != _tcpClientInstance)
                {
                    _tcpClientInstance.Close();
                    _tcpClientInstance = null;
                }
            }
            catch
            {
                // ignored
            }
           

        }

        /// <summary>
        /// Sends data across a raw socket connection
        /// </summary>
        public void WriteSocket(byte[] buffer)
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Write(buffer, 0, buffer.Length);
            else
            {
                _tcpClientInstance.Client.Send(buffer);
            }
        }

        /// <summary>
        /// Cleanse EFI shell output by removing all control characters.
        /// </summary>
        /// <param name="message">The message to normalize</param>
        /// <returns>The cleansed output</returns>
        public string Normalize(string message)
        {
            // EFI output is formatted with ANSI CSI codes.
            // See: http://en.wikipedia.org/wiki/ANSI_escape_code

            if (String.IsNullOrEmpty(message)) return String.Empty;

            // remove control sequences
            Regex ctrPattern = new Regex(@"(\x1b\[[\d;=?]*[ABCDEFGHJKSTfmnsulh]{1})|\u0000");
            string outputString = ctrPattern.Replace(message, "");

            // fix-up new lines
            //Regex newLine = new Regex(@"((?!\u000D)\u000A)|\u000D(?!\u000A)");
            //OutputString = newLine.Replace(OutputString, "\r\n");

            return outputString;
        }

        /// <summary>
        /// Wrapper for main WaitFor(Int32, Regex[], Boolean) method.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(string pattern)
        {
            return WaitFor(DefaultTimeoutInMsecs, new[] { new Regex(pattern) }, true, null);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until the pattern is matched or alloted time expires or the default time span expires
        /// </summary>
        /// <param name="pattern">Wait until pattern is matched</param>
        /// <param name="verbose">Ignore logging the matched output if set to false</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(string pattern, bool verbose)
        {
            return WaitFor(DefaultTimeoutInMsecs, new[] { new Regex(pattern) }, verbose, null);
        }

        /// <summary>
        /// Received data from connection (EFI/CE) until the pattern is matched or alloted time expires
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="pattern">Wait until pattern is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(int milliseconds, string pattern)
        {
            return WaitFor(milliseconds, new[] { new Regex(pattern) }, true, null);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until the pattern is matched or alloted time expires
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="pattern">Wait until pattern is matched</param>
        ///  <param name="verbose">Ignore logging the matched output if set to false</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(int milliseconds, string pattern, bool verbose)
        {
            return WaitFor(milliseconds, new[] { new Regex(pattern) }, verbose, null);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires
        /// </summary>
        /// <param name="waitTimeInMSecs">Wait until this timeout is reached in milliseconds</param>
        /// <param name="patterns">Wait until one of the given patterns is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(int waitTimeInMSecs, Regex[] patterns)
        {
            return WaitFor(waitTimeInMSecs, patterns, true, null);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires
        /// </summary>
        /// <param name="waitTimeInMSecs">Wait until this timeout is reached in milliseconds</param>
        /// <param name="waitTerminatePatterns">Wait until one of the given patterns is matched</param>
        /// <param name="verbose">Ignore logging the matched output if set to false</param>
        /// <param name="cancelFunction">If null, this has no effect. If supplied, this function will be used to determine if the wait should be cancelled.
        /// The function MUST return quickly - any blocking will hold up the main bash polling loop. If the function returns false, the loop continues as
        /// it normally would without cancelFunction. If it returns true, WaitFor will immediately return null.</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitFor(Int32 waitTimeInMSecs, Regex[] waitTerminatePatterns, Boolean verbose, Func<bool> cancelFunction)
        {
            const Int32 logStrTruncateLength = 250;
            StringBuilder accumReadBuf = new StringBuilder();
            String readBufSearchString = string.Empty;
            String waitTerminatePatternsStr = string.Empty;

            foreach (Regex waitTerminatePattern in waitTerminatePatterns)
            {
                waitTerminatePatternsStr += "'" + waitTerminatePattern + "'";
                if (waitTerminatePattern != waitTerminatePatterns.Last())
                    waitTerminatePatternsStr += " or ";
            }
            //PrebootLogger.Debug(CLASS_NAME, "WaitFor", "Waiting for patterns [" + WaitTerminatePatternsStr + "] with timeout value of " + waitTimeInMSecs + "ms.");

            DateTime startTime = DateTime.Now;
            while (DateTime.Now < startTime.AddMilliseconds(waitTimeInMSecs))
            {
                //This function should return quickly! Waiting too long here could mean we don't read from the buffer often enough, and could miss data.
                if (cancelFunction != null && cancelFunction.Invoke())
                {
                    //PrebootLogger.Debug(CLASS_NAME, "WaitFor", "Wait was canceled - returning null.");
                    return null;
                }

                String readBuf = ReadStream();
                if (!String.IsNullOrEmpty(readBuf))
                    readBufSearchString += Normalize(readBuf);
                else
                {
                    Thread.Sleep(250);
                    continue;
                }

                foreach (Regex waitTerminatePattern in waitTerminatePatterns)
                {
                    if (waitTerminatePattern.IsMatch(readBufSearchString))
                    {
                        if (verbose)
                        { }    //PrebootLogger.Debug(CLASS_NAME, "WaitFor", "Match found. Last ReadBufSearchString = \"" +Environment.NewLine +ReadBufSearchString + "\" Exited.");
                        else
                        { }   //PrebootLogger.Debug(CLASS_NAME, "WaitFor", "Match found.");
                        return (accumReadBuf + readBufSearchString);
                    }
                }

                if (readBufSearchString.Length > 2 * logStrTruncateLength)
                {
                    accumReadBuf.Append(readBufSearchString.Substring(0, (readBufSearchString.Length - logStrTruncateLength)));
                    readBufSearchString = readBufSearchString.Substring(readBufSearchString.Length - logStrTruncateLength);
                }
            }

            String errMsg = "Unable to find: " + waitTerminatePatternsStr + " after " + waitTimeInMSecs + " milliseconds. Last output:" + Environment.NewLine +
                                (accumReadBuf + readBufSearchString);
            //PrebootLogger.Error(CLASS_NAME, "WaitFor", ErrMsg);
            throw new ParserTimeoutException(errMsg);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires
        /// </summary>
        /// <param name="milliseconds">Wait until this timeout is reached in milliseconds</param>
        /// <param name="patterns">Wait until one of the given patterns is matched</param>
        /// <returns>Returns matched value or throws timeout exception</returns>
        public string WaitForShell(int milliseconds, Regex[] patterns)
        {
            string output = string.Empty;
            string patternsString = string.Empty;
            string lastOutput = string.Empty;
            const int strCapacity = 5000;
            const int trimLength = 10;

            foreach (Regex pattern in patterns)
            {
                patternsString += "'" + pattern + "'";
                if (pattern != patterns.Last()) patternsString += " or ";
            }

            //PrebootLogger.Debug(CLASS_NAME, "WaitForShell", "Waiting for pattern [" + PatternsString + "] with timeout value of " + milliseconds + "ms.");

            DateTime startTime = DateTime.Now;
            while (DateTime.Now < startTime.AddMilliseconds(milliseconds))
            {
                output += ReadBufferedStream();
                output = Normalize(output);
                foreach (Regex pattern in patterns)
                {
                    if (pattern.IsMatch(output))
                    {
                        lastOutput += output;

                        //PrebootLogger.Debug(CLASS_NAME, "WaitForShell", "**WaitTerminatePattern match found. Last AccumReadBuf = " + LastOutput);
                        return lastOutput;
                    }
                }
                //Give the socket enough time to retrieve data.
                Thread.Sleep(100);

                //store output in a StringBuilder object and reduce the size of the comparison string output
                if (output.Length >= strCapacity)
                {
                    lastOutput += output.Substring(0, output.Length - trimLength);
                    output = output.Remove(0, output.Length - trimLength);
                }
            }

            String errMsg = "Unable to find: " + patternsString + " after " + milliseconds + " milliseconds. Last output:\n" + Normalize(lastOutput);
            //PrebootLogger.Error(CLASS_NAME, "WaitForShell", ErrMsg);
            throw new ParserTimeoutException(errMsg);
        }

        /// <summary>
        /// Receive data from connection (EFI/CE) until one of the patterns is matched or alloted time expires and returns
        /// the line containing the matched pattern;
        /// Throws TimeOutExpection if pattern is not found and an Exception if the line containing pattern can't be found
        /// </summary>
        /// <param name="pattern">Wait until one of the given patterns is matched</param>
        /// <returns>Returns a line that contains the matched value or throws timeout exception</returns>
        public string WaitForLine(string pattern)
        {
            return WaitForLine(DefaultTimeoutInMsecs, pattern);
        }

        /// <summary>
        /// Received data from connection (EFI/CE) until one of the patterns is matched or alloted time expires and returns
        /// the line containing the matched pattern;
        /// Throws TimeOutExpection if pattern is not found and an Exception if the line containing pattern can't be found
        /// </summary>
        /// <param name="waitTimeInMSecs">Wait until this timeout is reached in milliseconds</param>
        /// <param name="pattern">Wait until one of the given patterns is matched</param>
        /// <returns>Returns a line that contains the matched value or throws timeout exception</returns>
        public string WaitForLine(Int32 waitTimeInMSecs, String pattern)
        {
            //PrebootLogger.Debug(CLASS_NAME, "WaitForLine(Int32, String)", "Called. Waiting for a line that includes the passed pattern '" + pattern + "' with timeout value of " + waitTimeInMSecs + "ms.");

            DateTime startTime = DateTime.Now;
            StringBuilder stringLine = new StringBuilder();
            Byte[] input = new byte[1];

            while (DateTime.Now < startTime.AddMilliseconds(waitTimeInMSecs))
            {
                if (_serialPort.IsOpen && _serialPort.BytesToRead > 0)
                {
                    while (_serialPort.BytesToRead > 0)
                    {
                        _serialPort.Read(input, 0, input.Length);
                        stringLine.Append(Encoding.ASCII.GetString(input));

                        if (bLF == input[0])
                        {
                            var data = stringLine.ToString();
                            if (data.Contains(pattern))
                            {
                                string normalizedReadData = Normalize(data);
                                //PrebootLogger.Debug(CLASS_NAME, "WaitForLine", "Found pattern match with: \"" + pattern + "\" in " + NormalizedReadData);
                                return normalizedReadData;
                            }
                            stringLine.Remove(0, stringLine.Length);
                        }
                    }
                }
            }
            String error = "Unable to find the line containing \'" + pattern + "\' after " + waitTimeInMSecs + " milliseconds.";
            //PrebootLogger.Error(CLASS_NAME, "WaitForLine", Error);
            throw new ParserTimeoutException(error);
        }

        /// <summary>
        /// Find matches in Serial output until pattern is matched or timeout
        /// </summary>
        /// <param name="find">Get all matches to this regex pattern</param>
        /// <param name="until">Stop when this regex is found</param>
        /// <param name="milliseconds">Timeout after period in milliseconds</param>
        /// <returns>Returns match collection or throws timeout exception</returns>
        public MatchCollection FindUntil(string find, string until, int milliseconds)
        {
            //PrebootLogger.Debug(CLASS_NAME, "FindUntil", string.Format("Finding matches of pattern [{0}] until pattern [{1}] is found...", find, until));
            // Set the read timeout property
            DateTime timeExpired = DateTime.Now.AddMilliseconds(milliseconds);
            Regex findPattern = new Regex(find);

            // Try reading all the input and using a regex to match the pattern
            string output = "";

            // FindMatches = FindPattern.Matches(output);
            while (!Regex.IsMatch(output, until))
            {
                if (DateTime.Now.CompareTo(timeExpired) > 0)
                {
                    string error = " Time expired at " + DateTime.Now + " after " + milliseconds + " milliseconds. Searching for " + until + ". Last output was:\n" + Normalize(output);
                    //PrebootLogger.Error(CLASS_NAME, "FindUntill", Error);
                    throw new ParserTimeoutException(error);
                }

                output += ReadStream();

                //Give the socket time to gather data
                Thread.Sleep(100);
            }
            var findMatches = findPattern.Matches(Normalize(output));
            return findMatches;
        }

        ///<summary>
        ///Sends a command and collects the output until a pattern match is found
        ///</summary>
        ///<param name = "command">The string command to be sent</param>
        ///<param name="pattern">The string pattern to match</param>
        public string SendWait(string command, string pattern)
        {
            return SendWait(command, pattern, DefaultTimeout);
        }

        ///<summary>
        ///Sends a command and collects the output until a pattern match is found or time expires
        ///</summary>
        ///<param name = "command">The string command to be sent</param>
        ///<param name="pattern">The string pattern to match</param>
        ///<param name="milliseconds">The time limit to collect the output and wait for a match</param>
        public string SendWait(string command, string pattern, int milliseconds)
        {
            WriteStream(command);
            return WaitFor(milliseconds, pattern);
        }

        /// <summary>
        /// Parses a delimited string and returns the requested filetype path.
        /// </summary>
        /// <param name="delimitedFileNames">Delimited string containing one or more file paths</param>
        /// <param name="filePath">The full path to the file to upload</param>
        /// <param name="requiredFileType">Either FileType.CSV or FileType.Bundle</param>
        public void ParseFileNames(string delimitedFileNames, out string filePath, string requiredFileType)
        {
            char[] delimiters = { ',', '|', ';' };
            filePath = string.Empty;

            // Get each filepath as a separate string
            string[] fileNames = delimitedFileNames.Split(delimiters);

            foreach (string fileName in fileNames)
            {
                if (fileName.Contains(requiredFileType))
                {
                    // Get rid of leading and trailing whitespace in filepaths
                    filePath = fileName.Trim();
                }
            }
        }

        private static String _hostName;
        private const Int32 DefaultTimeoutInMsecs = 10000;

        /// <summary>
        /// Receive NormalizedData from connection (EFI/CE) until one of the patterns is matched or alloted time expires and returns
        /// the line containing the matched pattern;
        /// Throws TimeOutExpection if pattern is not found and an Exception if the line containing pattern can't be found
        /// </summary>
        /// <param name="pattern">Wait until one of the given patterns is matched</param>
        /// <returns>Returns a line that contains the matched value or throws timeout exception</returns>
        private void _StringWaitFor(string pattern)
        {
            //PrebootLogger.Debug(CLASS_NAME, "_StringWaitFor(String)", "Called.");
            const Int32 timeout = 1500;
            DateTime startTime = DateTime.Now;
            String normalizedData = String.Empty;
            String lastReadRawData = String.Empty;

            //byte[] ByteBuf = new byte[1];

            while (DateTime.Now < startTime.AddMilliseconds(timeout))
            {
                //if (_TcpClientInstance.Connected && _NetworkStreamInstance.CanRead)
                //{
                //    while (_NetworkStreamInstance.DataAvailable)
                //    {
                normalizedData += ReadStream();
                normalizedData = Normalize(normalizedData);
                if (normalizedData.Contains(pattern))
                {
                    //PrebootLogger.Debug(CLASS_NAME, "_StringWaitFor(String)","Last read: \"" + ((NormalizedData.Length > STR_TRUNCATE) ? NormalizedData.Substring(NormalizedData.Length - STR_TRUNCATE) : NormalizedData) + "\"");
                    return;
                }
                //    }
                //}
            }
            String error = "Unable to find the line containing \'" + pattern + "\' after " + timeout + " milliseconds. " +
                               "All read normalized data = \"" + normalizedData + "\",\n" +
                               "last read non-normalized data = \"" + lastReadRawData + "\"";
            //PrebootLogger.Error(CLASS_NAME, "_StringWaitFor(String)", Error);
            throw new ParserTimeoutException(error);
        }

        #region IDisposable Members

        /// <summary>
        /// Cleanup method to dispose the class members.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        public void DeviceCanConnect(IPAddress ipAddress, int port)
        {
            DateTime start = DateTime.Now;
            DateTime finish = start.AddMinutes(10);
            using (TcpClient tcpClientInstance = new TcpClient())
            {
                do
                {
                    if (PingDevice(ipAddress, 2000))
                    {
                        try
                        {
                            // Validate we can connect
                            tcpClientInstance.Connect(ipAddress, port);
                            if (tcpClientInstance.Connected)
                            {
                                PreBootLogger.LogDebug($"{ClassName}: DeviceCanConnect: Connection succeeded to " + ipAddress + ":" + port);
                                return;
                            }
                            PreBootLogger.LogDebug($"{ClassName}: DeviceCanConnect: Connection failed to " + ipAddress + ":" + port);
                        }
                        //To Do: catch a specific exception
                        catch (Exception e)
                        {
                            PreBootLogger.LogError($"{ClassName}: DeviceCanConnect: Failed to connect to " + ipAddress + ":" + port + " Error: " + e.Message);
                        }
                        Thread.Sleep(5000);
                    }
                } while (DateTime.Now < finish);
            }//end: using (TcpClient TcpClientInstance = new System.Net.Sockets.TcpClient())
            PreBootLogger.LogError($"{ClassName}: DeviceCanConnect(string, int): Unable to verify " + ipAddress + " is connected after 10 minutes. Threw an Exception");
            throw new ParserException("Unable to verify " + ipAddress + " is connected after 10 minutes.");
        }

        public Boolean PingDevice(IPAddress ipAddress, int timeMs)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(ipAddress, timeMs);

            if (reply?.Status == IPStatus.Success)
            {
                PreBootLogger.LogDebug($"{ClassName}: pingDevice: Pinging the Emulator for " + timeMs + " milliseconds. Status = " + reply.Status);
                return true;
            }
            PreBootLogger.LogDebug($"{ClassName}: pingDevice: Pinging the Emulator for " + timeMs + " milliseconds. Status = " + reply?.Status);
            return false;
        }

        #endregion IDisposable Members
    }//end: public class LLParserLib

    #region Exception Classes

    /// <summary>
    /// Parser exception class.
    /// </summary>
    [Serializable]
    public class ParserException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ParserException()
        { }

        /// <summary>
        /// Constructor with string
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParserException(string message) : base(message) { }

        /// <summary>
        /// constructor with string and excpetion
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public ParserException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// constructor with serialization info and streaming context
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected ParserException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Parser timeout exception class.
    /// </summary>
    [Serializable]
    public class ParserTimeoutException : TimeoutException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ParserTimeoutException()
        { }

        /// <summary>
        /// Constructor with string
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParserTimeoutException(string message) : base(message) { }

        /// <summary>
        /// constructor with string and excpetion
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public ParserTimeoutException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// constructor with serialization info and streaming context
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected ParserTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    #endregion Exception Classes

    public class SerialDataReceivedEventArgs : EventArgs
    {
        public string SerialData { get; set; }
        public DateTime TimeReceived { get; set; }
    }
}//end: namespace HP.Test.LowLevel.LLPreboot.LLParser