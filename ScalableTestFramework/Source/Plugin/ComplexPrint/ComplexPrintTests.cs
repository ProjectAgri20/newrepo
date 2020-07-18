using HP.ScalableTest.PluginSupport.Connectivity;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    internal class ComplexPrintTests : CtcBaseTests
    {
        #region Local Variables

        ComplexPrintActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Print tests constructor
        /// </summary>
        /// <param name="activitydata"></param>
        public ComplexPrintTests(ComplexPrintActivityData activitydata)
            : base(activitydata.ProductName)
        {
            _activityData = activitydata;
            ProductFamily = activitydata.ProductFamily.ToString();
            Sliver = "ComplexPrint";
            NetworkConnectivity = activitydata.PrinterConnectivity;
        }

        #endregion

        [TestDetailsAttribute(Id = 1, Description = "Printing with P9100 - IPv4", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_P9100_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.RAW, 9100, 1);
        }

        [TestDetailsAttribute(Id = 2, Description = "Printing with P9100 - Linklocal", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_P9100_Linklocal()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6LinkLocalAddress, Printer.Printer.PrintProtocol.RAW, 9100, 2);
        }

        [TestDetailsAttribute(Id = 3, Description = "Printing with P9100 - Stateless", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_P9100_Stateless()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StatelessAddress, Printer.Printer.PrintProtocol.RAW, 9100, 3);
        }

        [TestDetailsAttribute(Id = 4, Description = "Printing with P9100 - Stateful", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_P9100_Stateful()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StateFullAddress, Printer.Printer.PrintProtocol.RAW, 9100, 4);
        }

        [TestDetailsAttribute(Id = 5, Description = "Printing with LDP - IPv4", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_LPD_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.LPD, 515, 5);
        }

        [TestDetailsAttribute(Id = 6, Description = "Printing with LDP - Linklocal", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_LPD_Linklocal()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6LinkLocalAddress, Printer.Printer.PrintProtocol.LPD, 515, 6);
        }

        [TestDetailsAttribute(Id = 7, Description = "Printing with LDP - Stateless", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_LPD_Stateless()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StatelessAddress, Printer.Printer.PrintProtocol.LPD, 515, 7);
        }

        [TestDetailsAttribute(Id = 8, Description = "Printing with LDP - Stateful", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_LPD_Stateful()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StateFullAddress, Printer.Printer.PrintProtocol.LPD, 515, 8);
        }

        [TestDetailsAttribute(Id = 9, Description = "Printing with IPP - IPv4", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_IPP80_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.IPP, 80, 9);
        }

        [TestDetailsAttribute(Id = 10, Description = "Printing with IPP - Linklocal", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP80_Linklocal()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6LinkLocalAddress, Printer.Printer.PrintProtocol.IPP, 80, 10);
        }

        [TestDetailsAttribute(Id = 11, Description = "Printing with IPP - Stateless", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP80_Stateless()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StatelessAddress, Printer.Printer.PrintProtocol.IPP, 80, 11);
        }

        [TestDetailsAttribute(Id = 12, Description = "Printing with IPP - Stateful", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP80_Stateful()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StateFullAddress, Printer.Printer.PrintProtocol.IPP, 80, 12);
        }

        [TestDetailsAttribute(Id = 13, Description = "Printing with IPP - IPv4", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_IPP631_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.IPP, 631, 13);
        }

        [TestDetailsAttribute(Id = 14, Description = "Printing with IPP - Linklocal", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP631_Linklocal()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6LinkLocalAddress, Printer.Printer.PrintProtocol.IPP, 631, 14);
        }

        [TestDetailsAttribute(Id = 15, Description = "Printing with IPP - Stateless", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP631_Stateless()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StatelessAddress, Printer.Printer.PrintProtocol.IPP, 631, 15);
        }

        [TestDetailsAttribute(Id = 16, Description = "Printing with IPP - Stateful", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_IPP631_Stateful()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StateFullAddress, Printer.Printer.PrintProtocol.IPP, 631, 16);
        }

        [TestDetailsAttribute(Id = 17, Description = "Printing with WSP - IPv4", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_WSP1_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.WSP, 3912, 17);
        }

        [TestDetailsAttribute(Id = 18, Description = "Printing with FTP - IPv4", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_FTP_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, 18);
        }

        [TestDetailsAttribute(Id = 19, Description = "Printing with FTP - Linklocal", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_FTP_Linklocal()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6LinkLocalAddress, 19);
        }

        [TestDetailsAttribute(Id = 20, Description = "Printing with FTP - Stateless", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_FTP_Stateless()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StatelessAddress, 20);
        }

        [TestDetailsAttribute(Id = 21, Description = "Printing with FTP - Stateful", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_FTP_Stateful()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv6StateFullAddress, 21);
        }

        [TestDetailsAttribute(Id = 22, Description = "Printing with IPP - IPv4", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_IPP443_IPv4()
        {
            ComplexPrintTemplates printTemplate = new ComplexPrintTemplates(_activityData);
            return printTemplate.ComplexPrinting(_activityData.Ipv4Address, Printer.Printer.PrintProtocol.IPPS, 443, 22);
        }
    }
}
