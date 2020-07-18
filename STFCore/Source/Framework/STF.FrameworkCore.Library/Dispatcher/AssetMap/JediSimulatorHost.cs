using System;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Dispatcher.DeviceSetup;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines an <see cref="AssetHost"/> that represents the Jedi simulator.  This includes
    /// information on the virtual machine running the simulator as well as the simulator
    /// itself.
    /// </summary>
    public class JediSimulatorHost : AssetHost
    {
        private CancellationTokenSource _cancel = null;
        private JediSimulatorDetail _simulatorDetail = null;
        private readonly ManagedMachine _machine = null;
        private readonly object _lockObject = new object();
        private bool _inShutdown = false;
        private volatile bool _isHandlingError = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediSimulatorHost"/> class.
        /// </summary>
        /// <param name="asset">The asset details from the manifest.</param>
        public JediSimulatorHost(AssetDetail asset)
            : base(asset, ((JediSimulatorDetail)asset).MachineName, ElementType.Device, "JediSimulator")
        {
            _simulatorDetail = (JediSimulatorDetail)asset;
            _machine = new ManagedMachine(_simulatorDetail.MachineName, ManagedMachineType.WindowsVirtual);
        }

        /// <summary>
        /// Gets the name associated with this host.
        /// </summary>
        public override string AssetName
        {
            get { return _simulatorDetail.MachineName; }
        }

        /// <summary>
        /// Handles the runtime error for this host.
        /// </summary>
        /// <param name="error">The error information.</param>
        public override bool HandleError(RuntimeError error)
        {
            TraceFactory.Logger.Debug(error.ErrorId);

            lock (_lockObject)
            {
                // Determine if the error is already being handled, if so then return,
                // if not, set the flag that this thread will take care of it.
                if (!_isHandlingError)
                {
                    TraceFactory.Logger.Debug("Handling error set true");
                    // Turn on indicator that this thread is already handling the error
                    _isHandlingError = true;
                }
                else
                {
                    // The error is already being handled, so return
                    TraceFactory.Logger.Debug("Error for {0} is already being handled, returning...".FormatWith(_machine.Name));
                    return true;
                }
            }
            bool success;
            // Attempt to power up the simulator, indicate failure if it doesn't work.
            if (!PowerUpSimulator())
            {
                MapElement.UpdateStatus("Recovery Failed", RuntimeState.Error);
                success = false;
            }
            else
            {
                MapElement.UpdateStatus("Running");
                success = true;
            }
            
            // Turn off indicator that a thread is already handling the error
            lock (_lockObject)
            {
                TraceFactory.Logger.Debug("HandlingError = false");
                _isHandlingError = false;
            }

            return success;
        }

        /// <summary>
        /// Restarts this asset.
        /// </summary>
        public override void Restart()
        {
            // Attempt to power up the simulator, indicate failure if it doesn't work.
            if (!PowerUpSimulator())
            {
                MapElement.UpdateStatus("Recovery Failed", RuntimeState.Error);
            }
            else
            {
                MapElement.UpdateStatus("Running");
            }
        }

        /// <summary>
        /// Validates this asset host
        /// </summary>
        /// <param name="loopState"></param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            if (MapElement.State == RuntimeState.Validated)
            {
                return;
            }
            
            Validate(loopState);
        }

        /// <summary>
        /// Initializes this asset
        /// </summary>
        public override void Validate(ParallelLoopState loopState)
        {
            TraceFactory.Logger.Debug("Validating {0}".FormatWith(_machine.Name));
            try
            {
                _inShutdown = false;

                MapElement.UpdateStatus("Validating", RuntimeState.Validating);

                if (_machine.IsPoweredOn())
                {
                    TraceFactory.Logger.Debug("{0} is already powered on...".FormatWith(_machine.Name));

                    if (JediSimulatorManager.IsSimulatorReady(_machine.Name))
                    {
                        MapElement.UpdateStatus("Simulator is already powered on and ready", RuntimeState.Validated);
                    }
                    else
                    {
                        MapElement.UpdateStatus("Simulator is in an unknown state and will be restarted", RuntimeState.Warning);
                    }
                }
                else
                {
                    MapElement.UpdateStatus("Simulator is powered off and will be started", RuntimeState.Validated);
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Validation failure: {0}".FormatWith(ex.Message), ex);
            }
        }

        /// <summary>
        /// Turns on this asset (bootup, power on, etc.).
        /// </summary>
        public override void PowerUp(ParallelLoopState loopState)
        {
            TraceFactory.Logger.Debug("Powerup called by {0}.".FormatWith(_machine.Name));
            if (!PowerUpSimulator())
            {
                loopState.Break();
            }
            LogDeviceInformation();
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

                SessionDeviceSetupManager.GetSessionDeviceInfo(sessionDeviceLogger, _simulatorDetail.Address, GlobalSettings.Items[Setting.DeviceAdminPassword]);
                ExecutionServices.DataLogger.AsInternal().Submit(sessionDeviceLogger);
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error gathering device information from DAT.", RuntimeState.Warning);
                TraceFactory.Logger.Error("Error gathering device information from DAT", ex);
            }
        }

        private bool PowerUpSimulator()
        {
            TraceFactory.Logger.Debug("Booting {0}".FormatWith(_machine.Name));

            int attempts = 0;
            while (true)
            {
                MapElement.UpdateStatus("Starting", RuntimeState.Starting);

                lock (_lockObject)
                {
                    _cancel = new CancellationTokenSource();
                }

                try
                {
                    TraceFactory.Logger.Debug("Starting powerup task {0}.".FormatWith(_machine.Name));
                    // Start the boot process and wait for it to complete or to be cancelled
                    var task = new Task(() => StartSimulatorHandler());
                    task.Start();
                    task.Wait(_cancel.Token);

                    MapElement.UpdateStatus("Ready", RuntimeState.Ready);
                    return true;
                }
                catch (Exception ex)
                {
                    // Regardless of the exception, log it, update the status and cancel 
                    // any other threads that may be running.  Pause for a few seconds
                    // and try again.
                    TraceFactory.Logger.Debug("Error : {0}".FormatWith(ex.Message));

                    if (!_cancel.Token.IsCancellationRequested)
                    {
                        _cancel.Cancel();

                        if (attempts++ > 1)
                        {
                            MapElement.UpdateStatus("Failed: {0}".FormatWith(ex.Message));
                            return false;
                        }
                    }
                    else if (_inShutdown)
                    {
                        // An exception was received, and it probably came from the shutdown method
                        // that caused a cancellation and exception.  If in a shutdown, then
                        // return out with a true so that errors are not sent back.
                        return true;
                    }
                }
                finally
                {
                    lock (_lockObject)
                    {
                        if (_cancel != null)
                        {
                            _cancel.Dispose();
                            _cancel = null;
                        }
                    }
                }
            }
        }

        private void StartSimulatorHandler()
        {
            bool alreadyRetried = false;
            TraceFactory.Logger.Debug("Starting simulator {0}".FormatWith(_machine.Name));
            while (true)
            {
                try
                {
                    if (_machine.IsPoweredOn())
                    {
                        // The machine is already powered on, see if the simulator is responsive
                        // and handle accordingly.
                        TraceFactory.Logger.Debug("{0} is already powered on...".FormatWith(_machine.Name));

                        if (JediSimulatorManager.IsSimulatorReady(_machine.Name))
                        {
                            // The simulator is on and ready to go, just return
                            return;
                        }
                        else
                        {
                            // Since the machine is running, but the simulator doesn't respond
                            // attempt to stop the simulator
                            try
                            {
                                // Attempt to shutdown the simulator.  If this fails then log it and continue
                                TraceFactory.Logger.Debug("Attempting to shutdown the simulator");
                                JediSimulatorManager.ShutdownSimulator(_machine.Name);
                            }
                            catch (Exception ex)
                            {
                                TraceFactory.Logger.Debug("Simulator shutdown failed, error: {0}".FormatWith(ex.Message));
                            }
                        }
                    }
                    else
                    {
                        // The machine isn't on, so boot it and then try to start the simulator.
                        try
                        {
                            MachineStart.Run(_machine.Name, () =>
                            {
                                TraceFactory.Logger.Debug("Booting {0}".FormatWith(_machine.Name));
                                MapElement.UpdateStatus("Booting");

                                // Don't track "In Use" for simulator VMs, they are already handled through
                                // asset reservation.
                                _machine.PowerOn(setInUse: false);
                            });
                        }
                        catch (Exception ex)
                        {
                            TraceFactory.Logger.Error("Error booting virtual machine", ex);
                            MapElement.UpdateStatus("Error booting VM", RuntimeState.Error);
                            return;
                        }

                        MapElement.UpdateStatus("ReadyWait");
                        if (!VMController.WaitOnMachineAvailable(_machine.Name, _cancel.Token))
                        {
                            TraceFactory.Logger.Debug("Cancellation request received, returning...");
                            throw new OperationCanceledException("Cancellation received");
                        }
                    }

                    // Now the machine should be booted and ready to start the simulator.
                    MapElement.UpdateStatus("Starting");
                    JediSimulatorManager.LaunchSimulator(_machine.Name, ((JediSimulatorDetail)Asset).Product, _machine.Name,_cancel.Token);
                    return;
                }
                catch (Exception ex)
                {
                    // If an exception is hit trying to start on the first attempt, then try one
                    // more time after first shutting down the machine.
                    TraceFactory.Logger.Debug("Error starting simulator on {0}".FormatWith(_machine.Name));
                    if (alreadyRetried)
                    {
                        MapElement.UpdateStatus("Unable to start this simulator: {0}".FormatWith(ex.Message), RuntimeState.Error);
                        throw new OperationCanceledException("Cancellation received");
                    }
                    else
                    {
                        // Unable to start the simulator, so perform a hard restart and try one more time.
                        _machine.Shutdown();
                        alreadyRetried = true;
                    }
                }
            }
        }

        /// <summary>
        /// Shuts down this asset
        /// </summary>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            // If the shutdown option is set to not shutdown the simulator, then 
            // set the state to offline and return.
            if (!options.ShutdownDeviceSimulators)
            {
                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
                return;
            }

            _inShutdown = true;

            TraceFactory.Logger.Debug("Shutting down {0}".FormatWith(_machine.Name));
            MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

            // Cancel the deployment if it's starting up.
            CancelBootup();

            if (_machine.IsPoweredOn())
            {
                try
                {
                    JediSimulatorManager.ShutdownSimulator(((JediSimulatorDetail)Asset).MachineName);
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Debug("Simulator shutdown failed.  Continuing with machine shutdown. {0}".FormatWith(ex.Message));
                }

                _machine.Shutdown();
            }

            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        private bool CancelBootup()
        {
            bool cancelled = false;

            lock (_lockObject)
            {
                if (_cancel != null)
                {
                    TraceFactory.Logger.Info("Cancelling bootup for {0}".FormatWith(_machine.Name));
                    _cancel.Cancel();
                    cancelled = true;
                }
                else
                {
                    TraceFactory.Logger.Info("Skipping... {0} not currently booting".FormatWith(_machine.Name));
                }
            }

            return cancelled;
        }
    }
}