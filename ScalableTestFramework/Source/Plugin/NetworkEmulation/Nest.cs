using System;
using Microsoft.VisualStudio.QualityTools.NetworkEmulation;

namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    static class NestService
    {
        static NetworkEmulationDriver nDriver = null;

        public static void Initialize()
        {
            if (nDriver == null)
            {
                try
                {
                    nDriver = new NetworkEmulationDriver();
                    if (nDriver.Initialize())
                    {
                        Framework.Logger.LogDebug("Network Emulator initialized");
                    }
                    else
                    {
                        Framework.Logger.LogDebug("Network Emulator failed to initialize");
                    }
                }
                catch (NetworkEmulationDriverNotInstalledException eInstalled)
                {
                    Framework.Logger.LogDebug(eInstalled.Message);
                    throw new Exception("Emulator Driver not installed", eInstalled);
                }
            }
        }

        public static bool StartEmulation(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                nDriver.LoadProfile(fileName);
                return nDriver.StartEmulation();
            }
            else
            {
                return false;
            }

        }

        public static bool StopEmulation()
        {
            return nDriver.StopEmulation();
        }
    }
}
