using DartLogCollectorService.BashLogCollector;
using HP.ScalableTest;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.DartLog;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace DartLogCollectorService
{
    /// <summary>
    /// Windows service that collects dart logs using dart.exe
    /// </summary>
    public class DartLogWindowsCollector : SelfInstallingServiceBase
    {
        private ServiceHost _logCollectionService;
        private ServiceHost _bashLogCollectionWindowsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DartLogWindowsCollector"/> class
        /// </summary>
        public DartLogWindowsCollector()
            : base("DartLogCollectionService", "STF Dart Log Collection Service")
        {
            this.Description = "STF Dart Log Collection Service";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            Thread.CurrentThread.SetName("Main");
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            TraceFactory.Logger.Debug("Load settings");
            // Load the settings, either from the command line or the local cache
            FrameworkServiceHelper.LoadSettings(args);

            StringBuilder exePath = new StringBuilder(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            exePath.Append("\\dart.exe");
            File.WriteAllBytes(exePath.ToString(), Properties.Resources.Dart);

            TraceFactory.Logger.Debug("Load settings - Complete");


            _logCollectionService = new WcfHost<IDartLogCollectorService>(
                typeof(DartCollectorService),
                MessageTransferType.Http,
                WcfService.CollectDartLogService.GetLocalHttpUri());

            _logCollectionService.Open();

            _bashLogCollectionWindowsService = new WcfHost<IBashLogCollectorService>(
               typeof(BashLogCollectorService),
               MessageTransferType.Http,
               WcfService.BashLogcollectorService.GetLocalHttpUri());

            _bashLogCollectionWindowsService.Open();
        }


        protected override void StopService()
        {
            if (_logCollectionService.State != CommunicationState.Closed)
            {
                _logCollectionService.Close();
            }

            if (_bashLogCollectionWindowsService.State != CommunicationState.Closed)
            {
                _bashLogCollectionWindowsService.Close();
            }
        }
    }
}


