using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Framework.Automation.OfficeWorker;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.SessionExecution.Properties;
using HP.ScalableTest.Print;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Main form for the application
    /// </summary>
    public partial class WorkerExecutionForm : Form
    {
        private OfficeWorkerActivityController _controller = null;
        private readonly string _clientControllerHostName;
        private readonly string _instanceId;
        private readonly string _logPath;
        
        private string TimeStamp
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture); }
        }

        static WorkerExecutionForm()
        {
            ScalableTest.Framework.UI.UserInterfaceStyler.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerExecutionForm"/> class.
        /// </summary>
        /// <param name="controllerHostname">the client controller name</param>
        /// <param name="instanceId">A unique id for this instance</param>
        /// <param name="logPath">The path to the datalogger database</param>
        public WorkerExecutionForm(string controllerHostname, string instanceId, string logPath)
        {
            InitializeComponent();
            this.Text = $"{instanceId} {this.Text}";
            userLabel.Text = instanceId;
            profilePictureBox.Image = GetAvatarImage();

            // The args should split into two items.  The first item is the Key, the second
            // is the hostname for the Client Factory (no more is it assumed that the Client Factory
            // is running on the same host as this application).

            // Instantiate the engine controller, passing in a reference to the client controller hostname.
            _clientControllerHostName = controllerHostname;
            _instanceId = instanceId;
            _logPath = logPath;

            // Attach unhandled exception handlers (make sure the default handler is detached first)
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler.UnhandledExceptionMethod;
            AppDomain.CurrentDomain.UnhandledException += (s, e) => UnhandledException((Exception)e.ExceptionObject);
            Application.ThreadException += (s, e) => UnhandledException(e.Exception);

            // Attach the DAT logger adapter
            DatLoggerAdapter.Attach();

            // Subscribe to this event so that the controller will be started after the form is done loading
            Application.Idle += new EventHandler(OnLoaded);

            //Make sure we've got the registry keys MS Office uses to print.
            PrintRegistryUtil.CreateUserRegistryKeys();

        }

        private void OnLoaded(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(OnLoaded);

            TraceFactory.Logger.Debug("Creating controller on {0}".FormatWith(_clientControllerHostName));
            _controller = OfficeWorkerActivityController.Create(_clientControllerHostName, _instanceId);

            SetLoggingContext();

            profileLabel.Text = GlobalDataStore.Manifest.ResourceType.ToString();

            TraceFactory.Logger.Debug("Registering event handlers...");
            _controller.ActivityStateChanged += engineController_ActivityStateChanged;
            _controller.WorkerStateChanged += engineController_WorkerStateChanged;

            TraceFactory.Logger.Debug("Loading plugins...");
            foreach (var pluginInfo in _controller.LoadPlugins())
            {
                TraceFactory.Logger.Info($"Adding control for {pluginInfo.Key.Name}");
                AddToMainTabControl(pluginInfo.Key, pluginInfo.Value);
            }

            TraceFactory.Logger.Debug("Starting execution engine...");
            _controller.Start();
            TraceFactory.Logger.Debug("Controller started.");
        }

        private static void SetLoggingContext()
        {
            var manifest = GlobalDataStore.Manifest;
            if (manifest != null)
            {
                TraceFactory.SetThreadContextProperty("Dispatcher", manifest.Dispatcher, false);
                TraceFactory.SetSessionContext(manifest.SessionId);
            }
        }

        private void engineController_ActivityStateChanged(object sender, ActivityStateEventArgs e)
        {
            if (e.State == ActivityState.Started)
            {
                SetActiveTab(e.ActivityId);
            }

            DisplayActivityStatus(e.ActivityName, e.State, e.Message);
        }

        private void engineController_WorkerStateChanged(object sender, ResourceEventArgs e)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new MethodInvoker(() => this.engineController_WorkerStateChanged(sender, e)));
            }
            else
            {
                switch (e.State)
                {
                    case RuntimeState.Running:
                    case RuntimeState.Paused:
                    case RuntimeState.Halted:
                    case RuntimeState.Completed:
                        string message = "{0}: Activities are {1}.".FormatWith(TimeStamp, e.State);
                        logTextBox.AppendText(message + Environment.NewLine);
                        break;

                    case RuntimeState.ShuttingDown:
                        logTextBox.AppendText(TimeStamp + ": Shutting down..." + Environment.NewLine);
                        TraceFactory.Logger.Info("Shutdown message received, exiting in a few seconds");

                        // Dispose the controller which will cascade down and dispose all participating plugins
                        TraceFactory.Logger.Info("Disposing controller, engines and activities");
                        try
                        {
                            _controller.Dispose();
                        }
                        catch (Exception ex)
                        {
                            TraceFactory.Logger.Error(ex.Message);
                        }

                        if (e.CopyLogs)
                        {
                            TraceFactory.Logger.Debug("Pushing log file back to SessionProxy.  Log Location: {0}".FormatWith(_logPath));
                            SessionProxyBackendConnection.SaveLogFiles(_logPath);
                            TraceFactory.Logger.Debug("Log files saved, changing state to offline.");
                        }
                        
                        SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Offline);

                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        Application.Exit();
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the activity status.
        /// </summary>
        /// <param name="name">The activity name</param>
        /// <param name="state">the activity state</param>
        /// <param name="message">the error message</param>
        private void DisplayActivityStatus(string name, ActivityState state, string message)
        {
            if (logTextBox.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => this.DisplayActivityStatus(name, state, message)));
            }
            else
            {
                string logText = "{0}: {1}: {2}".FormatWith(TimeStamp, name, state);

                if (!string.IsNullOrEmpty(message))
                {
                    logText += ": " + message;
                }

                logTextBox.AppendText(logText + Environment.NewLine);
            }
        }

        /// <summary>
        /// Adds plugin to main tab control.
        /// </summary>
        /// <param name="activityInfo">Activity details</param>
        /// <param name="plugin">The item.</param>
        private void AddToMainTabControl(ActivityInfo activityInfo, IPluginExecutionEngine plugin)
        {
            TraceFactory.Logger.Info("Enter addToMainTabControl");

            if (mainTabControl.InvokeRequired)
            {
                TraceFactory.Logger.Info("Enter Invokerequired");
                mainTabControl.Invoke(new MethodInvoker(() => AddToMainTabControl(activityInfo, plugin)));
                return;
            }

            // If the plugin is not a UserControl, then there is no need to add it to the tab control.
            if (!(plugin is Control))
            {
                TraceFactory.Logger.Info("Plugin '{0}' is not a UserControl plugin, thus do not add it to the tab.".FormatWith(activityInfo.ActivityType));
                return;
            }

            TraceFactory.Logger.Debug("Creating plugin tab page: " + activityInfo.ActivityType);
            Control control = (Control)plugin;
            control.Dock = DockStyle.Fill;

            TabPage tabPage = new TabPage(activityInfo.ActivityType);
            try
            {
                tabPage.Name = activityInfo.Id.ToString();
                tabPage.Controls.Add(control);
                TraceFactory.Logger.Debug("Plugin attached to tab page");

                mainTabControl.Controls.Add(tabPage);
                TraceFactory.Logger.Debug("Tab page added to main page");
            }
            catch
            {
                tabPage.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Updates the tab control.
        /// </summary>
        /// <param name="key">The key.</param>
        public void SetActiveTab(Guid key)
        {
            if (mainTabControl.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => this.SetActiveTab(key)));
            }
            else
            {
                mainTabControl.SelectedTab = (TabPage)mainTabControl.Controls[key.ToString()];
            }
        }

        /// <summary>
        /// Gets the avatar image.
        /// </summary>
        /// <returns></returns>
        private static Image GetAvatarImage()
        {
            //int imageNumber = _random.Next(11, 15);
            //int imageNumber = _random.Next(1, 11);
            int imageNumber = new Random(Process.GetCurrentProcess().Id).Next(20, 34);
            TraceFactory.Logger.Debug("Getting Avatar {0}".FormatWith(imageNumber));

            return (Bitmap)Resources.ResourceManager.GetObject("VU" + imageNumber.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Handles the Click event of the debugStart_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void debugStart_Button_Click(object sender, EventArgs e)
        {
            VirtualResourceEventBus.StartMainRun(this);
        }

        private void UnhandledException(Exception e)
        {
            TraceFactory.Logger.Fatal(e);
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Error);
            Environment.Exit(1);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.TaskManagerClosing:
                case CloseReason.UserClosing:
                    var res = MessageBox.Show(Resources.ManualClosePrompt, "Attempting Manual Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    e.Cancel = (res == DialogResult.No);
                    break;
            }
        }

        private void openLogFileButton_Click(object sender, EventArgs e)
        {
            var pid = Process.GetCurrentProcess().Id;
            var filePath = Directory.GetFiles(_logPath, "*-{0}.log".FormatWith(pid)).FirstOrDefault();
            if (!string.IsNullOrEmpty(filePath))
            {
                using (TextDisplayDialog dialog = new TextDisplayDialog(LogFileReader.Read(filePath), filePath))
                {
                    dialog.ShowDialog(this);
                }
            }
            else
            {
                MessageBox.Show("Log file not found for this worker");
            }
        }

        /// <summary>
        /// Cleans up log files if running as STB.
        /// Not used right now because it's more valuable for lab testers to leave the log files.
        /// If we do get a request from Solution Partners to turn this on, it would be better
        /// to clean the log files when this form launches, instead of when it closes.  That way
        /// it will give the tester time to look at the logs post run. -kyoungman
        /// </summary>
        private void CleanUpLogFiles()
        {
            //Clean up the log files if STB
            try
            {
                if (!GlobalSettings.IsDistributedSystem)
                {
                    TraceFactory.Logger.Debug("Deleting log files at: {0}".FormatWith(_logPath));
                    Directory.Delete(_logPath, true);
                }
            }
            catch { }
        }
    }
}
