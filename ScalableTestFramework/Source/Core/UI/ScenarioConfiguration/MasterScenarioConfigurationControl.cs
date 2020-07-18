using System;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Core.UI.ScenarioConfiguration
{
    /// <summary>
    /// Master control for the scenario configuration tab in the admin console.
    /// </summary>
    public partial class MasterScenarioConfigurationControl : UserControl
    {
        private ScenarioConfigurationUIController _uiController;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterScenarioConfigurationControl" /> class.
        /// </summary>
        public MasterScenarioConfigurationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the master scenario configuration control for use.
        /// </summary>
        public void Initialize()
        {
            _uiController = new ScenarioConfigurationUIController(DbConnect.EnterpriseTestConnectionString);
            scenarioConfigurationTreeView.Initialize(_uiController);

            _uiController.Load();
        }

        private void configurationTreeRefresh_ToolStripButton_Click(object sender, EventArgs e)
        {
            scenarioConfigurationTreeView.ClearTreeView();
            _uiController.Load();
        }

        private void save_ToolStripButton_Click(object sender, EventArgs e)
        {
            _uiController.Save();
        }
    }
}
