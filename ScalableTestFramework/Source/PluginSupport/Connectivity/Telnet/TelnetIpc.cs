using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.Connectivity.Telnet
{
    /// <summary>
    /// Telnet class that supports retrieving data using regular expressions.
    /// </summary>
    public sealed class TelnetIpc : IDisposable
    {
        private Socket _socket;
        private TcpClient _client;
        private byte[] _receiveBuffer = new byte[4096];
        private string _host;
        private int _port = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetIpc"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public TelnetIpc(string host, int port)
        {
            _host = host;
            _port = port;
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            _client = new TcpClient(_host, _port);
            _socket = _client.Client;
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// Gets or sets the receive timeout.
        /// </summary>
        /// <value>
        /// The receive timeout.
        /// </value>
        public TimeSpan ReceiveTimeout
        {
            get { return TimeSpan.FromMilliseconds(_socket.ReceiveTimeout); }
            set { _socket.ReceiveTimeout = (int)value.TotalMilliseconds; }
        }

        /// <summary>
        /// Receives data until the match is true.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns></returns>
        public string ReceiveUntilMatch(string pattern)
        {
            return ReceiveUntilMatch(pattern, string.Empty);
        }

        /// <summary>
        /// Receives data until the match is true.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="truncatePattern">The pattern to truncate from the found string.</param>
        /// <returns></returns>
        public string ReceiveUntilMatch(string pattern, string truncatePattern)
        {
            StringBuilder response = new StringBuilder();
            string result = string.Empty;

            int size = 0;
            Logger.LogDebug("Waiting for response.");
            while ((size = _socket.Receive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None)) > 0)
            {
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
                            int index = workString.LastIndexOf(truncatePattern, StringComparison.OrdinalIgnoreCase);
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
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Length <= 0)
            {
                throw new ArgumentException("Cannot send an empty data array.");
            }

            _socket.Send(data);
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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
