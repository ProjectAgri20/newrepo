using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Plugin.Authentication
{
    public sealed class AuthenticatorDriverFactory : DeviceFactoryCore<IAuthenticationDriver>
    {
        private static AuthenticatorDriverFactory _instance = new AuthenticatorDriverFactory();

        private AuthenticatorDriverFactory()
        {
            Add<JediOmniDevice, OmniAuthenticatorDriver>();
            Add<JediWindjammerDevice, WindjammerAuthenticatorDriver>();
            Add<SiriusUIv3Device, SiriusUIv3AuthenticationDriver>();
        }

        public static IAuthenticationDriver Create(IDevice device, string solutionButton, DeviceWorkflowLogger workflowLogger) => _instance.FactoryCreate(device, solutionButton, workflowLogger);
    }
}
