using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    public partial class WorkerActivityMetadataControl : UserControl, IScenarioConfigurationControl
    {
        protected VirtualResourceMetadata _metadata;
        protected IPluginConfigurationControl _editor;
        protected string _editorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerActivityMetadataControl"/> class.
        /// </summary>
        public WorkerActivityMetadataControl()
        {
            InitializeComponent();

            fieldValidator.RequireValue(name_TextBox, name_Label);
        }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public EnterpriseTestContext Context { get; set; }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public EntityObject EntityObject
        {
            get { return _metadata; }
        }

        /// <summary>
        /// The title displayed when the edit form of the plugin is loaded.
        /// </summary>
        public string EditFormTitle
        {
            get
            {
                string title = string.Empty;

                if (!string.IsNullOrEmpty(_editorType))
                {
                    title = _editorType + " Configuration";
                }

                return title;
            }
        }

        /// <summary>
        /// True if changes are pending, false otherwise.
        /// </summary>
        public bool HasUnsavedChanges { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            // Do nothing
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object based on the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Initialize(ConfigurationObjectTag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            Initialize(new VirtualResourceMetadata(tag.ResourceType.ToString(), tag.MetadataType));
        }

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        public void Initialize(object entity)
        {
            _metadata = entity as VirtualResourceMetadata;
            if (_metadata == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(VirtualResourceMetadata));
            }

            name_TextBox.Text = _metadata.Name;

            // Remove existing plugin from the editor panel
            foreach (IPluginConfigurationControl control in metadataEditor_Panel.Controls.OfType<IPluginConfigurationControl>())
            {
                control.ConfigurationChanged -= PluginControl_ConfigurationChanged;
            }
            metadataEditor_Panel.Controls.Clear();

            // Create the edit plugin and add it to the editor panel
            try
            {
                IPluginConfigurationControl pluginControl = CreatePluginControl();
                Control control = (Control)pluginControl;
                control.Dock = DockStyle.Fill;
                metadataEditor_Panel.Controls.Add(control);
            }
            catch (PluginLoadException ex)
            {
                MessageBox.Show(ex.Message, "Plugin Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Reset unsaved changes
            HasUnsavedChanges = false;
        }

        private IPluginConfigurationControl CreatePluginControl()
        {
            PluginAssembly assembly = null;
            try
            {
                assembly = PluginFactory.GetPlugin(_metadata.MetadataType);
            }
            catch (InvalidOperationException)
            {
                // Assembly stays null; error is thrown below
            }

            if (assembly?.Implements<IPluginConfigurationControl>() == true)
            {
                IPluginConfigurationControl configurationControl = assembly.Create<IPluginConfigurationControl>();
                configurationControl.ConfigurationChanged += PluginControl_ConfigurationChanged;
                InitializePluginControl(configurationControl);
                return configurationControl;
            }
            else
            {
                string errorMessage = Resource.PluginLoadErrorMessage.FormatWith(_metadata.MetadataType);
                TraceFactory.Logger.Error(errorMessage);
                throw new PluginLoadException(errorMessage);
            }
        }

        private void InitializePluginControl(IPluginConfigurationControl configurationControl)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (SystemSetting pluginSetting in context.SystemSettings.Where(n => n.Type.Equals("PluginSetting") && n.SubType.Equals(_metadata.MetadataType)))
                {
                    settings.Add(pluginSetting.Name, pluginSetting.Value);
                }
            }

            PluginEnvironment environment = new PluginEnvironment
            (
                new SettingsDictionary(settings),
                GlobalSettings.Items[Setting.Domain],
                GlobalSettings.Items[Setting.DnsDomain]
            );

            if (string.IsNullOrEmpty(_metadata.Metadata))
            {
                configurationControl.Initialize(environment);
            }
            else
            {
                PluginConfigurationData configurationData = _metadata.BuildConfigurationData();
                configurationControl.Initialize(configurationData, environment);
            }
            _editorType = _metadata.MetadataType;
            _editor = configurationControl;
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public void FinalizeEdit()
        {
            // Modify focus so that any data bindings will update
            name_Label.Focus();
            _metadata.Name = name_TextBox.Text;
            UpdateMetadata(_metadata, _editor);
        }

        private static void UpdateMetadata(VirtualResourceMetadata metadata, IPluginConfigurationControl control)
        {
            // Get the XML metadata from the plugin
            PluginConfigurationData editorData = control.GetConfiguration();
            metadata.Metadata = editorData.GetMetadata().ToString();
            metadata.MetadataVersion = editorData.MetadataVersion;

            // Save asset selection data
            if (editorData.Assets != null)
            {
                if (metadata.AssetUsage == null)
                {
                    metadata.AssetUsage = new VirtualResourceMetadataAssetUsage();
                }
                metadata.AssetUsage.AssetSelectionData = Serializer.Serialize(editorData.Assets).ToString();
            }
            else
            {
                metadata.AssetUsage = null;
            }

            // Save document selection data
            if (editorData.Documents != null)
            {
                if (metadata.DocumentUsage == null)
                {
                    metadata.DocumentUsage = new VirtualResourceMetadataDocumentUsage();
                }
                metadata.DocumentUsage.DocumentSelectionData = Serializer.Serialize(editorData.Documents).ToString();
            }
            else
            {
                metadata.DocumentUsage = null;
            }

            // Save server selection data
            if (editorData.Servers != null)
            {
                if (metadata.ServerUsage == null)
                {
                    metadata.ServerUsage = new VirtualResourceMetadataServerUsage();
                }
                metadata.ServerUsage.ServerSelectionData = Serializer.Serialize(editorData.Servers).ToString();
            }
            else
            {
                metadata.ServerUsage = null;
            }

            // Save print queue selection data
            if (editorData.PrintQueues != null)
            {
                if (metadata.PrintQueueUsage == null)
                {
                    metadata.PrintQueueUsage = new VirtualResourceMetadataPrintQueueUsage();
                }
                metadata.PrintQueueUsage.PrintQueueSelectionData = Serializer.Serialize(editorData.PrintQueues).ToString();
            }
            else
            {
                metadata.PrintQueueUsage = null;
            }
        }

        /// <summary>
        /// Validates this control.
        /// </summary>
        /// <returns>A <see cref="ValidationResult" /> representing the outcome of validation.</returns>
        public virtual ValidationResult Validate()
        {
            bool pluginValidationSuccess = true;
            StringBuilder builder = new StringBuilder();

            ValidationResult hasNameResult = fieldValidator.Validate(name_TextBox);
            if (!hasNameResult.Succeeded)
            {
                builder.AppendLine(hasNameResult.Message);
            }

            PluginValidationResult pluginResult = _editor.ValidateConfiguration();
            if (!pluginResult.Succeeded)
            {
                pluginValidationSuccess = false;
                builder.Append(string.Join("\n", pluginResult.ErrorMessages));
            }

            return new ValidationResult(pluginValidationSuccess && hasNameResult.Succeeded, builder.ToString().TrimEnd());
        }

        private void name_TextBox_TextChanged(object sender, EventArgs e)
        {
            HasUnsavedChanges = true;
        }

        private void PluginControl_ConfigurationChanged(object sender, EventArgs e)
        {
            HasUnsavedChanges = true;
        }
    }
}
