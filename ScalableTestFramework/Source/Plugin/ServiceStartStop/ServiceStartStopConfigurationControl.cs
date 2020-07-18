using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ServiceStartStop
{
    /// <summary>
    /// Provides the control to configure the ServiceStartStop activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ServiceStartStopConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private ServiceStartStopActivityData _activityData = null;

        /// <summary>
        /// Initializes a new instance of the ServiceStartStopConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public ServiceStartStopConfigurationControl()
        {
            InitializeComponent();

            fieldValidator.RequireSelection(server_ComboBox, serviceLabel);

            server_ComboBox.SelectionChanged += Server_ComboBox_SelectionChanged; ;
            serviceListBox.SelectedValueChanged += ServiceListBox_SelectedValueChanged;
            radioStop.CheckedChanged += ServiceActions_SelectedChanged;
            radioStart.CheckedChanged += ServiceActions_SelectedChanged;
            radioRestart.CheckedChanged += ServiceActions_SelectedChanged;
        }
        /// <summary>
        /// Pull Services from Enterprise Test
        /// </summary>
        /// <param name="selectedAddress"></param>
        private void LoadServices(string selectedAddress)
        {
            serviceListBox.DataSource = null;
            foreach (string service in ConfigurationServices.EnvironmentConfiguration.AsInternal().GetServerServices(selectedAddress))
            {
                serviceListBox.Items.Add(service);
            }

        }

        private void Server_ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            //_activityData.hostAddress = server_ComboBox.SelectedServer.Address;
            _activityData.serv = server_ComboBox.SelectedServer;

            serviceListBox.Items.Clear();

            if (_activityData.services == null)
            {
                _activityData.services = new List<string>();
                _activityData.serviceIDs = new List<int>();
            }
            else
            {
                _activityData.services = null;
                _activityData.services = new List<string>();
                _activityData.serviceIDs = new List<int>();
            }


            if (!string.IsNullOrEmpty(_activityData.serv.Address))
            {
                LoadServices(_activityData.serv.Address);
            }
        }

        private void ServiceListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            // Clear Services List as a user may deselect some items
            // Thus we have to check for all changes each time.
            if (_activityData.services == null)
            {
                _activityData.services = new List<string>();
                _activityData.serviceIDs = new List<int>();
            }
            else
            {
                _activityData.services = null;
                _activityData.services = new List<string>();
                _activityData.serviceIDs = new List<int>();
            }


            var services = serviceListBox.SelectedItems;
            var serviceIDs = serviceListBox.SelectedIndices;

            foreach (var item in services)
            {
                if (!_activityData.services.Contains(item.ToString()))
                {
                    _activityData.services.Add(item.ToString());
                }
            }
            foreach (var item in serviceIDs)
            {
                if (!_activityData.serviceIDs.Contains((int)item))
                {
                    _activityData.serviceIDs.Add((int)item);
                }
            }

            //get collection of values and execute

        }

        private void ServiceActions_SelectedChanged(object sender, EventArgs e)
        {
            foreach (RadioButton r in groupBox_Action.Controls)
            {
                if (r.Checked)
                {
                    _activityData.task = r.TabIndex;
                }
            }
        }

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            return new PluginConfigurationData(_activityData, "1.0");
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new ServiceStartStopActivityData();
            server_ComboBox.Initialize("Serviceable");

        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            GetConfigurationControls(configuration.GetMetadata<ServiceStartStopActivityData>());

        }

        private void GetConfigurationControls(ServiceStartStopActivityData data)
        {
            _activityData = data;
            server_ComboBox.Initialize(data.serv, "Serviceable");

            LoadServices(data.serv.Address);

            foreach (var item in data.serviceIDs)
            {
                serviceListBox.SetSelected(item, true);
            }
            if (data.task == 0)
                radioStop.Checked = true;
            else if (data.task == 1)
                radioStart.Checked = true;
            else if (data.task == 2)
                radioRestart.Checked = true;



        }


        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        ///// <summary>
        ///// Event handler to be called whenever the activity's configuration data changes.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnConfigurationChanged(object sender, EventArgs e)
        //{
        //    ConfigurationChanged?.Invoke(this, e);
        //}
    }
}
