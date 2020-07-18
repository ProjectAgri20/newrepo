using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Print.VirtualDevice;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.VirtualPrintDevice
{
    public class Program
    {
        private static int _delay = 2;
        private static int _packetSize = 100000;
        private static IPAddress _deviceAddress;
        private static int _port = 9100;
        private static string _settingsDatabase = null;
        private static DataLogger _dataLogger = null;

        static void Main(string[] args)
        {
            try
            {
                Thread.CurrentThread.SetName("Main");
                try
                {
                    ValidateArguments(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Usage();
                    Console.WriteLine("Press <Enter> to Exit");
                    Console.ReadLine();
                    Environment.Exit(1);
                }

                using (VirtualDeviceHost deviceHost = new VirtualDeviceHost(_deviceAddress, _port))
                {
                    deviceHost.PacketDelay = TimeSpan.FromMilliseconds(_delay);
                    deviceHost.PacketSize = _packetSize;
                    deviceHost.JobReceived += DeviceHost_JobReceived;
                    deviceHost.Start();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to open Virtual Print Listener", ex);
                Console.WriteLine("Press <Enter> to Exit");
                Console.ReadLine();
            }
        }

        private static void ValidateArguments(string[] args)
        {

            int i;
            // Process optional arguments first.
            for (i = 0; i < args.Length; i++)
            {
                string current = args[i];
                if (current == @"/s")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        throw new ArgumentException("Missing packet size value.");
                    }
                    _packetSize = int.Parse(args[i], CultureInfo.InvariantCulture);
                    continue;
                }

                if (current == @"/d")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        throw new ArgumentException("Missing delay value.");
                    }
                    _delay = int.Parse(args[i], CultureInfo.InvariantCulture);
                    continue;
                }

                if (current == @"/p")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        throw new ArgumentException("Missing port value.");
                    }
                    _port = int.Parse(args[i], CultureInfo.InvariantCulture);
                    continue;
                }

                if (current == @"/c")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        throw new ArgumentException("Missing System Server value.");
                    }

                    // Don't load the settings yet - need to wait until the logger thread context
                    // has been set from the IP address
                    _settingsDatabase = args[i];
                    continue;
                }

                // If we made it this far, we are done processing optional arguments.
                break;
            }

            // One required argument left.
            int argsLeft = args.Length - i;
            if (argsLeft < 1)
            {
                throw new ArgumentException("No octet specified.");
            }

            if (argsLeft > 1)
            {
                throw new ArgumentException("Too many arguments.");
            }

            // We passed all of the checks.
            _deviceAddress = BuildDeviceAddress(byte.Parse(args[i], CultureInfo.InvariantCulture));
            TraceFactory.SetThreadContextProperty("Address", _deviceAddress.ToString().Replace('.', '_'));

            string environment = "Unassigned";
            if (_settingsDatabase != null)
            {
                SettingsLoader.LoadSystemConfiguration(_settingsDatabase);
                environment = SettingsLoader.RetrieveSetting("Environment");
                _dataLogger = new DataLogger(SettingsLoader.RetrieveSetting("DataLog"));
            }

            Console.Title = "{0}  {1}".FormatWith(_deviceAddress.ToString(), environment);

        }

        /// <summary>
        /// Note: We can't make any calls to TraceFactory here because we haven't set the Log4Net filename yet,
        /// which needs to be the IP Address
        /// </summary>
        /// <param name="lastOctet"></param>
        /// <returns></returns>
        private static IPAddress BuildDeviceAddress(byte lastOctet)
        {
            // The last octet value 0 is reserved for the IP address of the machine, so we will not allow it to be used as a virtual printer.
            if (lastOctet < 1 || lastOctet > 255)
            {
                throw new ArgumentException("Last Octet is outside the range of valid values (1-255).");
            }
            
            var addresses = Dns.GetHostAddresses(Dns.GetHostName()).Where(n => n.AddressFamily == AddressFamily.InterNetwork);
            IPAddress local = addresses.FirstOrDefault(n => n.IsRoutable());
            Console.WriteLine("Host: {0}".FormatWith(local.ToString()));

            byte[] bytes = local.GetAddressBytes();
            bytes[3] = lastOctet;

            return new IPAddress(bytes);
        }

        private static void DeviceHost_JobReceived(object sender, VirtualPrinterJobInfoEventArgs e)
        {
            try
            {
                VirtualPrinterJobInfo jobInfo = e.Job;
                if (_dataLogger != null)
                {
                    // If we don't have the job name, there isn't enough data to log this print job
                    if (!string.IsNullOrEmpty(jobInfo.PjlHeader.JobName))
                    {
                        LogToDatabase(jobInfo);
                        TraceFactory.Logger.Debug(GetString(jobInfo.PjlHeader));
                        return;
                    }
                    //If we get to this point, there was no job name. Continue on to log the data. 
                    TraceFactory.Logger.Debug("Insufficient data to log print job.");
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to process print job.", ex);
            }
        }

        private static void LogToDatabase(VirtualPrinterJobInfo jobInfo)
        {
            try
            {
                VirtualPrinterJobLog logger = new VirtualPrinterJobLog(jobInfo);
                _dataLogger.Submit(logger);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error posting data to the STF data log service.", ex);
            }
        }

        public static string GetString(PjlHeader header)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(header.JobName))
            {
                builder.AppendLine("JobName: {0}".FormatWith(header.JobName));
            }
            if (!string.IsNullOrEmpty(header.Language))
            {
                builder.AppendLine("Language: {0}".FormatWith(header.Language));
            }
            foreach (int number in header.JobAccts.Keys.OrderBy(n => n))
            {
                builder.AppendLine("JobAcct{0}: {1}".FormatWith(number, header.JobAccts[number]));
            }
            foreach (string comment in header.Comments)
            {
                builder.AppendLine("Comment: {0}".FormatWith(comment));
            }
            return builder.ToString();
        }

        private static void Usage()
        {
            Console.WriteLine();
            Console.WriteLine(@"Usage: {0} [/s <Packet Size={1} bytes>] [/d <Delay={2} ms>] [/p <Port={3}>] [/c SystemSettingsServer] [Last Octet]".FormatWith(AppDomain.CurrentDomain.FriendlyName, _packetSize, _delay, _port));
            Console.WriteLine("  Packet Size is how many bytes to process from the network at a time.");
            Console.WriteLine("  Delay is how many milliseconds to wait between each packet of size <Packet Size>.");
            Console.WriteLine("  Port is a port other than 9100.");
            Console.WriteLine("  SystemSettingServer is the STF System database the contains config settings. If not provided, all header data is written to the TraceLog file.");
            Console.WriteLine("  Last Octet is the last octet of the IP address the device will listen on for print data.");
        }
    }
}
