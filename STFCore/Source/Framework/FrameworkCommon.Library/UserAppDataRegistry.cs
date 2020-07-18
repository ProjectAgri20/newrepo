using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// This is an STF specific class used to start user information in a defined location for STF.
    /// </summary>
    /// <remarks>
    /// It is intended to to provide a version independent registry key to provide a location where
    /// changes to STF console application can be stored.  It appends data to the HKCU software location.
    /// </remarks>
    public static class UserAppDataRegistry
    {
        /// <summary>
        /// Retrieves the registry value associated with the specified name.
        /// </summary>
        /// <param name="valueName">The registry value to obtain</param>
        /// <returns>The value, or null if the specified name does not exist</returns>
        public static object GetValue(string valueName)
        {
            return GetValue(valueName, null);
        }

        /// <summary>
        /// Retrieves the STF registry value associated with the specified name.
        /// If the name is not found, returns a default value that you provide,
        /// or null if the specified key does not exist.
        /// </summary>
        /// <param name="valueName">The registry value to obtain</param>
        /// <param name="defaultValue">The default value to return if the registry value is not found</param>
        /// <returns>A default value that you provide, or null if the specified name does not exist</returns>
        public static object GetValue(string valueName, object defaultValue)
        {
            return Registry.GetValue(SubKey, valueName, defaultValue);
        }

        /// <summary>
        /// Sets the name/value pair in the STF registry location.
        /// </summary>
        /// <param name="valueName"></param>
        /// <param name="value"></param>
        public static void SetValue(string valueName, object value)
        {
            Registry.SetValue(SubKey, valueName, value);
        }

        private static string SubKey
        {
            get
            {
                StringBuilder subKey = new StringBuilder(@"HKEY_CURRENT_USER\Software\");
                subKey.Append(AssemblyCompany).Append("\\");
                subKey.Append(AssemblyProduct);
                return subKey.ToString();
            }
        }

        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        /// <value>The assembly product.</value>
        private static string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if (attributes.Length == 0)
                {
                    return "HP Scalable Test Framework";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets the assembly company.
        /// </summary>
        /// <value>The assembly company.</value>
        private static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                if (attributes.Length == 0)
                {
                    return "Hewlett-Packard";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
    }
}