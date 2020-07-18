using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Factory for creating <see cref="IAppAuthenticator" /> objects.
    /// </summary>
    internal class JediWindjammerAppAuthenticatorFactory : AppAuthenticatorFactoryCore
    {
        private static JediWindjammerAppAuthenticatorFactory _instance = new JediWindjammerAppAuthenticatorFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerAppAuthenticatorFactory" /> class.
        /// </summary>
        private JediWindjammerAppAuthenticatorFactory()
        {
            Add<JediWindjammerWindowsAuthenticator>(AuthenticationProvider.Windows);
            Add<JediWindjammerWindowsAuthenticator>(AuthenticationProvider.DSS);
            Add<JediWindjammerWindowsAuthenticator>(AuthenticationProvider.LDAP);
            Add<JediWindjammerCardAuthenticator>(AuthenticationProvider.Card);
            Add<JediWindjammerLocalDeviceAuthenticator>(AuthenticationProvider.LocalDevice);
            Add<JediWindjammerSafeComAuthenticator>(AuthenticationProvider.SafeCom);
            Add<JediWindjammerPinAuthenticator>(AuthenticationProvider.HpacDra);
            Add<JediWindjammerPinAuthenticator>(AuthenticationProvider.HpacIrm);
            Add<JediWindjammerHpacAgentlessAuthenticator>(AuthenticationProvider.HpacAgentLess);
            Add<JediWindjammerHpacWindowsAuthenticator>(AuthenticationProvider.HpacWindows);
            Add<JediWindjammerEquitracAuthenticator>(AuthenticationProvider.Equitrac);
            Add<JediWindjammerWindowsAuthenticator>(AuthenticationProvider.EquitracWindows);
            Add<JediWindjammerBlueprintAuthenticator>(AuthenticationProvider.Blueprint);
            Add<JediWindjammerAutoStoreAuthenticator>(AuthenticationProvider.AutoStore);
            Add<JediWindjammerPaperCutAuthenticator>(AuthenticationProvider.PaperCut);
        }

        /// <summary>
        /// Creates an <see cref="IAppAuthenticator" /> for the specified <see cref="AuthenticationProvider"/> value. 
        /// </summary>
        /// <param name="provider">The <see cref="AuthenticationProvider"/> value.</param>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        /// <returns></returns>
        public static IAppAuthenticator Create(AuthenticationProvider provider, JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            object[] constructorParameters = new object[] { controlPanel, credential, pacekeeper };
            return _instance.FactoryCreate(provider, constructorParameters);
        }
    }
}
