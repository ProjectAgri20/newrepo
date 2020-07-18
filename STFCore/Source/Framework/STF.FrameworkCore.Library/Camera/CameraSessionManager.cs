using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace HP.ScalableTest.Framework.Camera
{
    public class CameraSessionManager : IDisposable
    {
        private readonly object sessionLock = new object();
        /// <summary>
        /// CONNECTION INFO
        /// </summary>
        private string _cameraServerIP = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _session = string.Empty;
        private CameraState _cameraState;

        
        /// <summary>
        /// Constructor for the CameraRecordingSession Class
        /// </summary>
        /// <param name="cameraServerIP">The IP of the Camera Server</param>
        /// <param name="username">The username to use for loging in (Authentication)</param>
        /// <param name="password">The password to use for login in (Authentication) </param>
        public CameraSessionManager(string cameraServerIP, string username, string password)
        {
            this._cameraServerIP = cameraServerIP;
            this._username = username;
            this._password = password;
            this._session = GetSessionIDCommand();
            LoginCommand();
        }

        /// <summary>
        /// Get the state of the Camera
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns></returns>
        public CameraState GetCameraState(string cameraName)
        {
            string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");

            JObject listOfCameras = ExecuteCameraService(jsonListOfCamera);

            try
            {
                _cameraState = new CameraState(listOfCameras, cameraName);
            }
            catch (NullReferenceException ex)
            {
                throw new CameraSessionException($"Camera '{cameraName}' was not found.", ex);
            }

            return _cameraState;
        }

        /// <summary>
        /// Overloading of the StartRecordingCommand 
        /// </summary>
        /// <param name="cameraName">The name of the Camera to start the recording on</param>
        public void StartRecordingCommand(string cameraName)
        {
            //Getting the states of the camera
            string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");
            
            var listOfCameras = ExecuteCameraService(jsonListOfCamera);
            StartRecordingCommand(cameraName, listOfCameras);
            
        }

        /// <summary>
        /// REMOTE EXECUTION OF THE RECORD SESSION OF THE CAMERA : START RECORDING COMMAND
        /// </summary>
        /// <param name="cameraName">The name of the camera to start the recording on</param>
        private void StartRecordingCommand(string cameraName, JObject listOfCameras)
        {
            _cameraState = new CameraState(listOfCameras, cameraName);
            TraceFactory.Logger.Debug($"Start Recording Camera : {cameraName}, Recording Status : {_cameraState.IsRecording}");

            //Number of  retry  to start a camera.
            int retryCount = 5;

            while (!_cameraState.IsRecording && (retryCount > 0))
            {
                
                string json = JsonStringBuilder(CameraCommand.StartRecording, cameraName);
                bool success = false;

                JObject result = ExecuteCameraService(json);
               
                success = result["result"].Value<string>().Equals("success") ? true : false;

                if (!success)
                {
                    TraceFactory.Logger.Debug($"Response from the server: { result["result"].Value<string>() } ");
                    throw new CameraSessionException("Cannot run the Start Recording Command on the server.");
                }


                //Give the server 2 seconds to change state
                Thread.Sleep(2000);

                //Getting the states of the camera
                string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");
                var newListOfCameras = ExecuteCameraService(jsonListOfCamera);
                _cameraState = new CameraState(newListOfCameras, cameraName);

                TraceFactory.Logger.Debug($"Camera {cameraName} recording state : {_cameraState.IsRecording} ");

                retryCount--;
            }
            
        }


        /// <summary>
        /// REMOTE EXECUTION OF THE RECORD SESSION OF THE CAMERA : STOP RECORDING COMMAND
        /// </summary>
        /// <param name="cameraName">The name of the camera to stop the recording from</param>
        private void StopRecordingCommand(string cameraName, JObject listOfCameras)
        {
            _cameraState = new CameraState(listOfCameras, cameraName);

            TraceFactory.Logger.Debug($"Stop Recording Camera : {cameraName}, Recording Status : {_cameraState.IsRecording}");

            //Number of  retry  to stop a camera.
            int retryCount = 5;

            while (_cameraState.IsRecording && (retryCount > 0) )
            {
                string json = JsonStringBuilder(CameraCommand.StopRecording, cameraName);
                bool success = false;

                JObject result = ExecuteCameraService(json);
                
                success = result["result"].Value<string>().Equals("success") ? true : false;

                if (!success)
                {
                    TraceFactory.Logger.Debug($"Response from the server: { result["result"].Value<string>() } ");
                    throw new CameraSessionException("Cannot run the Stop Recording Command on the server.");
                }


                //Give the server 2 seconds to change state
                Thread.Sleep(2000);

                //Getting the states of the camera
                string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");
                var newListOfCameras = ExecuteCameraService(jsonListOfCamera);
                _cameraState = new CameraState(newListOfCameras, cameraName);

                TraceFactory.Logger.Debug($"Camera {cameraName} recording state : {_cameraState.IsRecording} ");

                retryCount--;
            }

        }

        /// <summary>
        /// Overloading of the StoprecordingCommand
        /// </summary>
        /// <param name="cameraName">The name of Camera to stop the recording from</param>
        public void StopRecordingCommand(string cameraName)
        {
            //Getting the states of the camera
            string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");

            var listOfCameras = ExecuteCameraService(jsonListOfCamera);
            StopRecordingCommand(cameraName, listOfCameras);           
            
        }


        /// <summary>
        /// REMOTE EXECUTION OF THE RECORD SESSION OF THE CAMERA : RESTART RECORDING COMMAND
        /// </summary>
        /// <param name="cameraName">The name of the camera to be recorded on</param>
        public void RestartRecordingCommand(string cameraName)
        {
            //Getting the states of the camera
            string jsonListOfCamera = JsonStringBuilder(CameraCommand.Camlist, "");
           
            var listOfCameras = ExecuteCameraService(jsonListOfCamera);
            _cameraState = new CameraState(listOfCameras, cameraName);

            TraceFactory.Logger.Debug($"Restart Recording Camera : {cameraName}, Recording Status : {_cameraState.IsRecording}");

            if (_cameraState.IsRecording)
            {
                TraceFactory.Logger.Debug("Stoping the camera recording");
                //-------STOPING THE RECORDING --------------------------------------------------//
                StopRecordingCommand(cameraName, listOfCameras);
            }

            TraceFactory.Logger.Debug("Starting the camera recording");
            //--------STARTING THE RECORDING -----------------------------------------------//
            StartRecordingCommand(cameraName);
            
        }



        /// <summary>
        /// REMOTE EXECUTION OF THE TRIGGER COMMAND ON THE SERVER COMMAND
        /// </summary>
        /// <param name="cameraName">The name of the camera to be recorded on</param>
        public void TriggerNowCommand(string cameraName)
        {
            string json = JsonStringBuilder(CameraCommand.Trigger, cameraName);
            bool success = false;
            JObject result = ExecuteCameraService(json);
            success = result["result"].Value<string>().Equals("success") ? true : false;

            if (!success)
            {
                throw new CameraSessionException("Cannot run the Trigger Recording Command on the server.");
            }

            TraceFactory.Logger.Debug($"Response from the server: { result["result"].Value<string>() } ");
        }


        /// <summary>
        /// LOGIN TO THE SERVER COMMAND
        /// </summary>
        /// <param name="session">The session to use for login</param>
        public void LoginCommand()
        {
            string json = JsonStringBuilder(CameraCommand.Login, "");
            bool success = false;

            JObject result = ExecuteCameraService(json);
            
            success = result["result"].Value<string>().Equals("success") ? true : false;

            if (!success)
            {
                throw new CameraSessionException("Cannot run the Login Command on the server.");
            }
                

        }


        /// <summary>
        /// LOGOUT FROM THE SERVER COMMAND
        /// </summary>
        /// <param name="session">The session to logout from</param>
        public void LogoutCommand()
        {
            string json = JsonStringBuilder(CameraCommand.Logout, string.Empty);
            bool success = false;

            JObject result = ExecuteCameraService(json);
            
            success = result["result"].Value<string>().Equals("success") ? true : false;

            if (!success)
            {
                throw new CameraSessionException($"Cannot run the Logout Command on the server. result: {result["result"].Value<string>()}");
            }
                
            
        }



        /// <summary>
        /// GETTING SESSION ID FROM SERVER COMMAND
        /// </summary>
        /// <returns>The session ID as a String</returns>
        public string GetSessionIDCommand()
        {
            string json = JsonStringBuilder(CameraCommand.Session, string.Empty);

            JObject result = ExecuteCameraService(json);
            
            string session = result["session"].Value<string>();
            
            return session;
        }

        private JObject ExecuteCameraService(string jsonCommand)
        {
            JObject jsonResult = new JObject();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_cameraServerIP);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonCommand);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    //Getting the Session ID
                    var result = streamReader.ReadToEnd();

                    jsonResult = (JObject)JsonConvert.DeserializeObject(result);

                }
            }
            catch (Exception e)
            {
                throw new CameraSessionException("Cannot run one Command on the server.", e);
            }
            
            return jsonResult;
        }


        /// <summary>
        /// SMALL METHOD TO GENERATE A MD5 HASH STRING
        /// </summary>
        /// <param name="input">The string that needs to be hashed</param>
        /// <returns>An MD5 hash string</returns>
        private string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();

        }


        #region IDisposable Support
        /// <summary>
        /// IDisposable Support
        /// </summary>
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                try
                {
                    LogoutCommand();
                }
                catch (Exception e)
                {

                }
                

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~CameraSessionManager()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// StringBuilder for the json command sent to the server. 
        /// Commands supported by the class: session, login, logout, trigger, startrecording and stoprecording
        /// </summary>
        /// <param name="command">The command to use in the json sent to the server</param>
        /// <param name="session">The session withing which the command will be executed. If no session put an empty string.</param>
        /// <param name="cameraName">The name of the camera impacted by the command. If no camera name put an empty string.</param>
        /// <returns>The finished build json command that will be sent to the server.</returns>
        private string JsonStringBuilder(CameraCommand command, string cameraName) {

            StringBuilder json = new StringBuilder("{\"cmd\":");
            string response = CalculateMD5Hash(_username + ":" + _session + ":" + _password);


            switch (command) {
                //Getting a session ID
                case CameraCommand.Session:
                    json.Append("\"login\"}");
                    break;
                //Login to the server
                case CameraCommand.Login:
                    json.Append("\"login\"," +
                                  "\"session\":\"" + _session + "\"," + "\"response\":\"" + response + "\"" + "}");
                    break;
                //Logout from the server
                case CameraCommand.Logout:
                    json.Append("\"logout\"," +
                              "\"session\":\"" + _session + "\"," + "\"response\":\"" + response + "\"" + "}");
                    break;
                //Trigger the camera.
                case CameraCommand.Trigger:
                    json.Append("\"trigger\"," +
                              "\"camera\":\"" + cameraName + "\"," + "\"session\":\"" + _session + "\"" + "}");
                    break;
                //Start recording on the camera
                case CameraCommand.StartRecording:
                    json.Append("\"camconfig\"," +
                              "\"camera\":\"" + cameraName + "\"," + "\"session\":\"" + _session + "\"," + "\"manrec\":\"" + true + "\"" + "}");
                    break;
                //Stop recording of the camera
                case CameraCommand.StopRecording:
                    json.Append("\"camconfig\"," +
                              "\"camera\":\"" + cameraName + "\"," + "\"session\":\"" + _session + "\"," + "\"manrec\":\"" + false + "\"" + "}");
                    break;
                //Get the list of all the cameras
                case CameraCommand.Camlist:
                    json.Append("\"camlist\"," +
                              "\"session\":\"" + _session + "\"}");
                    break;
                //Others querries not handled
                default:
                    json.Append("COMMAND ERROR... PLEASE CHECK YOUR COMMAND");
                    break;
                    }
            
            return json.ToString();
        }

        /// <summary>
        ///Enum containing the list of commands supported 
        /// </summary>
        internal enum CameraCommand
        {
            Session,
            Login,
            Logout,
            Trigger,
            StartRecording,
            StopRecording,
            Camlist
        }

        /// <summary>
        /// This Class help keep information about the state of the Camera
        /// </summary>
        public class CameraState{

            private bool isRecording = false;
            private bool isPaused = false;
            private bool isTrigger = false;
            private bool isEnabled = false;
            private bool isOnline = false;
            
            /// <summary>
            /// Check if the camera is recording.
            /// </summary>
            public bool IsRecording
            {
                get
                {
                    return isRecording;
                }

                set
                {
                    isRecording = value;
                }
            }
            
            /// <summary>
            /// Check if the camera is paused.
            /// </summary>
            public bool IsPaused
            {
                get
                {
                    return isPaused;
                }

                set
                {
                    isPaused = value;
                }
            }
            
            /// <summary>
            /// Check if the camera is in the trigger state.
            /// </summary>
            public bool IsTriggered
            {
                get
                {
                    return isTrigger;
                }

                set
                {
                    isTrigger = value;
                }
            }
            
            /// <summary>
            /// Check if the camera is enable.
            /// </summary>
            public bool IsEnabled
            {
                get
                {
                    return isEnabled;
                }

                set
                {
                    isEnabled = value;
                }
            }
            
            /// <summary>
            /// Check if the Camera is online.
            /// </summary>
            public bool IsOnline
            {
                get
                {
                    return isOnline;
                }

                set
                {
                    isOnline = value;
                }
            }
            
            /// <summary>
            /// Constructor for the class
            /// </summary>
            /// <param name="listOfCameras">List of all the cameras from querrying the server.</param>
            /// <param name="cameraName">The name of the Camera</param>
            public CameraState(JObject listOfCameras, string cameraName) {

                IsRecording = (bool)listOfCameras["data"].Where(jt => jt["optionValue"].Value<string>().Equals(cameraName)).FirstOrDefault()["isRecording"];
                IsOnline = (bool)listOfCameras["data"].Where(jt => jt["optionValue"].Value<string>().Equals(cameraName)).FirstOrDefault()["isOnline"];
            }

        }
        
        /// <summary>
        /// Exception generated by having a failed execution of a command on the server.
        /// </summary>
        public class CameraSessionException : Exception
        {
            /// <summary>
            /// New Exception without a message.
            /// </summary>
            public CameraSessionException()
            {
               
            }
            /// <summary>
            /// New Exception with a message
            /// </summary>
            /// <param name="message">The message for the exception</param>
            public CameraSessionException(string message)
                 : base(message)
            {
               
            }
            /// <summary>
            /// New Exception with a message and an Exception
            /// </summary>
            /// <param name="message">The message for the exception</param>
            /// <param name="inner">The exception</param>
            public CameraSessionException(string message, Exception inner)
                : base(message, inner)
            {

            }


        }
    }
}
