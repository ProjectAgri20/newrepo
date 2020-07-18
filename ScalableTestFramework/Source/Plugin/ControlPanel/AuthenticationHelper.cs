using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.Copy;
using HP.ScalableTest.DeviceAutomation.NativeApps.Email;
using HP.ScalableTest.DeviceAutomation.NativeApps.Fax;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Dss;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    internal static class AuthenticationHelper
    {
        internal static void LaunchApp(IDevice device, string appName, IAuthenticator auth)
        {
            switch (appName)
            {
                case "E-mail":
                    var app = EmailAppFactory.Create(device);
                    app.Launch(auth, AuthenticationMode.Lazy);
                    break;

                case "Fax":
                    var appFax = FaxAppFactory.Create(device);
                    appFax.Launch(auth, AuthenticationMode.Lazy);
                    break;

                case "Workflow":
                    var appWorkflow = DssWorkflowAppFactory.Create(device);
                    appWorkflow.Launch(auth, AuthenticationMode.Lazy);
                    break;

                case "Save to Network Folder":
                    var appFolder = NetworkFolderAppFactory.Create(device);
                    appFolder.Launch(auth, AuthenticationMode.Lazy);
                    break;

                case "Copy":
                    var app1 = CopyAppFactory.Create(device);
                    app1.Launch(auth, AuthenticationMode.Lazy);
                    break;

                default:
                    throw new DeviceWorkflowException($"Unknown application: {appName}");
            }
        }
    }
}
