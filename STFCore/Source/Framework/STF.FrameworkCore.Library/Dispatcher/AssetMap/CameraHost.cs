using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Camera;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    public class CameraHost : AssetHost
    {
        private readonly CameraDetail _cameraDetail;
        //private bool _useRecording = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        public CameraHost(AssetDetail asset)
            : base(asset, asset.AssetId, ElementType.Camera, "Camera")
        {
            _cameraDetail = asset as CameraDetail;
        }


        /// <summary>
        /// revalidates
        /// </summary>
        /// <param name="loopState"></param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            Validate(loopState);
        }


        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="loopState"></param>
        public override void Validate(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Validating", RuntimeState.Validating);
            TraceFactory.Logger.Debug("Validating.");

            if (string.IsNullOrEmpty(_cameraDetail.Address) || string.IsNullOrEmpty(_cameraDetail.ServerUser) || string.IsNullOrEmpty(_cameraDetail.ServerPassword))
            {
                string message = $"Server Settings not configured for camera server '{_cameraDetail.CameraServer}'";
                TraceFactory.Logger.Debug(message);
                MapElement.UpdateStatus(message, RuntimeState.Warning);
                return;
            }

            try
            {
                using (CameraSessionManager manager = new CameraSessionManager(_cameraDetail.Address, _cameraDetail.ServerUser, _cameraDetail.ServerPassword))
                {
                    // Check if the camera is online
                    if (manager.GetCameraState(_cameraDetail.CameraId).IsOnline)
                    {
                        TraceFactory.Logger.Debug($"Camera State Online: {_cameraDetail.CameraId}");
                        MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                    }
                    else
                    {
                        TraceFactory.Logger.Debug($"Camera State Offline: '{_cameraDetail.CameraId}'");
                        MapElement.UpdateStatus($"Camera '{_cameraDetail.CameraId}' is not online.", RuntimeState.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                MapElement.UpdateStatus($"Unable to contact '{_cameraDetail.CameraId}'.", RuntimeState.Warning);
            }
        }


        /// <summary>
        /// Restarts this asset.
        /// </summary>
        public override void Restart()
        {
            // Implemented as needed by child classes
        }

        /// <summary>
        /// Executes this asset host, which may mean different things.
        /// </summary>
        public override void Run(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);

            //_useRecording = _cameraDetail.UseAutoStart;

            if (_cameraDetail.UseAutoStart)
            {
                using (CameraSessionManager manager = new CameraSessionManager(_cameraDetail.Address, _cameraDetail.ServerUser, _cameraDetail.ServerPassword))
                {
                    try
                    {
                        // Starting the camera if the camera wasn't already recording.
                        manager.StartRecordingCommand(_cameraDetail.CameraId);

                        if (manager.GetCameraState(_cameraDetail.CameraId).IsRecording)
                        {
                            MapElement.UpdateStatus("Recording", RuntimeState.Running);
                        }
                        else
                        {
                            MapElement.UpdateStatus("NOT Recording", RuntimeState.Running);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Cannot Start the camera: {_cameraDetail.CameraId}.", ex);
                        MapElement.UpdateStatus($"Camera {_cameraDetail.CameraId} is not recording.", RuntimeState.Warning);
                    }
                }
            }
            else
            {
                MapElement.UpdateStatus("Manual Setup", RuntimeState.Running);
            }
              
        }


        /// <summary>
        /// Marks this asset complete
        /// </summary>
        public override void Completed()
        {
            //_useRecording = _cameraDetail.UseAutoStart;

            if (_cameraDetail.UseAutoStart)
            {
                using (CameraSessionManager manager = new CameraSessionManager(_cameraDetail.Address, _cameraDetail.ServerUser, _cameraDetail.ServerPassword))
                {
                    try
                    {
                        // Stop the camera
                        manager.StopRecordingCommand(_cameraDetail.CameraId);

                        if (manager.GetCameraState(_cameraDetail.CameraId).IsRecording)
                        {
                            MapElement.UpdateStatus("Could not stop Recording", RuntimeState.Running);
                        }
                        else
                        {
                            MapElement.UpdateStatus("Completed", RuntimeState.Running);
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Cannot Stop the camera: {_cameraDetail.CameraId}.", ex);
                        MapElement.UpdateStatus($"Camera {_cameraDetail.CameraId} is not stopping properly.", RuntimeState.Warning);
                    }
                }
            }
            //MapElement.UpdateStatus("Completed", RuntimeState.Completed);
        }
        /// <summary>
        /// Shuts down this asset host
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loopState"></param>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            //_useRecording = _cameraDetail.UseAutoStart;

            if (_cameraDetail.UseAutoStart)
            {
                using (CameraSessionManager manager = new CameraSessionManager(_cameraDetail.Address, _cameraDetail.ServerUser, _cameraDetail.ServerPassword))
                {
                    try
                    {
                        // Stop the camera
                        manager.StopRecordingCommand(_cameraDetail.CameraId);
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Error($"Cannot Stop the camera: {_cameraDetail.CameraId}.", ex);
                        MapElement.UpdateStatus($"Camera {_cameraDetail.CameraId} is not stopping properly.", RuntimeState.Warning);
                    }
                }
            }

            base.Shutdown(options, loopState);
           
        }
    }
}
