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
    public partial class PasswordWindowsControl : UserControl, IGetSetComponentData
    {
        private PasswordWindowsData _passwordWindowsData;
        public bool Modified = false;

        public EventHandler ControlComponentChanged;

        public PasswordWindowsControl()
        {
            InitializeComponent();
            _passwordWindowsData = new PasswordWindowsData();

            AddEventHandlers();

        }

        public void OnControlComponentChanged(object sender, EventArgs e)
        {
            Modified = true;
            ControlComponentChanged?.Invoke(this, e);
        }

        public IComponentData GetData()
        {
            return _passwordWindowsData;
        }

        public void SetData()
        {
            _passwordWindowsData.MinPWLength.Key = maxPasswordLength_Textbox.text_Box.Text;
            _passwordWindowsData.MinPWLength.Value = maxPasswordLength_Textbox.onOff_CheckBox.Checked;


            if (!string.IsNullOrWhiteSpace(winDomains_TextBox.text_Box.Text))
            {
                _passwordWindowsData.WindowsDomains.Key = winDomains_TextBox.text_Box.Text;
            }
            _passwordWindowsData.WindowsDomains.Value = winDomains_TextBox.onOff_CheckBox.Checked;

            _passwordWindowsData.DefaultDomain.Key = defaultDomain_TextBox.text_Box.Text;
            _passwordWindowsData.DefaultDomain.Value = defaultDomain_TextBox.onOff_CheckBox.Checked;

            _passwordWindowsData.EnablePasswordComplexity.Key = passwordComplexity_CheckBox.Checked;
            _passwordWindowsData.EnableWindowsAuthentication.Key = windowsAuth_CheckBox.Checked;


        }


        public void SetControl(IEnumerable<IComponentData> list)
        {
            RemoveEventHandlers();

            _passwordWindowsData = list.OfType<PasswordWindowsData>().FirstOrDefault();

            maxPasswordLength_Textbox.text_Box.Text = _passwordWindowsData.MinPWLength.Key;
            maxPasswordLength_Textbox.onOff_CheckBox.Checked = _passwordWindowsData.MinPWLength.Value;

            winDomains_TextBox.text_Box.Text = _passwordWindowsData.WindowsDomains.Key.ToString();
            winDomains_TextBox.onOff_CheckBox.Checked = _passwordWindowsData.WindowsDomains.Value;

            defaultDomain_TextBox.text_Box.Text = _passwordWindowsData.DefaultDomain.Key;
            defaultDomain_TextBox.onOff_CheckBox.Checked = _passwordWindowsData.DefaultDomain.Value;

            passwordComplexity_CheckBox.Checked = _passwordWindowsData.EnablePasswordComplexity.Key;
            windowsAuth_CheckBox.Checked = _passwordWindowsData.EnableWindowsAuthentication.Key;
            

            AddEventHandlers();
        }

        public void RemoveEventHandlers()
        {
            maxPasswordLength_Textbox.text_Box.TextChanged -= OnControlComponentChanged;
            maxPasswordLength_Textbox.onOff_CheckBox.TextChanged -= OnControlComponentChanged;

            winDomains_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            winDomains_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            defaultDomain_TextBox.text_Box.TextChanged -= OnControlComponentChanged;
            defaultDomain_TextBox.onOff_CheckBox.CheckedChanged -= OnControlComponentChanged;

            passwordComplexity_CheckBox.CheckedChanged -= OnControlComponentChanged;
            windowsAuth_CheckBox.CheckedChanged -= OnControlComponentChanged;

        }

        public void AddEventHandlers()
        {
            maxPasswordLength_Textbox.text_Box.TextChanged += OnControlComponentChanged;
            maxPasswordLength_Textbox.onOff_CheckBox.TextChanged += OnControlComponentChanged;

            winDomains_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            winDomains_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            defaultDomain_TextBox.text_Box.TextChanged += OnControlComponentChanged;
            defaultDomain_TextBox.onOff_CheckBox.CheckedChanged += OnControlComponentChanged;

            passwordComplexity_CheckBox.CheckedChanged += OnControlComponentChanged;
            windowsAuth_CheckBox.CheckedChanged += OnControlComponentChanged;
        }


    }
}
