using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.TopCat;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;

namespace HP.ScalableTest.Plugin.TopCat
{
    [ToolboxItem(false)]
    public partial class TopCatExecutionControl : UserControl, IPluginExecutionEngine
    {
        private bool _setupDone = false;
        private TopCatActivityData _activityData = null;

        public TopCatExecutionControl()
        {
            InitializeComponent();
        }

        public TopCatExecutionControl(TopCatActivityData activityData)
        {
            InitializeComponent();
            _activityData = activityData;
        }

        #region IPluginSetup

        public void Setup(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<TopCatActivityData>(new[] { new TopCatActivityDataConverter() });

            if (_activityData.Script == null)
            {
                ExecutionServices.SystemTrace.LogWarn("No scripts were provided, returning");
                return;
            }

            if (!string.IsNullOrEmpty(_activityData.SetupFileName))
            {
                ProcessUtil.Execute(CopyInstaller(_activityData.SetupFileName, _activityData.CopyDirectory), string.Empty);
            }

            TopCatExecutionController topcatExecution = new TopCatExecutionController(_activityData.Script);
            topcatExecution.InstallPrerequisites(executionData.Environment.PluginSettings["TopCatSetup"]);

            if (_activityData.RunOnce)
            {
                topcatExecution.ExecuteTopCatTest();
            }
        }

        private static string CopyInstaller(string fileName, bool copyDirectory)
        {
            string localFilePath = string.Empty;
            if (copyDirectory)
            {
                localFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fileName), Path.GetFileName(fileName));
                FileSystem.CopyDirectory(Path.GetDirectoryName(fileName), localFilePath);
            }
            else
            {
                localFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
                if (!File.Exists(localFilePath))
                {
                    File.Copy(fileName, localFilePath);
                }
            }

            return localFilePath;
        }

        #endregion IPluginSetup

        #region IPluginExecution

        public void ReadResult(string resultFileName)
        {
            if (!string.IsNullOrEmpty(resultFileName))
            {
                XDocument resultDoc = XDocument.Load(resultFileName);
                var successTests = resultDoc.Descendants("SuccessfulTests").First().Descendants("test");
                foreach (var successTest in successTests)
                {
                    int row = results_dataGridView.Rows.Add();
                    results_dataGridView.Rows[row].Cells[0].Value = successTest.Element("Name").Value;
                    results_dataGridView.Rows[row].Cells[1].Value = "Passed";

                }

                var failedTests = resultDoc.Descendants("FailedTests").First().Descendants("test");
                foreach (var failedTest in failedTests)
                {
                    int row = results_dataGridView.Rows.Add();
                    results_dataGridView.Rows[row].Cells[0].Value = failedTest.Element("Name").Value;
                    results_dataGridView.Rows[row].Cells[1].Value = "Failed";
                    throw new Exception($"Test Name: {failedTest.Element("Name").Value} failed with Errors");
                }
            }
            else
            {
                throw new Exception("Unable to find the result file, please check your script again!");
            }
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (!_setupDone)
            {
                Setup(executionData);
                _setupDone = true;
            }

            try
            {
                _activityData = executionData.GetMetadata<TopCatActivityData>(new[] { new TopCatActivityDataConverter() });
                //since we cannot have multiple topcat instances running on the machine, we will use lock
                var action = new Action(() =>
                {
                    TopCatExecutionController topcatExecution = new TopCatExecutionController(_activityData.Script);
                    if (!_activityData.RunOnce)
                    {
                        topcatExecution.ExecuteTopCatTest();
                    }
                    ReadResult(topcatExecution.GetResultFilePath(executionData.Environment.PluginSettings["DomainAdminUserName"]));
                });

                ExecutionServices.CriticalSection.Run(new Framework.Synchronization.LocalLockToken($"TopCat Execution:{_activityData.Script.ScriptName}", new TimeSpan(0, 2, 0), new TimeSpan(0, 5, 0)), action);

                return new PluginExecutionResult(PluginResult.Passed);

            }

            catch (Exception genericException)
            {
                ExecutionServices.SystemTrace.LogDebug(genericException.Message);
                return new PluginExecutionResult(PluginResult.Failed, "Activity Failed :", genericException.Message);
            }

        }

        #endregion IPluginExecution
    }
}