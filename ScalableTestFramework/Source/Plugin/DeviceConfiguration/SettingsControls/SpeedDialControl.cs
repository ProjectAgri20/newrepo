using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Plugin.DeviceConfiguration.SettingsData;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    [ToolboxItem(false)]
    public partial class SpeedDialControl : UserControl, IGetSetComponentData
    {
        private SpeedDialData _speedDialData;
        public EventHandler ControlComponentChanged;
        public bool Modified;

        
        public SpeedDialControl()
        {
            InitializeComponent();
            _speedDialData = new SpeedDialData();
            AddEventHandlers();
            speedDial_ChoiceControl.field2_TextBox.MaxLength = 3;
            speedDial_ChoiceControl.field2_TextBox.KeyPress += speedDial_ChoiceControl_field2_TextBox_KeyPress;
            speedDial_ChoiceControl.field3_TextBox.KeyPress += speedDial_ChoiceControl_field3_TextBox_KeyPress;
        }

        public IComponentData GetData()
        {
            return _speedDialData;
        }
        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _speedDialData = list.OfType<SpeedDialData>().FirstOrDefault();
            if (_speedDialData != null)
            {               
                speedDial_ChoiceControl.onOff_CheckBox.Checked = _speedDialData.DisplayName.Value;
                speedDial_ChoiceControl.field1_TextBox.Text = _speedDialData.DisplayName.Key;
                speedDial_ChoiceControl.field2_TextBox.Text = _speedDialData.SpeedDialNumber.Key;
                speedDial_ChoiceControl.field3_TextBox.Text = _speedDialData.FaxNumbers.Key;
            }
                          
            AddEventHandlers();
        }

        public void SetData()
        {
            _speedDialData.DisplayName.Value = speedDial_ChoiceControl.onOff_CheckBox.Checked;            
            _speedDialData.DisplayName.Key = speedDial_ChoiceControl.field1_TextBox.Text;
            _speedDialData.SpeedDialNumber.Key = speedDial_ChoiceControl.field2_TextBox.Text;
            _speedDialData.FaxNumbers.Key = speedDial_ChoiceControl.field3_TextBox.Text;
           
        }

        public void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        public void RemoveEventHandlers()
        {
            speedDial_ChoiceControl.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;
            speedDial_ChoiceControl.field1_TextBox.TextChanged -= OnControlComponentChanged;
            speedDial_ChoiceControl.field2_TextBox.TextChanged -= OnControlComponentChanged;
            speedDial_ChoiceControl.field3_TextBox.TextChanged -= OnControlComponentChanged;           
        }

        public void AddEventHandlers()
        {            
            speedDial_ChoiceControl.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;
            speedDial_ChoiceControl.field1_TextBox.TextChanged += OnControlComponentChanged;
            speedDial_ChoiceControl.field2_TextBox.TextChanged += OnControlComponentChanged;
            speedDial_ChoiceControl.field3_TextBox.TextChanged += OnControlComponentChanged;     
        }

        private void speedDial_ChoiceControl_field3_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)42 && e.KeyChar != (char)35)
            {
                e.Handled = true;
            }
        }

        private void speedDial_ChoiceControl_field2_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
