using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using NetworkEmulator = Microsoft.VisualStudio.QualityTools.NetworkEmulation;

namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    [ToolboxItem(false)]
    public partial class NestExecutionControl : UserControl, IPluginExecutionEngine
    {
        private bool _setupDone = false;
        private readonly string _emulationFileName = Path.Combine(Path.GetTempPath(), "emulation.xml");


        public NestExecutionControl()
        {
            InitializeComponent();
        }

        public void Setup(PluginExecutionData executionData)
        {
            // Currently this plug-in do not having any thing do here
            //check if the environment is x64 or x86
            //copy the dll required by the network emulation library and specific to operating system.
            CopyDll();
            CanEmulate(executionData.Environment);
        }

        #region UIOperations

        private void UpdateUI(string emulationString)
        {

            var xDoc = XDocument.Parse(emulationString);
            var ns = xDoc.Root.GetDefaultNamespace();


            var emulationElement = xDoc.Element("Emulation");


            if (emulationElement != null)
            {
                var linkRulesElements = emulationElement.Element(ns + "VirtualChannel").Element(ns + "VirtualLink").Elements(ns + "LinkRule").ToList();

                //first is always upstream
                var upstreamElement = linkRulesElements.ElementAt(0);

                var upBandwidthElement = upstreamElement.Element(ns + "Bandwidth");
                if (upBandwidthElement != null)
                {
                    bandwidth_textBox.Text = $"Upload: { (object)upBandwidthElement.Element(ns + "Speed").Value} { (object)upBandwidthElement.Element(ns + "Speed").FirstAttribute.Value} ";
                }

                //get latency element
                var upLatencyElement = upstreamElement.Element(ns + "Latency");
                if (upLatencyElement != null)
                {
                    var fixedLatency = upLatencyElement.Element(ns + "Fixed");
                    if (fixedLatency != null)
                    {
                        latency_textBox.Text = $"{ (object)fixedLatency.Element(ns + "Time").Value} ms";
                    }

                    var uniformLatency = upLatencyElement.Element(ns + "Uniform");
                    if (uniformLatency != null)
                    {
                        latency_textBox.Text = $"Minimum: { (object)uniformLatency.Element(ns + "Min").Value} ms, Maximum: { (object)uniformLatency.Element(ns + "Max").Value} ms";
                    }

                    var normalLatency = upLatencyElement.Element(ns + "Normal");
                    if (normalLatency != null)
                    {
                        latency_textBox.Text = $"Average: { (object)normalLatency.Element(ns + "Average").Value} ms, Deviation: { (object)normalLatency.Element(ns + "Deviation").Value} ms";
                    }
                }

                // get the loss element
                var upLossElement = upstreamElement.Element(ns + "Loss");
                if (upLossElement != null)
                {
                    var periodicLoss = upLossElement.Element(ns + "Periodic");
                    if (periodicLoss != null)
                    {
                        packetloss_textBox.Text = $"Losing one packet for { (object)periodicLoss.Element(ns + "PerPackets").Value} packet(s)";
                    }

                    var randomLoss = upLossElement.Element(ns + "Random");
                    if (randomLoss != null)
                    {
                        packetloss_textBox.Text = $"Loss rate is { (object)(Convert.ToDouble(randomLoss.Element(ns + "Rate").Value, CultureInfo.InvariantCulture) * 100)} %";
                    }
                }

                var downstreamElement = linkRulesElements.ElementAt(1);

                //get bandwidth
                var downBandwidthElement = downstreamElement.Element(ns + "Bandwidth");
                if (downBandwidthElement != null)
                {
                    bandwidth_textBox.Text += $"Download: {downBandwidthElement.Element(ns + "Speed").Value} {downBandwidthElement.Element(ns + "Speed").FirstAttribute.Value}";
                }
            }

            nic_textBox.Text = $"Emulation on: {NetworkEmulator.NetworkEmulationInstaller.GetBoundNetworkInterfaceCards().ElementAtOrDefault(0)}";
            nic_textBox.BackColor = System.Drawing.Color.Green;
        }

        #endregion UIOperations

        private bool CanEmulate(PluginEnvironment environment)
        {
            try
            {
                NetworkEmulator.NetworkEmulationDriver nDriver = new NetworkEmulator.NetworkEmulationDriver();
                return nDriver.Initialize();
            }
            catch (NetworkEmulator.NetworkEmulationAccessDeniedException eAccess)
            {
                ExecutionServices.SystemTrace.LogDebug("You do not have privileges to run network emulation" + eAccess.Message);
                return false;
            }
            catch (NetworkEmulator.NetworkEmulationConnectionException eConnection)
            {
                ExecutionServices.SystemTrace.LogDebug("There was an error in your emulation profile, please check the file and try again! " + eConnection.Message);
                return false;
            }
            catch (NetworkEmulator.NetworkEmulationDriverNotInstalledException eInstalled)
            {
                ExecutionServices.SystemTrace.LogDebug(eInstalled.Message);
                return InstallNetworkEmulationDriver(environment);
            }
            catch (NetworkEmulator.NetworkEmulationNotInitializedException eNonInitialized)
            {
                ExecutionServices.SystemTrace.LogDebug("Emulator not Initialized " + eNonInitialized.Message);
                return false;
            }
            catch (Exception genericException)
            {
                ExecutionServices.SystemTrace.LogDebug(genericException.Message);
                return InstallNetworkEmulationDriver(environment);
            }
        }

        #region InstallOperations

        private static bool InstallNetworkEmulation(PluginEnvironment environment)
        {
            string vstestPath = string.Empty;

            NetworkCredential credential = new NetworkCredential
            {
                UserName = environment.PluginSettings["DomainAdminUserName"],
                Password = environment.PluginSettings["DomainAdminPassword"],
                Domain =
                    environment.UserDomain.Equals(".")
                        ? Environment.MachineName
                        : environment.UserDomain
            };

            //get the file executable path for x86 and x64 systems
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VS110COMNTOOLS")))
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VS120COMNTOOLS")))
                {
                    var vsCommonDir = Directory.GetParent(Environment.GetEnvironmentVariable("VS120COMNTOOLS")).Parent;
                    vstestPath = Path.Combine(vsCommonDir.FullName, "IDE\\vstestconfig.exe");
                }
            }
            else
            {
                var vsCommonDir = Directory.GetParent(Environment.GetEnvironmentVariable("VS110COMNTOOLS")).Parent;
                vstestPath = Path.Combine(vsCommonDir.FullName, "IDE\\vstestconfig.exe");
            }

            //check if the vs test agent has been installed else backout
            if (File.Exists(vstestPath))
            {
                var result = ProcessUtil.Execute(vstestPath, @"NetworkEmulation /Install", credential, TimeSpan.FromMinutes(1.0));
                if (result.ExitCode != 0)
                {
                    return result.ExitCode == -1 || string.IsNullOrEmpty(result.StandardError);

                }

                return true;
            }

            return false;
        }

        private bool InstallNetworkEmulationDriver(PluginEnvironment environment)
        {
            bool result = false;
            var action = new Action(() =>
           {
               ExecutionServices.SystemTrace.LogDebug("Installing Network Emulation Driver");
               if (InstallNetworkEmulation(environment))
               {
                   NetworkEmulator.NetworkEmulationDriver nDriver = new NetworkEmulator.NetworkEmulationDriver();
                   result = nDriver.Initialize();
               }
               else
               {
                   ExecutionServices.SystemTrace.LogDebug("Unable to run network emulation on this machine");
               }
           });

            ExecutionServices.CriticalSection.Run(new Framework.Synchronization.LocalLockToken("NESTInstall", new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), action);

            return result;
        }

        private static void CopyDll()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dllPath = Path.Combine(path, Environment.Is64BitOperatingSystem ? "Microsoft.VisualStudio.QualityTools.NetworkEmulationAPI.x64.dll" : "Microsoft.VisualStudio.QualityTools.NetworkEmulationAPI.x86.dll");

            if (!File.Exists(path + "\\Microsoft.VisualStudio.QualityTools.NetworkEmulationAPI.dll"))
            {
                File.Copy(dllPath, path + "\\Microsoft.VisualStudio.QualityTools.NetworkEmulationAPI.dll", true);
            }
        }

        #endregion InstallOperations

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            if (!_setupDone)
            {
                Setup(executionData);
                _setupDone = true;
            }

            try
            {
                var action = new Action(() =>
                {
                    ExecutionServices.SystemTrace.LogDebug("Emulating network condition now");
                    NestService.Initialize();
                    NestActivityData data = executionData.GetMetadata<NestActivityData>(); ;
                    if (!string.IsNullOrEmpty(data.EmulationString))
                    {
                        if (File.Exists(_emulationFileName))
                        {
                            File.Delete(_emulationFileName);
                        }

                        var xDocument = XDocument.Parse(data.EmulationString, LoadOptions.None);
                        xDocument.Save(_emulationFileName, SaveOptions.None);
                        ExecutionServices.SystemTrace.LogDebug("Emulation file created");

                        if (NestService.StartEmulation(_emulationFileName))
                        {
                            networkprofile_textBox.Text = data.EmulationProfileName;
                            UpdateUI(data.EmulationString);
                        }
                    }
                    else
                    {
                        if (NetworkEmulator.NetworkEmulationInstaller.IsNetworkEmulationBoundToAnyNetworkInterfaceCards())
                        {
                            if (NestService.StopEmulation())
                            {
                                nic_textBox.Text = @"Not emulating";
                                ExecutionServices.SystemTrace.LogInfo("Network Emulation Stopped");
                            }
                        }
                        else
                        {
                            ExecutionServices.SystemTrace.LogInfo("Network Emulation is currently not running");
                        }
                    }
                });

                ExecutionServices.CriticalSection.Run(new Framework.Synchronization.LocalLockToken("NetworkEmulation", new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), action);
            }
            catch (NetworkEmulator.NetworkEmulationNotInitializedException eNonInitialized)
            {
                ExecutionServices.SystemTrace.LogDebug("Emulator not Initialized " + eNonInitialized.Message);
                return new PluginExecutionResult(PluginResult.Failed, "Emulator not Initialized :" + eNonInitialized.Message);
            }
            catch (Exception genericException)
            {
                ExecutionServices.SystemTrace.LogDebug(genericException.Message);
                return new PluginExecutionResult(PluginResult.Failed, "Activity Failed :" + genericException.Message);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }
    }
}