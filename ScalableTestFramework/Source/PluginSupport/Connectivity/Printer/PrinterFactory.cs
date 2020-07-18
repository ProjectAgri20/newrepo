using System;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Printer factory class to create a printer based on the Printer family and IP Address
    /// </summary>
    public static class PrinterFactory
    {
        /// <summary>
        /// Create a printer based on the printer family (architecture) and ip address.
        /// </summary>
        /// <param name="family">Printer family</param>
        /// <param name="ipAddress">IP Address</param>
        /// <returns></returns>
        public static Printer Create(PrinterFamilies family, IPAddress ipAddress)
        {
            Printer printer = null;

            switch (family)
            {
                case PrinterFamilies.InkJet:
                    printer = new SiriusPrinter(ipAddress);
                    break;

                case PrinterFamilies.LFP:
                    printer = new PhoenixPrinter(ipAddress);
                    break;

                case PrinterFamilies.TPS:
                    printer = new MarvellPrinter(ipAddress);
                    break;

                case PrinterFamilies.VEP:
                    printer = new JediPrinter(ipAddress);
                    break;
            }

            return printer;
        }

        /// <summary>
        /// Create a printer based on the printer family (architecture) and ip address.
        /// </summary>
        /// <param name="family">Printer family</param>
        /// <param name="ipAddress">IP Address</param>
        /// <returns></returns>
        public static Printer Create(string family, string ipAddress)
        {
            Printer printer = null;

            PrinterFamilies familyEnum = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), family);

            switch (familyEnum)
            {
                case PrinterFamilies.InkJet:
                    printer = new SiriusPrinter(IPAddress.Parse(ipAddress));
                    break;

                case PrinterFamilies.LFP:
                    printer = new PhoenixPrinter(IPAddress.Parse(ipAddress));
                    break;

                case PrinterFamilies.TPS:
                    printer = new MarvellPrinter(IPAddress.Parse(ipAddress));
                    break;

                case PrinterFamilies.VEP:
                    printer = new JediPrinter(IPAddress.Parse(ipAddress));
                    break;
            }

            return printer;
        }
    }
}
