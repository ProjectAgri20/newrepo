using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Telnet class that supports retrieving data using regular expressions.
    /// </summary>
    public sealed class Telnet : IDisposable
    {
        private readonly TcpClient _tcp;
        private readonly byte[] _receiveBuffer = new byte[4096];

        /// <summary>
        /// Gets the address this <see cref="Telnet" /> object will connect to.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Gets the port this <see cref="Telnet" /> object will use.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Telnet"/> class.
        /// </summary>
        /// <param name="address">The address to connect to.</param>
        /// <param name="port">The port to use.</param>
        public Telnet(string address, int port)
        {
            Address = address;
            Port = port;
            _tcp = new TcpClient(Address, Port);
        }

        /// <summary>
        /// Gets or sets the receive timeout.
        /// </summary>
        public TimeSpan ReceiveTimeout
        {
            get { return TimeSpan.FromMilliseconds(_tcp.Client.ReceiveTimeout); }
            set { _tcp.Client.ReceiveTimeout = (int)value.TotalMilliseconds; }
        }

        /// <summary>
        /// Receives data until a string matching the specified pattern is found.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>The matching string.</returns>
        public string ReceiveUntilMatch(string pattern)
        {
            return ReceiveUntilMatch(pattern, string.Empty);
        }

        /// <summary>
        /// Receives data until a string matching the specified pattern is found.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="truncatePattern">The pattern to truncate from the matching string before returning.</param>
        /// <returns>The matching string, truncated based on the specified pattern.</returns>
        public string ReceiveUntilMatch(string pattern, string truncatePattern)
        {
            StringBuilder response = new StringBuilder();
            int size = ReceiveNext();

            while (size > 0)
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
                            return workString.Substring(0, index);
                        }
                        else
                        {
                            return workString;
                        }
                    }
                }
                size = ReceiveNext();
            }

            return string.Empty;
        }

        private int ReceiveNext()
        {
            return _tcp.Client.Receive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None);
        }

        /// <summary>
        /// Sends the specified string data, appending a newline.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        public void SendLine(string data)
        {
            Send(data);
            Send(Environment.NewLine);
        }

        /// <summary>
        /// Sends the specified string data.
        /// </summary>
        /// <param name="data">The data to send.</param>
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
        /// Sends the specified byte array.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="data" /> is an empty (zero-length) array.</exception>
        public void Send(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length == 0)
            {
                throw new ArgumentException("Cannot send an empty data array.", nameof(data));
            }

            _tcp.Client.Send(data);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_tcp != null && _tcp.Connected)
            {
                _tcp.Close();
            }
        }

        #endregion
    }
}
