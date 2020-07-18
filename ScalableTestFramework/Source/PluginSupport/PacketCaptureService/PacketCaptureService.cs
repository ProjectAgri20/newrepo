using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;

namespace HP.ScalableTest.PluginSupport.PacketCaptureService
{
    internal class PacketCaptureService : ServiceBase
    {
        public ServiceHost _serviceHost = null;
        public Timer _timer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCaptureService"/> class.
        /// </summary>
        public PacketCaptureService()
        {
            ServiceName = "PacketCaptureService";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Logger.LogDebug("Starting Packet Capture Service");

            _serviceHost = new WcfHost<IPacketCapture>(
                    typeof(PacketCapture),
                    MessageTransferType.Http,
                    PacketCaptureServiceClient.BuildUri("localhost"));

            _serviceHost.Open();

            // starting the timer
            _timer = new Timer(TimeSpan.FromDays(1).TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();

            Logger.LogDebug("Stopping Packet Capture Service");

            if (_serviceHost.State != CommunicationState.Closed)
            {
                _serviceHost.Close();
            }

            _timer.Stop();
        }

        /// <summary>
        /// Cleans the older packets, this event raises every day from the time service is started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.LogInfo("Cleanup process started");

            string basePath = ConfigurationManager.AppSettings["PacketsBaseLocation"];

            double days = double.Parse(ConfigurationManager.AppSettings["CleanUpOldPacketsInDays"]);

            foreach (string directory in Directory.EnumerateDirectories(basePath))
            {
                DirectoryInfo di = new DirectoryInfo(directory);

                // if the directory is older than the specified days delete
                if (DateTime.Now.Subtract(di.CreationTime).Days > days)
                {
                    Logger.LogInfo($"Cleaning up the directory {directory}");
                    Directory.Delete(directory, true);
                }
            }

            Logger.LogInfo("Cleanup process completed");
        }
    }
}