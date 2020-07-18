using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for configuring a Citrix Worker virtual resource.
    /// </summary>
    public class CitrixWorkerControl : OfficeWorkerControl
    {
        private Collection<FrameworkServer> _citrixServers = null;

        public CitrixWorkerControl()
            : base()
        {
            citrixServer_ComboBox.Validating += citrixServer_ComboBox_Validating;
            pubApp_ComboBox.Validating += pubApp_ComboBox_Validating;
        }

        /// <summary>
        /// Gets the resource title.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Citrix Worker Configuration";
            }
        }

        /// <summary>
        /// Gets the type of the worker.
        /// </summary>
        /// <value>
        /// The type of the worker.
        /// </value>
        protected override VirtualResourceType WorkerType
        {
            get { return VirtualResourceType.CitrixWorker; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Initialize(new CitrixWorker());
        }

        public override void Initialize(object entity)
        {
            base.Initialize(entity);
        }

        public override void FinalizeEdit()
        {
            base.FinalizeEdit();

            CitrixWorker worker = Worker as CitrixWorker;

            if (citrixServer_ComboBox.SelectedServer != null)
            {
                worker.ServerHostname = citrixServer_ComboBox.SelectedServer;
            }

            if (pubApp_CheckBox.Checked)
            {
                worker.PublishedApp = pubApp_ComboBox.Text;
            }
            else
            {
                worker.PublishedApp = string.Empty;
            }



            if (worker.RunMode == CitrixWorkerRunMode.None)
            {
                RemoveCitrixWorkerActivities();
            }
        }

        private void citrixServer_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                pubApp_ComboBox.DataSource = null;
                pubApp_ComboBox.DataSource = SelectApplicationNames(context, citrixServer_ComboBox.SelectedServer);
            }

            UpdateControls();
        }

        /// <summary>
        /// Sets up any child controls.
        /// </summary>
        /// <param name="officeWorker">The office worker.</param>
        protected override void SetupChildControls(OfficeWorker officeWorker)
        {
            if (officeWorker == null)
            {
                throw new ArgumentNullException("officeWorker");
            }

            CitrixWorker worker = officeWorker as CitrixWorker;

            citrixServer_ComboBox.Visible = citrixServer_Label.Visible = true;
            appOptions_GroupBox.Visible = true;
            workersPerVM_Label.Visible = workersPerVM_UpDown.Visible = false;

            if (worker.RunMode != CitrixWorkerRunMode.None)
            {
                citrixWorker_CheckBox.Checked = true;
                pubApp_RadioButton.Checked = (worker.RunMode == CitrixWorkerRunMode.PublishedApp);
                desktop_RadioButton.Checked = (worker.RunMode == CitrixWorkerRunMode.Desktop);
            }
            else
            {
                pubApp_RadioButton.Checked = true;
                pubApp_RadioButton.Enabled = false;
                desktop_RadioButton.Enabled = false;
            }

            pubApp_CheckBox.Checked = !string.IsNullOrEmpty(worker.PublishedApp);

            citrixWorker_CheckBox.CheckedChanged += runWorkerOrApp_CheckedChanged;
            pubApp_CheckBox.CheckedChanged += runWorkerOrApp_CheckedChanged;

            desktop_RadioButton.CheckedChanged += desktop_RadioButton_CheckedChanged;
            pubApp_RadioButton.CheckedChanged += pubApp_RadioButton_CheckedChanged;

            pubApp_ComboBox.Text = worker.PublishedApp;

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                string serverType = ServerType.Citrix.ToString();
                var servers = context.FrameworkServers.Where(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active).OrderBy(n => n.HostName);
                _citrixServers = new Collection<FrameworkServer>(servers.ToList());

                if (_citrixServers.Any())
                {
                    citrixServer_ComboBox.AddServers(servers);

                    var hostName = worker.ServerHostname;
                    if (!_citrixServers.Any(x => x.HostName.Equals(hostName)))
                    {
                        hostName = _citrixServers.First().HostName;
                    }
                    citrixServer_ComboBox.SetSelectedServer(hostName);

                    var appNames = SelectApplicationNames(context, hostName);
                    pubApp_ComboBox.DataSource = appNames;

                    if (appNames.Contains<string>(worker.PublishedApp))
                    {
                        pubApp_ComboBox.SelectedItem = worker.PublishedApp;
                    }
                }
            }

            UpdateControls();

            citrixServer_ComboBox.SelectedIndexChanged += citrixServer_ComboBox_SelectedIndexChanged;
        }

        private static List<string> SelectApplicationNames(AssetInventoryContext entities, string hostName)
        {
            return
                (
                    from n in entities.CitrixPublishedApps
                    where n.CitrixServer.Equals(hostName, StringComparison.OrdinalIgnoreCase)
                    select n.ApplicationName
                ).Distinct().ToList();
        }

        private void citrixServer_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (citrixServer_ComboBox.SelectedServer == null)
            {
                // If the server is null, the name of the server is also "null"
                HasValue(null, "Citrix Server", citrixServer_Label, e);
            }
        }

        void pubApp_ComboBox_Validating(object sender, CancelEventArgs e)
        {
            if (pubApp_CheckBox.Checked)
            {
                HasValue(pubApp_ComboBox.Text, app_Label, e, "No published apps configured for {0}, choose another server.".FormatWith(citrixServer_ComboBox.SelectedServer));
            }
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.workersPerVM_UpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // CitrixWorkerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.Name = "CitrixWorkerControl";
            ((System.ComponentModel.ISupportInitialize)(this.workersPerVM_UpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void appType_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            pubApp_ComboBox.Visible = pubApp_CheckBox.Checked;
            app_Label.Visible = pubApp_CheckBox.Checked;

            workerQuantity_GroupBox.Visible = citrixWorker_CheckBox.Checked;
            workflow_GroupBox.Visible = citrixWorker_CheckBox.Checked;
            executionOrder_GroupBox.Visible = citrixWorker_CheckBox.Checked;
            startup_TimeDelayControl.Visible = citrixWorker_CheckBox.Checked;

            EnableWorkerValidation(citrixWorker_CheckBox.Checked);

            if (!citrixWorker_CheckBox.Checked && configuration_TabControl.TabPages.Contains(workerActivities_TabPage))
            {
                configuration_TabControl.TabPages.Remove(workerActivities_TabPage);
            }
            else if (citrixWorker_CheckBox.Checked && !configuration_TabControl.TabPages.Contains(workerActivities_TabPage))
            {
                configuration_TabControl.TabPages.Add(workerActivities_TabPage);
            }

            CitrixWorker worker = Worker as CitrixWorker;

            if (!citrixWorker_CheckBox.Checked)
            {
                worker.RunMode = CitrixWorkerRunMode.None;
            }
            else
            {
                if (pubApp_RadioButton.Checked)
                {
                    worker.RunMode = CitrixWorkerRunMode.PublishedApp;
                }
                else
                {
                    worker.RunMode = CitrixWorkerRunMode.Desktop;
                }
            }

            var hostName = citrixServer_ComboBox.SelectedServer;
            var server = _citrixServers.First(x => x.HostName.Equals(hostName));
            int version = 0;
            if (!string.IsNullOrEmpty(server.ServiceVersion))
            {
                version = Convert.ToInt32(new string(server.ServiceVersion.Where(Char.IsDigit).ToArray()));
            }
            if (version < 75 || !GlobalSettings.Items.ContainsKey("{0}-CitrixWorkerDesktop".FormatWith(server.HostName)))
            {
                // Disable the XenDesktop option for running an office worker.
                desktop_RadioButton.Enabled = false;
                pubApp_RadioButton.Checked = true;
            }
            else
            {
                desktop_RadioButton.Enabled = true;
            }

        }

        void runWorkerOrApp_CheckedChanged(object sender, EventArgs e)
        {
            desktop_RadioButton.Enabled = pubApp_RadioButton.Enabled = citrixWorker_CheckBox.Checked;

            UpdateControls();
        }

        private void pubApp_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CitrixWorker worker = Worker as CitrixWorker;
            if (pubApp_RadioButton.Checked)
            {
                worker.RunMode = CitrixWorkerRunMode.PublishedApp;
            }
        }

        private void desktop_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            CitrixWorker worker = Worker as CitrixWorker;
            if (desktop_RadioButton.Checked)
            {
                worker.RunMode = CitrixWorkerRunMode.Desktop;
            }
        }
    }
}
