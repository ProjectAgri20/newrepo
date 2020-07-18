using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Dispatcher.DeviceEventLog;
using HP.ScalableTest.Framework.Dispatcher.DeviceSetup;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// An <see cref="AssetHost" /> that represents any printing device used
    /// within a test session.
    /// </summary>
    public class PrintDeviceHost : AssetHost
    {
        private readonly PrintDeviceDetail _deviceDetail;
        private IDevice _device = null;
        private readonly object _lockObject = new object();
        private volatile bool _isHandlingError = false;
        private List<DateTime> _errorTimeStamp;
        


        /// <summary>
        /// device event log collection start up time
        /// </summary>
        public DateTime StartupTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceHost"/> class.
        /// </summary>
        /// <param name="asset">The asset details from the manifest.</param>
        public PrintDeviceHost(AssetDetail asset)
            : base(asset, asset.AssetId, ElementType.Device, "PrintDevice")
        {
            _deviceDetail = asset as PrintDeviceDetail;
            StartupTime = DateTime.Now;
            _errorTimeStamp = new List<DateTime>();
        }

        /// <summary>
        /// Re-validates this asset host
        /// </summary>
        /// <param name="loopState"></param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            if (MapElement.State == RuntimeState.Validated)
            {
                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                return;
            }
            Validate(loopState);
        }

        /// <summary>
        /// Validates this asset host.  
        /// Validation failure for any device results in a warning, allowing the user to continue with the test, if desired.
        /// </summary>
        public override void Validate(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Validating", RuntimeState.Validating);
            StringBuilder statusMessage = new StringBuilder();

            try
            {
                //Veda: since the IP address is used whether the device has SNMP enabled or not, extracting it outside the if else block
                //setup a printerIP field and extract the incoming address, this will succeed if the address is in IPv4 or IPv6 format
                //if it fails then use DNS host entry to resolve the IP and get the IP address
                IPAddress printerAddress;

                if (!IPAddress.TryParse(_deviceDetail.Address, out printerAddress))
                {
                    printerAddress = GetDeviceAddressFromHostName(_deviceDetail.Address);
                    _deviceDetail.Address = printerAddress?.ToString();
                }

                if (_deviceDetail.Capability.HasFlag(AssetAttributes.ControlPanel) || _deviceDetail.Capability.HasFlag(AssetAttributes.Scanner))
                {
                    if (ValidatePrintDevice())
                    {
                        //Connection succeeded to the device.
                        if (! SetPaperlessPrintMode(_deviceDetail.UseCrc))
                        {
                            statusMessage.Append("Paperless Mode");
                        }

                        if (!ValidateDartCard())
                        {
                            if (statusMessage.Length > 0)
                            {
                                statusMessage.Append(", ");
                            }
                            statusMessage.Append("Dart Card");
                        }

                        CollectDeviceMemoryProfile("Session Start", true);
                    }
                    else
                    {
                        statusMessage.Append("Device Connection");
                    }
                }
                else
                {
                    if (!ValidateVirtualPrintDevice(printerAddress))
                    {
                        statusMessage.Append("Virtual Device Connection");
                    }
                }
            }
            catch (Exception ex)
            {
                if (statusMessage.Length > 0)
                {
                    statusMessage.Append("Device Validation");
                }
                TraceFactory.Logger.Error($"Device validation error {_deviceDetail.AssetId}.", ex);
            }

            if (statusMessage.Length > 0)
            {
                statusMessage.Append(" Error");
                MapElement.UpdateStatus(statusMessage.ToString(), RuntimeState.Warning);
                return;
            }

            // If we end up here, validation succeeded.
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            TraceFactory.Logger.Debug($"Device {_deviceDetail.AssetId} ready.");            
        }

        /// <summary>
        /// Turns on this asset host [print device only] (bootup, power on, etc.).
        /// </summary>
        /// <param name="loopState"></param>
        public override void PowerUp(ParallelLoopState loopState)
        {
            // Get Device Info for Physical devices.
            if (_deviceDetail.SnmpEnabled)
            {
                LogDeviceInformation();
            }

            //There is no reason to hold the device instance open for the duration of the test.
            _device?.Dispose();
            _device = null;

            MapElement.UpdateStatus("Ready", RuntimeState.Ready);
        }

        /// <summary>
        /// Handle Asset errors happening during tests
        /// </summary>
        /// <param name="error">Error</param>
        /// <returns>bool whether or not error is being handled.</returns>
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
                    TraceFactory.Logger.Debug("Error for is already being handled, returning...");
                    return true;
                }
            }

            //Keep track of the number of unhandled errors and how far appart they are from each other.
            //If we get unhandled error less than 15 minutes appart from each other then we keep track of that. 
            _errorTimeStamp.Add(DateTime.Now);
            _errorTimeStamp.RemoveAll(d => (DateTime.Now - d).Minutes > 15);
            
            //Track whether or not this error is being handled internally. 
            //If not then consider that an unhandled error that needs to be used to decide whether or not to take the asset offline. 
            bool success = true;
            //If we notice more than 3 unhandled error less than 5 minutes appart from each other, 
            //than trigger an error report to take the asset offline. 
            if (_errorTimeStamp.Count >= 3)
            {
                success = false;
            }                
              
            //Turn off indicator that a thread is already handling the error
            lock (_lockObject)
            {
                TraceFactory.Logger.Debug("HandlingError = false");
                _isHandlingError = false;
            }

            return success;
        }

        public override bool CanResumeActivity()
        {
            try
            {
                // Try to contact the device - if it is at least moderately reponsive, take it out of the penalty box
                TraceFactory.Logger.Debug($"Checking to see if device {_deviceDetail.AssetId} is responsive.");
                using (IDevice device = DeviceConstructor.Create(GetDeviceInfo()))
                {
                    _errorTimeStamp.Clear();
                    return true;
                }
            }
            catch
            {
                // Nope - device is not responsive
                return false;
            }
        }

        /// <summary>
        /// Mark this asset as Completed.
        /// </summary>
        public override void Completed()
        {
            //Call on the AssetHost completed() method first to mark the asset as completed. 
            base.Completed();

            //Make sure that when repeat session is called we start with a clean slate. 
            _errorTimeStamp.Clear(); 
        }

        private bool IsJediDevice
        {
            get { return _device?.FirmwareType?.ToLower() == "jedi"; }
        }

        private void LogDeviceInformation()
        {
            try
            {
                SessionDeviceLogger sessionDeviceLogger = new SessionDeviceLogger()
                {
                    SessionId = MapElement.SessionId,
                    DeviceId = _deviceDetail.AssetId,
                    ProductName = _deviceDetail.Product,
                    IpAddress = _deviceDetail.Address
                };

                try
                {
                    Retry.WhileThrowing<Exception>(() =>
                    {
                        SessionDeviceSetupManager.GetSessionDeviceInfo(sessionDeviceLogger, _device);

                    }, 3, TimeSpan.FromSeconds(30));

                }
                catch (Exception ex)
                {
                    MapElement.UpdateStatus("Error gathering device information from DAT.", RuntimeState.Warning);
                    TraceFactory.Logger.Error("Error gathering device information from DAT", ex);
                }
                finally
                {
                    ExecutionServices.DataLogger.AsInternal().Submit(sessionDeviceLogger);
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error($"Failed to add device {_deviceDetail.AssetId} to SessionDevice table.", ex);
            }
        }

        /// <summary>
        /// Shuts down the printer asset once the scenario is ended
        /// </summary>
        /// <param name="options">shutdown options</param>
        /// <param name="loopState">loop state parameter</param>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            try
            {
                _device = DeviceConstructor.Create(GetDeviceInfo());
            }
            catch (DeviceCommunicationException)
            {
                TraceFactory.Logger.Error($"An error occurred trying to shutdown device {_deviceDetail.AssetId}, please check the device");
                return;
            }
            if (_deviceDetail.SnmpEnabled && options.DisableDeviceCrc)
            {
                if (!string.IsNullOrEmpty(_deviceDetail.Address))
                {
                    SetPaperlessPrintMode(false);
                    TraceFactory.Logger.Debug("CRC mode is now disabled");
                }
            }

            if (options.CollectDeviceEventLogs && IsJediDevice)
            {
                TraceFactory.Logger.Debug("Collecting device event logs on or after " + StartupTime + ".");
                GetJediEventLogs();
            }
            CollectDeviceMemoryProfile("Session End", false);

            _device?.Dispose();
            _device = null;
            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        /// <summary>
        /// Process to spin out the Jedi memory collection on its own thread.
        /// </summary>
        /// <param name="snapShotLabel">The snap shot label.</param>
        /// <param name="startNewThread">Runs it as threaded operation</param>
        private void CollectDeviceMemoryProfile(string snapShotLabel, bool startNewThread)
        {
            // only valid for Jedi devices
            if (IsJediDevice)
            {
                TraceFactory.Logger.Debug($"Collecting Memory for label '{snapShotLabel}'");
                if (startNewThread)
                {
                    Task.Factory.StartNew(() => CollectMemoryHandler(snapShotLabel));                    
                }
                else
                {
                    CollectMemoryHandler(snapShotLabel);
                }
            }
            else
            {
                TraceFactory.Logger.Debug($"Device {_deviceDetail.AssetId} at {_deviceDetail.Address} does not evaluate as a Jedi device.");
            }
        }

        private bool ValidatePrintDevice()
        {
            try
            {
                //DAT will attempt to communicate with the device.
                _device = DeviceConstructor.Create(GetDeviceInfo());
                TraceFactory.Logger.Debug("Device communication successful: {0}".FormatWith(_deviceDetail.AssetId));
                return true;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error($"Failed to connect to device {_deviceDetail.AssetId}.", ex);
                return false;
            }
        }

        private bool ValidateVirtualPrintDevice(IPAddress printerAddress)
        {
            //Veda: using the print socket rather than tcp client as this is guaranteed to work on both IPv4 and IPv6
            Socket printerSocket = new Socket(printerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                printerSocket.Connect(printerAddress, _deviceDetail.PortNumber);
                return true;
            }
            catch (Exception ex)
            {
                StringBuilder logMessage = new StringBuilder("Device ");
                if (!string.IsNullOrWhiteSpace(_deviceDetail.Product))
                {
                    logMessage.Append("(");
                    logMessage.Append(_deviceDetail.Product);
                    logMessage.Append(") ");
                }
                logMessage.Append(_deviceDetail.AssetId).Append(":");
                logMessage.Append(_deviceDetail.Address).Append(":");
                logMessage.Append(_deviceDetail.PortNumber);
                logMessage.Append(" is unresponsive.  Check the host to ensure it is running.");

                TraceFactory.Logger.Error(logMessage.ToString(), ex);
                return false;
            }
            finally
            {
                printerSocket.Close();
                printerSocket.Dispose();
            }
        }

        private bool ValidateDartCard()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                DartBoard dartBoard = context.DartBoards.FirstOrDefault(n => n.PrinterId == _deviceDetail.AssetId);
                if (dartBoard != null)
                {
                    try
                    {
                        DartLog.DartLogCollectorClient client = new DartLog.DartLogCollectorClient();
                        client.Flush(dartBoard.Address);

                        return client.Start(dartBoard.Address);
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Failed to connect to dart card at {dartBoard.Address} associated with device {dartBoard.PrinterId}.", ex);
                        return false;
                    }
                }

                return true;
            }
        }

        private DeviceInfo GetDeviceInfo() => new DeviceInfo(_deviceDetail.AssetId, AssetAttributes.None, string.Empty, _deviceDetail.Address, _deviceDetail.Address2, _deviceDetail.AdminPassword, _deviceDetail.Product);

        /// <summary>
        /// Handler for collecting device memory.
        /// If IsDistributedSystem == false, the memory XML blob should NOT be saved.
        /// If we need to save the XML blob for STB, a new SystemSetting should be created to control this.  It should NOT be tied to EnterpriseEnabled.
        /// </summary>
        /// <param name="snapShotLabel">The snap shot label.</param>
        private void CollectMemoryHandler(string snapShotLabel)
        {
            string memoryPools = GlobalSettings.Items["MemoryPools"];

            try
            {
                JediMemoryRetrievalAgent jmr = new JediMemoryRetrievalAgent(GetDeviceInfo(), MapElement.SessionId, memoryPools, GlobalSettings.IsDistributedSystem);
                jmr.CollectDeviceMemoryCounters(snapShotLabel);
            }
            catch (DeviceWorkflowException ex )
            {
                TraceFactory.Logger.Debug($"Device memory collection failed for device {_deviceDetail.AssetId}." + ex);

            }            
        }

        /// <summary>
        /// Gets the Jedi event logs.
        /// </summary>
        private void GetJediEventLogs()
        {
            try
            {
                JediWsEventLogs etLogs = new JediWsEventLogs((JediDevice)_device, StartupTime);
                WSEventLogList listLogEvents = etLogs.RetrieveEventLogs();

                TraceFactory.Logger.Debug($"Received Device Event logs of count: {listLogEvents.Count}");
                if (listLogEvents.Count > 0)
                {
                    foreach (WSEventLog log in listLogEvents)
                    {
                        DeviceEventLogger dedl = new DeviceEventLogger
                        {
                            Address = _deviceDetail.Address,
                            AssetId = _deviceDetail.AssetId,
                            EventCode = log.EventCode,
                            EventDateTime = log.EventDateTime,
                            EventDescription = log.EventDescription,
                            EventType = log.EventType,
                            SessionId = MapElement.SessionId,
                            StartDateTime = StartupTime
                        };


                        ExecutionServices.DataLogger.AsInternal().Submit(dedl);
                    }
                }
            }
            catch (Exception ex)
            {
                MapElement.UpdateStatus("Error gathering device event logs.", RuntimeState.Warning);
                TraceFactory.Logger.Error("Error gathering device event logs", ex);
            }
        }

        /// <summary>
        /// Sets Paperless Print Mode to ON or OFF on the device
        /// </summary>
        /// <param name="paperlessModeOn"></param>
        /// <returns>Whether or not the operation succeeded.</returns>
        public bool SetPaperlessPrintMode(bool paperlessModeOn)
        {
            bool deviceInitializedHere = false;
            if (_device == null)
            {
                try
                {
                    //Since this is the only line which can throw if we throw _device will always be null
                    _device = DeviceConstructor.Create(GetDeviceInfo());
                    deviceInitializedHere = true;
                }
                catch (DeviceCommunicationException)
                {
                    TraceFactory.Logger.Error($"An error occurred trying to set paperless print mode device {_deviceDetail.AssetId}, please check the device");
                    return false;
                }
            }
            TraceFactory.Logger.Debug($"Setting paperless mode to: {paperlessModeOn}");
            JobMediaMode mode = paperlessModeOn ? JobMediaMode.Paperless : JobMediaMode.Paper;

            try
            {
                if (!HasPrintCapability())
                {
                    TraceFactory.Logger.Debug($"{_deviceDetail.AssetId} does not have Print capability, not sending PJL");
                    return true;  //For all intents and purposes, it succeeded.
                }

                IDeviceSettingsManager manager = DeviceSettingsManagerFactory.Create(_device);
                return manager.SetJobMediaMode(mode);

            }
            catch (DeviceFactoryCoreException ex)
            {
                TraceFactory.Logger.Error($"Paperless mode unknown for device type {_device.GetType()}", ex);
                return false;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                return false;
            }
            finally
            {
                if (deviceInitializedHere)
                {
                    _device?.Dispose();
                    _device = null;
                }
            }
        }

        private bool HasPrintCapability()
        {
            TraceFactory.Logger.Debug(_deviceDetail.AssetId + " " + _deviceDetail.Capability);
            AssetAttributes capability = _deviceDetail.Capability;

            return capability.HasFlag(AssetAttributes.Printer);
        }

        /// <summary>
        /// Chooses the device IP address using a device hostname.
        /// </summary>
        /// <returns></returns>
        private static IPAddress GetDeviceAddressFromHostName(string hostName)
        {
            var ipAddresses = Dns.GetHostAddresses(hostName);
            foreach (var ipAddress in ipAddresses)
            {
                //choose the IP address which is not loopback and is Ipv4 (need to work on Ipv6 support out of the box)
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipAddress))
                {
                    return ipAddress;
                }
            }

            //Unable to find an IP address for the given host name.
            return null;
        }

    }
}