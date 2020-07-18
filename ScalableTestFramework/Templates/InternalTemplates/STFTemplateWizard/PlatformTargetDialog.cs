using System.Windows.Forms;

namespace STFTemplateWizard
{
    public partial class PlatformTargetDialog : Form
    {
        public string PlatformTarget { get; private set; }

        public PlatformTargetDialog()
        {
            InitializeComponent();
            platformComboBox.SelectedIndex = 0;
        }

        private void okButton_Click( object sender, System.EventArgs e )
        {
            PlatformTarget = platformComboBox.Text;
        }
    }
}
