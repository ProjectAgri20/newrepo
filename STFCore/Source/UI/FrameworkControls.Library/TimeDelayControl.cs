using System;
using System.ComponentModel;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// UI widget for the <see cref="TimeDelayControl"/> object.
    /// </summary>
    public partial class TimeDelayControl : FieldValidatedControl
    {
        /// <summary>
        /// Occurs when one of the values in this control changes.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when one of the values in this control changes.")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Gets or sets the text associated with the control.
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("The text associated with the control."), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return groupBox.Text; }
            set { groupBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Randomize option should be selected.
        /// </summary>
        /// <value><c>true</c> if the Randomize option should be selected; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Randomize
        {
            get { return randomDelay_CheckBox.Checked; }
            set { randomDelay_CheckBox.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the minimum delay displayed by this control.
        /// </summary>
        /// <value>The minimum delay.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan MinDelay
        {
            get { return minDelay_TimeSpanControl.Value; }
            set { minDelay_TimeSpanControl.Value = value; }
        }

        /// <summary>
        /// Gets or sets the minimum delay displayed by this control.
        /// </summary>
        /// <value>The minimum delay.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan MaxDelay
        {
            get { return maxDelay_TimeSpanControl.Value; }
            set { maxDelay_TimeSpanControl.Value = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a valid selection.
        /// </summary>
        /// <value><c>true</c> if this instance has a valid selection; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasValidSelection
        {
            get { return Randomize == false || MinDelay <= MaxDelay; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayControl" /> class.
        /// </summary>
        public TimeDelayControl()
        {
            InitializeComponent();
            RandomDelayCheckBox_CheckedChanged(null, null);
        }

        /// <summary>
        /// "Randomize" selection changed.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void RandomDelayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var show = randomDelay_CheckBox.Checked;
            maxDelay_TimeSpanControl.Visible = show;
            max_Label.Visible = show;

            // Change the label.
            min_Label.Text = show ? "Min" : "Default";
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Flag control if selected values are invalid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeDelayControl_Validating(object sender, CancelEventArgs e)
        {
            // Make sure that the min value is less than the max value
            if (!HasValidSelection)
            {
                fieldValidator.SetError(randomDelay_CheckBox, "Max delay must be greater than or equal to min delay.");
                e.Cancel = true;
            }
            else
            {
                fieldValidator.SetError(randomDelay_CheckBox, "");
            }
        }

        private void minDelay_TimeSpanControl_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        private void maxDelay_TimeSpanControl_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
