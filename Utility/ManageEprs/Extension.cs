using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using HP.Epr.Client;

namespace HP.ScalableTest.EndpointResponder
{
    internal static class Extension
    {
        public static IPAddressEx ToEx(this IPAddress address)
        {
            IPAddressEx addressEx = new IPAddressEx();
            addressEx.IPAsString = address.ToString();
            return addressEx;
        }

        public static Collection<IPAddress> ToIP(this IPAddressEx[] addresses)
        {
            var returnList = new Collection<IPAddress>();
            foreach (var address in addresses)
            {
                returnList.Add(IPAddress.Parse(address.IPAsString));
            }

            return returnList;
        }

        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.IndexOf(value, comparisonType) >= 0;
        }
    }
}
