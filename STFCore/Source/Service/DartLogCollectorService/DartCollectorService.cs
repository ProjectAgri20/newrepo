using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;

namespace DartLogCollectorService
{
    /// <summary>
    /// Service that collect Dart Log Data
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class DartCollectorService : IDartLogCollectorService
    {
        /// <summary>
        /// Collects Dart Logs by calling the jta2 launcher and placing them in the location defined by the framework server setting.
        /// </summary>
        /// <remarks> Be sure that Java SDK, SNMPGet and JTA3 @ C:\JTA are located on the server.</remarks>
        /// <param name="assetID"></param>
        /// <param name="sessionID"></param>
        /// <param name="email"></param>
        public void CollectLog(string assetID, string sessionID, string email)
        {
            string printerIP = string.Empty;
            string dartIP = string.Empty;
            bool collectDart = false;
            string script = string.Empty;
            string modelName = string.Empty;
            try
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var temp = context.Assets.OfType<Printer>().FirstOrDefault(n => n.AssetId == assetID);
                    printerIP = temp.Address1;
                    modelName = temp.Product.Split(' ').First();

                    if (string.IsNullOrEmpty(printerIP))
                    {
                        TraceFactory.Logger.Debug($@"Printer IP Not found for asset: {assetID}");
                        return;
                    }

                    var dartBoard = context.DartBoards.FirstOrDefault(n => n.PrinterId == assetID);
                    if (dartBoard == null)
                    {
                        TraceFactory.Logger.Debug($@"No DartBoard found for: {assetID}");
                        collectDart = false;
                    }
                    else
                    {
                        dartIP = dartBoard.Address;
                        collectDart = true;
                    }

                    string serverType = ServerType.Dart.ToString();
                    FrameworkServer server = context.FrameworkServers.FirstOrDefault(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active);
                    string dartLocation = server.ServerSettings.First(n => n.Name == "DartServiceLocation").Value;
                    string jtaLocation = GlobalSettings.Items["JTALogCollectorDirectory"];

                    string inAddition = $@"\{sessionID}";
                    DirectoryInfo di = Directory.CreateDirectory(dartLocation + inAddition);

                    bool pingResult = false;
                    using (var ping = new Ping())
                    {
                        var response = ping.Send(printerIP, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
                        //DV: sometimes the device, being lazy as it is, will be in sleep mode and doesn't respond to the ping command
                        //so we poke it few more times so that it wakes up from its slumber and responds.
                        int retries = 0;
                        while (response.Status != IPStatus.Success && retries < 4)
                        {
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                            response = ping.Send(printerIP, (int)TimeSpan.FromSeconds(15).TotalMilliseconds);
                            retries++;
                        }

                        if (response.Status != IPStatus.Success)
                        {
                            TraceFactory.Logger.Debug($"Ping Unsuccessful: {printerIP.ToString()}:{response.Status}");
                            pingResult = false;
                        }
                        else
                        {
                            pingResult = true;
                        }
                    }

                    //Extra check that the device is in dev mode for JTA.
                    if (pingResult)
                    {
                        pingResult = CheckProd(temp.Address1, temp.Password);
                    }

                    //If JTA checks are not a success, use dart.exe, else use JTA --Don't use JTA for dart logs. It may fail before dart collection
                    if (pingResult)
                    {
                        if (collectDart)
                        {

                            bool result = false;
                            Task.Factory.StartNew(() =>
                            {
                                //Get DARTLOCATION FROM Global Settings

                                string timestamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                                ProcessStartInfo startInfo = new ProcessStartInfo();
                                startInfo.CreateNoWindow = true;
                                startInfo.UseShellExecute = true;
                                startInfo.FileName = @"dart.exe";
                                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                startInfo.Arguments = $@"{dartIP} dl {dartLocation + inAddition}\{assetID}_{timestamp}.bin";

                                try
                                {
                                    using (Process exeProcess = Process.Start(startInfo))
                                    {
                                        exeProcess.WaitForExit();
                                    }
                                    result = true;
                                    TraceFactory.Logger.Debug($@"Capture of {assetID} logs succeeded");
                                    SendEmail(sessionID, assetID, result, email);

                                }
                                catch (Exception a)
                                {
                                    TraceFactory.Logger.Error("Dart log collection failed.");
                                    TraceFactory.Logger.Error(a);
                                }
                            });

                            //TraceFactory.Logger.Info($@"Attempting to collect logs of {assetID}  for Session ID: {sessionID}.");
                            //script = $@"cd {jtaLocation} | java jta3.Launcher -p {modelName} -x {printerIP} -r {dartLocation + inAddition} -s false";
                        }

                        TraceFactory.Logger.Info($@"Attempting to collect logs of {assetID}  for Session ID: {sessionID}.");
                        script = $@"cd {jtaLocation} | java jta3.Launcher -p {modelName} -x {printerIP} -r {dartLocation + inAddition} -s false";
                        

                        Task.Factory.StartNew(() =>
                        {
                            bool result = false;
                            try
                            {
                                Runspace runspace = RunspaceFactory.CreateRunspace();
                                runspace.Open();
                                TraceFactory.Logger.Debug(Environment.CurrentDirectory);
                                Pipeline pipeline = runspace.CreatePipeline();
                                pipeline.Commands.AddScript(string.Format("$user = \"{0}\"", "jawa"));
                                pipeline.Commands.AddScript(script);

                                Collection<PSObject> results = pipeline.Invoke();
                                bool hadErrors = pipeline.HadErrors;
                                runspace.Close();


                                result = !hadErrors;
                                TraceFactory.Logger.Debug($@"Capture of {assetID} logs succeeded, Error during collection? {hadErrors}");
                                if (hadErrors)
                                {
                                    StringBuilder builder = new StringBuilder();
                                    foreach (PSObject obj in results)
                                    {
                                        builder.AppendLine(obj.ToString());
                                    }
                                    TraceFactory.Logger.Debug($@"{builder}");

                                }

                            }
                            catch (Exception a)
                            {
                                result = false;
                                TraceFactory.Logger.Debug("JTA log collection failed.");
                                TraceFactory.Logger.Debug(a);
                            }
                            SendEmail(sessionID, assetID, result, email);
                        });
                    }
                    else
                    {
                        if (collectDart)
                        {
                            bool result = false;
                            Task.Factory.StartNew(() =>
                            {
                                //Get DARTLOCATION FROM Global Settings

                                string timestamp = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                                ProcessStartInfo startInfo = new ProcessStartInfo();
                                startInfo.CreateNoWindow = true;
                                startInfo.UseShellExecute = true;
                                startInfo.FileName = @"dart.exe";
                                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                startInfo.Arguments = $@"{dartIP} dl {dartLocation + inAddition}\{assetID}_{timestamp}.bin";

                                try
                                {
                                    using (Process exeProcess = Process.Start(startInfo))
                                    {
                                        exeProcess.WaitForExit();
                                    }
                                    result = true;
                                    TraceFactory.Logger.Debug($@"Capture of {assetID} logs succeeded");
                                    SendEmail(sessionID, assetID, result, email);

                                }
                                catch (Exception a)
                                {
                                    TraceFactory.Logger.Error("Dart log collection failed.");
                                    TraceFactory.Logger.Error(a);
                                }
                            }
);
                        }
                        
                    }


                        

                        

             


                    }



            }
            catch (Exception e)
            {
                TraceFactory.Logger.Error(e);
            }

        }

