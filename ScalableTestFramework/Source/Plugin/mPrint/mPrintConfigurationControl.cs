using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.mPrint
{
    /// <summary>
    /// Provides the control to configure the mPrint activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class mPrintConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private mPrintActivityData _activityData = null;
        private PluginConfigurationData _configData = null;
        private AssetInfo _selectedAsset = null;
        private const long MAX_FILE_SIE = 10240;

        /// <summary>
        /// Initializes a new instance of the mPrintConfigurationControl class.
        /// </summary>
        /// <remarks>
        /// Link the property changed event of each control to this class's OnConfigurationChanged event handler method.
        /// </remarks>
        public mPrintConfigurationControl()
        {
            InitializeComponent();

            //Set up Validation
            fieldValidator.RequireSelection(mPrint_ServerComboBox, mPrintServer_Label);
            fieldValidator.RequireDocumentSelection(documentSelectionControl);
            fieldValidator.SetIconAlignment(mPrint_ServerComboBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(documentSelectionControl, ErrorIconAlignment.TopLeft);
            fieldValidator.SetIconAlignment(queueIndex_TextBox, ErrorIconAlignment.TopLeft);



            //Set up Change Notification
            mPrint_ServerComboBox.SelectionChanged += mPrint_ServerComboBox_SelectionChanged;
            documentSelectionControl.SelectionChanged += OnConfigurationChanged;
        }



        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Returns the configuration data for this activity.
        /// </summary>
        /// <returns></returns>
        public PluginConfigurationData GetConfiguration()
        {

            _activityData.serv = mPrint_ServerComboBox.SelectedServer;
            _activityData.queueIndex = queueIndex_TextBox.Text;

            PluginConfigurationData result = new PluginConfigurationData(_activityData, "1.0")
            {
                //Assets = GetAssociatedAssetData(),
                Documents = documentSelectionControl.DocumentSelectionData,
                Servers = new ServerSelectionData(mPrint_ServerComboBox.SelectedServer)
            };

            return result;
        }

        //private AssetSelectionData GetAssociatedAssetData()
        //{
        //    return new AssetSelectionData(_selectedAsset);
        //}

        private DocumentQuery GetDocumentFilter()
        {
            DocumentQuery query = new DocumentQuery();
            query.Criteria.Add(new DocumentQueryCriteria(DocumentQueryProperty.FileSize, QueryOperator.LessThanOrEqual, MAX_FILE_SIE));
            return query;
        }

        /// <summary>
        /// Initializes the configuration control with default settings.
        /// </summary>
        /// <param name="environment"></param>
        public void Initialize(PluginEnvironment environment)
        {
            var allExtensions = ConfigurationServices.DocumentLibrary.GetExtensions();
            var validExtensions = allExtensions.Where(n => n.Extension.Equals("JPG") || n.Extension.Equals("PDF"));
            documentSelectionControl.Initialize(validExtensions);
            _activityData = new mPrintActivityData();

            mPrint_ServerComboBox.Initialize("mPrint");
        }

        /// <summary>
        /// Initializes the configuration control with the specified settings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            //Initialize the activity data by deserializing it from the configuration information
            _configData = configuration;
            _activityData = configuration.GetMetadata<mPrintActivityData>();
            _selectedAsset = ConfigurationServices.AssetInventory.GetAsset(_configData.Assets.SelectedAssets.FirstOrDefault());

            mPrint_ServerComboBox.Initialize(_configData.Servers.SelectedServers.FirstOrDefault(), "mPrint");
            mPrint_ServerComboBox_SelectionChanged(mPrint_ServerComboBox, EventArgs.Empty);
            queueIndex_TextBox.Text = _activityData.queueIndex;

            var allExtensions = ConfigurationServices.DocumentLibrary.GetExtensions();
            var validExtensions = allExtensions.Where(n => n.Extension.Equals("JPG") || n.Extension.Equals("PDF"));

            documentSelectionControl.Initialize(configuration.Documents, validExtensions);
        }

        /// <summary>
        /// Validates the activity's configuration data.
        /// </summary>
        /// <returns></returns>
        public PluginValidationResult ValidateConfiguration() => new PluginValidationResult(fieldValidator.ValidateAll());

        private void mPrint_ServerComboBox_SelectionChanged(object sender, EventArgs e)
        {
            ServerInfo server = mPrint_ServerComboBox.SelectedServer;
            OnConfigurationChanged(sender, e);
        }

        /// <summary>
        /// Event handler to be called whenever the activity's configuration data changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(this, e);
        }
    }
}
