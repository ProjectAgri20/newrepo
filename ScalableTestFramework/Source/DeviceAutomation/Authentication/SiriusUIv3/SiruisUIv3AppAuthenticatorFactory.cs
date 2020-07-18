using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Factory for creating <see cref="IAppAuthenticator" /> objects.
    /// </summary>
    internal class SiriusUIv3AppAuthenticatorFactory : AppAuthenticatorFactoryCore
    {
        private static SiriusUIv3AppAuthenticatorFactory _instance = new SiriusUIv3AppAuthenticatorFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3AppAuthenticatorFactory" /> class.
        /// </summary>
        private SiriusUIv3AppAuthenticatorFactory()
        {
            Add<SiriusUIv3WindowsAuthenticator>(AuthenticationProvider.Windows);
            Add<SiriusUIv3WindowsAuthenticator>(AuthenticationProvider.DSS);
            Add<SiriusUIv3WindowsAuthenticator>(AuthenticationProvider.LDAP);
            Add<SiriusUIv3CardAuthenticator>(AuthenticationProvider.Card);
            Add<SiriusUIv3SafeComAuthenticator>(AuthenticationProvider.SafeCom);
            Add<SiriusUIv3HpacIrmAuthenticator>(AuthenticationProvider.HpacIrm);
            Add<SiriusUIv3PinAuthenticator>(AuthenticationProvider.Equitrac);
        }

        /// <summary>
        /// Creates an <see cref="IAppAuthenticator" /> for the specified <see cref="AuthenticationProvider"/> value. 
        /// </summary>
        /// <param name="provider">The <see cref="AuthenticationProvider"/> value.</param>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/>.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/>.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/>.</param>
        /// <returns></returns>
        public static IAppAuthenticator Create(AuthenticationProvider provider, SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            object[] constructorParameters = new object[] { controlPanel, credential, pacekeeper };
            return _instance.FactoryCreate(provider, constructorParameters);
        }
    }
}
