using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    public partial class ChoiceNumericControl : UserControl, IChoiceControl
    {
        public event EventHandler FieldChecked;
        public ChoiceNumericControl()
        {
            InitializeComponent();
            onOff_CheckBox.CheckedChanged += OnFieldChecked;
            OnFieldChecked(null, null);
        }

        private void OnFieldChecked(object sender, EventArgs e)
        {
            choice_numericUpDown.Enabled = onOff_CheckBox.Checked;
        }

        
    }
}
