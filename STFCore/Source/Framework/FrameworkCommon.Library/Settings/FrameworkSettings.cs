using System.Net;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// A collection of framework settings.
    /// </summary>
    [DataContract]
    public class FrameworkSettings : SettingsCollectionBase
    {
        /// <summary>
        /// Gets the value of the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public string this[string setting]
        {
            get { return GetSetting(setting); }
        }

        /// <summary>
        /// Gets the value of the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public string this[Setting setting]
        {
            get { return GetSetting(setting.ToString()); }
        }

        /// <summary>
        /// Adds the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        public void Add(string setting, string value)
        {
            this.AddValue(setting, value);
        }

        /// <summary>
        /// Adds the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="value">The value.</param>
        public void Add(Setting setting, string value)
        {
            this.Add(setting.ToString(), value);
        }

        /// <summary>
        /// Gets the domain administrator's credential.
        /// </summary>
        public NetworkCredential DomainAdminCredential
        {
            get
            {
                return new NetworkCredential(
                   domain: this[Setting.Domain],
                   userName: this[Setting.DomainAdminUserName],
                   password: this[Setting.DomainAdminPassword]);
            }
        }
    }
}
