using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpRoam
{
    public sealed class RoamDriverPrintManager
    {
        /// <summary>Gets or sets a value indicating whether [user signed in].</summary>
        /// <value>
        ///   <c>true</c> if [user signed in]; otherwise, <c>false</c>.</value>
        public bool UserSignedIn { get; set; } = false;

        /// <summary>Initializes a new instance of the <see cref="RoamDriverPrintManager"/> class.
        /// </summary>
        /// 
        public RoamDriverPrintManager( )
        {
        }

        /// <summary>Signs the into roam driver.</summary>
        /// <param name="credential">The credential.</param>
        /// <returns>True on success</returns>
        public bool SignIntoRoamDriver(NetworkCredential credential)
        {
            bool success = false;

            if (!UserSignedIn)
            {                
                ExecutionServices.SystemTrace.LogInfo($@"WanderRoamSignTool.exe" + $@" /si FilePath=C:\UserInfo\{credential.UserName}.json");

                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = $@"C:\Program Files (x86)\HP\HP Roam\WanderRoamSignTool.exe";
                startInfo.WorkingDirectory = Path.GetDirectoryName($@"C:\Program Files (x86)\HP\HP Roam\WanderRoamSignTool.exe");
                startInfo.Arguments = $@"/si FilePath=C:\UserInfo\{credential.UserName}.json";
                startInfo.UserName = credential.UserName;
                startInfo.Password = credential.SecurePassword;
                startInfo.LoadUserProfile = true;
                startInfo.Domain = "ETL.boi.rd.hpicorp.net";

                DateTime dtStart = DateTime.Now;

                var result = ProcessUtil.Execute(startInfo, TimeSpan.FromMinutes(1));

                TimeSpan dtEnd = DateTime.Now.Subtract(dtStart);

                ExecutionServices.SystemTrace.LogInfo($"SignIn - Exit Code: {result.ExitCode} \n" +
                    $"Error: {result.StandardError} \n" +
                    $"Output: {result.StandardOutput} \n" +
                    $"Success: {result.SuccessfulExit}" + 
                    $"Execute Time: {dtEnd.TotalSeconds.ToString()} seconds.");

                success = (result.ExitCode == 0) ? true : false;

                if (success)
                {
                    UserSignedIn = true;
                }
            }
            return success;
        }

        /// <summary>Executes the print task.</summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="documentIterator">The document iterator.</param>
        public void ExecutePrintTask(PluginExecutionData executionData, DocumentCollectionIterator documentIterator)
        {
            PrintManager printTask = new PrintManager(executionData, documentIterator);
            printTask.Execute();
        }
    }
}
