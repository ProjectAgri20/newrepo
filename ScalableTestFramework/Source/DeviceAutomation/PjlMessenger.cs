using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Messenger class for communicating with devices using PJL commands.
    /// </summary>
    public sealed class PjlMessenger : IDisposable
    {
        // Universal Exit Language command (UEL)
        private const string _uel = "\u001b%-12345X";

        private readonly Socket _socket;
        private readonly IPAddress _address;
        private readonly int _port;

        /// <summary>
        /// Initializes a new instance of the <see cref="PjlMessenger" /> class.
        /// </summary>
        /// <param name="address">The device IP address.</param>
        /// <param name="port">The port to use for PJL communication.</param>
        /// <exception cref="ArgumentNullException"><paramref name="address" /> is null.</exception>
        public PjlMessenger(IPAddress address, int port)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            _socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.ReceiveTimeout = 10000;
            _address = address;
            _port = port;
        }

        /// <summary>
        /// Opens a connection to the device.
        /// </summary>
        public void Connect()
        {
            _socket.Connect(_address, _port);
        }

        /// <summary>
        /// Sends the specified PJL job command to the device.
        /// </summary>
        /// <param name="pjlJob">The PJL job.</param>
        public void SendCommand(string pjlJob)
        {
            string command = pjlJob;
            if (!pjlJob.StartsWith(_uel))
            {
                command = _uel + pjlJob + "\n" + _uel;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(command);
            _socket.Send(bytes);
        }

        /// <summary>
        /// Sends the specified PJL job command to the device and reads the response.
        /// </summary>
        /// <param name="pjlJob">The PJL job.</param>
        /// <returns>The PJL response.</returns>
        public string SendInquire(string pjlJob)
        {
            SendCommand(pjlJob);

            byte[] buffer = new byte[256];
            int received = _socket.Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, received);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }

        #endregion
    }
}
