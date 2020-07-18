using System;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    public static class MachineStart
    {
        private static readonly int _batchSize;

        static MachineStart()
        {
            _batchSize = int.Parse(GlobalSettings.Items[Setting.VmBootUpBatchSize]);
            TraceFactory.Logger.Debug("VM Start Batch Size set to {0}".FormatWith(_batchSize));
        }

        public static void Run(string hostName, Action action)
        {
            GlobalLockToken token = new GlobalLockToken("MACHINE_START_B58B", TimeSpan.FromDays(1), TimeSpan.FromMinutes(10));
            ExecutionServices.CriticalSection.RunConcurrent(token, action, _batchSize);
        }
    }
}