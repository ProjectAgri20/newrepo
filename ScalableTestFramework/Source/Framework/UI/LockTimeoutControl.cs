using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Control for setting <see cref="LockTimeoutData" />.
    /// </summary>
    public partial class LockTimeoutControl : UserControl
    {
        /// <summary>
        /// Occurs when this control's selection is changed.
        /// </summary>
        [Browsable(true), Category("Action"), Description("Occurs when the value of the control changes.")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Gets the <see cref="LockTimeoutData" /> from this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LockTimeoutData Value
        {
            get
            {
                TimeSpan acquireTimeout = acquireTimeout_TimeSpanControl.Value;
                TimeSpan holdTimeout = holdTimeout_TimeSpanControl.Value;
                return new LockTimeoutData(acquireTimeout, holdTimeout);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockTimeoutControl" /> class.
        /// </summary>
        public LockTimeoutControl()
        {
            InitializeComponent();

            acquireTimeout_TimeSpanControl.ValueChanged += (s, e) => OnValueChanged();
            holdTimeout_TimeSpanControl.ValueChanged += (s, e) => OnValueChanged();
        }

        /// <summary>
        /// Initializes this control with the specified <see cref="LockTimeoutData" />.
        /// </summary>
        /// <param name="lockTimeoutData">The lock timeout data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="lockTimeoutData" /> is null.</exception>
        public void Initialize(LockTimeoutData lockTimeoutData)
        {
            if (lockTimeoutData == null)
            {
                throw new ArgumentNullException(nameof(lockTimeoutData));
            }

            acquireTimeout_TimeSpanControl.Value = lockTimeoutData.AcquireTimeout;
            holdTimeout_TimeSpanControl.Value = lockTimeoutData.HoldTimeout;
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
