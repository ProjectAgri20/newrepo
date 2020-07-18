using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System.ComponentModel;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    [ToolboxItem(false)]
    public partial class GeneralSettingsControl : UserControl, IGetSetComponentData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="=GeneralSettingsControl"/> class
        /// </summary>
        private GeneralSettingsData _generalSettingsData;
        public bool Modified = false;


        /// <summary>
        /// Occurs when configuration data in this control has changed.
        /// Used to track whether this control has unsaved changes.
        /// </summary>
        public event EventHandler ControlComponentChanged;

        public GeneralSettingsControl()
        {
            InitializeComponent();



            time_TimePicker.FormatTime(1);

            _generalSettingsData = new GeneralSettingsData();
            //Initialize Data
            activityTimeout_ComboBox.choice_Combo.DataSource = _inactivityTimeValues;
            sleepTimer_ComboBox.choice_Combo.DataSource = _sleepTimeValues;
            syncTime_ComboBox.choice_Combo.DataSource = _syncTimeValues;
            timeZone_ComboBox.choice_Combo.DataSource = _timeZones;


            //Set Event Handlers
            AutoSyncTime_CheckBox.CheckedChanged += Sync_Check;
            Sync_Check(null, null);

            AddEventHandlers();

        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        public IComponentData GetData()
        {
            //Get data from controls
            return _generalSettingsData;
        }

        /// <summary>
        /// If the field checkbox is set, write the value else leave it
        /// </summary>
        public void SetData()
        {

            _generalSettingsData.CurrentDate.Key = date_TimePicker.dateTime.Value;
            _generalSettingsData.CurrentDate.Value = date_TimePicker.onOff_CheckBox.Checked;

            _generalSettingsData.CurrentTime.Key = time_TimePicker.dateTime.Value;
            _generalSettingsData.CurrentTime.Value = time_TimePicker.onOff_CheckBox.Checked;

            _generalSettingsData.ActivityTimeout.Key = activityTimeout_ComboBox.choice_Combo.SelectedValue.ToString();
            _generalSettingsData.ActivityTimeout.Value = activityTimeout_ComboBox.onOff_CheckBox.Checked;

            _generalSettingsData.SleepTimer.Key = sleepTimer_ComboBox.choice_Combo.SelectedValue.ToString();
            _generalSettingsData.SleepTimer.Value = sleepTimer_ComboBox.onOff_CheckBox.Checked;

            _generalSettingsData.ServerAddress.Key = ipAddress_Control.ipAddressControl.Text;
            _generalSettingsData.ServerAddress.Value = ipAddress_Control.onOff_CheckBox.Checked;

            _generalSettingsData.Port.Key = port_TextBox.text_Box.Text;
            _generalSettingsData.Port.Value = port_TextBox.onOff_CheckBox.Checked;

            _generalSettingsData.SyncTime.Key = syncTime_ComboBox.choice_Combo.SelectedValue.ToString();
            _generalSettingsData.SyncTime.Value = syncTime_ComboBox.onOff_CheckBox.Checked;

            _generalSettingsData.TimeZone.Key = (TimeZoneInfo)timeZone_ComboBox.choice_Combo.SelectedValue;
            _generalSettingsData.TimeZone.Value = timeZone_ComboBox.onOff_CheckBox.Checked;


            _generalSettingsData.DefaultToEnglish.Key = defaultKeyLanguage_CheckBox.Checked;

            _generalSettingsData.SyncWithServer.Key = AutoSyncTime_CheckBox.Checked;


        }


        /// <summary>
        /// Sets the control data, remove event handers to prevent an event call to the parent that the component changed in this case
        /// </summary>
        /// <param name="list"></param>
        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _generalSettingsData = list.OfType<GeneralSettingsData>().FirstOrDefault();

            activityTimeout_ComboBox.choice_Combo.SelectedIndex = _inactivityTimeValues.FindIndex(x => x == _generalSettingsData.ActivityTimeout.Key);
            activityTimeout_ComboBox.onOff_CheckBox.Checked = _generalSettingsData.ActivityTimeout.Value;


            //Should cover conversion of FW settings
            sleepTimer_ComboBox.choice_Combo.SelectedIndex = (_generalSettingsData.SleepTimer.Key == "116" || _generalSettingsData.SleepTimer.Key == "118") ? _sleepTimeValues.Count-1 : _sleepTimeValues.FindIndex(x => x == _generalSettingsData.SleepTimer.Key);
            sleepTimer_ComboBox.onOff_CheckBox.Checked =  _generalSettingsData.SleepTimer.Value;

            ipAddress_Control.ipAddressControl.Text = _generalSettingsData.ServerAddress.Key;
            ipAddress_Control.onOff_CheckBox.Checked = _generalSettingsData.ServerAddress.Value;

            port_TextBox.text_Box.Text = _generalSettingsData.Port.Key;
            port_TextBox.onOff_CheckBox.Checked = _generalSettingsData.Port.Value;

            syncTime_ComboBox.choice_Combo.SelectedIndex = _syncTimeValues.FindIndex(x => x == _generalSettingsData.SyncTime.Key);
            syncTime_ComboBox.onOff_CheckBox.Checked = _generalSettingsData.SyncTime.Value;

            timeZone_ComboBox.choice_Combo.SelectedIndex = _timeZones.ToList().FindIndex(x => x.Id == _generalSettingsData.TimeZone.Key.Id);
            timeZone_ComboBox.onOff_CheckBox.Checked = _generalSettingsData.TimeZone.Value;

            date_TimePicker.dateTime.Value = _generalSettingsData.CurrentDate.Key;
            date_TimePicker.onOff_CheckBox.Checked = _generalSettingsData.CurrentDate.Value;

            time_TimePicker.dateTime.Value = _generalSettingsData.CurrentTime.Key;
            time_TimePicker.onOff_CheckBox.Checked = _generalSettingsData.CurrentTime.Value;

            defaultKeyLanguage_CheckBox.Checked = _generalSettingsData.DefaultToEnglish.Key;
            AutoSyncTime_CheckBox.Checked = _generalSettingsData.SyncWithServer.Key;
            //SETUP controls
            AddEventHandlers();


        }



        private void Sync_Check(object sender, EventArgs e)
        {
            if (AutoSyncTime_CheckBox.Checked)
            {
                //timeZone_ComboBox.Enabled = false;
                date_TimePicker.Enabled = false;
                time_TimePicker.Enabled = false;
                ipAddress_Control.Enabled = true;
                port_TextBox.Enabled = true;
                syncTime_ComboBox.Enabled = true;
            }
            else
            {
                //timeZone_ComboBox.Enabled = true;
                date_TimePicker.Enabled = true;
                time_TimePicker.Enabled = true;
                ipAddress_Control.Enabled = false;
                port_TextBox.Enabled = false;
                syncTime_ComboBox.Enabled = false;
            }



        }

        private void RemoveEventHandlers()
        {
            defaultKeyLanguage_CheckBox.CheckedChanged -= OnControlComponentChanged;
            activityTimeout_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            activityTimeout_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            sleepTimer_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            sleepTimer_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            ipAddress_Control.ipAddressControl.TextChanged -= OnControlComponentChanged;
            ipAddress_Control.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            port_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            port_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            syncTime_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            syncTime_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            timeZone_ComboBox.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
            timeZone_ComboBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            date_TimePicker.dateTime.TextChanged -= OnControlComponentChanged;
            date_TimePicker.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            time_TimePicker.dateTime.TextChanged -= OnControlComponentChanged;
            time_TimePicker.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
        }

        private void AddEventHandlers()
        {
            defaultKeyLanguage_CheckBox.CheckedChanged += OnControlComponentChanged;
            activityTimeout_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            activityTimeout_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            sleepTimer_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            sleepTimer_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            ipAddress_Control.ipAddressControl.TextChanged += OnControlComponentChanged;
            ipAddress_Control.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            port_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            port_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            syncTime_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            syncTime_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            timeZone_ComboBox.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
            timeZone_ComboBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            date_TimePicker.dateTime.TextChanged += OnControlComponentChanged;
            date_TimePicker.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            time_TimePicker.dateTime.TextChanged += OnControlComponentChanged;
            time_TimePicker.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
        }





        public List<string> _inactivityTimeValues = new List<string> { "15", "30", "60", "90", "120", "180", "300" };

        public IReadOnlyCollection<TimeZoneInfo> _timeZones = TimeZoneInfo.GetSystemTimeZones();

        public List<string> _syncTimeValues = new List<string> { "1 hour", "12 hours", "24 hours", "48 hours", "90 hours", "120 hours" };
        public List<string> _sleepTimeValues = new List<string> { "0", "15", "30", "60", "90", "110" };

        private void TimeSettings_groupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
