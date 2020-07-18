using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HP.ScalableTest.PluginSupport.Connectivity.Discovery
{
    static class DiscoveryBase
    {
        /// <summary>
        /// Probe the device with the given network interface and raises the event when device is discovered
        /// </summary>
        /// <param name="receiverAddress">Client Network Interface IP Address</param>
        /// <param name="targetAddress">Target device address</param>
        /// <param name="probeMessage">probe message</param>
        /// <param name="port">port number</param>
        /// <returns>List of discovered devices probe match string</returns>
        public static Collection<string> ProbeDevice(IPAddress receiverAddress, IPAddress targetAddress, string probeMessage, int port)
        {
            Socket socket = null;

            Collection<string> devices = new Collection<string>();

            try
            {
                EndPoint receiverEndPoint = new IPEndPoint(receiverAddress, 0);

                // Cast PROBE packet into a byte array
                ASCIIEncoding encoder = new ASCIIEncoding();
                Byte[] probeByteArray = encoder.GetBytes(probeMessage);

                // Setup the Socket bound to a receiver (local) EndPoint.
                IPEndPoint destinationEndPoint = new IPEndPoint(targetAddress, port);
                socket = new Socket(targetAddress.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
                socket.Bind(receiverEndPoint);

                // Send the PROBE Packet.
                socket.SendTo(probeByteArray, destinationEndPoint);

                DateTime start = DateTime.Now;

                // ProbeMatch must be sent within 4 seconds of the Probe message; otherwise, Windows Firewall may drop the packet.
                // on the safer side we are waiting for the 6 seconds. 
                // DP_MAX_TIMEOUT is 5 seconds this information is taken from the Web Services Dynamic Discovery document from Microsoft.
                while (DateTime.Now.Subtract(start).Seconds <= 6)
                {
                    if (socket.Available > 0)
                    {
                        byte[] buffer = new byte[50000];
                        IPEndPoint endPoint;

                        if (targetAddress.AddressFamily == AddressFamily.InterNetwork)
                        {
                            endPoint = new IPEndPoint(IPAddress.Any, 0);
                        }
                        else
                        {
                            endPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
                        }

                        EndPoint remoteHost = (EndPoint)endPoint;

                        // Get the probe match packet through socket connection
                        socket.ReceiveFrom(buffer, ref remoteHost);

                        buffer = RemoveNull(buffer);
                        // get the probe match string
                        string probeMatchString = Encoding.UTF8.GetString(buffer);

                        devices.Add(probeMatchString);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing if any exception occurs while looking for probe match keep reading the probe match until the time out.
            }
            finally
            {
                // close the socket
                if (null != socket)
                {
                    socket.Close();
                }
            }

            return devices;
        }

        /// <summary>
        /// Before we decode XML we should cut down all nulls
        /// </summary>
        private static byte[] RemoveNull(byte[] DataStream)
        {
            TcpClient clientSocket = new TcpClient();
            int i;
            byte[] temp = new byte[clientSocket.ReceiveBufferSize];
            for (i = 0; i < clientSocket.ReceiveBufferSize - 1; i++)
            {
                if (DataStream[i] == 0x00) break;
                temp[i] = DataStream[i];
            }
            byte[] NullLessDataStream = new byte[i];
            for (i = 0; i < NullLessDataStream.Length; i++)
            {
                NullLessDataStream[i] = temp[i];
            }
            return NullLessDataStream;
        }
    }
}
