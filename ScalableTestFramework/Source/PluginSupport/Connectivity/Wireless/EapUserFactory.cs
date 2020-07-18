using System;
using System.IO;
using System.Reflection;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wireless
{
    /// <summary>
    /// EAP User factory class
    /// </summary>
    internal static class EapUserFactory
    {
        /// <summary>
        /// Generates the EAP user XML
        /// </summary>
        internal static string Generate(Dot11CipherAlgorithm cipher, string username, string password, string domain)
        {
            // #warning Probably not properly implemented, only supports WPA- and WPA-2 Enterprise with PEAP-MSCHAPv2

            string profile = string.Empty;
            string template = string.Empty;

            switch (cipher)
            {
                case Dot11CipherAlgorithm.CCMP: // WPA-2
                case Dot11CipherAlgorithm.TKIP: // WPA
                    template = GetTemplate("PEAP-MS-CHAPv2");

                    profile = string.Format(template, username, FixPass(password), domain);
                    break;
                default:
                    throw new NotImplementedException("Profile for selected cipher algorithm is not implemented");
            }

            return profile;
        }

        /// <summary>
        /// Fetches the template for an EAP user
        /// </summary>
        private static string GetTemplate(string name)
        {
            string resourceName = string.Format("HP.ScalableTest.PluginSupport.Connectivity.Wireless.EapUserXML.{0}.xml", name);

            using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Encodes to base 64 and fixes all the special characters &, <, >
        /// </summary>
        /// <param name="password">Password to be fixed</param>
        /// <returns>Returns fixed password</returns>
		private static string FixPass(string password)
        {
            password = EncodeToBase64(password);
            password = password.Replace("&", "&#038;");
            password = password.Replace("<", "&#060;");
            password = password.Replace(">", "&#062;");

            return password;
        }

        /// <summary>
        /// Encodes to given string to base 64.
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
		private static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
