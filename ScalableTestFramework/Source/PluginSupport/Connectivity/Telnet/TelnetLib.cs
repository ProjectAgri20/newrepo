using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace HP.ScalableTest.PluginSupport.Connectivity.Telnet
{
    public class TelnetLib : IDisposable
    {
        private Socket _socket;
        private TcpClient _client;
        private byte[] _receiveBuffer = new byte[4096];
        private string _host;
        private int _port = 0;

        public TelnetLib(string printerIP)
        {
            _host = printerIP;
            _port = 23;
        }

        #region Properties
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Establish connection to the host
        /// </summary>
        /// <returns>true if the connection is established succesfully</returns>
        public bool Connect()
        {
            bool status = false;
            try
            {
                _client = new TcpClient(_host, _port);
                _socket = _client.Client;
                status = true;
                TraceFactory.Logger.Debug("Telnet Connected");
            }
            catch (Exception error)
            {
                TraceFactory.Logger.Debug("Telnet Connection Failed:" + error.Message);
            }
            return status;
        }

        /// <summary>
        /// Receives data until the match is true.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        public string ReceiveUntilMatch(string pattern)
        {
            return ReceiveUntilMatch(pattern, string.Empty);
        }

        /// <summary>
        /// Receives data until the match is true.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="truncatePattern">The pattern to truncate from the found string.</param>
        /// <returns>The found string.</returns>
        public string ReceiveUntilMatch(string pattern, string truncatePattern)
        {
            StringBuilder response = new StringBuilder();
            string result = string.Empty;

            int size = 0;
            while ((size = _socket.Receive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None)) > 0)
            {
                Thread.Sleep(5000);
                string newString = Encoding.ASCII.GetString(_receiveBuffer, 0, size);

                if (!string.IsNullOrEmpty(newString))
                {
                    response.Append(newString);

                    string workString = response.ToString().Trim();
                    Match match = Regex.Match(workString, pattern, RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (!string.IsNullOrEmpty(truncatePattern))
                        {
                            int index = workString.LastIndexOf(truncatePattern, StringComparison.CurrentCultureIgnoreCase);
                            result = workString.Substring(0, index);
                        }
                        else
                        {
                            result = workString;
                        }
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Receives data until the match is true.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param> 
        /// <returns>The found string</returns>
        public string ReceiveFile(string pattern)
        {
            StringBuilder response = new StringBuilder();
            string result = string.Empty;
            int count = 0;
            int size = 0;
            string newString;
            Thread.Sleep(3000);
            while ((size = _socket.Receive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None)) > 0)
            {
                Thread.Sleep(3000);
                newString = Encoding.ASCII.GetString(_receiveBuffer, 0, size);
                if (!string.IsNullOrEmpty(newString))
                {
                    response.Append(newString);
                    // Match match = Regex.Match(workString, pattern, RegexOptions.IgnoreCase);
                    string match = newString.ToString().Contains(pattern).ToString();
                    // if (match.Success)
                    if (match == "True")
                    {
                        Send(Environment.NewLine);
                        Thread.Sleep(1000);
                        count++;
                    }
                    else
                    {
                        //result = response.ToString();
                        break;
                    }
                }

            }
            //result.Replace("Press RETURN to continue:\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b", "");
            response.Replace("Press RETURN to continue:\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b\b \b", "");
            using (StreamWriter telnetOut = new StreamWriter(@"C:\TelnetLog.txt"))
            {
                telnetOut.Write(response.ToString());
            }
            result = response.ToString();
            return result;
        }

        /// <summary>
        /// Sends the data, appending a newline.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SendLine(string data)
        {
            Send(data);
            Send(Environment.NewLine);
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(string data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            if (byteData.Length > 0)
            {
                Send(byteData);
            }
        }

        /// <summary>
        /// Sends the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void Send(byte[] data)
        {
            if (data.Length <= 0)
            {
                //Logger.WriteDebug(CALLINGTYPE, "Send2", "An empty data array is trying to be sent");
            }
            else
            {
                try
                {
                    _socket.Send(data);
                }
                catch (SocketException)
                {
                    //Logger.WriteError(CALLINGTYPE, "Send2", "Data not sent through socket, error: " + exception.Message);
                }
            }
        }

        public string getValue(string input)
        {
            string output = string.Empty;
            using (StreamReader telnetIn = new StreamReader(@"C:\TelnetLog.txt"))
            {
                string line;

                while ((line = telnetIn.ReadLine()) != null)
                {
                    //if(Regex.IsMatch(line,input,RegexOptions.ExplicitCapture))
                    if (line.Contains(input))
                    {
                        string[] splitData = line.Split(':');
                        output += splitData[1].Trim() + " ";
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Terminates the telnet connection
        /// </summary>
        public void Close()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Close();
            }

            if (_client != null && _client != null)
            {
                _client.Close();
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_socket != null && _socket.Connected)
            {
                _socket.Close();
            }

            if (_client != null && _client != null)
            {
                _client.Close();
            }
        }

        #endregion
    }
}
