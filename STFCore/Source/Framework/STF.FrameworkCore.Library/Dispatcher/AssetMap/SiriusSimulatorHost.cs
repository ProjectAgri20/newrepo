using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using HP.ScalableTest.Framework.Dispatcher.DeviceSetup;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines an <see cref="AssetHost"/> that represents a Sirius simulator.
    /// Treats the simulator as if it was an actual device.  Expects host VM to be powered on.
    /// </summary>
    public class SiriusSimulatorHost : AssetHost
    {
        private SiriusSimulatorDetail _simulatorDetail = null;
        private SiriusDevice _device = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusSimulatorHost"/> class.
        /// </summary>
        /// <param name="asset">The asset details from the manifest.</param>
        public SiriusSimulatorHost(AssetDetail asset)
            : base(asset, ((SiriusSimulatorDetail)asset).AssetId, ElementType.Device, "SiriusSimulator")
        {
            _simulatorDetail = (SiriusSimulatorDetail)asset;

        }

        /// <summary>
        /// Checks this simulator instance to verify it is responsive.
        /// </summary>
        public override void Validate(ParallelLoopState loopState)
        {
            TraceFactory.Logger.Debug("Validating {0}".FormatWith(_simulatorDetail.AssetId));

            // Using DAT to validate because the host machine could be running with the simulators not running.
            // The host machine hosts the IP addresses of the simulators, so they will respond to a ping in that case.
            // Using DAT will return data about the actual simulator.
            try
            {
                _device = new SiriusDevice(_simulatorDetail.Address);

                if (IsSimulatorReady())
                {
                    MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                }
                else
                {
                    MapElement.UpdateStatus("Sirius Simulator is in an unknown state", RuntimeState.Warning);                    
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Validation failure: {0}".FormatWith(ex.Message), ex);
            }
        }

        /// <summary>
        /// Logs device information for this simulator.
        /// </summary>
        /// <param name="loopState"></param>
        public override void PowerUp(ParallelLoopState loopState)
        {
            // Log Device Info for the simulator.
            LogDeviceInformation();

            //There is no reason to hold the device instance open for the duration of the test.
            _device?.Dispose();
            _device = null;

            MapElement.UpdateStatus("Ready", RuntimeState.Ready);
        }

        private void LogDeviceInformation()
        {
            try
            {
                SessionDeviceLogger sessionDeviceLogger = new SessionDeviceLogger()
                {
                    SessionId = MapElement.SessionId,
                    DeviceId = _simulatorDetail.AssetId,
                    ProductName = "SIM [{0}]".FormatWith(_simulatorDetail.Product),
                    IpAddress = _simulatorDetail.Address

                };

                // Treating sim like a real device here.
                SessionDeviceSetupManager.GetSessionDeviceInfo(sessionDeviceLogger, _device);
                ExecutionServices.DataLogger.AsInternal().Submit(sessionDeviceLogger);
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error gathering device information from DAT", RuntimeState.Warning);
                TraceFactory.Logger.Error("Error gathering device information from DAT.", ex);
            }
        }

        /// <summary>
        /// Determines whether the simulator is ready.
        /// </summary>
        /// <returns><c>true</c> if the simulator is ready; otherwise, <c>false</c>.</returns>
        private bool IsSimulatorReady()
        {
            return _device.GetDeviceStatus() == DeviceStatus.Running &&
                   _device.GetPrinterStatus() >= PrinterStatus.Idle;
        }

    }
}
