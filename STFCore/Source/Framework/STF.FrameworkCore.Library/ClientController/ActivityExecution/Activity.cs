using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Contains metadata for an activity, as well as the associated <see cref="IPluginExecutionEngine"/> instance.
    /// </summary>
    public class Activity : IDisposable
    {
        private readonly IXPathNavigable _metadata = null;
        private IPluginExecutionEngine _plugin = null;
        private ActivityState _lastExecutionState = ActivityState.None;

        private readonly List<AssetInfo> _requestedAssets = new List<AssetInfo>();
        private readonly List<AssetInfo> _availableAssets = new List<AssetInfo>();
        private readonly List<Document> _documents = new List<Document>();
        private readonly List<ServerInfo> _servers = new List<ServerInfo>();
        private readonly List<PrintQueueInfo> _requestedPrintQueues = new List<PrintQueueInfo>();
        private readonly List<PrintQueueInfo> _availablePrintQueues = new List<PrintQueueInfo>();
        private readonly List<ExternalCredentialInfo> _externalCredentials = new List<ExternalCredentialInfo>();
        private readonly PluginExecutionContext _executionContext = new PluginExecutionContext();
        private readonly PluginEnvironment _environment;
        private readonly PluginExecutionData _executionData;
        private readonly PluginRetrySettingDictionary _retrySettings;

        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the activity.
        /// </summary>
        public string ActivityType { get; private set; }

        /// <summary>
        /// Gets the execution order.
        /// </summary>
        public int ExecutionOrder { get; private set; }

        /// <summary>
        /// Gets or sets the execution count.
        /// </summary>
        public int ExecutionValue { get; private set; }

        /// <summary>
        /// Gets the execution phase.
        /// </summary>
        /// <value>The phase.</value>
        public ResourceExecutionPhase ExecutionPhase { get; private set; }

        /// <summary>
        /// Gets the execution mode.
        /// </summary>
        /// <value>The execution mode.</value>
        public ExecutionMode ExecutionMode { get; private set; }

        /// <summary>
        /// Gets the pacing.
        /// </summary>
        /// <value>The pacing.</value>
        public ActivitySpecificPacing Pacing { get; private set; }

        /// <summary>
        /// Gets the last state of the execution.
        /// </summary>
        /// <value>The last state of the execution.</value>
        public ActivityState LastExecutionState
        {
            get { return _lastExecutionState; }
        }

        /// <summary>
        /// Gets the plugin instance.
        /// </summary>
        public IPluginExecutionEngine Plugin
        {
            get { return _plugin; }
        }

        /// <summary>
        /// Occurs when the activity state has changed.
        /// </summary>
        public event EventHandler<ActivityStateEventArgs> ActivityStateChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activity" /> class.
        /// </summary>
        /// <param name="activityDetail">The activity detail.</param>
        /// <param name="officeWorker">The office worker.</param>
        /// <exception cref="System.ArgumentNullException">activityDetail</exception>
        public Activity(ResourceMetadataDetail activityDetail, OfficeWorkerDetail officeWorker)
        {
            if (activityDetail == null)
            {
                throw new ArgumentNullException("activityDetail");
            }

            Id = activityDetail.Id;
            Name = activityDetail.Name;
            ActivityType = activityDetail.MetadataType;
            ExecutionOrder = activityDetail.Plan.Order;
            ExecutionValue = activityDetail.Plan.Value;
            ExecutionPhase = activityDetail.Plan.Phase;
            ExecutionMode = activityDetail.Plan.Mode;
            Pacing = activityDetail.Plan.ActivityPacing;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(activityDetail.Data);
            _metadata = doc;

            // Populate assets for this activity
            if (GlobalDataStore.Manifest.ActivityAssets.ContainsKey(activityDetail.Id))
            {
                var assetIds = GlobalDataStore.Manifest.ActivityAssets[activityDetail.Id];
                _requestedAssets.AddRange(GlobalDataStore.Manifest.AllAssets.Where(n => assetIds.Contains(n.AssetId)));
            }

            // Populate documents for this activity
            if (GlobalDataStore.Manifest.ActivityDocuments.ContainsKey(activityDetail.Id))
            {
                var documentIds = GlobalDataStore.Manifest.ActivityDocuments[activityDetail.Id];
                _documents.AddRange(GlobalDataStore.Manifest.Documents.Where(n => documentIds.Contains(n.DocumentId)));
            }

            // Populate servers for this activity
            if (GlobalDataStore.Manifest.ActivityServers.ContainsKey(activityDetail.Id))
            {
                var serverIds = GlobalDataStore.Manifest.ActivityServers[activityDetail.Id];
                _servers.AddRange(GlobalDataStore.Manifest.Servers.Where(n => serverIds.Contains(n.ServerId)));
            }

            // Populate print queues for this activity
            if (GlobalDataStore.Manifest.ActivityPrintQueues.ContainsKey(activityDetail.Id))
            {
                _requestedPrintQueues.AddRange(GlobalDataStore.Manifest.ActivityPrintQueues[activityDetail.Id]);
            }

            // Populate retry settings for this activity (or set them to a default instance if none are configured)
            if (GlobalDataStore.Manifest.ActivityRetrySettings.ContainsKey(activityDetail.Id))
            {
                _retrySettings = GlobalDataStore.Manifest.ActivityRetrySettings[activityDetail.Id];
            }
            else
            {
                _retrySettings = new PluginRetrySettingDictionary(new List<PluginRetrySetting>());
            }

            // Populate the list of external credentials that may be needed for activity execution.
            if (GlobalDataStore.Manifest.Resources.ExternalCredentials.Count() > 0)
            {
                _externalCredentials = GlobalDataStore.Manifest.GetExternalCredentials(GlobalDataStore.Credential);
            }

            // Populate environment info
            _environment = new PluginEnvironment(
                GlobalDataStore.Manifest.PluginDefinitions.First(n => n.Name == ActivityType).PluginSettings,
                GlobalSettings.Items[Setting.Domain],
                GlobalSettings.Items[Setting.DnsDomain]
            );

            // Populate execution context
            _executionContext.SessionId = GlobalDataStore.Manifest.SessionId;
            _executionContext.UserName = GlobalDataStore.Credential.UserName;
            _executionContext.UserPassword = GlobalDataStore.Credential.Password;
            _executionContext.UserDomain = GlobalDataStore.Credential.Domain;

            // Refresh asset availability
            RefreshAssetAvailability();

            // Create execution data
            _executionData = new PluginExecutionData(
                XElement.Parse(activityDetail.Data),
                activityDetail.MetadataVersion,
                new AssetInfoCollection(_availableAssets),
                new DocumentCollection(_documents),
                new ServerInfoCollection(_servers),
                new PrintQueueInfoCollection(_availablePrintQueues),
                _environment,
                _executionContext,
                _retrySettings,
                new ExternalCredentialInfoCollection(_externalCredentials)
            );

            CreatePlugin();
        }

        private void CreatePlugin()
        {
            TraceFactory.Logger.Debug("Loading plugin instance for {0} activity '{1}'".FormatWith(ActivityType, Name));

            try
            {
                _plugin = PluginFactory.GetPlugin(ActivityType).Create<IPluginExecutionEngine>();
            }
            catch (PluginLoadException ex)
            {
                TraceFactory.Logger.Error($"Unable to load plugin for Activity Type: {ActivityType}", ex);
                throw;
            }
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <exception cref="WorkerHaltedException"></exception>
        public void Execute()
        {
            // Process the activity, which will use the defined plugin, and once processed
            // then tell the Activity Dispatcher to release the next activity.
            Thread.CurrentThread.SetName(this.ActivityType);

            // Save the activity and transaction ID for global access
            GlobalDataStore.CurrentActivity = this.Id;
            GlobalDataStore.CurrentTransaction = SequentialGuid.NewGuid();
            _executionContext.ActivityExecutionId = GlobalDataStore.CurrentTransaction;

            // Prep logging for the activity
            var dataLogger = new ActivityExecutionLogger
                (
                    GlobalDataStore.ResourceInstanceId,
                    GlobalDataStore.Credential.UserName,
                    GlobalDataStore.CurrentTransaction,
                    this.Id,
                    GlobalDataStore.Manifest.SessionId,
                    this.Name,
                    this.ActivityType
                );
            dataLogger.Status = ActivityState.Started.ToString();

            // Set the initial status
            UpdateActivityStatus(ActivityState.Started);
            ExecutionServices.DataLogger.AsInternal().SubmitAsync(dataLogger);

            // Refresh asset availability
            RefreshAssetAvailability();

            // Attempt to process the activity, and catch any exceptions that are returned
            // If there is no failure, then update the event status to Completed
            TraceFactory.Logger.Debug($"Starting ProcessActivity for activity execution ID {_executionContext.ActivityExecutionId.ToString()}.");

            PluginExecutionResult result;
            try
            {
                result = ProcessActivity();
            }
            catch (WorkerHaltedException ex)
            {
                _lastExecutionState = ActivityState.Failed;
                UpdateActivityStatus(_lastExecutionState, ex.Message);

                TraceFactory.Logger.Debug($"Ending ProcessActivity for FAILED activity execution ID {_executionContext.ActivityExecutionId.ToString()}.");

                dataLogger.UpdateValues(_lastExecutionState.ToString(), ex.Message);
                ExecutionServices.DataLogger.AsInternal().UpdateAsync(dataLogger);
                throw;
            }

            switch (result.Result)
            {
                case PluginResult.Passed:
                    _lastExecutionState = ActivityState.Completed;
                    break;

                case PluginResult.Failed:
                    _lastExecutionState = ActivityState.Failed;
                    break;

                case PluginResult.Skipped:
                    _lastExecutionState = ActivityState.Skipped;
                    break;

                case PluginResult.Error:
                    _lastExecutionState = ActivityState.Error;
                    break;
            }
            UpdateActivityStatus(_lastExecutionState, result.Message);

            TraceFactory.Logger.Debug($"Ending ProcessActivity for activity execution ID {_executionContext.ActivityExecutionId.ToString()}.");

            dataLogger.UpdateResultValues(result);
            ExecutionServices.DataLogger.AsInternal().UpdateAsync(dataLogger);
        }

        private void RefreshAssetAvailability()
        {
            _availableAssets.Clear();
            _availableAssets.AddRange(_requestedAssets.Where(n => GlobalDataStore.Manifest.Assets.IsAvailable(n.AssetId)));
            _availablePrintQueues.Clear();
            _availablePrintQueues.AddRange(_requestedPrintQueues.Where(n => string.IsNullOrEmpty(n.AssociatedAssetId) || GlobalDataStore.Manifest.Assets.IsAvailable(n.AssociatedAssetId)));
        }

        private void UpdateActivityStatus(ActivityState state, string error = null)
        {
            if (ActivityStateChanged != null)
            {
                ActivityStateChanged(this, new ActivityStateEventArgs(this.Id, this.Name, state, error));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions thrown from plugins should be logged as errors and should not crash the worker.")]
        private PluginExecutionResult ProcessActivity()
        {
            PluginRetryManager retryManager = new PluginRetryManager(_executionData, TraceFactory.Logger.Debug);
            PluginExecutionResult finalResult = retryManager.Run(() =>
                {
                    PluginExecutionResult result = null;
                    try
                    {
                        result = _plugin.Execute(_executionData);
                    }
                    catch (WorkerHaltedException)
                    {
                        // This exception must be explicitly re-thrown so it isn't eaten by the general catch
                        throw;
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();

                        // Log the whole exception, including stack trace
                        TraceFactory.Logger.Error("Unhandled exception in plugin execution.", ex);
                        result = new PluginExecutionResult(PluginResult.Error, ex.ToString(), "Unhandled exception.");
                    }
                    catch (Exception ex)
                    {
                        // Log the whole exception, including stack trace
                        TraceFactory.Logger.Error("Unhandled exception in plugin execution.", ex);
                        result = new PluginExecutionResult(PluginResult.Error, ex.ToString(), "Unhandled exception.");
                    }
                    return result;
                });

            if (finalResult.RetryStatus == PluginRetryStatus.Halt)
            {
                throw new WorkerHaltedException("Retry limit reached");
            }
            return finalResult;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_plugin is IDisposable)
                {
                    ((IDisposable)_plugin).Dispose();
                }
            }
        }

        #endregion IDisposable Members

    }
}
