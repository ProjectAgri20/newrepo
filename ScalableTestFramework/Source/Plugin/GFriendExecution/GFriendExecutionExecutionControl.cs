using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using HP.GFriend.Core;
using HP.GFriend.Core.Execution;
using HP.GFriend.Event;
using HP.GFriend.Keywords;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    [ToolboxItem(false)]
    public partial class GFriendExecutionExecutionControl : UserControl, IPluginExecutionEngine
    {
        private OutputWriter _consoleWriter;
        private static bool _isGFPrepared = false;
        protected DeviceWorkflowLogger _workflowLogger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GFriendExecutionExecutionControl" /> class.
        /// </summary>
        public GFriendExecutionExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {

            UpdateStatus("Starting activity.");
            UpdateLabel(sessionId_value_label, executionData.SessionId);
            
            _workflowLogger = new DeviceWorkflowLogger(executionData);
            GFriendExecutionActivityData data = executionData.GetMetadata<GFriendExecutionActivityData>();

            UpdateStatus("Prepare Files.");
            string scriptPath;
            if (executionData.Environment.PluginSettings.ContainsKey("GFScriptPath"))
            {
                scriptPath = executionData.Environment.PluginSettings["GFScriptPath"];
            }
            else
            {
                scriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "scripts", executionData.SessionId);
            }
            UpdateStatus($"GFriend files will be saved to {scriptPath}");
            
            GFriendPreparationManager.PrepareFiles(data.GFriendFiles, scriptPath);

            string scriptToRun = data.GFriendFiles.Where(s => s.FileType.Equals(GFFileTypes.GFScript)).FirstOrDefault()?.FileName ?? string.Empty;

            if(string.IsNullOrEmpty(scriptToRun))
            {
                UpdateStatus("GF Script file does not exist. Please check activity data");
                return new PluginExecutionResult(PluginResult.Failed, "Invalid activity data (No Script File)");
            }

            scriptToRun = Path.Combine(scriptPath, scriptToRun);
            UpdateStatus($"GFriend test script {scriptToRun} will be exeucted.");

            // Run GFriend
            _consoleWriter = new OutputWriter(output_RichTextBox);
            Console.SetOut(_consoleWriter);

            IDeviceInfo deviceInfo;
            PluginExecutionResult executionResult = new PluginExecutionResult(PluginResult.Passed);
            if (executionData.Assets.Count > 0)
            {
                var devices = executionData.Assets.OfType<IDeviceInfo>();
                var assetTokens = devices.Select(n => new AssetLockToken(n, data.LockTimeouts));
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    UpdateLabel(dut_value_label, deviceInfo.AssetId);
                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(executionData, deviceInfo));
                    executionResult = RunGFriendScript(executionData, scriptToRun, deviceInfo);
                });
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            else
            {
                executionResult = RunGFriendScript(executionData, scriptToRun, null);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            UpdateStatus("Finished activity.");
            UpdateStatus($"Result = {executionResult.Result}");

            return executionResult;
        }

        /// <summary>
        /// Run GFriend Script
        /// </summary>
        /// <param name="executionData">Execution data</param>
        /// <param name="gfScriptPath">Path of GFriend Script to run</param>
        /// <param name="device">Device info</param>
        /// <returns></returns>
        private PluginExecutionResult RunGFriendScript(PluginExecutionData executionData, string gfScriptPath, IDeviceInfo device)
        {
            _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityBegin);
            string gfLibraryPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "GFriend", "libs");
            string outputPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GFriend_Output", executionData.SessionId, executionData.ActivityExecutionId.ToString());

            // Prepare GF Native Libraries and Add event handler
            // Developer Note : 
            // GFriend dynamically load native libraries (GFK.*.dll files) when execution.
            // For exeucting GFriend script path of library files should be given.
            // In this step plugin unzip the embeded archive which contains GFriend native library to Plugin\GFriend\libs
            if (!_isGFPrepared)
            {
                UpdateStatus("Preparing GF libraries");
                GFEvent.OnGFEvent += GFEvent_OnGFEvent;
                GFriendPreparationManager.PrepareLibrary(gfLibraryPath);
                LibraryUtils.LibraryPath = gfLibraryPath;

                _isGFPrepared = true;
            }
            TestDataManager testDataManager = new TestDataManager();
            List<DeviceUnderTest> gfDevices = new List<DeviceUnderTest>();
            if(device != null)
            {
                DeviceUnderTest dut = new DeviceUnderTest()
                {
                    DeviceId = device.AssetId,
                    DeviceAddress = device.Address,
                    LanDebugAddress = device.Address2,
                    AdminPassword = device.AdminPassword
                };
                gfDevices.Add(dut);
            }
            UpdateStatus("GFrined Initialization");
            Runner.InitGFRunner(gfDevices, outputPath);
            UpdateStatus("Running GFriend Script");
            Runner.Run(gfScriptPath, out testDataManager, null, gfLibraryPath);

            // Check Result with output.xml
            string outputXMLPath = Path.Combine(outputPath, "output.xml");
            XmlDocument outputXML = new XmlDocument();
            outputXML.Load(outputXMLPath);
            XmlNode testSuite = outputXML.SelectSingleNode("//TestSuite");
            int passCount = testSuite.SelectNodes("//TestCase/Result[contains(text(),'PASS')]")?.Count ?? 0;
            int failCount = testSuite.SelectNodes("//TestCase/Result[contains(text(),'FAIL')]")?.Count ?? 0;
            int errorCount = testSuite.SelectNodes("//TestCase/Result[contains(text(),'ERROR')]")?.Count ?? 0;

            _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityEnd);
            if (errorCount > 0)
            {
                return new PluginExecutionResult(PluginResult.Error, $"GFriend Execution Ended with Pass:{passCount}, Fail:{failCount}, Error:{errorCount}");
            }

            if (failCount > 0)
            {
                return new PluginExecutionResult(PluginResult.Failed, $"GFriend Execution Ended with Pass:{passCount}, Fail:{failCount}, Error:{errorCount}");
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Event Handler for GFEvent
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">GFEventArgs</param>
        private void GFEvent_OnGFEvent(object sender, GFEventArgs e)
        {
            switch (e.EventName.ToString())
            {
                case "StartTestSuite":
                    _workflowLogger.RecordEvent(DeviceWorkflowMarker.GFriendExeuctionStart);
                    break;
                case "EndTestSuite":
                    _workflowLogger.RecordEvent(DeviceWorkflowMarker.GFriendExeuctionEnd);
                    break;
            }
            

        }

        private void UpdateStatus(string message)
        {
            output_RichTextBox.InvokeIfRequired(n =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status = " + message);
                n.AppendText($"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {message}\n");
                n.Select(n.Text.Length, 0);
                n.ScrollToCaret();
            });
        }

        private void UpdateLabel(Label label, string value)
        {
            label.InvokeIfRequired(n =>
            {
                n.Text = value;
            });
        }
    }
}
