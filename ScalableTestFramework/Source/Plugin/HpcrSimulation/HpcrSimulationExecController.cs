using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.Hpcr;

namespace HP.ScalableTest.Plugin.HpcrSimulation
{
    /// <summary>
    /// Execution controller for the HPCR Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpcrSimulationExecController : UserControl, IPluginExecutionEngine
    {
        private HpcrSimulationData _activityData = null;
        private PluginExecutionData _pluginExecutionData = null;

        private HpcrExecutionProxyClient _hpcrClient = null;
        private IHpcrExecutionProxyService _proxyClient = null;

        private string _runtimeOriginator;
        private Collection<string> _runtimeRecipients;
        private Collection<string> _runtimeDocumentPaths;

        private List<string> _output = new List<string>();

        private delegate void TypeTextHandler(TextBox field, string data, int delay);
        private delegate void ClearFormHandler();

        public HpcrSimulationExecController()
        {
            InitializeComponent();
        }
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _pluginExecutionData = executionData;
            _activityData = executionData.GetMetadata<HpcrSimulationData>();

            _hpcrClient = new HpcrExecutionProxyClient(executionData.Environment.PluginSettings["HpcrProxy"]);

            var retryManager = new PluginRetryManager(executionData, UpdateStatus);
            PluginExecutionResult result = retryManager.Run(PerformActivity);

            return result;
        }

