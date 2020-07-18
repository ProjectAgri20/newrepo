using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Form for selecting session shutdown options.
    /// </summary>
    public partial class SessionShutdownForm : Form, ISessionShutdownForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionShutdownForm"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public SessionShutdownForm(string sessionId)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);

            sessionId_Label.Text = sessionId;
            copyLogsLocation_Label.Text = GlobalSettings.Items[Setting.DispatcherLogCopyLocation];
            workerFinish_CheckBox.Enabled = false;

            // Configure the form for either STE or STB
            if ((GlobalSettings.IsDistributedSystem)) 
            {
                ConfigureFormForDistributedSystem();
            }
            else // Builds STB Session Shutdown Form
            {
                ConfigureForm();
            }    
        }

        /// <summary>
        /// Gets the shutdown options.
        /// </summary>
        public ShutdownOptions ShutdownOptions
        {
            get
            {
                var options = new ShutdownOptions();

                options.PurgeRemotePrintQueues = purgeQueues_checkBox.Checked;
                options.AllowWorkerToComplete = workerFinish_CheckBox.Checked;
                options.CopyLogs = copyLogs_CheckBox.Checked;
                options.DisableDeviceCrc = disableCrc_CheckBox.Checked;
                options.CollectDeviceEventLogs = EventLogs_CheckBox.Checked;

                if (GlobalSettings.IsDistributedSystem)
                {
                    options.PowerOff = shutDown_CheckBox.Checked;
                    options.PowerOffOption = revert_RadioButton.Checked ?
                                             VMPowerOffOption.RevertToSnapshot :
                                             VMPowerOffOption.PowerOff;

                    
                    options.ShutdownDeviceSimulators = shutdownSimulator_CheckBox.Checked;                    
                }

              
                return options;
            }
        }

        private void shutDown_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            powerOff_RadioButton.Enabled = shutDown_CheckBox.Checked;
            revert_RadioButton.Enabled = shutDown_CheckBox.Checked;
        }

        private void copyLogs_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            panelCopyLogsSTF.Visible = copyLogs_CheckBox.Checked;
            copyLogsInfo_Label.Visible = copyLogs_CheckBox.Checked;
            copyLogsLocation_Label.Visible = copyLogs_CheckBox.Checked;        
        }

        private void shutDown_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Adjusts visibility and location of session shutdown form options if running STB
        /// </summary>
        private void ConfigureForm()
        {
            // Alter form size
            this.Size = new System.Drawing.Size(700, 310);

            purgeQueues_checkBox.Text = "Purge Print Queues	";
            copyLogs_CheckBox.Text = "Save Log Files";
            copyLogsInfo_Label.Text = "Log files copied to: ";

            // Hide STF options
            shutDown_CheckBox.Hide();
            powerOff_RadioButton.Hide();
            revert_RadioButton.Hide();
            shutdownSimulator_CheckBox.Hide();
            
        }

        /// <summary>
        /// Adjust visibility and location of session shutdown form options if running STE
        /// </summary>
        private void ConfigureFormForDistributedSystem()
        {
            // Alter Form Size
            this.Size = new System.Drawing.Size(700, 410);

            // Hide STB options
            purgeQueues_checkBox.Text = "Purge Remote Print Queues";
            copyLogs_CheckBox.Text = "Copy log files to dispatcher server";
            copyLogsInfo_Label.Text = "Session logs will be copied to: ";
        }

        private void CheckPurge_CompleteActivity(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            string tagValue = cb.Tag.ToString();

            if (cb.Checked)
            {
                if (tagValue == "purge")
                {
                    workerFinish_CheckBox.Enabled = false;
                }
                else
                {
                    purgeQueues_checkBox.Enabled = false;
                }
            }
            else
            {
                purgeQueues_checkBox.Enabled = true;
                workerFinish_CheckBox.Enabled = true;
            }
            
        }
    }

    public interface ISessionShutdownForm
    {
        ShutdownOptions ShutdownOptions { get; }
    }
}
