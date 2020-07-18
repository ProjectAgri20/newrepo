using System;
using System.Data.Objects.DataClasses;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Form that hosts a scenario configuration control.
    /// </summary>
    public partial class ScenarioConfigurationControlForm : Form
    {
        private IScenarioConfigurationControl _control;

        /// <summary>
        /// Gets the entity object owned by this control.
        /// </summary>
        public EntityObject EntityObject
        {
            get
            {
                _control.FinalizeEdit();
                return _control.EntityObject;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioConfigurationControlForm"/> class.
        /// </summary>
        private ScenarioConfigurationControlForm()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.Standard);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioConfigurationControlForm"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public ScenarioConfigurationControlForm(ConfigurationObjectTag tag, EnterpriseTestContext context)
            : this()
        {
            _control = ScenarioConfigurationControlFactory.Create(tag, context);
            _control.Initialize(tag);
            scenarioConfigurationControl_Panel.Controls.Add(_control as Control);
            (_control as Control).Dock = DockStyle.Fill;
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            ValidationResult result = _control.Validate();
            if (result.Succeeded)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("The following errors must be corrected before changes can be saved:\n" + result.Message,
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScenarioConfigurationControlForm_Load(object sender, EventArgs e)
        {
            resource_ToolStripLabel.Text = _control.EditFormTitle;
        }
    }
}
