using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    public partial class ThreeChoiceControl : UserControl //, IChoiceControl
    {
        public ThreeChoiceControl()
        {
            InitializeComponent();
            onOff_CheckBox.CheckedChanged += OnFieldChecked;
            OnFieldChecked(null, null);
        }


        //public event EventHandler FieldChecked;
        private void OnFieldChecked(object sender, EventArgs e)
        {
            field1_TextBox.Enabled = onOff_CheckBox.Checked;
            field2_TextBox.Enabled = onOff_CheckBox.Checked;
            field3_TextBox.Enabled = onOff_CheckBox.Checked;
        }

    }
}
