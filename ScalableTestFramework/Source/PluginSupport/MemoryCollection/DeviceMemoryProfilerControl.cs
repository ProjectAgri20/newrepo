using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.MemoryCollection
{
    /// <summary>
    /// User control for adding memory collection for plugins
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class DeviceMemoryProfilerControl : UserControl
    {
        DeviceMemoryProfilerConfig _data = new DeviceMemoryProfilerConfig();

        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the selection of the control changes.")]
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMemoryProfilerControl"/> class.
        /// </summary>
        public DeviceMemoryProfilerControl()
        {
            InitializeComponent();

            //fieldValidator.RequireCustom(timeSpanControlSampleTarget, TimeSpanMinimum);
            //fieldValidator.RequireCustom(numericUpDownCountSampleTarget, IntervalMinimum);

            checkBoxCountBased.CheckedChanged += (s, e) => OnSelectionChanged();
            checkBoxTimeBased.CheckedChanged += (s, e) => OnSelectionChanged();
            timeSpanControlSampleTarget.ValueChanged += (s, e) => OnSelectionChanged();
            numericUpDownCountSampleTarget.ValueChanged += (s, e) => OnSelectionChanged();
        }

        /// <summary>
        /// Gets or sets the selected data.
        /// </summary>
        /// <value>
        /// The selected data.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DeviceMemoryProfilerConfig SelectedData
        {
            get
            {
                UpdateDataFromControls();
                return _data;
            }
            set
            {
                _data = value;
                BindControls();
            }
        }

        private void BindControls()
        {
            checkBoxCountBased.Checked = _data.SampleAtCountIntervals;
            checkBoxTimeBased.Checked = _data.SampleAtTimeIntervals;
            timeSpanControlSampleTarget.Value = _data.TargetSampleTime;
            numericUpDownCountSampleTarget.Value = _data.TargetSampleCount;
        }

        private void UpdateDataFromControls()
        {
            _data.SampleAtCountIntervals = checkBoxCountBased.Checked;
            _data.SampleAtTimeIntervals = checkBoxTimeBased.Checked;
            _data.TargetSampleCount = (int)numericUpDownCountSampleTarget.Value;
            _data.TargetSampleTime = timeSpanControlSampleTarget.Value;
        }

        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void checkBoxTimeBased_CheckedChanged(object sender, EventArgs e)
        {
            timeSpanControlSampleTarget.Enabled = checkBoxTimeBased.Checked;
            if (checkBoxTimeBased.Checked)
            {
                checkBoxCountBased.Checked = false;
            }
        }

        private void checkBoxCountBased_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownCountSampleTarget.Enabled = checkBoxCountBased.Checked;
            if (checkBoxCountBased.Checked)
            {
                checkBoxTimeBased.Checked = false;
            }
        }

        /// <summary>
        /// Validates the memory collection settings.
        /// </summary>
        /// <returns>ValidationResult</returns>
        public ValidationResult ValidateMemoryCollectionSettings()
        {
            ValidationResult vr = ValidationResult.Success;
            if (numericUpDownCountSampleTarget.Enabled)
            {
                vr = IntervalMinimum();
            }
            else if (timeSpanControlSampleTarget.Enabled)
            {
                vr = TimeSpanMinimum();
            }
            return vr;
        }

        /// <summary>
        /// Check the number of intervals between memory gathering. If less than one, throws.
        /// </summary>
        /// <returns>ValidationResult</returns>
        private ValidationResult IntervalMinimum()
        {
            ValidationResult vr = ValidationResult.Success;

            if (numericUpDownCountSampleTarget.Enabled)
            {
                // the focus must be called in case the user manually changes the time.
                Focus();
                int interval = int.Parse(numericUpDownCountSampleTarget.Value.ToString());
                if (interval < 1)
                {
                    vr = new ValidationResult(false, "Minimum number of intervals is one (1).");
                }
            }

            return vr;
        }
        /// <summary>
        /// Validates the minimum time span, four minutes, for collecting memory.
        /// </summary>
        /// <returns></returns>
        private ValidationResult TimeSpanMinimum()
        {
            ValidationResult vr = ValidationResult.Success;

            if (timeSpanControlSampleTarget.Enabled)
            {
                // the focus must be called in case the user manually changes the time.
                Focus();
                TimeSpan ts = timeSpanControlSampleTarget.Value;
                TimeSpan min = new TimeSpan(0, 4, 0);

                if (ts.Minutes < min.Minutes)
                {
                    vr = new ValidationResult(false, "Minimum time is four (4) minutes");
                }
            }

            return vr;
        }

    }
}
