using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Email
{
    /// <summary>
    /// Control used for editing Email activities.
    /// </summary>
    [ToolboxItem(false)]
    public partial class EmailConfigControl : UserControl, IPluginConfigurationControl
    {
        private const long MAX_FILE_SIZE = 10240; // Max file size for the Exchange server

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="EmailConfigControl"/>
        /// </summary>
        public EmailConfigControl()
        {
            InitializeComponent();

            fieldValidator.RequireSelection(exchange_ServerComboBox, exchangeServer_Label);
            fieldValidator.RequireValue(subject_TextBox, subject_Label);
            fieldValidator.SetIconAlignment(exchange_ServerComboBox, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(toCount_NumericUpDown, ErrorIconAlignment.MiddleLeft);
            fieldValidator.SetIconAlignment(subject_TextBox, ErrorIconAlignment.MiddleLeft);

            exchange_ServerComboBox.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            toCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            ccCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            subject_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            body_TextBox.TextChanged += (s, e) => ConfigurationChanged(s, e);
            attachAll_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            documentCount_NumericUpDown.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            attachments_DocumentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
        }

        public void Initialize(PluginEnvironment environment)
        {
            attachments_DocumentSelectionControl.Initialize(GetDocumentFilter());
            exchange_ServerComboBox.Initialize("Exchange");
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            EmailActivityData emailData = configuration.GetMetadata<EmailActivityData>();
            attachments_DocumentSelectionControl.Initialize(configuration.Documents, GetDocumentFilter());
            exchange_ServerComboBox.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "Exchange");

            toCount_NumericUpDown.Value = emailData.ToRandomCount;
            ccCount_NumericUpDown.Value = emailData.CCRandomCount;
            subject_TextBox.Text = emailData.Subject;
            body_TextBox.Text = emailData.Body;
            attachAll_RadioButton.Checked = emailData.SelectAllDocuments;
            attachSome_RadioButton.Checked = !emailData.SelectAllDocuments;
            documentCount_NumericUpDown.Value = emailData.NumberOfDocuments;
        }

        public PluginConfigurationData GetConfiguration()
        {
            EmailActivityData emailData = new EmailActivityData()
            {
                ToRandomCount = (int)toCount_NumericUpDown.Value,
                CCRandomCount = (int)ccCount_NumericUpDown.Value,
                Subject = subject_TextBox.Text,
                Body = body_TextBox.Text,
                SelectAllDocuments = attachAll_RadioButton.Checked,
                NumberOfDocuments = (int)documentCount_NumericUpDown.Value
            };

            return new PluginConfigurationData(emailData, "1.0")
            {
                Documents = attachments_DocumentSelectionControl.DocumentSelectionData,
                Servers = new ServerSelectionData(exchange_ServerComboBox.SelectedServer)
            };
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        private DocumentQuery GetDocumentFilter()
        {
            DocumentQuery query = new DocumentQuery();
            query.Criteria.Add(new DocumentQueryCriteria(DocumentQueryProperty.FileSize, QueryOperator.LessThanOrEqual, MAX_FILE_SIZE));
            return query;
        }

        private bool ValidateNumericValue(NumericUpDown numericUpDownControl)
        {
            return numericUpDownControl.Value > 0;
        }
    }
}
