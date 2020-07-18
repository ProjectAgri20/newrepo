using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.mPrint
{
    /// <summary>
    /// Used to execute the activity of the mPrint plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class mPrintExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData = null;
        /// <summary>
        /// Initializes a new instance of the mPrintExecutionControl class.
        /// </summary>
        public mPrintExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the mPrint activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            //var retryManager = new PluginRetryManager(executionData, UpdateStatus);
            _executionData = executionData;
            mPrintActivityData activityData = executionData.GetMetadata<mPrintActivityData>();

            //UpdateStatus("Starting task engine");
            //var engine = new ActivityTaskEngine();
            //engine.StatusUpdateMessageTarget = UpdateStatus;

            return SendIPPCommandTomPrint(activityData.serv, activityData.queueIndex);


        }
        /// <summary>
        /// Sends IPP command to mPrint, If a send fails, it all fails
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>

        private PluginExecutionResult SendIPPCommandTomPrint(ServerInfo server, string queueIndex)
        {
            Document document = null;
            PluginExecutionResult testResult = new PluginExecutionResult(PluginResult.Passed);

            int docNumber = _executionData.Documents.Count;
            string filename = "";

            try
            {
                for (int i = 0; i < docNumber; i++)
                {
                    document = _executionData.Documents.GetRandom();
                    if (document != null)
                    {
                        FileInfo fInfo = ExecutionServices.FileRepository.GetFile(document);
                        filename = fInfo.FullName.Contains(" ") ? string.Format(@"""{0}""", fInfo.FullName) : fInfo.FullName;

                        UriBuilder urijob = new UriBuilder();
                        urijob.Scheme = "https";
                        urijob.Host = server.HostName + @".etl.boi.rd.hpicorp.net";
                        urijob.Path = "ipp/print/" + queueIndex; // PRINT RESOURCE PATH q path is : ipp/print/#
                        urijob.Port = 631;   //Verify port
                        urijob.UserName = _executionData.Credential.UserName;
                        urijob.Password = _executionData.Credential.Password;

                        string final_command = string.Format(@"-4 -v -f {0} {1} .\ipptool\print-job.test", filename, urijob);
                        ExecutionServices.SystemTrace.LogDebug(urijob.ToString());
                        ExecutionServices.SystemTrace.LogDebug(final_command);
                        testResult = SendJob(final_command);
                        if (testResult.Result == PluginResult.Error)
                        {
                            break;
                        }
                    }
                }
                return testResult;
            }
            catch (Exception e)
            {
                return new PluginExecutionResult(PluginResult.Error, e);
            }
        }

        private PluginExecutionResult SendJob(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            startInfo.WorkingDirectory = @"C:\Program Files (x86)\ipptool";
            startInfo.FileName = "ipptool.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = command;
            int successful = 0;
            try
            {
                using (Process ippProcess = Process.Start(startInfo))
                {
                    ippProcess.WaitForExit();
                    successful = ippProcess.ExitCode;
                }
            }
            catch (Exception e)
            {
                return new PluginExecutionResult(PluginResult.Error, "An issue occured running IPPTool, please make sure it it installed/setup correctly.");
                throw e;
            }
            if (successful == 0)
            {
                return new PluginExecutionResult(PluginResult.Passed);
            }
            else
            {
                return new PluginExecutionResult(PluginResult.Error, "IPPTool failed to run successfully");
            }

        }
    }
}