        private PluginExecutionResult PerformActivity()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            try
            {
                if (_activityData != null)
                {
                    switch (_activityData.TestType)
                    {
                        case HpcrTestType.DeliverToUserDistribution:
                            result = DeliverToDistribution();
                            break;
                        case HpcrTestType.DeliverToRandomEmails:
                            result = DeliverToEmail();
                            break;
                        case HpcrTestType.ShowUserDistributions:        // not implemented
                            result = ShowUserDistributions();
                            break;
                        case HpcrTestType.ShowUserGroupPolicy:          // not implemented
                            result = ShowGroupPolicies();
                            break;
                        case HpcrTestType.ShowUserGroupMembership:      // not implemented
                            result = ShowGroupMemberships();
                            break;
                        default:
                            result = new PluginExecutionResult(PluginResult.Failed, "Unknown HPCR Simulation type.");
                            break;
                    }
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, "Plugin activity data may not be null.");
                }
            }
            catch (Exception ex)
            {
                result = new PluginExecutionResult(PluginResult.Error, ex);
            }
            return result;
        }


        /// <summary>
        /// Deliver to email.
        /// </summary>
        private PluginExecutionResult DeliverToEmail()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            ClearForm();
            UpdateStatus("Deliver to Email");

            int count = _activityData.SendToEmail.NumberOfRandomRecipients;
            var toList = ExecutionServices.SessionRuntime.AsInternal().GetOfficeWorkerEmailAddresses(count);

            _runtimeRecipients = new Collection<string>(toList.Select(n => n.Address).ToList());
            _runtimeOriginator = _activityData.SendToEmail.Originator;

            SetRuntimeData();
            PopulateExecutionControls();

            // Deliver to HPCR Proxy
            foreach (var doc in _runtimeDocumentPaths)
            {
                foreach (var recipient in _runtimeRecipients)
                {
                    // Lock the document pool so that we don't get conflicts with other users.
                    ExecutionServices.CriticalSection.Run(new LocalLockToken("LocalDocument", new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), () =>
                    {
                        ProxyClient.DeliverToEmailByDocument(_pluginExecutionData.Servers.First().Address, doc, _runtimeOriginator, recipient);
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Deliver to distribution.
        /// </summary>
        private PluginExecutionResult DeliverToDistribution()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            UpdateStatus("Deliver to Distribution");

            _runtimeOriginator = _activityData.SendToDistribution.Originator;
            _runtimeRecipients = new Collection<string>() { _activityData.SendToDistribution.DistributionTitle };

            SetRuntimeData();
            PopulateExecutionControls();

            foreach (var doc in _runtimeDocumentPaths)
            {
                foreach (var recipient in _runtimeRecipients)
                {
                    // Lock the document pool so that we don't get conflicts with other users.
                    ExecutionServices.CriticalSection.Run(new LocalLockToken("LocalDocument", new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), () =>
                    {
                        ProxyClient.DeliverToDistribution(_pluginExecutionData.Servers.First().Address, doc, _runtimeOriginator, recipient);
                    });
                }
            }


            return result;
        }
        /// <summary>
        /// Sets the HpcrActivityData runtime information.
        /// </summary>
        private void SetRuntimeData()
        {
            // Massage originator email with runtime information (if necessary)
            _runtimeOriginator = ReplaceCurrentUser(_runtimeOriginator);

            // Massage recipient with runtime information (if necessary)
            if (_runtimeRecipients.Any(x => x.Equals(Constants.RANDOM_DISTRIBUTION)))
            {
                var distributionNames = ProxyClient.GetDistributions(_pluginExecutionData.Servers.First().Address, _runtimeOriginator).Select(x => x.Title);
                var revisedRecipients = new Collection<string>();
                foreach (var recipient in _runtimeRecipients)
                {
                    revisedRecipients.Add(ReplaceRandomDistribution(recipient, distributionNames));
                }
                _runtimeRecipients = revisedRecipients;
            }

            UpdateStatus("Fetching documents...");

            //Cap at 2 documents max (chosen at random if more than 2 exist).
            if (_pluginExecutionData.Documents.Count > 2)
            {
                DocumentCollectionIterator iterator = new DocumentCollectionIterator(CollectionSelectorMode.ShuffledRoundRobin);

                string path1 = ExecutionServices.FileRepository.AsInternal().GetDocumentSharePath(iterator.GetNext(_pluginExecutionData.Documents));
                string path2 = ExecutionServices.FileRepository.AsInternal().GetDocumentSharePath(iterator.GetNext(_pluginExecutionData.Documents));

                _runtimeDocumentPaths.Add(path1);
                _runtimeDocumentPaths.Add(path2);
            }
            else
            {
                string path = ExecutionServices.FileRepository.AsInternal().GetDocumentSharePath(_pluginExecutionData.Documents.First());
                _runtimeDocumentPaths = new Collection<string>();
                _runtimeDocumentPaths.Add(path);
            }
        }

        /// <summary>
        /// Replaces the current user tag with the actual current user
        /// </summary>
        /// <param name="test">The test.</param>
        /// <returns>System.String.</returns>
        private string ReplaceCurrentUser(string test)
        {
            string result = test;
            if (test.Equals(Constants.CURRENT_USER, StringComparison.InvariantCultureIgnoreCase))
            {
                result = new EmailBuilder(_pluginExecutionData.Credential.UserName, _pluginExecutionData).ToString();
                UpdateStatus($"Converting {Constants.CURRENT_USER} to {result}");
            }
            return result;
        }
        /// <summary>
        /// Replaces the random distribution tag with a randomly selected distribution.
        /// </summary>
        /// <param name="test">The test.</param>
        /// <param name="distributionNames">The distribution names.</param>
        /// <returns>System.String.</returns>
        private string ReplaceRandomDistribution(string test, IEnumerable<string> distributionNames)
        {
            Random random = new Random();
            string tag = Constants.RANDOM_DISTRIBUTION;
            string result = test;
            if (test.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
            {
                var tempList = distributionNames.ToList();
                result = tempList[random.Next(tempList.Count)];
                UpdateStatus($"Converting {tag} to {result}");
            }
            return result;
        }
        /// <summary>
        /// Shows the user distributions.
        /// </summary>
        private PluginExecutionResult ShowUserDistributions()
        {
            // Not used currently
            UpdateStatus("Show User Distributions");
            return new PluginExecutionResult(PluginResult.Error, "Show User Distribution has not been implemented.");
        }

        /// <summary>
        /// Shows the group policies.
        /// </summary>
        private PluginExecutionResult ShowGroupPolicies()
        {
            // Not used currently
            UpdateStatus("Show User Group Policy");
            return new PluginExecutionResult(PluginResult.Error, "Show group policies has not been implemented.");
        }

        /// <summary>
        /// Shows the group memberships.
        /// </summary>
        private PluginExecutionResult ShowGroupMemberships()
        {
            // Not used currently
            UpdateStatus("Show User Group Memberships");
            return new PluginExecutionResult(PluginResult.Error, "Show group memberships has not been implemented.");
        }

        /// <summary>
        /// Eye candy for typing text into a TextBox field.
        /// </summary>
        /// <param name="field">Field to type into</param>
        /// <param name="data">Text to type</param>
        /// <param name="delay">How long to delay between each character being typed.</param>
        private void TypeText(TextBox field, string data, int delay)
        {
            if (field.InvokeRequired)
            {
                field.Invoke(new TypeTextHandler(TypeText), field, data, delay);
                return;
            }

            field.Clear();
            foreach (char c in data.ToCharArray())
            {
                field.AppendText(c.ToString());
                Application.DoEvents();
                Thread.Sleep(delay);
            }
            ExecutionServices.SystemTrace.LogNotice($"textbox={field.Name}, text={data}");
        }
        /// <summary>
        /// Populates the execution controls.
        /// </summary>
        private void PopulateExecutionControls()
        {
            ClearForm();

            UpdateStatus("Populating fields");

            var deliveryType = (_activityData.TestType == HpcrTestType.DeliverToRandomEmails ? "To:" : "Distribution:");
            TypeText(textBox_RecipientType, deliveryType, 0);

            TypeText(textBox_Originator, _runtimeOriginator, 10);
            TypeText(textBox_Recipients, string.Join(";", _runtimeRecipients), 10);
            TypeText(textBox_Documents, string.Join(Environment.NewLine, _runtimeDocumentPaths), 10);
        }

        /// <summary>
        /// Clears the controls on the form
        /// </summary>
        private void ClearForm()
        {
            UpdateStatus("Clearing form");
            if (this.InvokeRequired)
            {
                this.Invoke(new ClearFormHandler(ClearForm));
                return;
            }

            textBox_Originator.Text = string.Empty;
            textBox_Recipients.Text = string.Empty;
            textBox_Documents.Text = string.Empty;
        }

        /// <summary>
        /// Logs messages to specified output.
        /// </summary>
        /// <param name="message">The message.</param>
        private void UpdateStatus(string message)
        {
            ExecutionServices.SystemTrace.LogDebug(message);
            _output.Add(message);
            RefreshLogDisplay();
        }
        private void RefreshLogDisplay()
        {
            string output = string.Empty;
            foreach (string msg in _output)
            {
                output += msg + "\n\r";
            }
            TypeText(output_TextBox, output, 0);
        }
        /// <summary>
        /// Gets the proxy client.
        /// </summary>
        /// <value>The proxy client.</value>
        private IHpcrExecutionProxyService ProxyClient
        {
            get
            {
                if (_hpcrClient != null)
                {
                    // Get the client with a timeout of 3 minutes.
                    ((IClientChannel)_hpcrClient.Channel).OperationTimeout = TimeSpan.FromMinutes(3);
                    _proxyClient = _hpcrClient.Channel;

                }
                return _proxyClient;
            }
        }
    }
}
