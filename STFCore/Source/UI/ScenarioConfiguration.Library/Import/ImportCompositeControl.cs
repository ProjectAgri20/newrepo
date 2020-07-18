using System.Windows.Forms;
using HP.ScalableTest.Core.ImportExport;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ImportCompositeControl : UserControl
    {
        private ErrorProvider _errorProvider = new ErrorProvider();
        private EnterpriseScenarioContract _contract = new EnterpriseScenarioContract();

        //public ImportCompositeControl(Control control)
        public ImportCompositeControl()
        {
            InitializeComponent();

            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            //control.Dock = DockStyle.Fill;
            //importMainPanel.Controls.Add(control);

            Dock = DockStyle.Fill;
        }

        public void SetPanel(Control control)
        {
            control.Dock = DockStyle.Fill;

            importMainPanel.Controls.Clear();
            importMainPanel.Controls.Add(control);
        }

        public new bool Validate()
        {
            return ValidateChildren();
        }

        public void UpdateContractData(EnterpriseScenarioContract contract)
        {
            scenarioNameTextBox.DataBindings.Clear();
            descriptionTextBox.DataBindings.Clear();

            _contract = contract;

            scenarioNameTextBox.Text = contract.Name;
            descriptionTextBox.Text = contract.Description;

            scenarioNameTextBox.DataBindings.Add("Text", _contract, "Name");
            descriptionTextBox.DataBindings.Add("Text", _contract, "Description");
        }

        private void scenarioNameTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(_contract.Name))
            {
                _errorProvider.SetError(scenarioNameTextBox, "A Scenario Name must be provided");
                e.Cancel = true;
                return;
            }
            else
            {
                _errorProvider.SetError(scenarioNameTextBox, string.Empty);
            }
        }
    }
}
