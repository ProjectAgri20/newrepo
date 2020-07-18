using HP.ScalableTest.Plugin.DeviceConfiguration.Classes;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    public partial class ProtocolDefaultControl : UserControl, IGetSetComponentData
    {
        private ProtocolSettingsData _protocolSettingsData;
        public EventHandler ControlComponentChanged;
        public bool Modified;

        public ProtocolDefaultControl()
        {
            InitializeComponent();

            _protocolSettingsData = new ProtocolSettingsData();
            bonjour_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            ftp_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            llmnr_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            lpd_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            multicast_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            slp_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            telnet_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            tftp_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            winsPort_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            winsReg_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            wsPrint_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            wsd_choiceComboControl.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            p9100_ComboBox.choice_Combo.DataSource = ListValues.OnOffList.ToArray();
            AddEventHandlers();
        }

        public IComponentData GetData()
        {
            return _protocolSettingsData;
        }

        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _protocolSettingsData = list.OfType<ProtocolSettingsData>().FirstOrDefault();

            if (_protocolSettingsData != null)
            {
                InitialiseChoiceComboControl(bonjour_choiceComboControl, _protocolSettingsData.Bonjour);
                InitialiseChoiceComboControl(ftp_choiceComboControl, _protocolSettingsData.Ftp);
                InitialiseChoiceComboControl(llmnr_choiceComboControl, _protocolSettingsData.Llmnr);
                InitialiseChoiceComboControl(lpd_choiceComboControl, _protocolSettingsData.Lpd);
                InitialiseChoiceComboControl(multicast_choiceComboControl, _protocolSettingsData.MultiCast);
                InitialiseChoiceComboControl(slp_choiceComboControl, _protocolSettingsData.Slp);
                InitialiseChoiceComboControl(telnet_choiceComboControl, _protocolSettingsData.Telnet);
                InitialiseChoiceComboControl(tftp_choiceComboControl, _protocolSettingsData.Tftp);
                InitialiseChoiceComboControl(winsPort_choiceComboControl, _protocolSettingsData.WinsPort);
                InitialiseChoiceComboControl(winsReg_choiceComboControl, _protocolSettingsData.WinsRegistration);
                InitialiseChoiceComboControl(wsPrint_choiceComboControl, _protocolSettingsData.WSPrint);
                InitialiseChoiceComboControl(wsd_choiceComboControl, _protocolSettingsData.WSDiscovery);
                InitialiseChoiceComboControl(p9100_ComboBox, _protocolSettingsData.P9100);
            }

            AddEventHandlers();
        }

        public void SetData()
        {
            GetDataFromChoiceComboBox(bonjour_choiceComboControl, _protocolSettingsData.Bonjour);
            GetDataFromChoiceComboBox(ftp_choiceComboControl, _protocolSettingsData.Ftp);
            GetDataFromChoiceComboBox(llmnr_choiceComboControl, _protocolSettingsData.Llmnr);
            GetDataFromChoiceComboBox(lpd_choiceComboControl, _protocolSettingsData.Lpd);
            GetDataFromChoiceComboBox(multicast_choiceComboControl, _protocolSettingsData.MultiCast);
            GetDataFromChoiceComboBox(slp_choiceComboControl, _protocolSettingsData.Slp);
            GetDataFromChoiceComboBox(telnet_choiceComboControl, _protocolSettingsData.Telnet);
            GetDataFromChoiceComboBox(tftp_choiceComboControl, _protocolSettingsData.Tftp);
            GetDataFromChoiceComboBox(winsPort_choiceComboControl, _protocolSettingsData.WinsPort);
            GetDataFromChoiceComboBox(winsReg_choiceComboControl, _protocolSettingsData.WinsRegistration);
            GetDataFromChoiceComboBox(wsPrint_choiceComboControl, _protocolSettingsData.WSPrint);
            GetDataFromChoiceComboBox(wsd_choiceComboControl, _protocolSettingsData.WSDiscovery);
            GetDataFromChoiceComboBox(p9100_ComboBox, _protocolSettingsData.P9100);
        }

        private void AddEventHandlers()
        {
            AddChoiceComboControlEventHandler(bonjour_choiceComboControl);
            AddChoiceComboControlEventHandler(ftp_choiceComboControl);
            AddChoiceComboControlEventHandler(llmnr_choiceComboControl);
            AddChoiceComboControlEventHandler(lpd_choiceComboControl);
            AddChoiceComboControlEventHandler(multicast_choiceComboControl);
            AddChoiceComboControlEventHandler(slp_choiceComboControl);
            AddChoiceComboControlEventHandler(telnet_choiceComboControl);
            AddChoiceComboControlEventHandler(tftp_choiceComboControl);
            AddChoiceComboControlEventHandler(winsPort_choiceComboControl);
            AddChoiceComboControlEventHandler(winsReg_choiceComboControl);
            AddChoiceComboControlEventHandler(wsPrint_choiceComboControl);
            AddChoiceComboControlEventHandler(wsd_choiceComboControl);
            AddChoiceComboControlEventHandler(p9100_ComboBox);
        }

        private void RemoveEventHandlers()
        {
            RemoveChoiceComboControlEventHandler(bonjour_choiceComboControl);
            RemoveChoiceComboControlEventHandler(ftp_choiceComboControl);
            RemoveChoiceComboControlEventHandler(llmnr_choiceComboControl);
            RemoveChoiceComboControlEventHandler(lpd_choiceComboControl);
            RemoveChoiceComboControlEventHandler(multicast_choiceComboControl);
            RemoveChoiceComboControlEventHandler(slp_choiceComboControl);
            RemoveChoiceComboControlEventHandler(telnet_choiceComboControl);
            RemoveChoiceComboControlEventHandler(tftp_choiceComboControl);
            RemoveChoiceComboControlEventHandler(winsPort_choiceComboControl);
            RemoveChoiceComboControlEventHandler(winsReg_choiceComboControl);
            RemoveChoiceComboControlEventHandler(wsPrint_choiceComboControl);
            RemoveChoiceComboControlEventHandler(wsd_choiceComboControl);
            RemoveChoiceComboControlEventHandler(p9100_ComboBox);
        }

        private void AddChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged += OnControlComponentChanged;
        }

        private void RemoveChoiceComboControlEventHandler(ChoiceComboControl choiceComboControl)
        {
            choiceComboControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            choiceComboControl.choice_Combo.SelectedIndexChanged -= OnControlComponentChanged;
        }

        private void InitialiseChoiceComboControl(ChoiceComboControl choiceComboControl, DataPair<string> itemPair)
        {
            choiceComboControl.choice_Combo.SelectedItem = itemPair.Key;
            choiceComboControl.onOff_CheckBox.Checked = itemPair.Value;
        }

        private void GetDataFromChoiceComboBox(ChoiceComboControl choiceComboControl, DataPair<string> dataPair)
        {
            dataPair.Key = choiceComboControl.choice_Combo.SelectedValue.ToString();
            dataPair.Value = choiceComboControl.onOff_CheckBox.Checked;
        }

        private void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }
    }
}