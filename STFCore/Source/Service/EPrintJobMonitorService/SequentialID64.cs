using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    /// <summary>
    /// Singleton class that uses the SessionId, Server Id and an int-based record Id to
    /// </summary>
    public class SequentialID64
    {
        private static readonly SequentialID64 _instance = new SequentialID64();
        private readonly DateTime _baseDateTime = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private long _machineID;
        private Random _random;

        private SequentialID64()
        {
             _machineID = GetMachineID();
             _random = new Random(DateTime.UtcNow.Millisecond + Process.GetCurrentProcess().Id + Thread.CurrentThread.GetHashCode());
             _random.Next();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static SequentialID64 Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Returns a numeric value that represents the machine that is executing this assembly.
        /// Returns all digits in the machine name first. If there are none, returns all digits
        /// from the machine's MAC address.
        /// </summary>
        /// <returns></returns>
        public static int MachineSeed()
        {
            string numberString = new string(Environment.MachineName.Where(Char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(numberString))
            {
                var nics = NetworkInterface.GetAllNetworkInterfaces()
                                                     .Where(n => n.OperationalStatus == OperationalStatus.Up
                                                              && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                                     .OrderByDescending(n => n.Speed);
                string macAddress = nics.First().GetPhysicalAddress().ToString();
                numberString = new string(macAddress.Where(char.IsDigit).ToArray());
            }

            return int.Parse(numberString);
        }

        /// <summary>
        /// Returns an Id based on current time, machine IP and a random number.
        /// This is the most likely to be unique.
        /// </summary>
        /// <returns></returns>
        public long GetID()
        {
            long id = (this.Now << 22) + _machineID + _random.Next(256);
            System.Threading.Thread.Sleep(1);
            return id;
        }

        /// <summary>
        /// Returns an Id based on current time, the specified int value, and a random number.
        /// This is highly likely to be unique.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public long GetID(int sequence)
        {
            long id = (this.Now << 22) + ((sequence << 8) & 0x003FFFFF) + _random.Next(256);
            System.Threading.Thread.Sleep(1);
            return id;
        }

        /// <summary>
        /// Returns an Id based on a sessionId, server Id (Machine Seed), and a specified int value.
        /// Has a greater possibility of returning a non-unique value, but also will return the same
        /// Id if the same 3 parameters are supplied.  This allows updates to be made for log records
        /// that require updates to existing records.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="serverId"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public long GetID(string sessionId, int serverId, int sequence)
        {
            long sessionIdInt = Int64.Parse(sessionId, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            long id = (long)(sessionIdInt & 0x7FFFFFFC) << 32;

            id += (long)(serverId & 0xF) << 30;
            id += (sequence & 0x3FFFFFFF);

            return id;
        }

        /// <summary>
        /// Displays the parameters used to build the Ids.
        /// </summary>
        public void ShowData()
        {
            var id = DateTime.Now.ToString("ddhhmmssff", CultureInfo.InvariantCulture);
            string SessionId = Int64.Parse(id, CultureInfo.InvariantCulture).ToString("X", CultureInfo.InvariantCulture);
            Console.WriteLine("SessionId = " + SessionId);
            int SessionIdInt = int.Parse(SessionId, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            //int.TryParse(SessionId, out SessionIdInt);
            Console.WriteLine("SessionIdInt = {0:D}", SessionIdInt);

            Console.WriteLine("GetID(SessionIdInt,13,715827882) = {0:D}", this.GetID(SessionId, 13, 715827882));
            Console.WriteLine("GetID(SessionIdInt,13,715827882) >> 32 = {0:D}", this.GetID(SessionId, 13, 715827882) >> 32);
            Console.WriteLine("GetID(SessionIdInt,13,715827882) & 0x00000000FFFFFFFFL = {0:D}", this.GetID(SessionId, 13, 715827882) & 0x00000000FFFFFFFFL);

            Console.WriteLine("Process.GetCurrentProcess().Id = {0:D}", Process.GetCurrentProcess().Id);
            Console.WriteLine("Thread.CurrentThread.GetHashCode() = {0:d}", Thread.CurrentThread.GetHashCode());
            Console.WriteLine("Environment.MachineName = " + Environment.MachineName);
            Console.WriteLine("Environment.UserName = " + Environment.UserName);
            Console.WriteLine("GetLocalIP() = " + GetLocalIP() + "\n");

            Console.WriteLine("_MachineID = {0:D}\n", _machineID);

            long d = this.Now;
            Console.WriteLine("d = this.Now = {0:D}", d);
            Console.WriteLine("(d << 22)/4194304 = {0:D}", (d << 22) / 4194304);

            System.Threading.Thread.Sleep(1);
        }

        /// <summary>
        /// Gets an Id for this machine based on it's IP address.
        /// </summary>
        /// <returns></returns>
        public long GetMachineID()
        {
            string ipAddress = GetLocalIP();
            IPAddress address = IPAddress.Parse(ipAddress);

            Byte[] bytes = address.GetAddressBytes();

            if (bytes[2] > 63) bytes[2] = 63;

            long machineID = bytes[2];
            machineID = (machineID << 8) + bytes[3];

            return (machineID << 8);
        }

        private long Now
        {
            get { return (long)(DateTime.UtcNow - _baseDateTime).TotalMilliseconds; }
        }

        private string GetLocalIP()
        {
            string localIP = "";
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("10.0.0.1", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}
