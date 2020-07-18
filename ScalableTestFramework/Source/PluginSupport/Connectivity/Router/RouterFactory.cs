using System;
using System.Net;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Router
{
    enum RouterType
    {
        None,
        HP,
        Cisco
    }

    /// <summary>
    /// Factory class to create a Router instance.
    /// </summary>  
    public static class RouterFactory
    {
        public static IRouter Create(IPAddress address, string userName, string password)
        {
            RouterType routerType = GetRouterType(address, userName, password);
            IRouter iRouter = null;

            switch (routerType)
            {
                case RouterType.None:
                    Logger.LogInfo("Router manufacturer can't be identified or it is not implemented");
                    break;

                case RouterType.HP:
                    Logger.LogInfo("HP Procurve Router instance is created with IP Address : {0}".FormatWith(address));
                    iRouter = new HPProcurveRouter(address, userName, password);
                    break;

                case RouterType.Cisco:
                    Logger.LogInfo("Cisco Network Switch is not implemented");
                    break;
            }

            return iRouter;
        }

        /// <summary>
        /// Get the RouterType using IP Address of the Router.
        /// </summary>
        /// <param name="ipAddress">IP Address of the Router.</param>
        /// <param name="userName">User name credentials to the router.</param>
        /// <param name="password">Password credentials to the router.</param>
        /// <returns><see cref="RouterType"/></returns>
        private static RouterType GetRouterType(IPAddress ipAddress, string userName, string password)
        {
            TelnetIpc telnet = new TelnetIpc(ipAddress.ToString(), 23);
            RouterType routerType = RouterType.None;

            try
            {
                telnet.Connect();
                telnet.SendLine(" ");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(password);
                Thread.Sleep(TimeSpan.FromSeconds(20));
                string data = telnet.ReceiveUntilMatch("$");

                // HP manufactured router can contain any of the strings.
                // ProCurve, Hewlett-Packard, Hewlett Packard
                if (data.Contains("ProCurve", StringComparison.CurrentCultureIgnoreCase) || data.Contains("Hewlett-Packard", StringComparison.CurrentCultureIgnoreCase) || data.Contains("Hewlett Packard", StringComparison.CurrentCultureIgnoreCase))
                {
                    routerType = RouterType.HP;
                }
            }
            finally
            {
                telnet.Dispose();
                Thread.Sleep(1000);
            }

            return routerType;
        }
    }
}
