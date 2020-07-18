using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution.Wizard
{
    public partial class SessionStartScheduleForm : Form
    {
        private SessionStartSchedule _schedule = null;

        public SessionStartScheduleForm(SessionStartSchedule schedule)
        {
            InitializeComponent();
            _schedule = schedule;

            delayedStartDateTimePicker.DataBindings.Add("Value", _schedule, "StartDateTime");
            setupBufferNumericUpDown.DataBindings.Add("Value", _schedule, "SetupTimePadding");
        }

        public SessionStartSchedule Schedule
        {
            get { return _schedule; }
        }

        private void enableButton_Click(object sender, EventArgs e)
        {
            _schedule.ScheduleEnabled = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void disableButton_Click(object sender, EventArgs e)
        {
            _schedule.ScheduleEnabled = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
