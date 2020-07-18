using System.Windows.Forms;

namespace HP.ScalableTest.Development.UI
{
    /// <summary>
    /// Simple form for renaming configuration data.
    /// </summary>
    public partial class ConfigurationDataRenameForm : Form
    {
        /// <summary>
        /// Gets or sets the configuration data name.
        /// </summary>
        public string ConfigurationDataName
        {
            get => nameTextbox.Text;
            set => nameTextbox.Text = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDataRenameForm" /> class.
        /// </summary>
        public ConfigurationDataRenameForm()
        {
            InitializeComponent();
        }

        private void ConfigurationDataRenameForm_Shown(object sender, System.EventArgs e)
        {
            nameTextbox.SelectAll();
            nameTextbox.Focus();
        }
    }
}
