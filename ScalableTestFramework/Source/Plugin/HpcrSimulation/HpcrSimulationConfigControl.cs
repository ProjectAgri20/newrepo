using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Hpcr;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpcrSimulation
{
    /// <summary>
    /// Edit control for a HPCR Plug-in
    /// Need as many office workers as recipients
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpcrSimulationConfigControl : UserControl, IPluginConfigurationControl
    {
        private HpcrSimulationData _activityData;
        private HpcrConfigurationProxyClient _hpcrClient = null;
        private IHpcrConfigurationProxyService _proxyClient = null;

        // used to load the initial distribution titles
        private string _distributionOriginator = string.Empty;

        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrSimulationConfigControl"/> class.
        /// </summary>
        public HpcrSimulationConfigControl()
        {
            InitializeComponent();

            documentSelectionControl.ShowDocumentBrowseControl = true;
            documentSelectionControl.ShowDocumentQueryControl = false;
            documentSelectionControl.ShowDocumentSetControl = false;

            fieldValidator.RequireSelection(serverComboBoxHpcr, "An HPCR Server");
            fieldValidator.RequireSelection(comboBox_EmailOriginator, "An email address", email_RadioButton);
            fieldValidator.RequireSelection(comboBox_Distributions, "A distribution", distribute_RadioButton);
            fieldValidator.RequireSelection(comboBox_DistributionOriginator, "A distribution originator", distribute_RadioButton);
            fieldValidator.RequireDocumentSelection(documentSelectionControl);

            serverComboBoxHpcr.SelectionChanged += (s, e) => ConfigurationChanged(s, e);
            comboBox_DistributionOriginator.TextChanged += (s, e) => ConfigurationChanged(s, e);
            comboBox_Distributions.TextChanged += (s, e) => ConfigurationChanged(s, e);
            comboBox_EmailOriginator.TextChanged += (s, e) => ConfigurationChanged(s, e);
            numericUpDownToCount.ValueChanged += (s, e) => ConfigurationChanged(s, e);
            email_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            distribute_RadioButton.CheckedChanged += (s, e) => ConfigurationChanged(s, e);
            documentSelectionControl.SelectionChanged += (s, e) => ConfigurationChanged(s, e);

        }
        /// <summary>
        /// Initializes control for new data.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public void Initialize(PluginEnvironment environment)
        {
            _hpcrClient = new HpcrConfigurationProxyClient(environment.PluginSettings["HpcrProxy"]);
            _activityData = new HpcrSimulationData();
            _distributionOriginator = string.Empty;

            serverComboBoxHpcr.Initialize("HPCR");
            LoadDistributionTitles();
            SetControlsByActivityData();

            documentSelectionControl.Initialize();
        }

        /// <summary>
        /// Initializes the specified configuration from stored meta data
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The environment.</param>
        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _hpcrClient = new HpcrConfigurationProxyClient(environment.PluginSettings["HpcrProxy"]);
            _activityData = configuration.GetMetadata<HpcrSimulationData>();
            _distributionOriginator = _activityData.SendToDistribution.Originator;

            documentSelectionControl.Initialize(configuration.Documents);

            serverComboBoxHpcr.Initialize(configuration.Servers.SelectedServers.FirstOrDefault(), "HPCR");

            LoadDistributionTitles();
            SetControlsByActivityData();
        }

        public PluginConfigurationData GetConfiguration()
        {
            _activityData.SendToEmail.NumberOfRandomRecipients = (int)numericUpDownToCount.Value;

            _activityData.SendToDistribution.Originator = comboBox_DistributionOriginator.Text;
            _activityData.SendToDistribution.DistributionTitle = comboBox_Distributions.Text;
            _activityData.SendToEmail.Originator = comboBox_EmailOriginator.Text;
            _activityData.TestType = GetTestType();

            return new PluginConfigurationData(_activityData, "1.0")
            {
                Documents = documentSelectionControl.DocumentSelectionData,
                Servers = new ServerSelectionData(serverComboBoxHpcr.SelectedServer)
            };
        }

        private HpcrTestType GetTestType()
        {
            HpcrTestType htt = HpcrTestType.DeliverToRandomEmails;

            if (distribute_RadioButton.Checked)
            {
                htt = HpcrTestType.DeliverToUserDistribution;
            }

            return htt;
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(fieldValidator.ValidateAll());
        }

        /// <summary>
        /// Gets the proxy client.
        /// </summary>
        /// <value>The proxy client.</value>
        private IHpcrConfigurationProxyService ProxyClient
        {
            get
            {
                if (_hpcrClient != null)
                {
                    _proxyClient = _hpcrClient.Channel;
                }
                return _proxyClient;
            }
        }
        private void SetControlsByActivityData()
        {
            numericUpDownToCount.Value = _activityData.SendToEmail.NumberOfRandomRecipients;
            if (_activityData.TestType.Equals(HpcrTestType.DeliverToUserDistribution))
            {
                distribute_RadioButton.Checked = true;
            }
            else
            {
                email_RadioButton.Checked = true;
            }
            comboBox_DistributionOriginator.SelectedText = _activityData.SendToDistribution.Originator;
            comboBox_Distributions.SelectedText = _activityData.SendToDistribution.DistributionTitle;
            comboBox_EmailOriginator.SelectedText = _activityData.SendToEmail.Originator;

        }

        private void distribute_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_DistributionOriginator.Enabled = distribute_RadioButton.Checked;
            comboBox_Distributions.Enabled = distribute_RadioButton.Checked;
            if (distribute_RadioButton.Checked)
            {
                comboBox_DistributionOriginator.Focus();
            }
        }

        private void email_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownToCount.Enabled = email_RadioButton.Checked;
            comboBox_EmailOriginator.Enabled = email_RadioButton.Checked;
            if (email_RadioButton.Checked)
            {
                numericUpDownToCount.Focus();
            }
        }
        /// <summary>
        /// Retrieves the distribution originators based on the selected HPCR server
        /// </summary>
        private void LoadDistributionOriginator()
        {
            try
            {
                ConfigurationServices.SystemTrace.LogDebug("Getting configured users from HPCR server...");
                comboBox_DistributionOriginator.Items.Clear();
                if (serverComboBoxHpcr.SelectedServer != null)
                {
                    comboBox_DistributionOriginator.Text = string.Empty;

                    comboBox_DistributionOriginator.Items.Add("Loading...");

                    // place in own thread
                    ServerInfo myData = serverComboBoxHpcr.SelectedServer;
                    var myList = Task<string[]>.Factory.StartNew(() => RetrieveDistributionOriginators(myData.Address));
                    myList.ContinueWith(_ => FillCbDistributionOriginator(myList.Result.ToArray()));
                }

            }
            catch (Exception ex)
            {
                ConfigurationServices.SystemTrace.LogError("Error loading Distribution Owners", ex);
                throw ex;
            }
        }
        /// <summary>
        /// Loads the distribution originators based on the given string array
        /// </summary>
        /// <param name="owners">string[]</param>
        private void FillCbDistributionOriginator(string[] owners)
        {
            comboBox_DistributionOriginator.InvokeIfRequired(c =>
            {
                comboBox_DistributionOriginator.Items.Clear();
                comboBox_DistributionOriginator.Items.Add(Constants.CURRENT_USER);
                comboBox_DistributionOriginator.Items.AddRange(owners.ToArray());
            });
        }
        /// <summary>
        /// Task thread method used to retrieve the distribution originators for the given HPCR server address
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <returns>string[]</returns>
        private string[] RetrieveDistributionOriginators(string ipAddress)
        {
            var list = ProxyClient.GetConfiguredUsers(ipAddress).Where(x => Regex.IsMatch(x, @"^u\d+"));

            return list.ToArray();
        }
        /// <summary>
        /// Calls to load the distribution titles when the distribution originator changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void comboBox_DistributionOriginator_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDistributionTitles();
            _activityData.SendToDistribution.DistributionTitle = string.Empty;
        }
        /// <summary>
        /// Thread task used to retrieve the Distribution titles for the selected HPCR Server and distribution
        /// originator
        /// </summary>
        private void LoadDistributionTitles()
        {
            var originator = (string.IsNullOrEmpty(comboBox_DistributionOriginator.Text) == false) ? comboBox_DistributionOriginator.Text : _distributionOriginator;

            ConfigurationServices.SystemTrace.LogDebug($"Getting distributions for owner {originator}...");

            comboBox_Distributions.DataSource = null;
            comboBox_Distributions.Items.Clear();
            comboBox_Distributions.Text = string.Empty;
            comboBox_Distributions.Items.Add(string.Empty);
            comboBox_Distributions.Items.Add(Constants.RANDOM_DISTRIBUTION);
            comboBox_Distributions.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(originator))
            {
                var list = Task<string[]>.Factory.StartNew(() => RetrieveDistributions(serverComboBoxHpcr.SelectedServer.Address, originator).ToArray());
                list.ContinueWith(_ => FillCbDistributions(list.Result.ToArray()));
            }

        }
        /// <summary>
        /// Loads the combo box comboBox_Distributions with the given distributions
        /// </summary>
        /// <param name="distributions">string[]</param>
        private void FillCbDistributions(string[] distributions)
        {
            comboBox_Distributions.InvokeIfRequired(c =>
                {
                    comboBox_Distributions.Items.AddRange(distributions.ToArray());
                });
        }
        /// <summary>
        /// Used by the task thread to retrieve the distributions for the given 
        /// HPCR server address and originator 
        /// </summary>
        /// <param name="ipAddress">string</param>
        /// <param name="user">string</param>
        /// <returns></returns>
        private List<string> RetrieveDistributions(string ipAddress, string user)
        {
            List<string> list = null;
            try
            {
                list = ProxyClient.GetDistributions(ipAddress, user).Select(x => x.Title).ToList();
            }
            catch (FaultException fe)
            {
                if (fe.Message.Contains("Invalid pointer"))
                {
                    list = new List<string>();
                    string distribuition = "HPCRPublicDistribution";
                    string value;
                    if (!string.IsNullOrWhiteSpace(value = GetText(comboBox_Distributions)))
                    {
                        list.Add(value);
                    }
                    list.Add(distribuition);
                }
            }

            return list;
        }

        private void serverComboBoxHpcr_SelectionChanged(object sender, EventArgs e)
        {
            LoadDistributionOriginator();
        }
        private string GetText(ComboBox cb)
        {
            string text = string.Empty;
            cb.InvokeIfRequired(c =>
            {
                text = c.Text;
            });
            return text;
        }
    }

    /// <summary>
    /// Class for binding contained radio buttons to enum values 
    /// Equate Tag of each radio button to the corresponding enum value
    /// </summary>
    internal class RadioGroupBox : GroupBox
    {
        public event EventHandler SelectedChanged = delegate { };

        int _selected;
        public int Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                int val = 0;
                var radioButton = this.Controls.OfType<RadioButton>()
                    .FirstOrDefault(radio =>
                        radio.Tag != null
                       && int.TryParse(radio.Tag.ToString(), out val) && val == value);

                if (radioButton != null)
                {
                    radioButton.Checked = true;
                    _selected = val;
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            var radioButton = e.Control as RadioButton;
            if (radioButton != null)
                radioButton.CheckedChanged += radioButton_CheckedChanged;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;
            int val = 0;
            if (radio.Checked && radio.Tag != null
                 && int.TryParse(radio.Tag.ToString(), out val))
            {
                _selected = val;
                SelectedChanged(this, new EventArgs());
            }
        }
    }
}