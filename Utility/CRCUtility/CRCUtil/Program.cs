using System;
using System.Net;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;

namespace PaperlessUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check to be sure there are two command line arguments
            if (args.Length != 2)
            {
                Console.WriteLine("Two command line arguments required: Printer IP Address, and 'on' or 'off'.");
                Environment.Exit(1);
            }

            // Verify that the first commmand line argument is a valid ip address
            IPAddress address;
            if (!IPAddress.TryParse(args[0], out address))
            {
                Console.WriteLine("IP Address specified is invalid.");
                Environment.Exit(1);
            }

            // Verify that the second command line argument is either 'on' or 'off'
            if (args[1].Equals("on", StringComparison.InvariantCultureIgnoreCase))
            {
                SetPaperlessMode(address, true);
            }
            else if (args[1].Equals("off", StringComparison.InvariantCultureIgnoreCase))
            {
                SetPaperlessMode(address, false);
            }
            else
            {
                Console.WriteLine("The second argument must be either 'on' or 'off'.");
                Environment.Exit(1);
            }
        }

        public const int port9100 = 9100;

        public static void SetPaperlessMode(IPAddress ipaddress, bool paperlessModeOn)
        {
            JobMediaMode mode = paperlessModeOn ? JobMediaMode.Paperless : JobMediaMode.Paper;
            using (IDevice device = DeviceFactory.Create(ipaddress, "!QAZ2wsx"))
            {
                IDeviceSettingsManager manager = DeviceSettingsManagerFactory.Create(device);
                manager.SetJobMediaMode(mode);
            }
            Console.WriteLine(string.Format("Paperless mode was turned {0}.", paperlessModeOn ? "on" : "off"));
        }
    }
}
