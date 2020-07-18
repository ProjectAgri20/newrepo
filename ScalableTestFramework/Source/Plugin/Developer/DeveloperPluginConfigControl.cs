using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Developer
{
    [ToolboxItem(false)]
    public partial class DeveloperPluginConfigControl : UserControl, IPluginConfigurationControl
    {
        public DeveloperPluginConfigControl()
        {
            InitializeComponent();

            resultComboBox.DataSource = EnumUtil.GetValues<DeveloperPluginResult>().ToList();
            resultComboBox.SelectedIndexChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            documentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
            remotePrintQueueSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            resultComboBox.SelectedItem = DeveloperPluginResult.Random;
            assetSelectionControl.Initialize(AssetAttributes.None);
            documentSelectionControl.Initialize();
            remotePrintQueueSelectionControl.Initialize();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            DeveloperPluginActivityData activityData = configuration.GetMetadata<DeveloperPluginActivityData>();

            resultComboBox.SelectedItem = activityData.Result;
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.None);
            documentSelectionControl.Initialize(configuration.Documents);
            remotePrintQueueSelectionControl.Initialize(configuration.PrintQueues);
        }

        public PluginConfigurationData GetConfiguration()
        {
            DeveloperPluginActivityData activityData = new DeveloperPluginActivityData
            {
                Result = (DeveloperPluginResult)resultComboBox.SelectedItem
            };

            return new PluginConfigurationData(activityData, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = documentSelectionControl.DocumentSelectionData,
                PrintQueues = remotePrintQueueSelectionControl.PrintQueueSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }
    }
}
