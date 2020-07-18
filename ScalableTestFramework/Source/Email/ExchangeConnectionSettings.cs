using System;
using System.Net.Mail;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Defines settings for connecting to an Exchange server using <see cref="ExchangeEmailController" />.
    /// </summary>
    public sealed class ExchangeConnectionSettings
    {
        /// <summary>
        /// Gets a value indicating whether EWS autodiscover is enabled.
        /// </summary>
        public bool AutodiscoverEnabled { get; } = true;

        /// <summary>
        /// Gets the email address to use for autodiscovering the Exchange Web Services URL.
        /// </summary>
        public MailAddress AutodiscoverEmail { get; }

        /// <summary>
        /// Gets the explicit Exchange Web Services URL to use.
        /// </summary>
        public Uri EwsUrl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeConnectionSettings" /> class
        /// with an email address that should be used to autodiscover the EWS URL.
        /// </summary>
        /// <param name="autodiscoverEmail">The email address to use for autodiscovering the Exchange Web Services URL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="autodiscoverEmail" /> is null.</exception>
        public ExchangeConnectionSettings(MailAddress autodiscoverEmail)
        {
            AutodiscoverEnabled = true;
            AutodiscoverEmail = autodiscoverEmail ?? throw new ArgumentNullException(nameof(autodiscoverEmail));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeConnectionSettings" /> class
        /// with a specific URL for Exchange Web Services.
        /// </summary>
        /// <param name="ewsUrl">The Exchange Web Services URL.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ewsUrl" /> is null.</exception>
        public ExchangeConnectionSettings(Uri ewsUrl)
        {
            AutodiscoverEnabled = false;
            EwsUrl = ewsUrl ?? throw new ArgumentNullException(nameof(ewsUrl));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeConnectionSettings" /> class
        /// using the settings from the specified <see cref="ServerInfo" /> object.
        /// </summary>
        /// <param name="exchangeServerInfo">A <see cref="ServerInfo" /> object with information about the Exchange server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exchangeServerInfo" /> is null.</exception>
        /// <exception cref="FormatException"><paramref name="exchangeServerInfo" /> contains invalid settings values.</exception>
        public ExchangeConnectionSettings(ServerInfo exchangeServerInfo)
            : this(exchangeServerInfo?.Settings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeConnectionSettings" /> class
        /// using the settings from the specified <see cref="SettingsDictionary" /> object.
        /// </summary>
        /// <param name="settings">A <see cref="SettingsDictionary" /> object with information about the Exchange server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is null.</exception>
        /// <exception cref="FormatException"><paramref name="settings" /> contains incorrectly formatted values.</exception>
        public ExchangeConnectionSettings(SettingsDictionary settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.TryGetValue("EwsAutodiscoverEnabled", out string autoDiscoverEnabledSetting))
            {
                AutodiscoverEnabled = bool.Parse(autoDiscoverEnabledSetting);
            }

            if (settings.TryGetValue("EwsUrl", out string ewsUrlSetting))
            {
                EwsUrl = new Uri(ewsUrlSetting);
            }
        }
    }
}
