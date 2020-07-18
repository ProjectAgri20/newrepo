using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// Simple form for displaying plugin configuration data.
    /// </summary>
    public partial class PluginConfigurationDataDisplayForm : Form
    {
        private PluginConfigurationDataDisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigurationDataDisplayForm" /> class.
        /// </summary>
        /// <param name="configurationData">The <see cref="PluginConfigurationData" /> to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="configurationData" /> is null.</exception>
        public PluginConfigurationDataDisplayForm(PluginConfigurationData configurationData)
            : this()
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            xmlDisplayTextBox.Text = configurationData.GetMetadata().ToString();
            xmlDisplayTextBox.Select(0, 0);
        }
    }
}
