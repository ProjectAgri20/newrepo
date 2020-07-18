using System;
using System.ServiceModel;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.Framework
{
    public class STFDispatcherManager
    {
        public static event EventHandler DispatcherChanged;

        /// <summary>
        /// The machine name of the dispatcher to connect to.
        /// </summary>
        public static FrameworkServer Dispatcher { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="STFDispatcherManager"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public static bool Connected { get; private set; }

        public static bool ConnectToDispatcher()
        {
            var result = false;
            if (GlobalSettings.IsDistributedSystem)
            {
                FrameworkServer dispatcher = null;
                using (MainFormConnectDialog connectDialog = new MainFormConnectDialog())
                {
                    connectDialog.ShowDialog();
                    if (connectDialog.DialogResult == DialogResult.OK)
                    {
                        try
                        {
                            dispatcher = connectDialog.SelectedDispatcher;

                            //Subscribe to dispatcher updates
                            SessionClient.Instance.Initialize(dispatcher.IPAddress);
                            SessionClient.Instance.Refresh();

                            Dispatcher = dispatcher;
                            GlobalSettings.SetDispatcher(dispatcher.IPAddress);
                            DispatcherChanged?.Invoke(dispatcher, new EventArgs());
                            result = true;
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Connected = result;
                            throw new EnvironmentConnectionException(
                                "Could not connect to '{0}' at {1}.".FormatWith(dispatcher.HostName, dispatcher.IPAddress), ex);
                        }
                    }
                }
            }
            else
            {
                // If we are in a standalone mode then there will not be a remote dispatcher running
                // and in this case just collect information on the local machine and use that for
                // the Dispatcher information.
                Dispatcher = new FrameworkServer() { FrameworkServerId = SequentialGuid.NewGuid() };
                Dispatcher.HostName = Environment.MachineName;

                // This is needed to run on a 192 or 10 net.
                Dispatcher.IPAddress = "localhost";

                SessionClient.Instance.Initialize(Dispatcher.IPAddress);
                SessionClient.Instance.Refresh();

                GlobalSettings.SetDispatcher(Dispatcher.IPAddress);
                DispatcherChanged(Dispatcher, new EventArgs());

                result = true;
            }
            Connected = result;
            return result
;
        }

        public static bool DisconnectFromDispatcher(bool promptForConfirmation)
        {
            var result = false;
            if (!promptForConfirmation || ContinueWithDisconnect())
            {
                if (Dispatcher != null)
                {
                    Dispatcher = null;  //Reset the dispatcher
                    DispatcherChanged(Dispatcher, new EventArgs());
                    SessionClient.Instance.Stop();

                    Connected = false;
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Notifies the user that their connection to the dispatcher will be terminated.  Allows an opportunity to cancel.
        /// </summary>
        /// <returns>Whether to continue or not.</returns>
        private static bool ContinueWithDisconnect()
        {
            DialogResult result = MessageBox.Show("This action will disconnect you from {0}.  Sessions will continue to run after you have disconnected, and you can re-connect at any time.".FormatWith(Dispatcher.HostName),
                "Dispatcher Connection", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            return result == DialogResult.OK;
        }

    }
}
