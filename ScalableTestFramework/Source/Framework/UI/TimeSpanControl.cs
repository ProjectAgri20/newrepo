using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for selecting a <see cref="TimeSpan" />.
    /// </summary>
    public partial class TimeSpanControl : UserControl
    {
        /// <summary>
        /// Occurs when the <see cref="Value" /> property changes.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the value of the control changes.")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan" /> value.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan Value
        {
            get { return TimeSpanFromDateTime(timeSpan_DateTimePicker.Value); }
            set { timeSpan_DateTimePicker.Value = DateTimeFromTimeSpan(value); }
        }

        /// <summary>
        /// Gets or sets the custom format for the <see cref="DateTimePicker" /> in this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected string CustomFormat
        {
            get { return timeSpan_DateTimePicker.CustomFormat; }
            set { timeSpan_DateTimePicker.CustomFormat = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanControl" /> class.
        /// </summary>
        public TimeSpanControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a <see cref="TimeSpan" /> from the specified <see cref="DateTime" />.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>A <see cref="TimeSpan" /> that corresponds to the user-picked <see cref="DateTime" />.</returns>
        protected virtual TimeSpan TimeSpanFromDateTime(DateTime dateTime)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        /// <summary>
        /// Creates a <see cref="DateTime" /> from the specified <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>A <see cref="DateTime" /> that should be display for the specified <see cref="TimeSpan" />.</returns>
        protected virtual DateTime DateTimeFromTimeSpan(TimeSpan timeSpan)
        {
            return timeSpan_DateTimePicker.MinDate + timeSpan;
        }

        private void timeSpan_DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
