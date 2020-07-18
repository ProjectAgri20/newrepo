using System;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DeviceInspector.FieldControls
{
    public partial class ChoiceTimePicker : UserControl, IChoiceControl
    {
        public ChoiceTimePicker()
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
            dateTime.Enabled = onOff_CheckBox.Checked;
        }
        /// <summary>
        /// 0 is short time, 1 is time
        /// </summary>
        /// <param name="i"></param>
        public void FormatTime(int i)
        {
            switch (i)
            {
                case 0:
                    dateTime.Format = DateTimePickerFormat.Short;
                    break;
                case 1:
                    dateTime.Format = DateTimePickerFormat.Time;
                    dateTime.ShowUpDown = true;
                    break;
            }
        }
    }
}
