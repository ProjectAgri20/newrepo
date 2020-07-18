using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EPrint
{
    [ToolboxItem(false)]
    public partial class EPrintConfigControl : UserControl, IPluginConfigurationControl
    {
        private PluginConfigurationData _configData = null;
        private EPrintActivityData _activityData = null;
        private AssetInfo _selectedAsset = null;
        private const long MAX_FILE_SIZE = 10240; // Max file size for the Exchange server

        public EPrintConfigControl()
        {
            InitializeComponent();

            // Set up Validation
            fieldValidator.RequireSelection(exchange_ServerComboBox, exchangeServer_Label);
            fieldValidator.RequireSelection(ePrint_ServerComboBox, ePrintServer_Label);
            fieldValidator.RequireSelection(email_ComboBox, email_Label);
            fieldValidator.RequireCustom(documentCount_NumericUpDown, () => ValidateDocumentNumber(documentCount_NumericUpDown.Value), "Invalid Document Count.");
            fieldValidator.RequireDocumentSelection(documentSelectionControl);
            fieldValidator.SetIconAlignment(exchange_ServerComboBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(ePrint_ServerComboBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(email_ComboBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(documentSelectionControl, ErrorIconAlignment.TopLeft);

            // Set up Change notification
            exchange_ServerComboBox.SelectionChanged += OnConfigurationChanged;
            ePrint_ServerComboBox.SelectionChanged += ePrint_ServerComboBox_SelectionChanged;
            email_ComboBox.SelectedIndexChanged += Email_ComboBox_SelectedIndexChanged;
            documentCount_NumericUpDown.ValueChanged += OnConfigurationChanged;
            documentSelectionControl.SelectionChanged += OnConfigurationChanged;
        }

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new EPrintActivityData();
            documentSelectionControl.Initialize();
            exchange_ServerComboBox.Initialize("Exchange");
            ePrint_ServerComboBox.Initialize("ePrint");
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            // Initialize the activity data by deserializing it from the configuration information.
            _configData = configuration;
            _activityData = configuration.GetMetadata<EPrintActivityData>();
            _selectedAsset = ConfigurationServices.AssetInventory.GetAsset(_configData.Assets.SelectedAssets.FirstOrDefault());

            // Order is deterministic based on the way it was saved - exchange server first, then ePrint server
            exchange_ServerComboBox.Initialize(_configData.Servers.SelectedServers.FirstOrDefault(), "Exchange");
            ePrint_ServerComboBox.Initialize(_configData.Servers.SelectedServers.Skip(1).FirstOrDefault(), "ePrint");
            ePrint_ServerComboBox_SelectionChanged(ePrint_ServerComboBox, EventArgs.Empty);
            documentSelectionControl.Initialize(configuration.Documents, GetDocumentFilter());

            // Brute force initialization
            email_ComboBox.SelectedIndex = email_ComboBox.FindString(_activityData.PrinterEmail);
            attachAll_RadioButton.Checked = (_activityData.NumberOfDocuments == _configData.Documents.SelectedDocuments.Count);
            documentCount_NumericUpDown.Value = _activityData.NumberOfDocuments;
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.PrinterEmail = email_ComboBox.Text;
            _activityData.NumberOfDocuments = (int)documentCount_NumericUpDown.Value;

            PluginConfigurationData result = new PluginConfigurationData(_activityData, "1.0")
            {
                Assets = GetAssociatedAssetData(),
                Documents = documentSelectionControl.DocumentSelectionData,
                Servers = new ServerSelectionData()
            };

            //Add the servers - order is important since this is used when loading saved metadata
            result.Servers.SelectedServers.Add(exchange_ServerComboBox.SelectedServer.ServerId);
            result.Servers.SelectedServers.Add(ePrint_ServerComboBox.SelectedServer.ServerId);

            return result;
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private AssetSelectionData GetAssociatedAssetData()
        {
            return new AssetSelectionData(_selectedAsset);
        }

        private DocumentQuery GetDocumentFilter()
        {
            DocumentQuery query = new DocumentQuery();
            query.Criteria.Add(new DocumentQueryCriteria(DocumentQueryProperty.FileSize, QueryOperator.LessThanOrEqual, MAX_FILE_SIZE));
            return query;
        }

        private void LoadPrintQueues(ServerInfo server)
        {
            email_ComboBox.DataSource = null;
            email_ComboBox.DataSource = ConfigurationServices.AssetInventory.GetRemotePrintQueues(server).ToList();
            email_ComboBox.DisplayMember = "QueueName";
            email_ComboBox.ValueMember = "PrintQueueId";
        }

        private void ePrint_ServerComboBox_SelectionChanged(object sender, EventArgs e)
        {
            ServerInfo server = ePrint_ServerComboBox.SelectedServer;
            if (server != null)
            {
                LoadPrintQueues(server);
            }
            OnConfigurationChanged(sender, e);
        }

        /// <summary>
        /// Set the selected asset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Email_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (email_ComboBox.SelectedItem != null)
            {
                RemotePrintQueueInfo printQueue = (RemotePrintQueueInfo)email_ComboBox.SelectedItem;
                _selectedAsset = ConfigurationServices.AssetInventory.GetAsset(printQueue.AssociatedAssetId);
                device_TextBox.Text = printQueue.AssociatedAssetId;
            }
            OnConfigurationChanged(sender, e);
        }

        private void OnConfigurationChanged(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(this, e);
                //System.Diagnostics.Debug.WriteLine("Config Changed.");
            }
        }

        private bool ValidateDocumentNumber(decimal numDocsSelected)
        {
            return numDocsSelected <= documentSelectionControl.DocumentSelectionData.SelectedDocuments.Count;
        }

    }
}
