using System;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Oz;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication.JediOmni;
using HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer;
using HP.ScalableTest.DeviceAutomation.Authentication.OzWindjammer;
using HP.ScalableTest.DeviceAutomation.Authentication.PhoenixMagicFrame;
using HP.ScalableTest.DeviceAutomation.Authentication.PhoenixNova;
using HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv2;
using HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.DeviceAutomation.Authentication
{
    /// <summary>
    /// Factory for creating <see cref="IAuthenticator" /> objects.
    /// </summary>
    public sealed class AuthenticatorFactory : DeviceFactoryCore<IAuthenticator>
    {
        private static AuthenticatorFactory _instance = new AuthenticatorFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticatorFactory" /> class.
        /// </summary>
        private AuthenticatorFactory()
        {
            Add<JediOmniDevice, JediOmniAuthenticator>();
            Add<JediWindjammerDevice, JediWindjammerAuthenticator>();
            Add<OzWindjammerDevice, OzWindjammerAuthenticator>();
            Add<PhoenixMagicFrameDevice, PhoenixMagicFrameAuthenticator>();
            Add<PhoenixNovaDevice, PhoenixNovaAuthenticator>();
            Add<SiriusUIv2Device, SiriusUIv2Authenticator>();
            Add<SiriusUIv3Device, SiriusUIv3Authenticator>();
        }

        /// <summary>
        /// Creates an <see cref="IAuthenticator" /> for the specified <see cref="IDevice" />
        /// with a default AuthenticationProvider.Auto.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <returns><see cref="IAuthenticator" /></returns>
        public static IAuthenticator Create(IDevice device, AuthenticationCredential credential)
        {
            return Create(device, credential, AuthenticationProvider.Auto);
        }

        ///<summary>
        /// Creates an <see cref="IAuthenticator" /> for the specified <see cref="IDevice" />
        /// with a default AuthenticationProvider.Auto.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <returns><see cref="IAuthenticator" /></returns>
        public static IAuthenticator Create(IDevice device, NetworkCredential credential)
        {
            return Create(device, credential, AuthenticationProvider.Auto);
        }

        /// <summary>
        /// Creates an <see cref="IAuthenticator" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The Provider.</param>
        /// <returns><see cref="IAuthenticator" /></returns>
        public static IAuthenticator Create(IDevice device, NetworkCredential credential, AuthenticationProvider provider)
        {
            AuthenticationCredential authCredential = new AuthenticationCredential(credential);
            return Create(device, authCredential, provider);
        }

        /// <summary>
        /// Creates an <see cref="IAuthenticator" /> for the specified <see cref="IDevice" /> and <see cref="PluginExecutionData" />.
        /// Handles setting of <see cref="BadgeBoxInfo" /> if <see cref="AuthenticationProvider" /> is Card.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="device">The device.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="pluginExecutionData">The Execution data.</param>
        /// <returns><see cref="IAuthenticator" /></returns>
        public static IAuthenticator Create(string deviceId, IDevice device, AuthenticationProvider provider, PluginExecutionData pluginExecutionData)
        {
            AuthenticationCredential authCredential = null;
            switch (provider)
            {
                case AuthenticationProvider.Card:
                    authCredential = new AuthenticationCredential(pluginExecutionData, deviceId, device.Address);
                    break;
                default:
                    authCredential = new AuthenticationCredential(pluginExecutionData.Credential);
                    break;
            }

            return Create(device, authCredential, provider);
        }

        /// <summary>
        /// Creates an <see cref="IAuthenticator" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The Provider.</param>
        /// <returns><see cref="IAuthenticator" /></returns>
        public static IAuthenticator Create(IDevice device, AuthenticationCredential credential, AuthenticationProvider provider)
        {
            switch (provider)
            {
                case AuthenticationProvider.Skip:
                    return CreateSkipAuthenticator(device, credential);
                default:
                    return _instance.FactoryCreate(device, credential, provider);
            }
        }

        private static IAuthenticator CreateSkipAuthenticator(IDevice device, AuthenticationCredential credential)
        {
            Type deviceType = device.GetType();

            if (deviceType == typeof(JediOmniDevice))
            {
                return new JediOmniSkipAuthenticator(device, credential);
            }

            throw new ArgumentException($"{deviceType.Name} does not have a SkipAuthenticator implementation.");
        }
    }
}