        /// <summary>
        /// Flushes Dart Log at specified IP address.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Flush(string ip)
        {
            bool result = false;

            string dartLocation = $@"dart.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.FileName = dartLocation;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $@"{ip} flush";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                }
                result = true;
                TraceFactory.Logger.Debug($@"Flush on {ip} succeeded");


            }
            catch (Exception a)
            {
                TraceFactory.Logger.Error(a);
            }
            return result;
        }

        /// <summary>
        /// Starts the dart log collection buffer at the specified IP address.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>True if success. False if failure</returns>
        public bool Start(string ip)
        {
            bool result = false;

            string dartLocation = $@"dart.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.FileName = dartLocation;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $@"{ip} start";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                }
                result = true;
                TraceFactory.Logger.Debug($@"Start on {ip} Succeeded");

            }
            catch (Exception a)
            {
                TraceFactory.Logger.Error(a);
            }

            return result;
        }

        /// <summary>
        /// Sets the Dart Card/Board Collector Service to start collecting at the specified IP address.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Stop(string ip)
        {
            bool result = false;

            string dartLocation = $@"dart.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = true;
            startInfo.FileName = dartLocation;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $@"{ip} stop";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                }
                result = true;
                TraceFactory.Logger.Debug($@"Stop on {ip} succeeded");
            }
            catch (Exception a)
            {
                TraceFactory.Logger.Error(a);
            }
            return result;
        }

        /// <summary>
        /// Sends an email to the specified email addresses if we collect dart card logs from a device.
        /// </summary>
        /// <param name="sessId"></param>
        /// <param name="device"></param>
        /// <param name="success"></param>
        /// <param name="emailsToSend"></param>
        private void SendEmail(string sessId, string device, bool success, string emailsToSend)
        {
            string EmailFrom = "donotreply@hp.com";
            string SmtpServer = GlobalSettings.Items[Setting.AdminEmailServer];
            string result = success ? "success" : "failure";
            string SessionId = sessId;
            try
            {

                SmtpClient smtpMail = new SmtpClient(SmtpServer);
                MailMessage message = new MailMessage { From = new MailAddress(EmailFrom) };
                message.To.Add(emailsToSend);
                message.Subject = $@"Dart Log Collection Result: {result}";
                message.Body = $@"Dart log collection for device {device} in session {sessId} result: {result}";

                smtpMail.Send(message);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex);
                TraceFactory.Logger.Debug(ex.Message);
            }
        }

        private bool CheckProd(string ip, string password)
        {
            JediDevice device = new JediDevice(ip, password);
            bool temp = false;
            try
            {
                temp = device.SystemTest.IsSupported();
            }
            catch
            {
                temp = true;
            }

            return !temp;
            //Console.WriteLine("Production Mode Is On:" + temp);
        }
    }
}
