using System;
using System.Net.Sockets;
using System.Text;
using HP.DeviceAutomation.Jedi;
using System.Threading;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class PrintExerciser
    {
        private readonly DirtyDeviceManager _owner;
        private readonly JediDevice _device;

        public int PrintJobCount { get; } = 64;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintExerciser" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal PrintExerciser(DirtyDeviceManager owner, JediDevice device)
        {
            _owner = owner;
            _device = device;
        }

        public void Exercise(DirtyDeviceActivityData activityData, int jobCountGoal)
        {
            for (int jobCount = 0; jobCount < jobCountGoal; jobCount++)
            {
                var progressMessage = $"{jobCount + 1,3}/{jobCountGoal}";
                var helloBytes = Encoding.ASCII.GetBytes($"Hello world! (print job {progressMessage})" + Environment.NewLine);
                
                try
                {
                    using (var tcpClient = new TcpClient(_device.Address, 9100))
                    using (var stream = tcpClient.GetStream())
                    {
                        stream.Write(helloBytes, 0, helloBytes.Length);
                    }
                }
                catch (Exception)
                {
                    _owner.OnUpdateStatus(this, $"Exception sending print job {progressMessage}");
                    throw;
                }
                Thread.Sleep(1000);
            }
        }

        internal void Exercise(DirtyDeviceActivityData activityData)
        {
            Exercise(activityData, PrintJobCount);
        }
    }
}
