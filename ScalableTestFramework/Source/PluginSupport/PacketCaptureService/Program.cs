﻿using System.ServiceProcess;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.PluginSupport.PacketCaptureService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.Initialize(new CtcServiceLogger());

            ServiceBase.Run(new PacketCaptureService());
        }
    }
}
