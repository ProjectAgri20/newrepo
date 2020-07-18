using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using HP.SPS.SES;

namespace HP.ScalableTest.Plugin.PrinterOnMobile
{
    [ToolboxItem(false)]
    public partial class PrinterOnMobileExecutionControl : UserControl, IPluginExecutionEngine
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterOnMobileExecutionControl" /> class.
        /// </summary>
        public PrinterOnMobileExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {            
            using (PrinterOnMobilePrintActivityManager manager = new PrinterOnMobilePrintActivityManager(executionData))
            {
                try
                {
                    UpdateStatus("Starting execution with mobile device");
                    manager.ActivityStatusChanged += UpdateStatus;
                    return manager.RunMobilePrintActivity();
                }
                catch(Exception ex)
                {
                    UpdateStatus(ex.ToString());
                    return new PluginExecutionResult(PluginResult.Error, ex, "Unknown Error");
                }
                
            }
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="status">The status.</param>
        private void UpdateStatus(string message)
        {
            statusRichTextBox.InvokeIfRequired(n =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status = " + message);
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}\n");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
    }
}