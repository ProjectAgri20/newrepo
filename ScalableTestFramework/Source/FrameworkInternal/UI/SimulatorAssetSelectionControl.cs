using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.UI
{
    /// <summary>
    /// Specialized version of <see cref="AssetSelectionControl" /> that includes support for device simulator configuration.
    /// </summary>
    public partial class SimulatorAssetSelectionControl : AssetSelectionControl
    {
        /// <summary>
        /// Gets or sets the automation pause to use for simulators.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AutomationPause { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the simulated ADF should be used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseAdf { get; set; }

        /// <summary>
        /// Gets or sets the set of documents to load into the ADF.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentSelectionData AdfDocuments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulatorAssetSelectionControl" /> class.
        /// </summary>
        public SimulatorAssetSelectionControl()
        {
            InitializeComponent();

            AutomationPause = TimeSpan.FromSeconds(1);
            UseAdf = false;
        }

        private void simulatorConfiguration_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (SimulatorConfigurationForm form = new SimulatorConfigurationForm(AutomationPause, UseAdf, AdfDocuments))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    AutomationPause = form.AutomationPause;
                    UseAdf = form.UseAdf;
                    AdfDocuments = form.AdfDocuments;
                }
            }
        }

        /// <summary>
        /// Called when there is a change in the displayed assets in this control.
        /// Can be overridden by inheriting classes to customize behavior.
        /// </summary>
        /// <param name="displayedAssets">The displayed assets.</param>
        protected override void OnDisplayedAssetsChanged(AssetInfoCollection displayedAssets)
        {
            // Only display the simulator configuration options if at least one simulator is selected.
            simulatorConfiguration_LinkLabel.Visible = displayedAssets.Any(n => n is DeviceSimulatorInfo);
        }
    }
}
