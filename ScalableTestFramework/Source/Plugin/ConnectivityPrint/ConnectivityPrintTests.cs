using System.Linq;
using HP.ScalableTest.PluginSupport.Connectivity;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    class ConnectivityPrintTests : CtcBaseTests
    {
        #region Local Variables

        ConnectivityPrintActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Print tests constructor
        /// </summary>
        /// <param name="activitydata"></param>
        public ConnectivityPrintTests(ConnectivityPrintActivityData activitydata) : base(activitydata.ProductName)
        {
            _activityData = activitydata;
            ProductFamily = activitydata.ProductFamily.ToString();
            Sliver = "Print";
            NetworkConnectivity = activitydata.PrinterConnectivity;
        }

        #endregion

        #region Private Functions

        private int GetTestDuration(int testNumber)
        {
            int duration = 0;

            var details = from item in _activityData.TestDetails where item.TestId.Equals(testNumber) select new { Duration = item.Duration };
            foreach (var item in details)
            {
                duration = item.Duration;
                break;
            }
            return duration;
        }

        #endregion

        #region Tests

        #region Installation and Simple Printing

        [TestDetailsAttribute(Id = 1, Description = "Installation and Simple Printing", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.RAW, 9100, 1);
        }

        [TestDetailsAttribute(Id = 2, Description = "Installation and Simple Printing", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.RAW, 9100, 2, true);
        }

        [TestDetailsAttribute(Id = 3, Description = "Installation and Simple Printing", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.LPD, 515, 3);
        }

        [TestDetailsAttribute(Id = 4, Description = "Installation and Simple Printing", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.LPD, 515, 4, true);
        }

        [TestDetailsAttribute(Id = 5, Description = "Installation and Simple Printing", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_IPP_80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.IPP, 80, 5);
        }

        [TestDetailsAttribute(Id = 6, Description = "Installation and Simple Printing", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_IPP_80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.IPP, 80, 6, true);
        }

        [TestDetailsAttribute(Id = 7, Description = "Installation and Simple Printing", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_IPP_631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.IPP, 631, 7);
        }

        [TestDetailsAttribute(Id = 8, Description = "Installation and Simple Printing", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_IPP_631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.IPP, 631, 8, true);
        }

        [TestDetailsAttribute(Id = 9, Description = "Installation and Simple Printing", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.IPPS, 443, 9);
        }

        [TestDetailsAttribute(Id = 11, Description = "Installation and Simple Printing", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.WSP, 3912, 11);
        }

        [TestDetailsAttribute(Id = 12, Description = "Installation and Simple Printing", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrinting(Printer.Printer.PrintProtocol.WSP, 3912, 12, true);
        }

        [TestDetailsAttribute(Id = 13, Description = "Simple Printing (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrintingFtp(13);
        }

        [TestDetailsAttribute(Id = 14, Description = "Simple Printing (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_FTP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrintingFtp(14, true);
        }

        [TestDetailsAttribute(Id = 15, Description = "Simple Printing (Passive)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintSimpleFiles_FTP_Passive()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrintingFtp(15, isPassiveMode: true);
        }

        [TestDetailsAttribute(Id = 16, Description = "Simple Printing (Passive)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintSimpleFiles_FTP_IPv6_Passive()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.SimplePrintingFtp(16, true, isPassiveMode: true);
        }

        #endregion

        #region Continuous Printing

        [TestDetailsAttribute(Id = 17, Description = "Continuous Printing", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, PrintDuration = 120, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintContinuousFiles_P9100()
        {
            int duration = GetTestDuration(17);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.RAW, 9100, 17, duration);
        }

        [TestDetailsAttribute(Id = 18, Description = "Continuous Printing", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, PrintDuration = 120, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintContinuousFiles_P9100_IPv6()
        {
            int duration = GetTestDuration(18);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.RAW, 9100, 18, duration, true);
        }

        [TestDetailsAttribute(Id = 19, Description = "Continuous Printing", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, PrintDuration = 120, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintContinuousFiles_LPD()
        {
            int duration = GetTestDuration(19);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.LPD, 515, 19, duration);
        }

        [TestDetailsAttribute(Id = 20, Description = "Continuous Printing", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, PrintDuration = 120, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintContinuousFiles_LPD_IPv6()
        {
            int duration = GetTestDuration(20);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.LPD, 515, 20, duration, true);
        }

        [TestDetailsAttribute(Id = 27, Description = "Continuous Printing", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, PrintDuration = 120, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintContinuousFiles_WSP()
        {
            int duration = GetTestDuration(27);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.WSP, 3912, 27, duration);
        }

        [TestDetailsAttribute(Id = 28, Description = "Continuous Printing", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, PrintDuration = 120, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintContinuousFiles_WSP_IPv6()
        {
            int duration = GetTestDuration(28);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrinting(Printer.Printer.PrintProtocol.WSP, 3912, 28, duration, true);
        }

        [TestDetailsAttribute(Id = 29, Description = "Continuous Printing (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, PrintDuration = 120, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintContinuousFiles_FTP()
        {
            int duration = GetTestDuration(29);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrintingFtp(29, duration);
        }

        [TestDetailsAttribute(Id = 30, Description = "Continuous Printing (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, PrintDuration = 120, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintContinuousFiles_FTP_IPv6()
        {
            int duration = GetTestDuration(30);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrintingFtp(30, duration, true);
        }

        [TestDetailsAttribute(Id = 31, Description = "Continuous Printing (Passive)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, PrintDuration = 120, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintContinuousFiles_FTP_Passive()
        {
            int duration = GetTestDuration(31);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrintingFtp(31, duration, isPassiveMode: true);
        }

        [TestDetailsAttribute(Id = 32, Description = "Continuous Printing (Passive)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, PrintDuration = 120, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintContinuousFiles_FTP_IPv6_Passive()
        {
            int duration = GetTestDuration(32);
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.ContinuousPrintingFtp(32, duration, true, isPassiveMode: true);
        }
        #endregion

        #region Hose Break

        #region Short Hose Break

        [TestDetailsAttribute(Id = 33, Description = "Short Hose Break", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.RAW, 9100, 33);
        }

        [TestDetailsAttribute(Id = 34, Description = "Short Hose Break", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.RAW, 9100, 34, true, true);
        }

        [TestDetailsAttribute(Id = 35, Description = "Short Hose Break", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.LPD, 515, 35);
        }

        [TestDetailsAttribute(Id = 36, Description = "Short Hose Break", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.LPD, 515, 36, true, true);
        }

        [TestDetailsAttribute(Id = 37, Description = "Short Hose Break", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_IPP_80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 80, 37);
        }

        [TestDetailsAttribute(Id = 38, Description = "Short Hose Break", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_IPP_80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 80, 38, true, true);
        }

        [TestDetailsAttribute(Id = 39, Description = "Short Hose Break", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_IPP_631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 631, 39);
        }

        [TestDetailsAttribute(Id = 40, Description = "Short Hose Break", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_IPP_631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 631, 40, true, true);
        }

        [TestDetailsAttribute(Id = 41, Description = "Short Hose Break", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPPS, 443, 41);
        }

        [TestDetailsAttribute(Id = 43, Description = "Short Hose Break", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.WSP, 3912, 43);
        }

        [TestDetailsAttribute(Id = 44, Description = "Short Hose Break", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.WSP, 3912, 44, true, true);
        }

        [TestDetailsAttribute(Id = 45, Description = "Short Hose Break (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ShortHoseBreak_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreakFtp(45);
        }

        [TestDetailsAttribute(Id = 46, Description = "Short Hose Break (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ShortHoseBreak_FTP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreakFtp(46, true, true);
        }

        #endregion Short

        #region Long Hose Break

        [TestDetailsAttribute(Id = 49, Description = "Long Hose Break", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.RAW, 9100, 49, false);
        }

        [TestDetailsAttribute(Id = 50, Description = "Long Hose Break", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.RAW, 9100, 50, false, true);
        }

        [TestDetailsAttribute(Id = 51, Description = "Long Hose Break", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.LPD, 515, 51, false);
        }

        [TestDetailsAttribute(Id = 52, Description = "Long Hose Break", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.LPD, 515, 52, false, true);
        }

        [TestDetailsAttribute(Id = 53, Description = "Long Hose Break", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_IPP_80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 80, 53, false);
        }

        [TestDetailsAttribute(Id = 54, Description = "Long Hose Break", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_IPP_80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 80, 54, false, true);
        }

        [TestDetailsAttribute(Id = 55, Description = "Long Hose Break", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_IPP_631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 631, 55, false);
        }

        [TestDetailsAttribute(Id = 56, Description = "Long Hose Break", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_IPP_631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPP, 631, 56, false, true);
        }

        [TestDetailsAttribute(Id = 57, Description = "Long Hose Break", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.IPPS, 443, 108, false);
        }

        [TestDetailsAttribute(Id = 59, Description = "Long Hose Break", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.WSP, 3912, 59, false);
        }

        [TestDetailsAttribute(Id = 60, Description = "Long Hose Break", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreak(Printer.Printer.PrintProtocol.WSP, 3912, 60, false, true);
        }

        [TestDetailsAttribute(Id = 61, Description = "Long Hose Break (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_LongHoseBreak_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreakFtp(61, false);
        }

        [TestDetailsAttribute(Id = 62, Description = "Long Hose Break (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_LongHoseBreak_FTP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.HoseBreakFtp(62, false, true);
        }

        #endregion

        #endregion

        #region Cancel Jobs

        #region InActive

        [TestDetailsAttribute(Id = 65, Description = "Cancel Inactive Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelInActiveJob_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.RAW, 9100, 65);
        }

        [TestDetailsAttribute(Id = 66, Description = "Cancel Inactive Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelInActiveJob_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.RAW, 9100, 66, true);
        }

        [TestDetailsAttribute(Id = 67, Description = "Cancel Inactive Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelInActiveJob_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.LPD, 515, 67);
        }

        [TestDetailsAttribute(Id = 68, Description = "Cancel Inactive Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelInActiveJob_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.LPD, 515, 68, true);
        }

        [TestDetailsAttribute(Id = 75, Description = "Cancel Inactive Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelInActiveJob_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.WSP, 3912, 75);
        }

        [TestDetailsAttribute(Id = 76, Description = "Cancel Inactive Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelInActiveJob_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelInactiveJob(Printer.Printer.PrintProtocol.WSP, 3912, 76, true);
        }

        #endregion

        #region Spooling

        [TestDetailsAttribute(Id = 81, Description = "Cancel Spooling Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelSpoolingJob_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.RAW, 9100, 81);
        }

        [TestDetailsAttribute(Id = 82, Description = "Cancel Spooling Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelSpoolingJob_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.RAW, 9100, 82, true);
        }

        [TestDetailsAttribute(Id = 83, Description = "Cancel Spooling Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelSpoolingJob_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.LPD, 515, 83);
        }

        [TestDetailsAttribute(Id = 84, Description = "Cancel Spooling Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelSpoolingJob_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.LPD, 515, 84, true);
        }

        [TestDetailsAttribute(Id = 91, Description = "Cancel Spooling Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelSpoolingJob_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.WSP, 3912, 91);
        }

        [TestDetailsAttribute(Id = 92, Description = "Cancel Spooling Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelSpoolingJob_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelSpoolingJob(Printer.Printer.PrintProtocol.WSP, 3912, 92, true);
        }

        #endregion

        #region Active

        [TestDetailsAttribute(Id = 97, Description = "Cancel Active Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelActiveJob_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.RAW, 9100, 97);
        }

        [TestDetailsAttribute(Id = 98, Description = "Cancel Active Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelActiveJob_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.RAW, 9100, 98, true);
        }

        [TestDetailsAttribute(Id = 99, Description = "Cancel Active Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelActiveJob_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.LPD, 515, 99);
        }

        [TestDetailsAttribute(Id = 100, Description = "Cancel Active Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelActiveJob_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.LPD, 515, 100, true);
        }

        [TestDetailsAttribute(Id = 107, Description = "Cancel Active Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_CancelActiveJob_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.WSP, 3912, 107);
        }

        [TestDetailsAttribute(Id = 108, Description = "Cancel Active Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_CancelActiveJob_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelActiveJob(Printer.Printer.PrintProtocol.WSP, 3912, 108, true);
        }

        [TestDetailsAttribute(Id = 109, Description = "Cancel FTP Job (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_Cancel_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelFtpJob(91);
        }

        [TestDetailsAttribute(Id = 110, Description = "Cancel FTP Job (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_Cancel_FTP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.CancelFtpJob(92, true);
        }

        #endregion

        #endregion

        #region Job Pause

        [TestDetailsAttribute(Id = 113, Description = "Pause Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PauseJob_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.RAW, 9100, 113);
        }

        [TestDetailsAttribute(Id = 114, Description = "Pause Job", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PauseJob_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.RAW, 9100, 114, true);
        }

        [TestDetailsAttribute(Id = 115, Description = "Pause Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PauseJob_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.LPD, 515, 115);
        }

        [TestDetailsAttribute(Id = 116, Description = "Pause Job", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PauseJob_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.LPD, 515, 116, true);
        }

        [TestDetailsAttribute(Id = 123, Description = "Pause Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PauseJob_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.WSP, 3912, 123);
        }

        [TestDetailsAttribute(Id = 124, Description = "Pause Job", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_PauseJob_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PauseJob(Printer.Printer.PrintProtocol.WSP, 3912, 124, true);
        }

        #endregion

        #region Printer Off-line

        [TestDetailsAttribute(Id = 129, Description = "Offline Printer", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 129);
        }

        [TestDetailsAttribute(Id = 130, Description = "Offline Printer", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintOffline_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 130, true);
        }

        [TestDetailsAttribute(Id = 131, Description = "Offline Printer", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 131);
        }

        [TestDetailsAttribute(Id = 132, Description = "Offline Printer", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintOffline_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 132, true);
        }

        [TestDetailsAttribute(Id = 133, Description = "Offline Printer", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 133);
        }

        [TestDetailsAttribute(Id = 134, Description = "Offline Printer", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintOffline_IPP80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 134, true);
        }

        [TestDetailsAttribute(Id = 135, Description = "Offline Printer", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 135);
        }

        [TestDetailsAttribute(Id = 136, Description = "Offline Printer", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintOffline_IPP631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 136, true);
        }

        [TestDetailsAttribute(Id = 137, Description = "Offline Printer", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 137);
        }

        [TestDetailsAttribute(Id = 139, Description = "Offline Printer", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintOffline_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 139);
        }

        [TestDetailsAttribute(Id = 140, Description = "Offline Printer", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintOffline_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOffline(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 140, true);
        }

        #endregion

        #region Printer Reboot

        [TestDetailsAttribute(Id = 145, Description = "Printer Reboot", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 145);
        }

        [TestDetailsAttribute(Id = 146, Description = "Printer Reboot", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithReboot_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 146, true);
        }

        [TestDetailsAttribute(Id = 147, Description = "Printer Reboot", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 147);
        }

        [TestDetailsAttribute(Id = 148, Description = "Printer Reboot", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithReboot_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 148, true);
        }

        [TestDetailsAttribute(Id = 149, Description = "Printer Reboot", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 149);
        }

        [TestDetailsAttribute(Id = 150, Description = "Printer Reboot", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithReboot_IPP80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 150, true);
        }

        [TestDetailsAttribute(Id = 151, Description = "Printer Reboot", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 151);
        }

        [TestDetailsAttribute(Id = 152, Description = "Printer Reboot", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithReboot_IPP631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 152, true);
        }

        [TestDetailsAttribute(Id = 153, Description = "Printer Reboot", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 153);
        }

        [TestDetailsAttribute(Id = 155, Description = "Printer Reboot", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithReboot_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 155);
        }

        [TestDetailsAttribute(Id = 156, Description = "Printer Reboot", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithReboot_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnReboot(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 156, true);
        }

        #endregion

        #region Changing IP address and Printing

        [TestDetailsAttribute(Id = 161, Description = "Printing After IP Change on Printer & Reinstallation", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAndReinstall_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 161);
        }

        [TestDetailsAttribute(Id = 162, Description = "Printing After IP Change on Printer & Reinstallation", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeIPAndReinstall_P9100_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 162, true);
        }

        [TestDetailsAttribute(Id = 163, Description = "Printing After IP Change on Printer & Reinstallation", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAndReinstall_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 163);
        }

        [TestDetailsAttribute(Id = 164, Description = "Printing After IP Change on Printer & Reinstallation", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeIPAndReinstall_LPD_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 164, true);
        }

        [TestDetailsAttribute(Id = 165, Description = "Printing After IP Change on Printer & Reinstallation", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAndReinstall_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 165);
        }

        [TestDetailsAttribute(Id = 166, Description = "Printing After IP Change on Printer & Reinstallation", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeIPAndReinstall_IPP80_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 166, true);
        }

        [TestDetailsAttribute(Id = 167, Description = "Printing After IP Change on Printer & Reinstallation", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAndReinstall_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 167);
        }

        [TestDetailsAttribute(Id = 168, Description = "Printing After IP Change on Printer & Reinstallation", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeIPAndReinstall_IPP631_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 168, true);
        }

        [TestDetailsAttribute(Id = 169, Description = "Printing After IP Change on Printer & Reinstallation", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAndReinstall_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 169);
        }

        #endregion

        #region Print With LAA
        [TestDetailsAttribute(Id = 177, Description = "Printing After LAA Change", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 177);
        }

        [TestDetailsAttribute(Id = 178, Description = "Printing After LAA Change", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintLAA_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 178, true);
        }

        [TestDetailsAttribute(Id = 179, Description = "Printing After LAA Change", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 179);
        }

        [TestDetailsAttribute(Id = 180, Description = "Printing After LAA Change", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintLAA_LPD_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 180, true);
        }

        [TestDetailsAttribute(Id = 181, Description = "Printing After LAA Change", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 181);
        }

        [TestDetailsAttribute(Id = 182, Description = "Printing After LAA Change", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintLAA_IPP80_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 182, true);
        }

        [TestDetailsAttribute(Id = 183, Description = "Printing After LAA Change", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 183);
        }

        [TestDetailsAttribute(Id = 184, Description = "Printing After LAA Change", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintLAA_IPP631_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 184, true);
        }

        [TestDetailsAttribute(Id = 185, Description = "Printing After LAA Change", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 185);
        }

        [TestDetailsAttribute(Id = 187, Description = "Printing After LAA Change", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintLAA_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintWithLAA(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 187);
        }

        #endregion

        #region Printing Across Subnets

        [TestDetailsAttribute(Id = 193, Description = "Printing Across Subnets", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 193);
        }

        [TestDetailsAttribute(Id = 194, Description = "Printing Across Subnets", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_P9100_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 194, true);
        }

        [TestDetailsAttribute(Id = 195, Description = "Printing Across Subnets", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 195);
        }

        [TestDetailsAttribute(Id = 196, Description = "Printing Across Subnets", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_LPD_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 196, true);
        }

        [TestDetailsAttribute(Id = 197, Description = "Printing Across Subnets", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 197, true);
        }

        [TestDetailsAttribute(Id = 198, Description = "Printing Across Subnets", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_IPP80_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 198, true);
        }

        [TestDetailsAttribute(Id = 199, Description = "Printing Across Subnets", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 199, true);
        }

        [TestDetailsAttribute(Id = 200, Description = "Printing Across Subnets", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_IPP631_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 200, true);
        }

        [TestDetailsAttribute(Id = 201, Description = "Printing Across Subnets", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 201);
        }

        [TestDetailsAttribute(Id = 203, Description = "Printing Across Subnets", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 203);
        }

        [TestDetailsAttribute(Id = 204, Description = "Printing Across Subnets", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_WSP_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 204, true);
        }

        [TestDetailsAttribute(Id = 205, Description = "Printing Across Subnets (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyPrintingAcrossInterfaces_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets_FTP(_activityData, 205);
        }

        [TestDetailsAttribute(Id = 206, Description = "Printing Across Subnets (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyPrintingAcrossInterfaces_FTP_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAcrossSubnets_FTP(_activityData, 206, true);
        }
        #endregion

        #region Editing Printer Properties and Printing

        [TestDetailsAttribute(Id = 209, Description = "Printing After IP Change on Printer & Editing Printer Properties", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAddressAndEditPrinter_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndEditPrinter(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 209);
        }

		[TestDetailsAttribute(Id = 210, Description = "Printing After IP Change on Printer & Editing Printer Properties", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
		public bool Test_ChangeIPAddressAndEditPrinter_P9100_IPv6()
		{
			ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
			return printTemplate.PrintOnIPChangeAndEditPrinter(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 210, true);
		}

        [TestDetailsAttribute(Id = 211, Description = "Printing After IP Change on Printer & Editing Printer Properties", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAddressAndEditPrinter_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnIPChangeAndEditPrinter(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 211);
        }

		[TestDetailsAttribute(Id = 212, Description = "Printing After IP Change on Printer & Editing Printer Properties", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
		public bool Test_ChangeIPAddressAndEditPrinter_LPD_IPv6()
		{
			ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
			return printTemplate.PrintOnIPChangeAndEditPrinter(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 212, true);
		}

        #endregion

        #region Verify PrinterBehaviour during FrontPanel Navigation

        [TestDetailsAttribute(Id = 225, Description = "Front Panel Navigation", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 225);
        }

        [TestDetailsAttribute(Id = 226, Description = "Front Panel Navigation", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithFrontPanelNavigation_P9100_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 226, true);
        }

        [TestDetailsAttribute(Id = 227, Description = "Front Panel Navigation", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 227);
        }

        [TestDetailsAttribute(Id = 228, Description = "Front Panel Navigation", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithFrontPanelNavigation_LPD_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 228, true);
        }

        [TestDetailsAttribute(Id = 229, Description = "Front Panel Navigation", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 229);
        }

        [TestDetailsAttribute(Id = 230, Description = "Front Panel Navigation", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithFrontPanelNavigation_IPP80_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 230, true);
        }

        [TestDetailsAttribute(Id = 231, Description = "Front Panel Navigation", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 231);
        }

        [TestDetailsAttribute(Id = 232, Description = "Front Panel Navigation", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithFrontPanelNavigation_IPP631_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 232, true);
        }

        [TestDetailsAttribute(Id = 233, Description = "Front Panel Navigation", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 233);
        }

        [TestDetailsAttribute(Id = 235, Description = "Front Panel Navigation", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFrontPanelNavigation_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFrontPanelNavigation(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 235);
        }

        #endregion

        #region Verify JobPause through FrontPanel

        [TestDetailsAttribute(Id = 241, Description = "Job Pause from Front Panel", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 241);
        }

        [TestDetailsAttribute(Id = 242, Description = "Job Pause from Front Panel", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobPause_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 242, true);
        }

        [TestDetailsAttribute(Id = 243, Description = "Job Pause from Front Panel", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 243);
        }

        [TestDetailsAttribute(Id = 244, Description = "Job Pause from Front Panel", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobPause_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 244, true);
        }

        [TestDetailsAttribute(Id = 245, Description = "Job Pause from Front Panel", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 245);
        }

        [TestDetailsAttribute(Id = 246, Description = "Job Pause from Front Panel", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobPause_IPP80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 246, true);
        }

        [TestDetailsAttribute(Id = 247, Description = "Job Pause from Front Panel", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 247);
        }

        [TestDetailsAttribute(Id = 248, Description = "Job Pause from Front Panel", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobPause_IPP631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 248, true);
        }

        [TestDetailsAttribute(Id = 249, Description = "Job Pause from Front Panel", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 249);
        }

        [TestDetailsAttribute(Id = 251, Description = "Job Pause from Front Panel", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobPause_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobPausethroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 251);
        }

        #endregion

        #region Verify JobCancel through FrontPanel

        [TestDetailsAttribute(Id = 257, Description = "Job Cancel from Front Panel", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_P9100()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 257);
        }

        [TestDetailsAttribute(Id = 258, Description = "Job Cancel from Front Panel", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobCancel_P9100_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.RAW, 9100, 258, true);
        }

        [TestDetailsAttribute(Id = 259, Description = "Job Cancel from Front Panel", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_LPD()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 259);
        }

        [TestDetailsAttribute(Id = 260, Description = "Job Cancel from Front Panel", Category = "LPD", PortNumber = 515, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobCancel_LPD_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.LPD, 515, 260, true);
        }

        [TestDetailsAttribute(Id = 261, Description = "Job Cancel from Front Panel", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_IPP80()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 261);
        }

        [TestDetailsAttribute(Id = 262, Description = "Job Cancel from Front Panel", Category = "IPP:80", PortNumber = 80, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobCancel_IPP80_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 80, 262, true);
        }

        [TestDetailsAttribute(Id = 263, Description = "Job Cancel from Front Panel", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_IPP631()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 263);
        }

        [TestDetailsAttribute(Id = 264, Description = "Job Cancel from Front Panel", Category = "IPP:631", PortNumber = 631, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobCancel_IPP631_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPP, 631, 264, true);
        }

        [TestDetailsAttribute(Id = 265, Description = "Job Cancel from Front Panel", Category = "IPP:443", PortNumber = 443, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_IPPS()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.IPPS, 443, 265);
        }

        [TestDetailsAttribute(Id = 267, Description = "Job Cancel from Front Panel", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 267);
        }

        [TestDetailsAttribute(Id = 269, Description = "Job Cancel from Front Panel(Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv4)]
        public bool Test_VerifyJobCancel_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel_FTP(_activityData, 269);
        }

        [TestDetailsAttribute(Id = 270, Description = "Job Cancel from Front Panel(Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP, Protocol = ProtocolType.IPv6)]
        public bool Test_VerifyJobCancel_FTP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyJobCancelthroughFrontPanel_FTP(_activityData, 270, true);
        }

        #endregion

        #region Verify PrinterBehaviour during FirmwareUpgrade

        [TestDetailsAttribute(Id = 501, Description = "Printing during Firmware Upgrade", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_PrintWithFirmwareUpgrade()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFirmwareUpgrade(_activityData, 501);
        }

        [TestDetailsAttribute(Id = 502, Description = "Printing during Firmware Upgrade", Category = "P9100", PortNumber = 9100, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_PrintWithFirmwareUpgrade_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.VerifyPrinterBehaviourduringFirmwareUpgrade(_activityData, 502, true);
        }

        #endregion

        #region Dynamic Raw Port

        [TestDetailsAttribute(Id = 503, Description = "RAW Port & Deleted RAW Port", Category = "RAW", PortNumber = 3500, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_Print_RAW_IPv4()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.DynamicRawPortPrinting(3500, 3501, 503);
        }

        [TestDetailsAttribute(Id = 504, Description = "RAW Port & Deleted RAW Port", Category = "RAW", PortNumber = 3500, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_Print_RAW_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.DynamicRawPortPrinting(3500, 3501, 504, true);
        }

        #endregion

        #region Changing IP Address and Reconnecting through FTP

        [TestDetailsAttribute(Id = 505, Description = "Printing After IP Change on Printer & Reconnection (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeIPAddressAndReconnect_FTP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAfterIPChangeAndReconnection_FTP(_activityData, 505);
        }

        [TestDetailsAttribute(Id = 506, Description = "Printing After IP Change on Printer & Reconnection (Active)", Category = "FTP", PortNumber = -1, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeIPAddressAndReconnect_FTP_IPV6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintAfterIPChangeAndReconnection_FTP(_activityData, 506, true);
        }
        #endregion

        #region Changing Host name and Reinstalling Printer

        [TestDetailsAttribute(Id = 507, Description = "Printing After Hostname Change & Reinstallation", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.All, Protocol = ProtocolType.IPv4)]
        public bool Test_ChangeHostNameAndReinstall_WSP()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnHostNameChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 507);
        }

        [TestDetailsAttribute(Id = 508, Description = "Printing After Hostname Change & Reinstallation", Category = "WSP", PortNumber = 3912, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Protocol = ProtocolType.IPv6)]
        public bool Test_ChangeHostNameAndReinstall_WSP_IPv6()
        {
            ConnectivityPrintTemplates printTemplate = new ConnectivityPrintTemplates(_activityData);
            return printTemplate.PrintOnHostNameChangeAndReinstall(_activityData, Printer.Printer.PrintProtocol.WSP, 3912, 508, true);
        }

        #endregion

        #endregion
    }
}
