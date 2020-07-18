using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    internal partial class WorkerActivityPacingForm : Form
    {
        private ActivityExecutionHelpForm _helpForm = null;
        private IEnumerable<WorkerActivityConfiguration> _configurations = null;

        public WorkerActivityPacingForm(IEnumerable<WorkerActivityConfiguration> configurations)
        {
            _configurations = configurations;

            UserInterfaceStyler.Configure(this, FormStyle.FixedDialogWithHelp);
            InitializeComponent();
            _helpForm = new ActivityExecutionHelpForm(ActivityExecutionHelpForm.Edge.Left);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (!enableCheckBox.Checked)
            {
                if (_configurations.Any(x => x.ExecutionPlan.ActivityPacing.Enabled))
                {
                    var result = MessageBox.Show
                        ("You are disabling Activity Specific Pacing for one or more Activities that currently have it set.  Do you want to continue?",
                        "Disable Activity Pacing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                foreach (var item in _configurations)
                {
                    item.ExecutionPlan.ActivityPacing.Clear();
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(pacingTimeDelayControl.ErrorList))
                {
                    MessageBox.Show("All errors must be resolved before submitting.", "Unresolved Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }
                else
                {
                    foreach (var item in _configurations)
                    {
                        var pacing = item.ExecutionPlan.ActivityPacing;

                        pacing.Enabled = enableCheckBox.Checked;
                        pacing.Randomize = pacingTimeDelayControl.Randomize;
                        pacing.MinDelay = pacingTimeDelayControl.MinDelay;
                        pacing.MaxDelay = pacingTimeDelayControl.MaxDelay;
                        pacing.DelayOnRepeat = delayForEachRadioButton.Checked;
                    }
                }
            }

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DisplayHelp(Control control)
        {
            if (!_helpForm.Visible && !_helpForm.CanFocus)
            {
                _helpForm.HelpString = (string)control.Tag;
                _helpForm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
                _helpForm.ShowDialog(control);
            }
        }

        private void HelpRequestedEvent(object sender, HelpEventArgs hlpevent)
        {
            Control requestingControl = (Control)sender;
            DisplayHelp(requestingControl);
            hlpevent.Handled = true;
        }

        private void WorkerActivityPacingForm_Load(object sender, EventArgs e)
        {
            if (_configurations.Count() == 1)
            {
                var plan = _configurations.First().ExecutionPlan;
                InitializeExecutionPlan(plan);
            }
            else
            {
                var plan = _configurations.First().ExecutionPlan;
                if (_configurations.All(i => i.ExecutionPlan.ActivityPacing.Equals(plan.ActivityPacing)))
                {
                    InitializeExecutionPlan(plan);
                }
                else
                {
                    UseDefaults();
                }
            }

            enableCheckBox.Checked = true;
        }

        private void InitializeExecutionPlan(ScalableTest.Framework.WorkerExecutionPlan plan)
        {
            pacingTimeDelayControl.Randomize = plan.ActivityPacing.Randomize;
            pacingTimeDelayControl.MinDelay = plan.ActivityPacing.MinDelay;
            pacingTimeDelayControl.MaxDelay = plan.ActivityPacing.MaxDelay;
            delayForEachRadioButton.Checked = plan.ActivityPacing.DelayOnRepeat;
            delayAfterAllRadioButton.Checked = !plan.ActivityPacing.DelayOnRepeat;
        }

        private void UseDefaults()
        {
            delayForEachRadioButton.Checked = true;
            delayAfterAllRadioButton.Checked = false;
        }

        private void delayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
