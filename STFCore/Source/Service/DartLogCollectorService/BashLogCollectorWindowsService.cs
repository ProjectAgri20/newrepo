using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DartLogCollectorService.BashLogCollector;
using HP.ScalableTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.DartLog;
using HP.ScalableTest.Framework.Service;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace DartLogCollectorService
{
    partial class BashLogCollectorWindowsService : FrameworkServiceBase
    {
        private ServiceHost _bashLogCollectionWindowsService = null;
        public BashLogCollectorWindowsService(): base("BashLogCollectionService")
        {
            InitializeComponent();
        }

      protected override void StartService(string[] args)
        {
            Thread.CurrentThread.SetName("Main");
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            TraceFactory.Logger.Debug("Load settings for BashCollector");
            // Load the settings, either from the command line or the local cache
            LoadSettings(args);


            TraceFactory.Logger.Debug("Load settings for BashCollector - Complete");
            Console.ReadLine();

            _bashLogCollectionWindowsService = new WcfHost<IBashLogCollectorService>(
                typeof(BashLogCollectorService),
                MessageTransferType.Http,
                WcfService.BashLogcollectorService.GetLocalHttpUri());

            _bashLogCollectionWindowsService.Open();
        }

        protected override void StopService()
        {
            if (_bashLogCollectionWindowsService.State != CommunicationState.Closed)
            {
                _bashLogCollectionWindowsService.Close();
            }
        }
    }
}
