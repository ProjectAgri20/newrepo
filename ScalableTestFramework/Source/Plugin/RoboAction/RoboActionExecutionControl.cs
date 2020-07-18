using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace HP.ScalableTest.Plugin.RoboAction
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class RoboActionExecutionControl : UserControl, IPluginExecutionEngine
    {
        private Uri _baseUri;

        /// <summary>
        ///
        /// </summary>
        public RoboActionExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// Executes this plug-in's workflow using the specified <see cref="PluginExecutionData"/>.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult"/> indicating the outcome of the
        /// execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
           
            var data = executionData.GetMetadata<RoboActionActivityData>();
            _baseUri = new Uri($"http://{data.PiAddress}:{data.PiPort}/api/pins");

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(data.PiAddress, data.PiPort);
                    if (!client.Connected)
                        return new PluginExecutionResult(PluginResult.Failed, $"Unable to connect to WebApi server on Raspberry, check if the application is running on {data.PiAddress}:{data.PiPort}.");
                }
                catch (Exception e)
                {
                    return new PluginExecutionResult(PluginResult.Failed, $"Unable to connect to WebApi server on Raspberry, check if the application is running on {data.PiAddress}:{data.PiPort}. Exception: {e.Message}");
                }
               
            }

            var response = ExecuteRestApi(data.PinNumber, true);

            if (string.IsNullOrEmpty(response))
            {
                return new PluginExecutionResult(PluginResult.Failed,
                    "Unable to communicate with WebApi Server on Raspberry. Please check and try again!");
            }
            Thread.Sleep(data.DurationTimeSpan);
            response = ExecuteRestApi(data.PinNumber, false);
            if (string.IsNullOrEmpty(response))
            {
                return new PluginExecutionResult(PluginResult.Failed,
                    "Unable to communicate with WebApi Server on Raspberry. Please check and try again!");
            }

            return new PluginExecutionResult(PluginResult.Passed, "Executed Successfully");
        }

        private string ExecuteRestApi(int pinNumber, bool pinStatus)
        {

            Uri url = new Uri(_baseUri, $"?pinId={pinNumber}&status={Convert.ToInt32(pinStatus)}");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = 0;

            try
            {
                var webResult = HttpWebEngine.Post(webRequest, string.Empty);

                if (webResult.StatusCode == HttpStatusCode.OK || webResult.StatusCode == HttpStatusCode.Accepted)
                {
                    return string.IsNullOrEmpty(webResult.Response)? "OK": webResult.Response;
                }
            }
            catch (WebException)
            {
            }

            return string.Empty;
        }

        #endregion IPluginExecutionEngine implementation
    }
}