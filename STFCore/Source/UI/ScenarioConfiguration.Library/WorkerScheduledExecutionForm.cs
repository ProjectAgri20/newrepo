using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal partial class WorkerScheduledExecutionForm : Form
    {
        SortableBindingList<ExecutionScheduleSegment> _bindingList = new SortableBindingList<ExecutionScheduleSegment>();
        ExecutionSchedule _schedule = null;
        bool _unsavedChangesExist = false;

        public WorkerScheduledExecutionForm(string scheduleXml)
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            errorProvider.SetIconAlignment(hourMin_Label, ErrorIconAlignment.MiddleRight);

            if (!string.IsNullOrEmpty(scheduleXml))
            {
                try
                {
                    _bindingList.Clear();
                    _schedule = LegacySerializer.DeserializeXml<ExecutionSchedule>(scheduleXml);
                    _bindingList = _schedule.BindingList;
                }
                catch (XmlException ex)
                {
                    TraceFactory.Logger.Error("Bad XML in Run Schedule", ex);
                    MessageBox.Show
                        (
                            "Unable to load existing schedule information.",
                            "Load Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                }
            }
            else
            {
                _schedule = new ExecutionSchedule();
            }

            dataGridViewCheckBoxColumn1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewTextBoxColumn2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewTextBoxColumn3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewTextBoxColumn4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// The XML representing the Execution Schedule configuration.
        /// </summary>
        public string ScheduleXml
        {
            get { return LegacySerializer.SerializeXml(_schedule).ToString(); }
        }

        /// <summary>
        /// The Execution Schedule configuration.
        /// </summary>
        public ExecutionSchedule Schedule
        {
            get { return _schedule; }
        }

        private int Duration
        {
            get
            {
                TimeSpan minutes = TimeSpanFormat.Parse(duration_TextBox.Text);
                return Convert.ToInt32(minutes.TotalMinutes);
            }
        }

        private int CumulativeDuration()
        {
            TimeSpan total = new TimeSpan();
            foreach (ExecutionScheduleSegment segment in _bindingList)
            {
                total += segment.Duration;
            }
            return Convert.ToInt32(total.TotalMinutes);
        }

        private int RepeatCount
        {
            get
            {
                return Convert.ToInt32(repeat_NumericUpDown.Text);
            }
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            if (_unsavedChangesExist)
            {
                var result = MessageBox.Show
                    (
                        "You have unsaved changes that will be lost.  Continue?",
                        "Unsaved changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void ScheduledExecutionConfigurationForm_Load(object sender, EventArgs e)
        {
            Binding repeatCountBinding = new Binding("Text", _schedule, "RepeatCount");
            repeat_NumericUpDown.DataBindings.Add(repeatCountBinding);

            Binding durationBinding = new Binding("Text", _schedule, "Duration");
            durationBinding.Format += new ConvertEventHandler(ConvertIntToHourMin);
            durationBinding.Parse += new ConvertEventHandler(ConvertHourMinToInt);
            duration_TextBox.DataBindings.Add(durationBinding);

            duration_RadioButton.Checked = _schedule.UseDuration;
            iterationCount_RadioButton.Checked = !_schedule.UseDuration;

            schedule_DataGridView.DataSource = _bindingList;
            schedule_DataGridView.CellValueChanged += schedule_DataGridView_CellValueChanged;

            UpdateCumulativeDuration();
            ValidateDuration();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            foreach (var row in _bindingList)
            {
                if (row.Days == 0 && row.Hours == 0 && row.Minutes == 0)
                {
                    MessageBox.Show
                        (
                            "Each row must have at least one column (Days, Hours or Minutes) with a value greater than 0.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                    return;
                }
            }

            _schedule.Clear();

            _bindingList.ToList().ForEach(delegate(ExecutionScheduleSegment item)
            {
                _schedule.Add(item);
            });

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void schedule_DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _unsavedChangesExist = true;
            UpdateCumulativeDuration();
        }

        private void repeatCount_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (duration_RadioButton.Checked)
            {
                _schedule.UseDuration = true;
                repeat_NumericUpDown.Enabled = false;
                duration_TextBox.Enabled = true;
            }
            else
            {
                _schedule.UseDuration = false;
                repeat_NumericUpDown.Enabled = true;
                duration_TextBox.Enabled = false;
            }
            UpdateCumulativeDuration();
            ValidateDuration();
        }

        private void UpdateCumulativeDuration()
        {
            int total = CumulativeDuration();

            if (iterationCount_RadioButton.Checked)
            {
                total = total * RepeatCount;
            }
            cumulativeTime_Label.Text = TimeSpanFormat.ToTimeSpanString(total);
        }

        private void ConvertHourMinToInt(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.Parse(((string)e.Value)).TotalMinutes;
        }

        private void ConvertIntToHourMin(object sender, ConvertEventArgs e)
        {
            e.Value = TimeSpanFormat.ToTimeSpanString((int)e.Value);
        }

        private void ScheduledExecutionConfigurationForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
            images.Add("DIAGRAM", Properties.Resources.ScheduledExecutionHelp);
            var page = Properties.Resources.ScheduledExecutionHelpPage;

            using (HelpDialog dialog = new HelpDialog(page, images))
            {
                dialog.Title = "Scheduled Execution Help";
                dialog.ShowDialog();
            }
        }

        private void ValidateDuration()
        {
            if (_schedule.UseDuration)
            {
                if (this.Duration < CumulativeDuration())
                {
                    errorProvider.SetError(hourMin_Label, "Duration time is less than total segment time.  Duration time will be adhered to.");
                    return;
                }
            }

            errorProvider.SetError(hourMin_Label, string.Empty);
        }

        private void duration_TextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateDuration();
        }

        private void duration_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Determine whether the keystroke is a number from either the numbers on the keyboard or the keypad
            e.SuppressKeyPress = ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) 
                && e.KeyCode != Keys.Back 
                && e.KeyCode != Keys.OemSemicolon 
                && e.KeyCode != Keys.Left
                && e.KeyCode != Keys.Right);

        }
    }
}
