using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    public partial class ChoiceIPControl : UserControl //, IChoiceControl
    {
        public ChoiceIPControl()
        {
            InitializeComponent();
            onOff_CheckBox.CheckedChanged += OnFieldChecked;
            OnFieldChecked(null, null);
        }

        //public bool WillSet()
        //{
        //    return onOff_CheckBox.Checked;
        //}

        //public event EventHandler FieldChecked;
        private void OnFieldChecked(object sender, EventArgs e)
        {
            ipAddressControl.Enabled = onOff_CheckBox.Checked;
        }
    }
}
