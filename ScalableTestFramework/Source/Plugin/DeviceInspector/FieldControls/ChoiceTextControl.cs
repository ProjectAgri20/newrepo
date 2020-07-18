using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    public partial class ChoiceTextControl : UserControl, IChoiceControl
    {
        public ChoiceTextControl()
        {
            InitializeComponent();
            onOff_CheckBox.CheckedChanged += OnFieldChecked;
            OnFieldChecked(null, null);
        }

        //public bool WillSet()
        //{
        //    return onOff_CheckBox.Checked;
        //}

        public event EventHandler FieldChecked;
        private void OnFieldChecked(object sender, EventArgs e)
        {
            text_Box.Enabled = onOff_CheckBox.Checked;
        }
    }
}
