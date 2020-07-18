using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.JetAdvantageUpload
{
    [ToolboxItem(false)]
    public partial class JetAdvantageUploadConfigControl : UserControl, IPluginConfigurationControl
    {
        private JetAdvantageUploadActivityData _activityData;

        /// <summary>
        /// Initializes a new instance of <see cref="JetAdvantageUploadConfigControl"/>
        /// </summary>
        public JetAdvantageUploadConfigControl()
        {
            InitializeComponent();

            _activityData = new JetAdvantageUploadActivityData();

            documentSelectionControl.ShowDocumentBrowseControl = true;
            documentSelectionControl.ShowDocumentQueryControl = false;
            documentSelectionControl.ShowDocumentSetControl = false;

            fieldValidator.RequireValue(titanLoginId_TextBox, loginId_Label);
            fieldValidator.RequireValue(titanPassword_TextBox, password_label);
            fieldValidator.RequireDocumentSelection(documentSelectionControl);
            fieldValidator.RequireValue(textBoxJetAdvantageProxy, label_JetProxy);
            fieldValidator.RequireValue(textBoxJetAdvantageURL, label_JetURL);

            titanLoginId_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            titanPassword_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            checkBoxShuffle.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            documentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);

            textBoxJetAdvantageProxy.TextChanged += (s, e) => ConfigurationChanged(s, e);
            textBoxJetAdvantageURL.TextChanged += (s, e) => ConfigurationChanged(s, e);
        }
        // This event can be used to tell the core framework that your edit control has changes.
        // This will cause the framework to prompt the user to save the changes if they
        // move off the control and haven't selected save.
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes this editor with a blank activity.
        /// </summary>
        public void Initialize(PluginEnvironment environment)
        {
            SetControlsByActivityData();

            // Constrain to files tagged as Titan compatible
            documentSelectionControl.Initialize(TitanOnlyFilter());
        }

        /// <summary>
        /// Initializes the control for use with an existing metadata item.
        /// </summary>
        /// <param name="configuration">PluginConfigurationData</param>
        /// <param name="environment">PluginEnvironment</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<JetAdvantageUploadActivityData>();
            SetControlsByActivityData();

            // Constrain to files tagged as Titan compatible
            documentSelectionControl.Initialize(configuration.Documents, TitanOnlyFilter());
        }
        /// <summary>
        /// Gets the current configuration of the plug-in
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {
            _activityData.Shuffle = checkBoxShuffle.Checked;
            _activityData.LoginPassword = titanPassword_TextBox.Text;
            _activityData.LoginId = titanLoginId_TextBox.Text;
            _activityData.StackProxy = textBoxJetAdvantageProxy.Text;
            _activityData.StackURL = textBoxJetAdvantageURL.Text;

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Documents = documentSelectionControl.DocumentSelectionData
            };
        }

        /// <summary>
        /// Document selection is required so checking if a selection has been made. If not, return false.
        /// </summary>
        /// <returns>bool: true if document selected</returns>
        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Titans the only filter.
        /// </summary>
        private DocumentQuery TitanOnlyFilter()
        {
            DocumentQuery query = new DocumentQuery();
            query.Criteria.Add(new DocumentQueryCriteria(DocumentQueryProperty.Tag, QueryOperator.IsIn, "Titan"));
            return query;
        }

        /// <summary>
        /// Sets the data bindings.
        /// </summary>
        private void SetControlsByActivityData()
        {
            checkBoxShuffle.Checked = _activityData.Shuffle;

            titanLoginId_TextBox.Text = _activityData.LoginId;
            titanPassword_TextBox.Text = _activityData.LoginPassword;
            textBoxJetAdvantageProxy.Text = _activityData.StackProxy;
            textBoxJetAdvantageURL.Text = _activityData.StackURL;
        }

    }
}
