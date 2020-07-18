using System;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to shut down machines in a controlled fashion using a concurrent critical section.
    /// </summary>
    public static class MachineStop
    {
        private static readonly int _batchSize;

        static MachineStop()
        {
            _batchSize = int.Parse(GlobalSettings.Items[Setting.VmBootUpBatchSize]);
            TraceFactory.Logger.Debug("VM Stop Batch Size set to {0}".FormatWith(_batchSize));

        }

        public static void Run(string hostName, Action action)
        {
            GlobalLockToken token = new GlobalLockToken("MACH_SHUT_3E94", TimeSpan.FromDays(1), TimeSpan.FromMinutes(10));
            CriticalSection criticalSection = new CriticalSection(new DistributedLockManager(GlobalSettings.WcfHosts["Lock"]));
            criticalSection.RunConcurrent(token, action, _batchSize);
        }
    }
}