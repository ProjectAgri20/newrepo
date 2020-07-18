/*
* © Copyright 2016 HP Development Company, L.P.
*/
using System;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace Plugin.SdkGeneralExample
{
    public partial class SdkGeneralExampleConfigControl : UserControl, IPluginConfigurationControl
    {
        // Create the definition of the data that will be used by this activity.  It will be 
        // instantiated later when the plugin is started up.
        private SdkGeneralExampleActivityData _data = null;

        public SdkGeneralExampleConfigControl()
        {
            InitializeComponent();

            // Register field validation for when ValidateConfiguration() is called

            // Require that the activity label textbox have a value
            fieldValidator.RequireValue(activityLabel_TextBox, label1);

            // Require that at least one asset and document be selected
            fieldValidator.RequireAssetSelection(assetSelectionControl);
            fieldValidator.RequireDocumentSelection(documentSelectionControl);

            // Register ConfigurationChanged events
            activityLabel_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            assetSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            documentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        #region IPluginConfigurationControl

        // This event can be used to tell the core framework that your edit control has changes.
        // This will cause the framework to prompt the user to save the changes if they
        // move off the control and haven't selected save.
        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            // Initialize the activity data with a default value for the Label
            _data = new SdkGeneralExampleActivityData("Default");
            activityLabel_TextBox.Text = _data.Label;

            // Initialize both the asset selection control and document selection control.
            // The document selection control will populate its tree control with all
            // documents available in the database.
            documentSelectionControl.Initialize();
            assetSelectionControl.Initialize(AssetAttributes.Printer);
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from an existing copy of configuration information.
            _data = configuration.GetMetadata<SdkGeneralExampleActivityData>();
            activityLabel_TextBox.Text = _data.Label;

            // Initialize both the asset selection control and document selection control.
            // In this case an existing configuration is being provided, so initialize the 
            // controls with any documents or assets that were chosen previously.  They
            // will show up in the lists as previously selected when the user makes a selection.
            documentSelectionControl.Initialize(configuration.Documents);
            assetSelectionControl.Initialize(configuration.Assets, AssetAttributes.Printer);
        }

        public PluginConfigurationData GetConfiguration()
        {
            _data.Label = activityLabel_TextBox.Text;

            // Construct and return a new configuration object.  The framework
            // will get this object and persist it to the database.  The 
            // version number (hardwired to "1.0" in this example) is used to
            // identify what version of metadata is being saved.  This will 
            // allow you to change your metdadata schema over time and assign a 
            // new version.  Then as you provide converters, the framework will 
            // call them on older versions to get them updated to the current version
            // used by the plugin. 
            return new PluginConfigurationData(_data, "1.0")
            {
                Assets = assetSelectionControl.AssetSelectionData,
                Documents = documentSelectionControl.DocumentSelectionData
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            // This is where you can add any validation for your UI and then
            // return the appropriate validation result when saving the data. 
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        #endregion
    }
}
