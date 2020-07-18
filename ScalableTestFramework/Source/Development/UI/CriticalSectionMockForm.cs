using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// A form that provides configuration options for an <see cref="ICriticalSectionMock" />.
    /// </summary>
    public partial class CriticalSectionMockForm : Form
    {
        private readonly IPluginFrameworkSimulator _simulator;
        private readonly Dictionary<CriticalSectionMockBehavior, RadioButton> _behaviorMap;
        private bool _initializing = true;

        private CriticalSectionMockForm()
        {
            InitializeComponent();

            _behaviorMap = new Dictionary<CriticalSectionMockBehavior, RadioButton>
            {
                [CriticalSectionMockBehavior.RunAction] = runActionRadioButton,
                [CriticalSectionMockBehavior.ThrowAcquireTimeoutException] = acquireTimeoutRadioButton,
                [CriticalSectionMockBehavior.ThrowHoldTimeoutException] = holdTimeoutRadioButton
            };

            foreach (RadioButton radioButton in _behaviorMap.Values)
            {
                radioButton.CheckedChanged += radioButton_CheckedChanged;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalSectionMockForm" /> class.
        /// </summary>
        /// <param name="simulator">The <see cref="IPluginFrameworkSimulator" /> to configure.</param>
        /// <exception cref="ArgumentNullException"><paramref name="simulator" /> is null.</exception>
        public CriticalSectionMockForm(IPluginFrameworkSimulator simulator)
            : this()
        {
            _simulator = simulator ?? throw new ArgumentNullException(nameof(simulator));
        }

        private void CriticalSectionMockForm_Shown(object sender, EventArgs e)
        {
            _initializing = true;
            if (_simulator.CriticalSection is ICriticalSectionMock mock)
            {
                _behaviorMap[mock.Behavior].Checked = true;
            }
            _initializing = false;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!_initializing)
            {
                if (_simulator.CriticalSection is ICriticalSectionMock mock)
                {
                    mock.Behavior = _behaviorMap.First(n => n.Value == sender).Key;
                }
            }
        }

        private void CriticalSectionMockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
