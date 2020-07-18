using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Factory for creating <see cref="IAppAuthenticator" /> objects.
    /// </summary>
    internal class JediOmniAppAuthenticatorFactory : AppAuthenticatorFactoryCore
    {
        private static JediOmniAppAuthenticatorFactory _instance = new JediOmniAppAuthenticatorFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniAppAuthenticatorFactory" /> class.
        /// </summary>
        private JediOmniAppAuthenticatorFactory()
        {
            Add<JediOmniWindowsAuthenticator>(AuthenticationProvider.Windows);
            Add<JediOmniWindowsAuthenticator>(AuthenticationProvider.DSS);
            Add<JediOmniWindowsAuthenticator>(AuthenticationProvider.LDAP);
            Add<JediOmniCardAuthenticator>(AuthenticationProvider.Card);
            Add<JediOmniLocalDeviceAuthenticator>(AuthenticationProvider.LocalDevice);
            Add<JediOmniSafeComAuthenticator>(AuthenticationProvider.SafeCom);
            Add<JediOmniSafeComUCAuthenticator>(AuthenticationProvider.SafeComUC);
            Add<JediOmniPinAuthenticator>(AuthenticationProvider.HpacDra);
            Add<JediOmniPinAuthenticator>(AuthenticationProvider.HpacIrm);
            Add<JediOmniHpacAgentlessAuthenticator>(AuthenticationProvider.HpacAgentLess);
            Add<JediOmniPinAuthenticator>(AuthenticationProvider.HpacPic);
            Add<JediOmniHpacWindowsAuthenticator>(AuthenticationProvider.HpacWindows);
            Add<JediOmniEquitracAuthenticator>(AuthenticationProvider.Equitrac);
            Add<JediOmniWindowsAuthenticator>(AuthenticationProvider.EquitracWindows);
            Add<JediOmniBlueprintAuthenticator>(AuthenticationProvider.Blueprint);
            Add<JediOmniPaperCutAuthenticator>(AuthenticationProvider.PaperCut);
            Add<JediOmniISecStarAuthenticator>(AuthenticationProvider.ISecStar);
            Add<JediOmniHPRoamAuthenticator>(AuthenticationProvider.HpRoamPin);
            Add<JediOmniCeliveoAuthenticator>(AuthenticationProvider.Celiveo);
            Add<JediOmniSafeQAuthenticator>(AuthenticationProvider.SafeQ);
            Add<JediOmniGeniusBytesAuthenticator>(AuthenticationProvider.GeniusBytesManual);
            Add<JediOmniGeniusBytesAuthenticator>(AuthenticationProvider.GeniusBytesPin);
            Add<JediOmniGeniusBytesAuthenticator>(AuthenticationProvider.GeniusBytesGuest);
            Add<JediOmniMyQAuthenticator>(AuthenticationProvider.MyQ);
            Add<JediOmniHpacScanAuthenticator>(AuthenticationProvider.HpacScan);
            Add<JediOmniAutoStoreAuthenticator>(AuthenticationProvider.AutoStore);
            Add<JediOmniPaperCutAgentlessAuthenticator>(AuthenticationProvider.PaperCutAgentless);
            Add<JediOmniUdocxScanAuthenticator>(AuthenticationProvider.UdocxScan);
            Add<JediOmniHpIdAuthenticator>(AuthenticationProvider.HpId);
        }

        /// <summary>
        /// Creates an <see cref="IAppAuthenticator" /> for the specified <see cref="AuthenticationProvider"/> value. 
        /// </summary>
        /// <param name="provider">The <see cref="AuthenticationProvider"/> value.</param>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        /// <returns></returns>
        public static IAppAuthenticator Create(AuthenticationProvider provider, JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            object[] constructorParameters = new object[] { controlPanel, credential, pacekeeper };
            return _instance.FactoryCreate(provider, constructorParameters);
        }
    }
}
